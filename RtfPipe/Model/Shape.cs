using RtfPipe.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RtfPipe.Model
{
  internal class Shape
  {
    public ShapeType Type { get; }
    public Dictionary<string, object> Properties { get; } = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

    public Shape(Group group)
    {
      var instructions = group.Contents
        .OfType<Group>()
        .FirstOrDefault(g => g.Destination is ShapeInstructions);
      if (instructions != null)
      {
        foreach (var propGroup in instructions
          .Contents.OfType<Group>().Where(g => g.Destination is ShapeProperty))
        {
          var nameGroup = propGroup.Contents.OfType<Group>()
            .FirstOrDefault(g => g.Destination is ShapePropertyName && g.Contents.Last() is TextToken);
          var valueGroup = propGroup.Contents.OfType<Group>()
            .FirstOrDefault(g => g.Destination is ShapePropertyValue);
          if (nameGroup != null && valueGroup != null)
          {
            var name = nameGroup.Contents.OfType<TextToken>().Last().Value;
            switch (name)
            {
              case "shapeType":
                Type = (ShapeType)int.Parse(valueGroup.Contents.OfType<TextToken>().Single().Value ?? "0");
                break;
              case "fBottomLine":
              case "fColumnLine":
              case "fColumnLineOK":
              case "fFlipH":
              case "fFlipV":
              case "fLeftLine":
              case "fLine":
              case "fLineOK":
              case "fLineRecolorFillAsPicture":
              case "fLineUseShapeAnchor":
              case "fRightLine":
              case "fTopLine":
              case "pictureActive":
                Properties[name] = valueGroup.Contents.OfType<TextToken>().Single().Value == "1";
                break;
              case "pib":
                Properties[name] = new Picture(valueGroup.Contents.OfType<Group>().Single());
                break;
              case "pictureId":
                Properties[name] = long.Parse(valueGroup.Contents.OfType<TextToken>().Single().Value);
                break;
              default:
                Properties[name] = string.Concat(valueGroup.Contents.OfType<TextToken>().Select(t => t.Value).ToArray());
                break;
            }
          }
        }
      }
    }
  }
}

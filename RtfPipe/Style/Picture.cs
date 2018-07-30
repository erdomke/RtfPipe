using RtfPipe.Tokens;
using System;
using System.Collections.Generic;
using System.Text;

namespace RtfPipe
{
  public class Picture
  {
    private readonly List<IToken> _tokens = new List<IToken>();

    public IEnumerable<IToken> Attributes { get { return _tokens; } }
    public UnitValue Width { get; }
    public UnitValue WidthGoal { get; }
    public UnitValue Height { get; }
    public UnitValue HeightGoal { get; }
    public IToken Type { get; }
    public byte[] Bytes { get; }

    public Picture(Group group)
    {
      foreach (var token in group.Contents)
      {
        if (token is PictureWidth width)
          Width = width.Value;
        else if (token is PictureHeight height)
          Height = height.Value;
        else if (token is PictureWidthGoal widthGoal)
          WidthGoal = widthGoal.Value;
        else if (token is PictureHeightGoal heightGoal)
          HeightGoal = heightGoal.Value;
        else if (token is BinaryToken binary)
          Bytes = binary.Value;
        else if (token.Type == TokenType.PictureTypeTag)
          Type = token;
        else if (!(token is PictureTag))
          _tokens.Add(token);
      }

      Type = Type ?? new WBitmap(0);
    }

    public string MimeType()
    {
      if (Type is EmfBlip)
        return "image/x-emf";
      else if (Type is PngBlip)
        return "image/png";
      else if (Type is JpegBlip)
        return "image/jpeg";
      else if (Type is WmMetafile)
        return "windows/metafile";
      else if (Type is MacPict)
        return "image/x-pict";
      else
        return "image/bmp";
    }
  }
}

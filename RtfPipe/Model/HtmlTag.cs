using RtfPipe.Tokens;
using System.Collections.Generic;
using System.Diagnostics;

namespace RtfPipe.Model
{
  [DebuggerDisplay("{Name}")]
  public class HtmlTag
  {
    public string Name { get; }
    public IList<IToken> Styles { get; } = new List<IToken>();

    public HtmlTag(string name)
    {
      Name = name;
    }

    public static HtmlTag A { get; } = new HtmlTag("a")
    {
      Styles =
      {
        new UnderlineToken(true),
        new ForegroundColor(new ColorValue(0, 0, 238))
      }
    };
    public static HtmlTag Div { get; } = new HtmlTag("div");
    public static HtmlTag Em { get; } = new HtmlTag("em")
    {
      Styles =
      {
        new ItalicToken(true)
      }
    };
    public static HtmlTag H1 { get; } = new HtmlTag("h1")
    {
      Styles =
      {
        new FontSize(new UnitValue(32, UnitType.Pixel)),
        new SpaceAfter(new UnitValue(21.44, UnitType.Pixel)),
        new SpaceBefore(new UnitValue(21.44, UnitType.Pixel)),
        new BoldToken(true)
      }
    };
    public static HtmlTag H2 { get; } = new HtmlTag("h2")
    {
      Styles =
      {
        new FontSize(new UnitValue(24, UnitType.Pixel)),
        new SpaceAfter(new UnitValue(19.92, UnitType.Pixel)),
        new SpaceBefore(new UnitValue(19.92, UnitType.Pixel)),
        new BoldToken(true)
      }
    };
    public static HtmlTag H3 { get; } = new HtmlTag("h3")
    {
      Styles =
      {
        new FontSize(new UnitValue(18.72, UnitType.Pixel)),
        new SpaceAfter(new UnitValue(18.72, UnitType.Pixel)),
        new SpaceBefore(new UnitValue(18.72, UnitType.Pixel)),
        new BoldToken(true)
      }
    };
    public static HtmlTag H4 { get; } = new HtmlTag("h4")
    {
      Styles =
      {
        new FontSize(new UnitValue(16, UnitType.Pixel)),
        new SpaceAfter(new UnitValue(21.28, UnitType.Pixel)),
        new SpaceBefore(new UnitValue(21.28, UnitType.Pixel)),
        new BoldToken(true)
      }
    };
    public static HtmlTag H5 { get; } = new HtmlTag("h5")
    {
      Styles =
      {
        new FontSize(new UnitValue(13.28, UnitType.Pixel)),
        new SpaceAfter(new UnitValue(22.1776, UnitType.Pixel)),
        new SpaceBefore(new UnitValue(22.1776, UnitType.Pixel)),
        new BoldToken(true)
      }
    };
    public static HtmlTag H6 { get; } = new HtmlTag("h6")
    {
      Styles =
      {
        new FontSize(new UnitValue(10.72, UnitType.Pixel)),
        new SpaceAfter(new UnitValue(24.9776, UnitType.Pixel)),
        new SpaceBefore(new UnitValue(24.9776, UnitType.Pixel)),
        new BoldToken(true)
      }
    };
    public static HtmlTag Li { get; } = new HtmlTag("li");
    public static HtmlTag Ol { get; } = new HtmlTag("ol")
    {
      Styles =
      {
        new NumberingTypeToken(NumberingType.Numbers)
      }
    };
    public static HtmlTag P { get; } = new HtmlTag("p")
    {
      Styles =
      {
        new SpaceBefore(new UnitValue(16, UnitType.Pixel)),
        new SpaceAfter(new UnitValue(16, UnitType.Pixel))
      }
    };
    public static HtmlTag Span { get; } = new HtmlTag("span");
    public static HtmlTag Strong { get; } = new HtmlTag("strong")
    {
      Styles =
      {
        new BoldToken(true)
      }
    };
    public static HtmlTag Table { get; } = new HtmlTag("table")
    {
      Styles =
      {
        new BottomCellSpacing(new UnitValue(1, UnitType.Pixel)),
        new LeftCellSpacing(new UnitValue(1, UnitType.Pixel)),
        new RightCellSpacing(new UnitValue(1, UnitType.Pixel)),
        new TopCellSpacing(new UnitValue(1, UnitType.Pixel)),
      }
    };
    public static HtmlTag Td { get; } = new HtmlTag("td")
    {
      Styles =
      {
        new CellVerticalAlign(VerticalAlignment.Center)
      }
    };
    public static HtmlTag Tr { get; } = new HtmlTag("tr");
    public static HtmlTag U { get; } = new HtmlTag("u")
    {
      Styles =
      {
        new UnderlineToken(true)
      }
    };
    public static HtmlTag Ul { get; } = new HtmlTag("ul")
    {
      Styles =
      {
        new NumberingTypeToken(NumberingType.Bullet)
      }
    };
  }
}

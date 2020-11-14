using RtfPipe.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RtfPipe.Model
{
  public class HtmlTag
  {
    public string Name { get; }
    public IList<IToken> Styles { get; } = new List<IToken>();

    public HtmlTag(string name)
    {
      Name = name;
    }

    public static HtmlTag P { get; } = new HtmlTag("p")
    {
      Styles = {
        new SpaceBefore(new UnitValue(16, UnitType.Pixel)),
        new SpaceAfter(new UnitValue(16, UnitType.Pixel))
      }
    };
    public static HtmlTag Div { get; } = new HtmlTag("div");
    public static HtmlTag Table { get; } = new HtmlTag("table");
    public static HtmlTag Tr { get; } = new HtmlTag("tr");
    public static HtmlTag Td { get; } = new HtmlTag("td");
    public static HtmlTag Ul { get; } = new HtmlTag("ul");
    public static HtmlTag Ol { get; } = new HtmlTag("ol");
    public static HtmlTag Li { get; } = new HtmlTag("li");
    public static HtmlTag A { get; } = new HtmlTag("a");
    public static HtmlTag Span { get; } = new HtmlTag("span");
  }
}

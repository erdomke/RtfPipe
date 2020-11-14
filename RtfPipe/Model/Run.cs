using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RtfPipe.Model
{
  public class Run : Node
  {
    private IEnumerable<IToken> _styles;

    public IEnumerable<IToken> Styles => _styles ?? Enumerable.Empty<IToken>();
    public string Value { get; set; }

    public Run() { }

    internal Run(string value, IEnumerable<IToken> styles)
    {
      Value = value;
      _styles = styles
        .Where(t => t.Type == TokenType.CharacterFormat)
        .ToList();
    }

    public override void Visit(INodeVisitor visitor)
    {
      visitor.Visit(this);
    }
  }
}

using System;
using System.Collections.Generic;
using System.Text;
using RtfPipe.Tokens;

namespace RtfPipe
{
  public class ListStyle
  {
    public int TemplateId { get; set; }
    public int Id { get; set; }

    public IList<ListLevelDefinition> Levels { get; } = new List<ListLevelDefinition>();

    internal ListStyle(Group group)
    {
      foreach (var token in group.Contents)
        Add(token);
    }

    internal void Add(IToken token)
    {
      if (token is ListTemplateId templateId)
        TemplateId = templateId.Value;
      else if (token is ListId listId)
        Id = listId.Value;
      else if (token is Group group && group.Destination is ListLevel)
        Levels.Add(new ListLevelDefinition(group));
    }
  }
}

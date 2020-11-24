using RtfPipe.Tokens;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace RtfPipe.Model
{
  internal static class FieldParser
  {
    private enum State
    {
      Whitespace,
      Token,
      Formula,
      QuotedString,
      Switch
    }

    public static Run ParseSymbol(IList<IToken> args, IEnumerable<IToken> styles)
    {
      var styleList = new StyleList(styles.Where(s => s.Type == TokenType.CharacterFormat));
      var text = "";

      for (var i = 1; i < args.Count; i++)
      {
        if (args[i] is FieldSwitch fieldSwitch)
        {
          switch (fieldSwitch.Value)
          {
            case "f":
              if ((i + 1) < args.Count && args[i + 1] is TextToken font)
              {
                styleList.Merge(new Font(-1).Add(font));
                i++;
              }
              break;
            case "s":
              if ((i + 1) < args.Count && args[i + 1] is TextToken size
                && int.TryParse(size.Value, out var fontSize))
              {
                styleList.Merge(new FontSize(new UnitValue(fontSize, UnitType.Point)));
                i++;
              }
              break;
          }
        }
        else if (args[i] is TextToken character)
        {
          var code = character.Value.StartsWith("0x")
            ? int.Parse(character.Value.Substring(2), NumberStyles.HexNumber)
            : int.Parse(character.Value);
          text = new string((char)code, 1);
        }
      }

      return new Run(text, styleList);
    }

    public static IEnumerable<IToken> Parse(Group group)
    {
      var instructions = new StringBuilder();
      AddText(group, instructions);

      var state = State.Whitespace;
      var buffer = new StringBuilder();
      for (var i = 0; i < instructions.Length; i++)
      {
        switch (state)
        {
          case State.Formula:
            if (instructions[i] == '\\')
            {
              yield return new TextToken() { Value = buffer.ToString() };
              buffer.Length = 0;
              state = State.Switch;
            }
            else
            {
              buffer.Append(instructions[i]);
            }
            break;
          case State.QuotedString:
            if (instructions[i] == '"')
            {
              yield return new TextToken() { Value = buffer.ToString() };
              buffer.Length = 0;
              state = State.Whitespace;
            }
            else
            {
              if (instructions[i] == '\\')
                i++;
              buffer.Append(instructions[i]);
            }
            break;
          case State.Switch:
            if (char.IsWhiteSpace(instructions[i]))
            {
              yield return new FieldSwitch(buffer.ToString());
              buffer.Length = 0;
              state = State.Whitespace;
            }
            else if (instructions[i] == '\\')
            {
              yield return new FieldSwitch(buffer.ToString());
              buffer.Length = 0;
            }
            else if (instructions[i] == '=')
            {
              yield return new FieldSwitch(buffer.ToString());
              buffer.Length = 0;
              yield return new FieldTypeTag(FieldType.Formula);
              state = State.Formula;
            }
            else
            {
              buffer.Append(instructions[i]);
            }
            break;
          case State.Token:
            var nextState = State.Token;

            if (char.IsWhiteSpace(instructions[i]))
              nextState = State.Whitespace;
            else if (instructions[i] == '\\')
              nextState = State.Switch;
            else if (instructions[i] == '"')
              nextState = State.QuotedString;
            else if (instructions[i] == '=')
              nextState = State.Formula;

            if (nextState == State.Token)
            {
              buffer.Append(instructions[i]);
            }
            else
            {
              if (!char.IsNumber(buffer[0]) && Enum.TryParse(buffer.ToString(), true, out FieldType fieldType))
                yield return new FieldTypeTag(fieldType);
              else
                yield return new TextToken() { Value = buffer.ToString() };
              buffer.Length = 0;
              if (nextState == State.Formula)
                yield return new FieldTypeTag(FieldType.Formula);
              state = nextState;
            }
            break;
          default: // case State.Whitespace:
            if (instructions[i] == '\\')
            {
              state = State.Switch;
            }
            else if (instructions[i] == '"')
            {
              state = State.QuotedString;
            }
            else if (instructions[i] == '=')
            {
              yield return new FieldTypeTag(FieldType.Formula);
              state = State.Formula;
            }
            else if (!char.IsWhiteSpace(instructions[i]))
            {
              buffer.Append(instructions[i]);
              state = State.Token;
            }
            break;
        }
      }

      if (buffer.Length > 0)
      {
        switch (state)
        {
          case State.Switch:
            yield return new FieldSwitch(buffer.ToString());
            break;
          case State.Token:
            if (!char.IsNumber(buffer[0]) && Enum.TryParse(buffer.ToString(), true, out FieldType fieldType))
              yield return new FieldTypeTag(fieldType);
            else
              yield return new TextToken() { Value = buffer.ToString() };
            break;
          case State.Formula:
          case State.QuotedString:
            yield return new TextToken() { Value = buffer.ToString() };
            break;
        }
      }
    }

    private static void AddText(Group group, StringBuilder builder)
    {
      var dest = group.Destination;
      var fallbackDest = default(IWord);
      if (group.Contents.Count > 1
        && (group.Contents[0] is TextToken ignoreText && ignoreText.Value == "*"))
        fallbackDest = group.Contents[1] as IWord;

      if (group.Contents.Count > 1
        && group.Contents[0] is IgnoreUnrecognized
        && (group.Contents[1].GetType().Name == "GenericTag" || group.Contents[1].GetType().Name == "GenericWord"))
      {
        return;
      }
      else if (fallbackDest?.GetType().Name == "GenericTag" || fallbackDest?.GetType().Name == "GenericWord")
      {
        return;
      }
      else
      {
        foreach (var token in group.Contents.Skip(fallbackDest == null ? 0 : 2))
        {
          if (token is TextToken text)
            builder.Append(text.Value);
          else if (token is Group childGroup)
            AddText(childGroup, builder);
        }
      }
    }
  }

  internal class FieldTypeTag : ControlWord<FieldType>
  {
    public override string Name => "field";

    public FieldTypeTag(FieldType value) : base(value) { }
  }

  internal class FieldSwitch : ControlWord<string>
  {
    public override string Name => "switch";

    public FieldSwitch(string value) : base(value) { }
  }
}

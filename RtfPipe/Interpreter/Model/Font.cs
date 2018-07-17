using System;
using System.Diagnostics;
using System.Text;
using RtfPipe.Sys;

namespace RtfPipe
{
  [DebuggerDisplay("{Id}:{Name}")]
  public sealed class Font : IEquatable<Font>
  {
    private readonly int _codePage;

    public int CharSet { get; }

    public int CodePage
    {
      get
      {
        // if a codepage is specified, it overrides the charset setting
        if (_codePage == 0)
        {
          // unspecified codepage: use the one derived from the charset:
          return RtfSpec.GetCodePage(CharSet);
        }
        return _codePage;
      }
    }

    public string Id { get; }

    public RtfFontKind Kind { get; }

    public string Name { get; }

    public RtfFontPitch Pitch { get; }

    public Font(string id, RtfFontKind kind, RtfFontPitch pitch, int charSet, int codePage, string name)
    {
      if (id == null)
        throw new ArgumentNullException(nameof(id));
      if (charSet < 0)
        throw new ArgumentException(Strings.InvalidCharacterSet(charSet));
      if (codePage < 0)
        throw new ArgumentException(Strings.InvalidCodePage(codePage));
      if (name == null)
        throw new ArgumentNullException(nameof(name));

      this.Id = id;
      this.Kind = kind;
      this.Pitch = pitch;
      this.CharSet = charSet;
      this._codePage = codePage;
      this.Name = name;
    }

    public Encoding GetEncoding()
    {
      return Encoding.GetEncoding(Parser.RtfParser._codePages[CodePage]);
    }

    public override bool Equals(object obj)
    {
      if (obj is Font font)
        return Equals(font);
      return false;
    }

    public bool Equals(Font other)
    {
      return Id.Equals(other.Id)
        && Kind == other.Kind
        && Pitch == other.Pitch
        && CharSet == other.CharSet
        && _codePage == other._codePage
        && string.Equals(Name, other.Name);
    }

    public override int GetHashCode()
    {
      return HashTool.AddHashCode(GetType().GetHashCode(), ComputeHashCode());
    }

    private int ComputeHashCode()
    {
      int hash = Id.GetHashCode();
      hash = HashTool.AddHashCode(hash, Kind);
      hash = HashTool.AddHashCode(hash, Pitch);
      hash = HashTool.AddHashCode(hash, CharSet);
      hash = HashTool.AddHashCode(hash, _codePage);
      hash = HashTool.AddHashCode(hash, Name);
      return hash;
    }

    public static bool operator ==(Font x, Font y)
    {
      if (ReferenceEquals(x, y))
        return true;

      if (ReferenceEquals(x, null) || ReferenceEquals(y, null))
        return false;

      return x.Equals(y);
    }

    public static bool operator !=(Font x, Font y)
    {
      return !(x == y);
    }
  }

}

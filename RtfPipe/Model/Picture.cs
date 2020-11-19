using RtfPipe.Model;
using RtfPipe.Tokens;
using System.Collections.Generic;

namespace RtfPipe
{
  /// <summary>
  /// A picture store in a RTF document
  /// </summary>
  public class Picture : Node
  {
    private readonly List<IToken> _tokens = new List<IToken>();
    private UnitValue _width;
    private UnitValue _widthGoal;
    private UnitValue _height;
    private UnitValue _heightGoal;
    private int _scaleX = 100;
    private int _scaleY = 100;

    /// <summary>
    /// Control tokens stored in the RTF document
    /// </summary>
    public IEnumerable<IToken> Attributes { get { return _tokens; } }

    /// <summary>
    /// The binary data describing the picture
    /// </summary>
    public byte[] Bytes { get; }

    /// <summary>
    /// The rendered height of the picture
    /// </summary>
    public UnitValue Height
    {
      get
      {
        var baseUnit = _heightGoal.HasValue ? _heightGoal : _height;
        if (!baseUnit.HasValue)
          return UnitValue.Empty;
        if (_scaleY < 100 && _scaleY > 0)
          return baseUnit * (_scaleY / 100.0);
        return baseUnit;
      }
    }

    /// <summary>
    /// The picture format
    /// </summary>
    public IToken Type { get; }

    /// <summary>
    /// The rendered width of the picture
    /// </summary>
    public UnitValue Width
    {
      get
      {
        var baseUnit = _widthGoal.HasValue ? _widthGoal : _width;
        if (!baseUnit.HasValue)
          return UnitValue.Empty;
        if (_scaleX < 100 && _scaleX > 0)
          return baseUnit * (_scaleX / 100.0);
        return baseUnit;
      }
    }

    /// <summary>
    /// Create a new <see cref="Picture"/> object
    /// </summary>
    /// <param name="group">An RTF token group</param>
    public Picture(Group group)
    {
      foreach (var token in group.Contents)
      {
        if (token is PictureWidth width)
          _width = width.Value;
        else if (token is PictureHeight height)
          _height = height.Value;
        else if (token is PictureWidthGoal widthGoal)
          _widthGoal = widthGoal.Value;
        else if (token is PictureHeightGoal heightGoal)
          _heightGoal = heightGoal.Value;
        else if (token is BinaryToken binary)
          Bytes = binary.Value;
        else if (token.Type == TokenType.PictureTypeTag)
          Type = token;
        else if (token is PictureScaleX scaleX)
          _scaleX = scaleX.Value;
        else if (token is PictureScaleY scaleY)
          _scaleY = scaleY.Value;
        else if (!(token is PictureTag))
          _tokens.Add(token);
      }

      Type = Type ?? new WBitmap(0);
    }

    /// <summary>
    /// The MIME type of the picture
    /// </summary>
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

    internal override void Visit(INodeVisitor visitor)
    {
      visitor.Visit(this);
    }
  }
}

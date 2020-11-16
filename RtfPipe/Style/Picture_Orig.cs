using RtfPipe.Tokens;
using System;
using System.Collections.Generic;
using System.Text;

namespace RtfPipe
{
  public class Picture_Orig
  {
    private readonly List<IToken> _tokens = new List<IToken>();
    private UnitValue _width;
    private UnitValue _widthGoal;
    private UnitValue _height;
    private UnitValue _heightGoal;
    private int _scaleX = 100;
    private int _scaleY = 100;

    public IEnumerable<IToken> Attributes { get { return _tokens; } }
    public byte[] Bytes { get; }
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
    public IToken Type { get; }
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

    public Picture_Orig(Group group)
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

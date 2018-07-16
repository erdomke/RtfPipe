namespace RtfPipe
{


  public interface IRtfVisualSpecialChar : IRtfVisual
  {

    RtfVisualSpecialCharKind CharKind { get; }

    IRtfTextFormat Format { get; }

  }

}


namespace RtfPipe
{


  public interface IRtfVisualSpecialChar : IRtfVisual
  {

    RtfVisualSpecialCharKind CharKind { get; }

    Style Format { get; }

  }

}


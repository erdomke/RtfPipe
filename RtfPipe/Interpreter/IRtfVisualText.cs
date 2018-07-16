namespace RtfPipe
{

  public interface IRtfVisualText : IRtfVisual
  {

    string Text { get; }

    IRtfTextFormat Format { get; }

  }

}

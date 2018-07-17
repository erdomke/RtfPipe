namespace RtfPipe
{

  public interface IRtfVisualText : IRtfVisual
  {

    string Text { get; }

    Style Format { get; }

  }

}

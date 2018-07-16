
namespace RtfPipe
{

  public class RtfInterpreterListener
  {

    public void BeginDocument(RtfInterpreterContext context)
    {
      if (context != null)
      {
        DoBeginDocument(context);
      }
    }

    public void InsertText(RtfInterpreterContext context, string text)
    {
      if (context != null)
      {
        DoInsertText(context, text);
      }
    }

    public void InsertSpecialChar(RtfInterpreterContext context, RtfVisualSpecialCharKind kind)
    {
      if (context != null)
      {
        DoInsertSpecialChar(context, kind);
      }
    }

    public void InsertBreak(RtfInterpreterContext context, RtfVisualBreakKind kind)
    {
      if (context != null)
      {
        DoInsertBreak(context, kind);
      }
    }

    public void InsertImage(RtfInterpreterContext context, RtfVisualImageFormat format,
      int width, int height, int desiredWidth, int desiredHeight,
      int scaleWidthPercent, int scaleHeightPercent, string imageDataHex
    )
    {
      if (context != null)
      {
        DoInsertImage(context, format,
          width, height, desiredWidth, desiredHeight,
          scaleWidthPercent, scaleHeightPercent, imageDataHex);
      }
    }

    public void EndDocument(RtfInterpreterContext context)
    {
      if (context != null)
      {
        DoEndDocument(context);
      }
    }

    protected virtual void DoBeginDocument(RtfInterpreterContext context)
    {
    }

    protected virtual void DoInsertText(RtfInterpreterContext context, string text)
    {
    }

    protected virtual void DoInsertSpecialChar(RtfInterpreterContext context, RtfVisualSpecialCharKind kind)
    {
    }

    protected virtual void DoInsertBreak(RtfInterpreterContext context, RtfVisualBreakKind kind)
    {
    }

    protected virtual void DoInsertImage(RtfInterpreterContext context,
      RtfVisualImageFormat format,
      int width, int height, int desiredWidth, int desiredHeight,
      int scaleWidthPercent, int scaleHeightPercent,
      string imageDataHex
    )
    {
    }

    protected virtual void DoEndDocument(RtfInterpreterContext context)
    {
    }

  }

}

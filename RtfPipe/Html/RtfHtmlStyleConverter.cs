using System;

namespace RtfPipe.Converter.Html
{


  public class RtfHtmlStyleConverter : IRtfHtmlStyleConverter
  {

    public virtual IRtfHtmlStyle TextToHtml(IRtfVisualText visualText)
    {
      if (visualText == null)
      {
        throw new ArgumentNullException("visualText");
      }

      RtfHtmlStyle htmlStyle = new RtfHtmlStyle();

      Style textFormat = visualText.Format;

      // background color
      var color = textFormat.Background as ColorValue;
      if (color != null && (color.Red != 255 || color.Green != 255 || color.Blue != 255))
      {
        htmlStyle.BackgroundColor = ToHtmlColor(color);
      }

      // foreground color
      color = textFormat.Color;
      if (color != null && (color.Red != 0 || color.Green != 0 || color.Blue != 0))
      {
        htmlStyle.ForegroundColor = ToHtmlColor(color);
      }

      // font
      htmlStyle.FontFamily = textFormat.Font.Name;
      if (textFormat.FontSize.HasValue)
      {
        htmlStyle.FontSize = textFormat.FontSize.ToPt().ToString("0.#") + "pt";
      }

      return htmlStyle;
    }

    private string ToHtmlColor(ColorValue color)
    {
      return string.Format("#{0:x2}{1:x2}{2:x2}", color.Red, color.Green, color.Blue);
    }
  }

}


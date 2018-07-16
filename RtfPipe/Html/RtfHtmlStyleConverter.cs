using System;

namespace RtfPipe.Converter.Html
{


  public class RtfHtmlStyleConverter : IRtfHtmlStyleConverter
  {

    public virtual IRtfHtmlStyle TextToHtml( IRtfVisualText visualText )
    {
      if ( visualText == null )
      {
        throw new ArgumentNullException( "visualText" );
      }

      RtfHtmlStyle htmlStyle = new RtfHtmlStyle();

      IRtfTextFormat textFormat = visualText.Format;

      // background color
      var color = textFormat.BackgroundColor;
      if (color.Red != 255 || color.Green != 255 || color.Blue != 255 )
      {
        htmlStyle.BackgroundColor = ToHtmlColor(color);
      }

      // foreground color
      color = textFormat.ForegroundColor;
      if (color.Red != 0 || color.Green != 0 || color.Blue != 0 )
      {
        htmlStyle.ForegroundColor = ToHtmlColor(color);
      }

      // font
      htmlStyle.FontFamily = textFormat.Font.Name;
      if ( textFormat.FontSize > 0 )
      {
        htmlStyle.FontSize = (textFormat.FontSize /2) + "pt";
      }

      return htmlStyle;
    }

    private string ToHtmlColor(IRtfColor color)
    {
      return string.Format("#{0:x2}{1:x2}{2:x2}", color.Red, color.Green, color.Blue);
    }
  }

}


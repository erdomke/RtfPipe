// -- FILE ------------------------------------------------------------------
// name       : RtfWindow.cs
// project    : RTF Framelet
// created    : Jani Giannoudis - 2008.07.29
// language   : c#
// environment: .NET 3.0
// copyright  : (c) 2004-2013 by Jani Giannoudis, Switzerland
// --------------------------------------------------------------------------
using System;
using System.IO;
using System.Xml;
using System.Text;
using System.Windows;
using System.Windows.Documents;
using Itenso.Rtf;
using Itenso.Rtf.Support;
using Itenso.Rtf.Converter.Text;
using Itenso.Rtf.Converter.Xml;
using Itenso.Rtf.Converter.Html;

namespace Itenso.Solutions.Community.RtfConverter.RtfWindows
{

	// ------------------------------------------------------------------------
	public partial class RtfWindow
	{

		// ----------------------------------------------------------------------
		public RtfWindow()
		{
			InitializeComponent();
		} // RtfWindow

		// ----------------------------------------------------------------------
		private TextRange ConversionText
		{
			get
			{
				if ( richTextBox.Selection != null && !richTextBox.Selection.IsEmpty )
				{
					return richTextBox.Selection;
				}
				return new TextRange( richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd );
			}
		} // ConversionText

		// ----------------------------------------------------------------------
		private void ToTextButtonClick( object sender, RoutedEventArgs e )
		{
			try
			{
				TextRange conversionText = ConversionText;
				using ( MemoryStream stream = new MemoryStream() )
				{
					conversionText.Save( stream, DataFormats.Rtf );
					stream.Seek( 0, SeekOrigin.Begin );
					using ( StreamReader reader = new StreamReader( stream ) )
					{
						IRtfGroup rtfStructure = RtfParserTool.Parse( reader );
						RtfTextConverter textConverter = new RtfTextConverter();
						RtfInterpreterTool.Interpret( rtfStructure, textConverter );

						textBox.Text = textConverter.PlainText;
					}
				}
			}
			catch ( Exception exception )
			{
				MessageBox.Show( this, "Error " + exception.Message, Title, MessageBoxButton.OK, MessageBoxImage.Error );
			}
		} // ToTextButtonClick

		// ----------------------------------------------------------------------
		private void ToXmlButtonClick( object sender, RoutedEventArgs e )
		{
			try
			{
				TextRange conversionText = ConversionText;
				using ( MemoryStream stream = new MemoryStream() )
				{
					conversionText.Save( stream, DataFormats.Rtf );
					stream.Seek( 0, SeekOrigin.Begin );
					using ( StreamReader reader = new StreamReader( stream ) )
					{
						IRtfDocument rtfDocument = RtfInterpreterTool.BuildDoc( reader );

						XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
						xmlWriterSettings.Indent = true;
						xmlWriterSettings.IndentChars = ( "  " );
						StringBuilder sb = new StringBuilder();
						using ( XmlWriter writer = XmlWriter.Create( sb, xmlWriterSettings ) )
						{
							RtfXmlConverter xmlConverter = new RtfXmlConverter( rtfDocument, writer );
							xmlConverter.Convert();
							writer.Flush();
							textBox.Text = sb.ToString();
						}
					}
				}
			}
			catch ( Exception exception )
			{
				MessageBox.Show( this, "Error " + exception.Message, Title, MessageBoxButton.OK, MessageBoxImage.Error );
			}
		} // ToXmlButtonClick

		// ----------------------------------------------------------------------
		private void ToHtmlButtonClick( object sender, RoutedEventArgs e )
		{
			try
			{
				TextRange conversionText = ConversionText;
				using ( MemoryStream stream = new MemoryStream() )
				{
					conversionText.Save( stream, DataFormats.Rtf );
					stream.Seek( 0, SeekOrigin.Begin );
					using ( StreamReader reader = new StreamReader( stream ) )
					{
						IRtfDocument rtfDocument = RtfInterpreterTool.BuildDoc( reader );
						RtfHtmlConverter htmlConverter = new RtfHtmlConverter( rtfDocument );
						textBox.Text = htmlConverter.Convert();
					}
				}
			}
			catch ( Exception exception )
			{
				MessageBox.Show( this, "Error " + exception.Message, Title, MessageBoxButton.OK, MessageBoxImage.Error );
			}
		} // ToHtmlButtonClick

	} // RtfWindow

} // namespace Itenso.Solutions.Community.RtfConverter.RtfWindows
// -- EOF -------------------------------------------------------------------

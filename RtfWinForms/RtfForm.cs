// -- FILE ------------------------------------------------------------------
// name       : RtfWindow.cs
// project    : RTF Framelet
// created    : Jani Giannoudis - 2008.07.30
// language   : c#
// environment: .NET 2.0
// copyright  : (c) 2004-2013 by Jani Giannoudis, Switzerland
// --------------------------------------------------------------------------
using System;
using System.IO;
using System.Xml;
using System.Text;
using System.Reflection;
using System.Windows.Forms;
using Itenso.Rtf;
using Itenso.Rtf.Support;
using Itenso.Rtf.Converter.Text;
using Itenso.Rtf.Converter.Xml;
using Itenso.Rtf.Converter.Html;

namespace Itenso.Solutions.Community.RtfConverter.RtfWinForms
{

	// ------------------------------------------------------------------------
	public partial class RtfForm : Form
	{

		// ----------------------------------------------------------------------
		public RtfForm()
		{
			InitializeComponent();
			SetupDefaultText( "DefaultText.rtf" );
		} // RtfForm

		// ----------------------------------------------------------------------
		private string ConversionText
		{
			get 
			{
				if ( richTextBox.SelectionLength > 0 )
				{
					return richTextBox.SelectedRtf;
				}
				return richTextBox.Rtf;
			}
		} // ConversionText

		// ----------------------------------------------------------------------
		private void SetupDefaultText( string resName )
		{
			Type type = GetType();
			Assembly assembly = type.Assembly;
			string fullName = type.Namespace + "." + resName.Replace( '\\', '.' ).Replace( '/', '.' );

			string defaultText = string.Empty;
			using ( Stream stream = assembly.GetManifestResourceStream( fullName ) )
			{
				if  ( stream != null )
				{
					using ( StreamReader streamReader = new StreamReader( stream, Encoding.Default ) )
					{
						defaultText = streamReader.ReadToEnd();
					}
				}
			}

			richTextBox.Rtf = defaultText;
		} // SetupDefaultText

		// ----------------------------------------------------------------------
		private void ToTextButtonClick( object sender, EventArgs e )
		{
			try
			{
				IRtfGroup rtfStructure = RtfParserTool.Parse( ConversionText );
				RtfTextConverter textConverter = new RtfTextConverter();
				RtfInterpreterTool.Interpret( rtfStructure, textConverter );
				textBox.Text = textConverter.PlainText;
			}
			catch ( Exception exception )
			{
				MessageBox.Show( this, "Error " + exception.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Error );
			}
		} // ToTextButtonClick

		// ----------------------------------------------------------------------
		private void ToXmlButtonClick( object sender, EventArgs e )
		{
			try
			{
				IRtfDocument rtfDocument = RtfInterpreterTool.BuildDoc( ConversionText );

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
			catch ( Exception exception )
			{
				MessageBox.Show( this, "Error " + exception.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Error );
			}
		} // ToXmlButtonClick

		// ----------------------------------------------------------------------
		private void ToHtmlButtonClick( object sender, EventArgs e )
		{
			try
			{
				IRtfDocument rtfDocument = RtfInterpreterTool.BuildDoc( ConversionText );
				RtfHtmlConverter htmlConverter = new RtfHtmlConverter( rtfDocument );
				textBox.Text = htmlConverter.Convert();
			}
			catch ( Exception exception )
			{
				MessageBox.Show( this, "Error " + exception.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Error );
			}
		} // ToHtmlButtonClick

	} // class RtfWindow

} // namespace Itenso.Solutions.Community.RtfConverter.RtfWinForms
// -- EOF -------------------------------------------------------------------
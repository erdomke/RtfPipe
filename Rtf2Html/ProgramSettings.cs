// -- FILE ------------------------------------------------------------------
// name       : ProgramSettings.cs
// project    : RTF Framelet
// created    : Jani Giannoudis - 2008.05.30
// language   : c#
// environment: .NET 2.0
// copyright  : (c) 2004-2013 by Jani Giannoudis, Switzerland
// --------------------------------------------------------------------------
using System.Drawing;
using System.IO;
using System.Text;
using System.Drawing.Imaging;
using RtfPipe.Converter.Html;
using RtfPipe.Sys.Application;

namespace RtfPipe.Solutions.Community.Rtf2Html
{

	// ------------------------------------------------------------------------
	class ProgramSettings
	{

		// ----------------------------------------------------------------------
		public ProgramSettings()
		{
			LoadApplicationArguments();
		} // ProgramSettings

		// ----------------------------------------------------------------------
		public bool IsHelpMode
		{
			get { return applicationArguments.IsHelpMode; }
		} // IsHelpMode

		// ----------------------------------------------------------------------
		public bool IsValid
		{
			get { return applicationArguments.IsValid; }
		} // IsValid

		// ----------------------------------------------------------------------
		public bool IsValidSourceFile
		{
			get
			{
				string sourceFile = SourceFile;
				return !string.IsNullOrEmpty( sourceFile ) && File.Exists( sourceFile );
			}
		} // IsValidSourceFile

		// ----------------------------------------------------------------------
		public string SourceFile
		{
			get { return sourceFileArgument.Value; }
		} // SourceFile

		// ----------------------------------------------------------------------
		public string SourceFileNameWithoutExtension
		{
			get
			{
				string sourceFile = SourceFile;
				if ( sourceFile == null )
				{
					return null;
				}
				FileInfo fi = new FileInfo( sourceFile );
				return fi.Name.Replace( fi.Extension, string.Empty );
			}
		} // SourceFileNameWithoutExtension

		// ----------------------------------------------------------------------
		public string DestinationDirectory
		{
			get
			{
				string destinationDirectory = destinationDirectoryArgument.Value;
				if ( string.IsNullOrEmpty( destinationDirectory ) && IsValidSourceFile )
				{
					FileInfo fi = new FileInfo( SourceFile );
					return fi.DirectoryName;
				}
				return destinationDirectory;
			}
		} // DestinationDirectory

		// ----------------------------------------------------------------------
		public string StyleSheets
		{
			get { return styleSheetsArgument.Value; }
		} // StyleSheets	

		// ----------------------------------------------------------------------
		public string ImagesDirectory
		{
			get { return imagesDirectoryArgument.Value; }
		} // ImagesDirectory	

		// ----------------------------------------------------------------------
		public string ImagesPath
		{
			get
			{
				string imagesPath = DestinationDirectory;
				if ( !string.IsNullOrEmpty( ImagesDirectory ) )
				{
					imagesPath = Path.Combine( imagesPath, ImagesDirectory );
				}
				return imagesPath;
			}
		} // ImagesPath	

		// ----------------------------------------------------------------------
		public string ImageType
		{
			get { return imageTypeArgument.Value; }
		} // ImageType	

		// ----------------------------------------------------------------------
		public string ImageFileNamePattern
		{
			get
			{
				string imageFileNamePattern = SourceFileNameWithoutExtension + "{0}{1}";
				if ( !string.IsNullOrEmpty( ImagesDirectory ) )
				{
					imageFileNamePattern = Path.Combine( ImagesDirectory, imageFileNamePattern );
				}
				return imageFileNamePattern;
			}
		} // ImageFileNamePattern	

		// ----------------------------------------------------------------------
		public ImageFormat ImageFormat
		{
			get
			{
				ImageFormat imageFormat = ImageFormat.Jpeg;
				if ( !string.IsNullOrEmpty( ImageType ) )
				{
					switch ( ImageType.ToLower() )
					{
						case "jpg":
							imageFormat = ImageFormat.Jpeg;
							break;
						case "gif":
							imageFormat = ImageFormat.Gif;
							break;
						case "png":
							imageFormat = ImageFormat.Png;
							break;
					}
				}
				return imageFormat;
			}
		} // ImageFormat	

		// ----------------------------------------------------------------------
		public Color? ImageBackgroundColor
		{
			get
			{
				string backgroundColorName = imageBackgroundColorNameArgument.Value;
				if ( string.IsNullOrEmpty( backgroundColorName ) )
				{
					return null;
				}

				Color backgroundColor = Color.FromName( backgroundColorName );
				if ( !backgroundColor.IsKnownColor )
				{
					return null;
				}
				return backgroundColor;
			}
		} // ImageBackgroundColor

		// ----------------------------------------------------------------------
		public string CharacterEncoding
		{
			get { return characterEncodingArgument.Value; }
		} // CharacterEncoding	

		// ----------------------------------------------------------------------
		public Encoding Encoding
		{
			get
			{
				Encoding encoding = Encoding.UTF8;

				if ( !string.IsNullOrEmpty( CharacterEncoding ) )
				{
					switch ( CharacterEncoding.ToLower() )
					{
						case "ascii":
							encoding = Encoding.ASCII;
							break;
						case "utf7":
							encoding = Encoding.UTF7;
							break;
						case "utf8":
							encoding = Encoding.UTF8;
							break;
						case "unicode":
							encoding = Encoding.Unicode;
							break;
						case "bigendianunicode":
							encoding = Encoding.BigEndianUnicode;
							break;
						case "utf32":
							encoding = Encoding.UTF32;
							break;
						case "operatingsystem":
							encoding = Encoding.Default;
							break;
					}
				}

				return encoding;
			}
		} // Encoding

		// ----------------------------------------------------------------------
		public string CharacterSet
		{
			get { return characterSetArgument.Value; }
		} // CharacterSet	

		// ----------------------------------------------------------------------
		public string SpecialCharsRepresentation
		{
			get { return specialCharsRepresentationArgument.Value; }
		} // SpecialCharsRepresentation	

		// ----------------------------------------------------------------------
		public bool HasDestinationOutput
		{
			get { return SaveHtml || SaveImage; }
		} // HasDestinationOutput	

		// ----------------------------------------------------------------------
		public bool SaveHtml
		{
			get { return saveHtmlArgument.Value; }
		} // SaveHtml	

		// ----------------------------------------------------------------------
		public bool SaveImage
		{
			get { return saveImageArgument.Value; }
		} // SaveImage	

		// ----------------------------------------------------------------------
		public bool DisplayHtml
		{
			get { return displayHtmlArgument.Value; }
		} // DisplayHtml

		// ----------------------------------------------------------------------
		public string LogDirectory
		{
			get { return logDirectoryArgument.Value; }
		} // LogDirectory	

		// ----------------------------------------------------------------------
		public bool LogParser
		{
			get { return logParserArgument.Value; }
		} // LogParser

		// ----------------------------------------------------------------------
		public bool LogInterpreter
		{
			get { return logInterpreterArgument.Value; }
		} // LogInterpreter

		// ----------------------------------------------------------------------
		public bool OpenHtml
		{
			get { return openHtmlArgument.Value; }
		} // OpenHtml

		// ----------------------------------------------------------------------
		public bool ShowHiddenText
		{
			get { return showHiddenTextArgument.Value; }
		} // ShowHiddenText

		// ----------------------------------------------------------------------
		public bool UseNonBreakingSpaces
		{
			get { return useNonBreakingSpacesArgument.Value; }
		} // UseNonBreakingSpaces

		// ----------------------------------------------------------------------
		public bool ConvertVisualHyperlinks
		{
			get { return convertVisualHyperlinksArgument.Value; }
		} // ConvertVisualHyperlinks

		// ----------------------------------------------------------------------
		public bool UseInlineStyles
		{
			get { return useInlineStylesArgument.Value; }
		} // UseInlineStyles

		// ----------------------------------------------------------------------
		public string VisualHyperlinkPattern
		{
			get { return visualHyperlinkPatternArgument.Value; }
		} // VisualHyperlinkPattern	

		// ----------------------------------------------------------------------
		public bool ExtendedImageScale
		{
			get { return extendedImageScaleArgument.Value; }
		} // ExtendedImageScale

		// ----------------------------------------------------------------------
		public string DocumentScope
		{
			get { return documentScopeArgument.Value; }
		} // DocumentScope	

		// ----------------------------------------------------------------------
		public bool UnscaledImages
		{
			get { return unscaledImagesArgument.Value; }
		} // UnscaledImages

		// ----------------------------------------------------------------------
		public bool IgnoreDuplicatedFonts
		{
			get { return ignoreDuplicatedFontsArgument.Value; }
		} // IgnoreDuplicatedFonts

		// ----------------------------------------------------------------------
		public bool IgnoreUnknownFonts
		{
			get { return ignoreUnknownFontsArgument.Value; }
		} // IgnoreUnknownFonts

		// ----------------------------------------------------------------------
		public RtfHtmlConvertScope ConvertScope
		{
			get
			{
				RtfHtmlConvertScope convertScope = RtfHtmlConvertScope.None;

				if ( !string.IsNullOrEmpty( DocumentScope ) )
				{
					string[] tokens = DocumentScope.Split( ',' );
					foreach ( string token in tokens )
					{
						switch ( token.ToLower() )
						{
							case "doc":
							case "document":
								convertScope |= RtfHtmlConvertScope.Document;
								break;
							case "html":
								convertScope |= RtfHtmlConvertScope.Html;
								break;
							case "head":
								convertScope |= RtfHtmlConvertScope.Head;
								break;
							case "body":
								convertScope |= RtfHtmlConvertScope.Body;
								break;
							case "content":
								convertScope |= RtfHtmlConvertScope.Content;
								break;
							case "*":
							case "all":
								convertScope |= RtfHtmlConvertScope.All;
								break;
						}
					}
				}
				return convertScope;
			}
		} // ConvertScope

		// ----------------------------------------------------------------------
		public string BuildDestinationFileName( string path, string extension )
		{
			string sourceFileNameWithoutExtension = SourceFileNameWithoutExtension;
			if ( sourceFileNameWithoutExtension == null )
			{
				return null;
			}

			return Path.Combine(
				string.IsNullOrEmpty( path ) ? DestinationDirectory : path,
				sourceFileNameWithoutExtension + extension );
		} // BuildDestinationFileName

		// ----------------------------------------------------------------------
		private void LoadApplicationArguments()
		{
			applicationArguments.Arguments.Add( new HelpModeArgument() );
			applicationArguments.Arguments.Add( sourceFileArgument );
			applicationArguments.Arguments.Add( destinationDirectoryArgument );
			applicationArguments.Arguments.Add( styleSheetsArgument );
			applicationArguments.Arguments.Add( imagesDirectoryArgument );
			applicationArguments.Arguments.Add( imageTypeArgument );
			applicationArguments.Arguments.Add( imageBackgroundColorNameArgument );
			applicationArguments.Arguments.Add( characterEncodingArgument );
			applicationArguments.Arguments.Add( characterSetArgument );
			applicationArguments.Arguments.Add( specialCharsRepresentationArgument );
			applicationArguments.Arguments.Add( saveHtmlArgument );
			applicationArguments.Arguments.Add( saveImageArgument );
			applicationArguments.Arguments.Add( logDirectoryArgument );
			applicationArguments.Arguments.Add( logParserArgument );
			applicationArguments.Arguments.Add( logInterpreterArgument );
			applicationArguments.Arguments.Add( displayHtmlArgument );
			applicationArguments.Arguments.Add( openHtmlArgument );
			applicationArguments.Arguments.Add( showHiddenTextArgument );
			applicationArguments.Arguments.Add( useNonBreakingSpacesArgument );
			applicationArguments.Arguments.Add( convertVisualHyperlinksArgument );
			applicationArguments.Arguments.Add( visualHyperlinkPatternArgument );
			applicationArguments.Arguments.Add( extendedImageScaleArgument );
			applicationArguments.Arguments.Add( documentScopeArgument );
			applicationArguments.Arguments.Add( useInlineStylesArgument );
			applicationArguments.Arguments.Add( unscaledImagesArgument );
			applicationArguments.Arguments.Add( ignoreDuplicatedFontsArgument );
			applicationArguments.Arguments.Add( ignoreUnknownFontsArgument );

			applicationArguments.Load(System.Environment.GetCommandLineArgs());
		} // LoadApplicationArguments

		// ----------------------------------------------------------------------
		// members
		private readonly ApplicationArguments applicationArguments = new ApplicationArguments();
		private readonly ValueArgument sourceFileArgument = new ValueArgument( ArgumentType.Mandatory );
		private readonly ValueArgument destinationDirectoryArgument = new ValueArgument();
		private readonly NamedValueArgument styleSheetsArgument = new NamedValueArgument( "CSS" );
		private readonly NamedValueArgument imagesDirectoryArgument = new NamedValueArgument( "ID" );
		private readonly NamedValueArgument imageTypeArgument = new NamedValueArgument( "IT" );
		private readonly NamedValueArgument imageBackgroundColorNameArgument = new NamedValueArgument( "BC" );
		private readonly NamedValueArgument characterEncodingArgument = new NamedValueArgument( "CE" );
		private readonly NamedValueArgument characterSetArgument = new NamedValueArgument( "CS" );
		private readonly NamedValueArgument specialCharsRepresentationArgument = new NamedValueArgument( "SC" );
		private readonly ToggleArgument saveHtmlArgument = new ToggleArgument( "SH", true );
		private readonly ToggleArgument saveImageArgument = new ToggleArgument( "SI", true );
		private readonly NamedValueArgument logDirectoryArgument = new NamedValueArgument( "LD" );
		private readonly ToggleArgument logParserArgument = new ToggleArgument( "LP", false );
		private readonly ToggleArgument logInterpreterArgument = new ToggleArgument( "LI", false );
		private readonly ToggleArgument displayHtmlArgument = new ToggleArgument( "D", false );
		private readonly ToggleArgument openHtmlArgument = new ToggleArgument( "O", false );
		private readonly ToggleArgument showHiddenTextArgument = new ToggleArgument( "HT", false );
		private readonly ToggleArgument useNonBreakingSpacesArgument = new ToggleArgument( "NBS", false );
		private readonly ToggleArgument convertVisualHyperlinksArgument = new ToggleArgument( "CH", false );
		private readonly ToggleArgument useInlineStylesArgument = new ToggleArgument( "IS", true );
		private readonly NamedValueArgument visualHyperlinkPatternArgument = new NamedValueArgument( "HP" );
		private readonly ToggleArgument extendedImageScaleArgument = new ToggleArgument( "XS", false );
		private readonly NamedValueArgument documentScopeArgument = new NamedValueArgument( "DS" );
		private readonly ToggleArgument unscaledImagesArgument = new ToggleArgument( "UI", false );
		private readonly ToggleArgument ignoreDuplicatedFontsArgument = new ToggleArgument( "IDF", false );
		private readonly ToggleArgument ignoreUnknownFontsArgument = new ToggleArgument( "IUF", false );

	} // class ProgramSettings

} // namespace RtfPipe.Solutions.Community.Rtf2Html
// -- EOF -------------------------------------------------------------------

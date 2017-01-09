// -- FILE ------------------------------------------------------------------
// name       : ProgramSettings.cs
// project    : RTF Framelet
// created    : Jani Giannoudis - 2008.06.10
// language   : c#
// environment: .NET 2.0
// copyright  : (c) 2004-2013 by Jani Giannoudis, Switzerland
// --------------------------------------------------------------------------
using System.IO;
using System.Text;
using Itenso.Sys.Application;

namespace Itenso.Solutions.Community.Rtf2Xml
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
			get 
			{
				if ( !applicationArguments.IsValid )
				{
					return false;
				}

				if ( !string.IsNullOrEmpty( XmlPrefix ) && string.IsNullOrEmpty( XmlNamespace ) )
				{
					return false;
				}

				return true;
			}
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
		public string XmlPrefix
		{
			get { return xmlPrefixArgument.Value; }
		} // XmlPrefix

		// ----------------------------------------------------------------------
		public string XmlNamespace
		{
			get { return xmlNamespaceArgument.Value; }
		} // XmlNamespace	
		
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
		public bool ShowHiddenText
		{
			get { return showHiddenTextArgument.Value; }
		} // ShowHiddenText

		// ----------------------------------------------------------------------
		public bool IgnoreDuplicatedFonts
		{
			get { return ignoreDuplicatedFontsArgument.Value; }
		} // IgnoreDuplicateFonts

		// ----------------------------------------------------------------------
		public bool IgnoreUnknownFonts
		{
			get { return ignoreUnknownFontsArgument.Value; }
		} // IgnoreUnknownFonts

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
			applicationArguments.Arguments.Add( characterEncodingArgument );
			applicationArguments.Arguments.Add( xmlPrefixArgument );
			applicationArguments.Arguments.Add( xmlNamespaceArgument );
			applicationArguments.Arguments.Add( logDirectoryArgument );
			applicationArguments.Arguments.Add( logParserArgument );
			applicationArguments.Arguments.Add( logInterpreterArgument );
			applicationArguments.Arguments.Add( showHiddenTextArgument );
			applicationArguments.Arguments.Add( ignoreDuplicatedFontsArgument );
			applicationArguments.Arguments.Add( ignoreUnknownFontsArgument );

			applicationArguments.Load();
		} // LoadApplicationArguments

		// ----------------------------------------------------------------------
		// members
		private readonly ApplicationArguments applicationArguments = new ApplicationArguments();
		private readonly ValueArgument sourceFileArgument = new ValueArgument( ArgumentType.Mandatory );
		private readonly ValueArgument destinationDirectoryArgument = new ValueArgument();
		private readonly NamedValueArgument characterEncodingArgument = new NamedValueArgument( "CE" );
		private readonly NamedValueArgument xmlPrefixArgument = new NamedValueArgument( "P" );
		private readonly NamedValueArgument xmlNamespaceArgument = new NamedValueArgument( "NS" );
		private readonly NamedValueArgument logDirectoryArgument = new NamedValueArgument( "LD" );
		private readonly ToggleArgument logParserArgument = new ToggleArgument( "LP", false );
		private readonly ToggleArgument logInterpreterArgument = new ToggleArgument( "LI", false );
		private readonly ToggleArgument showHiddenTextArgument = new ToggleArgument( "HT", false );
		private readonly ToggleArgument ignoreDuplicatedFontsArgument = new ToggleArgument( "IDF", false );
		private readonly ToggleArgument ignoreUnknownFontsArgument = new ToggleArgument( "IUF", false );

	} // class ProgramSettings

} // namespace Itenso.Solutions.Community.Rtf2Xml
// -- EOF -------------------------------------------------------------------

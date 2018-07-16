// project    : System Framelet
// environment: .NET 3.5
using System.Globalization;
using System.Resources;
using System.Text;

namespace RtfPipe.Sys
{


  /// <summary>Provides strongly typed resource access for this namespace.</summary>
  internal sealed class Strings : StringsBase
  {
    public static string ArgumentMayNotBeEmpty
    {
      get { return "argument string may not be empty"; }
    }

    public static string CollectionToolInvalidEnum( string value, string enumType, string possibleValues )
    {
      return Format("'{0}' is not a valid value for {1}. must be one of {2}.", value, enumType, possibleValues );
    }

    public static string LoggerNameMayNotBeEmpty
    {
      get { return "logger name may not be empty"; }
    }

    public static string LoggerFactoryConfigError
    {
      get { return "config error: none of the logger factories can be created"; }
    }

    public static string ProgramPressAnyKeyToQuit
    {
      get { return "press any key to quit ..."; }
    }

    public static string StringToolSeparatorIncludesQuoteOrEscapeChar
    {
      get { return "separators may not include quote or escape character"; }
    }

    public static string StringToolMissingEscapedHexCode
    {
      get { return "missing escaped hex code"; }
    }

    public static string StringToolMissingEscapedChar
    {
      get { return "missing escaped character"; }
    }

    public static string StringToolUnbalancedQuotes
    {
      get { return "unbalanced quotes"; }
    }

    public static string StringToolContainsInvalidHexChar
    {
      get { return "encountered invalid hex char in escape sequence"; }
    }

    public static string LoggerLogFileNotSupportedByType( string typeName )
    {
      return Format("adding log files not supported by {0}", typeName );
    }

    public static string LoggerLoggingLevelXmlError
    {
      get { return "cannot set new logging-level due to an XmlException"; }
    }

    public static string LoggerLoggingLevelRepository
    {
      get { return "cannot set new logging-level as the repository is not configurable"; }
    }
  }

}


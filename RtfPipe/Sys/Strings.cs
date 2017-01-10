// -- FILE ------------------------------------------------------------------
// name       : Strings.cs
// project    : System Framelet
// created    : Jani Giannoudis - 2009.02.19
// language   : c#
// environment: .NET 3.5
// copyright  : (c) 2004-2013 by Jani Giannoudis, Switzerland
// --------------------------------------------------------------------------
using System.Globalization;
using System.Resources;
using System.Text;

namespace RtfPipe.Sys
{

  // ------------------------------------------------------------------------
  /// <summary>Provides strongly typed resource access for this namespace.</summary>
  internal sealed class Strings : StringsBase
  {
    // ----------------------------------------------------------------------
    public static string ArgumentMayNotBeEmpty
    {
      get { return "argument string may not be empty"; }
    } // ArgumentMayNotBeEmpty

    // ----------------------------------------------------------------------
    public static string CollectionToolInvalidEnum( string value, string enumType, string possibleValues )
    {
      return Format("'{0}' is not a valid value for {1}. must be one of {2}.", value, enumType, possibleValues );
    } // CollectionToolInvalidEnum

    // ----------------------------------------------------------------------
    public static string LoggerNameMayNotBeEmpty
    {
      get { return "logger name may not be empty"; }
    } // LoggerNameMayNotBeEmpty

    // ----------------------------------------------------------------------
    public static string LoggerFactoryConfigError
    {
      get { return "config error: none of the logger factories can be created"; }
    } // LoggerFactoryConfigError

    // ----------------------------------------------------------------------
    public static string ProgramPressAnyKeyToQuit
    {
      get { return "press any key to quit ..."; }
    } // ProgramPressAnyKeyToQuit

    // ----------------------------------------------------------------------
    public static string StringToolSeparatorIncludesQuoteOrEscapeChar
    {
      get { return "separators may not include quote or escape character"; }
    } // StringToolSeparatorIncludesQuoteOrEscapeChar

    // ----------------------------------------------------------------------
    public static string StringToolMissingEscapedHexCode
    {
      get { return "missing escaped hex code"; }
    } // StringToolMissingEscapedHexCode

    // ----------------------------------------------------------------------
    public static string StringToolMissingEscapedChar
    {
      get { return "missing escaped character"; }
    } // StringToolMissingEscapedChar

    // ----------------------------------------------------------------------
    public static string StringToolUnbalancedQuotes
    {
      get { return "unbalanced quotes"; }
    } // StringToolUnbalancedQuotes

    // ----------------------------------------------------------------------
    public static string StringToolContainsInvalidHexChar
    {
      get { return "encountered invalid hex char in escape sequence"; }
    } // StringToolContainsInvalidHexChar

    // ----------------------------------------------------------------------
    public static string LoggerLogFileNotSupportedByType( string typeName )
    {
      return Format("adding log files not supported by {0}", typeName );
    } // LoggerLogFileNotSupportedByType

    // ----------------------------------------------------------------------
    public static string LoggerLoggingLevelXmlError
    {
      get { return "cannot set new logging-level due to an XmlException"; }
    } // LoggerLoggingLevelXmlError

    // ----------------------------------------------------------------------
    public static string LoggerLoggingLevelRepository
    {
      get { return "cannot set new logging-level as the repository is not configurable"; }
    } // LoggerLoggingLevelRepository
  } // class Strings

} // namespace RtfPipe.Sys
// -- EOF -------------------------------------------------------------------

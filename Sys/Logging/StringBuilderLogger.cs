// -- FILE ------------------------------------------------------------------
// name       : StringBuilderLogger.cs
// project    : System Framelet
// created    : Leon Poyyayil - 2008.05.14
// language   : c#
// environment: .NET 2.0
// copyright  : (c) 2004-2013 by Jani Giannoudis, Switzerland
// --------------------------------------------------------------------------
using System;
using System.Text;

namespace Itenso.Sys.Logging
{

	// ------------------------------------------------------------------------
	public sealed class StringBuilderLogger : LoggerImplBase
	{

		// ----------------------------------------------------------------------
		public StringBuilderLogger()
		{
		} // StringBuilderLogger

		// ----------------------------------------------------------------------
		public StringBuilderLogger( LoggerLevel level ) : 
			base( level )
		{
		} // StringBuilderLogger

		// ----------------------------------------------------------------------
		public string Buffer
		{
			get { return buffer.ToString(); }
		} // Buffer

		// ----------------------------------------------------------------------
		public void Clear()
		{
			buffer.Remove( 0, buffer.Length );
		} // Clear

		// ----------------------------------------------------------------------
		protected override void Output( LoggerLevel level, object message, Exception exception )
		{
			buffer.Append( level.ToString() );
			buffer.Append( ": " );
			buffer.AppendLine( message == null ? "null" : message.ToString() );
			Output( exception );
		} // Output

		// ----------------------------------------------------------------------
		private void Output( Exception exception )
		{
			if ( exception != null )
			{
				buffer.Append( exception.GetType().FullName );
				buffer.Append( ": " );
				buffer.AppendLine( exception.Message );
				buffer.AppendLine( exception.StackTrace );
				if ( exception.InnerException != null )
				{
					buffer.AppendLine( "Caused by:" );
					Output( exception.InnerException );
				}
			}
		} // Output

		// ----------------------------------------------------------------------
		// members
		private readonly StringBuilder buffer = new StringBuilder();

	} // class StringBuilderLogger

} // namespace Itenso.Sys.Logging
// -- EOF -------------------------------------------------------------------

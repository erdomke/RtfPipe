// project    : System Framelet
using System;
using System.Text;

namespace RtfPipe.Sys.Logging
{

	public sealed class StringBuilderLogger : LoggerImplBase
	{

		public StringBuilderLogger()
		{
		}

		public StringBuilderLogger( LoggerLevel level ) : 
			base( level )
		{
		}

		public string Buffer
		{
			get { return buffer.ToString(); }
		}

		public void Clear()
		{
			buffer.Remove( 0, buffer.Length );
		}

		protected override void Output( LoggerLevel level, object message, Exception exception )
		{
			buffer.Append( level.ToString() );
			buffer.Append( ": " );
			buffer.AppendLine( message == null ? "null" : message.ToString() );
			Output( exception );
		}

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
		}

		private readonly StringBuilder buffer = new StringBuilder();

	}

}


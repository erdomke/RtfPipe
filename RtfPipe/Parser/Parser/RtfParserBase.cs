using System;
using System.Collections;
using System.Collections.Generic;

namespace RtfPipe.Parser
{

	public abstract class RtfParserBase : IRtfParser
	{

		protected RtfParserBase()
		{
		}

		protected RtfParserBase( params IRtfParserListener[] listeners )
		{
			if ( listeners != null )
			{
				foreach ( IRtfParserListener listener in listeners )
				{
					AddParserListener( listener );
				}
			}
		}

		public bool IgnoreContentAfterRootGroup { get; set; }

		public void AddParserListener( IRtfParserListener listener )
		{
			if ( listener == null )
			{
				throw new ArgumentNullException( "listener" );
			}
			if ( listeners == null )
			{
				listeners = new List<IRtfParserListener>();
			}
			if ( !listeners.Contains( listener ) )
			{
				listeners.Add( listener );
			}
		}

		public void RemoveParserListener( IRtfParserListener listener )
		{
			if ( listener == null )
			{
				throw new ArgumentNullException( "listener" );
			}
			if ( listeners != null )
			{
				if ( listeners.Contains( listener ) )
				{
					listeners.Remove( listener );
				}
				if ( listeners.Count == 0 )
				{
					listeners = null;
				}
			}
		}

		public void Parse( IRtfSource rtfTextSource )
		{
			if ( rtfTextSource == null )
			{
				throw new ArgumentNullException( "rtfTextSource" );
			}
			DoParse( rtfTextSource );
		}

		protected abstract void DoParse( IRtfSource rtfTextSource );

		protected void NotifyParseBegin()
		{
			if ( listeners != null )
			{
				foreach ( IRtfParserListener listener in listeners )
				{
					listener.ParseBegin();
				}
			}
		}

		protected void NotifyGroupBegin()
		{
			if ( listeners != null )
			{
				foreach ( IRtfParserListener listener in listeners )
				{
					listener.GroupBegin();
				}
			}
		}

		protected void NotifyTagFound( IRtfTag tag )
		{
			if ( listeners != null )
			{
				foreach ( IRtfParserListener listener in listeners )
				{
					listener.TagFound( tag );
				}
			}
		}

		protected void NotifyTextFound( IRtfText text )
		{
			if ( listeners != null )
			{
				foreach ( IRtfParserListener listener in listeners )
				{
					listener.TextFound( text );
				}
			}
		}

		protected void NotifyGroupEnd()
		{
			if ( listeners != null )
			{
				foreach ( IRtfParserListener listener in listeners )
				{
					listener.GroupEnd();
				}
			}
		}

		protected void NotifyParseSuccess()
		{
			if ( listeners != null )
			{
				foreach ( IRtfParserListener listener in listeners )
				{
					listener.ParseSuccess();
				}
			}
		}

		protected void NotifyParseFail( RtfException reason )
		{
			if ( listeners != null )
			{
				foreach ( IRtfParserListener listener in listeners )
				{
					listener.ParseFail( reason );
				}
			}
		}

		protected void NotifyParseEnd()
		{
			if ( listeners != null )
			{
				foreach ( IRtfParserListener listener in listeners )
				{
					listener.ParseEnd();
				}
			}
		}

		private List<IRtfParserListener> listeners;

	}

}


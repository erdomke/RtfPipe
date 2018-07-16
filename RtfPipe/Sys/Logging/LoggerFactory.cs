// project    : System Framelet
using System;

namespace RtfPipe.Sys.Logging
{

	internal abstract class LoggerFactory
	{

		public static bool InitializeLoggerFactory( string factoryName )
		{
			if ( instance == null )
			{
				lock ( mutex )
				{
					if ( instance == null )
					{
						LoggerFactoryBuilder.SetDefaultLoggerFactory( factoryName );
					}
				}
			}
			return string.Equals( Instance.GetType().FullName, factoryName );
		}

		public static LoggerFactory Instance
		{
			get
			{
				if ( instance == null )
				{
					lock ( mutex )
					{
						if ( instance == null )
						{
							instance = LoggerFactoryBuilder.BuildFactoryInstance();
						}
					}
				}
				return instance;
			}
		}

		public abstract ILogger GetLogger( string name );

		public ILoggerMonitor Monitor
		{
			get
			{
				if ( monitor == null )
				{
					lock ( this )
					{
						if ( monitor == null )
						{
							monitor = CreateMonitor();
						}
					}
				}
				return monitor;
			}
		}

		protected virtual ILoggerMonitor CreateMonitor()
		{
			return new LoggerMonitorNone();
		}

		public virtual void SetLogFile( string absoluteLogFileName, bool append, string messagePattern )
		{
			throw new InvalidOperationException( Strings.LoggerLogFileNotSupportedByType( GetType().FullName ) );
		}

		private static readonly object mutex = new object();
		private static volatile LoggerFactory instance;

		private volatile ILoggerMonitor monitor;

	}

}


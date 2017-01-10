// -- FILE ------------------------------------------------------------------
// name       : LoggerFactoryBuilder.cs
// project    : System Framelet
// created    : Leon Poyyayil - 2005.05.04
// language   : c#
// environment: .NET 2.0
// copyright  : (c) 2004-2013 by Jani Giannoudis, Switzerland
// --------------------------------------------------------------------------
using System;

namespace Itenso.Sys.Logging
{

	// ------------------------------------------------------------------------
	internal static class LoggerFactoryBuilder
	{

		// ----------------------------------------------------------------------
		internal static void SetDefaultLoggerFactory( string factoryName )
		{
			defaultLoggerFactoryName = factoryName;
		} // SetDefaultLoggerFactory

		// ----------------------------------------------------------------------
		internal static LoggerFactory BuildFactoryInstance()
		{
			try
			{
				string envType = Environment.GetEnvironmentVariable( environmentVariableName );
				LoggerFactory factory = CreateInstance( envType );
				if ( factory == null && defaultLoggerFactoryName != null )
				{
					factory = CreateInstance( defaultLoggerFactoryName );
				}
				int i = 0;
				while ( factory == null && i < loggerFactoryChoices.Length )
				{
					factory = CreateInstance( loggerFactoryChoices[ i ] );
					i++;
				}
				if ( factory == null )
				{
					throw new InvalidOperationException( Strings.LoggerFactoryConfigError );
				}
				return factory;
			}
			catch ( System.Security.SecurityException e )
			{
				throw new InvalidOperationException( Strings.LoggerFactoryConfigError, e );
			}
		} // BuildFactoryInstance

		// ----------------------------------------------------------------------
		private static LoggerFactory CreateInstance( string typeName )
		{
			LoggerFactory factory = null;
			if ( !string.IsNullOrEmpty( typeName ) )
			{
				try
				{
					Type factoryType = Type.GetType( typeName, false );
// ReSharper disable AssignNullToNotNullAttribute
					LoggerFactory newFactory = (LoggerFactory)Activator.CreateInstance( factoryType );
// ReSharper restore AssignNullToNotNullAttribute
					// when creating the factory succeeded, we first need to test whether it will
					// be able to create loggers too, before we accept it as a valid factory.
					ILogger factoryLogger = newFactory.GetLogger( typeof( LoggerFactory ).FullName );
					factoryLogger.Info( "using LoggerFactory: " + typeName );
					// well, now we have been able to use the factory and a logger from it, so
					// we may safely accept it for usage outside.
					factory = newFactory;
				}
// ReSharper disable EmptyGeneralCatchClause
				catch
// ReSharper restore EmptyGeneralCatchClause
				{
					// yes, ignore for the moment
				}
			}
			return factory;
		} // CreateInstance

		// ----------------------------------------------------------------------
		// members
		private static readonly string environmentVariableName = typeof( LoggerFactory ).FullName;
		private static readonly string[] loggerFactoryChoices = {
			typeof( LoggerFactoryTrace ).FullName,
			typeof( LoggerFactoryNone ).FullName
		};
		private static string defaultLoggerFactoryName;

	} // class LoggerFactoryBuilder

} // namespace Itenso.Sys.Logging
// -- EOF -------------------------------------------------------------------

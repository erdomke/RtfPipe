// -- FILE ------------------------------------------------------------------
// name       : ConsoleRunner.cs
// project    : System Framelet
// created    : Jani Giannoudis - 2008.06.02
// language   : c#
// environment: .NET 2.0
// copyright  : (c) 2004-2013 by Jani Giannoudis, Switzerland
// --------------------------------------------------------------------------
using System;
using System.Reflection;
using NUnit.ConsoleRunner;

namespace RtfPipe.Sys.Test
{

	// ------------------------------------------------------------------------
	public class ConsoleRunner
	{

		// ----------------------------------------------------------------------
		static public void Execute( Assembly assembly, string[] args )
		{
			if ( assembly == null )
			{
				throw new ArgumentNullException( "assembly" );
			}

			bool waitAtEnd = false;
			bool waitBecauseOfError = false;
			Console.WriteLine( "===== Test::begin =====" );
			try
			{
				if ( args != null && args.Length > 0 )
				{
					foreach ( string arg in args )
					{
						if ( "-wait".Equals( arg ) )
						{
							waitAtEnd = true;
						}
					}
				}
// ReSharper disable RedundantExplicitArrayCreation
				string[] nunitArgs = new string[]{ assembly.GetName().Name + ".exe" };
// ReSharper restore RedundantExplicitArrayCreation
				ConsoleUi.Main( nunitArgs );
			}
			catch ( Exception e )
			{
				Console.WriteLine( e.Message );
				Console.WriteLine( e.StackTrace );
				waitBecauseOfError = true;
			}
			finally
			{
				Console.WriteLine( "===== Test::end =====" );
			}
			if ( waitAtEnd || waitBecauseOfError )
			{
				Console.WriteLine( "Press any key to quit" );
				Console.ReadKey( true );
			}
		} // Execute

	} // class ConsoleRunner

} // namespace RtfPipe.Sys.Test
// -- EOF -------------------------------------------------------------------

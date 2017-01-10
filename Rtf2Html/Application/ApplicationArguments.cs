// -- FILE ------------------------------------------------------------------
// name       : ApplicationArguments.cs
// project    : System Framelet
// created    : Jani Giannoudis - 2008.06.03
// language   : c#
// environment: .NET 2.0
// copyright  : (c) 2004-2013 by Jani Giannoudis, Switzerland
// --------------------------------------------------------------------------
using System;
using System.Collections.Generic;

namespace RtfPipe.Sys.Application
{

	// ------------------------------------------------------------------------
	public class ApplicationArguments
	{

		// ----------------------------------------------------------------------
		public IList<IArgument> Arguments
		{
			get { return arguments; }
		} // Arguments

		// ----------------------------------------------------------------------
		public bool IsValid
		{
			get
      {
        foreach (var argument in arguments)
        {
          if (!argument.IsValid)
          {
            return false;
          }
        }
        return true;
      }
		} // IsValid

		// ----------------------------------------------------------------------
		public bool IsHelpMode
		{
			get 
			{
				foreach ( IArgument argument in arguments )
				{
					if ( argument is HelpModeArgument )
					{
						return (bool)argument.Value;
					}
				}
				return false;
			}
		} // IsHelpMode

		// ----------------------------------------------------------------------
		public void Load(string[] commandLineArgs)
		{
			// skip zeron index which contians the program name
			for ( int i = 1; i < commandLineArgs.Length; i++ )
			{
				string commandLineArg = commandLineArgs[ i ];
				foreach ( IArgument argument in arguments )
				{
					if ( argument.IsLoaded )
					{
						continue;
					}
					argument.Load( commandLineArg );
					if ( argument.IsLoaded )
					{
						break;
					}
				}
			}
		} // Load

		// ----------------------------------------------------------------------
		// members
		private readonly List<IArgument> arguments = new List<IArgument>();

	} // class ApplicationArguments

} // namespace RtfPipe.Sys.Application
// -- EOF -------------------------------------------------------------------

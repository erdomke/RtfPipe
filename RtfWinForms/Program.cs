// -- FILE ------------------------------------------------------------------
// name       : Program.cs
// project    : RTF Framelet
// created    : Jani Giannoudis - 2008.07.30
// language   : c#
// environment: .NET 2.0
// copyright  : (c) 2004-2013 by Jani Giannoudis, Switzerland
// --------------------------------------------------------------------------
using System;
using System.Windows.Forms;

namespace Itenso.Solutions.Community.RtfConverter.RtfWinForms
{
	// ------------------------------------------------------------------------
	static class Program
	{

		// ----------------------------------------------------------------------
		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault( false );
			Application.Run( new RtfForm() );
		} // Main

	} // class Program

} // namespace Itenso.Solutions.Community.RtfConverter.RtfWinForms
// -- EOF -------------------------------------------------------------------
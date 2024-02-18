namespace WhyIUseAwait;

using System;
using System.Runtime.Versioning;
using System.Windows.Forms;

[SupportedOSPlatform( "windows" )]
public static class Program {

	/// <summary>
	///     The main entry point for the application.
	/// </summary>
	[STAThread]
	public static void Main() {
		Application.EnableVisualStyles();
		Application.SetCompatibleTextRenderingDefault( false );

		using var form = new Form1();

		Application.Run( form );
	}
}
namespace WhyIUseAwait;

using System;
using System.Diagnostics;
using System.Runtime.Versioning;
using System.Threading.Tasks;
using System.Windows.Forms;
using JetBrains.Annotations;

[SupportedOSPlatform( "windows" )]
public partial class Form1 : Form {

	public Form1() {
            this.InitializeComponent();
        }

	private static UInt64 LongCalculation( TimeSpan time, IProgress<Int32> report ) {
            var stopwatch = Stopwatch.StartNew();

            UInt64 summary = 0;
            var lastReport = 0;

            do {
                summary += 1;
                var current = ( Int32 )stopwatch.Elapsed.TotalSeconds;

                if ( current > lastReport ) {
                    report.Report( current );
                    lastReport = current;
                }
            } while ( stopwatch.Elapsed < time );

            report.Report( 0 );

            return summary;
        }

	private void ButtonDone_Click( Object sender, EventArgs e ) {
            this.InvokeAction( this.Close );
        }

	private async void ButtonGoWith_Click( Object sender, EventArgs e ) {
            try {
                ( sender as Control )?.Enabled( false );

                if ( this.GetValue( out var time ) ) {
                    this.Reset( time );
                    this.Message( String.Empty );
                    this.Message( "Running calculation WITH await." );
                    this.Message( "Try moving this form.." );
                    var result = await Task.Run( () => LongCalculation( time, new Progress<Int32>( this.ReportProgress ) ) ).ConfigureAwait( false );
                    this.Message( $"The result is {result:N0}." );
                }
            }
            finally {
                ( sender as Control )?.Enabled( true );
            }
        }

	private void ButtonGoWithout_Click( Object sender, EventArgs e ) {
            try {
                ( sender as Control )?.Enabled( false );

                if ( this.GetValue( out var time ) ) {
                    this.Reset( time );
                    this.Message( String.Empty );
                    this.Message( "Running calculation WITHOUT await." );
                    this.Message( "Try moving this form.." );
                    var result = LongCalculation( time, new Progress<Int32>( this.ReportProgress ) );
                    this.Message( $"The result is {result:N0}." );
                }
            }
            finally {
                ( sender as Control )?.Enabled( true );
            }
        }

	private void Form1_Shown( Object sender, EventArgs e ) {
            this.Message( "Ready. Press either Run button." );
        }

	private Boolean GetValue( out TimeSpan time ) {
            if ( !Int32.TryParse( this.textBoxSeconds.Text, out var seconds ) ) {
                Console.Beep();
                this.Message( "Unable to parse value. Please enter a number." );
                time = default( TimeSpan );

                return default( Boolean );
            }

            time = TimeSpan.FromSeconds( seconds );

            return true;
        }

	private void Message( String? text ) {
            this.textBoxMain.Text( $"{this.textBoxMain.Text()}{Environment.NewLine}{text}" );
            this.textBoxMain.InvokeAction( () => this.textBoxMain.ScrollToCaret() );
        }

	private void ReportProgress( Int32 value ) {
		var owner = this.toolStripProgressBar1.Owner;

		if ( owner is null || owner.IsDisposed ) {
			return;
		}

		owner.InvokeAction( () => {
			this.toolStripProgressBar1.Value = value;

			if ( !owner.IsDisposed ) {
				owner.Refresh();
			}
		} );
	}

	private void Reset( TimeSpan time ) {
            this.toolStripProgressBar1.Minimum = 0;
            this.toolStripProgressBar1.Maximum = 1 + ( Int32 )time.TotalSeconds;
            this.toolStripProgressBar1.Value = this.toolStripProgressBar1.Minimum;
        }
}
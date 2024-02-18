namespace WhyIUseAwait;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Versioning;
using System.Windows.Forms;

[SupportedOSPlatform( "windows" )]
public static class Extensions {

	/// <summary>
	///     Safely set the <see cref="Control.Enabled" /> of the control across threads.
	/// </summary>
	/// <param name="control"></param>
	/// <param name="value">  </param>
	public static void Enabled( this Control control, Boolean value ) {
		ArgumentNullException.ThrowIfNull(control);

		control.InvokeAction( () => {
			if ( !control.IsDisposed ) {
				control.Enabled = value;

				control.Refresh();
			}
		} );
	}

	/// <summary>
	///     <para>Perform an <see cref="Action" /> on the control's thread.</para>
	/// </summary>
	/// <param name="control"></param>
	/// <param name="action"> </param>
	/// <seealso />
	public static void InvokeAction( this Control control, Action action ) {
		ArgumentNullException.ThrowIfNull(control);

		ArgumentNullException.ThrowIfNull(action);

		if ( control.InvokeRequired ) {
			if ( !control.IsDisposed ) {
				control.Invoke( action );
			}
		}
		else {
			action();
		}
	}

	/// <summary>
	///     <para>Works like the SQL "nullif" function.</para>
	///     <para>
	///         If <paramref name="left" /> is equal to <paramref name="right" /> then return null for classes or the default
	///         value for
	///         value types.
	///     </para>
	///     <para>Otherwise return <paramref name="left" />.</para>
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="left"> </param>
	/// <param name="right"></param>
	/// <returns></returns>
	[DebuggerStepThrough]
	public static T? NullIf<T>( [DisallowNull] this T left, T right ) => Comparer<T>.Default.Compare( left, right ) == 0 ? default( T? ) : left;

	/// <summary>
	///     <para>Safely get the <see cref="Control.Text" /> of a <see cref="Control" /> across threads.</para>
	/// </summary>
	/// <param name="control"></param>
	/// <returns></returns>
	public static String? Text( this Control control ) {
		ArgumentNullException.ThrowIfNull(control);

		if ( control.IsDisposed ) {
			return default( String? );
		}

		if ( !control.InvokeRequired ) {
			return control.Text;
		}

		if ( control.Invoke( () => control.Text ) is String s ) {
			return s;
		}

		return default( String? );
	}

	/// <summary>
	///     <para>Safely set the <see cref="Control.Text" /> of a control across threads.</para>
	/// </summary>
	/// <remarks></remarks>
	/// <param name="control"></param>
	/// <param name="value">  </param>
	public static void Text( this Control control, String? value ) {
		ArgumentNullException.ThrowIfNull(control);

		control.InvokeAction( () => {
			if ( control.IsDisposed ) {
				return;
			}

			control.Text = value;
			control.Update();
		} );
	}
}
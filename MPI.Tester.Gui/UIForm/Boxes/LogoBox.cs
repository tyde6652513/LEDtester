using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace MPI.Test.Gui
{
	/// <summary>
	/// IInitializationList interface.
	/// </summary>
	public interface IInitializationList
	{
		void AddInitializeItem( string description );
		void ReportInitializeItem( bool success );
		void ReportProgress( int percentage );
	}

	/// <summary>
	/// LogoBox class.
	/// </summary>
	public partial class LogoBox : System.Windows.Forms.Form, IInitializationList
	{
		private const int ITEM_DESCRIPTION = 0;
		private const int ITEM_STATUS = 1;
		private const int ITEM_TIME = 2;
		private const string ITEM_STATUS_SUCCESS = "OK";
		private const string ITEM_STATUS_FAIL = "Fail";

		/// <summary>
		/// Constructor.
		/// </summary>
		public LogoBox()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
		}

		#region >>> Public Method <<<

		/// <summary>
		/// Add new initialize item to list.
		/// </summary>
		public void AddInitializeItem( string description )
		{
			// clear selection
			this.lstInitializeList.SelectedIndices.Clear();

			// add new item
			this.lstInitializeList.Items.Add( description );

			// select focus item
			this.lstInitializeList.SelectedIndices.Add( this.lstInitializeList.Items.Count - 1 );

			this.lstInitializeList.EnsureVisible( this.lstInitializeList.Items.Count - 1 );

			// update
			this.lstInitializeList.Update();
		}

		/// <summary>
		/// Report initialize status of item.
		/// </summary>
		public void ReportInitializeItem( bool success )
		{ 
			if ( this.lstInitializeList.Items.Count > 0 )
			{
				int index = this.lstInitializeList.Items.Count - 1;

				// status
				if ( success )
					this.lstInitializeList.Items[ index ].SubItems.Add( ITEM_STATUS_SUCCESS );
				else
					this.lstInitializeList.Items[ index ].SubItems.Add( ITEM_STATUS_FAIL );

				// time
				this.lstInitializeList.Items[ index ].SubItems.Add( DateTime.Now.ToString( "hh:mm:ss" ) );

				// update
				this.lstInitializeList.Update();
			}
		}

		/// <summary>
		/// Report progress of initialization.
		/// </summary>
		public void ReportProgress( int percentage )
		{
			if ( ( percentage > this.prgInitializeProgress.Maximum ) || ( percentage < this.prgInitializeProgress.Minimum ) )
				return;

			this.prgInitializeProgress.Value = percentage;
		}

		#endregion
	}
}

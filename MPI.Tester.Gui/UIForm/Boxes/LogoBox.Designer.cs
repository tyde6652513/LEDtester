using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace MPI.Test.Gui
{
	public partial class LogoBox
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose( bool disposing )
		{
			if ( disposing )
			{
				if ( components != null )
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager( typeof( LogoBox ) );
			this.prgInitializeProgress = new System.Windows.Forms.ProgressBar();
			this.lstInitializeList = new System.Windows.Forms.ListView();
			this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
			this.SuspendLayout();
			// 
			// prgInitializeProgress
			// 
			this.prgInitializeProgress.Location = new System.Drawing.Point( 518, 211 );
			this.prgInitializeProgress.Name = "prgInitializeProgress";
			this.prgInitializeProgress.Size = new System.Drawing.Size( 424, 14 );
			this.prgInitializeProgress.TabIndex = 0;
			// 
			// lstInitializeList
			// 
			this.lstInitializeList.Columns.AddRange( new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3} );
			this.lstInitializeList.FullRowSelect = true;
			this.lstInitializeList.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.lstInitializeList.HideSelection = false;
			this.lstInitializeList.Location = new System.Drawing.Point( 518, 72 );
			this.lstInitializeList.MultiSelect = false;
			this.lstInitializeList.Name = "lstInitializeList";
			this.lstInitializeList.ShowGroups = false;
			this.lstInitializeList.Size = new System.Drawing.Size( 424, 135 );
			this.lstInitializeList.TabIndex = 1;
			this.lstInitializeList.UseCompatibleStateImageBehavior = false;
			this.lstInitializeList.View = System.Windows.Forms.View.Details;
			// 
			// columnHeader1
			// 
			this.columnHeader1.Text = "Items";
			this.columnHeader1.Width = 260;
			// 
			// columnHeader2
			// 
			this.columnHeader2.Text = "Status";
			// 
			// columnHeader3
			// 
			this.columnHeader3.Text = "Time";
			this.columnHeader3.Width = 80;
			// 
			// LogoBox
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.BackgroundImage = ( ( System.Drawing.Image ) ( resources.GetObject( "$this.BackgroundImage" ) ) );
			this.ClientSize = new System.Drawing.Size( 954, 239 );
			this.ControlBox = false;
			this.Controls.Add( this.lstInitializeList );
			this.Controls.Add( this.prgInitializeProgress );
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.KeyPreview = true;
			this.Name = "LogoBox";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.ResumeLayout( false );

		}
		#endregion

		private ProgressBar prgInitializeProgress;
		private ListView lstInitializeList;
		private ColumnHeader columnHeader1;
		private ColumnHeader columnHeader2;
		private ColumnHeader columnHeader3;
	}
}

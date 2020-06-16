using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace MPI.Test.Gui
{
	public partial class OpenFileBox
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
			this.grpInput = new System.Windows.Forms.GroupBox();
			this.btnOpenDialog = new System.Windows.Forms.Button();
			this.txtInput = new System.Windows.Forms.TextBox();
			this.btnCancel_I18N = new System.Windows.Forms.Button();
			this.btnOK_I18N = new System.Windows.Forms.Button();
			this.labPrompt = new System.Windows.Forms.Label();
			this.grpInput.SuspendLayout();
			this.SuspendLayout();
			// 
			// grpInput
			// 
			this.grpInput.BackColor = System.Drawing.Color.WhiteSmoke;
			this.grpInput.Controls.Add( this.btnOpenDialog );
			this.grpInput.Controls.Add( this.txtInput );
			this.grpInput.Controls.Add( this.btnCancel_I18N );
			this.grpInput.Controls.Add( this.btnOK_I18N );
			this.grpInput.Controls.Add( this.labPrompt );
			this.grpInput.Font = new System.Drawing.Font( "Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ( ( byte ) ( 0 ) ) );
			this.grpInput.Location = new System.Drawing.Point( 2, 3 );
			this.grpInput.Name = "grpInput";
			this.grpInput.Size = new System.Drawing.Size( 402, 143 );
			this.grpInput.TabIndex = 86;
			this.grpInput.TabStop = false;
			this.grpInput.Text = "Open File";
			// 
			// btnOpenDialog
			// 
			this.btnOpenDialog.BackColor = System.Drawing.Color.Silver;
			this.btnOpenDialog.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnOpenDialog.Font = new System.Drawing.Font( "Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( ( byte ) ( 0 ) ) );
			this.btnOpenDialog.ForeColor = System.Drawing.Color.Black;
			this.btnOpenDialog.Location = new System.Drawing.Point( 365, 62 );
			this.btnOpenDialog.Name = "btnOpenDialog";
			this.btnOpenDialog.Size = new System.Drawing.Size( 23, 22 );
			this.btnOpenDialog.TabIndex = 78;
			this.btnOpenDialog.Text = "...";
			this.btnOpenDialog.UseVisualStyleBackColor = false;
			this.btnOpenDialog.Click += new System.EventHandler( this.btnOpenDialog_Click );
			// 
			// txtInput
			// 
			this.txtInput.Font = new System.Drawing.Font( "Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( ( byte ) ( 0 ) ) );
			this.txtInput.Location = new System.Drawing.Point( 24, 62 );
			this.txtInput.Name = "txtInput";
			this.txtInput.Size = new System.Drawing.Size( 339, 22 );
			this.txtInput.TabIndex = 75;
			// 
			// btnCancel_I18N
			// 
			this.btnCancel_I18N.BackColor = System.Drawing.Color.CornflowerBlue;
			this.btnCancel_I18N.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel_I18N.Font = new System.Drawing.Font( "Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( ( byte ) ( 0 ) ) );
			this.btnCancel_I18N.ForeColor = System.Drawing.Color.Black;
			this.btnCancel_I18N.Location = new System.Drawing.Point( 300, 95 );
			this.btnCancel_I18N.Name = "btnCancel_I18N";
			this.btnCancel_I18N.Size = new System.Drawing.Size( 88, 38 );
			this.btnCancel_I18N.TabIndex = 77;
			this.btnCancel_I18N.Text = "Cancel";
			this.btnCancel_I18N.UseVisualStyleBackColor = false;
			this.btnCancel_I18N.Click += new System.EventHandler( this.btnCancel_I18N_Click );
			// 
			// btnOK_I18N
			// 
			this.btnOK_I18N.BackColor = System.Drawing.Color.CornflowerBlue;
			this.btnOK_I18N.Font = new System.Drawing.Font( "Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( ( byte ) ( 0 ) ) );
			this.btnOK_I18N.ForeColor = System.Drawing.Color.Black;
			this.btnOK_I18N.Location = new System.Drawing.Point( 202, 95 );
			this.btnOK_I18N.Name = "btnOK_I18N";
			this.btnOK_I18N.Size = new System.Drawing.Size( 88, 38 );
			this.btnOK_I18N.TabIndex = 76;
			this.btnOK_I18N.Text = "OK";
			this.btnOK_I18N.UseVisualStyleBackColor = false;
			this.btnOK_I18N.Click += new System.EventHandler( this.btnOK_I18N_Click );
			// 
			// labPrompt
			// 
			this.labPrompt.BackColor = System.Drawing.Color.LightSteelBlue;
			this.labPrompt.Font = new System.Drawing.Font( "Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( ( byte ) ( 0 ) ) );
			this.labPrompt.Location = new System.Drawing.Point( 24, 38 );
			this.labPrompt.Name = "labPrompt";
			this.labPrompt.Size = new System.Drawing.Size( 364, 22 );
			this.labPrompt.TabIndex = 72;
			this.labPrompt.Text = "File Name";
			this.labPrompt.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// OpenFileBoxEx
			// 
			this.AcceptButton = this.btnOK_I18N;
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.CancelButton = this.btnCancel_I18N;
			this.ClientSize = new System.Drawing.Size( 406, 148 );
			this.ControlBox = false;
			this.Controls.Add( this.grpInput );
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "OpenFileBoxEx";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Open File Box";
			this.TopMost = true;
			this.Activated += new System.EventHandler( this.InputBox_Activated );
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler( this.OpenFileBox_FormClosing );
			this.grpInput.ResumeLayout( false );
			this.grpInput.PerformLayout();
			this.ResumeLayout( false );

		}
		#endregion

		private System.Windows.Forms.Button btnOK_I18N;
		private System.Windows.Forms.Button btnCancel_I18N;
		private System.Windows.Forms.Label labPrompt;
		private System.Windows.Forms.GroupBox grpInput;
		private System.Windows.Forms.TextBox txtInput;
		private Button btnOpenDialog;
	}
}

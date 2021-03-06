﻿using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace MPI.Tester.Gui
{
	/// <summary>
	/// Summary description for InputComboBox.
	/// </summary>
	public class InputComboBox : System.Windows.Forms.Form
	{
		// private member
		private DialogResult _dialogResult;

		private System.Windows.Forms.Button btnOK_I18N;
		private System.Windows.Forms.Button btnCancel_I18N;
		private System.Windows.Forms.ComboBox cmbInput;
		private System.Windows.Forms.Label labPrompt;
		private System.Windows.Forms.GroupBox grpInput;

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public InputComboBox( string title, string inputFrame, string prompt, object [] list, object defaultValue )
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
			// initial member fields
			this._dialogResult = DialogResult.None;

			// set title
			this.Title = title;

			// set prompt
			this.Prompt = prompt;

			// input frame title
			this.InputFrameTitle = inputFrame;

			// set combo value list
			this.cmbInput.Items.AddRange( list );

			// set default value
			if ( defaultValue != null )
			{
				int index = this.cmbInput.Items.IndexOf( defaultValue );
				if ( index >= 0 )
					this.cmbInput.SelectedIndex = index;
			}

			// set active control
			this.cmbInput.Focus();
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
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
			this.btnCancel_I18N = new System.Windows.Forms.Button();
			this.btnOK_I18N = new System.Windows.Forms.Button();
			this.cmbInput = new System.Windows.Forms.ComboBox();
			this.labPrompt = new System.Windows.Forms.Label();
			this.grpInput.SuspendLayout();
			this.SuspendLayout();
			// 
			// grpInput
			// 
			this.grpInput.BackColor = System.Drawing.Color.WhiteSmoke;
			this.grpInput.Controls.Add( this.btnCancel_I18N );
			this.grpInput.Controls.Add( this.btnOK_I18N );
			this.grpInput.Controls.Add( this.cmbInput );
			this.grpInput.Controls.Add( this.labPrompt );
			this.grpInput.Font = new System.Drawing.Font( "Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ( ( byte ) ( 0 ) ) );
			this.grpInput.Location = new System.Drawing.Point( 2, 3 );
			this.grpInput.Name = "grpInput";
			this.grpInput.Size = new System.Drawing.Size( 212, 198 );
			this.grpInput.TabIndex = 86;
			this.grpInput.TabStop = false;
			this.grpInput.Text = "Input";
			// 
			// btnCancel_I18N
			// 
			this.btnCancel_I18N.BackColor = System.Drawing.Color.CornflowerBlue;
			this.btnCancel_I18N.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel_I18N.Font = new System.Drawing.Font( "Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( ( byte ) ( 0 ) ) );
			this.btnCancel_I18N.ForeColor = System.Drawing.Color.Black;
			this.btnCancel_I18N.Location = new System.Drawing.Point( 110, 146 );
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
			this.btnOK_I18N.Location = new System.Drawing.Point( 12, 146 );
			this.btnOK_I18N.Name = "btnOK_I18N";
			this.btnOK_I18N.Size = new System.Drawing.Size( 88, 38 );
			this.btnOK_I18N.TabIndex = 76;
			this.btnOK_I18N.Text = "OK";
			this.btnOK_I18N.UseVisualStyleBackColor = false;
			this.btnOK_I18N.Click += new System.EventHandler( this.btnOK_I18N_Click );
			// 
			// cmbInput
			// 
			this.cmbInput.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbInput.Font = new System.Drawing.Font( "Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( ( byte ) ( 0 ) ) );
			this.cmbInput.Location = new System.Drawing.Point( 24, 72 );
			this.cmbInput.Name = "cmbInput";
			this.cmbInput.Size = new System.Drawing.Size( 164, 22 );
			this.cmbInput.TabIndex = 73;
			// 
			// labPrompt
			// 
			this.labPrompt.BackColor = System.Drawing.Color.LightSteelBlue;
			this.labPrompt.Font = new System.Drawing.Font( "Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( ( byte ) ( 0 ) ) );
			this.labPrompt.Location = new System.Drawing.Point( 24, 48 );
			this.labPrompt.Name = "labPrompt";
			this.labPrompt.Size = new System.Drawing.Size( 164, 22 );
			this.labPrompt.TabIndex = 72;
			this.labPrompt.Text = "Prompt";
			this.labPrompt.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// InputComboBox
			// 
			this.AcceptButton = this.btnOK_I18N;
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.CancelButton = this.btnCancel_I18N;
			this.ClientSize = new System.Drawing.Size( 216, 203 );
			this.ControlBox = false;
			this.Controls.Add( this.grpInput );
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "InputComboBox";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "InputComboBox";
			this.TopMost = true;
			this.grpInput.ResumeLayout( false );
			this.ResumeLayout( false );

		}
		#endregion

		#region >>> Public Property <<<

		/// <summary>
		/// Prompt.
		/// </summary>
		public string Prompt
		{
			get { return this.labPrompt.Text; }
			set { this.labPrompt.Text = value; }
		}

		/// <summary>
		/// Dialog result.
		/// </summary>
		public DialogResult DialogResult
		{
			get { return this._dialogResult; }
		}

		/// <summary>
		/// Title.
		/// </summary>
		public string Title
		{
			get { return this.Text; }
			set { this.Text = value; }
		}

		/// <summary>
		/// Input frame title.
		/// </summary>
		public string InputFrameTitle
		{
			get { return this.grpInput.Text; }
			set { this.grpInput.Text = value; }
		}

		/// <summary>
		/// Input value.
		/// </summary>
		public object InputValue
		{
			get { return this.cmbInput.SelectedItem; }
		}

		#endregion

		private void btnOK_I18N_Click(object sender, System.EventArgs e)
		{
			this._dialogResult = DialogResult.OK; 
			this.Close();
		}

		private void btnCancel_I18N_Click(object sender, System.EventArgs e)
		{
			this._dialogResult = DialogResult.Cancel; 
			this.Close();
		}
	}
}

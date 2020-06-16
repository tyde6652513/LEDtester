using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace MPI.Test.Gui
{
	/// <summary>
	/// InputBox class.
	/// </summary>
	public partial class InputBox : DevComponents.DotNetBar.Office2007RibbonForm
	{
		// private member
		private DialogResult _dialogResult;

		/// <summary>
		/// Constructor.
		/// </summary>
		public InputBox( string title, string inputFrame, string prompt, string defaultValue, char passwordChar )
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

			// set password char
			this.txtInput.PasswordChar = passwordChar;

			// set default value
			this.txtInput.Text = defaultValue;
			this.txtInput.SelectAll();

			// set active control
			this.txtInput.Focus();
		}

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
		public string InputValue
		{
			get { return this.txtInput.Text; }
		}

		/// <summary>
		/// Password char.
		/// </summary>
		public char PasswordChar
		{
			get { return this.txtInput.PasswordChar; }
			set { this.txtInput.PasswordChar = value; }
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

		private void InputBox_Activated(object sender, System.EventArgs e)
		{
			this.txtInput.SelectAll();

			// set active control
			this.txtInput.Focus();
		}

		private void InputBox_Load(object sender, System.EventArgs e)
		{
		
		}
	}
}

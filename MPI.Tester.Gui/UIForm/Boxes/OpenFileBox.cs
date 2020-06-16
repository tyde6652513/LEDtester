using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace MPI.Test.Gui
{
	/// <summary>
	/// OpenFileBoxEx class.
	/// </summary>
	public partial class OpenFileBox : System.Windows.Forms.Form
	{
		// private member
		private DialogResult _dialogResult;
		private bool _canCloseBox;
		private string _initialPath;
		private string _defaultExt;
		private string _filter;

		/// <summary>
		/// Constructor.
		/// </summary>
		public OpenFileBox( string title, string inputFrame, string prompt, string initialPath, string defaultExt, string filter )
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
			this._initialPath = initialPath;
			this._defaultExt = defaultExt;
			this._filter = filter;
				
			this.btnOpenDialog.Enabled = true;

			// initial member fields
			this._canCloseBox = false;

			this._dialogResult = DialogResult.None;

			// set title
			this.Title = title;

			// set prompt
			this.Prompt = prompt;

			// input frame title
			this.InputFrameTitle = inputFrame;

			// set default value
			this.txtInput.Text = "";
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
			get 
			{
				string str = this.txtInput.Text;
				
				// default extension
				if ( ( this._defaultExt != null ) && ( this._defaultExt != "" ) )
				{
					string ext = System.IO.Path.GetExtension( str );

					if ( ( ext == null ) || ( ext == "" ) )
					{
						str = str.Replace( ".", "" );
						str = str + "." + this._defaultExt;
					}
				}

				// initial path 
				if ( ( this._initialPath != null ) && ( this._initialPath != "" ) )
				{
					if ( this._initialPath[ this._initialPath.Length - 1 ] != '\\' )
						str = this._initialPath + @"\" + str;
					else
						str = this._initialPath + str;
				}

				return str;
			}
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

		private void InputBox_Activated( object sender, System.EventArgs e )
		{
			this.txtInput.SelectAll();

			// set active control
			this.txtInput.Focus();
		}

		private void btnOK_I18N_Click(object sender, System.EventArgs e)
		{
			this._canCloseBox = true;

			this._dialogResult = DialogResult.OK;
			
			this.Close();
		}

		private void btnCancel_I18N_Click(object sender, System.EventArgs e)
		{
			this._canCloseBox = true;

			this._dialogResult = DialogResult.Cancel;

			this.Close();
		}

		private void btnOpenDialog_Click( object sender, EventArgs e )
		{
			OpenFileDialog dialog = new OpenFileDialog();

			dialog.RestoreDirectory = true;
			dialog.InitialDirectory = this._initialPath;
			dialog.DefaultExt = this._defaultExt;
			dialog.Filter = this._filter;
			dialog.FilterIndex = 0;

			DialogResult result = dialog.ShowDialog( this );

			if ( result == DialogResult.OK )
			{
				this.txtInput.Text = System.IO.Path.GetFileName( dialog.FileName );

				this._initialPath = System.IO.Path.GetDirectoryName( dialog.FileName );
			}

			dialog.Dispose();
			dialog = null;
		}

		private void OpenFileBox_FormClosing( object sender, FormClosingEventArgs e )
		{
			if ( !this._canCloseBox )
				e.Cancel = true;
		}
	}
}

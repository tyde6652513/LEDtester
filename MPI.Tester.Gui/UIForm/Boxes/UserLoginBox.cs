using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace MPI.Tester.Gui
{
	/// <summary>
	/// UserLoginBox class.
	/// </summary>
	public class UserLoginBox : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button btnCancel_I18N;
		private System.Windows.Forms.Button btnLogin_I18N;
		private System.Windows.Forms.Label labUserName_I18N;
		private System.Windows.Forms.TextBox txtUserName;
		private System.Windows.Forms.TextBox txtPassword;
		private System.Windows.Forms.Label labPassword_I18N;
		private System.Windows.Forms.Label labSystemVersion;
		private Label labUIVersion;
		private Label labUIVersionLabel;
		private Label labSytemVersionLabel;
		private Label labLoaderVersionLabel;
		private Label labLoaderVersion;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public UserLoginBox( bool isCanCancel, Version versionSys, Version versionUI, Version versionLoader )
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			this.DialogResult = DialogResult.None;

			if ( !isCanCancel )
				this.btnCancel_I18N.Enabled = false;

			// version
			this.labUIVersion.Text = String.Format( "{0}.{1}.{2}", versionUI.Major, versionUI.Minor, versionUI.Revision );
			this.labSystemVersion.Text = String.Format( "{0}.{1}.{2}", versionSys.Major, versionSys.Minor, versionSys.Revision );

			if ( versionLoader == null )
			{
				this.labLoaderVersionLabel.Visible = false;
				this.labLoaderVersion.Visible = false;
			}
			else
			{
				this.labLoaderVersion.Text = String.Format( "{0}.{1}.{2}", versionLoader.Major, versionLoader.Minor, versionLoader.Revision );
			}

			// internationalization
			//Host.ProcessMultilingualAgent( this );
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager( typeof( UserLoginBox ) );
			this.txtPassword = new System.Windows.Forms.TextBox();
			this.labPassword_I18N = new System.Windows.Forms.Label();
			this.txtUserName = new System.Windows.Forms.TextBox();
			this.btnCancel_I18N = new System.Windows.Forms.Button();
			this.btnLogin_I18N = new System.Windows.Forms.Button();
			this.labUserName_I18N = new System.Windows.Forms.Label();
			this.labSystemVersion = new System.Windows.Forms.Label();
			this.labUIVersion = new System.Windows.Forms.Label();
			this.labUIVersionLabel = new System.Windows.Forms.Label();
			this.labSytemVersionLabel = new System.Windows.Forms.Label();
			this.labLoaderVersionLabel = new System.Windows.Forms.Label();
			this.labLoaderVersion = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// txtPassword
			// 
			this.txtPassword.Font = new System.Drawing.Font( "Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( ( byte ) ( 0 ) ) );
			this.txtPassword.Location = new System.Drawing.Point( 706, 136 );
			this.txtPassword.Name = "txtPassword";
			this.txtPassword.PasswordChar = 'x';
			this.txtPassword.Size = new System.Drawing.Size( 234, 22 );
			this.txtPassword.TabIndex = 1;
			this.txtPassword.KeyDown += new System.Windows.Forms.KeyEventHandler( this.txtPassword_KeyDown );
			// 
			// labPassword_I18N
			// 
			this.labPassword_I18N.BackColor = System.Drawing.Color.LightSteelBlue;
			this.labPassword_I18N.Font = new System.Drawing.Font( "Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( ( byte ) ( 0 ) ) );
			this.labPassword_I18N.Location = new System.Drawing.Point( 624, 136 );
			this.labPassword_I18N.Name = "labPassword_I18N";
			this.labPassword_I18N.Size = new System.Drawing.Size( 76, 22 );
			this.labPassword_I18N.TabIndex = 79;
			this.labPassword_I18N.Text = "Password";
			this.labPassword_I18N.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// txtUserName
			// 
			this.txtUserName.Font = new System.Drawing.Font( "Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( ( byte ) ( 0 ) ) );
			this.txtUserName.Location = new System.Drawing.Point( 706, 111 );
			this.txtUserName.Name = "txtUserName";
			this.txtUserName.Size = new System.Drawing.Size( 234, 22 );
			this.txtUserName.TabIndex = 0;
			this.txtUserName.KeyDown += new System.Windows.Forms.KeyEventHandler( this.txtUserName_KeyDown );
			// 
			// btnCancel_I18N
			// 
			this.btnCancel_I18N.BackColor = System.Drawing.Color.CornflowerBlue;
			this.btnCancel_I18N.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel_I18N.Font = new System.Drawing.Font( "Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( ( byte ) ( 0 ) ) );
			this.btnCancel_I18N.ForeColor = System.Drawing.Color.Black;
			//this.btnCancel_I18N.Image = global::MPI.Tester.Gui.Properties.Resources._16_cancel;
			this.btnCancel_I18N.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.btnCancel_I18N.Location = new System.Drawing.Point( 833, 176 );
			this.btnCancel_I18N.Name = "btnCancel_I18N";
			this.btnCancel_I18N.Padding = new System.Windows.Forms.Padding( 10, 0, 5, 0 );
			this.btnCancel_I18N.Size = new System.Drawing.Size( 110, 44 );
			this.btnCancel_I18N.TabIndex = 77;
			this.btnCancel_I18N.Text = "Cancel";
			this.btnCancel_I18N.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.btnCancel_I18N.UseVisualStyleBackColor = false;
			this.btnCancel_I18N.Click += new System.EventHandler( this.btnCancel_I18N_Click );
			// 
			// btnLogin_I18N
			// 
			this.btnLogin_I18N.BackColor = System.Drawing.Color.CornflowerBlue;
			this.btnLogin_I18N.Font = new System.Drawing.Font( "Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( ( byte ) ( 0 ) ) );
			this.btnLogin_I18N.ForeColor = System.Drawing.Color.Black;
			//this.btnLogin_I18N.Image = global::MPI.Tester.Gui.Properties.Resources._32_identity;
			this.btnLogin_I18N.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.btnLogin_I18N.Location = new System.Drawing.Point( 706, 175 );
			this.btnLogin_I18N.Name = "btnLogin_I18N";
			this.btnLogin_I18N.Padding = new System.Windows.Forms.Padding( 10, 0, 5, 0 );
			this.btnLogin_I18N.Size = new System.Drawing.Size( 110, 45 );
			this.btnLogin_I18N.TabIndex = 76;
			this.btnLogin_I18N.Text = "Login";
			this.btnLogin_I18N.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.btnLogin_I18N.UseVisualStyleBackColor = false;
			this.btnLogin_I18N.Click += new System.EventHandler( this.btnLogin_I18N_Click );
			// 
			// labUserName_I18N
			// 
			this.labUserName_I18N.BackColor = System.Drawing.Color.LightSteelBlue;
			this.labUserName_I18N.Font = new System.Drawing.Font( "Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( ( byte ) ( 0 ) ) );
			this.labUserName_I18N.Location = new System.Drawing.Point( 624, 111 );
			this.labUserName_I18N.Name = "labUserName_I18N";
			this.labUserName_I18N.Size = new System.Drawing.Size( 76, 22 );
			this.labUserName_I18N.TabIndex = 72;
			this.labUserName_I18N.Text = "Name";
			this.labUserName_I18N.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// labSystemVersion
			// 
			this.labSystemVersion.BackColor = System.Drawing.Color.Gainsboro;
			this.labSystemVersion.Font = new System.Drawing.Font( "Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ( ( byte ) ( 0 ) ) );
			this.labSystemVersion.Location = new System.Drawing.Point( 864, 88 );
			this.labSystemVersion.Name = "labSystemVersion";
			this.labSystemVersion.Size = new System.Drawing.Size( 76, 18 );
			this.labSystemVersion.TabIndex = 80;
			this.labSystemVersion.Text = "0.00.00";
			this.labSystemVersion.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// labUIVersion
			// 
			this.labUIVersion.BackColor = System.Drawing.Color.Gainsboro;
			this.labUIVersion.Font = new System.Drawing.Font( "Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ( ( byte ) ( 0 ) ) );
			this.labUIVersion.Location = new System.Drawing.Point( 864, 69 );
			this.labUIVersion.Name = "labUIVersion";
			this.labUIVersion.Size = new System.Drawing.Size( 76, 18 );
			this.labUIVersion.TabIndex = 81;
			this.labUIVersion.Text = "0.00.00";
			this.labUIVersion.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// labUIVersionLabel
			// 
			this.labUIVersionLabel.BackColor = System.Drawing.Color.LightSteelBlue;
			this.labUIVersionLabel.Font = new System.Drawing.Font( "Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ( ( byte ) ( 0 ) ) );
			this.labUIVersionLabel.Location = new System.Drawing.Point( 808, 69 );
			this.labUIVersionLabel.Name = "labUIVersionLabel";
			this.labUIVersionLabel.Size = new System.Drawing.Size( 55, 18 );
			this.labUIVersionLabel.TabIndex = 83;
			this.labUIVersionLabel.Text = "UI";
			this.labUIVersionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// labSytemVersionLabel
			// 
			this.labSytemVersionLabel.BackColor = System.Drawing.Color.LightSteelBlue;
			this.labSytemVersionLabel.Font = new System.Drawing.Font( "Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ( ( byte ) ( 0 ) ) );
			this.labSytemVersionLabel.Location = new System.Drawing.Point( 808, 88 );
			this.labSytemVersionLabel.Name = "labSytemVersionLabel";
			this.labSytemVersionLabel.Size = new System.Drawing.Size( 55, 18 );
			this.labSytemVersionLabel.TabIndex = 82;
			this.labSytemVersionLabel.Text = "System";
			this.labSytemVersionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// labLoaderVersionLabel
			// 
			this.labLoaderVersionLabel.BackColor = System.Drawing.Color.LightSteelBlue;
			this.labLoaderVersionLabel.Font = new System.Drawing.Font( "Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ( ( byte ) ( 0 ) ) );
			this.labLoaderVersionLabel.Location = new System.Drawing.Point( 808, 41 );
			this.labLoaderVersionLabel.Name = "labLoaderVersionLabel";
			this.labLoaderVersionLabel.Size = new System.Drawing.Size( 55, 18 );
			this.labLoaderVersionLabel.TabIndex = 85;
			this.labLoaderVersionLabel.Text = "Loader";
			this.labLoaderVersionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// labLoaderVersion
			// 
			this.labLoaderVersion.BackColor = System.Drawing.Color.Gainsboro;
			this.labLoaderVersion.Font = new System.Drawing.Font( "Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ( ( byte ) ( 0 ) ) );
			this.labLoaderVersion.Location = new System.Drawing.Point( 864, 41 );
			this.labLoaderVersion.Name = "labLoaderVersion";
			this.labLoaderVersion.Size = new System.Drawing.Size( 76, 18 );
			this.labLoaderVersion.TabIndex = 84;
			this.labLoaderVersion.Text = "0.00.00";
			this.labLoaderVersion.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// UserLoginBox
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
//			this.BackgroundImage = ( ( System.Drawing.Image ) ( resources.GetObject( "$this.BackgroundImage" ) ) );
			this.ClientSize = new System.Drawing.Size( 954, 240 );
			this.ControlBox = false;
			this.Controls.Add( this.labLoaderVersionLabel );
			this.Controls.Add( this.labLoaderVersion );
			this.Controls.Add( this.labUIVersionLabel );
			this.Controls.Add( this.labSytemVersionLabel );
			this.Controls.Add( this.labUIVersion );
			this.Controls.Add( this.labSystemVersion );
			this.Controls.Add( this.labPassword_I18N );
			this.Controls.Add( this.txtUserName );
			this.Controls.Add( this.txtPassword );
			this.Controls.Add( this.btnCancel_I18N );
			this.Controls.Add( this.btnLogin_I18N );
			this.Controls.Add( this.labUserName_I18N );
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.KeyPreview = true;
			this.Name = "UserLoginBox";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.KeyDown += new System.Windows.Forms.KeyEventHandler( this.UserLoginBox_KeyDown );
			this.ResumeLayout( false );
			this.PerformLayout();

		}
		#endregion

		private void btnLogin_I18N_Click( object sender, System.EventArgs e )
		{
			// check user name
			int index = AuthorityCenter.UserCenter.UserTable.IndexOf( this.txtUserName.Text );

			if ( index >= 0 )
			{
				// check password
				if ( AuthorityCenter.UserCenter.UserTable[index].Password.CompareTo( this.txtPassword.Text ) == 0 )
				{
					AuthorityCenter.UserCenter.Login( AuthorityCenter.UserCenter.UserTable[index] );

					// log
					//Logs.LogServers.PutLog( MPI.MappingSorter.Logs.ELogMessage.UserLogin );

					this.DialogResult = DialogResult.OK;
					this.Hide();
				}
				else
				{
					//UIBoxes.WarningBox_I18N( EWarningMsg.IllegalPassword );

					this.txtPassword.Focus();

					this.txtPassword.SelectAll();
				}
			}
			else
			{
				//UIBoxes.WarningBox_I18N( EWarningMsg.IllegalUserName );

				this.txtUserName.Focus();

				this.txtUserName.SelectAll();
			}
		}

		private void btnCancel_I18N_Click( object sender, System.EventArgs e )
		{
			this.DialogResult = DialogResult.OK;
			// 暫時先開放
			//Program._frmMain.SystemsecurityLevel( MPI.AuthorityControl.EAuthorityLevel.Designer );
			this.Hide();
		}

		private void txtUserName_KeyDown( object sender, System.Windows.Forms.KeyEventArgs e )
		{
			if ( e.KeyCode == Keys.Return )
			{
				this.txtPassword.Focus();
				this.txtPassword.SelectAll();
			}
		}

		private void txtPassword_KeyDown( object sender, System.Windows.Forms.KeyEventArgs e )
		{
			if ( e.KeyCode == Keys.Return )
			{
				this.btnLogin_I18N_Click( null, null );
			}
		}

		private void UserLoginBox_KeyDown( object sender, System.Windows.Forms.KeyEventArgs e )
		{
			switch ( e.KeyCode )
			{
				case Keys.Escape:
					if ( this.btnCancel_I18N.Enabled )
						this.Hide();
					break;
			}
		}
	}
}

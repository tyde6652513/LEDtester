using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace MPI.Tester.Gui
{
	/// <summary>
	/// Summary description for AlertCustom.
	/// </summary>
	public class AlertCustom : DevComponents.DotNetBar.Balloon
	{
		private DevComponents.DotNetBar.Controls.ReflectionImage reflectionImage1;
        private DevComponents.DotNetBar.Bar bar1;
        private DevComponents.DotNetBar.LabelX lblTitle;
        private DevComponents.DotNetBar.ButtonX btnConfirm;
        private DevComponents.DotNetBar.ButtonX btnCancel;
        private DevComponents.DotNetBar.Controls.TextBoxX txtMessage;
        private DialogResult _result;
        private Label lblErrorHandler;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public AlertCustom()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AlertCustom));
            this.reflectionImage1 = new DevComponents.DotNetBar.Controls.ReflectionImage();
            this.bar1 = new DevComponents.DotNetBar.Bar();
            this.lblTitle = new DevComponents.DotNetBar.LabelX();
            this.btnConfirm = new DevComponents.DotNetBar.ButtonX();
            this.btnCancel = new DevComponents.DotNetBar.ButtonX();
            this.txtMessage = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.lblErrorHandler = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.bar1)).BeginInit();
            this.SuspendLayout();
            // 
            // reflectionImage1
            // 
            this.reflectionImage1.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.reflectionImage1.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10001;
            this.reflectionImage1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.reflectionImage1.Image = ((System.Drawing.Image)(resources.GetObject("reflectionImage1.Image")));
            this.reflectionImage1.Location = new System.Drawing.Point(14, 85);
            this.reflectionImage1.Name = "reflectionImage1";
            this.reflectionImage1.Size = new System.Drawing.Size(77, 65);
            this.reflectionImage1.TabIndex = 0;
            // 
            // bar1
            // 
            this.bar1.BackColor = System.Drawing.Color.Transparent;
            this.bar1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.bar1.Location = new System.Drawing.Point(0, 272);
            this.bar1.Name = "bar1";
            this.bar1.Size = new System.Drawing.Size(539, 25);
            this.bar1.Stretch = true;
            this.bar1.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.bar1.TabIndex = 1;
            this.bar1.TabStop = false;
            this.bar1.Text = "bar1";
            // 
            // lblTitle
            // 
            this.lblTitle.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.lblTitle.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.lblTitle.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblTitle.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.ForeColor = System.Drawing.Color.RoyalBlue;
            this.lblTitle.Location = new System.Drawing.Point(95, 43);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(184, 18);
            this.lblTitle.TabIndex = 2;
            this.lblTitle.Text = "Hello World!";
            // 
            // btnConfirm
            // 
            this.btnConfirm.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnConfirm.AntiAlias = true;
            this.btnConfirm.BackColor = System.Drawing.Color.Transparent;
            this.btnConfirm.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnConfirm.EnableMarkup = false;
            this.btnConfirm.FocusCuesEnabled = false;
            this.btnConfirm.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.btnConfirm.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnConfirm.Image = global::MPI.Tester.Gui.Properties.Resources.btnDataOK;
            this.btnConfirm.ImageFixedSize = new System.Drawing.Size(20, 20);
            this.btnConfirm.Location = new System.Drawing.Point(237, 249);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Shape = new DevComponents.DotNetBar.RoundRectangleShapeDescriptor(8);
            this.btnConfirm.Size = new System.Drawing.Size(123, 39);
            this.btnConfirm.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnConfirm.TabIndex = 5;
            this.btnConfirm.Text = "OK ";
            this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnCancel.AntiAlias = true;
            this.btnCancel.BackColor = System.Drawing.Color.Transparent;
            this.btnCancel.EnableMarkup = false;
            this.btnCancel.FocusCuesEnabled = false;
            this.btnCancel.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.btnCancel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnCancel.Image = global::MPI.Tester.Gui.Properties.Resources.btnDataCancel;
            this.btnCancel.ImageFixedSize = new System.Drawing.Size(20, 20);
            this.btnCancel.Location = new System.Drawing.Point(376, 249);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Shape = new DevComponents.DotNetBar.RoundRectangleShapeDescriptor(8);
            this.btnCancel.Size = new System.Drawing.Size(120, 39);
            this.btnCancel.Style = DevComponents.DotNetBar.eDotNetBarStyle.Windows7;
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // txtMessage
            // 
            // 
            // 
            // 
            this.txtMessage.Border.Class = "TextBoxBorder";
            this.txtMessage.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtMessage.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMessage.Location = new System.Drawing.Point(97, 70);
            this.txtMessage.Multiline = true;
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.Size = new System.Drawing.Size(407, 93);
            this.txtMessage.TabIndex = 6;
            // 
            // lblErrorHandler
            // 
            this.lblErrorHandler.BackColor = System.Drawing.Color.Transparent;
            this.lblErrorHandler.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblErrorHandler.ForeColor = System.Drawing.Color.Black;
            this.lblErrorHandler.Location = new System.Drawing.Point(95, 166);
            this.lblErrorHandler.Name = "lblErrorHandler";
            this.lblErrorHandler.Size = new System.Drawing.Size(409, 69);
            this.lblErrorHandler.TabIndex = 7;
            this.lblErrorHandler.Text = "label1";
            // 
            // AlertCustom
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 15);
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(207)))), ((int)(((byte)(221)))), ((int)(((byte)(244)))));
            this.BackColor2 = System.Drawing.Color.White;
            this.CaptionFont = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.ClientSize = new System.Drawing.Size(539, 297);
            this.Controls.Add(this.lblErrorHandler);
            this.Controls.Add(this.txtMessage);
            this.Controls.Add(this.btnConfirm);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.bar1);
            this.Controls.Add(this.reflectionImage1);
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(114)))), ((int)(((byte)(196)))));
            this.Location = new System.Drawing.Point(0, 0);
            this.Name = "AlertCustom";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Style = DevComponents.DotNetBar.eBallonStyle.Alert;
            this.Load += new System.EventHandler(this.AlertCustom_Load);
            ((System.ComponentModel.ISupportInitialize)(this.bar1)).EndInit();
            this.ResumeLayout(false);

		}
		#endregion


		public void ShowAt(int x, int y)
		{
			Rectangle r = Screen.GetWorkingArea( this );
			this.Location = new Point( r.Right - this.Width, r.Bottom - this.Height );
			this.AutoClose = true;
			this.AutoCloseTimeOut = 15;
			this.AlertAnimation = DevComponents.DotNetBar.eAlertAnimation.BottomToTop;
			this.AlertAnimationDuration = 300;
			this.Show( false );
		}

		public string Title
		{
			set
			{
				lblTitle.Text = value;
			}
		}

		public string Message
		{
			set
			{
				txtMessage.Text = value;
			}
		}

        public string ErrorHandler
        {
            set
            {
                this.lblErrorHandler.Text = value;
            }
        }

        public DialogResult Result
        {
            get
            {
               return  this._result;
            }
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            this._result = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this._result = DialogResult.Cancel;
            this.Close();
        }

        private void AlertCustom_Load(object sender, EventArgs e)
        {
            this.TopMost = true;
        }
	}
}

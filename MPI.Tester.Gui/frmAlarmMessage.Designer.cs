namespace MPI.Tester.Gui
{
    partial class frmAlarmMessage
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAlarmMessage));
            this.reflectionImage1 = new DevComponents.DotNetBar.Controls.ReflectionImage();
            this.lblErrorHandler = new System.Windows.Forms.Label();
            this.txtMessage = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.btnConfirm = new DevComponents.DotNetBar.ButtonX();
            this.btnCancel = new DevComponents.DotNetBar.ButtonX();
            this.lblTitle = new DevComponents.DotNetBar.LabelX();
            this.SuspendLayout();
            // 
            // reflectionImage1
            // 
            this.reflectionImage1.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.reflectionImage1.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10026;
            this.reflectionImage1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.reflectionImage1.Image = ((System.Drawing.Image)(resources.GetObject("reflectionImage1.Image")));
            this.reflectionImage1.Location = new System.Drawing.Point(12, 12);
            this.reflectionImage1.Name = "reflectionImage1";
            this.reflectionImage1.Size = new System.Drawing.Size(72, 102);
            this.reflectionImage1.TabIndex = 1;
            // 
            // lblErrorHandler
            // 
            this.lblErrorHandler.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lblErrorHandler.ForeColor = System.Drawing.Color.Black;
            this.lblErrorHandler.Location = new System.Drawing.Point(103, 159);
            this.lblErrorHandler.Name = "lblErrorHandler";
            this.lblErrorHandler.Size = new System.Drawing.Size(409, 33);
            this.lblErrorHandler.TabIndex = 12;
            this.lblErrorHandler.Text = "label1";
            // 
            // txtMessage
            // 
            // 
            // 
            // 
            this.txtMessage.Border.Class = "TextBoxBorder";
            this.txtMessage.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMessage.Location = new System.Drawing.Point(105, 62);
            this.txtMessage.Multiline = true;
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.Size = new System.Drawing.Size(407, 94);
            this.txtMessage.TabIndex = 11;
            // 
            // btnConfirm
            // 
            this.btnConfirm.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnConfirm.AntiAlias = true;
            this.btnConfirm.BackColor = System.Drawing.Color.Transparent;
            this.btnConfirm.EnableMarkup = false;
            this.btnConfirm.FocusCuesEnabled = false;
            this.btnConfirm.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.btnConfirm.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnConfirm.Image = global::MPI.Tester.Gui.Properties.Resources.btnDataOK;
            this.btnConfirm.ImageFixedSize = new System.Drawing.Size(20, 20);
            this.btnConfirm.Location = new System.Drawing.Point(248, 195);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Shape = new DevComponents.DotNetBar.RoundRectangleShapeDescriptor(8);
            this.btnConfirm.Size = new System.Drawing.Size(123, 40);
            this.btnConfirm.TabIndex = 10;
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
            this.btnCancel.Location = new System.Drawing.Point(387, 195);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Shape = new DevComponents.DotNetBar.RoundRectangleShapeDescriptor(8);
            this.btnCancel.Size = new System.Drawing.Size(120, 40);
            this.btnCancel.Style = DevComponents.DotNetBar.eDotNetBarStyle.Windows7;
            this.btnCancel.TabIndex = 9;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblTitle
            // 
            this.lblTitle.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.lblTitle.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10026;
            this.lblTitle.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblTitle.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.ForeColor = System.Drawing.Color.RoyalBlue;
            this.lblTitle.Location = new System.Drawing.Point(105, 35);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(407, 18);
            this.lblTitle.TabIndex = 8;
            this.lblTitle.Text = "Hello World!";
            // 
            // frmAlarmMessage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(559, 250);
            this.Controls.Add(this.lblErrorHandler);
            this.Controls.Add(this.txtMessage);
            this.Controls.Add(this.btnConfirm);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.reflectionImage1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmAlarmMessage";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = " Message";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmAlarmMessage_Closd);
            this.Load += new System.EventHandler(this.frmAlarmMessage_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.Controls.ReflectionImage reflectionImage1;
        private System.Windows.Forms.Label lblErrorHandler;
        private DevComponents.DotNetBar.Controls.TextBoxX txtMessage;
        private DevComponents.DotNetBar.ButtonX btnConfirm;
        private DevComponents.DotNetBar.ButtonX btnCancel;
        private DevComponents.DotNetBar.LabelX lblTitle;
    }
}
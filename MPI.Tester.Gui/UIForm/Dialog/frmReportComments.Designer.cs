namespace MPI.Tester.Gui
{
    partial class frmReportComments
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
            this.btnConfirm = new DevComponents.DotNetBar.ButtonX();
            this.btnCancel = new DevComponents.DotNetBar.ButtonX();
            this.grpTaskName = new System.Windows.Forms.GroupBox();
            this.txtReportComments = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.grpTaskName.SuspendLayout();
            this.SuspendLayout();
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
            this.btnConfirm.Location = new System.Drawing.Point(364, 113);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Shape = new DevComponents.DotNetBar.RoundRectangleShapeDescriptor(3);
            this.btnConfirm.Size = new System.Drawing.Size(123, 52);
            this.btnConfirm.Style = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
            this.btnConfirm.TabIndex = 169;
            this.btnConfirm.Text = "Confirm";
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
            this.btnCancel.Location = new System.Drawing.Point(493, 113);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Shape = new DevComponents.DotNetBar.RoundRectangleShapeDescriptor(3);
            this.btnCancel.Size = new System.Drawing.Size(120, 52);
            this.btnCancel.Style = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
            this.btnCancel.TabIndex = 170;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // grpTaskName
            // 
            this.grpTaskName.Controls.Add(this.txtReportComments);
            this.grpTaskName.Font = new System.Drawing.Font("Arial", 20.25F, System.Drawing.FontStyle.Bold);
            this.grpTaskName.ForeColor = System.Drawing.Color.DimGray;
            this.grpTaskName.Location = new System.Drawing.Point(12, 14);
            this.grpTaskName.Name = "grpTaskName";
            this.grpTaskName.Size = new System.Drawing.Size(602, 92);
            this.grpTaskName.TabIndex = 171;
            this.grpTaskName.TabStop = false;
            this.grpTaskName.Text = "Report Comments";
            // 
            // txtReportComments
            // 
            this.txtReportComments.BackColor = System.Drawing.SystemColors.Window;
            // 
            // 
            // 
            this.txtReportComments.Border.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.txtReportComments.Border.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.txtReportComments.Border.BorderBottomWidth = 1;
            this.txtReportComments.Border.BorderColor = System.Drawing.Color.DarkSalmon;
            this.txtReportComments.Border.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.txtReportComments.Border.BorderLeftWidth = 1;
            this.txtReportComments.Border.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.txtReportComments.Border.BorderRightWidth = 1;
            this.txtReportComments.Border.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.txtReportComments.Border.BorderTopWidth = 1;
            this.txtReportComments.Border.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10001;
            this.txtReportComments.Border.CornerDiameter = 4;
            this.txtReportComments.Border.CornerType = DevComponents.DotNetBar.eCornerType.Rounded;
            this.txtReportComments.Font = new System.Drawing.Font("Arial", 15.75F);
            this.txtReportComments.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtReportComments.Location = new System.Drawing.Point(6, 46);
            this.txtReportComments.Name = "txtReportComments";
            this.txtReportComments.Size = new System.Drawing.Size(590, 31);
            this.txtReportComments.TabIndex = 161;
            this.txtReportComments.Tag = "8";
            // 
            // frmReportComments
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackgroundImage = global::MPI.Tester.Gui.Properties.Resources.mini_hero_bg;
            this.ClientSize = new System.Drawing.Size(620, 171);
            this.ControlBox = false;
            this.Controls.Add(this.grpTaskName);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnConfirm);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "frmReportComments";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Report Comments";
            this.Load += new System.EventHandler(this.frmReportComments_Load);
            this.grpTaskName.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.ButtonX btnConfirm;
        private DevComponents.DotNetBar.ButtonX btnCancel;
        private System.Windows.Forms.GroupBox grpTaskName;
        private DevComponents.DotNetBar.Controls.TextBoxX txtReportComments;
    }
}
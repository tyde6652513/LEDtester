namespace MPI.Tester.Gui
{
    partial class frmSetTaskSheet
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
            this.grpTaskName = new System.Windows.Forms.GroupBox();
            this.lblTaskSheetFileTitle = new DevComponents.DotNetBar.LabelX();
            this.txtNewProductFileName = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.panel1 = new System.Windows.Forms.Panel();
            this.cmbImportBinFileNames = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.comboItem7 = new DevComponents.Editors.ComboItem();
            this.comboItem8 = new DevComponents.Editors.ComboItem();
            this.comboItem9 = new DevComponents.Editors.ComboItem();
            this.labelX29 = new DevComponents.DotNetBar.LabelX();
            this.plCMFormat = new System.Windows.Forms.Panel();
            this.cmbImportCalibrateFileNames = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.comboItem10 = new DevComponents.Editors.ComboItem();
            this.comboItem11 = new DevComponents.Editors.ComboItem();
            this.comboItem12 = new DevComponents.Editors.ComboItem();
            this.labelX28 = new DevComponents.DotNetBar.LabelX();
            this.btnConfirm = new DevComponents.DotNetBar.ButtonX();
            this.btnCancel = new DevComponents.DotNetBar.ButtonX();
            this.grpTaskName.SuspendLayout();
            this.panel1.SuspendLayout();
            this.plCMFormat.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpTaskName
            // 
            this.grpTaskName.Controls.Add(this.lblTaskSheetFileTitle);
            this.grpTaskName.Controls.Add(this.txtNewProductFileName);
            this.grpTaskName.Font = new System.Drawing.Font("Arial", 20.25F, System.Drawing.FontStyle.Bold);
            this.grpTaskName.ForeColor = System.Drawing.Color.DimGray;
            this.grpTaskName.Location = new System.Drawing.Point(23, 12);
            this.grpTaskName.Name = "grpTaskName";
            this.grpTaskName.Size = new System.Drawing.Size(599, 85);
            this.grpTaskName.TabIndex = 163;
            this.grpTaskName.TabStop = false;
            this.grpTaskName.Text = "ABC";
            // 
            // lblTaskSheetFileTitle
            // 
            this.lblTaskSheetFileTitle.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.lblTaskSheetFileTitle.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10001;
            this.lblTaskSheetFileTitle.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblTaskSheetFileTitle.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold);
            this.lblTaskSheetFileTitle.ForeColor = System.Drawing.Color.MidnightBlue;
            this.lblTaskSheetFileTitle.Location = new System.Drawing.Point(69, 40);
            this.lblTaskSheetFileTitle.Name = "lblTaskSheetFileTitle";
            this.lblTaskSheetFileTitle.Size = new System.Drawing.Size(150, 32);
            this.lblTaskSheetFileTitle.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
            this.lblTaskSheetFileTitle.TabIndex = 160;
            this.lblTaskSheetFileTitle.Text = "Task Name ";
            // 
            // txtNewProductFileName
            // 
            this.txtNewProductFileName.BackColor = System.Drawing.SystemColors.Window;
            // 
            // 
            // 
            this.txtNewProductFileName.Border.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.txtNewProductFileName.Border.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.txtNewProductFileName.Border.BorderBottomWidth = 1;
            this.txtNewProductFileName.Border.BorderColor = System.Drawing.Color.DarkSalmon;
            this.txtNewProductFileName.Border.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.txtNewProductFileName.Border.BorderLeftWidth = 1;
            this.txtNewProductFileName.Border.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.txtNewProductFileName.Border.BorderRightWidth = 1;
            this.txtNewProductFileName.Border.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.txtNewProductFileName.Border.BorderTopWidth = 1;
            this.txtNewProductFileName.Border.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10001;
            this.txtNewProductFileName.Border.CornerDiameter = 4;
            this.txtNewProductFileName.Border.CornerType = DevComponents.DotNetBar.eCornerType.Rounded;
            this.txtNewProductFileName.Font = new System.Drawing.Font("Arial", 15.75F);
            this.txtNewProductFileName.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtNewProductFileName.Location = new System.Drawing.Point(225, 42);
            this.txtNewProductFileName.Name = "txtNewProductFileName";
            this.txtNewProductFileName.Size = new System.Drawing.Size(351, 31);
            this.txtNewProductFileName.TabIndex = 161;
            this.txtNewProductFileName.Tag = "8";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.cmbImportBinFileNames);
            this.panel1.Controls.Add(this.labelX29);
            this.panel1.Location = new System.Drawing.Point(23, 145);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(593, 37);
            this.panel1.TabIndex = 166;
            this.panel1.Visible = false;
            // 
            // cmbImportBinFileNames
            // 
            this.cmbImportBinFileNames.DisplayMember = "Text";
            this.cmbImportBinFileNames.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbImportBinFileNames.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbImportBinFileNames.Font = new System.Drawing.Font("Arial", 12F);
            this.cmbImportBinFileNames.FormattingEnabled = true;
            this.cmbImportBinFileNames.ItemHeight = 20;
            this.cmbImportBinFileNames.Items.AddRange(new object[] {
            this.comboItem7,
            this.comboItem8,
            this.comboItem9});
            this.cmbImportBinFileNames.Location = new System.Drawing.Point(225, 5);
            this.cmbImportBinFileNames.Name = "cmbImportBinFileNames";
            this.cmbImportBinFileNames.Size = new System.Drawing.Size(352, 26);
            this.cmbImportBinFileNames.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.cmbImportBinFileNames.TabIndex = 156;
            this.cmbImportBinFileNames.WatermarkEnabled = false;
            // 
            // comboItem7
            // 
            this.comboItem7.Text = "aa";
            // 
            // comboItem8
            // 
            this.comboItem8.Text = "bb";
            // 
            // comboItem9
            // 
            this.comboItem9.Text = "cc";
            // 
            // labelX29
            // 
            this.labelX29.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX29.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10001;
            this.labelX29.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX29.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.labelX29.ForeColor = System.Drawing.Color.Black;
            this.labelX29.Location = new System.Drawing.Point(69, 5);
            this.labelX29.Name = "labelX29";
            this.labelX29.Size = new System.Drawing.Size(129, 30);
            this.labelX29.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
            this.labelX29.TabIndex = 159;
            this.labelX29.Text = "Bin Table";
            // 
            // plCMFormat
            // 
            this.plCMFormat.Controls.Add(this.cmbImportCalibrateFileNames);
            this.plCMFormat.Controls.Add(this.labelX28);
            this.plCMFormat.Location = new System.Drawing.Point(23, 103);
            this.plCMFormat.Name = "plCMFormat";
            this.plCMFormat.Size = new System.Drawing.Size(593, 37);
            this.plCMFormat.TabIndex = 165;
            // 
            // cmbImportCalibrateFileNames
            // 
            this.cmbImportCalibrateFileNames.DisplayMember = "Text";
            this.cmbImportCalibrateFileNames.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbImportCalibrateFileNames.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbImportCalibrateFileNames.Font = new System.Drawing.Font("Arial", 12F);
            this.cmbImportCalibrateFileNames.FormattingEnabled = true;
            this.cmbImportCalibrateFileNames.ItemHeight = 20;
            this.cmbImportCalibrateFileNames.Items.AddRange(new object[] {
            this.comboItem10,
            this.comboItem11,
            this.comboItem12});
            this.cmbImportCalibrateFileNames.Location = new System.Drawing.Point(225, 6);
            this.cmbImportCalibrateFileNames.Name = "cmbImportCalibrateFileNames";
            this.cmbImportCalibrateFileNames.Size = new System.Drawing.Size(351, 26);
            this.cmbImportCalibrateFileNames.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.cmbImportCalibrateFileNames.TabIndex = 157;
            this.cmbImportCalibrateFileNames.WatermarkEnabled = false;
            // 
            // comboItem11
            // 
            this.comboItem11.Text = "bb";
            // 
            // comboItem12
            // 
            this.comboItem12.Text = "cc";
            // 
            // labelX28
            // 
            this.labelX28.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX28.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10001;
            this.labelX28.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX28.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.labelX28.ForeColor = System.Drawing.Color.Black;
            this.labelX28.Location = new System.Drawing.Point(69, 6);
            this.labelX28.Name = "labelX28";
            this.labelX28.Size = new System.Drawing.Size(138, 30);
            this.labelX28.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
            this.labelX28.TabIndex = 158;
            this.labelX28.Text = "Gain and Coef";
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
            this.btnConfirm.Location = new System.Drawing.Point(366, 189);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Shape = new DevComponents.DotNetBar.RoundRectangleShapeDescriptor(3);
            this.btnConfirm.Size = new System.Drawing.Size(123, 48);
            this.btnConfirm.Style = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
            this.btnConfirm.TabIndex = 168;
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
            this.btnCancel.Location = new System.Drawing.Point(498, 189);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Shape = new DevComponents.DotNetBar.RoundRectangleShapeDescriptor(3);
            this.btnCancel.Size = new System.Drawing.Size(120, 48);
            this.btnCancel.Style = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
            this.btnCancel.TabIndex = 167;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // frmSetTaskSheet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.BackgroundImage = global::MPI.Tester.Gui.Properties.Resources.mini_hero_bg;
            this.ClientSize = new System.Drawing.Size(638, 245);
            this.Controls.Add(this.btnConfirm);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.plCMFormat);
            this.Controls.Add(this.grpTaskName);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmSetTaskSheet";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = " Set Task Sheet";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.frmSetTaskSheet_Load);
            this.grpTaskName.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.plCMFormat.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.LabelX lblTaskSheetFileTitle;
        private DevComponents.DotNetBar.Controls.TextBoxX txtNewProductFileName;
        private System.Windows.Forms.GroupBox grpTaskName;
        private System.Windows.Forms.Panel panel1;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cmbImportBinFileNames;
        private DevComponents.Editors.ComboItem comboItem7;
        private DevComponents.Editors.ComboItem comboItem8;
        private DevComponents.Editors.ComboItem comboItem9;
        private DevComponents.DotNetBar.LabelX labelX29;
        private System.Windows.Forms.Panel plCMFormat;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cmbImportCalibrateFileNames;
        private DevComponents.Editors.ComboItem comboItem10;
        private DevComponents.Editors.ComboItem comboItem11;
        private DevComponents.Editors.ComboItem comboItem12;
        private DevComponents.DotNetBar.LabelX labelX28;
        private DevComponents.DotNetBar.ButtonX btnConfirm;
        private DevComponents.DotNetBar.ButtonX btnCancel;

    }
}
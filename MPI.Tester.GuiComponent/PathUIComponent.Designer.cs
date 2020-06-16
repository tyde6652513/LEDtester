namespace MPI.Tester.GuiComponent
{
    partial class PathUIComponent
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblPathName = new System.Windows.Forms.Label();
            this.button1 = new DevComponents.DotNetBar.ButtonX();
            this.cmbFolder = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.txtPath = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.chkEnable = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.SuspendLayout();
            // 
            // lblPathName
            // 
            this.lblPathName.AutoSize = true;
            this.lblPathName.BackColor = System.Drawing.Color.MistyRose;
            this.lblPathName.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPathName.Location = new System.Drawing.Point(-3, 3);
            this.lblPathName.Name = "lblPathName";
            this.lblPathName.Size = new System.Drawing.Size(160, 18);
            this.lblPathName.TabIndex = 1;
            this.lblPathName.Text = "Path000000000000000";
            // 
            // button1
            // 
            this.button1.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.button1.ColorTable = DevComponents.DotNetBar.eButtonColor.Office2007WithBackground;
            this.button1.Location = new System.Drawing.Point(531, 1);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(26, 23);
            this.button1.TabIndex = 5;
            this.button1.Text = "...";
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // cmbFolder
            // 
            this.cmbFolder.DisplayMember = "Text";
            this.cmbFolder.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbFolder.FormattingEnabled = true;
            this.cmbFolder.ItemHeight = 19;
            this.cmbFolder.Location = new System.Drawing.Point(563, 0);
            this.cmbFolder.Name = "cmbFolder";
            this.cmbFolder.Size = new System.Drawing.Size(137, 25);
            this.cmbFolder.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.cmbFolder.TabIndex = 6;
            // 
            // txtPath
            // 
            this.txtPath.BackColor = System.Drawing.SystemColors.Window;
            // 
            // 
            // 
            this.txtPath.Border.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.txtPath.Border.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.txtPath.Border.BorderBottomWidth = 1;
            this.txtPath.Border.BorderColor = System.Drawing.Color.DarkSalmon;
            this.txtPath.Border.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.txtPath.Border.BorderLeftWidth = 1;
            this.txtPath.Border.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.txtPath.Border.BorderRightWidth = 1;
            this.txtPath.Border.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.txtPath.Border.BorderTopWidth = 1;
            this.txtPath.Border.Class = "ItemPanel";
            this.txtPath.Border.CornerDiameter = 2;
            this.txtPath.Border.CornerType = DevComponents.DotNetBar.eCornerType.Rounded;
            this.txtPath.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold);
            this.txtPath.Location = new System.Drawing.Point(206, 0);
            this.txtPath.Name = "txtPath";
            this.txtPath.Size = new System.Drawing.Size(319, 24);
            this.txtPath.TabIndex = 200;
            this.txtPath.Tag = "8";
            // 
            // chkEnable
            // 
            // 
            // 
            // 
            this.chkEnable.BackgroundStyle.Class = "TextBoxBorder";
            this.chkEnable.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.chkEnable.Checked = true;
            this.chkEnable.CheckSignSize = new System.Drawing.Size(20, 20);
            this.chkEnable.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkEnable.CheckValue = "Y";
            this.chkEnable.Location = new System.Drawing.Point(168, 1);
            this.chkEnable.Name = "chkEnable";
            this.chkEnable.Size = new System.Drawing.Size(32, 23);
            this.chkEnable.Style = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
            this.chkEnable.TabIndex = 8;
            // 
            // PathUIComponent
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.chkEnable);
            this.Controls.Add(this.txtPath);
            this.Controls.Add(this.cmbFolder);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.lblPathName);
            this.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "PathUIComponent";
            this.Size = new System.Drawing.Size(700, 24);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblPathName;
        private DevComponents.DotNetBar.ButtonX button1;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cmbFolder;
        private DevComponents.DotNetBar.Controls.TextBoxX txtPath;
        private DevComponents.DotNetBar.Controls.CheckBoxX chkEnable;
    }
}

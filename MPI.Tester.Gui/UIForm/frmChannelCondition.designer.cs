namespace MPI.Tester.Gui
{
    partial class frmChannelCondition
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmChannelCondition));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.pnlDutChDisplay = new System.Windows.Forms.Panel();
            this.dgvChannelCondiSetting = new DevComponents.DotNetBar.Controls.DataGridViewX();
            this.lblRecipeCondiTableInfo = new System.Windows.Forms.Label();
            this.btnConfirm = new DevComponents.DotNetBar.ButtonX();
            this.chkSamplingOpticalTest = new DevComponents.DotNetBar.Controls.CheckBoxX();
            ((System.ComponentModel.ISupportInitialize)(this.dgvChannelCondiSetting)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlDutChDisplay
            // 
            resources.ApplyResources(this.pnlDutChDisplay, "pnlDutChDisplay");
            this.pnlDutChDisplay.BackColor = System.Drawing.Color.Transparent;
            this.pnlDutChDisplay.Name = "pnlDutChDisplay";
            // 
            // dgvChannelCondiSetting
            // 
            this.dgvChannelCondiSetting.AllowUserToAddRows = false;
            this.dgvChannelCondiSetting.AllowUserToDeleteRows = false;
            this.dgvChannelCondiSetting.AllowUserToResizeColumns = false;
            this.dgvChannelCondiSetting.AllowUserToResizeRows = false;
            this.dgvChannelCondiSetting.BackgroundColor = System.Drawing.Color.White;
            this.dgvChannelCondiSetting.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.Disable;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvChannelCondiSetting.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            resources.ApplyResources(this.dgvChannelCondiSetting, "dgvChannelCondiSetting");
            this.dgvChannelCondiSetting.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Arial", 9F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvChannelCondiSetting.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvChannelCondiSetting.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(215)))), ((int)(((byte)(230)))));
            this.dgvChannelCondiSetting.HighlightSelectedColumnHeaders = false;
            this.dgvChannelCondiSetting.MultiSelect = false;
            this.dgvChannelCondiSetting.Name = "dgvChannelCondiSetting";
            this.dgvChannelCondiSetting.PaintEnhancedSelection = false;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvChannelCondiSetting.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgvChannelCondiSetting.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvChannelCondiSetting.RowTemplate.Height = 24;
            this.dgvChannelCondiSetting.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgvChannelCondiSetting.ShowCellErrors = false;
            this.dgvChannelCondiSetting.ShowCellToolTips = false;
            this.dgvChannelCondiSetting.ShowEditingIcon = false;
            this.dgvChannelCondiSetting.ShowRowErrors = false;
            this.dgvChannelCondiSetting.TabStop = false;
            this.dgvChannelCondiSetting.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvChannelCondiSetting_CellFormatting);
            this.dgvChannelCondiSetting.CellValidated += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvChannelCondiSetting_CellValidated);
            // 
            // lblRecipeCondiTableInfo
            // 
            resources.ApplyResources(this.lblRecipeCondiTableInfo, "lblRecipeCondiTableInfo");
            this.lblRecipeCondiTableInfo.Name = "lblRecipeCondiTableInfo";
            // 
            // btnConfirm
            // 
            this.btnConfirm.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnConfirm.AntiAlias = true;
            this.btnConfirm.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.btnConfirm, "btnConfirm");
            this.btnConfirm.Image = global::MPI.Tester.Gui.Properties.Resources.btnConfirm;
            this.btnConfirm.ImageFixedSize = new System.Drawing.Size(20, 20);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Shape = new DevComponents.DotNetBar.RoundRectangleShapeDescriptor(8);
            this.btnConfirm.Style = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
            this.btnConfirm.TextAlignment = DevComponents.DotNetBar.eButtonTextAlignment.Left;
            this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click);
            // 
            // chkSamplingOpticalTest
            // 
            this.chkSamplingOpticalTest.BackColor = System.Drawing.Color.LightSteelBlue;
            // 
            // 
            // 
            this.chkSamplingOpticalTest.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.chkSamplingOpticalTest.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.chkSamplingOpticalTest.CheckSignSize = new System.Drawing.Size(18, 18);
            resources.ApplyResources(this.chkSamplingOpticalTest, "chkSamplingOpticalTest");
            this.chkSamplingOpticalTest.Name = "chkSamplingOpticalTest";
            this.chkSamplingOpticalTest.Style = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
            // 
            // frmChannelCondition
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ControlBox = false;
            this.Controls.Add(this.chkSamplingOpticalTest);
            this.Controls.Add(this.lblRecipeCondiTableInfo);
            this.Controls.Add(this.btnConfirm);
            this.Controls.Add(this.dgvChannelCondiSetting);
            this.Controls.Add(this.pnlDutChDisplay);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmChannelCondition";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            ((System.ComponentModel.ISupportInitialize)(this.dgvChannelCondiSetting)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevComponents.DotNetBar.Controls.DataGridViewX dgvChannelCondiSetting;
        private System.Windows.Forms.Panel pnlDutChDisplay;
        private System.Windows.Forms.Label lblRecipeCondiTableInfo;
        private DevComponents.DotNetBar.ButtonX btnConfirm;
        private DevComponents.DotNetBar.Controls.CheckBoxX chkSamplingOpticalTest;
    }
}
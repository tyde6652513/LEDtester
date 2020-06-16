namespace MPI.Tester.Gui
{
    partial class frmConditionItemSetting
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmConditionItemSetting));
            this.pnlCondition = new System.Windows.Forms.Panel();
            this.btnCancel = new DevComponents.DotNetBar.ButtonX();
            this.btnConfirm = new DevComponents.DotNetBar.ButtonX();
            this.lblTestItemName = new DevComponents.DotNetBar.LabelX();
            this.cmbTestSelected = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.SuspendLayout();
            // 
            // pnlCondition
            // 
            this.pnlCondition.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.pnlCondition, "pnlCondition");
            this.pnlCondition.Name = "pnlCondition";
            // 
            // btnCancel
            // 
            this.btnCancel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnCancel.AntiAlias = true;
            this.btnCancel.BackColor = System.Drawing.Color.Transparent;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FocusCuesEnabled = false;
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.Image = ((System.Drawing.Image)(resources.GetObject("btnCancel.Image")));
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Shape = new DevComponents.DotNetBar.RoundRectangleShapeDescriptor(8);
            this.btnCancel.ShowSubItems = false;
            this.btnCancel.Style = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
            this.btnCancel.TextAlignment = DevComponents.DotNetBar.eButtonTextAlignment.Left;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnConfirm
            // 
            this.btnConfirm.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnConfirm.AntiAlias = true;
            this.btnConfirm.BackColor = System.Drawing.Color.Transparent;
            this.btnConfirm.FocusCuesEnabled = false;
            resources.ApplyResources(this.btnConfirm, "btnConfirm");
            this.btnConfirm.Image = ((System.Drawing.Image)(resources.GetObject("btnConfirm.Image")));
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Shape = new DevComponents.DotNetBar.RoundRectangleShapeDescriptor(8);
            this.btnConfirm.ShowSubItems = false;
            this.btnConfirm.Style = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
            this.btnConfirm.TextAlignment = DevComponents.DotNetBar.eButtonTextAlignment.Left;
            this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click);
            // 
            // lblTestItemName
            // 
            this.lblTestItemName.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.lblTestItemName.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.lblTestItemName.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            resources.ApplyResources(this.lblTestItemName, "lblTestItemName");
            this.lblTestItemName.ForeColor = System.Drawing.Color.Black;
            this.lblTestItemName.Name = "lblTestItemName";
            this.lblTestItemName.TextAlignment = System.Drawing.StringAlignment.Center;
            // 
            // cmbTestSelected
            // 
            this.cmbTestSelected.DisplayMember = "Text";
            this.cmbTestSelected.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTestSelected.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            resources.ApplyResources(this.cmbTestSelected, "cmbTestSelected");
            this.cmbTestSelected.FormattingEnabled = true;
            this.cmbTestSelected.Name = "cmbTestSelected";
            this.cmbTestSelected.Style = DevComponents.DotNetBar.eDotNetBarStyle.Windows7;
            this.cmbTestSelected.SelectedIndexChanged += new System.EventHandler(this.cmbTestSelected_SelectedIndexChanged);
            // 
            // frmConditionItemSetting
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackgroundImage = global::MPI.Tester.Gui.Properties.Resources.mini_hero_bg;
            resources.ApplyResources(this, "$this");
            this.ControlBox = false;
            this.Controls.Add(this.pnlCondition);
            this.Controls.Add(this.cmbTestSelected);
            this.Controls.Add(this.btnConfirm);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.lblTestItemName);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmConditionItemSetting";
            this.TopMost = true;
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlCondition;
        private DevComponents.DotNetBar.ButtonX btnCancel;
        private DevComponents.DotNetBar.ButtonX btnConfirm;
        private DevComponents.DotNetBar.LabelX lblTestItemName;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cmbTestSelected;

    }
}
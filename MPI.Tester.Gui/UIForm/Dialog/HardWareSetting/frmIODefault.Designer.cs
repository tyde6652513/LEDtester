namespace MPI.Tester.Gui
{
    partial class frmIODefault
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmIODefault));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.grpIODefault = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.dgvIODefault = new DevComponents.DotNetBar.Controls.DataGridViewX();
            this.groupPanel1 = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.btnLoad = new DevComponents.DotNetBar.ButtonX();
            this.btnSaveAs = new DevComponents.DotNetBar.ButtonX();
            this.btnSave = new DevComponents.DotNetBar.ButtonX();
            this.colShow = new DevComponents.DotNetBar.Controls.DataGridViewCheckBoxXColumn();
            this.colPin = new DevComponents.DotNetBar.Controls.DataGridViewIntegerInputColumn();
            this.colMode = new DevComponents.DotNetBar.Controls.DataGridViewComboBoxExColumn();
            this.colState = new DevComponents.DotNetBar.Controls.DataGridViewComboBoxExColumn();
            this.colAct = new DevComponents.DotNetBar.Controls.DataGridViewComboBoxExColumn();
            this.colPulseWidth = new DevComponents.DotNetBar.Controls.DataGridViewDoubleInputColumn();
            this.colMark = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.grpIODefault.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvIODefault)).BeginInit();
            this.groupPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            resources.ApplyResources(this.splitContainer1, "splitContainer1");
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.grpIODefault);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.groupPanel1);
            // 
            // grpIODefault
            // 
            this.grpIODefault.BackColor = System.Drawing.SystemColors.Control;
            this.grpIODefault.CanvasColor = System.Drawing.SystemColors.Control;
            this.grpIODefault.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.grpIODefault.Controls.Add(this.dgvIODefault);
            resources.ApplyResources(this.grpIODefault, "grpIODefault");
            this.grpIODefault.DrawTitleBox = false;
            this.grpIODefault.Name = "grpIODefault";
            // 
            // 
            // 
            this.grpIODefault.Style.BackColor = System.Drawing.SystemColors.Control;
            this.grpIODefault.Style.BackColor2 = System.Drawing.SystemColors.Control;
            this.grpIODefault.Style.BackColorGradientAngle = 90;
            this.grpIODefault.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.grpIODefault.Style.BorderBottomWidth = 1;
            this.grpIODefault.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.grpIODefault.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Dash;
            this.grpIODefault.Style.BorderLeftWidth = 1;
            this.grpIODefault.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.grpIODefault.Style.BorderRightWidth = 1;
            this.grpIODefault.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.grpIODefault.Style.BorderTopWidth = 1;
            this.grpIODefault.Style.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.grpIODefault.Style.CornerDiameter = 4;
            this.grpIODefault.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded;
            this.grpIODefault.Style.TextColor = System.Drawing.Color.DarkOrange;
            this.grpIODefault.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near;
            // 
            // 
            // 
            this.grpIODefault.StyleMouseDown.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.grpIODefault.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this.grpIODefault.StyleMouseOver.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.grpIODefault.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // dgvIODefault
            // 
            this.dgvIODefault.AllowUserToAddRows = false;
            this.dgvIODefault.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvIODefault.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colShow,
            this.colPin,
            this.colMode,
            this.colState,
            this.colAct,
            this.colPulseWidth,
            this.colMark});
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvIODefault.DefaultCellStyle = dataGridViewCellStyle1;
            resources.ApplyResources(this.dgvIODefault, "dgvIODefault");
            this.dgvIODefault.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(215)))), ((int)(((byte)(229)))));
            this.dgvIODefault.Name = "dgvIODefault";
            this.dgvIODefault.RowTemplate.Height = 24;
            this.dgvIODefault.CellValidated += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvIODefault_CellValidated);
            // 
            // groupPanel1
            // 
            this.groupPanel1.BackColor = System.Drawing.SystemColors.Control;
            this.groupPanel1.CanvasColor = System.Drawing.SystemColors.Control;
            this.groupPanel1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.groupPanel1.Controls.Add(this.btnLoad);
            this.groupPanel1.Controls.Add(this.btnSaveAs);
            this.groupPanel1.Controls.Add(this.btnSave);
            resources.ApplyResources(this.groupPanel1, "groupPanel1");
            this.groupPanel1.DrawTitleBox = false;
            this.groupPanel1.Name = "groupPanel1";
            // 
            // 
            // 
            this.groupPanel1.Style.BackColor = System.Drawing.SystemColors.Control;
            this.groupPanel1.Style.BackColor2 = System.Drawing.SystemColors.Control;
            this.groupPanel1.Style.BackColorGradientAngle = 90;
            this.groupPanel1.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel1.Style.BorderBottomWidth = 1;
            this.groupPanel1.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.groupPanel1.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Dash;
            this.groupPanel1.Style.BorderLeftWidth = 1;
            this.groupPanel1.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel1.Style.BorderRightWidth = 1;
            this.groupPanel1.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel1.Style.BorderTopWidth = 1;
            this.groupPanel1.Style.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.groupPanel1.Style.CornerDiameter = 4;
            this.groupPanel1.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded;
            this.groupPanel1.Style.TextColor = System.Drawing.Color.DarkOrange;
            this.groupPanel1.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near;
            // 
            // 
            // 
            this.groupPanel1.StyleMouseDown.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.groupPanel1.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this.groupPanel1.StyleMouseOver.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.groupPanel1.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // btnLoad
            // 
            this.btnLoad.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnLoad.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            resources.ApplyResources(this.btnLoad, "btnLoad");
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Style = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // btnSaveAs
            // 
            this.btnSaveAs.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnSaveAs.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            resources.ApplyResources(this.btnSaveAs, "btnSaveAs");
            this.btnSaveAs.Name = "btnSaveAs";
            this.btnSaveAs.Style = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
            this.btnSaveAs.Click += new System.EventHandler(this.btnSaveAs_Click);
            // 
            // btnSave
            // 
            this.btnSave.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnSave.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            resources.ApplyResources(this.btnSave, "btnSave");
            this.btnSave.Name = "btnSave";
            this.btnSave.Style = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // colShow
            // 
            this.colShow.Checked = true;
            this.colShow.CheckState = System.Windows.Forms.CheckState.Indeterminate;
            this.colShow.CheckValue = "N";
            this.colShow.FillWeight = 50F;
            resources.ApplyResources(this.colShow, "colShow");
            this.colShow.Name = "colShow";
            this.colShow.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // colPin
            // 
            // 
            // 
            // 
            this.colPin.BackgroundStyle.BackColor = System.Drawing.SystemColors.Window;
            this.colPin.BackgroundStyle.Class = "DataGridViewNumericBorder";
            this.colPin.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.colPin.BackgroundStyle.TextColor = System.Drawing.SystemColors.ControlText;
            this.colPin.FillWeight = 50F;
            resources.ApplyResources(this.colPin, "colPin");
            this.colPin.InputHorizontalAlignment = DevComponents.Editors.eHorizontalAlignment.Left;
            this.colPin.MaxValue = 256;
            this.colPin.MinValue = 0;
            this.colPin.Name = "colPin";
            this.colPin.ReadOnly = true;
            this.colPin.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // colMode
            // 
            this.colMode.DisplayMember = "Text";
            this.colMode.DropDownHeight = 106;
            this.colMode.DropDownWidth = 121;
            this.colMode.FillWeight = 150F;
            this.colMode.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            resources.ApplyResources(this.colMode, "colMode");
            this.colMode.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.colMode.IntegralHeight = false;
            this.colMode.ItemHeight = 17;
            this.colMode.Name = "colMode";
            this.colMode.RightToLeft = System.Windows.Forms.RightToLeft.No;
            // 
            // colState
            // 
            this.colState.DisplayMember = "Text";
            this.colState.DropDownHeight = 106;
            this.colState.DropDownWidth = 121;
            this.colState.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            resources.ApplyResources(this.colState, "colState");
            this.colState.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.colState.IntegralHeight = false;
            this.colState.ItemHeight = 17;
            this.colState.Name = "colState";
            this.colState.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colState.RightToLeft = System.Windows.Forms.RightToLeft.No;
            // 
            // colAct
            // 
            this.colAct.DropDownHeight = 106;
            this.colAct.DropDownWidth = 121;
            this.colAct.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            resources.ApplyResources(this.colAct, "colAct");
            this.colAct.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.colAct.ItemHeight = 17;
            this.colAct.Name = "colAct";
            this.colAct.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colAct.RightToLeft = System.Windows.Forms.RightToLeft.No;
            // 
            // colPulseWidth
            // 
            // 
            // 
            // 
            this.colPulseWidth.BackgroundStyle.BackColor = System.Drawing.SystemColors.Window;
            this.colPulseWidth.BackgroundStyle.Class = "DataGridViewNumericBorder";
            this.colPulseWidth.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.colPulseWidth.BackgroundStyle.TextColor = System.Drawing.SystemColors.ControlText;
            this.colPulseWidth.DisplayFormat = "0.000###";
            this.colPulseWidth.FillWeight = 120F;
            resources.ApplyResources(this.colPulseWidth, "colPulseWidth");
            this.colPulseWidth.Increment = 1D;
            this.colPulseWidth.InputHorizontalAlignment = DevComponents.Editors.eHorizontalAlignment.Left;
            this.colPulseWidth.MaxValue = 10000D;
            this.colPulseWidth.MinValue = 0D;
            this.colPulseWidth.Name = "colPulseWidth";
            this.colPulseWidth.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // colMark
            // 
            this.colMark.FillWeight = 150F;
            resources.ApplyResources(this.colMark, "colMark");
            this.colMark.Name = "colMark";
            this.colMark.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // frmIODefault
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::MPI.Tester.Gui.Properties.Resources.mini_hero_bg;
            this.Controls.Add(this.splitContainer1);
            this.Name = "frmIODefault";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.grpIODefault.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvIODefault)).EndInit();
            this.groupPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.Controls.GroupPanel grpIODefault;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private DevComponents.DotNetBar.Controls.DataGridViewX dgvIODefault;
        private DevComponents.DotNetBar.Controls.GroupPanel groupPanel1;
        private DevComponents.DotNetBar.ButtonX btnSave;
        private DevComponents.DotNetBar.ButtonX btnSaveAs;
        private DevComponents.DotNetBar.ButtonX btnLoad;
        private DevComponents.DotNetBar.Controls.DataGridViewCheckBoxXColumn colShow;
        private DevComponents.DotNetBar.Controls.DataGridViewIntegerInputColumn colPin;
        private DevComponents.DotNetBar.Controls.DataGridViewComboBoxExColumn colMode;
        private DevComponents.DotNetBar.Controls.DataGridViewComboBoxExColumn colState;
        private DevComponents.DotNetBar.Controls.DataGridViewComboBoxExColumn colAct;
        private DevComponents.DotNetBar.Controls.DataGridViewDoubleInputColumn colPulseWidth;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMark;


    }
}
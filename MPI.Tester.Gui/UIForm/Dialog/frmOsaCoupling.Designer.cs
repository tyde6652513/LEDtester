namespace MPI.Tester.Gui
{
    partial class frmOsaCoupling
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
            this.grpOsaSetting = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.btnOsaStop = new DevComponents.DotNetBar.ButtonX();
            this.btnOsaRepeat = new DevComponents.DotNetBar.ButtonX();
            this.grpSmuSetting = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.btnSmuOutputOFF = new DevComponents.DotNetBar.ButtonX();
            this.pnlMsrtClamp = new System.Windows.Forms.Panel();
            this.lblMsrtClamp = new DevComponents.DotNetBar.LabelX();
            this.dinMsrtClamp = new DevComponents.Editors.DoubleInput();
            this.lblMsrtClampUnit = new DevComponents.DotNetBar.LabelX();
            this.btnSmuOutputON = new DevComponents.DotNetBar.ButtonX();
            this.pnlForceValue = new System.Windows.Forms.Panel();
            this.lblForceValue = new DevComponents.DotNetBar.LabelX();
            this.dinForceValue = new DevComponents.Editors.DoubleInput();
            this.lblForceValueUnit = new DevComponents.DotNetBar.LabelX();
            this.grpOsaSetting.SuspendLayout();
            this.grpSmuSetting.SuspendLayout();
            this.pnlMsrtClamp.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dinMsrtClamp)).BeginInit();
            this.pnlForceValue.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dinForceValue)).BeginInit();
            this.SuspendLayout();
            // 
            // grpOsaSetting
            // 
            this.grpOsaSetting.BackColor = System.Drawing.SystemColors.Control;
            this.grpOsaSetting.CanvasColor = System.Drawing.SystemColors.Control;
            this.grpOsaSetting.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.grpOsaSetting.Controls.Add(this.btnOsaStop);
            this.grpOsaSetting.Controls.Add(this.btnOsaRepeat);
            this.grpOsaSetting.DrawTitleBox = false;
            this.grpOsaSetting.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold);
            this.grpOsaSetting.Location = new System.Drawing.Point(12, 158);
            this.grpOsaSetting.Name = "grpOsaSetting";
            this.grpOsaSetting.Size = new System.Drawing.Size(483, 140);
            // 
            // 
            // 
            this.grpOsaSetting.Style.BackColor = System.Drawing.SystemColors.Control;
            this.grpOsaSetting.Style.BackColor2 = System.Drawing.SystemColors.Control;
            this.grpOsaSetting.Style.BackColorGradientAngle = 90;
            this.grpOsaSetting.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.grpOsaSetting.Style.BorderBottomWidth = 1;
            this.grpOsaSetting.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.grpOsaSetting.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Dash;
            this.grpOsaSetting.Style.BorderLeftWidth = 1;
            this.grpOsaSetting.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.grpOsaSetting.Style.BorderRightWidth = 1;
            this.grpOsaSetting.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.grpOsaSetting.Style.BorderTopWidth = 1;
            this.grpOsaSetting.Style.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10015;
            this.grpOsaSetting.Style.CornerDiameter = 4;
            this.grpOsaSetting.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded;
            this.grpOsaSetting.Style.TextColor = System.Drawing.Color.DarkOrange;
            this.grpOsaSetting.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near;
            // 
            // 
            // 
            this.grpOsaSetting.StyleMouseDown.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10015;
            this.grpOsaSetting.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this.grpOsaSetting.StyleMouseOver.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10015;
            this.grpOsaSetting.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.grpOsaSetting.TabIndex = 2;
            this.grpOsaSetting.Text = "OSA Setting";
            // 
            // btnOsaStop
            // 
            this.btnOsaStop.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnOsaStop.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnOsaStop.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold);
            this.btnOsaStop.Location = new System.Drawing.Point(325, 51);
            this.btnOsaStop.Name = "btnOsaStop";
            this.btnOsaStop.Size = new System.Drawing.Size(132, 42);
            this.btnOsaStop.Style = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
            this.btnOsaStop.TabIndex = 14;
            this.btnOsaStop.Text = "Stop Sweep";
            this.btnOsaStop.Click += new System.EventHandler(this.btnOsaStop_Click);
            // 
            // btnOsaRepeat
            // 
            this.btnOsaRepeat.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnOsaRepeat.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnOsaRepeat.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold);
            this.btnOsaRepeat.Location = new System.Drawing.Point(325, 3);
            this.btnOsaRepeat.Name = "btnOsaRepeat";
            this.btnOsaRepeat.Size = new System.Drawing.Size(132, 42);
            this.btnOsaRepeat.Style = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
            this.btnOsaRepeat.TabIndex = 13;
            this.btnOsaRepeat.Text = "Repeat Sweep";
            this.btnOsaRepeat.Click += new System.EventHandler(this.btnOsaRepeat_Click);
            // 
            // grpSmuSetting
            // 
            this.grpSmuSetting.BackColor = System.Drawing.SystemColors.Control;
            this.grpSmuSetting.CanvasColor = System.Drawing.SystemColors.Control;
            this.grpSmuSetting.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.grpSmuSetting.Controls.Add(this.btnSmuOutputOFF);
            this.grpSmuSetting.Controls.Add(this.pnlMsrtClamp);
            this.grpSmuSetting.Controls.Add(this.btnSmuOutputON);
            this.grpSmuSetting.Controls.Add(this.pnlForceValue);
            this.grpSmuSetting.DrawTitleBox = false;
            this.grpSmuSetting.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpSmuSetting.Location = new System.Drawing.Point(12, 14);
            this.grpSmuSetting.Name = "grpSmuSetting";
            this.grpSmuSetting.Size = new System.Drawing.Size(483, 138);
            // 
            // 
            // 
            this.grpSmuSetting.Style.BackColor = System.Drawing.SystemColors.Control;
            this.grpSmuSetting.Style.BackColor2 = System.Drawing.SystemColors.Control;
            this.grpSmuSetting.Style.BackColorGradientAngle = 90;
            this.grpSmuSetting.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.grpSmuSetting.Style.BorderBottomWidth = 1;
            this.grpSmuSetting.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.grpSmuSetting.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Dash;
            this.grpSmuSetting.Style.BorderLeftWidth = 1;
            this.grpSmuSetting.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.grpSmuSetting.Style.BorderRightWidth = 1;
            this.grpSmuSetting.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.grpSmuSetting.Style.BorderTopWidth = 1;
            this.grpSmuSetting.Style.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10015;
            this.grpSmuSetting.Style.CornerDiameter = 4;
            this.grpSmuSetting.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded;
            this.grpSmuSetting.Style.TextColor = System.Drawing.Color.DarkOrange;
            this.grpSmuSetting.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near;
            // 
            // 
            // 
            this.grpSmuSetting.StyleMouseDown.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10015;
            this.grpSmuSetting.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this.grpSmuSetting.StyleMouseOver.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10015;
            this.grpSmuSetting.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.grpSmuSetting.TabIndex = 1;
            this.grpSmuSetting.Text = "SourceMeter Setting";
            // 
            // btnSmuOutputOFF
            // 
            this.btnSmuOutputOFF.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnSmuOutputOFF.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnSmuOutputOFF.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold);
            this.btnSmuOutputOFF.Location = new System.Drawing.Point(325, 51);
            this.btnSmuOutputOFF.Name = "btnSmuOutputOFF";
            this.btnSmuOutputOFF.Size = new System.Drawing.Size(132, 42);
            this.btnSmuOutputOFF.Style = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
            this.btnSmuOutputOFF.TabIndex = 16;
            this.btnSmuOutputOFF.Text = "Output OFF";
            this.btnSmuOutputOFF.Click += new System.EventHandler(this.btnSmuOutputOFF_Click);
            // 
            // pnlMsrtClamp
            // 
            this.pnlMsrtClamp.Controls.Add(this.lblMsrtClamp);
            this.pnlMsrtClamp.Controls.Add(this.dinMsrtClamp);
            this.pnlMsrtClamp.Controls.Add(this.lblMsrtClampUnit);
            this.pnlMsrtClamp.Location = new System.Drawing.Point(19, 40);
            this.pnlMsrtClamp.Name = "pnlMsrtClamp";
            this.pnlMsrtClamp.Size = new System.Drawing.Size(266, 31);
            this.pnlMsrtClamp.TabIndex = 189;
            // 
            // lblMsrtClamp
            // 
            this.lblMsrtClamp.AutoSize = true;
            this.lblMsrtClamp.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.lblMsrtClamp.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10015;
            this.lblMsrtClamp.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblMsrtClamp.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold);
            this.lblMsrtClamp.ForeColor = System.Drawing.Color.Black;
            this.lblMsrtClamp.Location = new System.Drawing.Point(3, 5);
            this.lblMsrtClamp.Name = "lblMsrtClamp";
            this.lblMsrtClamp.Size = new System.Drawing.Size(92, 21);
            this.lblMsrtClamp.TabIndex = 162;
            this.lblMsrtClamp.Text = "Msrt Clamp";
            // 
            // dinMsrtClamp
            // 
            // 
            // 
            // 
            this.dinMsrtClamp.BackgroundStyle.Class = "DateTimeInputBackground";
            this.dinMsrtClamp.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.dinMsrtClamp.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.dinMsrtClamp.DisplayFormat = "0.0";
            this.dinMsrtClamp.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold);
            this.dinMsrtClamp.Increment = 1D;
            this.dinMsrtClamp.Location = new System.Drawing.Point(109, 3);
            this.dinMsrtClamp.MaxValue = 15D;
            this.dinMsrtClamp.MinValue = 1D;
            this.dinMsrtClamp.Name = "dinMsrtClamp";
            this.dinMsrtClamp.ShowUpDown = true;
            this.dinMsrtClamp.Size = new System.Drawing.Size(110, 25);
            this.dinMsrtClamp.TabIndex = 163;
            this.dinMsrtClamp.Value = 8D;
            // 
            // lblMsrtClampUnit
            // 
            this.lblMsrtClampUnit.AutoSize = true;
            this.lblMsrtClampUnit.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.lblMsrtClampUnit.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10015;
            this.lblMsrtClampUnit.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblMsrtClampUnit.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold);
            this.lblMsrtClampUnit.ForeColor = System.Drawing.Color.Black;
            this.lblMsrtClampUnit.Location = new System.Drawing.Point(226, 5);
            this.lblMsrtClampUnit.Name = "lblMsrtClampUnit";
            this.lblMsrtClampUnit.Size = new System.Drawing.Size(18, 21);
            this.lblMsrtClampUnit.TabIndex = 164;
            this.lblMsrtClampUnit.Text = "V";
            // 
            // btnSmuOutputON
            // 
            this.btnSmuOutputON.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnSmuOutputON.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnSmuOutputON.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold);
            this.btnSmuOutputON.Location = new System.Drawing.Point(325, 3);
            this.btnSmuOutputON.Name = "btnSmuOutputON";
            this.btnSmuOutputON.Size = new System.Drawing.Size(132, 42);
            this.btnSmuOutputON.Style = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
            this.btnSmuOutputON.TabIndex = 15;
            this.btnSmuOutputON.Text = "Output ON";
            this.btnSmuOutputON.Click += new System.EventHandler(this.btnSmuOutputON_Click);
            // 
            // pnlForceValue
            // 
            this.pnlForceValue.Controls.Add(this.lblForceValue);
            this.pnlForceValue.Controls.Add(this.dinForceValue);
            this.pnlForceValue.Controls.Add(this.lblForceValueUnit);
            this.pnlForceValue.Location = new System.Drawing.Point(19, 3);
            this.pnlForceValue.Name = "pnlForceValue";
            this.pnlForceValue.Size = new System.Drawing.Size(266, 31);
            this.pnlForceValue.TabIndex = 111;
            // 
            // lblForceValue
            // 
            this.lblForceValue.AutoSize = true;
            this.lblForceValue.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.lblForceValue.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10015;
            this.lblForceValue.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblForceValue.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold);
            this.lblForceValue.ForeColor = System.Drawing.Color.Black;
            this.lblForceValue.Location = new System.Drawing.Point(3, 5);
            this.lblForceValue.Name = "lblForceValue";
            this.lblForceValue.Size = new System.Drawing.Size(95, 21);
            this.lblForceValue.TabIndex = 82;
            this.lblForceValue.Text = "Force Value";
            // 
            // dinForceValue
            // 
            // 
            // 
            // 
            this.dinForceValue.BackgroundStyle.Class = "DateTimeInputBackground";
            this.dinForceValue.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.dinForceValue.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.dinForceValue.DisplayFormat = "0.00";
            this.dinForceValue.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold);
            this.dinForceValue.Increment = 1D;
            this.dinForceValue.Location = new System.Drawing.Point(109, 3);
            this.dinForceValue.MaxValue = 100D;
            this.dinForceValue.MinValue = 0.01D;
            this.dinForceValue.Name = "dinForceValue";
            this.dinForceValue.ShowUpDown = true;
            this.dinForceValue.Size = new System.Drawing.Size(110, 25);
            this.dinForceValue.TabIndex = 98;
            this.dinForceValue.Value = 5D;
            // 
            // lblForceValueUnit
            // 
            this.lblForceValueUnit.AutoSize = true;
            this.lblForceValueUnit.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.lblForceValueUnit.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10015;
            this.lblForceValueUnit.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblForceValueUnit.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold);
            this.lblForceValueUnit.ForeColor = System.Drawing.Color.Black;
            this.lblForceValueUnit.Location = new System.Drawing.Point(226, 5);
            this.lblForceValueUnit.Name = "lblForceValueUnit";
            this.lblForceValueUnit.Size = new System.Drawing.Size(32, 21);
            this.lblForceValueUnit.TabIndex = 107;
            this.lblForceValueUnit.Text = "mA";
            // 
            // frmOsaCoupling
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::MPI.Tester.Gui.Properties.Resources.mini_hero_bg;
            this.ClientSize = new System.Drawing.Size(510, 314);
            this.Controls.Add(this.grpSmuSetting);
            this.Controls.Add(this.grpOsaSetting);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmOsaCoupling";
            this.Text = "OSA Coupling";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmOsaCoupling_FormClosing);
            this.grpOsaSetting.ResumeLayout(false);
            this.grpSmuSetting.ResumeLayout(false);
            this.pnlMsrtClamp.ResumeLayout(false);
            this.pnlMsrtClamp.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dinMsrtClamp)).EndInit();
            this.pnlForceValue.ResumeLayout(false);
            this.pnlForceValue.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dinForceValue)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.Controls.GroupPanel grpOsaSetting;
        private DevComponents.DotNetBar.Controls.GroupPanel grpSmuSetting;
        private DevComponents.DotNetBar.ButtonX btnOsaStop;
        private DevComponents.DotNetBar.ButtonX btnOsaRepeat;
        private System.Windows.Forms.Panel pnlForceValue;
        private DevComponents.DotNetBar.LabelX lblForceValue;
        private DevComponents.Editors.DoubleInput dinForceValue;
        private DevComponents.DotNetBar.LabelX lblForceValueUnit;
        private System.Windows.Forms.Panel pnlMsrtClamp;
        private DevComponents.DotNetBar.LabelX lblMsrtClamp;
        private DevComponents.Editors.DoubleInput dinMsrtClamp;
        private DevComponents.DotNetBar.LabelX lblMsrtClampUnit;
        private DevComponents.DotNetBar.ButtonX btnSmuOutputOFF;
        private DevComponents.DotNetBar.ButtonX btnSmuOutputON;


    }
}
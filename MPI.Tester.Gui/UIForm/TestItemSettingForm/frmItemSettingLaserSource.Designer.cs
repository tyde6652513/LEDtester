namespace MPI.Tester.Gui
{
    partial class frmItemSettingLaserSource
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
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.grpOS = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.grpAtt = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.grpChannel = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.cmbLaserChannel = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.pnlWaitTime = new System.Windows.Forms.Panel();
            this.lblWaitTime = new DevComponents.DotNetBar.LabelX();
            this.dinForceDelay = new DevComponents.Editors.DoubleInput();
            this.lblWaitTimeUnit = new DevComponents.DotNetBar.LabelX();
            this.lblSelectedChannel = new DevComponents.DotNetBar.LabelX();
            this.btnAutoSetAtt = new System.Windows.Forms.Button();
            this.grpPowerMeter = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.gpAutoTuneVOA = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.grpChannel.SuspendLayout();
            this.pnlWaitTime.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dinForceDelay)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.panel1);
            this.panel2.Controls.Add(this.grpChannel);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel2.Location = new System.Drawing.Point(3, 35);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(666, 399);
            this.panel2.TabIndex = 1;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.grpOS);
            this.panel1.Controls.Add(this.grpAtt);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 97);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(666, 302);
            this.panel1.TabIndex = 194;
            // 
            // grpOS
            // 
            this.grpOS.BackColor = System.Drawing.Color.Transparent;
            this.grpOS.CanvasColor = System.Drawing.Color.Empty;
            this.grpOS.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
            this.grpOS.Dock = System.Windows.Forms.DockStyle.Left;
            this.grpOS.DrawTitleBox = false;
            this.grpOS.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.grpOS.Location = new System.Drawing.Point(368, 0);
            this.grpOS.Margin = new System.Windows.Forms.Padding(0);
            this.grpOS.Name = "grpOS";
            this.grpOS.Size = new System.Drawing.Size(295, 302);
            // 
            // 
            // 
            this.grpOS.Style.BackColor = System.Drawing.Color.WhiteSmoke;
            this.grpOS.Style.BackColor2 = System.Drawing.SystemColors.Window;
            this.grpOS.Style.BackColorGradientAngle = 90;
            this.grpOS.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.grpOS.Style.BorderBottomWidth = 1;
            this.grpOS.Style.BorderColor = System.Drawing.Color.Gray;
            this.grpOS.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.grpOS.Style.BorderLeftWidth = 1;
            this.grpOS.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.grpOS.Style.BorderRightWidth = 1;
            this.grpOS.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.grpOS.Style.BorderTopWidth = 1;
            this.grpOS.Style.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.grpOS.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded;
            this.grpOS.Style.TextColor = System.Drawing.Color.DarkOrange;
            // 
            // 
            // 
            this.grpOS.StyleMouseDown.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.grpOS.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this.grpOS.StyleMouseOver.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.grpOS.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.grpOS.TabIndex = 193;
            this.grpOS.Text = "OpticalSwitch";
            // 
            // grpAtt
            // 
            this.grpAtt.BackColor = System.Drawing.Color.Transparent;
            this.grpAtt.CanvasColor = System.Drawing.Color.Empty;
            this.grpAtt.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Windows7;
            this.grpAtt.Dock = System.Windows.Forms.DockStyle.Left;
            this.grpAtt.DrawTitleBox = false;
            this.grpAtt.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.grpAtt.Location = new System.Drawing.Point(0, 0);
            this.grpAtt.Margin = new System.Windows.Forms.Padding(0);
            this.grpAtt.Name = "grpAtt";
            this.grpAtt.Size = new System.Drawing.Size(368, 302);
            // 
            // 
            // 
            this.grpAtt.Style.BackColor = System.Drawing.Color.WhiteSmoke;
            this.grpAtt.Style.BackColor2 = System.Drawing.SystemColors.Window;
            this.grpAtt.Style.BackColorGradientAngle = 90;
            this.grpAtt.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.grpAtt.Style.BorderBottomWidth = 1;
            this.grpAtt.Style.BorderColor = System.Drawing.Color.Gray;
            this.grpAtt.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.grpAtt.Style.BorderLeftWidth = 1;
            this.grpAtt.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.grpAtt.Style.BorderRightWidth = 1;
            this.grpAtt.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.grpAtt.Style.BorderTopWidth = 1;
            this.grpAtt.Style.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.grpAtt.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded;
            this.grpAtt.Style.TextColor = System.Drawing.Color.DarkOrange;
            // 
            // 
            // 
            this.grpAtt.StyleMouseDown.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.grpAtt.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this.grpAtt.StyleMouseOver.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.grpAtt.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.grpAtt.TabIndex = 192;
            this.grpAtt.Text = "Attenuator";
            // 
            // grpChannel
            // 
            this.grpChannel.BackColor = System.Drawing.Color.Transparent;
            this.grpChannel.CanvasColor = System.Drawing.Color.Empty;
            this.grpChannel.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
            this.grpChannel.Controls.Add(this.cmbLaserChannel);
            this.grpChannel.Controls.Add(this.pnlWaitTime);
            this.grpChannel.Controls.Add(this.lblSelectedChannel);
            this.grpChannel.Controls.Add(this.btnAutoSetAtt);
            this.grpChannel.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpChannel.DrawTitleBox = false;
            this.grpChannel.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.grpChannel.Location = new System.Drawing.Point(0, 0);
            this.grpChannel.Margin = new System.Windows.Forms.Padding(0);
            this.grpChannel.Name = "grpChannel";
            this.grpChannel.Size = new System.Drawing.Size(666, 97);
            // 
            // 
            // 
            this.grpChannel.Style.BackColor = System.Drawing.Color.WhiteSmoke;
            this.grpChannel.Style.BackColor2 = System.Drawing.SystemColors.Window;
            this.grpChannel.Style.BackColorGradientAngle = 90;
            this.grpChannel.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.grpChannel.Style.BorderBottomWidth = 1;
            this.grpChannel.Style.BorderColor = System.Drawing.Color.Gray;
            this.grpChannel.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.grpChannel.Style.BorderLeftWidth = 1;
            this.grpChannel.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.grpChannel.Style.BorderRightWidth = 1;
            this.grpChannel.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.grpChannel.Style.BorderTopWidth = 1;
            this.grpChannel.Style.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.grpChannel.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded;
            this.grpChannel.Style.TextColor = System.Drawing.Color.DarkOrange;
            // 
            // 
            // 
            this.grpChannel.StyleMouseDown.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.grpChannel.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this.grpChannel.StyleMouseOver.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.grpChannel.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.grpChannel.TabIndex = 191;
            this.grpChannel.Text = "CHANNEL";
            // 
            // cmbLaserChannel
            // 
            this.cmbLaserChannel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLaserChannel.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            this.cmbLaserChannel.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold);
            this.cmbLaserChannel.Location = new System.Drawing.Point(134, 3);
            this.cmbLaserChannel.MaxDropDownItems = 20;
            this.cmbLaserChannel.Name = "cmbLaserChannel";
            this.cmbLaserChannel.Size = new System.Drawing.Size(110, 26);
            this.cmbLaserChannel.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
            this.cmbLaserChannel.TabIndex = 114;
            this.cmbLaserChannel.SelectedValueChanged += new System.EventHandler(this.cmbLaserChannel_SelectedValueChanged);
            // 
            // pnlWaitTime
            // 
            this.pnlWaitTime.Controls.Add(this.lblWaitTime);
            this.pnlWaitTime.Controls.Add(this.dinForceDelay);
            this.pnlWaitTime.Controls.Add(this.lblWaitTimeUnit);
            this.pnlWaitTime.Location = new System.Drawing.Point(4, 33);
            this.pnlWaitTime.Name = "pnlWaitTime";
            this.pnlWaitTime.Size = new System.Drawing.Size(285, 31);
            this.pnlWaitTime.TabIndex = 116;
            // 
            // lblWaitTime
            // 
            this.lblWaitTime.AutoSize = true;
            this.lblWaitTime.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.lblWaitTime.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.lblWaitTime.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblWaitTime.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold);
            this.lblWaitTime.ForeColor = System.Drawing.Color.Black;
            this.lblWaitTime.Location = new System.Drawing.Point(3, 5);
            this.lblWaitTime.Name = "lblWaitTime";
            this.lblWaitTime.Size = new System.Drawing.Size(80, 21);
            this.lblWaitTime.TabIndex = 84;
            this.lblWaitTime.Text = "Wait Time";
            // 
            // dinForceDelay
            // 
            this.dinForceDelay.AutoResolveFreeTextEntries = false;
            // 
            // 
            // 
            this.dinForceDelay.BackgroundStyle.Class = "DateTimeInputBackground";
            this.dinForceDelay.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.dinForceDelay.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.dinForceDelay.DisplayFormat = "0.0";
            this.dinForceDelay.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold);
            this.dinForceDelay.Increment = 0.5D;
            this.dinForceDelay.Location = new System.Drawing.Point(130, 3);
            this.dinForceDelay.MaxValue = 90000D;
            this.dinForceDelay.MinValue = 0D;
            this.dinForceDelay.Name = "dinForceDelay";
            this.dinForceDelay.ShowUpDown = true;
            this.dinForceDelay.Size = new System.Drawing.Size(110, 25);
            this.dinForceDelay.TabIndex = 97;
            // 
            // lblWaitTimeUnit
            // 
            this.lblWaitTimeUnit.AutoSize = true;
            this.lblWaitTimeUnit.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.lblWaitTimeUnit.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.lblWaitTimeUnit.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblWaitTimeUnit.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold);
            this.lblWaitTimeUnit.ForeColor = System.Drawing.Color.Black;
            this.lblWaitTimeUnit.Location = new System.Drawing.Point(247, 5);
            this.lblWaitTimeUnit.Name = "lblWaitTimeUnit";
            this.lblWaitTimeUnit.Size = new System.Drawing.Size(30, 21);
            this.lblWaitTimeUnit.TabIndex = 88;
            this.lblWaitTimeUnit.Text = "ms";
            // 
            // lblSelectedChannel
            // 
            this.lblSelectedChannel.AutoSize = true;
            this.lblSelectedChannel.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.lblSelectedChannel.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.lblSelectedChannel.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblSelectedChannel.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold);
            this.lblSelectedChannel.ForeColor = System.Drawing.Color.Black;
            this.lblSelectedChannel.Location = new System.Drawing.Point(7, 3);
            this.lblSelectedChannel.Name = "lblSelectedChannel";
            this.lblSelectedChannel.Size = new System.Drawing.Size(111, 21);
            this.lblSelectedChannel.TabIndex = 113;
            this.lblSelectedChannel.Text = "Light Channel";
            // 
            // btnAutoSetAtt
            // 
            this.btnAutoSetAtt.Font = new System.Drawing.Font("PMingLiU", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnAutoSetAtt.ForeColor = System.Drawing.Color.Black;
            this.btnAutoSetAtt.Location = new System.Drawing.Point(568, 3);
            this.btnAutoSetAtt.Name = "btnAutoSetAtt";
            this.btnAutoSetAtt.Size = new System.Drawing.Size(85, 31);
            this.btnAutoSetAtt.TabIndex = 115;
            this.btnAutoSetAtt.Text = "Tune VOA";
            this.btnAutoSetAtt.UseVisualStyleBackColor = true;
            this.btnAutoSetAtt.Visible = false;
            this.btnAutoSetAtt.Click += new System.EventHandler(this.btnAutoSetAtt_Click_1);
            // 
            // grpPowerMeter
            // 
            this.grpPowerMeter.BackColor = System.Drawing.Color.Transparent;
            this.grpPowerMeter.CanvasColor = System.Drawing.Color.Empty;
            this.grpPowerMeter.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
            this.grpPowerMeter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.grpPowerMeter.DrawTitleBox = false;
            this.grpPowerMeter.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.grpPowerMeter.Location = new System.Drawing.Point(0, 110);
            this.grpPowerMeter.Margin = new System.Windows.Forms.Padding(0);
            this.grpPowerMeter.Name = "grpPowerMeter";
            this.grpPowerMeter.Size = new System.Drawing.Size(372, 289);
            // 
            // 
            // 
            this.grpPowerMeter.Style.BackColor = System.Drawing.Color.WhiteSmoke;
            this.grpPowerMeter.Style.BackColor2 = System.Drawing.SystemColors.Window;
            this.grpPowerMeter.Style.BackColorGradientAngle = 90;
            this.grpPowerMeter.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.grpPowerMeter.Style.BorderBottomWidth = 1;
            this.grpPowerMeter.Style.BorderColor = System.Drawing.Color.Gray;
            this.grpPowerMeter.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.grpPowerMeter.Style.BorderLeftWidth = 1;
            this.grpPowerMeter.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.grpPowerMeter.Style.BorderRightWidth = 1;
            this.grpPowerMeter.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.grpPowerMeter.Style.BorderTopWidth = 1;
            this.grpPowerMeter.Style.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.grpPowerMeter.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded;
            this.grpPowerMeter.Style.TextColor = System.Drawing.Color.DarkOrange;
            // 
            // 
            // 
            this.grpPowerMeter.StyleMouseDown.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.grpPowerMeter.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this.grpPowerMeter.StyleMouseOver.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.grpPowerMeter.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.grpPowerMeter.TabIndex = 194;
            this.grpPowerMeter.Text = "PowerMeter";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.panel3);
            this.groupBox1.Controls.Add(this.panel2);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Font = new System.Drawing.Font("Arial", 20.25F, System.Drawing.FontStyle.Bold);
            this.groupBox1.ForeColor = System.Drawing.Color.DimGray;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1044, 437);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Light Source";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.gpAutoTuneVOA);
            this.panel3.Controls.Add(this.grpPowerMeter);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(669, 35);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(372, 399);
            this.panel3.TabIndex = 195;
            // 
            // gpAutoTuneVOA
            // 
            this.gpAutoTuneVOA.BackColor = System.Drawing.Color.Transparent;
            this.gpAutoTuneVOA.CanvasColor = System.Drawing.Color.Empty;
            this.gpAutoTuneVOA.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
            this.gpAutoTuneVOA.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gpAutoTuneVOA.DrawTitleBox = false;
            this.gpAutoTuneVOA.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.gpAutoTuneVOA.Location = new System.Drawing.Point(0, 0);
            this.gpAutoTuneVOA.Margin = new System.Windows.Forms.Padding(0);
            this.gpAutoTuneVOA.Name = "gpAutoTuneVOA";
            this.gpAutoTuneVOA.Size = new System.Drawing.Size(372, 110);
            // 
            // 
            // 
            this.gpAutoTuneVOA.Style.BackColor = System.Drawing.Color.WhiteSmoke;
            this.gpAutoTuneVOA.Style.BackColor2 = System.Drawing.SystemColors.Window;
            this.gpAutoTuneVOA.Style.BackColorGradientAngle = 90;
            this.gpAutoTuneVOA.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.gpAutoTuneVOA.Style.BorderBottomWidth = 1;
            this.gpAutoTuneVOA.Style.BorderColor = System.Drawing.Color.Gray;
            this.gpAutoTuneVOA.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.gpAutoTuneVOA.Style.BorderLeftWidth = 1;
            this.gpAutoTuneVOA.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.gpAutoTuneVOA.Style.BorderRightWidth = 1;
            this.gpAutoTuneVOA.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.gpAutoTuneVOA.Style.BorderTopWidth = 1;
            this.gpAutoTuneVOA.Style.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.gpAutoTuneVOA.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded;
            this.gpAutoTuneVOA.Style.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gpAutoTuneVOA.Style.TextColor = System.Drawing.Color.DarkOrange;
            // 
            // 
            // 
            this.gpAutoTuneVOA.StyleMouseDown.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.gpAutoTuneVOA.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this.gpAutoTuneVOA.StyleMouseOver.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.gpAutoTuneVOA.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.gpAutoTuneVOA.TabIndex = 195;
            this.gpAutoTuneVOA.Text = "Auto Tune VOA";
            this.gpAutoTuneVOA.Visible = false;
            // 
            // frmItemSettingLaserSource
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1044, 437);
            this.Controls.Add(this.groupBox1);
            this.Name = "frmItemSettingLaserSource";
            this.Text = "Laser Source";
            this.panel2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.grpChannel.ResumeLayout(false);
            this.grpChannel.PerformLayout();
            this.pnlWaitTime.ResumeLayout(false);
            this.pnlWaitTime.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dinForceDelay)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.GroupBox groupBox1;
        private DevComponents.DotNetBar.Controls.GroupPanel grpAtt;
        private DevComponents.DotNetBar.Controls.GroupPanel grpChannel;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cmbLaserChannel;
        private DevComponents.DotNetBar.LabelX lblSelectedChannel;
        private DevComponents.DotNetBar.Controls.GroupPanel grpOS;
        private DevComponents.DotNetBar.Controls.GroupPanel grpPowerMeter;
        private System.Windows.Forms.Button btnAutoSetAtt;
        private System.Windows.Forms.Panel pnlWaitTime;
        private DevComponents.DotNetBar.LabelX lblWaitTime;
        private DevComponents.Editors.DoubleInput dinForceDelay;
        private DevComponents.DotNetBar.LabelX lblWaitTimeUnit;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel3;
        private DevComponents.DotNetBar.Controls.GroupPanel gpAutoTuneVOA;
    }
}
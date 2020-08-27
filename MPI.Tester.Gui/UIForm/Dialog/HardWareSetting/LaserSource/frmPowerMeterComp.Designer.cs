

namespace MPI.Tester.Gui.UIForm.Dialog.HardWareSetting.LaserSource
{
    partial class frmPowerMeterComp
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
            this.pnlSMU = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.dinPDGain = new DevComponents.Editors.DoubleInput();
            this.pnlClamp = new System.Windows.Forms.Panel();
            this.label8 = new System.Windows.Forms.Label();
            this.pnlBias = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.dinBias = new DevComponents.Editors.DoubleInput();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label10 = new System.Windows.Forms.Label();
            this.dinSysGain = new DevComponents.Editors.DoubleInput();
            this.pnlTarPow = new System.Windows.Forms.Panel();
            this.label11 = new System.Windows.Forms.Label();
            this.dinCheckLim = new DevComponents.Editors.DoubleInput();
            this.label7 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.dinTarPow = new DevComponents.Editors.DoubleInput();
            this.btnMsrt = new System.Windows.Forms.Button();
            this.pnlWaveLength = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.dinWaveLength = new DevComponents.Editors.DoubleInput();
            this.lblResultPow = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.chkRecord = new System.Windows.Forms.CheckBox();
            this.CmpClampA = new MPI.Tester.GuiComponent.UnitA();
            this.doubleInput1 = new DevComponents.Editors.DoubleInput();
            this.pnlSMU.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dinPDGain)).BeginInit();
            this.pnlClamp.SuspendLayout();
            this.pnlBias.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dinBias)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dinSysGain)).BeginInit();
            this.pnlTarPow.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dinCheckLim)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dinTarPow)).BeginInit();
            this.pnlWaveLength.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dinWaveLength)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.doubleInput1)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlSMU
            // 
            this.pnlSMU.Controls.Add(this.panel3);
            this.pnlSMU.Controls.Add(this.pnlClamp);
            this.pnlSMU.Controls.Add(this.pnlBias);
            this.pnlSMU.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSMU.Location = new System.Drawing.Point(0, 161);
            this.pnlSMU.Name = "pnlSMU";
            this.pnlSMU.Size = new System.Drawing.Size(360, 107);
            this.pnlSMU.TabIndex = 3;
            this.pnlSMU.Visible = false;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.label12);
            this.panel3.Controls.Add(this.label13);
            this.panel3.Controls.Add(this.dinPDGain);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Font = new System.Drawing.Font("PMingLiU", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.panel3.ForeColor = System.Drawing.Color.Black;
            this.panel3.Location = new System.Drawing.Point(0, 63);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(360, 34);
            this.panel3.TabIndex = 8;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("PMingLiU", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label12.Location = new System.Drawing.Point(257, 4);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(47, 19);
            this.label12.TabIndex = 4;
            this.label12.Text = "A/W";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("PMingLiU", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label13.Location = new System.Drawing.Point(3, 4);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(79, 19);
            this.label13.TabIndex = 1;
            this.label13.Text = "PD Gain";
            // 
            // dinPDGain
            // 
            // 
            // 
            // 
            this.dinPDGain.BackgroundStyle.Class = "DateTimeInputBackground";
            this.dinPDGain.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.dinPDGain.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.dinPDGain.DisplayFormat = "E3";
            this.dinPDGain.Font = new System.Drawing.Font("PMingLiU", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.dinPDGain.Increment = 1D;
            this.dinPDGain.Location = new System.Drawing.Point(126, 3);
            this.dinPDGain.MaxValue = 1000D;
            this.dinPDGain.MinValue = 0D;
            this.dinPDGain.Name = "dinPDGain";
            this.dinPDGain.ShowUpDown = true;
            this.dinPDGain.Size = new System.Drawing.Size(125, 27);
            this.dinPDGain.TabIndex = 0;
            this.dinPDGain.Value = 1D;
            // 
            // pnlClamp
            // 
            this.pnlClamp.Controls.Add(this.CmpClampA);
            this.pnlClamp.Controls.Add(this.label8);
            this.pnlClamp.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlClamp.Font = new System.Drawing.Font("PMingLiU", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.pnlClamp.ForeColor = System.Drawing.Color.Black;
            this.pnlClamp.Location = new System.Drawing.Point(0, 28);
            this.pnlClamp.Name = "pnlClamp";
            this.pnlClamp.Size = new System.Drawing.Size(360, 35);
            this.pnlClamp.TabIndex = 6;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("PMingLiU", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label8.Location = new System.Drawing.Point(3, 4);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(62, 19);
            this.label8.TabIndex = 1;
            this.label8.Text = "Clamp";
            // 
            // pnlBias
            // 
            this.pnlBias.Controls.Add(this.label5);
            this.pnlBias.Controls.Add(this.label6);
            this.pnlBias.Controls.Add(this.doubleInput1);
            this.pnlBias.Controls.Add(this.dinBias);
            this.pnlBias.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlBias.Font = new System.Drawing.Font("PMingLiU", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.pnlBias.ForeColor = System.Drawing.Color.Black;
            this.pnlBias.Location = new System.Drawing.Point(0, 0);
            this.pnlBias.Name = "pnlBias";
            this.pnlBias.Size = new System.Drawing.Size(360, 28);
            this.pnlBias.TabIndex = 5;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("PMingLiU", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label5.Location = new System.Drawing.Point(257, 4);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(23, 19);
            this.label5.TabIndex = 3;
            this.label5.Text = "V";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("PMingLiU", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label6.Location = new System.Drawing.Point(3, 4);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(45, 19);
            this.label6.TabIndex = 1;
            this.label6.Text = "Bias";
            // 
            // dinBias
            // 
            // 
            // 
            // 
            this.dinBias.BackgroundStyle.Class = "DateTimeInputBackground";
            this.dinBias.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.dinBias.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.dinBias.DisplayFormat = "0.00";
            this.dinBias.Font = new System.Drawing.Font("PMingLiU", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.dinBias.Increment = 1D;
            this.dinBias.Location = new System.Drawing.Point(126, 3);
            this.dinBias.MaxValue = 10D;
            this.dinBias.MinValue = -10D;
            this.dinBias.Name = "dinBias";
            this.dinBias.ShowUpDown = true;
            this.dinBias.Size = new System.Drawing.Size(125, 27);
            this.dinBias.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label10);
            this.panel1.Controls.Add(this.dinSysGain);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Font = new System.Drawing.Font("PMingLiU", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.panel1.ForeColor = System.Drawing.Color.Black;
            this.panel1.Location = new System.Drawing.Point(0, 129);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(360, 32);
            this.panel1.TabIndex = 7;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("PMingLiU", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label10.Location = new System.Drawing.Point(3, 4);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(83, 19);
            this.label10.TabIndex = 1;
            this.label10.Text = "Sys Gain";
            // 
            // dinSysGain
            // 
            // 
            // 
            // 
            this.dinSysGain.BackgroundStyle.Class = "DateTimeInputBackground";
            this.dinSysGain.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.dinSysGain.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.dinSysGain.DisplayFormat = "E3";
            this.dinSysGain.Font = new System.Drawing.Font("PMingLiU", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.dinSysGain.Increment = 1D;
            this.dinSysGain.Location = new System.Drawing.Point(126, 3);
            this.dinSysGain.MaxValue = 1000D;
            this.dinSysGain.MinValue = 0D;
            this.dinSysGain.Name = "dinSysGain";
            this.dinSysGain.ShowUpDown = true;
            this.dinSysGain.Size = new System.Drawing.Size(125, 27);
            this.dinSysGain.TabIndex = 0;
            this.dinSysGain.Value = 1D;
            // 
            // pnlTarPow
            // 
            this.pnlTarPow.Controls.Add(this.label11);
            this.pnlTarPow.Controls.Add(this.dinCheckLim);
            this.pnlTarPow.Controls.Add(this.label7);
            this.pnlTarPow.Controls.Add(this.label2);
            this.pnlTarPow.Controls.Add(this.label3);
            this.pnlTarPow.Controls.Add(this.dinTarPow);
            this.pnlTarPow.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTarPow.Font = new System.Drawing.Font("PMingLiU", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.pnlTarPow.ForeColor = System.Drawing.Color.Black;
            this.pnlTarPow.Location = new System.Drawing.Point(0, 68);
            this.pnlTarPow.Name = "pnlTarPow";
            this.pnlTarPow.Size = new System.Drawing.Size(360, 61);
            this.pnlTarPow.TabIndex = 4;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("PMingLiU", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label11.Location = new System.Drawing.Point(257, 35);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(36, 19);
            this.label11.TabIndex = 6;
            this.label11.Text = "±%";
            // 
            // dinCheckLim
            // 
            // 
            // 
            // 
            this.dinCheckLim.BackgroundStyle.Class = "DateTimeInputBackground";
            this.dinCheckLim.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.dinCheckLim.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.dinCheckLim.DisplayFormat = "0";
            this.dinCheckLim.Font = new System.Drawing.Font("PMingLiU", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.dinCheckLim.Increment = 1D;
            this.dinCheckLim.Location = new System.Drawing.Point(126, 31);
            this.dinCheckLim.MaxValue = 1000D;
            this.dinCheckLim.MinValue = 20D;
            this.dinCheckLim.Name = "dinCheckLim";
            this.dinCheckLim.ShowUpDown = true;
            this.dinCheckLim.Size = new System.Drawing.Size(125, 27);
            this.dinCheckLim.TabIndex = 5;
            this.dinCheckLim.Value = 50D;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("PMingLiU", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label7.Location = new System.Drawing.Point(3, 35);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(99, 19);
            this.label7.TabIndex = 4;
            this.label7.Text = "Check Lim";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("PMingLiU", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label2.Location = new System.Drawing.Point(257, 4);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(27, 19);
            this.label2.TabIndex = 3;
            this.label2.Text = "W";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("PMingLiU", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label3.Location = new System.Drawing.Point(3, 4);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(62, 19);
            this.label3.TabIndex = 1;
            this.label3.Text = "Target";
            // 
            // dinTarPow
            // 
            // 
            // 
            // 
            this.dinTarPow.BackgroundStyle.Class = "DateTimeInputBackground";
            this.dinTarPow.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.dinTarPow.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.dinTarPow.DisplayFormat = "E3";
            this.dinTarPow.Font = new System.Drawing.Font("PMingLiU", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.dinTarPow.Increment = 1D;
            this.dinTarPow.Location = new System.Drawing.Point(126, 3);
            this.dinTarPow.MaxValue = 30D;
            this.dinTarPow.MinValue = -60D;
            this.dinTarPow.Name = "dinTarPow";
            this.dinTarPow.ShowUpDown = true;
            this.dinTarPow.Size = new System.Drawing.Size(125, 27);
            this.dinTarPow.TabIndex = 0;
            // 
            // btnMsrt
            // 
            this.btnMsrt.Font = new System.Drawing.Font("PMingLiU", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnMsrt.ForeColor = System.Drawing.Color.Black;
            this.btnMsrt.Location = new System.Drawing.Point(3, 0);
            this.btnMsrt.Name = "btnMsrt";
            this.btnMsrt.Size = new System.Drawing.Size(115, 31);
            this.btnMsrt.TabIndex = 5;
            this.btnMsrt.Text = "MsrtPower";
            this.btnMsrt.UseVisualStyleBackColor = true;
            this.btnMsrt.Click += new System.EventHandler(this.btnMsrt_Click);
            // 
            // pnlWaveLength
            // 
            this.pnlWaveLength.Controls.Add(this.label4);
            this.pnlWaveLength.Controls.Add(this.label1);
            this.pnlWaveLength.Controls.Add(this.dinWaveLength);
            this.pnlWaveLength.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlWaveLength.Font = new System.Drawing.Font("PMingLiU", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.pnlWaveLength.ForeColor = System.Drawing.Color.Black;
            this.pnlWaveLength.Location = new System.Drawing.Point(0, 34);
            this.pnlWaveLength.Name = "pnlWaveLength";
            this.pnlWaveLength.Size = new System.Drawing.Size(360, 34);
            this.pnlWaveLength.TabIndex = 6;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("PMingLiU", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label4.Location = new System.Drawing.Point(257, 4);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(34, 19);
            this.label4.TabIndex = 3;
            this.label4.Text = "nm";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("PMingLiU", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label1.Location = new System.Drawing.Point(3, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(112, 19);
            this.label1.TabIndex = 1;
            this.label1.Text = "Wave length";
            // 
            // dinWaveLength
            // 
            // 
            // 
            // 
            this.dinWaveLength.BackgroundStyle.Class = "DateTimeInputBackground";
            this.dinWaveLength.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.dinWaveLength.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.dinWaveLength.Font = new System.Drawing.Font("PMingLiU", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.dinWaveLength.Increment = 1D;
            this.dinWaveLength.Location = new System.Drawing.Point(126, 3);
            this.dinWaveLength.MaxValue = 2000D;
            this.dinWaveLength.MinValue = 300D;
            this.dinWaveLength.Name = "dinWaveLength";
            this.dinWaveLength.ShowUpDown = true;
            this.dinWaveLength.Size = new System.Drawing.Size(125, 27);
            this.dinWaveLength.TabIndex = 0;
            this.dinWaveLength.Value = 300D;
            // 
            // lblResultPow
            // 
            this.lblResultPow.AutoSize = true;
            this.lblResultPow.Font = new System.Drawing.Font("PMingLiU", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lblResultPow.Location = new System.Drawing.Point(124, 6);
            this.lblResultPow.Name = "lblResultPow";
            this.lblResultPow.Size = new System.Drawing.Size(27, 19);
            this.lblResultPow.TabIndex = 7;
            this.lblResultPow.Text = "W";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.chkRecord);
            this.panel2.Controls.Add(this.btnMsrt);
            this.panel2.Controls.Add(this.lblResultPow);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Font = new System.Drawing.Font("PMingLiU", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.panel2.ForeColor = System.Drawing.Color.Black;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(360, 34);
            this.panel2.TabIndex = 8;
            // 
            // chkRecord
            // 
            this.chkRecord.AutoSize = true;
            this.chkRecord.Location = new System.Drawing.Point(240, 7);
            this.chkRecord.Name = "chkRecord";
            this.chkRecord.Size = new System.Drawing.Size(72, 20);
            this.chkRecord.TabIndex = 9;
            this.chkRecord.Text = "Record";
            this.chkRecord.UseVisualStyleBackColor = true;
            this.chkRecord.Visible = false;
            // 
            // CmpClampA
            // 
            this.CmpClampA.EnableModifyUnit = true;
            this.CmpClampA.Location = new System.Drawing.Point(126, 2);
            this.CmpClampA.Name = "CmpClampA";
            this.CmpClampA.Size = new System.Drawing.Size(189, 32);
            this.CmpClampA.TabIndex = 9;
            // 
            // doubleInput1
            // 
            // 
            // 
            // 
            this.doubleInput1.BackgroundStyle.Class = "DateTimeInputBackground";
            this.doubleInput1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.doubleInput1.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.doubleInput1.DisplayFormat = "0.00";
            this.doubleInput1.Font = new System.Drawing.Font("PMingLiU", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.doubleInput1.Increment = 1D;
            this.doubleInput1.Location = new System.Drawing.Point(126, 1);
            this.doubleInput1.MaxValue = 10D;
            this.doubleInput1.MinValue = -10D;
            this.doubleInput1.Name = "doubleInput1";
            this.doubleInput1.ShowUpDown = true;
            this.doubleInput1.Size = new System.Drawing.Size(125, 27);
            this.doubleInput1.TabIndex = 0;
            // 
            // frmPowerMeterComp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(360, 300);
            this.Controls.Add(this.pnlSMU);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.pnlTarPow);
            this.Controls.Add(this.pnlWaveLength);
            this.Controls.Add(this.panel2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmPowerMeterComp";
            this.Text = "frmPowerMeterComp";
            this.pnlSMU.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dinPDGain)).EndInit();
            this.pnlClamp.ResumeLayout(false);
            this.pnlClamp.PerformLayout();
            this.pnlBias.ResumeLayout(false);
            this.pnlBias.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dinBias)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dinSysGain)).EndInit();
            this.pnlTarPow.ResumeLayout(false);
            this.pnlTarPow.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dinCheckLim)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dinTarPow)).EndInit();
            this.pnlWaveLength.ResumeLayout(false);
            this.pnlWaveLength.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dinWaveLength)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.doubleInput1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlSMU;
        private System.Windows.Forms.Panel pnlClamp;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Panel pnlBias;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private DevComponents.Editors.DoubleInput dinBias;
        private System.Windows.Forms.Panel pnlTarPow;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private DevComponents.Editors.DoubleInput dinTarPow;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label10;
        private DevComponents.Editors.DoubleInput dinSysGain;
        private System.Windows.Forms.Button btnMsrt;
        private System.Windows.Forms.Panel pnlWaveLength;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label1;
        private DevComponents.Editors.DoubleInput dinWaveLength;
        private System.Windows.Forms.Label lblResultPow;
        private System.Windows.Forms.Panel panel2;
        private GuiComponent.UnitA CmpClampA;
        private System.Windows.Forms.Label label11;
        private DevComponents.Editors.DoubleInput dinCheckLim;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.CheckBox chkRecord;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private DevComponents.Editors.DoubleInput dinPDGain;
        private DevComponents.Editors.DoubleInput doubleInput1;
    }
}
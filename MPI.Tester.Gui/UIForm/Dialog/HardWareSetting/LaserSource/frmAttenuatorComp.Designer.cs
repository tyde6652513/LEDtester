namespace MPI.Tester.Gui.UIForm.Dialog.HardWareSetting.LaserSource
{
    partial class frmAttenuatorComp
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
            this.numWaveLength = new DevComponents.Editors.DoubleInput();
            this.pnlWaveLength = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.pnlAttPower = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.numOutPower = new DevComponents.Editors.DoubleInput();
            this.chkPowerCtrl = new System.Windows.Forms.CheckBox();
            this.pnlPowerMonitor = new System.Windows.Forms.Panel();
            this.chkPowRec = new System.Windows.Forms.CheckBox();
            this.pnlPowerCtrlMode = new System.Windows.Forms.Panel();
            this.numAtt = new DevComponents.Editors.DoubleInput();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.pnlAtt = new System.Windows.Forms.Panel();
            this.btnMsrt = new System.Windows.Forms.Button();
            this.lblResultPow = new System.Windows.Forms.Label();
            this.pnlAttMsrt = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.numWaveLength)).BeginInit();
            this.pnlWaveLength.SuspendLayout();
            this.pnlAttPower.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numOutPower)).BeginInit();
            this.pnlPowerMonitor.SuspendLayout();
            this.pnlPowerCtrlMode.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numAtt)).BeginInit();
            this.pnlAtt.SuspendLayout();
            this.pnlAttMsrt.SuspendLayout();
            this.SuspendLayout();
            // 
            // numWaveLength
            // 
            // 
            // 
            // 
            this.numWaveLength.BackgroundStyle.Class = "DateTimeInputBackground";
            this.numWaveLength.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.numWaveLength.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.numWaveLength.Font = new System.Drawing.Font("PMingLiU", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.numWaveLength.Increment = 1D;
            this.numWaveLength.Location = new System.Drawing.Point(126, 3);
            this.numWaveLength.MaxValue = 2000D;
            this.numWaveLength.MinValue = 300D;
            this.numWaveLength.Name = "numWaveLength";
            this.numWaveLength.ShowUpDown = true;
            this.numWaveLength.Size = new System.Drawing.Size(125, 27);
            this.numWaveLength.TabIndex = 0;
            this.numWaveLength.Value = 300D;
            // 
            // pnlWaveLength
            // 
            this.pnlWaveLength.Controls.Add(this.label4);
            this.pnlWaveLength.Controls.Add(this.label1);
            this.pnlWaveLength.Controls.Add(this.numWaveLength);
            this.pnlWaveLength.Font = new System.Drawing.Font("PMingLiU", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.pnlWaveLength.ForeColor = System.Drawing.Color.Black;
            this.pnlWaveLength.Location = new System.Drawing.Point(12, 48);
            this.pnlWaveLength.Name = "pnlWaveLength";
            this.pnlWaveLength.Size = new System.Drawing.Size(310, 34);
            this.pnlWaveLength.TabIndex = 1;
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
            // pnlAttPower
            // 
            this.pnlAttPower.Controls.Add(this.label3);
            this.pnlAttPower.Controls.Add(this.label2);
            this.pnlAttPower.Controls.Add(this.numOutPower);
            this.pnlAttPower.Font = new System.Drawing.Font("PMingLiU", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.pnlAttPower.Location = new System.Drawing.Point(3, 37);
            this.pnlAttPower.Name = "pnlAttPower";
            this.pnlAttPower.Size = new System.Drawing.Size(310, 34);
            this.pnlAttPower.TabIndex = 2;
            this.pnlAttPower.Visible = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("PMingLiU", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label3.Location = new System.Drawing.Point(257, 7);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(27, 19);
            this.label3.TabIndex = 2;
            this.label3.Text = "W";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("PMingLiU", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label2.Location = new System.Drawing.Point(3, 7);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(96, 19);
            this.label2.TabIndex = 1;
            this.label2.Text = "Out Power";
            // 
            // numOutPower
            // 
            // 
            // 
            // 
            this.numOutPower.BackgroundStyle.Class = "DateTimeInputBackground";
            this.numOutPower.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.numOutPower.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.numOutPower.DisplayFormat = "E3";
            this.numOutPower.Font = new System.Drawing.Font("PMingLiU", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.numOutPower.Increment = 1D;
            this.numOutPower.Location = new System.Drawing.Point(126, 3);
            this.numOutPower.MaxValue = 1D;
            this.numOutPower.MinValue = 1E-09D;
            this.numOutPower.Name = "numOutPower";
            this.numOutPower.ShowUpDown = true;
            this.numOutPower.Size = new System.Drawing.Size(125, 27);
            this.numOutPower.TabIndex = 0;
            this.numOutPower.Value = 1E-09D;
            // 
            // chkPowerCtrl
            // 
            this.chkPowerCtrl.AutoSize = true;
            this.chkPowerCtrl.Font = new System.Drawing.Font("PMingLiU", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.chkPowerCtrl.Location = new System.Drawing.Point(3, 8);
            this.chkPowerCtrl.Name = "chkPowerCtrl";
            this.chkPowerCtrl.Size = new System.Drawing.Size(199, 23);
            this.chkPowerCtrl.TabIndex = 4;
            this.chkPowerCtrl.Text = "Power Control Mode";
            this.chkPowerCtrl.UseVisualStyleBackColor = true;
            this.chkPowerCtrl.CheckedChanged += new System.EventHandler(this.chkPower_CheckedChanged);
            // 
            // pnlPowerMonitor
            // 
            this.pnlPowerMonitor.Controls.Add(this.chkPowRec);
            this.pnlPowerMonitor.Font = new System.Drawing.Font("PMingLiU", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.pnlPowerMonitor.Location = new System.Drawing.Point(3, 75);
            this.pnlPowerMonitor.Name = "pnlPowerMonitor";
            this.pnlPowerMonitor.Size = new System.Drawing.Size(310, 34);
            this.pnlPowerMonitor.TabIndex = 5;
            this.pnlPowerMonitor.Visible = false;
            // 
            // chkPowRec
            // 
            this.chkPowRec.AutoSize = true;
            this.chkPowRec.Font = new System.Drawing.Font("PMingLiU", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.chkPowRec.Location = new System.Drawing.Point(3, 8);
            this.chkPowRec.Name = "chkPowRec";
            this.chkPowRec.Size = new System.Drawing.Size(143, 23);
            this.chkPowRec.TabIndex = 5;
            this.chkPowRec.Text = "Power Record";
            this.chkPowRec.UseVisualStyleBackColor = true;
            // 
            // pnlPowerCtrlMode
            // 
            this.pnlPowerCtrlMode.Controls.Add(this.chkPowerCtrl);
            this.pnlPowerCtrlMode.Controls.Add(this.pnlAttPower);
            this.pnlPowerCtrlMode.Controls.Add(this.pnlPowerMonitor);
            this.pnlPowerCtrlMode.Font = new System.Drawing.Font("PMingLiU", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.pnlPowerCtrlMode.ForeColor = System.Drawing.Color.Black;
            this.pnlPowerCtrlMode.Location = new System.Drawing.Point(9, 124);
            this.pnlPowerCtrlMode.Name = "pnlPowerCtrlMode";
            this.pnlPowerCtrlMode.Size = new System.Drawing.Size(320, 114);
            this.pnlPowerCtrlMode.TabIndex = 6;
            this.pnlPowerCtrlMode.Visible = false;
            // 
            // numAtt
            // 
            // 
            // 
            // 
            this.numAtt.BackgroundStyle.Class = "DateTimeInputBackground";
            this.numAtt.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.numAtt.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.numAtt.Font = new System.Drawing.Font("PMingLiU", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.numAtt.Increment = 1D;
            this.numAtt.Location = new System.Drawing.Point(126, 4);
            this.numAtt.MaxValue = 60D;
            this.numAtt.MinValue = 0D;
            this.numAtt.Name = "numAtt";
            this.numAtt.ShowUpDown = true;
            this.numAtt.Size = new System.Drawing.Size(125, 27);
            this.numAtt.TabIndex = 0;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("PMingLiU", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label6.Location = new System.Drawing.Point(3, 4);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(105, 19);
            this.label6.TabIndex = 1;
            this.label6.Text = "Attenuation";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("PMingLiU", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label5.Location = new System.Drawing.Point(257, 4);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(32, 19);
            this.label5.TabIndex = 2;
            this.label5.Text = "dB";
            // 
            // pnlAtt
            // 
            this.pnlAtt.Controls.Add(this.label5);
            this.pnlAtt.Controls.Add(this.label6);
            this.pnlAtt.Controls.Add(this.numAtt);
            this.pnlAtt.ForeColor = System.Drawing.Color.Black;
            this.pnlAtt.Location = new System.Drawing.Point(12, 84);
            this.pnlAtt.Name = "pnlAtt";
            this.pnlAtt.Size = new System.Drawing.Size(310, 34);
            this.pnlAtt.TabIndex = 3;
            // 
            // btnMsrt
            // 
            this.btnMsrt.Font = new System.Drawing.Font("PMingLiU", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnMsrt.ForeColor = System.Drawing.Color.Black;
            this.btnMsrt.Location = new System.Drawing.Point(17, 12);
            this.btnMsrt.Name = "btnMsrt";
            this.btnMsrt.Size = new System.Drawing.Size(115, 31);
            this.btnMsrt.TabIndex = 7;
            this.btnMsrt.Text = "MsrtPower";
            this.btnMsrt.UseVisualStyleBackColor = true;
            this.btnMsrt.Click += new System.EventHandler(this.btnMsrt_Click);
            // 
            // lblResultPow
            // 
            this.lblResultPow.AutoSize = true;
            this.lblResultPow.Font = new System.Drawing.Font("PMingLiU", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lblResultPow.Location = new System.Drawing.Point(3, 7);
            this.lblResultPow.Name = "lblResultPow";
            this.lblResultPow.Size = new System.Drawing.Size(47, 19);
            this.lblResultPow.TabIndex = 8;
            this.lblResultPow.Text = "dBm";
            // 
            // pnlAttMsrt
            // 
            this.pnlAttMsrt.Controls.Add(this.lblResultPow);
            this.pnlAttMsrt.Font = new System.Drawing.Font("PMingLiU", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.pnlAttMsrt.ForeColor = System.Drawing.Color.Black;
            this.pnlAttMsrt.Location = new System.Drawing.Point(138, 11);
            this.pnlAttMsrt.Name = "pnlAttMsrt";
            this.pnlAttMsrt.Size = new System.Drawing.Size(184, 34);
            this.pnlAttMsrt.TabIndex = 9;
            // 
            // frmAttenuatorComp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(400, 310);
            this.Controls.Add(this.btnMsrt);
            this.Controls.Add(this.pnlAttMsrt);
            this.Controls.Add(this.pnlPowerCtrlMode);
            this.Controls.Add(this.pnlAtt);
            this.Controls.Add(this.pnlWaveLength);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmAttenuatorComp";
            this.Text = "frmAttenuatorComp";
            ((System.ComponentModel.ISupportInitialize)(this.numWaveLength)).EndInit();
            this.pnlWaveLength.ResumeLayout(false);
            this.pnlWaveLength.PerformLayout();
            this.pnlAttPower.ResumeLayout(false);
            this.pnlAttPower.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numOutPower)).EndInit();
            this.pnlPowerMonitor.ResumeLayout(false);
            this.pnlPowerMonitor.PerformLayout();
            this.pnlPowerCtrlMode.ResumeLayout(false);
            this.pnlPowerCtrlMode.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numAtt)).EndInit();
            this.pnlAtt.ResumeLayout(false);
            this.pnlAtt.PerformLayout();
            this.pnlAttMsrt.ResumeLayout(false);
            this.pnlAttMsrt.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.Editors.DoubleInput numWaveLength;
        private DevComponents.Editors.DoubleInput numOutPower;
        private System.Windows.Forms.Panel pnlWaveLength;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel pnlAttPower;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox chkPowerCtrl;
        private System.Windows.Forms.Panel pnlPowerMonitor;
        private System.Windows.Forms.CheckBox chkPowRec;
        private System.Windows.Forms.Panel pnlPowerCtrlMode;
        private DevComponents.Editors.DoubleInput numAtt;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Panel pnlAtt;
        private System.Windows.Forms.Button btnMsrt;
        private System.Windows.Forms.Label lblResultPow;
        private System.Windows.Forms.Panel pnlAttMsrt;
    }
}
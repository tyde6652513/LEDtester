namespace MPI.Tester.Gui.UIForm.Dialog.HardWareSetting.LaserSource
{
    partial class frmAutoVOAComp
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
            this.label11 = new System.Windows.Forms.Label();
            this.dinTriggerLim = new DevComponents.Editors.DoubleInput();
            this.label7 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.dinTuneTolerance = new DevComponents.Editors.DoubleInput();
            this.label2 = new System.Windows.Forms.Label();
            this.btnAutoSetAtt = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dinTriggerLim)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dinTuneTolerance)).BeginInit();
            this.SuspendLayout();
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("PMingLiU", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label11.ForeColor = System.Drawing.Color.Black;
            this.label11.Location = new System.Drawing.Point(233, 16);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(36, 19);
            this.label11.TabIndex = 9;
            this.label11.Text = "±%";
            // 
            // dinTriggerLim
            // 
            // 
            // 
            // 
            this.dinTriggerLim.BackgroundStyle.Class = "DateTimeInputBackground";
            this.dinTriggerLim.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.dinTriggerLim.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.dinTriggerLim.DisplayFormat = "0";
            this.dinTriggerLim.Font = new System.Drawing.Font("PMingLiU", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.dinTriggerLim.ForeColor = System.Drawing.Color.Black;
            this.dinTriggerLim.Increment = 1D;
            this.dinTriggerLim.Location = new System.Drawing.Point(148, 12);
            this.dinTriggerLim.MaxValue = 100D;
            this.dinTriggerLim.MinValue = 2D;
            this.dinTriggerLim.Name = "dinTriggerLim";
            this.dinTriggerLim.ShowUpDown = true;
            this.dinTriggerLim.Size = new System.Drawing.Size(87, 27);
            this.dinTriggerLim.TabIndex = 8;
            this.dinTriggerLim.Value = 20D;
            this.dinTriggerLim.Validated += new System.EventHandler(this.dinTriggerLim_Validated);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("PMingLiU", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label7.ForeColor = System.Drawing.Color.Black;
            this.label7.Location = new System.Drawing.Point(5, 16);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(109, 19);
            this.label7.TabIndex = 7;
            this.label7.Text = "Trigger Lim";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("PMingLiU", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(5, 49);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(137, 19);
            this.label1.TabIndex = 10;
            this.label1.Text = "Tune Tolerance";
            // 
            // dinTuneTolerance
            // 
            // 
            // 
            // 
            this.dinTuneTolerance.BackgroundStyle.Class = "DateTimeInputBackground";
            this.dinTuneTolerance.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.dinTuneTolerance.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.dinTuneTolerance.DisplayFormat = "0";
            this.dinTuneTolerance.Font = new System.Drawing.Font("PMingLiU", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.dinTuneTolerance.ForeColor = System.Drawing.Color.Black;
            this.dinTuneTolerance.Increment = 1D;
            this.dinTuneTolerance.Location = new System.Drawing.Point(148, 45);
            this.dinTuneTolerance.MaxValue = 100D;
            this.dinTuneTolerance.MinValue = 1D;
            this.dinTuneTolerance.Name = "dinTuneTolerance";
            this.dinTuneTolerance.ShowUpDown = true;
            this.dinTuneTolerance.Size = new System.Drawing.Size(87, 27);
            this.dinTuneTolerance.TabIndex = 11;
            this.dinTuneTolerance.Value = 20D;
            this.dinTuneTolerance.Validated += new System.EventHandler(this.dinTuneTolerance_Validated);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("PMingLiU", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(233, 49);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(36, 19);
            this.label2.TabIndex = 12;
            this.label2.Text = "±%";
            // 
            // btnAutoSetAtt
            // 
            this.btnAutoSetAtt.Font = new System.Drawing.Font("PMingLiU", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnAutoSetAtt.ForeColor = System.Drawing.Color.Black;
            this.btnAutoSetAtt.Location = new System.Drawing.Point(275, 8);
            this.btnAutoSetAtt.Name = "btnAutoSetAtt";
            this.btnAutoSetAtt.Size = new System.Drawing.Size(57, 46);
            this.btnAutoSetAtt.TabIndex = 116;
            this.btnAutoSetAtt.Text = "Tune\r\nVOA";
            this.btnAutoSetAtt.UseVisualStyleBackColor = true;
            this.btnAutoSetAtt.Click += new System.EventHandler(this.btnAutoSetAtt_Click);
            // 
            // frmAutoVOAComp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(372, 120);
            this.Controls.Add(this.btnAutoSetAtt);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dinTuneTolerance);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.dinTriggerLim);
            this.Controls.Add(this.label7);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmAutoVOAComp";
            this.Text = "frmAutoVOAComp";
            ((System.ComponentModel.ISupportInitialize)(this.dinTriggerLim)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dinTuneTolerance)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label11;
        private DevComponents.Editors.DoubleInput dinTriggerLim;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label1;
        private DevComponents.Editors.DoubleInput dinTuneTolerance;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnAutoSetAtt;
    }
}
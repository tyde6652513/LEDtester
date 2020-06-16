namespace MPI.Tester.Gui.UIForm.Dialog.HardWareSetting
{
    partial class frmAttenuator
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
            this.pgdAttenuator = new System.Windows.Forms.PropertyGrid();
            this.btnMsrt = new System.Windows.Forms.Button();
            this.txtMsrt = new System.Windows.Forms.TextBox();
            this.btnSet = new System.Windows.Forms.Button();
            this.lblMsrtPowerUnit = new System.Windows.Forms.Label();
            this.pgdOutput = new System.Windows.Forms.PropertyGrid();
            this.SuspendLayout();
            // 
            // pgdAttenuator
            // 
            this.pgdAttenuator.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pgdAttenuator.Font = new System.Drawing.Font("PMingLiU", 12F);
            this.pgdAttenuator.Location = new System.Drawing.Point(0, 223);
            this.pgdAttenuator.Name = "pgdAttenuator";
            this.pgdAttenuator.PropertySort = System.Windows.Forms.PropertySort.Categorized;
            this.pgdAttenuator.Size = new System.Drawing.Size(545, 283);
            this.pgdAttenuator.TabIndex = 0;
            this.pgdAttenuator.ToolbarVisible = false;
            this.pgdAttenuator.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.pgdAttenuator_PropertyValueChanged);
            // 
            // btnMsrt
            // 
            this.btnMsrt.Font = new System.Drawing.Font("PMingLiU", 12F);
            this.btnMsrt.Location = new System.Drawing.Point(26, 12);
            this.btnMsrt.Name = "btnMsrt";
            this.btnMsrt.Size = new System.Drawing.Size(100, 35);
            this.btnMsrt.TabIndex = 5;
            this.btnMsrt.Text = "Measure";
            this.btnMsrt.UseVisualStyleBackColor = true;
            this.btnMsrt.Click += new System.EventHandler(this.btnMsrt_Click);
            // 
            // txtMsrt
            // 
            this.txtMsrt.Location = new System.Drawing.Point(156, 18);
            this.txtMsrt.Name = "txtMsrt";
            this.txtMsrt.Size = new System.Drawing.Size(100, 22);
            this.txtMsrt.TabIndex = 6;
            this.txtMsrt.Text = "0";
            // 
            // btnSet
            // 
            this.btnSet.Font = new System.Drawing.Font("PMingLiU", 12F);
            this.btnSet.Location = new System.Drawing.Point(435, 8);
            this.btnSet.Name = "btnSet";
            this.btnSet.Size = new System.Drawing.Size(75, 36);
            this.btnSet.TabIndex = 7;
            this.btnSet.Text = "Set";
            this.btnSet.UseVisualStyleBackColor = true;
            this.btnSet.Click += new System.EventHandler(this.btnSet_Click);
            // 
            // lblMsrtPowerUnit
            // 
            this.lblMsrtPowerUnit.AutoSize = true;
            this.lblMsrtPowerUnit.Font = new System.Drawing.Font("PMingLiU", 12F);
            this.lblMsrtPowerUnit.Location = new System.Drawing.Point(277, 21);
            this.lblMsrtPowerUnit.Name = "lblMsrtPowerUnit";
            this.lblMsrtPowerUnit.Size = new System.Drawing.Size(38, 16);
            this.lblMsrtPowerUnit.TabIndex = 11;
            this.lblMsrtPowerUnit.Text = "dBm";
            // 
            // pgdOutput
            // 
            this.pgdOutput.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pgdOutput.Font = new System.Drawing.Font("PMingLiU", 12F);
            this.pgdOutput.HelpVisible = false;
            this.pgdOutput.Location = new System.Drawing.Point(0, 104);
            this.pgdOutput.Name = "pgdOutput";
            this.pgdOutput.Size = new System.Drawing.Size(545, 119);
            this.pgdOutput.TabIndex = 12;
            this.pgdOutput.ToolbarVisible = false;
            // 
            // frmAttenuator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(545, 506);
            this.Controls.Add(this.pgdOutput);
            this.Controls.Add(this.lblMsrtPowerUnit);
            this.Controls.Add(this.btnSet);
            this.Controls.Add(this.txtMsrt);
            this.Controls.Add(this.btnMsrt);
            this.Controls.Add(this.pgdAttenuator);
            this.Name = "frmAttenuator";
            this.Text = "Attenuator Setting";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PropertyGrid pgdAttenuator;
        private System.Windows.Forms.Button btnMsrt;
        private System.Windows.Forms.TextBox txtMsrt;
        private System.Windows.Forms.Button btnSet;
        private System.Windows.Forms.Label lblMsrtPowerUnit;
        private System.Windows.Forms.PropertyGrid pgdOutput;
    }
}
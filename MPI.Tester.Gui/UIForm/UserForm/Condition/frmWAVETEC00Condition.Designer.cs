namespace MPI.Tester.Gui.UIForm.UserForm.Condition
{
    partial class frmWAVETEC00Condition
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
            this.cmbEdgeSensor = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnSet = new DevComponents.DotNetBar.ButtonX();
            this.txtProductType = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // cmbEdgeSensor
            // 
            this.cmbEdgeSensor.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbEdgeSensor.FormattingEnabled = true;
            this.cmbEdgeSensor.Location = new System.Drawing.Point(223, 12);
            this.cmbEdgeSensor.Name = "cmbEdgeSensor";
            this.cmbEdgeSensor.Size = new System.Drawing.Size(139, 27);
            this.cmbEdgeSensor.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("PMingLiU", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label1.Location = new System.Drawing.Point(29, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(148, 19);
            this.label1.TabIndex = 1;
            this.label1.Text = "Edge Sensor Name";
            // 
            // btnSet
            // 
            this.btnSet.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnSet.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnSet.Font = new System.Drawing.Font("PMingLiU", 14F);
            this.btnSet.Location = new System.Drawing.Point(591, 9);
            this.btnSet.Name = "btnSet";
            this.btnSet.Size = new System.Drawing.Size(79, 43);
            this.btnSet.Style = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
            this.btnSet.TabIndex = 2;
            this.btnSet.Text = "Set";
            this.btnSet.Click += new System.EventHandler(this.btnSet_Click);
            // 
            // txtProductType
            // 
            this.txtProductType.Font = new System.Drawing.Font("Arial", 12F);
            this.txtProductType.Location = new System.Drawing.Point(223, 51);
            this.txtProductType.Name = "txtProductType";
            this.txtProductType.Size = new System.Drawing.Size(139, 26);
            this.txtProductType.TabIndex = 472;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("PMingLiU", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label2.Location = new System.Drawing.Point(29, 54);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(113, 19);
            this.label2.TabIndex = 474;
            this.label2.Text = "Product Name";
            // 
            // frmWAVETEC00Condition
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.ClientSize = new System.Drawing.Size(714, 286);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtProductType);
            this.Controls.Add(this.btnSet);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmbEdgeSensor);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmWAVETEC00Condition";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "frmWAVETEC00Condition";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbEdgeSensor;
        private System.Windows.Forms.Label label1;
        private DevComponents.DotNetBar.ButtonX btnSet;
        private System.Windows.Forms.TextBox txtProductType;
        private System.Windows.Forms.Label label2;
    }
}
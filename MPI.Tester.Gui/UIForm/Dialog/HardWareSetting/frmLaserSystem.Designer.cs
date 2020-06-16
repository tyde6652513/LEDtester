namespace MPI.Tester.Gui.UIForm.Dialog.HardWareSetting
{
    partial class frmLaserSystem
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
            this.pgdLaserSys = new System.Windows.Forms.PropertyGrid();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // pgdLaserSys
            // 
            this.pgdLaserSys.Dock = System.Windows.Forms.DockStyle.Left;
            this.pgdLaserSys.Font = new System.Drawing.Font("PMingLiU", 12F);
            this.pgdLaserSys.Location = new System.Drawing.Point(0, 0);
            this.pgdLaserSys.Name = "pgdLaserSys";
            this.pgdLaserSys.Size = new System.Drawing.Size(371, 411);
            this.pgdLaserSys.TabIndex = 0;
            this.pgdLaserSys.ToolbarVisible = false;
            // 
            // btnSave
            // 
            this.btnSave.Font = new System.Drawing.Font("PMingLiU", 12F);
            this.btnSave.Location = new System.Drawing.Point(390, 22);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 30);
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Font = new System.Drawing.Font("PMingLiU", 12F);
            this.btnCancel.Location = new System.Drawing.Point(390, 64);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 30);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // frmLaserSystem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(477, 411);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.pgdLaserSys);
            this.Name = "frmLaserSystem";
            this.Text = "Laser Config";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PropertyGrid pgdLaserSys;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
    }
}
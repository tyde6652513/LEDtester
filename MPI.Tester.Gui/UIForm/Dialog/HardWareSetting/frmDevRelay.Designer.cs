namespace MPI.Tester.Gui.UIForm.Dialog.HardWareSetting
{
    partial class frmDevRelay
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dgvRealyCnt = new System.Windows.Forms.DataGridView();
            this.btnSave = new System.Windows.Forms.Button();
            this.colSN = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDeviceName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRelay = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCnt = new DevComponents.DotNetBar.Controls.DataGridViewDoubleInputColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRealyCnt)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvRealyCnt
            // 
            this.dgvRealyCnt.AllowUserToAddRows = false;
            this.dgvRealyCnt.AllowUserToDeleteRows = false;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvRealyCnt.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgvRealyCnt.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvRealyCnt.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colSN,
            this.colDeviceName,
            this.colRelay,
            this.colCnt});
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvRealyCnt.DefaultCellStyle = dataGridViewCellStyle4;
            this.dgvRealyCnt.Location = new System.Drawing.Point(12, 12);
            this.dgvRealyCnt.Name = "dgvRealyCnt";
            this.dgvRealyCnt.RowTemplate.Height = 24;
            this.dgvRealyCnt.Size = new System.Drawing.Size(494, 357);
            this.dgvRealyCnt.TabIndex = 0;
            // 
            // btnSave
            // 
            this.btnSave.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.Location = new System.Drawing.Point(527, 12);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(78, 34);
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // colSN
            // 
            this.colSN.HeaderText = "SN";
            this.colSN.Name = "colSN";
            this.colSN.Visible = false;
            // 
            // colDeviceName
            // 
            this.colDeviceName.FillWeight = 200F;
            this.colDeviceName.HeaderText = "Device";
            this.colDeviceName.Name = "colDeviceName";
            this.colDeviceName.ReadOnly = true;
            this.colDeviceName.Width = 200;
            // 
            // colRelay
            // 
            this.colRelay.FillWeight = 150F;
            this.colRelay.HeaderText = "RelayName";
            this.colRelay.Name = "colRelay";
            this.colRelay.ReadOnly = true;
            this.colRelay.Width = 150;
            // 
            // colCnt
            // 
            // 
            // 
            // 
            this.colCnt.BackgroundStyle.Class = "DataGridViewNumericBorder";
            this.colCnt.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.colCnt.DisplayFormat = "0";
            this.colCnt.HeaderText = "Count";
            this.colCnt.Increment = 1D;
            this.colCnt.MinValue = 0D;
            this.colCnt.Name = "colCnt";
            this.colCnt.ReadOnly = true;
            this.colCnt.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // frmDevRelay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(626, 377);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.dgvRealyCnt);
            this.Name = "frmDevRelay";
            this.Text = "frmDevRelay";
            ((System.ComponentModel.ISupportInitialize)(this.dgvRealyCnt)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvRealyCnt;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSN;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDeviceName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRelay;
        private DevComponents.DotNetBar.Controls.DataGridViewDoubleInputColumn colCnt;
    }
}
namespace MPI.Tester.Gui
{
    partial class frmItemSettingIO
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.grpItemCondition = new System.Windows.Forms.GroupBox();
            this.dgvIO = new DevComponents.DotNetBar.Controls.DataGridViewX();
            this.colPin = new DevComponents.DotNetBar.Controls.DataGridViewIntegerInputColumn();
            this.colEn = new DevComponents.DotNetBar.Controls.DataGridViewCheckBoxXColumn();
            this.colAct = new DevComponents.DotNetBar.Controls.DataGridViewLabelXColumn();
            this.colPulseWidth = new DevComponents.DotNetBar.Controls.DataGridViewDoubleInputColumn();
            this.colMark = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMode = new DevComponents.DotNetBar.Controls.DataGridViewComboBoxExColumn();
            this.colState = new DevComponents.DotNetBar.Controls.DataGridViewComboBoxExColumn();
            this.grpItemCondition.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvIO)).BeginInit();
            this.SuspendLayout();
            // 
            // grpItemCondition
            // 
            this.grpItemCondition.Controls.Add(this.dgvIO);
            this.grpItemCondition.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpItemCondition.Font = new System.Drawing.Font("Arial", 20.25F, System.Drawing.FontStyle.Bold);
            this.grpItemCondition.ForeColor = System.Drawing.Color.DimGray;
            this.grpItemCondition.Location = new System.Drawing.Point(0, 0);
            this.grpItemCondition.Name = "grpItemCondition";
            this.grpItemCondition.Size = new System.Drawing.Size(1044, 465);
            this.grpItemCondition.TabIndex = 1;
            this.grpItemCondition.TabStop = false;
            this.grpItemCondition.Text = "IO";
            // 
            // dgvIO
            // 
            this.dgvIO.AllowUserToAddRows = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Arial Narrow", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.DimGray;
            this.dgvIO.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvIO.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvIO.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvIO.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colPin,
            this.colEn,
            this.colAct,
            this.colPulseWidth,
            this.colMark,
            this.colMode,
            this.colState});
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Arial", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.Color.DimGray;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvIO.DefaultCellStyle = dataGridViewCellStyle4;
            this.dgvIO.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvIO.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(215)))), ((int)(((byte)(229)))));
            this.dgvIO.Location = new System.Drawing.Point(3, 35);
            this.dgvIO.MultiSelect = false;
            this.dgvIO.Name = "dgvIO";
            this.dgvIO.RowHeadersWidth = 30;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.Color.Black;
            this.dgvIO.RowsDefaultCellStyle = dataGridViewCellStyle5;
            this.dgvIO.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvIO.RowTemplate.Height = 30;
            this.dgvIO.Size = new System.Drawing.Size(1038, 427);
            this.dgvIO.TabIndex = 1;
            this.dgvIO.CellValidated += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvIO_CellValidated);
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
            this.colPin.FillWeight = 80F;
            this.colPin.HeaderText = "Pin";
            this.colPin.InputHorizontalAlignment = DevComponents.Editors.eHorizontalAlignment.Left;
            this.colPin.MaxValue = 256;
            this.colPin.MinValue = 0;
            this.colPin.Name = "colPin";
            this.colPin.ReadOnly = true;
            this.colPin.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colPin.Width = 80;
            // 
            // colEn
            // 
            this.colEn.Checked = true;
            this.colEn.CheckState = System.Windows.Forms.CheckState.Indeterminate;
            this.colEn.CheckValue = "N";
            this.colEn.HeaderText = "Enable";
            this.colEn.Name = "colEn";
            this.colEn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colEn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.colEn.TextColor = System.Drawing.Color.Black;
            this.colEn.Width = 120;
            // 
            // colAct
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.colAct.DefaultCellStyle = dataGridViewCellStyle3;
            this.colAct.FillWeight = 180F;
            this.colAct.HeaderText = "Action";
            this.colAct.Name = "colAct";
            this.colAct.ReadOnly = true;
            this.colAct.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colAct.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.colAct.Width = 180;
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
            this.colPulseWidth.FillWeight = 200F;
            this.colPulseWidth.HeaderText = "Pulse Width(S)";
            this.colPulseWidth.Increment = 1D;
            this.colPulseWidth.InputHorizontalAlignment = DevComponents.Editors.eHorizontalAlignment.Left;
            this.colPulseWidth.MaxValue = 10000D;
            this.colPulseWidth.MinValue = -1D;
            this.colPulseWidth.Name = "colPulseWidth";
            this.colPulseWidth.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colPulseWidth.Width = 200;
            // 
            // colMark
            // 
            this.colMark.FillWeight = 250F;
            this.colMark.HeaderText = "Mark";
            this.colMark.Name = "colMark";
            this.colMark.ReadOnly = true;
            this.colMark.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colMark.Width = 250;
            // 
            // colMode
            // 
            this.colMode.DropDownHeight = 106;
            this.colMode.DropDownWidth = 121;
            this.colMode.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.colMode.HeaderText = "Mode";
            this.colMode.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.colMode.IntegralHeight = false;
            this.colMode.ItemHeight = 17;
            this.colMode.Name = "colMode";
            this.colMode.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colMode.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.colMode.Visible = false;
            // 
            // colState
            // 
            this.colState.DropDownHeight = 106;
            this.colState.DropDownWidth = 121;
            this.colState.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.colState.HeaderText = "State";
            this.colState.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.colState.IntegralHeight = false;
            this.colState.ItemHeight = 17;
            this.colState.Name = "colState";
            this.colState.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.colState.Visible = false;
            // 
            // frmItemSettingIO
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1044, 465);
            this.Controls.Add(this.grpItemCondition);
            this.Name = "frmItemSettingIO";
            this.Text = "frmItemSettingIO";
            this.grpItemCondition.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvIO)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grpItemCondition;
        private DevComponents.DotNetBar.Controls.DataGridViewX dgvIO;
        private DevComponents.DotNetBar.Controls.DataGridViewIntegerInputColumn colPin;
        private DevComponents.DotNetBar.Controls.DataGridViewCheckBoxXColumn colEn;
        private DevComponents.DotNetBar.Controls.DataGridViewLabelXColumn colAct;
        private DevComponents.DotNetBar.Controls.DataGridViewDoubleInputColumn colPulseWidth;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMark;
        private DevComponents.DotNetBar.Controls.DataGridViewComboBoxExColumn colMode;
        private DevComponents.DotNetBar.Controls.DataGridViewComboBoxExColumn colState;

    }
}
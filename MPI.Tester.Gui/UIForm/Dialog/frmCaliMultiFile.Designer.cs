namespace MPI.Tester.Gui
{
    partial class frmMultiFile
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMultiFile));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.btnOpenMutiMsrtFile = new DevComponents.DotNetBar.ButtonX();
            this.listBox2 = new System.Windows.Forms.ListBox();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.btnMultiFileOK = new DevComponents.DotNetBar.ButtonX();
            this.btnLeave = new DevComponents.DotNetBar.ButtonX();
            this.dgvOpenFile = new DevComponents.DotNetBar.Controls.DataGridViewX();
            this.colGainOffsetNo = new DevComponents.DotNetBar.Controls.DataGridViewIntegerInputColumn();
            this.colGainOffsetName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colGainOffsetKeyName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnDeleteSingle = new DevComponents.DotNetBar.ButtonX();
            this.btnLoadStdFile = new DevComponents.DotNetBar.ButtonX();
            this.btnDeleteAllFile = new DevComponents.DotNetBar.ButtonX();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOpenFile)).BeginInit();
            this.SuspendLayout();
            // 
            // btnOpenMutiMsrtFile
            // 
            this.btnOpenMutiMsrtFile.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnOpenMutiMsrtFile.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.btnOpenMutiMsrtFile, "btnOpenMutiMsrtFile");
            this.btnOpenMutiMsrtFile.ForeColor = System.Drawing.Color.Black;
            this.btnOpenMutiMsrtFile.Image = ((System.Drawing.Image)(resources.GetObject("btnOpenMutiMsrtFile.Image")));
            this.btnOpenMutiMsrtFile.Name = "btnOpenMutiMsrtFile";
            this.btnOpenMutiMsrtFile.Shape = new DevComponents.DotNetBar.RoundRectangleShapeDescriptor(8);
            this.btnOpenMutiMsrtFile.Click += new System.EventHandler(this.btnOpenMutiMsrtFile_Click);
            // 
            // listBox2
            // 
            resources.ApplyResources(this.listBox2, "listBox2");
            this.listBox2.FormattingEnabled = true;
            this.listBox2.Name = "listBox2";
            // 
            // listBox1
            // 
            resources.ApplyResources(this.listBox1, "listBox1");
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Name = "listBox1";
            // 
            // btnMultiFileOK
            // 
            this.btnMultiFileOK.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnMultiFileOK.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.btnMultiFileOK, "btnMultiFileOK");
            this.btnMultiFileOK.ForeColor = System.Drawing.Color.Black;
            this.btnMultiFileOK.Image = global::MPI.Tester.Gui.Properties.Resources.btnDataOK;
            this.btnMultiFileOK.ImageFixedSize = new System.Drawing.Size(20, 20);
            this.btnMultiFileOK.Name = "btnMultiFileOK";
            this.btnMultiFileOK.Shape = new DevComponents.DotNetBar.RoundRectangleShapeDescriptor(8);
            this.btnMultiFileOK.Style = DevComponents.DotNetBar.eDotNetBarStyle.Windows7;
            this.btnMultiFileOK.Click += new System.EventHandler(this.btnMultiFileOK_Click);
            // 
            // btnLeave
            // 
            this.btnLeave.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnLeave.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.btnLeave, "btnLeave");
            this.btnLeave.ForeColor = System.Drawing.Color.Black;
            this.btnLeave.Name = "btnLeave";
            this.btnLeave.Shape = new DevComponents.DotNetBar.RoundRectangleShapeDescriptor(8);
            this.btnLeave.Style = DevComponents.DotNetBar.eDotNetBarStyle.Windows7;
            this.btnLeave.Click += new System.EventHandler(this.buttonX1_Click);
            // 
            // dgvOpenFile
            // 
            this.dgvOpenFile.AllowUserToAddRows = false;
            this.dgvOpenFile.AllowUserToResizeColumns = false;
            this.dgvOpenFile.AllowUserToResizeRows = false;
            this.dgvOpenFile.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvOpenFile.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvOpenFile.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvOpenFile.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colGainOffsetNo,
            this.colGainOffsetName,
            this.colGainOffsetKeyName});
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvOpenFile.DefaultCellStyle = dataGridViewCellStyle3;
            this.dgvOpenFile.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(215)))), ((int)(((byte)(229)))));
            resources.ApplyResources(this.dgvOpenFile, "dgvOpenFile");
            this.dgvOpenFile.Name = "dgvOpenFile";
            this.dgvOpenFile.RowHeadersVisible = false;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvOpenFile.RowsDefaultCellStyle = dataGridViewCellStyle4;
            this.dgvOpenFile.RowTemplate.Height = 24;
            this.dgvOpenFile.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            // 
            // colGainOffsetNo
            // 
            this.colGainOffsetNo.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            // 
            // 
            // 
            this.colGainOffsetNo.BackgroundStyle.Class = "DataGridViewNumericBorder";
            this.colGainOffsetNo.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.colGainOffsetNo.DefaultCellStyle = dataGridViewCellStyle2;
            this.colGainOffsetNo.FillWeight = 20F;
            resources.ApplyResources(this.colGainOffsetNo, "colGainOffsetNo");
            this.colGainOffsetNo.Name = "colGainOffsetNo";
            this.colGainOffsetNo.ReadOnly = true;
            // 
            // colGainOffsetName
            // 
            this.colGainOffsetName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            resources.ApplyResources(this.colGainOffsetName, "colGainOffsetName");
            this.colGainOffsetName.Name = "colGainOffsetName";
            this.colGainOffsetName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // colGainOffsetKeyName
            // 
            this.colGainOffsetKeyName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            resources.ApplyResources(this.colGainOffsetKeyName, "colGainOffsetKeyName");
            this.colGainOffsetKeyName.Name = "colGainOffsetKeyName";
            this.colGainOffsetKeyName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // btnDeleteSingle
            // 
            this.btnDeleteSingle.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnDeleteSingle.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.btnDeleteSingle, "btnDeleteSingle");
            this.btnDeleteSingle.ForeColor = System.Drawing.Color.Black;
            this.btnDeleteSingle.Image = ((System.Drawing.Image)(resources.GetObject("btnDeleteSingle.Image")));
            this.btnDeleteSingle.ImageFixedSize = new System.Drawing.Size(18, 18);
            this.btnDeleteSingle.Name = "btnDeleteSingle";
            this.btnDeleteSingle.Shape = new DevComponents.DotNetBar.RoundRectangleShapeDescriptor(8);
            this.btnDeleteSingle.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
            this.btnDeleteSingle.Click += new System.EventHandler(this.btnDeleteSingle_Click);
            // 
            // btnLoadStdFile
            // 
            this.btnLoadStdFile.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnLoadStdFile.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.btnLoadStdFile, "btnLoadStdFile");
            this.btnLoadStdFile.ForeColor = System.Drawing.Color.Black;
            this.btnLoadStdFile.Image = ((System.Drawing.Image)(resources.GetObject("btnLoadStdFile.Image")));
            this.btnLoadStdFile.Name = "btnLoadStdFile";
            this.btnLoadStdFile.Shape = new DevComponents.DotNetBar.RoundRectangleShapeDescriptor(8);
            this.btnLoadStdFile.Click += new System.EventHandler(this.btnLoadStdFile_Click);
            // 
            // btnDeleteAllFile
            // 
            this.btnDeleteAllFile.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnDeleteAllFile.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.btnDeleteAllFile, "btnDeleteAllFile");
            this.btnDeleteAllFile.ForeColor = System.Drawing.Color.Black;
            this.btnDeleteAllFile.Image = ((System.Drawing.Image)(resources.GetObject("btnDeleteAllFile.Image")));
            this.btnDeleteAllFile.ImageFixedSize = new System.Drawing.Size(18, 18);
            this.btnDeleteAllFile.Name = "btnDeleteAllFile";
            this.btnDeleteAllFile.Shape = new DevComponents.DotNetBar.RoundRectangleShapeDescriptor(8);
            this.btnDeleteAllFile.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
            this.btnDeleteAllFile.Click += new System.EventHandler(this.btnDeleteAllFile_Click);
            // 
            // frmMultiFile
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.btnDeleteAllFile);
            this.Controls.Add(this.btnLoadStdFile);
            this.Controls.Add(this.btnOpenMutiMsrtFile);
            this.Controls.Add(this.btnDeleteSingle);
            this.Controls.Add(this.dgvOpenFile);
            this.Controls.Add(this.btnLeave);
            this.Controls.Add(this.btnMultiFileOK);
            this.Controls.Add(this.listBox2);
            this.Controls.Add(this.listBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "frmMultiFile";
            this.Load += new System.EventHandler(this.CaliMultiFile_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvOpenFile)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.ButtonX btnOpenMutiMsrtFile;
        private System.Windows.Forms.ListBox listBox2;
        private System.Windows.Forms.ListBox listBox1;
        private DevComponents.DotNetBar.ButtonX btnMultiFileOK;
        private DevComponents.DotNetBar.ButtonX btnLeave;
        private DevComponents.DotNetBar.Controls.DataGridViewX dgvOpenFile;
        private DevComponents.DotNetBar.Controls.DataGridViewIntegerInputColumn colGainOffsetNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGainOffsetName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGainOffsetKeyName;
        private DevComponents.DotNetBar.ButtonX btnDeleteSingle;
        private DevComponents.DotNetBar.ButtonX btnLoadStdFile;
        private DevComponents.DotNetBar.ButtonX btnDeleteAllFile;
    }
}
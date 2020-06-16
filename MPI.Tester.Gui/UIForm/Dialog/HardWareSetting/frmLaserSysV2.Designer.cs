namespace MPI.Tester.Gui.UIForm.Dialog.HardWareSetting
{
    partial class frmLaserSysV2
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
            this.dgvLaserSys = new System.Windows.Forms.DataGridView();
            this.colSysCh = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDefault = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnDelet = new System.Windows.Forms.Button();
            this.pgdChSet = new System.Windows.Forms.PropertyGrid();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnExport = new System.Windows.Forms.Button();
            this.btnLoad = new System.Windows.Forms.Button();
            this.numAutoAttPerCheck = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.grbAutoAtt = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLaserSys)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numAutoAttPerCheck)).BeginInit();
            this.grbAutoAtt.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvLaserSys
            // 
            this.dgvLaserSys.AllowUserToAddRows = false;
            this.dgvLaserSys.AllowUserToDeleteRows = false;
            this.dgvLaserSys.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvLaserSys.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colSysCh,
            this.colDefault,
            this.colName});
            this.dgvLaserSys.Location = new System.Drawing.Point(2, 12);
            this.dgvLaserSys.Name = "dgvLaserSys";
            this.dgvLaserSys.RowTemplate.Height = 24;
            this.dgvLaserSys.Size = new System.Drawing.Size(289, 327);
            this.dgvLaserSys.TabIndex = 0;
            this.dgvLaserSys.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvLaserSys_CellClick);
            // 
            // colSysCh
            // 
            this.colSysCh.FillWeight = 50F;
            this.colSysCh.HeaderText = "Ch";
            this.colSysCh.Name = "colSysCh";
            this.colSysCh.ReadOnly = true;
            this.colSysCh.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colSysCh.Width = 50;
            // 
            // colDefault
            // 
            this.colDefault.FillWeight = 50F;
            this.colDefault.HeaderText = "Default";
            this.colDefault.Name = "colDefault";
            this.colDefault.Width = 50;
            // 
            // colName
            // 
            this.colName.HeaderText = "Name";
            this.colName.Name = "colName";
            this.colName.ReadOnly = true;
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(317, 35);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 23);
            this.btnAdd.TabIndex = 1;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnDelet
            // 
            this.btnDelet.Location = new System.Drawing.Point(317, 90);
            this.btnDelet.Name = "btnDelet";
            this.btnDelet.Size = new System.Drawing.Size(75, 23);
            this.btnDelet.TabIndex = 2;
            this.btnDelet.Text = "Delet";
            this.btnDelet.UseVisualStyleBackColor = true;
            this.btnDelet.Click += new System.EventHandler(this.btnDelet_Click);
            // 
            // pgdChSet
            // 
            this.pgdChSet.Location = new System.Drawing.Point(470, 12);
            this.pgdChSet.Name = "pgdChSet";
            this.pgdChSet.Size = new System.Drawing.Size(318, 327);
            this.pgdChSet.TabIndex = 3;
            this.pgdChSet.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.pgdChSet_PropertyValueChanged);
            // 
            // btnSave
            // 
            this.btnSave.Font = new System.Drawing.Font("PMingLiU", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnSave.Location = new System.Drawing.Point(196, 372);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(95, 35);
            this.btnSave.TabIndex = 4;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnExport
            // 
            this.btnExport.Location = new System.Drawing.Point(607, 354);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(75, 23);
            this.btnExport.TabIndex = 5;
            this.btnExport.Text = "Export";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // btnLoad
            // 
            this.btnLoad.Location = new System.Drawing.Point(713, 354);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(75, 23);
            this.btnLoad.TabIndex = 6;
            this.btnLoad.Text = "Load";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // numAutoAttPerCheck
            // 
            this.numAutoAttPerCheck.Location = new System.Drawing.Point(6, 31);
            this.numAutoAttPerCheck.Name = "numAutoAttPerCheck";
            this.numAutoAttPerCheck.Size = new System.Drawing.Size(120, 22);
            this.numAutoAttPerCheck.TabIndex = 8;
            this.numAutoAttPerCheck.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("PMingLiU", 12F);
            this.label1.Location = new System.Drawing.Point(3, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(148, 16);
            this.label1.TabIndex = 9;
            this.label1.Text = "Auto VOA Per Check:";
            // 
            // grbAutoAtt
            // 
            this.grbAutoAtt.Controls.Add(this.label1);
            this.grbAutoAtt.Controls.Add(this.numAutoAttPerCheck);
            this.grbAutoAtt.Location = new System.Drawing.Point(297, 372);
            this.grbAutoAtt.Name = "grbAutoAtt";
            this.grbAutoAtt.Size = new System.Drawing.Size(173, 73);
            this.grbAutoAtt.TabIndex = 10;
            this.grbAutoAtt.TabStop = false;
            this.grbAutoAtt.Text = "Auto Set Att";
            // 
            // frmLaserSysV2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.grbAutoAtt);
            this.Controls.Add(this.btnLoad);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.pgdChSet);
            this.Controls.Add(this.btnDelet);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.dgvLaserSys);
            this.Name = "frmLaserSysV2";
            this.Text = "frmLaserSysV2";
            ((System.ComponentModel.ISupportInitialize)(this.dgvLaserSys)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numAutoAttPerCheck)).EndInit();
            this.grbAutoAtt.ResumeLayout(false);
            this.grbAutoAtt.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvLaserSys;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColOSModel;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnDelet;
        private System.Windows.Forms.PropertyGrid pgdChSet;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSysCh;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colDefault;
        private System.Windows.Forms.DataGridViewTextBoxColumn colName;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.NumericUpDown numAutoAttPerCheck;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox grbAutoAtt;
    }
}
namespace MPI.Tester.Gui
{
    partial class frmChannelCali2
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmChannelCali2));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle25 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle27 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle28 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle26 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle29 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle31 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle32 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle30 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle33 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle35 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle36 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle34 = new System.Windows.Forms.DataGridViewCellStyle();
            this.buttonItem1 = new DevComponents.DotNetBar.ButtonItem();
            this.btnCalcuate = new DevComponents.DotNetBar.ButtonX();
            this.btnLoadStd = new DevComponents.DotNetBar.ButtonX();
            this.btnLoadMsrt = new DevComponents.DotNetBar.ButtonX();
            this.txtStdPathAndFile = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.txtMsrtPathAndFile = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.plShowBoxPlot = new System.Windows.Forms.Panel();
            this.dgvCHInfo = new DevComponents.DotNetBar.Controls.DataGridViewX();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewDoubleInputColumn6 = new DevComponents.DotNetBar.Controls.DataGridViewDoubleInputColumn();
            this.dataGridViewDoubleInputColumn9 = new DevComponents.DotNetBar.Controls.DataGridViewDoubleInputColumn();
            this.dgvFilterSpec = new DevComponents.DotNetBar.Controls.DataGridViewX();
            this.dataGridViewCheckBoxColumn1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.min = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new DevComponents.DotNetBar.Controls.DataGridViewIntegerInputColumn();
            this.listTitle = new System.Windows.Forms.ListBox();
            this.btnCalcChannelGain = new DevComponents.DotNetBar.ButtonX();
            this.cmbItemSelect = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.dgvSysChannelFactor = new DevComponents.DotNetBar.Controls.DataGridViewX();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewDoubleInputColumn7 = new DevComponents.DotNetBar.Controls.DataGridViewDoubleInputColumn();
            this.dataGridViewDoubleInputColumn8 = new DevComponents.DotNetBar.Controls.DataGridViewDoubleInputColumn();
            this.btnSaveToProduct = new DevComponents.DotNetBar.ButtonX();
            this.btnCombine = new DevComponents.DotNetBar.ButtonX();
            this.btnSetFilter = new DevComponents.DotNetBar.ButtonX();
            this.txtSameRowColCount = new System.Windows.Forms.MaskedTextBox();
            this.txtFilteredCount = new System.Windows.Forms.MaskedTextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCHInfo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvFilterSpec)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSysChannelFactor)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonItem1
            // 
            this.buttonItem1.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText;
            this.buttonItem1.FixedSize = new System.Drawing.Size(100, 70);
            this.buttonItem1.Image = global::MPI.Tester.Gui.Properties.Resources.btnSelectSource;
            this.buttonItem1.ImageFixedSize = new System.Drawing.Size(28, 28);
            this.buttonItem1.ImagePosition = DevComponents.DotNetBar.eImagePosition.Top;
            this.buttonItem1.ItemAlignment = DevComponents.DotNetBar.eItemAlignment.Center;
            this.buttonItem1.Name = "buttonItem1";
            resources.ApplyResources(this.buttonItem1, "buttonItem1");
            // 
            // btnCalcuate
            // 
            this.btnCalcuate.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnCalcuate.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            resources.ApplyResources(this.btnCalcuate, "btnCalcuate");
            this.btnCalcuate.ForeColor = System.Drawing.Color.Black;
            this.btnCalcuate.Image = global::MPI.Tester.Gui.Properties.Resources.btnUpdateItem;
            this.btnCalcuate.ImageFixedSize = new System.Drawing.Size(20, 20);
            this.btnCalcuate.Name = "btnCalcuate";
            this.btnCalcuate.Shape = new DevComponents.DotNetBar.RoundRectangleShapeDescriptor(8);
            this.btnCalcuate.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnCalcuate.Click += new System.EventHandler(this.btnCalcuate_Click);
            // 
            // btnLoadStd
            // 
            this.btnLoadStd.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnLoadStd.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            resources.ApplyResources(this.btnLoadStd, "btnLoadStd");
            this.btnLoadStd.ForeColor = System.Drawing.Color.Black;
            this.btnLoadStd.Image = global::MPI.Tester.Gui.Properties.Resources.btnTestItem_32;
            this.btnLoadStd.ImageFixedSize = new System.Drawing.Size(20, 20);
            this.btnLoadStd.Name = "btnLoadStd";
            this.btnLoadStd.Shape = new DevComponents.DotNetBar.RoundRectangleShapeDescriptor(5);
            this.btnLoadStd.Style = DevComponents.DotNetBar.eDotNetBarStyle.Windows7;
            this.btnLoadStd.Click += new System.EventHandler(this.btnLoadStd_Click);
            // 
            // btnLoadMsrt
            // 
            this.btnLoadMsrt.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnLoadMsrt.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            resources.ApplyResources(this.btnLoadMsrt, "btnLoadMsrt");
            this.btnLoadMsrt.ForeColor = System.Drawing.Color.Black;
            this.btnLoadMsrt.Image = global::MPI.Tester.Gui.Properties.Resources.btnTestItem_32;
            this.btnLoadMsrt.ImageFixedSize = new System.Drawing.Size(20, 20);
            this.btnLoadMsrt.Name = "btnLoadMsrt";
            this.btnLoadMsrt.Shape = new DevComponents.DotNetBar.RoundRectangleShapeDescriptor(5);
            this.btnLoadMsrt.Style = DevComponents.DotNetBar.eDotNetBarStyle.Windows7;
            this.btnLoadMsrt.Click += new System.EventHandler(this.btnLoadMsrt_Click);
            // 
            // txtStdPathAndFile
            // 
            this.txtStdPathAndFile.BackColor = System.Drawing.SystemColors.Window;
            // 
            // 
            // 
            this.txtStdPathAndFile.Border.Class = "TextBoxBorder";
            this.txtStdPathAndFile.Border.CornerType = DevComponents.DotNetBar.eCornerType.Rounded;
            resources.ApplyResources(this.txtStdPathAndFile, "txtStdPathAndFile");
            this.txtStdPathAndFile.Name = "txtStdPathAndFile";
            this.txtStdPathAndFile.ReadOnly = true;
            // 
            // txtMsrtPathAndFile
            // 
            this.txtMsrtPathAndFile.BackColor = System.Drawing.SystemColors.Window;
            // 
            // 
            // 
            this.txtMsrtPathAndFile.Border.Class = "TextBoxBorder";
            this.txtMsrtPathAndFile.Border.CornerType = DevComponents.DotNetBar.eCornerType.Rounded;
            resources.ApplyResources(this.txtMsrtPathAndFile, "txtMsrtPathAndFile");
            this.txtMsrtPathAndFile.Name = "txtMsrtPathAndFile";
            this.txtMsrtPathAndFile.ReadOnly = true;
            // 
            // plShowBoxPlot
            // 
            this.plShowBoxPlot.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.plShowBoxPlot, "plShowBoxPlot");
            this.plShowBoxPlot.Name = "plShowBoxPlot";
            // 
            // dgvCHInfo
            // 
            this.dgvCHInfo.AllowUserToAddRows = false;
            this.dgvCHInfo.AllowUserToDeleteRows = false;
            this.dgvCHInfo.AllowUserToResizeRows = false;
            this.dgvCHInfo.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle25.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle25.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle25.Font = new System.Drawing.Font("Arial", 9F);
            dataGridViewCellStyle25.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle25.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle25.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle25.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvCHInfo.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle25;
            this.dgvCHInfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCHInfo.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn3,
            this.dataGridViewDoubleInputColumn6,
            this.dataGridViewDoubleInputColumn9});
            dataGridViewCellStyle27.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle27.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle27.Font = new System.Drawing.Font("Arial", 9F);
            dataGridViewCellStyle27.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle27.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle27.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle27.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvCHInfo.DefaultCellStyle = dataGridViewCellStyle27;
            this.dgvCHInfo.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(215)))), ((int)(((byte)(229)))));
            resources.ApplyResources(this.dgvCHInfo, "dgvCHInfo");
            this.dgvCHInfo.MultiSelect = false;
            this.dgvCHInfo.Name = "dgvCHInfo";
            this.dgvCHInfo.RowHeadersVisible = false;
            dataGridViewCellStyle28.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvCHInfo.RowsDefaultCellStyle = dataGridViewCellStyle28;
            this.dgvCHInfo.RowTemplate.Height = 24;
            this.dgvCHInfo.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle26.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.dataGridViewTextBoxColumn3.DefaultCellStyle = dataGridViewCellStyle26;
            resources.ApplyResources(this.dataGridViewTextBoxColumn3, "dataGridViewTextBoxColumn3");
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            this.dataGridViewTextBoxColumn3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dataGridViewDoubleInputColumn6
            // 
            // 
            // 
            // 
            this.dataGridViewDoubleInputColumn6.BackgroundStyle.Class = "DataGridViewNumericBorder";
            this.dataGridViewDoubleInputColumn6.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.dataGridViewDoubleInputColumn6.DisplayFormat = "0.000";
            resources.ApplyResources(this.dataGridViewDoubleInputColumn6, "dataGridViewDoubleInputColumn6");
            this.dataGridViewDoubleInputColumn6.Increment = 1D;
            this.dataGridViewDoubleInputColumn6.Name = "dataGridViewDoubleInputColumn6";
            this.dataGridViewDoubleInputColumn6.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dataGridViewDoubleInputColumn9
            // 
            // 
            // 
            // 
            this.dataGridViewDoubleInputColumn9.BackgroundStyle.Class = "DataGridViewNumericBorder";
            this.dataGridViewDoubleInputColumn9.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.dataGridViewDoubleInputColumn9.DisplayFormat = "0.000";
            resources.ApplyResources(this.dataGridViewDoubleInputColumn9, "dataGridViewDoubleInputColumn9");
            this.dataGridViewDoubleInputColumn9.Increment = 1D;
            this.dataGridViewDoubleInputColumn9.Name = "dataGridViewDoubleInputColumn9";
            this.dataGridViewDoubleInputColumn9.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dgvFilterSpec
            // 
            this.dgvFilterSpec.AllowUserToAddRows = false;
            this.dgvFilterSpec.AllowUserToDeleteRows = false;
            this.dgvFilterSpec.AllowUserToResizeRows = false;
            this.dgvFilterSpec.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle29.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle29.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle29.Font = new System.Drawing.Font("Arial", 9F);
            dataGridViewCellStyle29.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle29.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle29.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle29.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvFilterSpec.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle29;
            this.dgvFilterSpec.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvFilterSpec.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewCheckBoxColumn1,
            this.dataGridViewTextBoxColumn5,
            this.min,
            this.Column1,
            this.Column2});
            dataGridViewCellStyle31.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle31.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle31.Font = new System.Drawing.Font("Arial", 9F);
            dataGridViewCellStyle31.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle31.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle31.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle31.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvFilterSpec.DefaultCellStyle = dataGridViewCellStyle31;
            this.dgvFilterSpec.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(215)))), ((int)(((byte)(229)))));
            resources.ApplyResources(this.dgvFilterSpec, "dgvFilterSpec");
            this.dgvFilterSpec.MultiSelect = false;
            this.dgvFilterSpec.Name = "dgvFilterSpec";
            this.dgvFilterSpec.RowHeadersVisible = false;
            dataGridViewCellStyle32.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvFilterSpec.RowsDefaultCellStyle = dataGridViewCellStyle32;
            this.dgvFilterSpec.RowTemplate.Height = 24;
            this.dgvFilterSpec.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            // 
            // dataGridViewCheckBoxColumn1
            // 
            this.dataGridViewCheckBoxColumn1.FillWeight = 60F;
            resources.ApplyResources(this.dataGridViewCheckBoxColumn1, "dataGridViewCheckBoxColumn1");
            this.dataGridViewCheckBoxColumn1.Name = "dataGridViewCheckBoxColumn1";
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle30.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.dataGridViewTextBoxColumn5.DefaultCellStyle = dataGridViewCellStyle30;
            resources.ApplyResources(this.dataGridViewTextBoxColumn5, "dataGridViewTextBoxColumn5");
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.ReadOnly = true;
            this.dataGridViewTextBoxColumn5.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // min
            // 
            resources.ApplyResources(this.min, "min");
            this.min.Name = "min";
            // 
            // Column1
            // 
            resources.ApplyResources(this.Column1, "Column1");
            this.Column1.Name = "Column1";
            // 
            // Column2
            // 
            // 
            // 
            // 
            this.Column2.BackgroundStyle.Class = "DataGridViewNumericBorder";
            this.Column2.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            resources.ApplyResources(this.Column2, "Column2");
            this.Column2.Name = "Column2";
            // 
            // listTitle
            // 
            this.listTitle.FormattingEnabled = true;
            resources.ApplyResources(this.listTitle, "listTitle");
            this.listTitle.Name = "listTitle";
            // 
            // btnCalcChannelGain
            // 
            this.btnCalcChannelGain.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnCalcChannelGain.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            resources.ApplyResources(this.btnCalcChannelGain, "btnCalcChannelGain");
            this.btnCalcChannelGain.ForeColor = System.Drawing.Color.Black;
            this.btnCalcChannelGain.ImageFixedSize = new System.Drawing.Size(20, 20);
            this.btnCalcChannelGain.Name = "btnCalcChannelGain";
            this.btnCalcChannelGain.Shape = new DevComponents.DotNetBar.RoundRectangleShapeDescriptor(8);
            this.btnCalcChannelGain.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnCalcChannelGain.Click += new System.EventHandler(this.btnCalcChannelGain_Click);
            // 
            // cmbItemSelect
            // 
            resources.ApplyResources(this.cmbItemSelect, "cmbItemSelect");
            this.cmbItemSelect.FormattingEnabled = true;
            this.cmbItemSelect.Name = "cmbItemSelect";
            this.cmbItemSelect.SelectedIndexChanged += new System.EventHandler(this.cmbItemSelect_SelectedIndexChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.dgvSysChannelFactor);
            this.groupBox1.Controls.Add(this.btnSaveToProduct);
            this.groupBox1.Controls.Add(this.btnCombine);
            this.groupBox1.Controls.Add(this.dgvCHInfo);
            this.groupBox1.Controls.Add(this.plShowBoxPlot);
            this.groupBox1.Controls.Add(this.cmbItemSelect);
            this.groupBox1.Controls.Add(this.btnCalcChannelGain);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // dgvSysChannelFactor
            // 
            this.dgvSysChannelFactor.AllowUserToAddRows = false;
            this.dgvSysChannelFactor.AllowUserToDeleteRows = false;
            this.dgvSysChannelFactor.AllowUserToResizeRows = false;
            this.dgvSysChannelFactor.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle33.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle33.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle33.Font = new System.Drawing.Font("Arial", 9F);
            dataGridViewCellStyle33.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle33.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle33.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle33.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvSysChannelFactor.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle33;
            this.dgvSysChannelFactor.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSysChannelFactor.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn6,
            this.dataGridViewDoubleInputColumn7,
            this.dataGridViewDoubleInputColumn8});
            dataGridViewCellStyle35.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle35.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle35.Font = new System.Drawing.Font("Arial", 9F);
            dataGridViewCellStyle35.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle35.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle35.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle35.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvSysChannelFactor.DefaultCellStyle = dataGridViewCellStyle35;
            this.dgvSysChannelFactor.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(215)))), ((int)(((byte)(229)))));
            resources.ApplyResources(this.dgvSysChannelFactor, "dgvSysChannelFactor");
            this.dgvSysChannelFactor.MultiSelect = false;
            this.dgvSysChannelFactor.Name = "dgvSysChannelFactor";
            this.dgvSysChannelFactor.RowHeadersVisible = false;
            dataGridViewCellStyle36.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvSysChannelFactor.RowsDefaultCellStyle = dataGridViewCellStyle36;
            this.dgvSysChannelFactor.RowTemplate.Height = 24;
            this.dgvSysChannelFactor.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle34.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.dataGridViewTextBoxColumn6.DefaultCellStyle = dataGridViewCellStyle34;
            resources.ApplyResources(this.dataGridViewTextBoxColumn6, "dataGridViewTextBoxColumn6");
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            this.dataGridViewTextBoxColumn6.ReadOnly = true;
            this.dataGridViewTextBoxColumn6.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dataGridViewDoubleInputColumn7
            // 
            // 
            // 
            // 
            this.dataGridViewDoubleInputColumn7.BackgroundStyle.Class = "DataGridViewNumericBorder";
            this.dataGridViewDoubleInputColumn7.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.dataGridViewDoubleInputColumn7.DisplayFormat = "0.000";
            resources.ApplyResources(this.dataGridViewDoubleInputColumn7, "dataGridViewDoubleInputColumn7");
            this.dataGridViewDoubleInputColumn7.Increment = 1D;
            this.dataGridViewDoubleInputColumn7.Name = "dataGridViewDoubleInputColumn7";
            this.dataGridViewDoubleInputColumn7.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dataGridViewDoubleInputColumn8
            // 
            // 
            // 
            // 
            this.dataGridViewDoubleInputColumn8.BackgroundStyle.Class = "DataGridViewNumericBorder";
            this.dataGridViewDoubleInputColumn8.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.dataGridViewDoubleInputColumn8.DisplayFormat = "0.000";
            resources.ApplyResources(this.dataGridViewDoubleInputColumn8, "dataGridViewDoubleInputColumn8");
            this.dataGridViewDoubleInputColumn8.Increment = 1D;
            this.dataGridViewDoubleInputColumn8.Name = "dataGridViewDoubleInputColumn8";
            this.dataGridViewDoubleInputColumn8.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // btnSaveToProduct
            // 
            this.btnSaveToProduct.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnSaveToProduct.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            resources.ApplyResources(this.btnSaveToProduct, "btnSaveToProduct");
            this.btnSaveToProduct.ForeColor = System.Drawing.Color.Black;
            this.btnSaveToProduct.Image = global::MPI.Tester.Gui.Properties.Resources.btnConfirm;
            this.btnSaveToProduct.ImageFixedSize = new System.Drawing.Size(20, 20);
            this.btnSaveToProduct.Name = "btnSaveToProduct";
            this.btnSaveToProduct.Shape = new DevComponents.DotNetBar.RoundRectangleShapeDescriptor(8);
            this.btnSaveToProduct.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnSaveToProduct.Click += new System.EventHandler(this.btnSaveToProduct_Click);
            // 
            // btnCombine
            // 
            this.btnCombine.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnCombine.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            resources.ApplyResources(this.btnCombine, "btnCombine");
            this.btnCombine.ForeColor = System.Drawing.Color.Black;
            this.btnCombine.Image = global::MPI.Tester.Gui.Properties.Resources.arrow_left;
            this.btnCombine.ImageFixedSize = new System.Drawing.Size(20, 20);
            this.btnCombine.Name = "btnCombine";
            this.btnCombine.Shape = new DevComponents.DotNetBar.RoundRectangleShapeDescriptor(8);
            this.btnCombine.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnCombine.Click += new System.EventHandler(this.btnCombine_Click);
            // 
            // btnSetFilter
            // 
            this.btnSetFilter.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnSetFilter.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            resources.ApplyResources(this.btnSetFilter, "btnSetFilter");
            this.btnSetFilter.ForeColor = System.Drawing.Color.Black;
            this.btnSetFilter.ImageFixedSize = new System.Drawing.Size(20, 20);
            this.btnSetFilter.Name = "btnSetFilter";
            this.btnSetFilter.Shape = new DevComponents.DotNetBar.RoundRectangleShapeDescriptor(8);
            this.btnSetFilter.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnSetFilter.Click += new System.EventHandler(this.btnSetFilter_Click);
            // 
            // txtSameRowColCount
            // 
            resources.ApplyResources(this.txtSameRowColCount, "txtSameRowColCount");
            this.txtSameRowColCount.Name = "txtSameRowColCount";
            // 
            // txtFilteredCount
            // 
            resources.ApplyResources(this.txtFilteredCount, "txtFilteredCount");
            this.txtFilteredCount.Name = "txtFilteredCount";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // frmChannelCali2
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtFilteredCount);
            this.Controls.Add(this.txtSameRowColCount);
            this.Controls.Add(this.btnSetFilter);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.dgvFilterSpec);
            this.Controls.Add(this.listTitle);
            this.Controls.Add(this.txtMsrtPathAndFile);
            this.Controls.Add(this.btnCalcuate);
            this.Controls.Add(this.txtStdPathAndFile);
            this.Controls.Add(this.btnLoadStd);
            this.Controls.Add(this.btnLoadMsrt);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmChannelCali2";
            this.TopMost = true;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmChannelCali_Closed);
            this.Load += new System.EventHandler(this.frmChannelCali_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvCHInfo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvFilterSpec)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSysChannelFactor)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevComponents.DotNetBar.ButtonItem buttonItem1;
        private DevComponents.DotNetBar.ButtonX btnCalcuate;
        private DevComponents.DotNetBar.ButtonX btnLoadMsrt;
        private DevComponents.DotNetBar.ButtonX btnLoadStd;
        private DevComponents.DotNetBar.Controls.TextBoxX txtStdPathAndFile;
        private DevComponents.DotNetBar.Controls.TextBoxX txtMsrtPathAndFile;
        private System.Windows.Forms.ListBox listTitle;
        private DevComponents.DotNetBar.ButtonX btnCalcChannelGain;
        private System.Windows.Forms.ComboBox cmbItemSelect;
        private DevComponents.DotNetBar.Controls.DataGridViewX dgvCHInfo;
        private DevComponents.DotNetBar.Controls.DataGridViewX dgvFilterSpec;
        private System.Windows.Forms.Panel plShowBoxPlot;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private DevComponents.DotNetBar.Controls.DataGridViewDoubleInputColumn dataGridViewDoubleInputColumn6;
        private DevComponents.DotNetBar.Controls.DataGridViewDoubleInputColumn dataGridViewDoubleInputColumn9;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.DataGridViewTextBoxColumn min;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private DevComponents.DotNetBar.Controls.DataGridViewIntegerInputColumn Column2;
        private DevComponents.DotNetBar.ButtonX btnSetFilter;
        private DevComponents.DotNetBar.Controls.DataGridViewX dgvSysChannelFactor;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private DevComponents.DotNetBar.Controls.DataGridViewDoubleInputColumn dataGridViewDoubleInputColumn7;
        private DevComponents.DotNetBar.Controls.DataGridViewDoubleInputColumn dataGridViewDoubleInputColumn8;
        private DevComponents.DotNetBar.ButtonX btnSaveToProduct;
        private DevComponents.DotNetBar.ButtonX btnCombine;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.MaskedTextBox txtSameRowColCount;
        private System.Windows.Forms.MaskedTextBox txtFilteredCount;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;

    }
}
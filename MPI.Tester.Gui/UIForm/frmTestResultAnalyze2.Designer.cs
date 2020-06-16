namespace MPI.Tester.Gui
{
	partial class frmTestResultAnalyze2
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose( bool disposing )
		{
			if ( disposing && ( components != null ) )
			{
				components.Dispose();
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmTestResultAnalyze2));
            this.gChartA = new ZedGraph.ZedGraphControl();
            this.gChartB = new ZedGraph.ZedGraphControl();
            this.lblChartItemA = new DevComponents.DotNetBar.LabelX();
            this.lblDataSourceA = new DevComponents.DotNetBar.LabelX();
            this.lblChartItemB = new DevComponents.DotNetBar.LabelX();
            this.lblDataSourceB = new DevComponents.DotNetBar.LabelX();
            this.cmbChartAItems = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.cmbChartADataSource = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.cmbChartBItems = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.cmbChartBDataSource = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.cmbChartABaseItems = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.lblBaseItem = new DevComponents.DotNetBar.LabelX();
            this.btnChartSave01 = new DevComponents.DotNetBar.ButtonX();
            this.btnExportToCSV = new DevComponents.DotNetBar.ButtonX();
            this.groupPanel24 = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblDelayTimeLIV = new DevComponents.DotNetBar.LabelX();
            this.labelX118 = new DevComponents.DotNetBar.LabelX();
            this.lblStepValueLIV = new DevComponents.DotNetBar.LabelX();
            this.lblMsrtRangeUnitLIV = new DevComponents.DotNetBar.LabelX();
            this.labelX117 = new DevComponents.DotNetBar.LabelX();
            this.dinStartValueLIV = new DevComponents.Editors.DoubleInput();
            this.dinMsrtRangeLIV = new DevComponents.Editors.DoubleInput();
            this.lblStartValueLIV = new DevComponents.DotNetBar.LabelX();
            this.lblMsrtRangeLIV = new DevComponents.DotNetBar.LabelX();
            this.dinForceTimeLIV = new DevComponents.Editors.DoubleInput();
            this.dinTurnOffTimeLIV = new DevComponents.Editors.DoubleInput();
            this.labelX110 = new DevComponents.DotNetBar.LabelX();
            this.lblMaxValueLIV = new DevComponents.DotNetBar.LabelX();
            this.dinForceDealyLIV = new DevComponents.Editors.DoubleInput();
            this.numFlatCountLIV = new DevComponents.Editors.IntegerInput();
            this.labelX109 = new DevComponents.DotNetBar.LabelX();
            this.lblFlatCountLIV = new DevComponents.DotNetBar.LabelX();
            this.lblEndValueLIV = new DevComponents.DotNetBar.LabelX();
            this.labelX95 = new DevComponents.DotNetBar.LabelX();
            this.lblRiseCountLIV = new DevComponents.DotNetBar.LabelX();
            this.labelX102 = new DevComponents.DotNetBar.LabelX();
            this.labelX98 = new DevComponents.DotNetBar.LabelX();
            this.dinStepValueLIV = new DevComponents.Editors.DoubleInput();
            this.numRiseCountLIV = new DevComponents.Editors.IntegerInput();
            this.labelX99 = new DevComponents.DotNetBar.LabelX();
            this.lblForceTimeLIV = new DevComponents.DotNetBar.LabelX();
            this.lblTurnOffTimeLIV = new DevComponents.DotNetBar.LabelX();
            this.lblLIVItem = new DevComponents.DotNetBar.LabelX();
            this.cmbLIVItem = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.btnEditLIV = new DevComponents.DotNetBar.ButtonX();
            this.labelX104 = new DevComponents.DotNetBar.LabelX();
            this.groupPanel24.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dinStartValueLIV)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dinMsrtRangeLIV)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dinForceTimeLIV)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dinTurnOffTimeLIV)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dinForceDealyLIV)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numFlatCountLIV)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dinStepValueLIV)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numRiseCountLIV)).BeginInit();
            this.SuspendLayout();
            // 
            // gChartA
            // 
            resources.ApplyResources(this.gChartA, "gChartA");
            this.gChartA.Name = "gChartA";
            this.gChartA.ScrollGrace = 0D;
            this.gChartA.ScrollMaxX = 0D;
            this.gChartA.ScrollMaxY = 0D;
            this.gChartA.ScrollMaxY2 = 0D;
            this.gChartA.ScrollMinX = 0D;
            this.gChartA.ScrollMinY = 0D;
            this.gChartA.ScrollMinY2 = 0D;
            // 
            // gChartB
            // 
            resources.ApplyResources(this.gChartB, "gChartB");
            this.gChartB.Name = "gChartB";
            this.gChartB.ScrollGrace = 0D;
            this.gChartB.ScrollMaxX = 0D;
            this.gChartB.ScrollMaxY = 0D;
            this.gChartB.ScrollMaxY2 = 0D;
            this.gChartB.ScrollMinX = 0D;
            this.gChartB.ScrollMinY = 0D;
            this.gChartB.ScrollMinY2 = 0D;
            // 
            // lblChartItemA
            // 
            // 
            // 
            // 
            this.lblChartItemA.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10015;
            this.lblChartItemA.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            resources.ApplyResources(this.lblChartItemA, "lblChartItemA");
            this.lblChartItemA.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.lblChartItemA.Name = "lblChartItemA";
            this.lblChartItemA.TextAlignment = System.Drawing.StringAlignment.Center;
            // 
            // lblDataSourceA
            // 
            // 
            // 
            // 
            this.lblDataSourceA.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10015;
            this.lblDataSourceA.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            resources.ApplyResources(this.lblDataSourceA, "lblDataSourceA");
            this.lblDataSourceA.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.lblDataSourceA.Name = "lblDataSourceA";
            this.lblDataSourceA.TextAlignment = System.Drawing.StringAlignment.Center;
            // 
            // lblChartItemB
            // 
            // 
            // 
            // 
            this.lblChartItemB.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10015;
            this.lblChartItemB.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            resources.ApplyResources(this.lblChartItemB, "lblChartItemB");
            this.lblChartItemB.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.lblChartItemB.Name = "lblChartItemB";
            this.lblChartItemB.TextAlignment = System.Drawing.StringAlignment.Center;
            // 
            // lblDataSourceB
            // 
            // 
            // 
            // 
            this.lblDataSourceB.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10015;
            this.lblDataSourceB.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            resources.ApplyResources(this.lblDataSourceB, "lblDataSourceB");
            this.lblDataSourceB.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.lblDataSourceB.Name = "lblDataSourceB";
            this.lblDataSourceB.TextAlignment = System.Drawing.StringAlignment.Center;
            // 
            // cmbChartAItems
            // 
            this.cmbChartAItems.DisplayMember = "Text";
            this.cmbChartAItems.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbChartAItems.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbChartAItems.FormattingEnabled = true;
            resources.ApplyResources(this.cmbChartAItems, "cmbChartAItems");
            this.cmbChartAItems.Name = "cmbChartAItems";
            this.cmbChartAItems.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.cmbChartAItems.SelectedIndexChanged += new System.EventHandler(this.cmbChartAItems_SelectedIndexChanged);
            // 
            // cmbChartADataSource
            // 
            this.cmbChartADataSource.DisplayMember = "Text";
            this.cmbChartADataSource.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbChartADataSource.FormattingEnabled = true;
            resources.ApplyResources(this.cmbChartADataSource, "cmbChartADataSource");
            this.cmbChartADataSource.Name = "cmbChartADataSource";
            this.cmbChartADataSource.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.cmbChartADataSource.SelectedIndexChanged += new System.EventHandler(this.cmbChartADataSource_SelectedIndexChanged);
            // 
            // cmbChartBItems
            // 
            this.cmbChartBItems.DisplayMember = "Text";
            this.cmbChartBItems.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbChartBItems.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbChartBItems.FormattingEnabled = true;
            resources.ApplyResources(this.cmbChartBItems, "cmbChartBItems");
            this.cmbChartBItems.Name = "cmbChartBItems";
            this.cmbChartBItems.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.cmbChartBItems.SelectedIndexChanged += new System.EventHandler(this.cmbChartBItems_SelectedIndexChanged);
            // 
            // cmbChartBDataSource
            // 
            this.cmbChartBDataSource.DisplayMember = "Text";
            this.cmbChartBDataSource.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbChartBDataSource.FormattingEnabled = true;
            resources.ApplyResources(this.cmbChartBDataSource, "cmbChartBDataSource");
            this.cmbChartBDataSource.Name = "cmbChartBDataSource";
            this.cmbChartBDataSource.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.cmbChartBDataSource.SelectedIndexChanged += new System.EventHandler(this.cmbChartBDataSource_SelectedIndexChanged);
            // 
            // cmbChartABaseItems
            // 
            this.cmbChartABaseItems.DisplayMember = "Text";
            this.cmbChartABaseItems.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbChartABaseItems.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbChartABaseItems.FormattingEnabled = true;
            resources.ApplyResources(this.cmbChartABaseItems, "cmbChartABaseItems");
            this.cmbChartABaseItems.Name = "cmbChartABaseItems";
            this.cmbChartABaseItems.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.cmbChartABaseItems.SelectedIndexChanged += new System.EventHandler(this.cmbChartABaseItems_SelectedIndexChanged);
            // 
            // lblBaseItem
            // 
            // 
            // 
            // 
            this.lblBaseItem.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10015;
            this.lblBaseItem.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            resources.ApplyResources(this.lblBaseItem, "lblBaseItem");
            this.lblBaseItem.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.lblBaseItem.Name = "lblBaseItem";
            this.lblBaseItem.TextAlignment = System.Drawing.StringAlignment.Center;
            // 
            // btnChartSave01
            // 
            this.btnChartSave01.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnChartSave01.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.btnChartSave01, "btnChartSave01");
            this.btnChartSave01.Name = "btnChartSave01";
            this.btnChartSave01.Shape = new DevComponents.DotNetBar.RoundRectangleShapeDescriptor(8);
            this.btnChartSave01.Style = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
            this.btnChartSave01.Click += new System.EventHandler(this.btnChartSave01_Click_1);
            // 
            // btnExportToCSV
            // 
            this.btnExportToCSV.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnExportToCSV.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.btnExportToCSV, "btnExportToCSV");
            this.btnExportToCSV.Image = global::MPI.Tester.Gui.Properties.Resources.btnSaveFile_B;
            this.btnExportToCSV.ImageFixedSize = new System.Drawing.Size(20, 20);
            this.btnExportToCSV.Name = "btnExportToCSV";
            this.btnExportToCSV.Shape = new DevComponents.DotNetBar.RoundRectangleShapeDescriptor(8);
            this.btnExportToCSV.Style = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
            this.btnExportToCSV.Click += new System.EventHandler(this.btnExportToCSV_Click);
            // 
            // groupPanel24
            // 
            this.groupPanel24.BackColor = System.Drawing.Color.Transparent;
            this.groupPanel24.CanvasColor = System.Drawing.Color.Empty;
            this.groupPanel24.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
            this.groupPanel24.Controls.Add(this.panel1);
            this.groupPanel24.Controls.Add(this.lblLIVItem);
            this.groupPanel24.Controls.Add(this.cmbLIVItem);
            this.groupPanel24.Controls.Add(this.btnEditLIV);
            this.groupPanel24.Controls.Add(this.labelX104);
            this.groupPanel24.DrawTitleBox = false;
            resources.ApplyResources(this.groupPanel24, "groupPanel24");
            this.groupPanel24.IsShadowEnabled = true;
            this.groupPanel24.Name = "groupPanel24";
            // 
            // 
            // 
            this.groupPanel24.Style.BackColor = System.Drawing.Color.WhiteSmoke;
            this.groupPanel24.Style.BackColor2 = System.Drawing.Color.Lavender;
            this.groupPanel24.Style.BackColorGradientAngle = 90;
            this.groupPanel24.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel24.Style.BorderBottomWidth = 1;
            this.groupPanel24.Style.BorderColor = System.Drawing.Color.Gray;
            this.groupPanel24.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel24.Style.BorderLeftWidth = 1;
            this.groupPanel24.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel24.Style.BorderRightWidth = 1;
            this.groupPanel24.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel24.Style.BorderTopWidth = 1;
            this.groupPanel24.Style.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10015;
            this.groupPanel24.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded;
            this.groupPanel24.Style.TextColor = System.Drawing.Color.DarkOrange;
            // 
            // 
            // 
            this.groupPanel24.StyleMouseDown.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10015;
            this.groupPanel24.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this.groupPanel24.StyleMouseOver.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10015;
            this.groupPanel24.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lblDelayTimeLIV);
            this.panel1.Controls.Add(this.labelX118);
            this.panel1.Controls.Add(this.lblStepValueLIV);
            this.panel1.Controls.Add(this.lblMsrtRangeUnitLIV);
            this.panel1.Controls.Add(this.labelX117);
            this.panel1.Controls.Add(this.dinStartValueLIV);
            this.panel1.Controls.Add(this.dinMsrtRangeLIV);
            this.panel1.Controls.Add(this.lblStartValueLIV);
            this.panel1.Controls.Add(this.lblMsrtRangeLIV);
            this.panel1.Controls.Add(this.dinForceTimeLIV);
            this.panel1.Controls.Add(this.dinTurnOffTimeLIV);
            this.panel1.Controls.Add(this.labelX110);
            this.panel1.Controls.Add(this.lblMaxValueLIV);
            this.panel1.Controls.Add(this.dinForceDealyLIV);
            this.panel1.Controls.Add(this.numFlatCountLIV);
            this.panel1.Controls.Add(this.labelX109);
            this.panel1.Controls.Add(this.lblFlatCountLIV);
            this.panel1.Controls.Add(this.lblEndValueLIV);
            this.panel1.Controls.Add(this.labelX95);
            this.panel1.Controls.Add(this.lblRiseCountLIV);
            this.panel1.Controls.Add(this.labelX102);
            this.panel1.Controls.Add(this.labelX98);
            this.panel1.Controls.Add(this.dinStepValueLIV);
            this.panel1.Controls.Add(this.numRiseCountLIV);
            this.panel1.Controls.Add(this.labelX99);
            this.panel1.Controls.Add(this.lblForceTimeLIV);
            this.panel1.Controls.Add(this.lblTurnOffTimeLIV);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // lblDelayTimeLIV
            // 
            this.lblDelayTimeLIV.BackColor = System.Drawing.Color.PaleTurquoise;
            // 
            // 
            // 
            this.lblDelayTimeLIV.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10015;
            this.lblDelayTimeLIV.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            resources.ApplyResources(this.lblDelayTimeLIV, "lblDelayTimeLIV");
            this.lblDelayTimeLIV.ForeColor = System.Drawing.Color.Black;
            this.lblDelayTimeLIV.Name = "lblDelayTimeLIV";
            // 
            // labelX118
            // 
            resources.ApplyResources(this.labelX118, "labelX118");
            this.labelX118.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX118.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10015;
            this.labelX118.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX118.ForeColor = System.Drawing.Color.Black;
            this.labelX118.Name = "labelX118";
            // 
            // lblStepValueLIV
            // 
            this.lblStepValueLIV.BackColor = System.Drawing.Color.PaleTurquoise;
            // 
            // 
            // 
            this.lblStepValueLIV.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10015;
            this.lblStepValueLIV.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            resources.ApplyResources(this.lblStepValueLIV, "lblStepValueLIV");
            this.lblStepValueLIV.ForeColor = System.Drawing.Color.Black;
            this.lblStepValueLIV.Name = "lblStepValueLIV";
            // 
            // lblMsrtRangeUnitLIV
            // 
            resources.ApplyResources(this.lblMsrtRangeUnitLIV, "lblMsrtRangeUnitLIV");
            this.lblMsrtRangeUnitLIV.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.lblMsrtRangeUnitLIV.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10015;
            this.lblMsrtRangeUnitLIV.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblMsrtRangeUnitLIV.ForeColor = System.Drawing.Color.Black;
            this.lblMsrtRangeUnitLIV.Name = "lblMsrtRangeUnitLIV";
            // 
            // labelX117
            // 
            resources.ApplyResources(this.labelX117, "labelX117");
            this.labelX117.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX117.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10015;
            this.labelX117.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX117.ForeColor = System.Drawing.Color.Black;
            this.labelX117.Name = "labelX117";
            // 
            // dinStartValueLIV
            // 
            // 
            // 
            // 
            this.dinStartValueLIV.BackgroundStyle.Class = "DateTimeInputBackground";
            this.dinStartValueLIV.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.dinStartValueLIV.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.dinStartValueLIV.DisplayFormat = "0.0000";
            resources.ApplyResources(this.dinStartValueLIV, "dinStartValueLIV");
            this.dinStartValueLIV.Increment = 0.0001D;
            this.dinStartValueLIV.IsInputReadOnly = true;
            this.dinStartValueLIV.MaxValue = 10000D;
            this.dinStartValueLIV.MinValue = 0D;
            this.dinStartValueLIV.Name = "dinStartValueLIV";
            this.dinStartValueLIV.Value = 0.01D;
            // 
            // dinMsrtRangeLIV
            // 
            // 
            // 
            // 
            this.dinMsrtRangeLIV.BackgroundStyle.Class = "DateTimeInputBackground";
            this.dinMsrtRangeLIV.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.dinMsrtRangeLIV.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.dinMsrtRangeLIV.DisplayFormat = "0.0";
            resources.ApplyResources(this.dinMsrtRangeLIV, "dinMsrtRangeLIV");
            this.dinMsrtRangeLIV.Increment = 0.1D;
            this.dinMsrtRangeLIV.IsInputReadOnly = true;
            this.dinMsrtRangeLIV.MaxValue = 285D;
            this.dinMsrtRangeLIV.MinValue = 8D;
            this.dinMsrtRangeLIV.Name = "dinMsrtRangeLIV";
            this.dinMsrtRangeLIV.Value = 8D;
            // 
            // lblStartValueLIV
            // 
            this.lblStartValueLIV.BackColor = System.Drawing.Color.PaleTurquoise;
            // 
            // 
            // 
            this.lblStartValueLIV.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10015;
            this.lblStartValueLIV.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            resources.ApplyResources(this.lblStartValueLIV, "lblStartValueLIV");
            this.lblStartValueLIV.ForeColor = System.Drawing.Color.Black;
            this.lblStartValueLIV.Name = "lblStartValueLIV";
            // 
            // lblMsrtRangeLIV
            // 
            this.lblMsrtRangeLIV.BackColor = System.Drawing.Color.PaleTurquoise;
            // 
            // 
            // 
            this.lblMsrtRangeLIV.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10015;
            this.lblMsrtRangeLIV.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            resources.ApplyResources(this.lblMsrtRangeLIV, "lblMsrtRangeLIV");
            this.lblMsrtRangeLIV.ForeColor = System.Drawing.Color.Black;
            this.lblMsrtRangeLIV.Name = "lblMsrtRangeLIV";
            // 
            // dinForceTimeLIV
            // 
            // 
            // 
            // 
            this.dinForceTimeLIV.BackgroundStyle.Class = "DateTimeInputBackground";
            this.dinForceTimeLIV.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.dinForceTimeLIV.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.dinForceTimeLIV.DisplayFormat = "0.0";
            resources.ApplyResources(this.dinForceTimeLIV, "dinForceTimeLIV");
            this.dinForceTimeLIV.Increment = 0.5D;
            this.dinForceTimeLIV.IsInputReadOnly = true;
            this.dinForceTimeLIV.MaxValue = 10000D;
            this.dinForceTimeLIV.MinValue = 0D;
            this.dinForceTimeLIV.Name = "dinForceTimeLIV";
            this.dinForceTimeLIV.Value = 1D;
            // 
            // dinTurnOffTimeLIV
            // 
            // 
            // 
            // 
            this.dinTurnOffTimeLIV.BackgroundStyle.Class = "DateTimeInputBackground";
            this.dinTurnOffTimeLIV.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.dinTurnOffTimeLIV.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.dinTurnOffTimeLIV.DisplayFormat = "0.0";
            resources.ApplyResources(this.dinTurnOffTimeLIV, "dinTurnOffTimeLIV");
            this.dinTurnOffTimeLIV.Increment = 0.1D;
            this.dinTurnOffTimeLIV.IsInputReadOnly = true;
            this.dinTurnOffTimeLIV.MaxValue = 10000D;
            this.dinTurnOffTimeLIV.MinValue = 0D;
            this.dinTurnOffTimeLIV.Name = "dinTurnOffTimeLIV";
            // 
            // labelX110
            // 
            resources.ApplyResources(this.labelX110, "labelX110");
            this.labelX110.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX110.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10015;
            this.labelX110.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX110.ForeColor = System.Drawing.Color.Black;
            this.labelX110.Name = "labelX110";
            // 
            // lblMaxValueLIV
            // 
            this.lblMaxValueLIV.BackColor = System.Drawing.Color.White;
            // 
            // 
            // 
            this.lblMaxValueLIV.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10015;
            this.lblMaxValueLIV.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            resources.ApplyResources(this.lblMaxValueLIV, "lblMaxValueLIV");
            this.lblMaxValueLIV.ForeColor = System.Drawing.Color.Black;
            this.lblMaxValueLIV.Name = "lblMaxValueLIV";
            this.lblMaxValueLIV.TextAlignment = System.Drawing.StringAlignment.Center;
            // 
            // dinForceDealyLIV
            // 
            this.dinForceDealyLIV.AutoResolveFreeTextEntries = false;
            // 
            // 
            // 
            this.dinForceDealyLIV.BackgroundStyle.Class = "DateTimeInputBackground";
            this.dinForceDealyLIV.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.dinForceDealyLIV.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.dinForceDealyLIV.DisplayFormat = "0.0";
            resources.ApplyResources(this.dinForceDealyLIV, "dinForceDealyLIV");
            this.dinForceDealyLIV.Increment = 0.5D;
            this.dinForceDealyLIV.IsInputReadOnly = true;
            this.dinForceDealyLIV.MaxValue = 90000D;
            this.dinForceDealyLIV.MinValue = 0D;
            this.dinForceDealyLIV.Name = "dinForceDealyLIV";
            // 
            // numFlatCountLIV
            // 
            // 
            // 
            // 
            this.numFlatCountLIV.BackgroundStyle.Class = "DateTimeInputBackground";
            this.numFlatCountLIV.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.numFlatCountLIV.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.numFlatCountLIV.DisplayFormat = "0";
            resources.ApplyResources(this.numFlatCountLIV, "numFlatCountLIV");
            this.numFlatCountLIV.IsInputReadOnly = true;
            this.numFlatCountLIV.MaxValue = 2000;
            this.numFlatCountLIV.MinValue = 0;
            this.numFlatCountLIV.Name = "numFlatCountLIV";
            // 
            // labelX109
            // 
            resources.ApplyResources(this.labelX109, "labelX109");
            this.labelX109.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX109.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10015;
            this.labelX109.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX109.ForeColor = System.Drawing.Color.Black;
            this.labelX109.Name = "labelX109";
            // 
            // lblFlatCountLIV
            // 
            this.lblFlatCountLIV.BackColor = System.Drawing.Color.PaleTurquoise;
            // 
            // 
            // 
            this.lblFlatCountLIV.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10015;
            this.lblFlatCountLIV.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            resources.ApplyResources(this.lblFlatCountLIV, "lblFlatCountLIV");
            this.lblFlatCountLIV.ForeColor = System.Drawing.Color.Black;
            this.lblFlatCountLIV.Name = "lblFlatCountLIV";
            // 
            // lblEndValueLIV
            // 
            this.lblEndValueLIV.BackColor = System.Drawing.Color.PaleTurquoise;
            // 
            // 
            // 
            this.lblEndValueLIV.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10015;
            this.lblEndValueLIV.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            resources.ApplyResources(this.lblEndValueLIV, "lblEndValueLIV");
            this.lblEndValueLIV.ForeColor = System.Drawing.Color.Black;
            this.lblEndValueLIV.Name = "lblEndValueLIV";
            // 
            // labelX95
            // 
            resources.ApplyResources(this.labelX95, "labelX95");
            this.labelX95.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX95.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10015;
            this.labelX95.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX95.ForeColor = System.Drawing.Color.Black;
            this.labelX95.Name = "labelX95";
            // 
            // lblRiseCountLIV
            // 
            this.lblRiseCountLIV.BackColor = System.Drawing.Color.PaleTurquoise;
            // 
            // 
            // 
            this.lblRiseCountLIV.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10015;
            this.lblRiseCountLIV.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            resources.ApplyResources(this.lblRiseCountLIV, "lblRiseCountLIV");
            this.lblRiseCountLIV.ForeColor = System.Drawing.Color.Black;
            this.lblRiseCountLIV.Name = "lblRiseCountLIV";
            // 
            // labelX102
            // 
            resources.ApplyResources(this.labelX102, "labelX102");
            this.labelX102.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX102.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10015;
            this.labelX102.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX102.ForeColor = System.Drawing.Color.Black;
            this.labelX102.Name = "labelX102";
            // 
            // labelX98
            // 
            resources.ApplyResources(this.labelX98, "labelX98");
            this.labelX98.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX98.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10015;
            this.labelX98.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX98.ForeColor = System.Drawing.Color.Black;
            this.labelX98.Name = "labelX98";
            // 
            // dinStepValueLIV
            // 
            // 
            // 
            // 
            this.dinStepValueLIV.BackgroundStyle.Class = "DateTimeInputBackground";
            this.dinStepValueLIV.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.dinStepValueLIV.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.dinStepValueLIV.DisplayFormat = "0.0000";
            resources.ApplyResources(this.dinStepValueLIV, "dinStepValueLIV");
            this.dinStepValueLIV.Increment = 0.0001D;
            this.dinStepValueLIV.IsInputReadOnly = true;
            this.dinStepValueLIV.MaxValue = 10000D;
            this.dinStepValueLIV.MinValue = 0D;
            this.dinStepValueLIV.Name = "dinStepValueLIV";
            this.dinStepValueLIV.Value = 0.01D;
            // 
            // numRiseCountLIV
            // 
            // 
            // 
            // 
            this.numRiseCountLIV.BackgroundStyle.Class = "DateTimeInputBackground";
            this.numRiseCountLIV.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.numRiseCountLIV.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.numRiseCountLIV.DisplayFormat = "0";
            resources.ApplyResources(this.numRiseCountLIV, "numRiseCountLIV");
            this.numRiseCountLIV.IsInputReadOnly = true;
            this.numRiseCountLIV.MaxValue = 2000;
            this.numRiseCountLIV.MinValue = 0;
            this.numRiseCountLIV.Name = "numRiseCountLIV";
            // 
            // labelX99
            // 
            resources.ApplyResources(this.labelX99, "labelX99");
            this.labelX99.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX99.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10015;
            this.labelX99.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX99.ForeColor = System.Drawing.Color.Black;
            this.labelX99.Name = "labelX99";
            // 
            // lblForceTimeLIV
            // 
            this.lblForceTimeLIV.BackColor = System.Drawing.Color.PaleTurquoise;
            // 
            // 
            // 
            this.lblForceTimeLIV.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10015;
            this.lblForceTimeLIV.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            resources.ApplyResources(this.lblForceTimeLIV, "lblForceTimeLIV");
            this.lblForceTimeLIV.ForeColor = System.Drawing.Color.Black;
            this.lblForceTimeLIV.Name = "lblForceTimeLIV";
            // 
            // lblTurnOffTimeLIV
            // 
            this.lblTurnOffTimeLIV.BackColor = System.Drawing.Color.PaleTurquoise;
            // 
            // 
            // 
            this.lblTurnOffTimeLIV.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10015;
            this.lblTurnOffTimeLIV.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            resources.ApplyResources(this.lblTurnOffTimeLIV, "lblTurnOffTimeLIV");
            this.lblTurnOffTimeLIV.ForeColor = System.Drawing.Color.Black;
            this.lblTurnOffTimeLIV.Name = "lblTurnOffTimeLIV";
            // 
            // lblLIVItem
            // 
            resources.ApplyResources(this.lblLIVItem, "lblLIVItem");
            this.lblLIVItem.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.lblLIVItem.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10015;
            this.lblLIVItem.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblLIVItem.ForeColor = System.Drawing.Color.Black;
            this.lblLIVItem.Name = "lblLIVItem";
            // 
            // cmbLIVItem
            // 
            this.cmbLIVItem.DisplayMember = "Text";
            this.cmbLIVItem.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbLIVItem.FormattingEnabled = true;
            resources.ApplyResources(this.cmbLIVItem, "cmbLIVItem");
            this.cmbLIVItem.Name = "cmbLIVItem";
            this.cmbLIVItem.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.cmbLIVItem.SelectedIndexChanged += new System.EventHandler(this.cmbLIVItem_SelectedIndexChanged);
            // 
            // btnEditLIV
            // 
            this.btnEditLIV.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnEditLIV.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.btnEditLIV, "btnEditLIV");
            this.btnEditLIV.Name = "btnEditLIV";
            this.btnEditLIV.Shape = new DevComponents.DotNetBar.RoundRectangleShapeDescriptor(8);
            this.btnEditLIV.Style = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
            this.btnEditLIV.Click += new System.EventHandler(this.btnEditLIV_Click);
            // 
            // labelX104
            // 
            this.labelX104.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX104.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10015;
            this.labelX104.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX104.ForeColor = System.Drawing.Color.Black;
            resources.ApplyResources(this.labelX104, "labelX104");
            this.labelX104.Name = "labelX104";
            // 
            // frmTestResultAnalyze2
            // 
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            resources.ApplyResources(this, "$this");
            this.ControlBox = false;
            this.Controls.Add(this.groupPanel24);
            this.Controls.Add(this.btnExportToCSV);
            this.Controls.Add(this.btnChartSave01);
            this.Controls.Add(this.lblBaseItem);
            this.Controls.Add(this.cmbChartABaseItems);
            this.Controls.Add(this.cmbChartBDataSource);
            this.Controls.Add(this.cmbChartBItems);
            this.Controls.Add(this.cmbChartADataSource);
            this.Controls.Add(this.cmbChartAItems);
            this.Controls.Add(this.lblDataSourceB);
            this.Controls.Add(this.lblChartItemB);
            this.Controls.Add(this.lblDataSourceA);
            this.Controls.Add(this.lblChartItemA);
            this.Controls.Add(this.gChartB);
            this.Controls.Add(this.gChartA);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmTestResultAnalyze2";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmTestResultAnalyze_Load);
            this.VisibleChanged += new System.EventHandler(this.frmTestResultAnalyze_VisibleChanged);
            this.groupPanel24.ResumeLayout(false);
            this.groupPanel24.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dinStartValueLIV)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dinMsrtRangeLIV)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dinForceTimeLIV)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dinTurnOffTimeLIV)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dinForceDealyLIV)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numFlatCountLIV)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dinStepValueLIV)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numRiseCountLIV)).EndInit();
            this.ResumeLayout(false);

		}

		#endregion

		private ZedGraph.ZedGraphControl gChartA;
		private ZedGraph.ZedGraphControl gChartB;
		private DevComponents.DotNetBar.LabelX lblChartItemA;
		private DevComponents.DotNetBar.LabelX lblDataSourceA;
		private DevComponents.DotNetBar.LabelX lblChartItemB;
		private DevComponents.DotNetBar.LabelX lblDataSourceB;
		private DevComponents.DotNetBar.Controls.ComboBoxEx cmbChartAItems;
		private DevComponents.DotNetBar.Controls.ComboBoxEx cmbChartADataSource;
		private DevComponents.DotNetBar.Controls.ComboBoxEx cmbChartBItems;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cmbChartBDataSource;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cmbChartABaseItems;
        private DevComponents.DotNetBar.LabelX lblBaseItem;
        private DevComponents.DotNetBar.ButtonX btnChartSave01;
        private DevComponents.DotNetBar.ButtonX btnExportToCSV;
        private DevComponents.DotNetBar.Controls.GroupPanel groupPanel24;
        private DevComponents.DotNetBar.LabelX lblMaxValueLIV;
        private DevComponents.Editors.IntegerInput numFlatCountLIV;
        private DevComponents.DotNetBar.LabelX lblFlatCountLIV;
        private DevComponents.DotNetBar.LabelX labelX95;
        private DevComponents.DotNetBar.LabelX lblRiseCountLIV;
        private DevComponents.DotNetBar.LabelX labelX98;
        private DevComponents.Editors.DoubleInput dinTurnOffTimeLIV;
        private DevComponents.DotNetBar.LabelX labelX99;
        private DevComponents.DotNetBar.LabelX lblTurnOffTimeLIV;
        private DevComponents.DotNetBar.LabelX lblForceTimeLIV;
        private DevComponents.Editors.IntegerInput numRiseCountLIV;
        private DevComponents.Editors.DoubleInput dinStepValueLIV;
        private DevComponents.DotNetBar.LabelX labelX102;
        private DevComponents.Editors.DoubleInput dinForceTimeLIV;
        private DevComponents.DotNetBar.LabelX labelX104;
        private DevComponents.DotNetBar.LabelX lblEndValueLIV;
        private DevComponents.DotNetBar.LabelX labelX109;
        private DevComponents.DotNetBar.LabelX lblDelayTimeLIV;
        private DevComponents.Editors.DoubleInput dinForceDealyLIV;
        private DevComponents.DotNetBar.LabelX labelX110;
        private DevComponents.DotNetBar.LabelX lblStartValueLIV;
        private DevComponents.Editors.DoubleInput dinStartValueLIV;
        private DevComponents.DotNetBar.LabelX labelX117;
        private DevComponents.DotNetBar.LabelX lblStepValueLIV;
        private DevComponents.DotNetBar.LabelX labelX118;
        private DevComponents.DotNetBar.ButtonX btnEditLIV;
        private DevComponents.DotNetBar.LabelX lblMsrtRangeUnitLIV;
        private DevComponents.Editors.DoubleInput dinMsrtRangeLIV;
        private DevComponents.DotNetBar.LabelX lblMsrtRangeLIV;
        private DevComponents.DotNetBar.LabelX lblLIVItem;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cmbLIVItem;
        private System.Windows.Forms.Panel panel1;
	}
}
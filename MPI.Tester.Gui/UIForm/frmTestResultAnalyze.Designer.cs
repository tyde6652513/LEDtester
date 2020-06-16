namespace MPI.Tester.Gui
{
	partial class frmTestResultAnalyze
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmTestResultAnalyze));
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
            this.btnChartSave01 = new DevComponents.DotNetBar.ButtonX();
            this.lblChartChA = new DevComponents.DotNetBar.LabelX();
            this.numChartAChannel = new DevComponents.Editors.IntegerInput();
            this.numChartBChannel = new DevComponents.Editors.IntegerInput();
            this.lblChartChB = new DevComponents.DotNetBar.LabelX();
            ((System.ComponentModel.ISupportInitialize)(this.numChartAChannel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numChartBChannel)).BeginInit();
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
            this.lblChartItemA.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
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
            this.lblDataSourceA.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
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
            this.lblChartItemB.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
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
            this.lblDataSourceB.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
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
            // btnChartSave01
            // 
            this.btnChartSave01.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnChartSave01.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.btnChartSave01, "btnChartSave01");
            this.btnChartSave01.Image = global::MPI.Tester.Gui.Properties.Resources.btnSaveFile_B;
            this.btnChartSave01.ImageFixedSize = new System.Drawing.Size(20, 20);
            this.btnChartSave01.Name = "btnChartSave01";
            this.btnChartSave01.Shape = new DevComponents.DotNetBar.RoundRectangleShapeDescriptor(8);
            this.btnChartSave01.Style = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
            this.btnChartSave01.Click += new System.EventHandler(this.btnChartSave01_Click);
            // 
            // lblChartChA
            // 
            // 
            // 
            // 
            this.lblChartChA.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.lblChartChA.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            resources.ApplyResources(this.lblChartChA, "lblChartChA");
            this.lblChartChA.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.lblChartChA.Name = "lblChartChA";
            this.lblChartChA.TextAlignment = System.Drawing.StringAlignment.Center;
            // 
            // numChartAChannel
            // 
            // 
            // 
            // 
            this.numChartAChannel.BackgroundStyle.Class = "DateTimeInputBackground";
            this.numChartAChannel.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.numChartAChannel.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            resources.ApplyResources(this.numChartAChannel, "numChartAChannel");
            this.numChartAChannel.MinValue = 1;
            this.numChartAChannel.Name = "numChartAChannel";
            this.numChartAChannel.ShowUpDown = true;
            this.numChartAChannel.Value = 1;
            // 
            // numChartBChannel
            // 
            // 
            // 
            // 
            this.numChartBChannel.BackgroundStyle.Class = "DateTimeInputBackground";
            this.numChartBChannel.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.numChartBChannel.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            resources.ApplyResources(this.numChartBChannel, "numChartBChannel");
            this.numChartBChannel.MinValue = 1;
            this.numChartBChannel.Name = "numChartBChannel";
            this.numChartBChannel.ShowUpDown = true;
            this.numChartBChannel.Value = 1;
            // 
            // lblChartChB
            // 
            // 
            // 
            // 
            this.lblChartChB.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.lblChartChB.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            resources.ApplyResources(this.lblChartChB, "lblChartChB");
            this.lblChartChB.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.lblChartChB.Name = "lblChartChB";
            this.lblChartChB.TextAlignment = System.Drawing.StringAlignment.Center;
            // 
            // frmTestResultAnalyze
            // 
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            resources.ApplyResources(this, "$this");
            this.ControlBox = false;
            this.Controls.Add(this.numChartBChannel);
            this.Controls.Add(this.lblChartChB);
            this.Controls.Add(this.numChartAChannel);
            this.Controls.Add(this.lblChartChA);
            this.Controls.Add(this.btnChartSave01);
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
            this.Name = "frmTestResultAnalyze";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmTestResultAnalyze_Load);
            this.VisibleChanged += new System.EventHandler(this.frmTestResultAnalyze_VisibleChanged);
            ((System.ComponentModel.ISupportInitialize)(this.numChartAChannel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numChartBChannel)).EndInit();
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
		private DevComponents.DotNetBar.ButtonX btnChartSave01;
        private DevComponents.DotNetBar.LabelX lblChartChA;
        private DevComponents.Editors.IntegerInput numChartAChannel;
        private DevComponents.Editors.IntegerInput numChartBChannel;
        private DevComponents.DotNetBar.LabelX lblChartChB;
	}
}
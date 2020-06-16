namespace MPI.Tester.Gui
{
	partial class frmTestResultSpectrum
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmTestResultSpectrum));
            this.tabItemSpectrum = new DevComponents.DotNetBar.SuperTabItem();
            this.tabcSpectrum = new System.Windows.Forms.TabControl();
            this.tabpCount = new System.Windows.Forms.TabPage();
            this.dinCountMaxY = new DevComponents.Editors.DoubleInput();
            this.dinCountMinX = new DevComponents.Editors.DoubleInput();
            this.dinCountMaxX = new DevComponents.Editors.DoubleInput();
            this.gSptCount = new ZedGraph.ZedGraphControl();
            this.tabpAbsoluteIntensity = new System.Windows.Forms.TabPage();
            this.dinIntensityMaxY = new DevComponents.Editors.DoubleInput();
            this.dinIntensityMinX = new DevComponents.Editors.DoubleInput();
            this.dinIntensityMaxX = new DevComponents.Editors.DoubleInput();
            this.gSptIntensity = new ZedGraph.ZedGraphControl();
            this.cmbSpectroGraph = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.lblSpectroCh = new DevComponents.DotNetBar.LabelX();
            this.numSpectroChannel = new DevComponents.Editors.IntegerInput();
            this.tabcSpectrum.SuspendLayout();
            this.tabpCount.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dinCountMaxY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dinCountMinX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dinCountMaxX)).BeginInit();
            this.tabpAbsoluteIntensity.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dinIntensityMaxY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dinIntensityMinX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dinIntensityMaxX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSpectroChannel)).BeginInit();
            this.SuspendLayout();
            // 
            // tabItemSpectrum
            // 
            this.tabItemSpectrum.AutoCollapseOnClick = false;
            this.tabItemSpectrum.CloseButtonVisible = false;
            this.tabItemSpectrum.EnableMarkup = false;
            this.tabItemSpectrum.GlobalItem = false;
            this.tabItemSpectrum.Icon = ((System.Drawing.Icon)(resources.GetObject("tabItemSpectrum.Icon")));
            this.tabItemSpectrum.Name = "tabItemSpectrum";
            this.tabItemSpectrum.ShowSubItems = false;
            resources.ApplyResources(this.tabItemSpectrum, "tabItemSpectrum");
            // 
            // tabcSpectrum
            // 
            this.tabcSpectrum.Controls.Add(this.tabpCount);
            this.tabcSpectrum.Controls.Add(this.tabpAbsoluteIntensity);
            resources.ApplyResources(this.tabcSpectrum, "tabcSpectrum");
            this.tabcSpectrum.Name = "tabcSpectrum";
            this.tabcSpectrum.SelectedIndex = 0;
            // 
            // tabpCount
            // 
            this.tabpCount.Controls.Add(this.dinCountMaxY);
            this.tabpCount.Controls.Add(this.dinCountMinX);
            this.tabpCount.Controls.Add(this.dinCountMaxX);
            this.tabpCount.Controls.Add(this.gSptCount);
            resources.ApplyResources(this.tabpCount, "tabpCount");
            this.tabpCount.Name = "tabpCount";
            this.tabpCount.UseVisualStyleBackColor = true;
            // 
            // dinCountMaxY
            // 
            this.dinCountMaxY.AntiAlias = true;
            // 
            // 
            // 
            this.dinCountMaxY.BackgroundStyle.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.DockSiteBackColor;
            this.dinCountMaxY.BackgroundStyle.Class = "DateTimeInputBackground";
            this.dinCountMaxY.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.dinCountMaxY.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.dinCountMaxY.DisplayFormat = "0";
            resources.ApplyResources(this.dinCountMaxY, "dinCountMaxY");
            this.dinCountMaxY.Increment = 1D;
            this.dinCountMaxY.MaxValue = 1000000D;
            this.dinCountMaxY.MinValue = 0D;
            this.dinCountMaxY.Name = "dinCountMaxY";
            this.dinCountMaxY.ShowUpDown = true;
            this.dinCountMaxY.Value = 60000D;
            this.dinCountMaxY.ValueChanged += new System.EventHandler(this.dinCountMaxY_ValueChanged);
            // 
            // dinCountMinX
            // 
            this.dinCountMinX.AntiAlias = true;
            this.dinCountMinX.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.dinCountMinX.BackgroundStyle.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.DockSiteBackColor;
            this.dinCountMinX.BackgroundStyle.Class = "DateTimeInputBackground";
            this.dinCountMinX.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.dinCountMinX.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            resources.ApplyResources(this.dinCountMinX, "dinCountMinX");
            this.dinCountMinX.Increment = 1D;
            this.dinCountMinX.MaxValue = 2000D;
            this.dinCountMinX.MinValue = 0D;
            this.dinCountMinX.Name = "dinCountMinX";
            this.dinCountMinX.ShowUpDown = true;
            this.dinCountMinX.Value = 300D;
            this.dinCountMinX.ValueChanged += new System.EventHandler(this.dinCountMinX_ValueChanged);
            // 
            // dinCountMaxX
            // 
            this.dinCountMaxX.AntiAlias = true;
            // 
            // 
            // 
            this.dinCountMaxX.BackgroundStyle.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.DockSiteBackColor;
            this.dinCountMaxX.BackgroundStyle.Class = "DateTimeInputBackground";
            this.dinCountMaxX.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.dinCountMaxX.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            resources.ApplyResources(this.dinCountMaxX, "dinCountMaxX");
            this.dinCountMaxX.Increment = 1D;
            this.dinCountMaxX.MaxValue = 2000D;
            this.dinCountMaxX.MinValue = 0D;
            this.dinCountMaxX.Name = "dinCountMaxX";
            this.dinCountMaxX.ShowUpDown = true;
            this.dinCountMaxX.Value = 1050D;
            this.dinCountMaxX.ValueChanged += new System.EventHandler(this.dinCountMaxX_ValueChanged);
            // 
            // gSptCount
            // 
            resources.ApplyResources(this.gSptCount, "gSptCount");
            this.gSptCount.Name = "gSptCount";
            this.gSptCount.ScrollGrace = 0D;
            this.gSptCount.ScrollMaxX = 0D;
            this.gSptCount.ScrollMaxY = 0D;
            this.gSptCount.ScrollMaxY2 = 0D;
            this.gSptCount.ScrollMinX = 0D;
            this.gSptCount.ScrollMinY = 0D;
            this.gSptCount.ScrollMinY2 = 0D;
            this.gSptCount.ZoomEvent += new ZedGraph.ZedGraphControl.ZoomEventHandler(this.gSptCount_ZoomEvent);
            // 
            // tabpAbsoluteIntensity
            // 
            this.tabpAbsoluteIntensity.Controls.Add(this.dinIntensityMaxY);
            this.tabpAbsoluteIntensity.Controls.Add(this.dinIntensityMinX);
            this.tabpAbsoluteIntensity.Controls.Add(this.dinIntensityMaxX);
            this.tabpAbsoluteIntensity.Controls.Add(this.gSptIntensity);
            resources.ApplyResources(this.tabpAbsoluteIntensity, "tabpAbsoluteIntensity");
            this.tabpAbsoluteIntensity.Name = "tabpAbsoluteIntensity";
            this.tabpAbsoluteIntensity.UseVisualStyleBackColor = true;
            // 
            // dinIntensityMaxY
            // 
            this.dinIntensityMaxY.AntiAlias = true;
            // 
            // 
            // 
            this.dinIntensityMaxY.BackgroundStyle.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.DockSiteBackColor;
            this.dinIntensityMaxY.BackgroundStyle.Class = "DateTimeInputBackground";
            this.dinIntensityMaxY.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.dinIntensityMaxY.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.dinIntensityMaxY.DisplayFormat = "0";
            resources.ApplyResources(this.dinIntensityMaxY, "dinIntensityMaxY");
            this.dinIntensityMaxY.Increment = 1D;
            this.dinIntensityMaxY.MaxValue = 100000D;
            this.dinIntensityMaxY.MinValue = 0D;
            this.dinIntensityMaxY.Name = "dinIntensityMaxY";
            this.dinIntensityMaxY.ShowUpDown = true;
            this.dinIntensityMaxY.Value = 1000D;
            this.dinIntensityMaxY.ValueChanged += new System.EventHandler(this.dinIntensityMaxY_ValueChanged);
            // 
            // dinIntensityMinX
            // 
            this.dinIntensityMinX.AntiAlias = true;
            this.dinIntensityMinX.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.dinIntensityMinX.BackgroundStyle.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.DockSiteBackColor;
            this.dinIntensityMinX.BackgroundStyle.Class = "DateTimeInputBackground";
            this.dinIntensityMinX.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.dinIntensityMinX.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            resources.ApplyResources(this.dinIntensityMinX, "dinIntensityMinX");
            this.dinIntensityMinX.Increment = 1D;
            this.dinIntensityMinX.MaxValue = 2000D;
            this.dinIntensityMinX.MinValue = 0D;
            this.dinIntensityMinX.Name = "dinIntensityMinX";
            this.dinIntensityMinX.ShowUpDown = true;
            this.dinIntensityMinX.Value = 300D;
            this.dinIntensityMinX.ValueChanged += new System.EventHandler(this.dinIntensityMinX_ValueChanged);
            // 
            // dinIntensityMaxX
            // 
            this.dinIntensityMaxX.AntiAlias = true;
            // 
            // 
            // 
            this.dinIntensityMaxX.BackgroundStyle.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.DockSiteBackColor;
            this.dinIntensityMaxX.BackgroundStyle.Class = "DateTimeInputBackground";
            this.dinIntensityMaxX.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.dinIntensityMaxX.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            resources.ApplyResources(this.dinIntensityMaxX, "dinIntensityMaxX");
            this.dinIntensityMaxX.Increment = 1D;
            this.dinIntensityMaxX.MaxValue = 2000D;
            this.dinIntensityMaxX.MinValue = 0D;
            this.dinIntensityMaxX.Name = "dinIntensityMaxX";
            this.dinIntensityMaxX.ShowUpDown = true;
            this.dinIntensityMaxX.Value = 1050D;
            this.dinIntensityMaxX.ValueChanged += new System.EventHandler(this.dinIntensityMaxX_ValueChanged);
            // 
            // gSptIntensity
            // 
            resources.ApplyResources(this.gSptIntensity, "gSptIntensity");
            this.gSptIntensity.Name = "gSptIntensity";
            this.gSptIntensity.ScrollGrace = 0D;
            this.gSptIntensity.ScrollMaxX = 0D;
            this.gSptIntensity.ScrollMaxY = 0D;
            this.gSptIntensity.ScrollMaxY2 = 0D;
            this.gSptIntensity.ScrollMinX = 0D;
            this.gSptIntensity.ScrollMinY = 0D;
            this.gSptIntensity.ScrollMinY2 = 0D;
            this.gSptIntensity.ZoomEvent += new ZedGraph.ZedGraphControl.ZoomEventHandler(this.gSptIntensity_ZoomEvent);
            // 
            // cmbSpectroGraph
            // 
            this.cmbSpectroGraph.DisplayMember = "Text";
            this.cmbSpectroGraph.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbSpectroGraph.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            resources.ApplyResources(this.cmbSpectroGraph, "cmbSpectroGraph");
            this.cmbSpectroGraph.Name = "cmbSpectroGraph";
            this.cmbSpectroGraph.Style = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
            // 
            // lblSpectroCh
            // 
            // 
            // 
            // 
            this.lblSpectroCh.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10001;
            this.lblSpectroCh.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            resources.ApplyResources(this.lblSpectroCh, "lblSpectroCh");
            this.lblSpectroCh.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.lblSpectroCh.Name = "lblSpectroCh";
            this.lblSpectroCh.TextAlignment = System.Drawing.StringAlignment.Center;
            // 
            // numSpectroChannel
            // 
            // 
            // 
            // 
            this.numSpectroChannel.BackgroundStyle.Class = "DateTimeInputBackground";
            this.numSpectroChannel.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.numSpectroChannel.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            resources.ApplyResources(this.numSpectroChannel, "numSpectroChannel");
            this.numSpectroChannel.MinValue = 1;
            this.numSpectroChannel.Name = "numSpectroChannel";
            this.numSpectroChannel.ShowUpDown = true;
            this.numSpectroChannel.Value = 1;
            // 
            // frmTestResultSpectrum
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            resources.ApplyResources(this, "$this");
            this.ControlBox = false;
            this.Controls.Add(this.numSpectroChannel);
            this.Controls.Add(this.lblSpectroCh);
            this.Controls.Add(this.tabcSpectrum);
            this.Controls.Add(this.cmbSpectroGraph);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmTestResultSpectrum";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmTestResult_Load);
            this.VisibleChanged += new System.EventHandler(this.frmTestResultSpectrum_VisibleChanged);
            this.tabcSpectrum.ResumeLayout(false);
            this.tabpCount.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dinCountMaxY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dinCountMinX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dinCountMaxX)).EndInit();
            this.tabpAbsoluteIntensity.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dinIntensityMaxY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dinIntensityMinX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dinIntensityMaxX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSpectroChannel)).EndInit();
            this.ResumeLayout(false);

		}

		#endregion

		private DevComponents.DotNetBar.SuperTabItem tabItemSpectrum;
		private System.Windows.Forms.TabControl tabcSpectrum;
		private System.Windows.Forms.TabPage tabpCount;
		private DevComponents.Editors.DoubleInput dinCountMaxY;
		private DevComponents.Editors.DoubleInput dinCountMinX;
		private DevComponents.Editors.DoubleInput dinCountMaxX;
		private System.Windows.Forms.TabPage tabpAbsoluteIntensity;
		private DevComponents.Editors.DoubleInput dinIntensityMaxY;
		private DevComponents.Editors.DoubleInput dinIntensityMinX;
		private DevComponents.Editors.DoubleInput dinIntensityMaxX;
		private ZedGraph.ZedGraphControl gSptIntensity;
		private ZedGraph.ZedGraphControl gSptCount;
		private DevComponents.DotNetBar.Controls.ComboBoxEx cmbSpectroGraph;
        private DevComponents.DotNetBar.LabelX lblSpectroCh;
        private DevComponents.Editors.IntegerInput numSpectroChannel;
	}
}
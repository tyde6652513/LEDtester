namespace MPI.Tester.Gui
{
	partial class frmMain
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
			this.tlsMain = new System.Windows.Forms.ToolStrip();
			this.btnCondition = new System.Windows.Forms.ToolStripButton();
			this.btnTestResult = new System.Windows.Forms.ToolStripButton();
			this.btnSetting = new System.Windows.Forms.ToolStripButton();
			this.btnTool = new System.Windows.Forms.ToolStripButton();
			this.btnBinSetting = new System.Windows.Forms.ToolStripButton();
			this.btnExitApp = new System.Windows.Forms.ToolStripButton();
			this.tlsMain.SuspendLayout();
			this.SuspendLayout();
			// 
			// tlsMain
			// 
			resources.ApplyResources(this.tlsMain, "tlsMain");
			this.tlsMain.ImageScalingSize = new System.Drawing.Size(32, 32);
			this.tlsMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnCondition,
            this.btnTestResult,
            this.btnSetting,
            this.btnTool,
            this.btnBinSetting,
            this.btnExitApp});
			this.tlsMain.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
			this.tlsMain.Name = "tlsMain";
			this.tlsMain.MouseDown += new System.Windows.Forms.MouseEventHandler(this.tlsMain_MouseDown);
			this.tlsMain.MouseHover += new System.EventHandler(this.tlsMain_MouseHover);
			this.tlsMain.MouseMove += new System.Windows.Forms.MouseEventHandler(this.tlsMain_MouseMove);
			// 
			// btnCondition
			// 
			resources.ApplyResources(this.btnCondition, "btnCondition");
			this.btnCondition.AutoToolTip = false;
			this.btnCondition.BackColor = System.Drawing.Color.Transparent;
			this.btnCondition.Image = global::MPI.Tester.Gui.Properties.Resources.btnTestItem_32;
			this.btnCondition.Name = "btnCondition";
			this.btnCondition.Tag = "Condition";
			this.btnCondition.Click += new System.EventHandler(this.btnCondition_Click);
			// 
			// btnTestResult
			// 
			resources.ApplyResources(this.btnTestResult, "btnTestResult");
			this.btnTestResult.AutoToolTip = false;
			this.btnTestResult.BackColor = System.Drawing.Color.Transparent;
			this.btnTestResult.Image = global::MPI.Tester.Gui.Properties.Resources.btnTestResult_32;
			this.btnTestResult.Name = "btnTestResult";
			this.btnTestResult.Tag = "TestResult";
			// 
			// btnSetting
			// 
			resources.ApplyResources(this.btnSetting, "btnSetting");
			this.btnSetting.AutoToolTip = false;
			this.btnSetting.BackColor = System.Drawing.Color.Transparent;
			this.btnSetting.Image = global::MPI.Tester.Gui.Properties.Resources.btnSetting_32;
			this.btnSetting.Name = "btnSetting";
			this.btnSetting.Tag = "Setting";
			// 
			// btnTool
			// 
			resources.ApplyResources(this.btnTool, "btnTool");
			this.btnTool.AutoToolTip = false;
			this.btnTool.Image = global::MPI.Tester.Gui.Properties.Resources.btnPowerTool_32;
			this.btnTool.Name = "btnTool";
			this.btnTool.Tag = "Tools";
			// 
			// btnBinSetting
			// 
			resources.ApplyResources(this.btnBinSetting, "btnBinSetting");
			this.btnBinSetting.AutoToolTip = false;
			this.btnBinSetting.BackColor = System.Drawing.Color.Transparent;
			this.btnBinSetting.Image = global::MPI.Tester.Gui.Properties.Resources.btnBinSetting_32;
			this.btnBinSetting.Name = "btnBinSetting";
			this.btnBinSetting.Tag = "BinSetting";
			// 
			// btnExitApp
			// 
			resources.ApplyResources(this.btnExitApp, "btnExitApp");
			this.btnExitApp.AutoToolTip = false;
			this.btnExitApp.BackColor = System.Drawing.Color.Transparent;
			this.btnExitApp.Image = global::MPI.Tester.Gui.Properties.Resources.btnExitApp_32;
			this.btnExitApp.Name = "btnExitApp";
			this.btnExitApp.Tag = "ExitApp";
			this.btnExitApp.Click += new System.EventHandler(this.btnExitApp_Click);
			// 
			// frmMain
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.BackColor = System.Drawing.Color.WhiteSmoke;
			resources.ApplyResources(this, "$this");
			this.Controls.Add(this.tlsMain);
			this.DoubleBuffered = true;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Name = "frmMain";
			this.Opacity = 0.75D;
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
			this.Load += new System.EventHandler(this.frmMain_Load);
			this.Shown += new System.EventHandler(this.frmMain_Shown);
			this.VisibleChanged += new System.EventHandler(this.frmMain_VisibleChanged);
			this.tlsMain.ResumeLayout(false);
			this.tlsMain.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ToolStripButton btnSetting;
		private System.Windows.Forms.ToolStripButton btnCondition;
		private System.Windows.Forms.ToolStripButton btnBinSetting;
		private System.Windows.Forms.ToolStripButton btnTestResult;
		private System.Windows.Forms.ToolStripButton btnExitApp;
		private System.Windows.Forms.ToolStrip tlsMain;
		private System.Windows.Forms.ToolStripButton btnTool;


	}
}
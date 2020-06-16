namespace MPI.Tester.Gui
{
	partial class frmItemContactCheck
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmItemContactCheck));
			this.grpItemCondition = new System.Windows.Forms.GroupBox();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.grpApplySetting = new DevComponents.DotNetBar.Controls.GroupPanel();
			this.pnlForceRange = new System.Windows.Forms.Panel();
			this.lblSpeed = new DevComponents.DotNetBar.LabelX();
			this.cmbSpeed = new DevComponents.DotNetBar.Controls.ComboBoxEx();
			this.grpItemCondition.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.grpApplySetting.SuspendLayout();
			this.pnlForceRange.SuspendLayout();
			this.SuspendLayout();
			// 
			// grpItemCondition
			// 
			resources.ApplyResources(this.grpItemCondition, "grpItemCondition");
			this.grpItemCondition.Controls.Add(this.splitContainer1);
			this.grpItemCondition.ForeColor = System.Drawing.Color.DimGray;
			this.grpItemCondition.Name = "grpItemCondition";
			this.grpItemCondition.TabStop = false;
			// 
			// splitContainer1
			// 
			resources.ApplyResources(this.splitContainer1, "splitContainer1");
			this.splitContainer1.ForeColor = System.Drawing.Color.Black;
			this.splitContainer1.Name = "splitContainer1";
			// 
			// splitContainer1.Panel1
			// 
			resources.ApplyResources(this.splitContainer1.Panel1, "splitContainer1.Panel1");
			this.splitContainer1.Panel1.Controls.Add(this.grpApplySetting);
			// 
			// splitContainer1.Panel2
			// 
			resources.ApplyResources(this.splitContainer1.Panel2, "splitContainer1.Panel2");
			// 
			// grpApplySetting
			// 
			resources.ApplyResources(this.grpApplySetting, "grpApplySetting");
			this.grpApplySetting.BackColor = System.Drawing.Color.Transparent;
			this.grpApplySetting.CanvasColor = System.Drawing.Color.Empty;
			this.grpApplySetting.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
			this.grpApplySetting.Controls.Add(this.pnlForceRange);
			this.grpApplySetting.DrawTitleBox = false;
			this.grpApplySetting.Name = "grpApplySetting";
			// 
			// 
			// 
			this.grpApplySetting.Style.BackColor = System.Drawing.Color.WhiteSmoke;
			this.grpApplySetting.Style.BackColor2 = System.Drawing.SystemColors.Window;
			this.grpApplySetting.Style.BackColorGradientAngle = 90;
			this.grpApplySetting.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
			this.grpApplySetting.Style.BorderBottomWidth = 1;
			this.grpApplySetting.Style.BorderColor = System.Drawing.Color.Gray;
			this.grpApplySetting.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
			this.grpApplySetting.Style.BorderLeftWidth = 1;
			this.grpApplySetting.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
			this.grpApplySetting.Style.BorderRightWidth = 1;
			this.grpApplySetting.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
			this.grpApplySetting.Style.BorderTopWidth = 1;
			this.grpApplySetting.Style.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
			this.grpApplySetting.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded;
			this.grpApplySetting.Style.TextColor = System.Drawing.Color.DarkOrange;
			// 
			// 
			// 
			this.grpApplySetting.StyleMouseDown.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
			this.grpApplySetting.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square;
			// 
			// 
			// 
			this.grpApplySetting.StyleMouseOver.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
			this.grpApplySetting.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square;
			// 
			// pnlForceRange
			// 
			resources.ApplyResources(this.pnlForceRange, "pnlForceRange");
			this.pnlForceRange.Controls.Add(this.lblSpeed);
			this.pnlForceRange.Controls.Add(this.cmbSpeed);
			this.pnlForceRange.Name = "pnlForceRange";
			// 
			// lblSpeed
			// 
			resources.ApplyResources(this.lblSpeed, "lblSpeed");
			this.lblSpeed.BackColor = System.Drawing.Color.Transparent;
			// 
			// 
			// 
			this.lblSpeed.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
			this.lblSpeed.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
			this.lblSpeed.ForeColor = System.Drawing.Color.Black;
			this.lblSpeed.Name = "lblSpeed";
			// 
			// cmbSpeed
			// 
			resources.ApplyResources(this.cmbSpeed, "cmbSpeed");
			this.cmbSpeed.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbSpeed.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
			this.cmbSpeed.Name = "cmbSpeed";
			this.cmbSpeed.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
			this.cmbSpeed.WatermarkText = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
			// 
			// frmItemContactCheck
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ControlBox = false;
			this.Controls.Add(this.grpItemCondition);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Name = "frmItemContactCheck";
			this.grpItemCondition.ResumeLayout(false);
			this.splitContainer1.Panel1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
			this.splitContainer1.ResumeLayout(false);
			this.grpApplySetting.ResumeLayout(false);
			this.pnlForceRange.ResumeLayout(false);
			this.pnlForceRange.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox grpItemCondition;
		private System.Windows.Forms.SplitContainer splitContainer1;
		private DevComponents.DotNetBar.Controls.GroupPanel grpApplySetting;
		private System.Windows.Forms.Panel pnlForceRange;
		private DevComponents.DotNetBar.LabelX lblSpeed;
		private DevComponents.DotNetBar.Controls.ComboBoxEx cmbSpeed;
	}
}
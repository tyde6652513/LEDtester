namespace MPI.Tester.Gui
{
	partial class frmWaferMap
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
            this.ControlPanel = new System.Windows.Forms.Panel();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.lblIndex = new DevComponents.DotNetBar.LabelX();
            this.btnZoomIn = new DevComponents.DotNetBar.ButtonX();
            this.mLabel2 = new DevComponents.DotNetBar.LabelX();
            this.mLabel5 = new DevComponents.DotNetBar.LabelX();
            this.mLabel1 = new DevComponents.DotNetBar.LabelX();
            this.lblPosY = new DevComponents.DotNetBar.LabelX();
            this.lblPosX = new DevComponents.DotNetBar.LabelX();
            this.lblValue = new DevComponents.DotNetBar.LabelX();
            this.btnZoomOut = new DevComponents.DotNetBar.ButtonX();
            this.WaferMap = new MPI.Windows.Forms.KBlendWaferMap();
            this.ControlPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // ControlPanel
            // 
            this.ControlPanel.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ControlPanel.Controls.Add(this.labelX1);
            this.ControlPanel.Controls.Add(this.lblIndex);
            this.ControlPanel.Controls.Add(this.btnZoomIn);
            this.ControlPanel.Controls.Add(this.mLabel2);
            this.ControlPanel.Controls.Add(this.mLabel5);
            this.ControlPanel.Controls.Add(this.mLabel1);
            this.ControlPanel.Controls.Add(this.lblPosY);
            this.ControlPanel.Controls.Add(this.lblPosX);
            this.ControlPanel.Controls.Add(this.lblValue);
            this.ControlPanel.Controls.Add(this.btnZoomOut);
            this.ControlPanel.Location = new System.Drawing.Point(-3, 393);
            this.ControlPanel.Margin = new System.Windows.Forms.Padding(0);
            this.ControlPanel.Name = "ControlPanel";
            this.ControlPanel.Size = new System.Drawing.Size(390, 43);
            this.ControlPanel.TabIndex = 2;
            // 
            // labelX1
            // 
            this.labelX1.AutoSize = true;
            this.labelX1.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX1.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.labelX1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX1.ForeColor = System.Drawing.Color.Black;
            this.labelX1.Location = new System.Drawing.Point(58, 4);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(33, 16);
            this.labelX1.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.labelX1.TabIndex = 0;
            this.labelX1.Text = "Index";
            // 
            // lblIndex
            // 
            this.lblIndex.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.lblIndex.BackgroundStyle.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.lblIndex.BackgroundStyle.BorderBottomWidth = 2;
            this.lblIndex.BackgroundStyle.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.MenuBorder;
            this.lblIndex.BackgroundStyle.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.lblIndex.BackgroundStyle.BorderLeftWidth = 2;
            this.lblIndex.BackgroundStyle.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.lblIndex.BackgroundStyle.BorderRightWidth = 2;
            this.lblIndex.BackgroundStyle.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.lblIndex.BackgroundStyle.BorderTopWidth = 2;
            this.lblIndex.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.lblIndex.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblIndex.EnableMarkup = false;
            this.lblIndex.Font = new System.Drawing.Font("PMingLiU", 9F);
            this.lblIndex.ForeColor = System.Drawing.Color.OrangeRed;
            this.lblIndex.Location = new System.Drawing.Point(58, 20);
            this.lblIndex.Name = "lblIndex";
            this.lblIndex.Size = new System.Drawing.Size(65, 22);
            this.lblIndex.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.lblIndex.TabIndex = 0;
            this.lblIndex.TextAlignment = System.Drawing.StringAlignment.Center;
            // 
            // btnZoomIn
            // 
            this.btnZoomIn.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnZoomIn.Image = global::MPI.Tester.Gui.Properties.Resources.btnZoom_in;
            this.btnZoomIn.ImageFixedSize = new System.Drawing.Size(30, 30);
            this.btnZoomIn.Location = new System.Drawing.Point(342, 3);
            this.btnZoomIn.Name = "btnZoomIn";
            this.btnZoomIn.Size = new System.Drawing.Size(60, 40);
            this.btnZoomIn.Style = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
            this.btnZoomIn.TabIndex = 0;
            this.btnZoomIn.TabStop = false;
            this.btnZoomIn.Visible = false;
            this.btnZoomIn.Click += new System.EventHandler(this.btnZoomIn_Click);
            // 
            // mLabel2
            // 
            this.mLabel2.AutoSize = true;
            this.mLabel2.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.mLabel2.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.mLabel2.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.mLabel2.ForeColor = System.Drawing.Color.Black;
            this.mLabel2.Location = new System.Drawing.Point(271, 4);
            this.mLabel2.Name = "mLabel2";
            this.mLabel2.Size = new System.Drawing.Size(32, 16);
            this.mLabel2.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.mLabel2.TabIndex = 0;
            this.mLabel2.Text = "PosY";
            // 
            // mLabel5
            // 
            this.mLabel5.AutoSize = true;
            this.mLabel5.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.mLabel5.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.mLabel5.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.mLabel5.EnableMarkup = false;
            this.mLabel5.ForeColor = System.Drawing.Color.Black;
            this.mLabel5.Location = new System.Drawing.Point(200, 4);
            this.mLabel5.Name = "mLabel5";
            this.mLabel5.Size = new System.Drawing.Size(32, 16);
            this.mLabel5.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.mLabel5.TabIndex = 0;
            this.mLabel5.Text = "PosX";
            // 
            // mLabel1
            // 
            this.mLabel1.AutoSize = true;
            this.mLabel1.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.mLabel1.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.mLabel1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.mLabel1.EnableMarkup = false;
            this.mLabel1.ForeColor = System.Drawing.Color.Black;
            this.mLabel1.Location = new System.Drawing.Point(129, 4);
            this.mLabel1.Name = "mLabel1";
            this.mLabel1.Size = new System.Drawing.Size(34, 16);
            this.mLabel1.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.mLabel1.TabIndex = 0;
            this.mLabel1.Text = "Value";
            // 
            // lblPosY
            // 
            this.lblPosY.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.lblPosY.BackgroundStyle.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.lblPosY.BackgroundStyle.BorderBottomWidth = 2;
            this.lblPosY.BackgroundStyle.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.MenuBorder;
            this.lblPosY.BackgroundStyle.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.lblPosY.BackgroundStyle.BorderLeftWidth = 2;
            this.lblPosY.BackgroundStyle.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.lblPosY.BackgroundStyle.BorderRightWidth = 2;
            this.lblPosY.BackgroundStyle.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.lblPosY.BackgroundStyle.BorderTopWidth = 2;
            this.lblPosY.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.lblPosY.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblPosY.EnableMarkup = false;
            this.lblPosY.Font = new System.Drawing.Font("PMingLiU", 9F);
            this.lblPosY.ForeColor = System.Drawing.Color.OrangeRed;
            this.lblPosY.Location = new System.Drawing.Point(271, 20);
            this.lblPosY.Name = "lblPosY";
            this.lblPosY.Size = new System.Drawing.Size(65, 22);
            this.lblPosY.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.lblPosY.TabIndex = 0;
            this.lblPosY.TextAlignment = System.Drawing.StringAlignment.Center;
            // 
            // lblPosX
            // 
            this.lblPosX.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.lblPosX.BackgroundStyle.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.lblPosX.BackgroundStyle.BorderBottomWidth = 2;
            this.lblPosX.BackgroundStyle.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.MenuBorder;
            this.lblPosX.BackgroundStyle.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.lblPosX.BackgroundStyle.BorderLeftWidth = 2;
            this.lblPosX.BackgroundStyle.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.lblPosX.BackgroundStyle.BorderRightWidth = 2;
            this.lblPosX.BackgroundStyle.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.lblPosX.BackgroundStyle.BorderTopWidth = 2;
            this.lblPosX.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.lblPosX.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblPosX.EnableMarkup = false;
            this.lblPosX.Font = new System.Drawing.Font("PMingLiU", 9F);
            this.lblPosX.ForeColor = System.Drawing.Color.OrangeRed;
            this.lblPosX.Location = new System.Drawing.Point(200, 20);
            this.lblPosX.Name = "lblPosX";
            this.lblPosX.Size = new System.Drawing.Size(65, 22);
            this.lblPosX.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.lblPosX.TabIndex = 0;
            this.lblPosX.TextAlignment = System.Drawing.StringAlignment.Center;
            // 
            // lblValue
            // 
            this.lblValue.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.lblValue.BackgroundStyle.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.lblValue.BackgroundStyle.BorderBottomWidth = 2;
            this.lblValue.BackgroundStyle.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.MenuBorder;
            this.lblValue.BackgroundStyle.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.lblValue.BackgroundStyle.BorderLeftWidth = 2;
            this.lblValue.BackgroundStyle.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.lblValue.BackgroundStyle.BorderRightWidth = 2;
            this.lblValue.BackgroundStyle.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.lblValue.BackgroundStyle.BorderTopWidth = 2;
            this.lblValue.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.lblValue.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblValue.EnableMarkup = false;
            this.lblValue.Font = new System.Drawing.Font("PMingLiU", 9F);
            this.lblValue.ForeColor = System.Drawing.Color.OrangeRed;
            this.lblValue.Location = new System.Drawing.Point(129, 20);
            this.lblValue.Name = "lblValue";
            this.lblValue.Size = new System.Drawing.Size(65, 22);
            this.lblValue.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.lblValue.TabIndex = 0;
            this.lblValue.TextAlignment = System.Drawing.StringAlignment.Center;
            // 
            // btnZoomOut
            // 
            this.btnZoomOut.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnZoomOut.Image = global::MPI.Tester.Gui.Properties.Resources.btnAutoScale2;
            this.btnZoomOut.ImageFixedSize = new System.Drawing.Size(50, 50);
            this.btnZoomOut.Location = new System.Drawing.Point(8, 5);
            this.btnZoomOut.Name = "btnZoomOut";
            this.btnZoomOut.Size = new System.Drawing.Size(40, 36);
            this.btnZoomOut.TabIndex = 0;
            this.btnZoomOut.TabStop = false;
            this.btnZoomOut.Click += new System.EventHandler(this.btnZoomOut_Click);
            // 
            // WaferMap
            // 
            this.WaferMap.AutoBoundary = true;
            this.WaferMap.BackColor = System.Drawing.Color.Black;
            this.WaferMap.BlendBar = false;
            this.WaferMap.CellGap = 1;
            this.WaferMap.Cursor = System.Windows.Forms.Cursors.Cross;
            this.WaferMap.DragToGo = false;
            this.WaferMap.DynamicMode = true;
            this.WaferMap.EraseDieColor = System.Drawing.SystemColors.Control;
            this.WaferMap.FocusBox = true;
            this.WaferMap.FocusBoxEnabled = true;
            this.WaferMap.FocusColor = System.Drawing.Color.Yellow;
            this.WaferMap.GradeMethod = MPI.UCF.Forms.Domain.EGradeColorMethod.KBlendColor;
            this.WaferMap.GrowthDirection = MPI.Windows.Forms.EGrowthDirection.Downward;
            this.WaferMap.HoverColor = System.Drawing.Color.White;
            this.WaferMap.InvalidStatus = MPI.UCF.Forms.Domain.EDieStatus.NotExist;
            this.WaferMap.IsSeamless = false;
            this.WaferMap.Location = new System.Drawing.Point(0, 1);
            this.WaferMap.MaxLevelColor = System.Drawing.Color.Blue;
            this.WaferMap.MaxLevelValue = 130F;
            this.WaferMap.MaxScale = 4D;
            this.WaferMap.MinLevelColor = System.Drawing.Color.Red;
            this.WaferMap.MinLevelValue = 50F;
            this.WaferMap.MinScale = 0.07D;
            this.WaferMap.Name = "WaferMap";
            this.WaferMap.Redraw = true;
            this.WaferMap.ScaledChip = false;
            this.WaferMap.ScaleToContent = true;
            this.WaferMap.ScrollBarEnabled = true;
            this.WaferMap.Seamless = false;
            this.WaferMap.Selectable = true;
            this.WaferMap.SelectionColor = System.Drawing.Color.Yellow;
            this.WaferMap.SelectionZoom = true;
            this.WaferMap.SelectWindowEnabled = true;
            this.WaferMap.Size = new System.Drawing.Size(390, 390);
            this.WaferMap.Snap = false;
            this.WaferMap.SymbolId = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.WaferMap.TabIndex = 1;
            this.WaferMap.TabStop = false;
            this.WaferMap.UseAutoScale = false;
            this.WaferMap.OnChipClick += new MPI.UCF.Forms.Domain.ChipFocusEventHandler(this.OnClickUapateStatusUI);
            // 
            // frmWaferMap
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(616, 778);
            this.ControlBox = false;
            this.Controls.Add(this.WaferMap);
            this.Controls.Add(this.ControlPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmWaferMap";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Wafer Map";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmWaferMap_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmWaferMap_FormClosed);
            this.Load += new System.EventHandler(this.frmWaferMap_Load);
            this.ControlPanel.ResumeLayout(false);
            this.ControlPanel.PerformLayout();
            this.ResumeLayout(false);

		}

		#endregion
		
		private System.Windows.Forms.Panel ControlPanel;
		private DevComponents.DotNetBar.LabelX mLabel2;
		private DevComponents.DotNetBar.LabelX mLabel5;
		private DevComponents.DotNetBar.LabelX mLabel1;
		private DevComponents.DotNetBar.LabelX lblPosY;
		private DevComponents.DotNetBar.LabelX lblPosX;
		private DevComponents.DotNetBar.LabelX lblValue;
		private DevComponents.DotNetBar.ButtonX btnZoomOut;
		private DevComponents.DotNetBar.ButtonX btnZoomIn;
		private DevComponents.DotNetBar.LabelX labelX1;
		private DevComponents.DotNetBar.LabelX lblIndex;
		public MPI.Windows.Forms.KBlendWaferMap WaferMap;


	}
}
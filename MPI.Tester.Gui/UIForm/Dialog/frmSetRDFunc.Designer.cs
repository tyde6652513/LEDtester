namespace MPI.Tester.Gui
{
	partial class frmSetRDFunc
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSetRDFunc));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dgvItemBoundary = new DevComponents.DotNetBar.Controls.DataGridViewX();
            this.labelX3 = new DevComponents.DotNetBar.LabelX();
            this.numESDPrechargeWaitTime = new DevComponents.Editors.IntegerInput();
            this.lblESDPrechargeWaitTime = new DevComponents.DotNetBar.LabelX();
            this.numMDParallelDelayTime = new DevComponents.Editors.IntegerInput();
            this.labelX7 = new DevComponents.DotNetBar.LabelX();
            this.labelX5 = new DevComponents.DotNetBar.LabelX();
            this.numMDSeriesDelayTime = new DevComponents.Editors.IntegerInput();
            this.lblMDSeriesDelayTime = new DevComponents.DotNetBar.LabelX();
            this.lblMDParallelDelayTime = new DevComponents.DotNetBar.LabelX();
            this.dinRTHTdTime = new DevComponents.Editors.DoubleInput();
            this.lblRTHTdTime = new DevComponents.DotNetBar.LabelX();
            this.labelX4 = new DevComponents.DotNetBar.LabelX();
            this.chkIsEnableAbsMsrtIR = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.chkIsEnableKeepRecoveryData = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.numVRDelayTime = new DevComponents.Editors.IntegerInput();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.chkIsEnableVRDelayTime = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.lblHighSpeedModeDelayTime = new DevComponents.DotNetBar.LabelX();
            this.numHighSpeedModeDelayTime = new DevComponents.Editors.IntegerInput();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.ipRthAddress = new DevComponents.Editors.IpAddressInput();
            this.lblrthIPAddress = new DevComponents.DotNetBar.LabelX();
            this.chkIsEnableRTHTestItem = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.lblESDHighSpeedDelayTime = new DevComponents.DotNetBar.LabelX();
            this.numESDHighSpeedDelayTime = new DevComponents.Editors.IntegerInput();
            this.labelX6 = new DevComponents.DotNetBar.LabelX();
            this.chkIsEnableESDHighSpeedMode = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.btnSave = new DevComponents.DotNetBar.ButtonX();
            this.btnReload = new DevComponents.DotNetBar.ButtonX();
            this.gpParameterSetting = new System.Windows.Forms.GroupBox();
            this.btnSetIO = new System.Windows.Forms.Button();
            this.cmbTesterConfigType = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.lblTesterType = new DevComponents.DotNetBar.LabelX();
            this.gpTestItemSpecSetting = new System.Windows.Forms.GroupBox();
            this.lblSettingInfo = new System.Windows.Forms.Label();
            this.btnResetSelectedItem = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.btnEditEnableItems = new System.Windows.Forms.Button();
            this.lstBoxEnableTestItems = new System.Windows.Forms.ListBox();
            this.lblItemBoundaryTitle = new System.Windows.Forms.Label();
            this.lblUserID = new System.Windows.Forms.Label();
            this.colpropKeyName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colIsVisible = new DevComponents.DotNetBar.Controls.DataGridViewCheckBoxXColumn();
            this.colpropMin = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colpropMax = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colpropDefault = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colpropUnit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colpropFormat = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvItemBoundary)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numESDPrechargeWaitTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMDParallelDelayTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMDSeriesDelayTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dinRTHTdTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numVRDelayTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numHighSpeedModeDelayTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ipRthAddress)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numESDHighSpeedDelayTime)).BeginInit();
            this.gpParameterSetting.SuspendLayout();
            this.gpTestItemSpecSetting.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvItemBoundary
            // 
            this.dgvItemBoundary.AllowUserToAddRows = false;
            this.dgvItemBoundary.AllowUserToDeleteRows = false;
            this.dgvItemBoundary.AllowUserToResizeColumns = false;
            this.dgvItemBoundary.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("PMingLiU", 9F);
            this.dgvItemBoundary.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvItemBoundary.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvItemBoundary.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colpropKeyName,
            this.colIsVisible,
            this.colpropMin,
            this.colpropMax,
            this.colpropDefault,
            this.colpropUnit,
            this.colpropFormat});
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Arial", 9.75F);
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvItemBoundary.DefaultCellStyle = dataGridViewCellStyle3;
            this.dgvItemBoundary.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(215)))), ((int)(((byte)(229)))));
            resources.ApplyResources(this.dgvItemBoundary, "dgvItemBoundary");
            this.dgvItemBoundary.MultiSelect = false;
            this.dgvItemBoundary.Name = "dgvItemBoundary";
            this.dgvItemBoundary.RowHeadersVisible = false;
            this.dgvItemBoundary.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("PMingLiU", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.dgvItemBoundary.RowsDefaultCellStyle = dataGridViewCellStyle4;
            this.dgvItemBoundary.RowTemplate.Height = 24;
            this.dgvItemBoundary.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgvItemBoundary.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvItemBoundary_CellEndEdit);
            this.dgvItemBoundary.CellLeave += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvItemBoundary_CellEndEdit);
            // 
            // labelX3
            // 
            this.labelX3.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX3.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10026;
            this.labelX3.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            resources.ApplyResources(this.labelX3, "labelX3");
            this.labelX3.ForeColor = System.Drawing.Color.Black;
            this.labelX3.Name = "labelX3";
            this.labelX3.TextAlignment = System.Drawing.StringAlignment.Center;
            // 
            // numESDPrechargeWaitTime
            // 
            this.numESDPrechargeWaitTime.AntiAlias = true;
            // 
            // 
            // 
            this.numESDPrechargeWaitTime.BackgroundStyle.BorderColor = System.Drawing.Color.CornflowerBlue;
            this.numESDPrechargeWaitTime.BackgroundStyle.Class = "DateTimeInputBackground";
            this.numESDPrechargeWaitTime.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            resources.ApplyResources(this.numESDPrechargeWaitTime, "numESDPrechargeWaitTime");
            this.numESDPrechargeWaitTime.MaxValue = 300;
            this.numESDPrechargeWaitTime.MinValue = 0;
            this.numESDPrechargeWaitTime.Name = "numESDPrechargeWaitTime";
            this.numESDPrechargeWaitTime.ShowUpDown = true;
            this.numESDPrechargeWaitTime.Value = 100;
            // 
            // lblESDPrechargeWaitTime
            // 
            this.lblESDPrechargeWaitTime.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.lblESDPrechargeWaitTime.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10026;
            this.lblESDPrechargeWaitTime.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            resources.ApplyResources(this.lblESDPrechargeWaitTime, "lblESDPrechargeWaitTime");
            this.lblESDPrechargeWaitTime.ForeColor = System.Drawing.Color.Black;
            this.lblESDPrechargeWaitTime.Name = "lblESDPrechargeWaitTime";
            this.lblESDPrechargeWaitTime.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
            // 
            // numMDParallelDelayTime
            // 
            this.numMDParallelDelayTime.AntiAlias = true;
            // 
            // 
            // 
            this.numMDParallelDelayTime.BackgroundStyle.BorderColor = System.Drawing.Color.CornflowerBlue;
            this.numMDParallelDelayTime.BackgroundStyle.Class = "DateTimeInputBackground";
            this.numMDParallelDelayTime.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            resources.ApplyResources(this.numMDParallelDelayTime, "numMDParallelDelayTime");
            this.numMDParallelDelayTime.MaxValue = 15;
            this.numMDParallelDelayTime.MinValue = 0;
            this.numMDParallelDelayTime.Name = "numMDParallelDelayTime";
            this.numMDParallelDelayTime.ShowUpDown = true;
            this.numMDParallelDelayTime.Value = 3;
            // 
            // labelX7
            // 
            this.labelX7.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX7.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10026;
            this.labelX7.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            resources.ApplyResources(this.labelX7, "labelX7");
            this.labelX7.ForeColor = System.Drawing.Color.Black;
            this.labelX7.Name = "labelX7";
            this.labelX7.TextAlignment = System.Drawing.StringAlignment.Center;
            // 
            // labelX5
            // 
            this.labelX5.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX5.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10026;
            this.labelX5.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            resources.ApplyResources(this.labelX5, "labelX5");
            this.labelX5.ForeColor = System.Drawing.Color.Black;
            this.labelX5.Name = "labelX5";
            this.labelX5.TextAlignment = System.Drawing.StringAlignment.Center;
            // 
            // numMDSeriesDelayTime
            // 
            this.numMDSeriesDelayTime.AntiAlias = true;
            // 
            // 
            // 
            this.numMDSeriesDelayTime.BackgroundStyle.BorderColor = System.Drawing.Color.CornflowerBlue;
            this.numMDSeriesDelayTime.BackgroundStyle.Class = "DateTimeInputBackground";
            this.numMDSeriesDelayTime.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            resources.ApplyResources(this.numMDSeriesDelayTime, "numMDSeriesDelayTime");
            this.numMDSeriesDelayTime.MaxValue = 15;
            this.numMDSeriesDelayTime.MinValue = 0;
            this.numMDSeriesDelayTime.Name = "numMDSeriesDelayTime";
            this.numMDSeriesDelayTime.ShowUpDown = true;
            this.numMDSeriesDelayTime.Value = 5;
            // 
            // lblMDSeriesDelayTime
            // 
            this.lblMDSeriesDelayTime.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.lblMDSeriesDelayTime.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10026;
            this.lblMDSeriesDelayTime.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            resources.ApplyResources(this.lblMDSeriesDelayTime, "lblMDSeriesDelayTime");
            this.lblMDSeriesDelayTime.ForeColor = System.Drawing.Color.Black;
            this.lblMDSeriesDelayTime.Name = "lblMDSeriesDelayTime";
            this.lblMDSeriesDelayTime.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
            // 
            // lblMDParallelDelayTime
            // 
            this.lblMDParallelDelayTime.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.lblMDParallelDelayTime.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10026;
            this.lblMDParallelDelayTime.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            resources.ApplyResources(this.lblMDParallelDelayTime, "lblMDParallelDelayTime");
            this.lblMDParallelDelayTime.ForeColor = System.Drawing.Color.Black;
            this.lblMDParallelDelayTime.Name = "lblMDParallelDelayTime";
            this.lblMDParallelDelayTime.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
            // 
            // dinRTHTdTime
            // 
            // 
            // 
            // 
            this.dinRTHTdTime.BackgroundStyle.Class = "DateTimeInputBackground";
            this.dinRTHTdTime.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.dinRTHTdTime.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.dinRTHTdTime.DisplayFormat = "0.0";
            resources.ApplyResources(this.dinRTHTdTime, "dinRTHTdTime");
            this.dinRTHTdTime.Increment = 0.5D;
            this.dinRTHTdTime.MaxValue = 10000D;
            this.dinRTHTdTime.MinValue = 0D;
            this.dinRTHTdTime.Name = "dinRTHTdTime";
            this.dinRTHTdTime.ShowUpDown = true;
            this.dinRTHTdTime.Value = 1.6D;
            // 
            // lblRTHTdTime
            // 
            this.lblRTHTdTime.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.lblRTHTdTime.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10026;
            this.lblRTHTdTime.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            resources.ApplyResources(this.lblRTHTdTime, "lblRTHTdTime");
            this.lblRTHTdTime.ForeColor = System.Drawing.Color.Black;
            this.lblRTHTdTime.Name = "lblRTHTdTime";
            this.lblRTHTdTime.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
            // 
            // labelX4
            // 
            this.labelX4.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX4.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10026;
            this.labelX4.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            resources.ApplyResources(this.labelX4, "labelX4");
            this.labelX4.ForeColor = System.Drawing.Color.Black;
            this.labelX4.Name = "labelX4";
            this.labelX4.TextAlignment = System.Drawing.StringAlignment.Center;
            // 
            // chkIsEnableAbsMsrtIR
            // 
            this.chkIsEnableAbsMsrtIR.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.chkIsEnableAbsMsrtIR.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10026;
            this.chkIsEnableAbsMsrtIR.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.chkIsEnableAbsMsrtIR.CheckSignSize = new System.Drawing.Size(15, 15);
            resources.ApplyResources(this.chkIsEnableAbsMsrtIR, "chkIsEnableAbsMsrtIR");
            this.chkIsEnableAbsMsrtIR.Name = "chkIsEnableAbsMsrtIR";
            this.chkIsEnableAbsMsrtIR.Style = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
            this.chkIsEnableAbsMsrtIR.TabStop = false;
            // 
            // chkIsEnableKeepRecoveryData
            // 
            this.chkIsEnableKeepRecoveryData.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.chkIsEnableKeepRecoveryData.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10026;
            this.chkIsEnableKeepRecoveryData.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.chkIsEnableKeepRecoveryData.CheckSignSize = new System.Drawing.Size(15, 15);
            resources.ApplyResources(this.chkIsEnableKeepRecoveryData, "chkIsEnableKeepRecoveryData");
            this.chkIsEnableKeepRecoveryData.Name = "chkIsEnableKeepRecoveryData";
            this.chkIsEnableKeepRecoveryData.Style = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
            this.chkIsEnableKeepRecoveryData.TabStop = false;
            // 
            // numVRDelayTime
            // 
            this.numVRDelayTime.AntiAlias = true;
            // 
            // 
            // 
            this.numVRDelayTime.BackgroundStyle.BorderColor = System.Drawing.Color.CornflowerBlue;
            this.numVRDelayTime.BackgroundStyle.Class = "DateTimeInputBackground";
            this.numVRDelayTime.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            resources.ApplyResources(this.numVRDelayTime, "numVRDelayTime");
            this.numVRDelayTime.MaxValue = 15;
            this.numVRDelayTime.MinValue = 0;
            this.numVRDelayTime.Name = "numVRDelayTime";
            this.numVRDelayTime.ShowUpDown = true;
            this.numVRDelayTime.Value = 15;
            // 
            // labelX2
            // 
            this.labelX2.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX2.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10026;
            this.labelX2.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            resources.ApplyResources(this.labelX2, "labelX2");
            this.labelX2.ForeColor = System.Drawing.Color.Black;
            this.labelX2.Name = "labelX2";
            this.labelX2.TextAlignment = System.Drawing.StringAlignment.Center;
            // 
            // chkIsEnableVRDelayTime
            // 
            this.chkIsEnableVRDelayTime.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.chkIsEnableVRDelayTime.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10026;
            this.chkIsEnableVRDelayTime.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.chkIsEnableVRDelayTime.CheckSignSize = new System.Drawing.Size(15, 15);
            resources.ApplyResources(this.chkIsEnableVRDelayTime, "chkIsEnableVRDelayTime");
            this.chkIsEnableVRDelayTime.Name = "chkIsEnableVRDelayTime";
            this.chkIsEnableVRDelayTime.Style = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
            this.chkIsEnableVRDelayTime.TabStop = false;
            // 
            // lblHighSpeedModeDelayTime
            // 
            this.lblHighSpeedModeDelayTime.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.lblHighSpeedModeDelayTime.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10026;
            this.lblHighSpeedModeDelayTime.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            resources.ApplyResources(this.lblHighSpeedModeDelayTime, "lblHighSpeedModeDelayTime");
            this.lblHighSpeedModeDelayTime.ForeColor = System.Drawing.Color.Red;
            this.lblHighSpeedModeDelayTime.Name = "lblHighSpeedModeDelayTime";
            this.lblHighSpeedModeDelayTime.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
            // 
            // numHighSpeedModeDelayTime
            // 
            this.numHighSpeedModeDelayTime.AntiAlias = true;
            // 
            // 
            // 
            this.numHighSpeedModeDelayTime.BackgroundStyle.BorderColor = System.Drawing.Color.CornflowerBlue;
            this.numHighSpeedModeDelayTime.BackgroundStyle.Class = "DateTimeInputBackground";
            this.numHighSpeedModeDelayTime.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            resources.ApplyResources(this.numHighSpeedModeDelayTime, "numHighSpeedModeDelayTime");
            this.numHighSpeedModeDelayTime.ForeColor = System.Drawing.Color.Red;
            this.numHighSpeedModeDelayTime.MaxValue = 15;
            this.numHighSpeedModeDelayTime.MinValue = 0;
            this.numHighSpeedModeDelayTime.Name = "numHighSpeedModeDelayTime";
            this.numHighSpeedModeDelayTime.ShowUpDown = true;
            this.numHighSpeedModeDelayTime.Value = 15;
            // 
            // labelX1
            // 
            this.labelX1.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX1.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10026;
            this.labelX1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            resources.ApplyResources(this.labelX1, "labelX1");
            this.labelX1.ForeColor = System.Drawing.Color.Black;
            this.labelX1.Name = "labelX1";
            this.labelX1.TextAlignment = System.Drawing.StringAlignment.Center;
            // 
            // ipRthAddress
            // 
            this.ipRthAddress.AutoOverwrite = true;
            // 
            // 
            // 
            this.ipRthAddress.BackgroundStyle.Class = "DateTimeInputBackground";
            this.ipRthAddress.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.ipRthAddress.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.ipRthAddress.ButtonFreeText.Visible = true;
            resources.ApplyResources(this.ipRthAddress, "ipRthAddress");
            this.ipRthAddress.Name = "ipRthAddress";
            this.ipRthAddress.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            // 
            // lblrthIPAddress
            // 
            this.lblrthIPAddress.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.lblrthIPAddress.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10026;
            this.lblrthIPAddress.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            resources.ApplyResources(this.lblrthIPAddress, "lblrthIPAddress");
            this.lblrthIPAddress.ForeColor = System.Drawing.Color.Black;
            this.lblrthIPAddress.Name = "lblrthIPAddress";
            this.lblrthIPAddress.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
            // 
            // chkIsEnableRTHTestItem
            // 
            this.chkIsEnableRTHTestItem.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.chkIsEnableRTHTestItem.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10026;
            this.chkIsEnableRTHTestItem.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.chkIsEnableRTHTestItem.CheckSignSize = new System.Drawing.Size(15, 15);
            resources.ApplyResources(this.chkIsEnableRTHTestItem, "chkIsEnableRTHTestItem");
            this.chkIsEnableRTHTestItem.Name = "chkIsEnableRTHTestItem";
            this.chkIsEnableRTHTestItem.Style = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
            this.chkIsEnableRTHTestItem.TabStop = false;
            // 
            // lblESDHighSpeedDelayTime
            // 
            this.lblESDHighSpeedDelayTime.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.lblESDHighSpeedDelayTime.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10026;
            this.lblESDHighSpeedDelayTime.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            resources.ApplyResources(this.lblESDHighSpeedDelayTime, "lblESDHighSpeedDelayTime");
            this.lblESDHighSpeedDelayTime.ForeColor = System.Drawing.Color.Black;
            this.lblESDHighSpeedDelayTime.Name = "lblESDHighSpeedDelayTime";
            this.lblESDHighSpeedDelayTime.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
            // 
            // numESDHighSpeedDelayTime
            // 
            this.numESDHighSpeedDelayTime.AntiAlias = true;
            // 
            // 
            // 
            this.numESDHighSpeedDelayTime.BackgroundStyle.BorderColor = System.Drawing.Color.CornflowerBlue;
            this.numESDHighSpeedDelayTime.BackgroundStyle.Class = "DateTimeInputBackground";
            this.numESDHighSpeedDelayTime.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            resources.ApplyResources(this.numESDHighSpeedDelayTime, "numESDHighSpeedDelayTime");
            this.numESDHighSpeedDelayTime.MaxValue = 15;
            this.numESDHighSpeedDelayTime.MinValue = 0;
            this.numESDHighSpeedDelayTime.Name = "numESDHighSpeedDelayTime";
            this.numESDHighSpeedDelayTime.ShowUpDown = true;
            this.numESDHighSpeedDelayTime.Value = 15;
            // 
            // labelX6
            // 
            this.labelX6.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX6.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10026;
            this.labelX6.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            resources.ApplyResources(this.labelX6, "labelX6");
            this.labelX6.ForeColor = System.Drawing.Color.Black;
            this.labelX6.Name = "labelX6";
            this.labelX6.TextAlignment = System.Drawing.StringAlignment.Center;
            // 
            // chkIsEnableESDHighSpeedMode
            // 
            this.chkIsEnableESDHighSpeedMode.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.chkIsEnableESDHighSpeedMode.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10026;
            this.chkIsEnableESDHighSpeedMode.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.chkIsEnableESDHighSpeedMode.CheckSignSize = new System.Drawing.Size(15, 15);
            resources.ApplyResources(this.chkIsEnableESDHighSpeedMode, "chkIsEnableESDHighSpeedMode");
            this.chkIsEnableESDHighSpeedMode.Name = "chkIsEnableESDHighSpeedMode";
            this.chkIsEnableESDHighSpeedMode.Style = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
            this.chkIsEnableESDHighSpeedMode.TabStop = false;
            // 
            // btnSave
            // 
            this.btnSave.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnSave.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.btnSave, "btnSave");
            this.btnSave.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnSave.Image = global::MPI.Tester.Gui.Properties.Resources.btnSaveFile_B;
            this.btnSave.Name = "btnSave";
            this.btnSave.Shape = new DevComponents.DotNetBar.RoundRectangleShapeDescriptor(8);
            this.btnSave.Style = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
            this.btnSave.Tag = "40";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnReload
            // 
            this.btnReload.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnReload.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.btnReload, "btnReload");
            this.btnReload.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnReload.Image = global::MPI.Tester.Gui.Properties.Resources.btnUndo;
            this.btnReload.Name = "btnReload";
            this.btnReload.Shape = new DevComponents.DotNetBar.RoundRectangleShapeDescriptor(8);
            this.btnReload.Style = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
            this.btnReload.Tag = "40";
            this.btnReload.Click += new System.EventHandler(this.btnReload_Click);
            // 
            // gpParameterSetting
            // 
            this.gpParameterSetting.Controls.Add(this.btnSetIO);
            this.gpParameterSetting.Controls.Add(this.cmbTesterConfigType);
            this.gpParameterSetting.Controls.Add(this.lblTesterType);
            this.gpParameterSetting.Controls.Add(this.labelX3);
            this.gpParameterSetting.Controls.Add(this.lblHighSpeedModeDelayTime);
            this.gpParameterSetting.Controls.Add(this.numESDPrechargeWaitTime);
            this.gpParameterSetting.Controls.Add(this.chkIsEnableRTHTestItem);
            this.gpParameterSetting.Controls.Add(this.labelX4);
            this.gpParameterSetting.Controls.Add(this.chkIsEnableESDHighSpeedMode);
            this.gpParameterSetting.Controls.Add(this.lblRTHTdTime);
            this.gpParameterSetting.Controls.Add(this.lblESDPrechargeWaitTime);
            this.gpParameterSetting.Controls.Add(this.dinRTHTdTime);
            this.gpParameterSetting.Controls.Add(this.labelX6);
            this.gpParameterSetting.Controls.Add(this.ipRthAddress);
            this.gpParameterSetting.Controls.Add(this.numMDParallelDelayTime);
            this.gpParameterSetting.Controls.Add(this.lblrthIPAddress);
            this.gpParameterSetting.Controls.Add(this.numESDHighSpeedDelayTime);
            this.gpParameterSetting.Controls.Add(this.labelX7);
            this.gpParameterSetting.Controls.Add(this.lblESDHighSpeedDelayTime);
            this.gpParameterSetting.Controls.Add(this.labelX5);
            this.gpParameterSetting.Controls.Add(this.numMDSeriesDelayTime);
            this.gpParameterSetting.Controls.Add(this.lblMDSeriesDelayTime);
            this.gpParameterSetting.Controls.Add(this.lblMDParallelDelayTime);
            this.gpParameterSetting.Controls.Add(this.labelX1);
            this.gpParameterSetting.Controls.Add(this.numHighSpeedModeDelayTime);
            this.gpParameterSetting.Controls.Add(this.chkIsEnableVRDelayTime);
            this.gpParameterSetting.Controls.Add(this.labelX2);
            this.gpParameterSetting.Controls.Add(this.chkIsEnableAbsMsrtIR);
            this.gpParameterSetting.Controls.Add(this.numVRDelayTime);
            this.gpParameterSetting.Controls.Add(this.chkIsEnableKeepRecoveryData);
            resources.ApplyResources(this.gpParameterSetting, "gpParameterSetting");
            this.gpParameterSetting.ForeColor = System.Drawing.Color.Black;
            this.gpParameterSetting.Name = "gpParameterSetting";
            this.gpParameterSetting.TabStop = false;
            // 
            // btnSetIO
            // 
            resources.ApplyResources(this.btnSetIO, "btnSetIO");
            this.btnSetIO.ForeColor = System.Drawing.Color.Black;
            this.btnSetIO.Name = "btnSetIO";
            this.btnSetIO.UseVisualStyleBackColor = true;
            this.btnSetIO.Click += new System.EventHandler(this.btnSetIO_Click);
            // 
            // cmbTesterConfigType
            // 
            this.cmbTesterConfigType.DisplayMember = "Text";
            this.cmbTesterConfigType.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbTesterConfigType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTesterConfigType.ForeColor = System.Drawing.Color.Green;
            this.cmbTesterConfigType.FormattingEnabled = true;
            resources.ApplyResources(this.cmbTesterConfigType, "cmbTesterConfigType");
            this.cmbTesterConfigType.Name = "cmbTesterConfigType";
            this.cmbTesterConfigType.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            // 
            // lblTesterType
            // 
            this.lblTesterType.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.lblTesterType.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10026;
            this.lblTesterType.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            resources.ApplyResources(this.lblTesterType, "lblTesterType");
            this.lblTesterType.ForeColor = System.Drawing.Color.Green;
            this.lblTesterType.Name = "lblTesterType";
            this.lblTesterType.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
            // 
            // gpTestItemSpecSetting
            // 
            this.gpTestItemSpecSetting.Controls.Add(this.lblSettingInfo);
            this.gpTestItemSpecSetting.Controls.Add(this.btnResetSelectedItem);
            this.gpTestItemSpecSetting.Controls.Add(this.label1);
            this.gpTestItemSpecSetting.Controls.Add(this.btnEditEnableItems);
            this.gpTestItemSpecSetting.Controls.Add(this.lstBoxEnableTestItems);
            this.gpTestItemSpecSetting.Controls.Add(this.lblItemBoundaryTitle);
            this.gpTestItemSpecSetting.Controls.Add(this.dgvItemBoundary);
            resources.ApplyResources(this.gpTestItemSpecSetting, "gpTestItemSpecSetting");
            this.gpTestItemSpecSetting.ForeColor = System.Drawing.Color.Black;
            this.gpTestItemSpecSetting.Name = "gpTestItemSpecSetting";
            this.gpTestItemSpecSetting.TabStop = false;
            // 
            // lblSettingInfo
            // 
            resources.ApplyResources(this.lblSettingInfo, "lblSettingInfo");
            this.lblSettingInfo.Name = "lblSettingInfo";
            // 
            // btnResetSelectedItem
            // 
            resources.ApplyResources(this.btnResetSelectedItem, "btnResetSelectedItem");
            this.btnResetSelectedItem.ForeColor = System.Drawing.Color.Black;
            this.btnResetSelectedItem.Name = "btnResetSelectedItem";
            this.btnResetSelectedItem.UseVisualStyleBackColor = true;
            this.btnResetSelectedItem.Click += new System.EventHandler(this.btnResetSelectedItem_Click);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Name = "label1";
            // 
            // btnEditEnableItems
            // 
            resources.ApplyResources(this.btnEditEnableItems, "btnEditEnableItems");
            this.btnEditEnableItems.ForeColor = System.Drawing.Color.Black;
            this.btnEditEnableItems.Name = "btnEditEnableItems";
            this.btnEditEnableItems.UseVisualStyleBackColor = true;
            this.btnEditEnableItems.Click += new System.EventHandler(this.btnEditEnableItems_Click);
            // 
            // lstBoxEnableTestItems
            // 
            resources.ApplyResources(this.lstBoxEnableTestItems, "lstBoxEnableTestItems");
            this.lstBoxEnableTestItems.FormattingEnabled = true;
            this.lstBoxEnableTestItems.Name = "lstBoxEnableTestItems";
            this.lstBoxEnableTestItems.SelectedIndexChanged += new System.EventHandler(this.lstEnableTestItems_SelectedIndexChanged);
            // 
            // lblItemBoundaryTitle
            // 
            resources.ApplyResources(this.lblItemBoundaryTitle, "lblItemBoundaryTitle");
            this.lblItemBoundaryTitle.ForeColor = System.Drawing.Color.Black;
            this.lblItemBoundaryTitle.Name = "lblItemBoundaryTitle";
            // 
            // lblUserID
            // 
            resources.ApplyResources(this.lblUserID, "lblUserID");
            this.lblUserID.Name = "lblUserID";
            // 
            // colpropKeyName
            // 
            resources.ApplyResources(this.colpropKeyName, "colpropKeyName");
            this.colpropKeyName.Name = "colpropKeyName";
            this.colpropKeyName.ReadOnly = true;
            this.colpropKeyName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // colIsVisible
            // 
            this.colIsVisible.Checked = true;
            this.colIsVisible.CheckState = System.Windows.Forms.CheckState.Indeterminate;
            this.colIsVisible.CheckValue = "N";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.colIsVisible.DefaultCellStyle = dataGridViewCellStyle2;
            this.colIsVisible.FillWeight = 30F;
            resources.ApplyResources(this.colIsVisible, "colIsVisible");
            this.colIsVisible.Name = "colIsVisible";
            // 
            // colpropMin
            // 
            resources.ApplyResources(this.colpropMin, "colpropMin");
            this.colpropMin.Name = "colpropMin";
            this.colpropMin.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colpropMin.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // colpropMax
            // 
            resources.ApplyResources(this.colpropMax, "colpropMax");
            this.colpropMax.Name = "colpropMax";
            this.colpropMax.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colpropMax.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // colpropDefault
            // 
            resources.ApplyResources(this.colpropDefault, "colpropDefault");
            this.colpropDefault.Name = "colpropDefault";
            this.colpropDefault.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colpropDefault.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // colpropUnit
            // 
            resources.ApplyResources(this.colpropUnit, "colpropUnit");
            this.colpropUnit.Name = "colpropUnit";
            this.colpropUnit.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // colpropFormat
            // 
            resources.ApplyResources(this.colpropFormat, "colpropFormat");
            this.colpropFormat.Name = "colpropFormat";
            this.colpropFormat.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // frmSetRDFunc
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.BackgroundImage = global::MPI.Tester.Gui.Properties.Resources.mini_hero_bg;
            this.Controls.Add(this.lblUserID);
            this.Controls.Add(this.gpTestItemSpecSetting);
            this.Controls.Add(this.gpParameterSetting);
            this.Controls.Add(this.btnReload);
            this.Controls.Add(this.btnSave);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmSetRDFunc";
            ((System.ComponentModel.ISupportInitialize)(this.dgvItemBoundary)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numESDPrechargeWaitTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMDParallelDelayTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMDSeriesDelayTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dinRTHTdTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numVRDelayTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numHighSpeedModeDelayTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ipRthAddress)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numESDHighSpeedDelayTime)).EndInit();
            this.gpParameterSetting.ResumeLayout(false);
            this.gpTestItemSpecSetting.ResumeLayout(false);
            this.gpTestItemSpecSetting.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

        private DevComponents.DotNetBar.ButtonX btnSave;
		private DevComponents.DotNetBar.ButtonX btnReload;
        private DevComponents.DotNetBar.Controls.CheckBoxX chkIsEnableESDHighSpeedMode;
        private DevComponents.DotNetBar.LabelX lblESDHighSpeedDelayTime;
        private DevComponents.Editors.IntegerInput numESDHighSpeedDelayTime;
        private DevComponents.DotNetBar.LabelX labelX6;
        private DevComponents.DotNetBar.Controls.CheckBoxX chkIsEnableRTHTestItem;
        private DevComponents.Editors.IpAddressInput ipRthAddress;
		private DevComponents.DotNetBar.LabelX lblrthIPAddress;
		private DevComponents.DotNetBar.LabelX lblHighSpeedModeDelayTime;
		private DevComponents.Editors.IntegerInput numHighSpeedModeDelayTime;
		private DevComponents.DotNetBar.LabelX labelX1;
		private DevComponents.DotNetBar.Controls.CheckBoxX chkIsEnableVRDelayTime;
		private DevComponents.Editors.IntegerInput numVRDelayTime;
		private DevComponents.DotNetBar.LabelX labelX2;
        private DevComponents.DotNetBar.Controls.CheckBoxX chkIsEnableKeepRecoveryData;
        private DevComponents.DotNetBar.Controls.CheckBoxX chkIsEnableAbsMsrtIR;
        private DevComponents.DotNetBar.LabelX lblRTHTdTime;
        private DevComponents.DotNetBar.LabelX labelX4;
        private DevComponents.Editors.DoubleInput dinRTHTdTime;
        private DevComponents.Editors.IntegerInput numMDParallelDelayTime;
        private DevComponents.DotNetBar.LabelX labelX7;
        private DevComponents.DotNetBar.LabelX labelX5;
        private DevComponents.Editors.IntegerInput numMDSeriesDelayTime;
        private DevComponents.DotNetBar.LabelX lblMDSeriesDelayTime;
        private DevComponents.DotNetBar.LabelX lblMDParallelDelayTime;
        private DevComponents.DotNetBar.LabelX labelX3;
        private DevComponents.Editors.IntegerInput numESDPrechargeWaitTime;
        private DevComponents.DotNetBar.LabelX lblESDPrechargeWaitTime;
        private System.Windows.Forms.GroupBox gpParameterSetting;
        private System.Windows.Forms.GroupBox gpTestItemSpecSetting;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnEditEnableItems;
        private System.Windows.Forms.ListBox lstBoxEnableTestItems;
        private System.Windows.Forms.Label lblItemBoundaryTitle;
		private DevComponents.DotNetBar.Controls.DataGridViewX dgvItemBoundary;
        private System.Windows.Forms.Label lblUserID;
        private System.Windows.Forms.Button btnResetSelectedItem;
        private System.Windows.Forms.Label lblSettingInfo;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cmbTesterConfigType;
        private DevComponents.DotNetBar.LabelX lblTesterType;
        private System.Windows.Forms.Button btnSetIO;
        private System.Windows.Forms.DataGridViewTextBoxColumn colpropKeyName;
        private DevComponents.DotNetBar.Controls.DataGridViewCheckBoxXColumn colIsVisible;
        private System.Windows.Forms.DataGridViewTextBoxColumn colpropMin;
        private System.Windows.Forms.DataGridViewTextBoxColumn colpropMax;
        private System.Windows.Forms.DataGridViewTextBoxColumn colpropDefault;
        private System.Windows.Forms.DataGridViewTextBoxColumn colpropUnit;
        private System.Windows.Forms.DataGridViewTextBoxColumn colpropFormat;
	}
}
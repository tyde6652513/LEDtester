namespace MPI.Tester.Gui
{
    partial class frmSetParameter
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSetParameter));
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.labelX3 = new DevComponents.DotNetBar.LabelX();
            this.labelX7 = new DevComponents.DotNetBar.LabelX();
            this.labelX15 = new DevComponents.DotNetBar.LabelX();
            this.checkBox3 = new System.Windows.Forms.CheckBox();
            this.groupPanel5 = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.groupPanel7 = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.labelX6 = new DevComponents.DotNetBar.LabelX();
            this.labelX5 = new DevComponents.DotNetBar.LabelX();
            this.dinCollectArea = new DevComponents.Editors.DoubleInput();
            this.dinFiberDiameter = new DevComponents.Editors.DoubleInput();
            this.radCollect = new System.Windows.Forms.RadioButton();
            this.radFiber = new System.Windows.Forms.RadioButton();
            this.radSphere = new System.Windows.Forms.RadioButton();
            this.chkHWTrigger = new System.Windows.Forms.CheckBox();
            this.txtLimitStartTime = new System.Windows.Forms.TextBox();
            this.txtLimitTargetCount = new System.Windows.Forms.TextBox();
            this.txtAutoMaxCount = new System.Windows.Forms.TextBox();
            this.txtAutoMinCount = new System.Windows.Forms.TextBox();
            this.chkSptMeterCalc = new System.Windows.Forms.CheckBox();
            this.chkSptMeterTrigger = new System.Windows.Forms.CheckBox();
            this.chkGetRowData = new System.Windows.Forms.CheckBox();
            this.labelX28 = new DevComponents.DotNetBar.LabelX();
            this.labelX27 = new DevComponents.DotNetBar.LabelX();
            this.labelX26 = new DevComponents.DotNetBar.LabelX();
            this.labelX25 = new DevComponents.DotNetBar.LabelX();
            this.txtLimiMaxCount = new System.Windows.Forms.TextBox();
            this.txtLimitMinCount = new System.Windows.Forms.TextBox();
            this.txtMinCatchCount = new System.Windows.Forms.TextBox();
            this.labelX24 = new DevComponents.DotNetBar.LabelX();
            this.labelX23 = new DevComponents.DotNetBar.LabelX();
            this.txtCieObserver = new System.Windows.Forms.TextBox();
            this.txtCieIlluminant = new System.Windows.Forms.TextBox();
            this.txtScanAverage = new System.Windows.Forms.TextBox();
            this.txtBoxCar = new System.Windows.Forms.TextBox();
            this.txtEndWave = new System.Windows.Forms.TextBox();
            this.txtStartWave = new System.Windows.Forms.TextBox();
            this.txtSptOperationMode = new System.Windows.Forms.TextBox();
            this.labelX19 = new DevComponents.DotNetBar.LabelX();
            this.dinWattCoeff = new DevComponents.Editors.DoubleInput();
            this.dinLumensCoeff = new DevComponents.Editors.DoubleInput();
            this.labelX14 = new DevComponents.DotNetBar.LabelX();
            this.labelX13 = new DevComponents.DotNetBar.LabelX();
            this.labelX16 = new DevComponents.DotNetBar.LabelX();
            this.labelX11 = new DevComponents.DotNetBar.LabelX();
            this.chkCorrectForNonlineatity = new System.Windows.Forms.CheckBox();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.labelX4 = new DevComponents.DotNetBar.LabelX();
            this.zedCalibration = new ZedGraph.ZedGraphControl();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.labelX18 = new DevComponents.DotNetBar.LabelX();
            this.zedDarkArray = new ZedGraph.ZedGraphControl();
            this.chkEnableAutoGetdark = new System.Windows.Forms.CheckBox();
            this.btnGetCurrentDark = new DevComponents.DotNetBar.ButtonX();
            this.lblDarkAvg = new DevComponents.DotNetBar.LabelX();
            this.labelX20 = new DevComponents.DotNetBar.LabelX();
            this.lblCurrentDark = new DevComponents.DotNetBar.LabelX();
            this.labelX21 = new DevComponents.DotNetBar.LabelX();
            this.btnSaveDarkArrayToSystem = new DevComponents.DotNetBar.ButtonX();
            this.btnLoadDarkArray = new DevComponents.DotNetBar.ButtonX();
            this.btnSaveCaliSpectrumToSystem = new DevComponents.DotNetBar.ButtonX();
            this.btnCancel = new DevComponents.DotNetBar.ButtonX();
            this.btnLoadCaliFile = new DevComponents.DotNetBar.ButtonX();
            this.btnSetParameter = new DevComponents.DotNetBar.ButtonX();
            this.groupPanel5.SuspendLayout();
            this.groupPanel7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dinCollectArea)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dinFiberDiameter)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dinWattCoeff)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dinLumensCoeff)).BeginInit();
            this.SuspendLayout();
            // 
            // labelX2
            // 
            this.labelX2.BackColor = System.Drawing.Color.PaleTurquoise;
            // 
            // 
            // 
            this.labelX2.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10001;
            this.labelX2.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.labelX2.Location = new System.Drawing.Point(5, 111);
            this.labelX2.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(173, 28);
            this.labelX2.TabIndex = 225;
            this.labelX2.Text = "Box Car";
            // 
            // labelX3
            // 
            this.labelX3.BackColor = System.Drawing.Color.PaleTurquoise;
            // 
            // 
            // 
            this.labelX3.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10001;
            this.labelX3.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.labelX3.Location = new System.Drawing.Point(4, 3);
            this.labelX3.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.labelX3.Name = "labelX3";
            this.labelX3.Size = new System.Drawing.Size(173, 28);
            this.labelX3.TabIndex = 232;
            this.labelX3.Text = "Operate Mode";
            // 
            // labelX7
            // 
            this.labelX7.BackColor = System.Drawing.Color.PaleTurquoise;
            // 
            // 
            // 
            this.labelX7.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10001;
            this.labelX7.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.labelX7.Location = new System.Drawing.Point(4, 286);
            this.labelX7.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.labelX7.Name = "labelX7";
            this.labelX7.Size = new System.Drawing.Size(173, 28);
            this.labelX7.TabIndex = 231;
            this.labelX7.Text = "Limit Min Count";
            // 
            // labelX15
            // 
            this.labelX15.BackColor = System.Drawing.Color.PaleTurquoise;
            // 
            // 
            // 
            this.labelX15.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10001;
            this.labelX15.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX15.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.labelX15.Location = new System.Drawing.Point(5, 147);
            this.labelX15.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.labelX15.Name = "labelX15";
            this.labelX15.Size = new System.Drawing.Size(173, 28);
            this.labelX15.TabIndex = 226;
            this.labelX15.Text = "Scan Average";
            // 
            // checkBox3
            // 
            this.checkBox3.AutoSize = true;
            this.checkBox3.Checked = true;
            this.checkBox3.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBox3.ForeColor = System.Drawing.Color.DarkBlue;
            this.checkBox3.Location = new System.Drawing.Point(389, 309);
            this.checkBox3.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.checkBox3.Name = "checkBox3";
            this.checkBox3.Size = new System.Drawing.Size(127, 28);
            this.checkBox3.TabIndex = 101;
            this.checkBox3.Text = "Attenuator";
            this.checkBox3.UseVisualStyleBackColor = true;
            // 
            // groupPanel5
            // 
            this.groupPanel5.BackColor = System.Drawing.Color.Transparent;
            this.groupPanel5.CanvasColor = System.Drawing.Color.Empty;
            this.groupPanel5.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
            this.groupPanel5.Controls.Add(this.groupPanel7);
            this.groupPanel5.Controls.Add(this.chkHWTrigger);
            this.groupPanel5.Controls.Add(this.txtLimitStartTime);
            this.groupPanel5.Controls.Add(this.txtLimitTargetCount);
            this.groupPanel5.Controls.Add(this.txtAutoMaxCount);
            this.groupPanel5.Controls.Add(this.txtAutoMinCount);
            this.groupPanel5.Controls.Add(this.chkSptMeterCalc);
            this.groupPanel5.Controls.Add(this.chkSptMeterTrigger);
            this.groupPanel5.Controls.Add(this.chkGetRowData);
            this.groupPanel5.Controls.Add(this.labelX28);
            this.groupPanel5.Controls.Add(this.labelX27);
            this.groupPanel5.Controls.Add(this.labelX26);
            this.groupPanel5.Controls.Add(this.labelX25);
            this.groupPanel5.Controls.Add(this.txtLimiMaxCount);
            this.groupPanel5.Controls.Add(this.txtLimitMinCount);
            this.groupPanel5.Controls.Add(this.txtMinCatchCount);
            this.groupPanel5.Controls.Add(this.labelX24);
            this.groupPanel5.Controls.Add(this.labelX23);
            this.groupPanel5.Controls.Add(this.txtCieObserver);
            this.groupPanel5.Controls.Add(this.txtCieIlluminant);
            this.groupPanel5.Controls.Add(this.txtScanAverage);
            this.groupPanel5.Controls.Add(this.txtBoxCar);
            this.groupPanel5.Controls.Add(this.txtEndWave);
            this.groupPanel5.Controls.Add(this.txtStartWave);
            this.groupPanel5.Controls.Add(this.txtSptOperationMode);
            this.groupPanel5.Controls.Add(this.labelX19);
            this.groupPanel5.Controls.Add(this.dinWattCoeff);
            this.groupPanel5.Controls.Add(this.dinLumensCoeff);
            this.groupPanel5.Controls.Add(this.labelX14);
            this.groupPanel5.Controls.Add(this.labelX13);
            this.groupPanel5.Controls.Add(this.labelX16);
            this.groupPanel5.Controls.Add(this.labelX11);
            this.groupPanel5.Controls.Add(this.chkCorrectForNonlineatity);
            this.groupPanel5.Controls.Add(this.labelX1);
            this.groupPanel5.Controls.Add(this.labelX4);
            this.groupPanel5.Controls.Add(this.checkBox3);
            this.groupPanel5.Controls.Add(this.labelX15);
            this.groupPanel5.Controls.Add(this.labelX7);
            this.groupPanel5.Controls.Add(this.labelX3);
            this.groupPanel5.Controls.Add(this.labelX2);
            this.groupPanel5.DrawTitleBox = false;
            this.groupPanel5.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupPanel5.Location = new System.Drawing.Point(12, 8);
            this.groupPanel5.Margin = new System.Windows.Forms.Padding(0);
            this.groupPanel5.Name = "groupPanel5";
            this.groupPanel5.Size = new System.Drawing.Size(1280, 387);
            // 
            // 
            // 
            this.groupPanel5.Style.BackColor = System.Drawing.Color.WhiteSmoke;
            this.groupPanel5.Style.BackColor2 = System.Drawing.Color.Lavender;
            this.groupPanel5.Style.BackColorGradientAngle = 90;
            this.groupPanel5.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel5.Style.BorderBottomWidth = 1;
            this.groupPanel5.Style.BorderColor = System.Drawing.Color.Gray;
            this.groupPanel5.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel5.Style.BorderLeftWidth = 1;
            this.groupPanel5.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel5.Style.BorderRightWidth = 1;
            this.groupPanel5.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel5.Style.BorderTopWidth = 1;
            this.groupPanel5.Style.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10001;
            this.groupPanel5.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded;
            this.groupPanel5.Style.TextColor = System.Drawing.Color.SteelBlue;
            // 
            // 
            // 
            this.groupPanel5.StyleMouseDown.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10001;
            this.groupPanel5.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this.groupPanel5.StyleMouseOver.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10001;
            this.groupPanel5.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.groupPanel5.TabIndex = 243;
            this.groupPanel5.Text = "Spectrometer Setting";
            // 
            // groupPanel7
            // 
            this.groupPanel7.BackColor = System.Drawing.Color.Transparent;
            this.groupPanel7.CanvasColor = System.Drawing.Color.Empty;
            this.groupPanel7.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
            this.groupPanel7.Controls.Add(this.labelX6);
            this.groupPanel7.Controls.Add(this.labelX5);
            this.groupPanel7.Controls.Add(this.dinCollectArea);
            this.groupPanel7.Controls.Add(this.dinFiberDiameter);
            this.groupPanel7.Controls.Add(this.radCollect);
            this.groupPanel7.Controls.Add(this.radFiber);
            this.groupPanel7.Controls.Add(this.radSphere);
            this.groupPanel7.DrawTitleBox = false;
            this.groupPanel7.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupPanel7.IsShadowEnabled = true;
            this.groupPanel7.Location = new System.Drawing.Point(705, 181);
            this.groupPanel7.Margin = new System.Windows.Forms.Padding(0);
            this.groupPanel7.Name = "groupPanel7";
            this.groupPanel7.Size = new System.Drawing.Size(536, 135);
            // 
            // 
            // 
            this.groupPanel7.Style.BackColor = System.Drawing.Color.WhiteSmoke;
            this.groupPanel7.Style.BackColor2 = System.Drawing.Color.Lavender;
            this.groupPanel7.Style.BackColorGradientAngle = 90;
            this.groupPanel7.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel7.Style.BorderBottomWidth = 1;
            this.groupPanel7.Style.BorderColor = System.Drawing.Color.Gray;
            this.groupPanel7.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel7.Style.BorderLeftWidth = 1;
            this.groupPanel7.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel7.Style.BorderRightWidth = 1;
            this.groupPanel7.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel7.Style.BorderTopWidth = 1;
            this.groupPanel7.Style.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10001;
            this.groupPanel7.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded;
            this.groupPanel7.Style.TextColor = System.Drawing.Color.SteelBlue;
            // 
            // 
            // 
            this.groupPanel7.StyleMouseDown.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10001;
            this.groupPanel7.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this.groupPanel7.StyleMouseOver.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10001;
            this.groupPanel7.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.groupPanel7.TabIndex = 277;
            this.groupPanel7.Text = "Device";
            // 
            // labelX6
            // 
            // 
            // 
            // 
            this.labelX6.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10001;
            this.labelX6.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.labelX6.Location = new System.Drawing.Point(452, 63);
            this.labelX6.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.labelX6.Name = "labelX6";
            this.labelX6.Size = new System.Drawing.Size(135, 29);
            this.labelX6.TabIndex = 227;
            this.labelX6.Text = "cm^2";
            // 
            // labelX5
            // 
            // 
            // 
            // 
            this.labelX5.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10001;
            this.labelX5.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.labelX5.Location = new System.Drawing.Point(452, 32);
            this.labelX5.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.labelX5.Name = "labelX5";
            this.labelX5.Size = new System.Drawing.Size(135, 29);
            this.labelX5.TabIndex = 226;
            this.labelX5.Text = "um";
            // 
            // dinCollectArea
            // 
            // 
            // 
            // 
            this.dinCollectArea.BackgroundStyle.Class = "DateTimeInputBackground";
            this.dinCollectArea.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.dinCollectArea.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.dinCollectArea.DisplayFormat = "0.00000000";
            this.dinCollectArea.Increment = 1D;
            this.dinCollectArea.Location = new System.Drawing.Point(303, 70);
            this.dinCollectArea.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.dinCollectArea.Name = "dinCollectArea";
            this.dinCollectArea.ShowUpDown = true;
            this.dinCollectArea.Size = new System.Drawing.Size(136, 26);
            this.dinCollectArea.TabIndex = 4;
            this.dinCollectArea.Value = 0.002827433D;
            // 
            // dinFiberDiameter
            // 
            // 
            // 
            // 
            this.dinFiberDiameter.BackgroundStyle.Class = "DateTimeInputBackground";
            this.dinFiberDiameter.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.dinFiberDiameter.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.dinFiberDiameter.Increment = 1D;
            this.dinFiberDiameter.Location = new System.Drawing.Point(303, 40);
            this.dinFiberDiameter.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.dinFiberDiameter.Name = "dinFiberDiameter";
            this.dinFiberDiameter.ShowUpDown = true;
            this.dinFiberDiameter.Size = new System.Drawing.Size(136, 26);
            this.dinFiberDiameter.TabIndex = 3;
            this.dinFiberDiameter.Value = 600D;
            // 
            // radCollect
            // 
            this.radCollect.AutoSize = true;
            this.radCollect.Location = new System.Drawing.Point(72, 70);
            this.radCollect.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.radCollect.Name = "radCollect";
            this.radCollect.Size = new System.Drawing.Size(124, 23);
            this.radCollect.TabIndex = 2;
            this.radCollect.Text = "Collect Area";
            this.radCollect.UseVisualStyleBackColor = true;
            // 
            // radFiber
            // 
            this.radFiber.AutoSize = true;
            this.radFiber.Checked = true;
            this.radFiber.Location = new System.Drawing.Point(72, 40);
            this.radFiber.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.radFiber.Name = "radFiber";
            this.radFiber.Size = new System.Drawing.Size(191, 23);
            this.radFiber.TabIndex = 1;
            this.radFiber.TabStop = true;
            this.radFiber.Text = "Use Fiber : Diameter";
            this.radFiber.UseVisualStyleBackColor = true;
            // 
            // radSphere
            // 
            this.radSphere.AutoSize = true;
            this.radSphere.Location = new System.Drawing.Point(72, 10);
            this.radSphere.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.radSphere.Name = "radSphere";
            this.radSphere.Size = new System.Drawing.Size(202, 23);
            this.radSphere.TabIndex = 0;
            this.radSphere.Text = "Use Intgrating Sphere";
            this.radSphere.UseVisualStyleBackColor = true;
            // 
            // chkHWTrigger
            // 
            this.chkHWTrigger.AutoSize = true;
            this.chkHWTrigger.Checked = true;
            this.chkHWTrigger.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkHWTrigger.Enabled = false;
            this.chkHWTrigger.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkHWTrigger.ForeColor = System.Drawing.Color.DarkBlue;
            this.chkHWTrigger.Location = new System.Drawing.Point(389, 254);
            this.chkHWTrigger.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.chkHWTrigger.Name = "chkHWTrigger";
            this.chkHWTrigger.Size = new System.Drawing.Size(191, 24);
            this.chkHWTrigger.TabIndex = 276;
            this.chkHWTrigger.Text = "Enable HW Trigger";
            this.chkHWTrigger.UseVisualStyleBackColor = true;
            // 
            // txtLimitStartTime
            // 
            this.txtLimitStartTime.Location = new System.Drawing.Point(571, 110);
            this.txtLimitStartTime.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtLimitStartTime.Name = "txtLimitStartTime";
            this.txtLimitStartTime.ReadOnly = true;
            this.txtLimitStartTime.Size = new System.Drawing.Size(179, 26);
            this.txtLimitStartTime.TabIndex = 275;
            // 
            // txtLimitTargetCount
            // 
            this.txtLimitTargetCount.Location = new System.Drawing.Point(571, 77);
            this.txtLimitTargetCount.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtLimitTargetCount.Name = "txtLimitTargetCount";
            this.txtLimitTargetCount.ReadOnly = true;
            this.txtLimitTargetCount.Size = new System.Drawing.Size(179, 26);
            this.txtLimitTargetCount.TabIndex = 274;
            // 
            // txtAutoMaxCount
            // 
            this.txtAutoMaxCount.Location = new System.Drawing.Point(571, 39);
            this.txtAutoMaxCount.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtAutoMaxCount.Name = "txtAutoMaxCount";
            this.txtAutoMaxCount.ReadOnly = true;
            this.txtAutoMaxCount.Size = new System.Drawing.Size(179, 26);
            this.txtAutoMaxCount.TabIndex = 273;
            // 
            // txtAutoMinCount
            // 
            this.txtAutoMinCount.Location = new System.Drawing.Point(571, 3);
            this.txtAutoMinCount.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtAutoMinCount.Name = "txtAutoMinCount";
            this.txtAutoMinCount.ReadOnly = true;
            this.txtAutoMinCount.Size = new System.Drawing.Size(179, 26);
            this.txtAutoMinCount.TabIndex = 272;
            // 
            // chkSptMeterCalc
            // 
            this.chkSptMeterCalc.AutoSize = true;
            this.chkSptMeterCalc.Checked = true;
            this.chkSptMeterCalc.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSptMeterCalc.Enabled = false;
            this.chkSptMeterCalc.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkSptMeterCalc.ForeColor = System.Drawing.Color.DarkBlue;
            this.chkSptMeterCalc.Location = new System.Drawing.Point(389, 224);
            this.chkSptMeterCalc.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.chkSptMeterCalc.Name = "chkSptMeterCalc";
            this.chkSptMeterCalc.Size = new System.Drawing.Size(255, 24);
            this.chkSptMeterCalc.TabIndex = 271;
            this.chkSptMeterCalc.Text = "Enable SptMeter Calculate";
            this.chkSptMeterCalc.UseVisualStyleBackColor = true;
            // 
            // chkSptMeterTrigger
            // 
            this.chkSptMeterTrigger.AutoSize = true;
            this.chkSptMeterTrigger.Checked = true;
            this.chkSptMeterTrigger.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSptMeterTrigger.Enabled = false;
            this.chkSptMeterTrigger.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkSptMeterTrigger.ForeColor = System.Drawing.Color.DarkBlue;
            this.chkSptMeterTrigger.Location = new System.Drawing.Point(389, 194);
            this.chkSptMeterTrigger.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.chkSptMeterTrigger.Name = "chkSptMeterTrigger";
            this.chkSptMeterTrigger.Size = new System.Drawing.Size(236, 24);
            this.chkSptMeterTrigger.TabIndex = 270;
            this.chkSptMeterTrigger.Text = "Enable SptMeter Trigger";
            this.chkSptMeterTrigger.UseVisualStyleBackColor = true;
            // 
            // chkGetRowData
            // 
            this.chkGetRowData.AutoSize = true;
            this.chkGetRowData.Checked = true;
            this.chkGetRowData.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkGetRowData.Enabled = false;
            this.chkGetRowData.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkGetRowData.ForeColor = System.Drawing.Color.DarkBlue;
            this.chkGetRowData.Location = new System.Drawing.Point(389, 165);
            this.chkGetRowData.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.chkGetRowData.Name = "chkGetRowData";
            this.chkGetRowData.Size = new System.Drawing.Size(149, 24);
            this.chkGetRowData.TabIndex = 269;
            this.chkGetRowData.Text = "Get Row Data";
            this.chkGetRowData.UseVisualStyleBackColor = true;
            // 
            // labelX28
            // 
            this.labelX28.BackColor = System.Drawing.Color.PaleTurquoise;
            // 
            // 
            // 
            this.labelX28.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10001;
            this.labelX28.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX28.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.labelX28.Location = new System.Drawing.Point(389, 111);
            this.labelX28.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.labelX28.Name = "labelX28";
            this.labelX28.Size = new System.Drawing.Size(173, 28);
            this.labelX28.TabIndex = 268;
            this.labelX28.Text = "Limit Start Time";
            // 
            // labelX27
            // 
            this.labelX27.BackColor = System.Drawing.Color.PaleTurquoise;
            // 
            // 
            // 
            this.labelX27.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10001;
            this.labelX27.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX27.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.labelX27.Location = new System.Drawing.Point(389, 77);
            this.labelX27.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.labelX27.Name = "labelX27";
            this.labelX27.Size = new System.Drawing.Size(173, 28);
            this.labelX27.TabIndex = 267;
            this.labelX27.Text = "Limit Target Count";
            // 
            // labelX26
            // 
            this.labelX26.BackColor = System.Drawing.Color.PaleTurquoise;
            // 
            // 
            // 
            this.labelX26.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10001;
            this.labelX26.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX26.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.labelX26.Location = new System.Drawing.Point(389, 39);
            this.labelX26.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.labelX26.Name = "labelX26";
            this.labelX26.Size = new System.Drawing.Size(173, 28);
            this.labelX26.TabIndex = 266;
            this.labelX26.Text = "Auto Max Count";
            // 
            // labelX25
            // 
            this.labelX25.BackColor = System.Drawing.Color.PaleTurquoise;
            // 
            // 
            // 
            this.labelX25.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10001;
            this.labelX25.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX25.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.labelX25.Location = new System.Drawing.Point(389, 3);
            this.labelX25.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.labelX25.Name = "labelX25";
            this.labelX25.Size = new System.Drawing.Size(173, 28);
            this.labelX25.TabIndex = 265;
            this.labelX25.Text = "Auto Min Count";
            // 
            // txtLimiMaxCount
            // 
            this.txtLimiMaxCount.Location = new System.Drawing.Point(187, 321);
            this.txtLimiMaxCount.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtLimiMaxCount.Name = "txtLimiMaxCount";
            this.txtLimiMaxCount.ReadOnly = true;
            this.txtLimiMaxCount.Size = new System.Drawing.Size(179, 26);
            this.txtLimiMaxCount.TabIndex = 264;
            // 
            // txtLimitMinCount
            // 
            this.txtLimitMinCount.Location = new System.Drawing.Point(187, 286);
            this.txtLimitMinCount.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtLimitMinCount.Name = "txtLimitMinCount";
            this.txtLimitMinCount.ReadOnly = true;
            this.txtLimitMinCount.Size = new System.Drawing.Size(179, 26);
            this.txtLimitMinCount.TabIndex = 263;
            // 
            // txtMinCatchCount
            // 
            this.txtMinCatchCount.Location = new System.Drawing.Point(187, 250);
            this.txtMinCatchCount.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtMinCatchCount.Name = "txtMinCatchCount";
            this.txtMinCatchCount.ReadOnly = true;
            this.txtMinCatchCount.Size = new System.Drawing.Size(179, 26);
            this.txtMinCatchCount.TabIndex = 262;
            // 
            // labelX24
            // 
            this.labelX24.BackColor = System.Drawing.Color.PaleTurquoise;
            // 
            // 
            // 
            this.labelX24.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10001;
            this.labelX24.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX24.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.labelX24.Location = new System.Drawing.Point(5, 252);
            this.labelX24.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.labelX24.Name = "labelX24";
            this.labelX24.Size = new System.Drawing.Size(173, 28);
            this.labelX24.TabIndex = 261;
            this.labelX24.Text = "Min Catch Counts";
            // 
            // labelX23
            // 
            this.labelX23.BackColor = System.Drawing.Color.PaleTurquoise;
            // 
            // 
            // 
            this.labelX23.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10001;
            this.labelX23.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX23.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.labelX23.Location = new System.Drawing.Point(4, 321);
            this.labelX23.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.labelX23.Name = "labelX23";
            this.labelX23.Size = new System.Drawing.Size(173, 28);
            this.labelX23.TabIndex = 260;
            this.labelX23.Text = "Limit Max Count";
            // 
            // txtCieObserver
            // 
            this.txtCieObserver.Location = new System.Drawing.Point(187, 179);
            this.txtCieObserver.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtCieObserver.Name = "txtCieObserver";
            this.txtCieObserver.ReadOnly = true;
            this.txtCieObserver.Size = new System.Drawing.Size(179, 26);
            this.txtCieObserver.TabIndex = 259;
            // 
            // txtCieIlluminant
            // 
            this.txtCieIlluminant.Location = new System.Drawing.Point(187, 216);
            this.txtCieIlluminant.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtCieIlluminant.Name = "txtCieIlluminant";
            this.txtCieIlluminant.ReadOnly = true;
            this.txtCieIlluminant.Size = new System.Drawing.Size(179, 26);
            this.txtCieIlluminant.TabIndex = 258;
            // 
            // txtScanAverage
            // 
            this.txtScanAverage.Location = new System.Drawing.Point(187, 147);
            this.txtScanAverage.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtScanAverage.Name = "txtScanAverage";
            this.txtScanAverage.ReadOnly = true;
            this.txtScanAverage.Size = new System.Drawing.Size(179, 26);
            this.txtScanAverage.TabIndex = 257;
            // 
            // txtBoxCar
            // 
            this.txtBoxCar.Location = new System.Drawing.Point(187, 111);
            this.txtBoxCar.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtBoxCar.Name = "txtBoxCar";
            this.txtBoxCar.ReadOnly = true;
            this.txtBoxCar.Size = new System.Drawing.Size(179, 26);
            this.txtBoxCar.TabIndex = 256;
            // 
            // txtEndWave
            // 
            this.txtEndWave.Location = new System.Drawing.Point(187, 76);
            this.txtEndWave.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtEndWave.Name = "txtEndWave";
            this.txtEndWave.ReadOnly = true;
            this.txtEndWave.Size = new System.Drawing.Size(179, 26);
            this.txtEndWave.TabIndex = 255;
            // 
            // txtStartWave
            // 
            this.txtStartWave.Location = new System.Drawing.Point(187, 42);
            this.txtStartWave.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtStartWave.Name = "txtStartWave";
            this.txtStartWave.ReadOnly = true;
            this.txtStartWave.Size = new System.Drawing.Size(179, 26);
            this.txtStartWave.TabIndex = 254;
            // 
            // txtSptOperationMode
            // 
            this.txtSptOperationMode.Location = new System.Drawing.Point(187, 2);
            this.txtSptOperationMode.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtSptOperationMode.Name = "txtSptOperationMode";
            this.txtSptOperationMode.ReadOnly = true;
            this.txtSptOperationMode.Size = new System.Drawing.Size(179, 26);
            this.txtSptOperationMode.TabIndex = 253;
            // 
            // labelX19
            // 
            this.labelX19.BackColor = System.Drawing.Color.PaleTurquoise;
            // 
            // 
            // 
            this.labelX19.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10001;
            this.labelX19.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX19.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.labelX19.Location = new System.Drawing.Point(795, 78);
            this.labelX19.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.labelX19.Name = "labelX19";
            this.labelX19.Size = new System.Drawing.Size(173, 28);
            this.labelX19.TabIndex = 246;
            this.labelX19.Text = "Integrating Time";
            // 
            // dinWattCoeff
            // 
            // 
            // 
            // 
            this.dinWattCoeff.BackgroundStyle.Class = "DateTimeInputBackground";
            this.dinWattCoeff.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.dinWattCoeff.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.dinWattCoeff.DisplayFormat = "0.0000000";
            this.dinWattCoeff.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold);
            this.dinWattCoeff.Increment = 1D;
            this.dinWattCoeff.Location = new System.Drawing.Point(976, 5);
            this.dinWattCoeff.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.dinWattCoeff.Name = "dinWattCoeff";
            this.dinWattCoeff.ShowUpDown = true;
            this.dinWattCoeff.Size = new System.Drawing.Size(143, 26);
            this.dinWattCoeff.TabIndex = 250;
            // 
            // dinLumensCoeff
            // 
            // 
            // 
            // 
            this.dinLumensCoeff.BackgroundStyle.Class = "DateTimeInputBackground";
            this.dinLumensCoeff.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.dinLumensCoeff.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.dinLumensCoeff.DisplayFormat = "0.0000000";
            this.dinLumensCoeff.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold);
            this.dinLumensCoeff.Increment = 1D;
            this.dinLumensCoeff.Location = new System.Drawing.Point(976, 42);
            this.dinLumensCoeff.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.dinLumensCoeff.Name = "dinLumensCoeff";
            this.dinLumensCoeff.ShowUpDown = true;
            this.dinLumensCoeff.Size = new System.Drawing.Size(143, 26);
            this.dinLumensCoeff.TabIndex = 249;
            // 
            // labelX14
            // 
            this.labelX14.BackColor = System.Drawing.Color.PaleTurquoise;
            // 
            // 
            // 
            this.labelX14.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10001;
            this.labelX14.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX14.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelX14.Location = new System.Drawing.Point(5, 76);
            this.labelX14.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.labelX14.Name = "labelX14";
            this.labelX14.Size = new System.Drawing.Size(173, 28);
            this.labelX14.TabIndex = 252;
            this.labelX14.Text = "End Wavelength";
            // 
            // labelX13
            // 
            this.labelX13.BackColor = System.Drawing.Color.PaleTurquoise;
            // 
            // 
            // 
            this.labelX13.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10001;
            this.labelX13.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX13.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelX13.Location = new System.Drawing.Point(795, 5);
            this.labelX13.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.labelX13.Name = "labelX13";
            this.labelX13.Size = new System.Drawing.Size(173, 28);
            this.labelX13.TabIndex = 248;
            this.labelX13.Text = "Watt Coeff";
            // 
            // labelX16
            // 
            this.labelX16.BackColor = System.Drawing.Color.PaleTurquoise;
            // 
            // 
            // 
            this.labelX16.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10001;
            this.labelX16.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX16.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelX16.Location = new System.Drawing.Point(5, 42);
            this.labelX16.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.labelX16.Name = "labelX16";
            this.labelX16.Size = new System.Drawing.Size(173, 28);
            this.labelX16.TabIndex = 251;
            this.labelX16.Text = "Start Wavelength";
            // 
            // labelX11
            // 
            this.labelX11.BackColor = System.Drawing.Color.PaleTurquoise;
            // 
            // 
            // 
            this.labelX11.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10001;
            this.labelX11.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX11.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelX11.Location = new System.Drawing.Point(795, 39);
            this.labelX11.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.labelX11.Name = "labelX11";
            this.labelX11.Size = new System.Drawing.Size(173, 28);
            this.labelX11.TabIndex = 247;
            this.labelX11.Text = "LuminCoeff";
            // 
            // chkCorrectForNonlineatity
            // 
            this.chkCorrectForNonlineatity.AutoSize = true;
            this.chkCorrectForNonlineatity.Checked = true;
            this.chkCorrectForNonlineatity.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkCorrectForNonlineatity.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkCorrectForNonlineatity.ForeColor = System.Drawing.Color.DarkBlue;
            this.chkCorrectForNonlineatity.Location = new System.Drawing.Point(389, 283);
            this.chkCorrectForNonlineatity.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.chkCorrectForNonlineatity.Name = "chkCorrectForNonlineatity";
            this.chkCorrectForNonlineatity.Size = new System.Drawing.Size(234, 24);
            this.chkCorrectForNonlineatity.TabIndex = 246;
            this.chkCorrectForNonlineatity.Text = "Correct For Nonlinearity";
            this.chkCorrectForNonlineatity.UseVisualStyleBackColor = true;
            // 
            // labelX1
            // 
            this.labelX1.BackColor = System.Drawing.Color.PaleTurquoise;
            // 
            // 
            // 
            this.labelX1.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10001;
            this.labelX1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.labelX1.Location = new System.Drawing.Point(5, 181);
            this.labelX1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(173, 28);
            this.labelX1.TabIndex = 242;
            this.labelX1.Text = "CIE Oberver";
            // 
            // labelX4
            // 
            this.labelX4.BackColor = System.Drawing.Color.PaleTurquoise;
            // 
            // 
            // 
            this.labelX4.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10001;
            this.labelX4.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.labelX4.Location = new System.Drawing.Point(5, 216);
            this.labelX4.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.labelX4.Name = "labelX4";
            this.labelX4.Size = new System.Drawing.Size(173, 28);
            this.labelX4.TabIndex = 243;
            this.labelX4.Text = "CIE illuminant";
            // 
            // zedCalibration
            // 
            this.zedCalibration.BackColor = System.Drawing.Color.WhiteSmoke;
            this.zedCalibration.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.zedCalibration.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.zedCalibration.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.zedCalibration.IsSynchronizeXAxes = true;
            this.zedCalibration.Location = new System.Drawing.Point(677, 437);
            this.zedCalibration.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.zedCalibration.Name = "zedCalibration";
            this.zedCalibration.ScrollGrace = 0D;
            this.zedCalibration.ScrollMaxX = 0D;
            this.zedCalibration.ScrollMaxY = 0D;
            this.zedCalibration.ScrollMaxY2 = 0D;
            this.zedCalibration.ScrollMinX = 0D;
            this.zedCalibration.ScrollMinY = 0D;
            this.zedCalibration.ScrollMinY2 = 0D;
            this.zedCalibration.Size = new System.Drawing.Size(633, 398);
            this.zedCalibration.TabIndex = 246;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // labelX18
            // 
            this.labelX18.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX18.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10001;
            this.labelX18.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX18.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelX18.ForeColor = System.Drawing.Color.Red;
            this.labelX18.Location = new System.Drawing.Point(677, 406);
            this.labelX18.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.labelX18.Name = "labelX18";
            this.labelX18.Size = new System.Drawing.Size(144, 23);
            this.labelX18.TabIndex = 249;
            this.labelX18.Text = "Cali Spetrum";
            // 
            // zedDarkArray
            // 
            this.zedDarkArray.BackColor = System.Drawing.Color.WhiteSmoke;
            this.zedDarkArray.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.zedDarkArray.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.zedDarkArray.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.zedDarkArray.IsSynchronizeXAxes = true;
            this.zedDarkArray.Location = new System.Drawing.Point(19, 512);
            this.zedDarkArray.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.zedDarkArray.Name = "zedDarkArray";
            this.zedDarkArray.ScrollGrace = 0D;
            this.zedDarkArray.ScrollMaxX = 0D;
            this.zedDarkArray.ScrollMaxY = 0D;
            this.zedDarkArray.ScrollMaxY2 = 0D;
            this.zedDarkArray.ScrollMinX = 0D;
            this.zedDarkArray.ScrollMinY = 0D;
            this.zedDarkArray.ScrollMinY2 = 0D;
            this.zedDarkArray.Size = new System.Drawing.Size(633, 323);
            this.zedDarkArray.TabIndex = 250;
            // 
            // chkEnableAutoGetdark
            // 
            this.chkEnableAutoGetdark.AutoSize = true;
            this.chkEnableAutoGetdark.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkEnableAutoGetdark.ForeColor = System.Drawing.Color.Lavender;
            this.chkEnableAutoGetdark.Location = new System.Drawing.Point(19, 406);
            this.chkEnableAutoGetdark.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.chkEnableAutoGetdark.Name = "chkEnableAutoGetdark";
            this.chkEnableAutoGetdark.Size = new System.Drawing.Size(166, 22);
            this.chkEnableAutoGetdark.TabIndex = 252;
            this.chkEnableAutoGetdark.Text = "AutoGetDarkArray";
            this.chkEnableAutoGetdark.UseVisualStyleBackColor = true;
            // 
            // btnGetCurrentDark
            // 
            this.btnGetCurrentDark.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnGetCurrentDark.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnGetCurrentDark.Location = new System.Drawing.Point(53, 437);
            this.btnGetCurrentDark.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnGetCurrentDark.Name = "btnGetCurrentDark";
            this.btnGetCurrentDark.Size = new System.Drawing.Size(165, 39);
            this.btnGetCurrentDark.Style = DevComponents.DotNetBar.eDotNetBarStyle.Windows7;
            this.btnGetCurrentDark.TabIndex = 253;
            this.btnGetCurrentDark.Text = "Get Current Dark";
            this.btnGetCurrentDark.Click += new System.EventHandler(this.btnGetCurrentDark_Click);
            // 
            // lblDarkAvg
            // 
            this.lblDarkAvg.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.lblDarkAvg.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10001;
            this.lblDarkAvg.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblDarkAvg.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDarkAvg.ForeColor = System.Drawing.Color.RoyalBlue;
            this.lblDarkAvg.Location = new System.Drawing.Point(224, 486);
            this.lblDarkAvg.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.lblDarkAvg.Name = "lblDarkAvg";
            this.lblDarkAvg.Size = new System.Drawing.Size(153, 23);
            this.lblDarkAvg.TabIndex = 254;
            this.lblDarkAvg.Text = "0";
            // 
            // labelX20
            // 
            this.labelX20.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX20.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10001;
            this.labelX20.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX20.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelX20.ForeColor = System.Drawing.Color.Blue;
            this.labelX20.Location = new System.Drawing.Point(40, 485);
            this.labelX20.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.labelX20.Name = "labelX20";
            this.labelX20.Size = new System.Drawing.Size(176, 23);
            this.labelX20.TabIndex = 255;
            this.labelX20.Text = "System Dark avg =";
            // 
            // lblCurrentDark
            // 
            this.lblCurrentDark.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.lblCurrentDark.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10001;
            this.lblCurrentDark.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblCurrentDark.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCurrentDark.ForeColor = System.Drawing.Color.Red;
            this.lblCurrentDark.Location = new System.Drawing.Point(517, 485);
            this.lblCurrentDark.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.lblCurrentDark.Name = "lblCurrentDark";
            this.lblCurrentDark.Size = new System.Drawing.Size(117, 23);
            this.lblCurrentDark.TabIndex = 256;
            this.lblCurrentDark.Text = "0";
            // 
            // labelX21
            // 
            this.labelX21.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX21.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10001;
            this.labelX21.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX21.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelX21.ForeColor = System.Drawing.Color.Red;
            this.labelX21.Location = new System.Drawing.Point(328, 485);
            this.labelX21.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.labelX21.Name = "labelX21";
            this.labelX21.Size = new System.Drawing.Size(181, 23);
            this.labelX21.TabIndex = 257;
            this.labelX21.Text = "Current Dark avg =";
            // 
            // btnSaveDarkArrayToSystem
            // 
            this.btnSaveDarkArrayToSystem.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnSaveDarkArrayToSystem.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnSaveDarkArrayToSystem.Location = new System.Drawing.Point(461, 437);
            this.btnSaveDarkArrayToSystem.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnSaveDarkArrayToSystem.Name = "btnSaveDarkArrayToSystem";
            this.btnSaveDarkArrayToSystem.Size = new System.Drawing.Size(165, 39);
            this.btnSaveDarkArrayToSystem.Style = DevComponents.DotNetBar.eDotNetBarStyle.Windows7;
            this.btnSaveDarkArrayToSystem.TabIndex = 251;
            this.btnSaveDarkArrayToSystem.Text = "Setting  To System";
            this.btnSaveDarkArrayToSystem.Click += new System.EventHandler(this.btnSaveDarkArrayToSystem_Click);
            // 
            // btnLoadDarkArray
            // 
            this.btnLoadDarkArray.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnLoadDarkArray.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnLoadDarkArray.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLoadDarkArray.ImageFixedSize = new System.Drawing.Size(20, 20);
            this.btnLoadDarkArray.Location = new System.Drawing.Point(251, 437);
            this.btnLoadDarkArray.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnLoadDarkArray.Name = "btnLoadDarkArray";
            this.btnLoadDarkArray.Size = new System.Drawing.Size(173, 40);
            this.btnLoadDarkArray.Style = DevComponents.DotNetBar.eDotNetBarStyle.Windows7;
            this.btnLoadDarkArray.TabIndex = 248;
            this.btnLoadDarkArray.Text = "Load Dark File";
            this.btnLoadDarkArray.Click += new System.EventHandler(this.btnLoadDarkArray_Click);
            // 
            // btnSaveCaliSpectrumToSystem
            // 
            this.btnSaveCaliSpectrumToSystem.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnSaveCaliSpectrumToSystem.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnSaveCaliSpectrumToSystem.Location = new System.Drawing.Point(1120, 437);
            this.btnSaveCaliSpectrumToSystem.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnSaveCaliSpectrumToSystem.Name = "btnSaveCaliSpectrumToSystem";
            this.btnSaveCaliSpectrumToSystem.Size = new System.Drawing.Size(165, 39);
            this.btnSaveCaliSpectrumToSystem.Style = DevComponents.DotNetBar.eDotNetBarStyle.Windows7;
            this.btnSaveCaliSpectrumToSystem.TabIndex = 248;
            this.btnSaveCaliSpectrumToSystem.Text = "Setting  To System";
            this.btnSaveCaliSpectrumToSystem.Click += new System.EventHandler(this.btnSaveCaliSpectrumToSystem_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnCancel.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnCancel.Image = ((System.Drawing.Image)(resources.GetObject("btnCancel.Image")));
            this.btnCancel.Location = new System.Drawing.Point(1315, 103);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(189, 61);
            this.btnCancel.Style = DevComponents.DotNetBar.eDotNetBarStyle.Windows7;
            this.btnCancel.TabIndex = 245;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnLoadCaliFile
            // 
            this.btnLoadCaliFile.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnLoadCaliFile.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnLoadCaliFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLoadCaliFile.ImageFixedSize = new System.Drawing.Size(20, 20);
            this.btnLoadCaliFile.Location = new System.Drawing.Point(917, 437);
            this.btnLoadCaliFile.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnLoadCaliFile.Name = "btnLoadCaliFile";
            this.btnLoadCaliFile.Size = new System.Drawing.Size(173, 40);
            this.btnLoadCaliFile.Style = DevComponents.DotNetBar.eDotNetBarStyle.Windows7;
            this.btnLoadCaliFile.TabIndex = 247;
            this.btnLoadCaliFile.Text = "Load Cali. File";
            this.btnLoadCaliFile.Click += new System.EventHandler(this.btnLoadCaliFile_Click);
            // 
            // btnSetParameter
            // 
            this.btnSetParameter.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnSetParameter.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnSetParameter.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnSetParameter.Image = ((System.Drawing.Image)(resources.GetObject("btnSetParameter.Image")));
            this.btnSetParameter.Location = new System.Drawing.Point(1315, 35);
            this.btnSetParameter.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnSetParameter.Name = "btnSetParameter";
            this.btnSetParameter.Size = new System.Drawing.Size(189, 61);
            this.btnSetParameter.Style = DevComponents.DotNetBar.eDotNetBarStyle.Windows7;
            this.btnSetParameter.TabIndex = 242;
            this.btnSetParameter.Text = "Set";
            this.btnSetParameter.Click += new System.EventHandler(this.btnSetParameter_Click);
            // 
            // frmSetParameter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1520, 856);
            this.Controls.Add(this.labelX21);
            this.Controls.Add(this.lblCurrentDark);
            this.Controls.Add(this.labelX20);
            this.Controls.Add(this.lblDarkAvg);
            this.Controls.Add(this.btnGetCurrentDark);
            this.Controls.Add(this.chkEnableAutoGetdark);
            this.Controls.Add(this.btnSaveDarkArrayToSystem);
            this.Controls.Add(this.btnLoadDarkArray);
            this.Controls.Add(this.zedDarkArray);
            this.Controls.Add(this.labelX18);
            this.Controls.Add(this.zedCalibration);
            this.Controls.Add(this.btnSaveCaliSpectrumToSystem);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnLoadCaliFile);
            this.Controls.Add(this.btnSetParameter);
            this.Controls.Add(this.groupPanel5);
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "frmSetParameter";
            this.Text = "frmSetParameter";
            this.Load += new System.EventHandler(this.frmSetParameter_Load);
            this.groupPanel5.ResumeLayout(false);
            this.groupPanel5.PerformLayout();
            this.groupPanel7.ResumeLayout(false);
            this.groupPanel7.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dinCollectArea)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dinFiberDiameter)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dinWattCoeff)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dinLumensCoeff)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

		private DevComponents.DotNetBar.LabelX labelX2;
		private DevComponents.DotNetBar.LabelX labelX3;
		private DevComponents.DotNetBar.LabelX labelX7;
		private DevComponents.DotNetBar.LabelX labelX15;
		private System.Windows.Forms.CheckBox checkBox3;
        private DevComponents.DotNetBar.ButtonX btnSetParameter;
        private DevComponents.DotNetBar.Controls.GroupPanel groupPanel5;
        private DevComponents.DotNetBar.LabelX labelX1;
		private DevComponents.DotNetBar.LabelX labelX4;
		 private System.Windows.Forms.CheckBox chkCorrectForNonlineatity;
		 private DevComponents.DotNetBar.ButtonX btnCancel;
         private DevComponents.Editors.DoubleInput dinWattCoeff;
         private DevComponents.Editors.DoubleInput dinLumensCoeff;
         private DevComponents.DotNetBar.LabelX labelX13;
		 private DevComponents.DotNetBar.LabelX labelX11;
         private DevComponents.DotNetBar.LabelX labelX14;
         private DevComponents.DotNetBar.LabelX labelX16;
		 private ZedGraph.ZedGraphControl zedCalibration;
		 private DevComponents.DotNetBar.ButtonX btnLoadCaliFile;
		 private System.Windows.Forms.OpenFileDialog openFileDialog1;
		 private DevComponents.DotNetBar.ButtonX btnSaveCaliSpectrumToSystem;
		 private DevComponents.DotNetBar.LabelX labelX18;
         private DevComponents.DotNetBar.LabelX labelX19;
         private DevComponents.DotNetBar.ButtonX btnLoadDarkArray;
         private ZedGraph.ZedGraphControl zedDarkArray;
         private DevComponents.DotNetBar.ButtonX btnSaveDarkArrayToSystem;
         private System.Windows.Forms.CheckBox chkEnableAutoGetdark;
         private DevComponents.DotNetBar.ButtonX btnGetCurrentDark;
         private DevComponents.DotNetBar.LabelX lblDarkAvg;
         private DevComponents.DotNetBar.LabelX labelX20;
         private DevComponents.DotNetBar.LabelX lblCurrentDark;
		 private DevComponents.DotNetBar.LabelX labelX21;
		 private System.Windows.Forms.TextBox txtEndWave;
		 private System.Windows.Forms.TextBox txtStartWave;
		 private System.Windows.Forms.TextBox txtSptOperationMode;
		 private System.Windows.Forms.TextBox txtLimitStartTime;
		 private System.Windows.Forms.TextBox txtLimitTargetCount;
		 private System.Windows.Forms.TextBox txtAutoMaxCount;
		 private System.Windows.Forms.TextBox txtAutoMinCount;
		 private System.Windows.Forms.CheckBox chkSptMeterCalc;
		 private System.Windows.Forms.CheckBox chkSptMeterTrigger;
		 private System.Windows.Forms.CheckBox chkGetRowData;
		 private DevComponents.DotNetBar.LabelX labelX28;
		 private DevComponents.DotNetBar.LabelX labelX27;
		 private DevComponents.DotNetBar.LabelX labelX26;
		 private DevComponents.DotNetBar.LabelX labelX25;
		 private System.Windows.Forms.TextBox txtLimiMaxCount;
		 private System.Windows.Forms.TextBox txtLimitMinCount;
		 private System.Windows.Forms.TextBox txtMinCatchCount;
		 private DevComponents.DotNetBar.LabelX labelX24;
		 private DevComponents.DotNetBar.LabelX labelX23;
		 private System.Windows.Forms.TextBox txtCieObserver;
		 private System.Windows.Forms.TextBox txtCieIlluminant;
		 private System.Windows.Forms.TextBox txtScanAverage;
		 private System.Windows.Forms.TextBox txtBoxCar;
		 private System.Windows.Forms.CheckBox chkHWTrigger;
		 private DevComponents.DotNetBar.Controls.GroupPanel groupPanel7;
		 private DevComponents.DotNetBar.LabelX labelX6;
		 private DevComponents.DotNetBar.LabelX labelX5;
		 private DevComponents.Editors.DoubleInput dinCollectArea;
		 private DevComponents.Editors.DoubleInput dinFiberDiameter;
		 private System.Windows.Forms.RadioButton radCollect;
		 private System.Windows.Forms.RadioButton radFiber;
		 private System.Windows.Forms.RadioButton radSphere;
    }
}
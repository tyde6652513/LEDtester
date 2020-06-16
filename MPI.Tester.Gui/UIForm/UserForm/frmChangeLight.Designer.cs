namespace MPI.Tester.Gui
{
    partial class frmChangeLight
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
			this.lblOutputFileName = new DevComponents.DotNetBar.LabelX();
			this.lblLotNumber = new DevComponents.DotNetBar.LabelX();
			this.lblWaferNumber = new DevComponents.DotNetBar.LabelX();
			this.lblOperatorName = new DevComponents.DotNetBar.LabelX();
			this.txtOperatorName = new DevComponents.DotNetBar.Controls.TextBoxX();
			this.txtWaferNumber = new DevComponents.DotNetBar.Controls.TextBoxX();
			this.txtLotNumber = new DevComponents.DotNetBar.Controls.TextBoxX();
			this.txtSpec = new DevComponents.DotNetBar.Controls.TextBoxX();
			this.labelX1 = new DevComponents.DotNetBar.LabelX();
			this.btnEndAndSave = new DevComponents.DotNetBar.ButtonX();
			this.btnCycleRun = new DevComponents.DotNetBar.ButtonX();
			this.btnStartAndOpen = new DevComponents.DotNetBar.ButtonX();
			this.labelX2 = new DevComponents.DotNetBar.LabelX();
			this.labelX3 = new DevComponents.DotNetBar.LabelX();
			this.labelX4 = new DevComponents.DotNetBar.LabelX();
			this.labelX5 = new DevComponents.DotNetBar.LabelX();
			this.numLotStart = new DevComponents.Editors.IntegerInput();
			this.numLotEnd = new DevComponents.Editors.IntegerInput();
			this.numSpecEnd = new DevComponents.Editors.IntegerInput();
			this.numSpecStart = new DevComponents.Editors.IntegerInput();
			this.txtOutputFileName = new DevComponents.DotNetBar.Controls.TextBoxX();
			this.rdoCOT = new System.Windows.Forms.RadioButton();
			this.rdoCOW = new System.Windows.Forms.RadioButton();
			((System.ComponentModel.ISupportInitialize)(this.numLotStart)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numLotEnd)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numSpecEnd)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numSpecStart)).BeginInit();
			this.SuspendLayout();
			// 
			// lblOutputFileName
			// 
			this.lblOutputFileName.BackColor = System.Drawing.Color.PowderBlue;
			// 
			// 
			// 
			this.lblOutputFileName.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
			this.lblOutputFileName.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
			this.lblOutputFileName.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblOutputFileName.ForeColor = System.Drawing.Color.Black;
			this.lblOutputFileName.Location = new System.Drawing.Point(18, 4);
			this.lblOutputFileName.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.lblOutputFileName.Name = "lblOutputFileName";
			this.lblOutputFileName.Size = new System.Drawing.Size(141, 22);
			this.lblOutputFileName.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
			this.lblOutputFileName.TabIndex = 98;
			this.lblOutputFileName.Text = "File Name";
			this.lblOutputFileName.TextAlignment = System.Drawing.StringAlignment.Center;
			// 
			// lblLotNumber
			// 
			this.lblLotNumber.BackColor = System.Drawing.Color.PowderBlue;
			// 
			// 
			// 
			this.lblLotNumber.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
			this.lblLotNumber.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
			this.lblLotNumber.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblLotNumber.ForeColor = System.Drawing.Color.Black;
			this.lblLotNumber.Location = new System.Drawing.Point(18, 56);
			this.lblLotNumber.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.lblLotNumber.Name = "lblLotNumber";
			this.lblLotNumber.Size = new System.Drawing.Size(141, 22);
			this.lblLotNumber.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
			this.lblLotNumber.TabIndex = 92;
			this.lblLotNumber.Text = "Lot No";
			this.lblLotNumber.TextAlignment = System.Drawing.StringAlignment.Center;
			// 
			// lblWaferNumber
			// 
			this.lblWaferNumber.BackColor = System.Drawing.Color.PowderBlue;
			// 
			// 
			// 
			this.lblWaferNumber.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
			this.lblWaferNumber.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
			this.lblWaferNumber.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblWaferNumber.ForeColor = System.Drawing.Color.Black;
			this.lblWaferNumber.Location = new System.Drawing.Point(18, 30);
			this.lblWaferNumber.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.lblWaferNumber.Name = "lblWaferNumber";
			this.lblWaferNumber.Size = new System.Drawing.Size(141, 22);
			this.lblWaferNumber.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
			this.lblWaferNumber.TabIndex = 91;
			this.lblWaferNumber.Text = "Wafer ID";
			this.lblWaferNumber.TextAlignment = System.Drawing.StringAlignment.Center;
			// 
			// lblOperatorName
			// 
			this.lblOperatorName.BackColor = System.Drawing.Color.PowderBlue;
			// 
			// 
			// 
			this.lblOperatorName.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
			this.lblOperatorName.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
			this.lblOperatorName.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblOperatorName.ForeColor = System.Drawing.Color.Black;
			this.lblOperatorName.Location = new System.Drawing.Point(18, 160);
			this.lblOperatorName.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.lblOperatorName.Name = "lblOperatorName";
			this.lblOperatorName.Size = new System.Drawing.Size(141, 22);
			this.lblOperatorName.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
			this.lblOperatorName.TabIndex = 88;
			this.lblOperatorName.Text = "Operator";
			this.lblOperatorName.TextAlignment = System.Drawing.StringAlignment.Center;
			// 
			// txtOperatorName
			// 
			this.txtOperatorName.BackColor = System.Drawing.Color.White;
			// 
			// 
			// 
			this.txtOperatorName.Border.Class = "TextBoxBorder";
			this.txtOperatorName.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
			this.txtOperatorName.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtOperatorName.Location = new System.Drawing.Point(166, 160);
			this.txtOperatorName.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.txtOperatorName.Name = "txtOperatorName";
			this.txtOperatorName.Size = new System.Drawing.Size(350, 22);
			this.txtOperatorName.TabIndex = 131;
			this.txtOperatorName.TextChanged += new System.EventHandler(this.UI_DataChanged);
			// 
			// txtWaferNumber
			// 
			this.txtWaferNumber.BackColor = System.Drawing.Color.White;
			// 
			// 
			// 
			this.txtWaferNumber.Border.Class = "TextBoxBorder";
			this.txtWaferNumber.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
			this.txtWaferNumber.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtWaferNumber.Location = new System.Drawing.Point(166, 30);
			this.txtWaferNumber.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.txtWaferNumber.Name = "txtWaferNumber";
			this.txtWaferNumber.Size = new System.Drawing.Size(349, 22);
			this.txtWaferNumber.TabIndex = 130;
			this.txtWaferNumber.TextChanged += new System.EventHandler(this.UI_DataChanged);
			// 
			// txtLotNumber
			// 
			this.txtLotNumber.BackColor = System.Drawing.Color.White;
			// 
			// 
			// 
			this.txtLotNumber.Border.Class = "TextBoxBorder";
			this.txtLotNumber.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
			this.txtLotNumber.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtLotNumber.Location = new System.Drawing.Point(166, 56);
			this.txtLotNumber.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.txtLotNumber.Name = "txtLotNumber";
			this.txtLotNumber.Size = new System.Drawing.Size(349, 22);
			this.txtLotNumber.TabIndex = 129;
			// 
			// txtSpec
			// 
			this.txtSpec.BackColor = System.Drawing.Color.White;
			// 
			// 
			// 
			this.txtSpec.Border.Class = "TextBoxBorder";
			this.txtSpec.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
			this.txtSpec.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtSpec.Location = new System.Drawing.Point(166, 108);
			this.txtSpec.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.txtSpec.Name = "txtSpec";
			this.txtSpec.Size = new System.Drawing.Size(349, 22);
			this.txtSpec.TabIndex = 133;
			// 
			// labelX1
			// 
			this.labelX1.BackColor = System.Drawing.Color.PowderBlue;
			// 
			// 
			// 
			this.labelX1.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
			this.labelX1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
			this.labelX1.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelX1.ForeColor = System.Drawing.Color.Black;
			this.labelX1.Location = new System.Drawing.Point(18, 108);
			this.labelX1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.labelX1.Name = "labelX1";
			this.labelX1.Size = new System.Drawing.Size(142, 22);
			this.labelX1.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
			this.labelX1.TabIndex = 132;
			this.labelX1.Text = "Spec";
			this.labelX1.TextAlignment = System.Drawing.StringAlignment.Center;
			// 
			// btnEndAndSave
			// 
			this.btnEndAndSave.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
			this.btnEndAndSave.AntiAlias = true;
			this.btnEndAndSave.ColorTable = DevComponents.DotNetBar.eButtonColor.Flat;
			this.btnEndAndSave.EnableMarkup = false;
			this.btnEndAndSave.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold);
			this.btnEndAndSave.Image = global::MPI.Tester.Gui.Properties.Resources.btnBlueStop_32;
			this.btnEndAndSave.ImageFixedSize = new System.Drawing.Size(45, 45);
			this.btnEndAndSave.ImagePosition = DevComponents.DotNetBar.eImagePosition.Bottom;
			this.btnEndAndSave.Location = new System.Drawing.Point(395, 197);
			this.btnEndAndSave.Name = "btnEndAndSave";
			this.btnEndAndSave.Shape = new DevComponents.DotNetBar.RoundRectangleShapeDescriptor(2);
			this.btnEndAndSave.ShowSubItems = false;
			this.btnEndAndSave.Size = new System.Drawing.Size(55, 51);
			this.btnEndAndSave.Style = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
			this.btnEndAndSave.TabIndex = 140;
			this.btnEndAndSave.TabStop = false;
			this.btnEndAndSave.Tooltip = "SaveFile";
			this.btnEndAndSave.Visible = false;
			// 
			// btnCycleRun
			// 
			this.btnCycleRun.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
			this.btnCycleRun.AntiAlias = true;
			this.btnCycleRun.ColorTable = DevComponents.DotNetBar.eButtonColor.Flat;
			this.btnCycleRun.EnableMarkup = false;
			this.btnCycleRun.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold);
			this.btnCycleRun.Image = global::MPI.Tester.Gui.Properties.Resources.btnCycleRun_32;
			this.btnCycleRun.ImageFixedSize = new System.Drawing.Size(45, 45);
			this.btnCycleRun.ImagePosition = DevComponents.DotNetBar.eImagePosition.Bottom;
			this.btnCycleRun.Location = new System.Drawing.Point(456, 197);
			this.btnCycleRun.Name = "btnCycleRun";
			this.btnCycleRun.Shape = new DevComponents.DotNetBar.RoundRectangleShapeDescriptor(2);
			this.btnCycleRun.ShowSubItems = false;
			this.btnCycleRun.Size = new System.Drawing.Size(55, 51);
			this.btnCycleRun.Style = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
			this.btnCycleRun.TabIndex = 139;
			this.btnCycleRun.TabStop = false;
			this.btnCycleRun.Tooltip = "RunTest";
			this.btnCycleRun.Visible = false;
			// 
			// btnStartAndOpen
			// 
			this.btnStartAndOpen.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
			this.btnStartAndOpen.AntiAlias = true;
			this.btnStartAndOpen.AutoCheckOnClick = true;
			this.btnStartAndOpen.EnableMarkup = false;
			this.btnStartAndOpen.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold);
			this.btnStartAndOpen.Image = global::MPI.Tester.Gui.Properties.Resources.btnBlueStart_32;
			this.btnStartAndOpen.ImageFixedSize = new System.Drawing.Size(45, 45);
			this.btnStartAndOpen.ImagePosition = DevComponents.DotNetBar.eImagePosition.Bottom;
			this.btnStartAndOpen.Location = new System.Drawing.Point(329, 197);
			this.btnStartAndOpen.Name = "btnStartAndOpen";
			this.btnStartAndOpen.Shape = new DevComponents.DotNetBar.RoundRectangleShapeDescriptor(2);
			this.btnStartAndOpen.ShowSubItems = false;
			this.btnStartAndOpen.Size = new System.Drawing.Size(55, 51);
			this.btnStartAndOpen.Style = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
			this.btnStartAndOpen.TabIndex = 138;
			this.btnStartAndOpen.TabStop = false;
			this.btnStartAndOpen.Tooltip = "OpenFile";
			this.btnStartAndOpen.Visible = false;
			// 
			// labelX2
			// 
			this.labelX2.BackColor = System.Drawing.Color.PowderBlue;
			// 
			// 
			// 
			this.labelX2.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
			this.labelX2.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
			this.labelX2.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelX2.ForeColor = System.Drawing.Color.Black;
			this.labelX2.Location = new System.Drawing.Point(18, 82);
			this.labelX2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.labelX2.Name = "labelX2";
			this.labelX2.Size = new System.Drawing.Size(141, 22);
			this.labelX2.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
			this.labelX2.TabIndex = 141;
			this.labelX2.Text = "Lot No開始位置";
			this.labelX2.TextAlignment = System.Drawing.StringAlignment.Center;
			// 
			// labelX3
			// 
			this.labelX3.BackColor = System.Drawing.Color.PowderBlue;
			// 
			// 
			// 
			this.labelX3.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
			this.labelX3.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
			this.labelX3.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelX3.ForeColor = System.Drawing.Color.Black;
			this.labelX3.Location = new System.Drawing.Point(317, 82);
			this.labelX3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.labelX3.Name = "labelX3";
			this.labelX3.Size = new System.Drawing.Size(141, 22);
			this.labelX3.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
			this.labelX3.TabIndex = 143;
			this.labelX3.Text = "Lot No結束位置";
			this.labelX3.TextAlignment = System.Drawing.StringAlignment.Center;
			// 
			// labelX4
			// 
			this.labelX4.BackColor = System.Drawing.Color.PowderBlue;
			// 
			// 
			// 
			this.labelX4.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
			this.labelX4.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
			this.labelX4.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelX4.ForeColor = System.Drawing.Color.Black;
			this.labelX4.Location = new System.Drawing.Point(317, 134);
			this.labelX4.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.labelX4.Name = "labelX4";
			this.labelX4.Size = new System.Drawing.Size(141, 22);
			this.labelX4.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
			this.labelX4.TabIndex = 147;
			this.labelX4.Text = "Spec結束位置";
			this.labelX4.TextAlignment = System.Drawing.StringAlignment.Center;
			// 
			// labelX5
			// 
			this.labelX5.BackColor = System.Drawing.Color.PowderBlue;
			// 
			// 
			// 
			this.labelX5.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
			this.labelX5.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
			this.labelX5.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelX5.ForeColor = System.Drawing.Color.Black;
			this.labelX5.Location = new System.Drawing.Point(18, 134);
			this.labelX5.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.labelX5.Name = "labelX5";
			this.labelX5.Size = new System.Drawing.Size(141, 22);
			this.labelX5.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
			this.labelX5.TabIndex = 145;
			this.labelX5.Text = "Spec開始位置";
			this.labelX5.TextAlignment = System.Drawing.StringAlignment.Center;
			// 
			// numLotStart
			// 
			this.numLotStart.AntiAlias = true;
			// 
			// 
			// 
			this.numLotStart.BackgroundStyle.BorderColor = System.Drawing.Color.CornflowerBlue;
			this.numLotStart.BackgroundStyle.Class = "DateTimeInputBackground";
			this.numLotStart.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
			this.numLotStart.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold);
			this.numLotStart.Location = new System.Drawing.Point(166, 81);
			this.numLotStart.MaxValue = 30;
			this.numLotStart.MinValue = 1;
			this.numLotStart.Name = "numLotStart";
			this.numLotStart.Size = new System.Drawing.Size(51, 25);
			this.numLotStart.TabIndex = 474;
			this.numLotStart.Value = 1;
			this.numLotStart.ValueChanged += new System.EventHandler(this.UI_DataChanged);
			// 
			// numLotEnd
			// 
			this.numLotEnd.AntiAlias = true;
			// 
			// 
			// 
			this.numLotEnd.BackgroundStyle.BorderColor = System.Drawing.Color.CornflowerBlue;
			this.numLotEnd.BackgroundStyle.Class = "DateTimeInputBackground";
			this.numLotEnd.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
			this.numLotEnd.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold);
			this.numLotEnd.Location = new System.Drawing.Point(465, 81);
			this.numLotEnd.MaxValue = 30;
			this.numLotEnd.MinValue = 1;
			this.numLotEnd.Name = "numLotEnd";
			this.numLotEnd.Size = new System.Drawing.Size(51, 25);
			this.numLotEnd.TabIndex = 475;
			this.numLotEnd.Value = 10;
			this.numLotEnd.ValueChanged += new System.EventHandler(this.UI_DataChanged);
			// 
			// numSpecEnd
			// 
			this.numSpecEnd.AntiAlias = true;
			// 
			// 
			// 
			this.numSpecEnd.BackgroundStyle.BorderColor = System.Drawing.Color.CornflowerBlue;
			this.numSpecEnd.BackgroundStyle.Class = "DateTimeInputBackground";
			this.numSpecEnd.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
			this.numSpecEnd.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold);
			this.numSpecEnd.Location = new System.Drawing.Point(465, 133);
			this.numSpecEnd.MaxValue = 30;
			this.numSpecEnd.MinValue = 1;
			this.numSpecEnd.Name = "numSpecEnd";
			this.numSpecEnd.Size = new System.Drawing.Size(51, 25);
			this.numSpecEnd.TabIndex = 477;
			this.numSpecEnd.Value = 10;
			this.numSpecEnd.ValueChanged += new System.EventHandler(this.UI_DataChanged);
			// 
			// numSpecStart
			// 
			this.numSpecStart.AntiAlias = true;
			// 
			// 
			// 
			this.numSpecStart.BackgroundStyle.BorderColor = System.Drawing.Color.CornflowerBlue;
			this.numSpecStart.BackgroundStyle.Class = "DateTimeInputBackground";
			this.numSpecStart.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
			this.numSpecStart.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold);
			this.numSpecStart.Location = new System.Drawing.Point(166, 133);
			this.numSpecStart.MaxValue = 30;
			this.numSpecStart.MinValue = 1;
			this.numSpecStart.Name = "numSpecStart";
			this.numSpecStart.Size = new System.Drawing.Size(51, 25);
			this.numSpecStart.TabIndex = 476;
			this.numSpecStart.Value = 1;
			this.numSpecStart.ValueChanged += new System.EventHandler(this.UI_DataChanged);
			// 
			// txtOutputFileName
			// 
			this.txtOutputFileName.BackColor = System.Drawing.Color.White;
			// 
			// 
			// 
			this.txtOutputFileName.Border.Class = "TextBoxBorder";
			this.txtOutputFileName.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
			this.txtOutputFileName.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtOutputFileName.Location = new System.Drawing.Point(166, 4);
			this.txtOutputFileName.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.txtOutputFileName.Name = "txtOutputFileName";
			this.txtOutputFileName.Size = new System.Drawing.Size(349, 22);
			this.txtOutputFileName.TabIndex = 478;
			this.txtOutputFileName.TextChanged += new System.EventHandler(this.UI_DataChanged);
			// 
			// rdoCOT
			// 
			this.rdoCOT.AutoSize = true;
			this.rdoCOT.Checked = true;
			this.rdoCOT.Font = new System.Drawing.Font("PMingLiU", 12F);
			this.rdoCOT.Location = new System.Drawing.Point(536, 6);
			this.rdoCOT.Name = "rdoCOT";
			this.rdoCOT.Size = new System.Drawing.Size(56, 20);
			this.rdoCOT.TabIndex = 479;
			this.rdoCOT.TabStop = true;
			this.rdoCOT.Text = "COT";
			this.rdoCOT.UseVisualStyleBackColor = true;
			this.rdoCOT.CheckedChanged += new System.EventHandler(this.UI_DataChanged);
			// 
			// rdoCOW
			// 
			this.rdoCOW.AutoSize = true;
			this.rdoCOW.Font = new System.Drawing.Font("PMingLiU", 12F);
			this.rdoCOW.Location = new System.Drawing.Point(536, 32);
			this.rdoCOW.Name = "rdoCOW";
			this.rdoCOW.Size = new System.Drawing.Size(61, 20);
			this.rdoCOW.TabIndex = 480;
			this.rdoCOW.Text = "COW";
			this.rdoCOW.UseVisualStyleBackColor = true;
			this.rdoCOW.CheckedChanged += new System.EventHandler(this.UI_DataChanged);
			// 
			// frmChangeLight
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.WhiteSmoke;
			this.ClientSize = new System.Drawing.Size(634, 299);
			this.Controls.Add(this.rdoCOW);
			this.Controls.Add(this.rdoCOT);
			this.Controls.Add(this.txtOutputFileName);
			this.Controls.Add(this.numSpecEnd);
			this.Controls.Add(this.numSpecStart);
			this.Controls.Add(this.numLotEnd);
			this.Controls.Add(this.numLotStart);
			this.Controls.Add(this.labelX4);
			this.Controls.Add(this.labelX5);
			this.Controls.Add(this.labelX3);
			this.Controls.Add(this.labelX2);
			this.Controls.Add(this.btnEndAndSave);
			this.Controls.Add(this.btnCycleRun);
			this.Controls.Add(this.btnStartAndOpen);
			this.Controls.Add(this.txtSpec);
			this.Controls.Add(this.labelX1);
			this.Controls.Add(this.txtOperatorName);
			this.Controls.Add(this.txtWaferNumber);
			this.Controls.Add(this.txtLotNumber);
			this.Controls.Add(this.lblOutputFileName);
			this.Controls.Add(this.lblLotNumber);
			this.Controls.Add(this.lblWaferNumber);
			this.Controls.Add(this.lblOperatorName);
			this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.Name = "frmChangeLight";
			this.Text = "frmChangeLight";
			this.Load += new System.EventHandler(this.frmChangeLight_Load);
			((System.ComponentModel.ISupportInitialize)(this.numLotStart)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numLotEnd)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numSpecEnd)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numSpecStart)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

		private DevComponents.DotNetBar.LabelX lblOutputFileName;
        private DevComponents.DotNetBar.LabelX lblLotNumber;
        private DevComponents.DotNetBar.LabelX lblWaferNumber;
        private DevComponents.DotNetBar.LabelX lblOperatorName;
        private DevComponents.DotNetBar.Controls.TextBoxX txtOperatorName;
        private DevComponents.DotNetBar.Controls.TextBoxX txtWaferNumber;
		private DevComponents.DotNetBar.Controls.TextBoxX txtLotNumber;
        private DevComponents.DotNetBar.Controls.TextBoxX txtSpec;
		private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.ButtonX btnEndAndSave;
        private DevComponents.DotNetBar.ButtonX btnCycleRun;
		private DevComponents.DotNetBar.ButtonX btnStartAndOpen;
		private DevComponents.DotNetBar.LabelX labelX2;
		private DevComponents.DotNetBar.LabelX labelX3;
		private DevComponents.DotNetBar.LabelX labelX4;
		private DevComponents.DotNetBar.LabelX labelX5;
		private DevComponents.Editors.IntegerInput numLotStart;
		private DevComponents.Editors.IntegerInput numLotEnd;
		private DevComponents.Editors.IntegerInput numSpecEnd;
		private DevComponents.Editors.IntegerInput numSpecStart;
		private DevComponents.DotNetBar.Controls.TextBoxX txtOutputFileName;
		private System.Windows.Forms.RadioButton rdoCOT;
		private System.Windows.Forms.RadioButton rdoCOW;
    }
}
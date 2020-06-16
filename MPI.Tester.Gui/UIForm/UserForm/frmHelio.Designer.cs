namespace MPI.Tester.Gui
{
	partial class frmHelio
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmHelio));
			this.gplLeft = new DevComponents.DotNetBar.Controls.GroupPanel();
			this.cmbBarcodePrintFormat = new DevComponents.DotNetBar.Controls.ComboBoxEx();
			this.lalBarcodePrintFormat = new DevComponents.DotNetBar.LabelX();
			this.txtBarcode = new DevComponents.DotNetBar.Controls.TextBoxX();
			this.lblLotNumber = new DevComponents.DotNetBar.LabelX();
			this.comboBoxProductTypeID = new DevComponents.DotNetBar.Controls.ComboBoxEx();
			this.txtOutputFileName = new DevComponents.DotNetBar.Controls.TextBoxX();
			this.txtTestResultPath = new DevComponents.DotNetBar.Controls.TextBoxX();
			this.lblOutputFileAlias = new DevComponents.DotNetBar.LabelX();
			this.lblProductType = new DevComponents.DotNetBar.LabelX();
			this.txtProductType = new DevComponents.DotNetBar.Controls.TextBoxX();
			this.txtProductName = new DevComponents.DotNetBar.Controls.TextBoxX();
			this.lblBarcode = new DevComponents.DotNetBar.LabelX();
			this.lblOutputFileTitle = new DevComponents.DotNetBar.LabelX();
			this.txtOperatorName = new DevComponents.DotNetBar.Controls.TextBoxX();
			this.lblProductName = new DevComponents.DotNetBar.LabelX();
			this.lblOperatorName = new DevComponents.DotNetBar.LabelX();
			this.txtLotNumber2 = new DevComponents.DotNetBar.Controls.TextBoxX();
			this.cmbMonth = new DevComponents.DotNetBar.Controls.ComboBoxEx();
			this.cmbYear = new DevComponents.DotNetBar.Controls.ComboBoxEx();
			this.txtLotNumber1 = new DevComponents.DotNetBar.Controls.TextBoxX();
			this.gplLeft.SuspendLayout();
			this.SuspendLayout();
			// 
			// gplLeft
			// 
			this.gplLeft.BackColor = System.Drawing.Color.MistyRose;
			this.gplLeft.CanvasColor = System.Drawing.SystemColors.Control;
			this.gplLeft.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
			this.gplLeft.Controls.Add(this.txtLotNumber2);
			this.gplLeft.Controls.Add(this.cmbMonth);
			this.gplLeft.Controls.Add(this.cmbYear);
			this.gplLeft.Controls.Add(this.txtLotNumber1);
			this.gplLeft.Controls.Add(this.cmbBarcodePrintFormat);
			this.gplLeft.Controls.Add(this.lalBarcodePrintFormat);
			this.gplLeft.Controls.Add(this.txtBarcode);
			this.gplLeft.Controls.Add(this.lblLotNumber);
			this.gplLeft.Controls.Add(this.comboBoxProductTypeID);
			this.gplLeft.Controls.Add(this.txtOutputFileName);
			this.gplLeft.Controls.Add(this.txtTestResultPath);
			this.gplLeft.Controls.Add(this.lblOutputFileAlias);
			this.gplLeft.Controls.Add(this.lblProductType);
			this.gplLeft.Controls.Add(this.txtProductType);
			this.gplLeft.Controls.Add(this.txtProductName);
			this.gplLeft.Controls.Add(this.lblBarcode);
			this.gplLeft.Controls.Add(this.lblOutputFileTitle);
			this.gplLeft.Controls.Add(this.txtOperatorName);
			this.gplLeft.Controls.Add(this.lblProductName);
			this.gplLeft.Controls.Add(this.lblOperatorName);
			resources.ApplyResources(this.gplLeft, "gplLeft");
			this.gplLeft.Name = "gplLeft";
			// 
			// 
			// 
			this.gplLeft.Style.BackColor = System.Drawing.Color.WhiteSmoke;
			this.gplLeft.Style.BackColor2 = System.Drawing.Color.Lavender;
			this.gplLeft.Style.BackColorGradientAngle = 90;
			this.gplLeft.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
			this.gplLeft.Style.BorderBottomWidth = 1;
			this.gplLeft.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
			this.gplLeft.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
			this.gplLeft.Style.BorderLeftWidth = 1;
			this.gplLeft.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
			this.gplLeft.Style.BorderRightWidth = 1;
			this.gplLeft.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
			this.gplLeft.Style.BorderTopWidth = 1;
			this.gplLeft.Style.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
			this.gplLeft.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded;
			this.gplLeft.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center;
			this.gplLeft.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
			this.gplLeft.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near;
			// 
			// 
			// 
			this.gplLeft.StyleMouseDown.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
			this.gplLeft.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square;
			// 
			// 
			// 
			this.gplLeft.StyleMouseOver.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
			this.gplLeft.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square;
			// 
			// cmbBarcodePrintFormat
			// 
			this.cmbBarcodePrintFormat.DisplayMember = "Text";
			this.cmbBarcodePrintFormat.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.cmbBarcodePrintFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbBarcodePrintFormat.FormattingEnabled = true;
			resources.ApplyResources(this.cmbBarcodePrintFormat, "cmbBarcodePrintFormat");
			this.cmbBarcodePrintFormat.Name = "cmbBarcodePrintFormat";
			this.cmbBarcodePrintFormat.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
			// 
			// lalBarcodePrintFormat
			// 
			this.lalBarcodePrintFormat.BackColor = System.Drawing.Color.MistyRose;
			// 
			// 
			// 
			this.lalBarcodePrintFormat.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
			this.lalBarcodePrintFormat.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
			resources.ApplyResources(this.lalBarcodePrintFormat, "lalBarcodePrintFormat");
			this.lalBarcodePrintFormat.ForeColor = System.Drawing.Color.Black;
			this.lalBarcodePrintFormat.Name = "lalBarcodePrintFormat";
			this.lalBarcodePrintFormat.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
			this.lalBarcodePrintFormat.TextAlignment = System.Drawing.StringAlignment.Center;
			// 
			// txtBarcode
			// 
			this.txtBarcode.BackColor = System.Drawing.Color.White;
			// 
			// 
			// 
			this.txtBarcode.Border.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
			this.txtBarcode.Border.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
			this.txtBarcode.Border.BorderBottomWidth = 1;
			this.txtBarcode.Border.BorderColor = System.Drawing.Color.DarkSalmon;
			this.txtBarcode.Border.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
			this.txtBarcode.Border.BorderLeftWidth = 1;
			this.txtBarcode.Border.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
			this.txtBarcode.Border.BorderRightWidth = 1;
			this.txtBarcode.Border.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
			this.txtBarcode.Border.BorderTopWidth = 1;
			this.txtBarcode.Border.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
			this.txtBarcode.Border.CornerDiameter = 4;
			this.txtBarcode.Border.CornerType = DevComponents.DotNetBar.eCornerType.Rounded;
			resources.ApplyResources(this.txtBarcode, "txtBarcode");
			this.txtBarcode.Name = "txtBarcode";
			this.txtBarcode.Tag = "20";
			// 
			// lblLotNumber
			// 
			this.lblLotNumber.BackColor = System.Drawing.Color.MistyRose;
			// 
			// 
			// 
			this.lblLotNumber.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
			this.lblLotNumber.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
			resources.ApplyResources(this.lblLotNumber, "lblLotNumber");
			this.lblLotNumber.ForeColor = System.Drawing.Color.Black;
			this.lblLotNumber.Name = "lblLotNumber";
			this.lblLotNumber.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
			this.lblLotNumber.TextAlignment = System.Drawing.StringAlignment.Center;
			// 
			// comboBoxProductTypeID
			// 
			this.comboBoxProductTypeID.DisplayMember = "Text";
			this.comboBoxProductTypeID.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.comboBoxProductTypeID.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBoxProductTypeID.FormattingEnabled = true;
			resources.ApplyResources(this.comboBoxProductTypeID, "comboBoxProductTypeID");
			this.comboBoxProductTypeID.Name = "comboBoxProductTypeID";
			this.comboBoxProductTypeID.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
			// 
			// txtOutputFileName
			// 
			this.txtOutputFileName.BackColor = System.Drawing.Color.WhiteSmoke;
			// 
			// 
			// 
			this.txtOutputFileName.Border.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
			this.txtOutputFileName.Border.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
			this.txtOutputFileName.Border.BorderBottomWidth = 1;
			this.txtOutputFileName.Border.BorderColor = System.Drawing.Color.LightSteelBlue;
			this.txtOutputFileName.Border.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
			this.txtOutputFileName.Border.BorderLeftWidth = 1;
			this.txtOutputFileName.Border.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
			this.txtOutputFileName.Border.BorderRightWidth = 1;
			this.txtOutputFileName.Border.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
			this.txtOutputFileName.Border.BorderTopWidth = 1;
			this.txtOutputFileName.Border.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
			this.txtOutputFileName.Border.CornerDiameter = 4;
			this.txtOutputFileName.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
			resources.ApplyResources(this.txtOutputFileName, "txtOutputFileName");
			this.txtOutputFileName.ForeColor = System.Drawing.SystemColors.WindowText;
			this.txtOutputFileName.Name = "txtOutputFileName";
			this.txtOutputFileName.ReadOnly = true;
			this.txtOutputFileName.Tag = "8";
			// 
			// txtTestResultPath
			// 
			this.txtTestResultPath.BackColor = System.Drawing.Color.WhiteSmoke;
			// 
			// 
			// 
			this.txtTestResultPath.Border.Class = "TextBoxBorder";
			this.txtTestResultPath.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
			resources.ApplyResources(this.txtTestResultPath, "txtTestResultPath");
			this.txtTestResultPath.Name = "txtTestResultPath";
			this.txtTestResultPath.ReadOnly = true;
			// 
			// lblOutputFileAlias
			// 
			this.lblOutputFileAlias.BackColor = System.Drawing.Color.Transparent;
			// 
			// 
			// 
			this.lblOutputFileAlias.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
			this.lblOutputFileAlias.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
			resources.ApplyResources(this.lblOutputFileAlias, "lblOutputFileAlias");
			this.lblOutputFileAlias.ForeColor = System.Drawing.Color.Black;
			this.lblOutputFileAlias.Name = "lblOutputFileAlias";
			this.lblOutputFileAlias.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
			this.lblOutputFileAlias.TextAlignment = System.Drawing.StringAlignment.Center;
			// 
			// lblProductType
			// 
			this.lblProductType.BackColor = System.Drawing.Color.MistyRose;
			// 
			// 
			// 
			this.lblProductType.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
			this.lblProductType.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
			resources.ApplyResources(this.lblProductType, "lblProductType");
			this.lblProductType.ForeColor = System.Drawing.Color.Black;
			this.lblProductType.Name = "lblProductType";
			this.lblProductType.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
			this.lblProductType.TextAlignment = System.Drawing.StringAlignment.Center;
			// 
			// txtProductType
			// 
			this.txtProductType.BackColor = System.Drawing.SystemColors.Window;
			// 
			// 
			// 
			this.txtProductType.Border.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
			this.txtProductType.Border.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
			this.txtProductType.Border.BorderBottomWidth = 1;
			this.txtProductType.Border.BorderColor = System.Drawing.Color.DarkSalmon;
			this.txtProductType.Border.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
			this.txtProductType.Border.BorderLeftWidth = 1;
			this.txtProductType.Border.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
			this.txtProductType.Border.BorderRightWidth = 1;
			this.txtProductType.Border.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
			this.txtProductType.Border.BorderTopWidth = 1;
			this.txtProductType.Border.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
			this.txtProductType.Border.CornerDiameter = 4;
			this.txtProductType.Border.CornerType = DevComponents.DotNetBar.eCornerType.Rounded;
			resources.ApplyResources(this.txtProductType, "txtProductType");
			this.txtProductType.ForeColor = System.Drawing.SystemColors.WindowText;
			this.txtProductType.Name = "txtProductType";
			this.txtProductType.Tag = "8";
			// 
			// txtProductName
			// 
			this.txtProductName.BackColor = System.Drawing.SystemColors.Window;
			// 
			// 
			// 
			this.txtProductName.Border.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
			this.txtProductName.Border.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
			this.txtProductName.Border.BorderBottomWidth = 1;
			this.txtProductName.Border.BorderColor = System.Drawing.Color.DarkSalmon;
			this.txtProductName.Border.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
			this.txtProductName.Border.BorderLeftWidth = 1;
			this.txtProductName.Border.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
			this.txtProductName.Border.BorderRightWidth = 1;
			this.txtProductName.Border.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
			this.txtProductName.Border.BorderTopWidth = 1;
			this.txtProductName.Border.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
			this.txtProductName.Border.CornerDiameter = 4;
			this.txtProductName.Border.CornerType = DevComponents.DotNetBar.eCornerType.Rounded;
			resources.ApplyResources(this.txtProductName, "txtProductName");
			this.txtProductName.ForeColor = System.Drawing.SystemColors.WindowText;
			this.txtProductName.Name = "txtProductName";
			this.txtProductName.Tag = "8";
			// 
			// lblBarcode
			// 
			this.lblBarcode.BackColor = System.Drawing.Color.MistyRose;
			// 
			// 
			// 
			this.lblBarcode.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
			this.lblBarcode.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
			resources.ApplyResources(this.lblBarcode, "lblBarcode");
			this.lblBarcode.ForeColor = System.Drawing.Color.Black;
			this.lblBarcode.Name = "lblBarcode";
			this.lblBarcode.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
			this.lblBarcode.TextAlignment = System.Drawing.StringAlignment.Center;
			// 
			// lblOutputFileTitle
			// 
			this.lblOutputFileTitle.BackColor = System.Drawing.Color.LightGray;
			// 
			// 
			// 
			this.lblOutputFileTitle.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
			this.lblOutputFileTitle.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
			resources.ApplyResources(this.lblOutputFileTitle, "lblOutputFileTitle");
			this.lblOutputFileTitle.ForeColor = System.Drawing.Color.Black;
			this.lblOutputFileTitle.Name = "lblOutputFileTitle";
			this.lblOutputFileTitle.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
			this.lblOutputFileTitle.TextAlignment = System.Drawing.StringAlignment.Center;
			// 
			// txtOperatorName
			// 
			this.txtOperatorName.BackColor = System.Drawing.Color.White;
			// 
			// 
			// 
			this.txtOperatorName.Border.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
			this.txtOperatorName.Border.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
			this.txtOperatorName.Border.BorderBottomWidth = 1;
			this.txtOperatorName.Border.BorderColor = System.Drawing.Color.DarkSalmon;
			this.txtOperatorName.Border.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
			this.txtOperatorName.Border.BorderLeftWidth = 1;
			this.txtOperatorName.Border.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
			this.txtOperatorName.Border.BorderRightWidth = 1;
			this.txtOperatorName.Border.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
			this.txtOperatorName.Border.BorderTopWidth = 1;
			this.txtOperatorName.Border.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
			this.txtOperatorName.Border.CornerDiameter = 4;
			this.txtOperatorName.Border.CornerType = DevComponents.DotNetBar.eCornerType.Rounded;
			resources.ApplyResources(this.txtOperatorName, "txtOperatorName");
			this.txtOperatorName.Name = "txtOperatorName";
			this.txtOperatorName.Tag = "20";
			// 
			// lblProductName
			// 
			this.lblProductName.BackColor = System.Drawing.Color.MistyRose;
			// 
			// 
			// 
			this.lblProductName.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
			this.lblProductName.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
			resources.ApplyResources(this.lblProductName, "lblProductName");
			this.lblProductName.ForeColor = System.Drawing.Color.Black;
			this.lblProductName.Name = "lblProductName";
			this.lblProductName.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
			this.lblProductName.TextAlignment = System.Drawing.StringAlignment.Center;
			// 
			// lblOperatorName
			// 
			this.lblOperatorName.BackColor = System.Drawing.Color.MistyRose;
			// 
			// 
			// 
			this.lblOperatorName.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
			this.lblOperatorName.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
			resources.ApplyResources(this.lblOperatorName, "lblOperatorName");
			this.lblOperatorName.ForeColor = System.Drawing.Color.Black;
			this.lblOperatorName.Name = "lblOperatorName";
			this.lblOperatorName.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
			this.lblOperatorName.TextAlignment = System.Drawing.StringAlignment.Center;
			// 
			// txtLotNumber2
			// 
			this.txtLotNumber2.BackColor = System.Drawing.SystemColors.Window;
			// 
			// 
			// 
			this.txtLotNumber2.Border.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
			this.txtLotNumber2.Border.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
			this.txtLotNumber2.Border.BorderBottomWidth = 1;
			this.txtLotNumber2.Border.BorderColor = System.Drawing.Color.DarkSalmon;
			this.txtLotNumber2.Border.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
			this.txtLotNumber2.Border.BorderLeftWidth = 1;
			this.txtLotNumber2.Border.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
			this.txtLotNumber2.Border.BorderRightWidth = 1;
			this.txtLotNumber2.Border.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
			this.txtLotNumber2.Border.BorderTopWidth = 1;
			this.txtLotNumber2.Border.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
			this.txtLotNumber2.Border.CornerDiameter = 4;
			this.txtLotNumber2.Border.CornerType = DevComponents.DotNetBar.eCornerType.Rounded;
			resources.ApplyResources(this.txtLotNumber2, "txtLotNumber2");
			this.txtLotNumber2.ForeColor = System.Drawing.SystemColors.WindowText;
			this.txtLotNumber2.Name = "txtLotNumber2";
			this.txtLotNumber2.Tag = "8";
			// 
			// cmbMonth
			// 
			this.cmbMonth.DisplayMember = "Text";
			this.cmbMonth.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.cmbMonth.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbMonth.FormattingEnabled = true;
			resources.ApplyResources(this.cmbMonth, "cmbMonth");
			this.cmbMonth.Name = "cmbMonth";
			this.cmbMonth.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
			// 
			// cmbYear
			// 
			this.cmbYear.DisplayMember = "Text";
			this.cmbYear.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.cmbYear.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbYear.FormattingEnabled = true;
			resources.ApplyResources(this.cmbYear, "cmbYear");
			this.cmbYear.Name = "cmbYear";
			this.cmbYear.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
			// 
			// txtLotNumber1
			// 
			this.txtLotNumber1.BackColor = System.Drawing.SystemColors.Window;
			// 
			// 
			// 
			this.txtLotNumber1.Border.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
			this.txtLotNumber1.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
			resources.ApplyResources(this.txtLotNumber1, "txtLotNumber1");
			this.txtLotNumber1.ForeColor = System.Drawing.SystemColors.WindowText;
			this.txtLotNumber1.Name = "txtLotNumber1";
			this.txtLotNumber1.Tag = "8";
			// 
			// frmHelio
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.BackColor = System.Drawing.SystemColors.Control;
			resources.ApplyResources(this, "$this");
			this.ControlBox = false;
			this.Controls.Add(this.gplLeft);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "frmHelio";
			this.Load += new System.EventHandler(this.frmHelio_Load);
			this.gplLeft.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private DevComponents.DotNetBar.Controls.GroupPanel gplLeft;
		private DevComponents.DotNetBar.Controls.TextBoxX txtOutputFileName;
		private DevComponents.DotNetBar.Controls.TextBoxX txtTestResultPath;
		private DevComponents.DotNetBar.LabelX lblOutputFileAlias;
		private DevComponents.DotNetBar.LabelX lblProductType;
		private DevComponents.DotNetBar.Controls.TextBoxX txtProductType;
		private DevComponents.DotNetBar.Controls.TextBoxX txtProductName;
		private DevComponents.DotNetBar.LabelX lblBarcode;
		private DevComponents.DotNetBar.LabelX lblOutputFileTitle;
		private DevComponents.DotNetBar.Controls.TextBoxX txtOperatorName;
		private DevComponents.DotNetBar.LabelX lblProductName;
		private DevComponents.DotNetBar.LabelX lblOperatorName;
		private DevComponents.DotNetBar.Controls.ComboBoxEx comboBoxProductTypeID;
		private DevComponents.DotNetBar.LabelX lblLotNumber;
		private DevComponents.DotNetBar.Controls.TextBoxX txtBarcode;
		private DevComponents.DotNetBar.LabelX lalBarcodePrintFormat;
		private DevComponents.DotNetBar.Controls.ComboBoxEx cmbBarcodePrintFormat;
		private DevComponents.DotNetBar.Controls.TextBoxX txtLotNumber2;
		private DevComponents.DotNetBar.Controls.ComboBoxEx cmbMonth;
		private DevComponents.DotNetBar.Controls.ComboBoxEx cmbYear;
		private DevComponents.DotNetBar.Controls.TextBoxX txtLotNumber1;

	}
}
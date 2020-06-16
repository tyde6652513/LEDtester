using System;
using System.Collections.Generic;
using System.Text;

namespace MPI.Tester.Gui
{
	public partial class frmUploadRecipe
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
			if ( disposing )
			{
				if ( components != null )
				{
					components.Dispose();
				}
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmUploadRecipe));
            this.btnUpload = new DevComponents.DotNetBar.ButtonX();
            this.btnExit = new DevComponents.DotNetBar.ButtonX();
            this.lblPdPath = new DevComponents.DotNetBar.LabelX();
            this.txtUploadProductPath = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.lblMapPath = new DevComponents.DotNetBar.LabelX();
            this.txtUploadMapPath = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.chkIsEnableUploadProduct = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.chkIsEnableUploadMap = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.btnTestResultSelect01 = new DevComponents.DotNetBar.ButtonX();
            this.btnTestResultSelect02 = new DevComponents.DotNetBar.ButtonX();
            this.SuspendLayout();
            // 
            // btnUpload
            // 
            this.btnUpload.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnUpload.BackColor = System.Drawing.Color.LightSteelBlue;
            resources.ApplyResources(this.btnUpload, "btnUpload");
            this.btnUpload.ForeColor = System.Drawing.Color.Black;
            this.btnUpload.Image = global::MPI.Tester.Gui.Properties.Resources.btnSwitch;
            this.btnUpload.ImageFixedSize = new System.Drawing.Size(25, 25);
            this.btnUpload.Name = "btnUpload";
            this.btnUpload.Style = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
            this.btnUpload.Click += new System.EventHandler(this.btnUpload_Click);
            // 
            // btnExit
            // 
            this.btnExit.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnExit.BackColor = System.Drawing.Color.LightSteelBlue;
            resources.ApplyResources(this.btnExit, "btnExit");
            this.btnExit.ForeColor = System.Drawing.Color.Black;
            this.btnExit.Image = global::MPI.Tester.Gui.Properties.Resources.btnUndo;
            this.btnExit.ImageFixedSize = new System.Drawing.Size(25, 25);
            this.btnExit.Name = "btnExit";
            this.btnExit.Style = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // lblPdPath
            // 
            this.lblPdPath.BackColor = System.Drawing.Color.MistyRose;
            // 
            // 
            // 
            this.lblPdPath.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.lblPdPath.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            resources.ApplyResources(this.lblPdPath, "lblPdPath");
            this.lblPdPath.ForeColor = System.Drawing.Color.Black;
            this.lblPdPath.Name = "lblPdPath";
            this.lblPdPath.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
            // 
            // txtUploadProductPath
            // 
            this.txtUploadProductPath.BackColor = System.Drawing.SystemColors.Window;
            // 
            // 
            // 
            this.txtUploadProductPath.Border.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.txtUploadProductPath.Border.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.txtUploadProductPath.Border.BorderBottomWidth = 1;
            this.txtUploadProductPath.Border.BorderColor = System.Drawing.Color.DarkSalmon;
            this.txtUploadProductPath.Border.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.txtUploadProductPath.Border.BorderLeftWidth = 1;
            this.txtUploadProductPath.Border.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.txtUploadProductPath.Border.BorderRightWidth = 1;
            this.txtUploadProductPath.Border.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.txtUploadProductPath.Border.BorderTopWidth = 1;
            this.txtUploadProductPath.Border.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.txtUploadProductPath.Border.CornerDiameter = 4;
            this.txtUploadProductPath.Border.CornerType = DevComponents.DotNetBar.eCornerType.Rounded;
            resources.ApplyResources(this.txtUploadProductPath, "txtUploadProductPath");
            this.txtUploadProductPath.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtUploadProductPath.Name = "txtUploadProductPath";
            this.txtUploadProductPath.Tag = "8";
            // 
            // lblMapPath
            // 
            this.lblMapPath.BackColor = System.Drawing.Color.MistyRose;
            // 
            // 
            // 
            this.lblMapPath.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.lblMapPath.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            resources.ApplyResources(this.lblMapPath, "lblMapPath");
            this.lblMapPath.ForeColor = System.Drawing.Color.Black;
            this.lblMapPath.Name = "lblMapPath";
            this.lblMapPath.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
            // 
            // txtUploadMapPath
            // 
            this.txtUploadMapPath.BackColor = System.Drawing.SystemColors.Window;
            // 
            // 
            // 
            this.txtUploadMapPath.Border.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.txtUploadMapPath.Border.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.txtUploadMapPath.Border.BorderBottomWidth = 1;
            this.txtUploadMapPath.Border.BorderColor = System.Drawing.Color.DarkSalmon;
            this.txtUploadMapPath.Border.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.txtUploadMapPath.Border.BorderLeftWidth = 1;
            this.txtUploadMapPath.Border.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.txtUploadMapPath.Border.BorderRightWidth = 1;
            this.txtUploadMapPath.Border.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.txtUploadMapPath.Border.BorderTopWidth = 1;
            this.txtUploadMapPath.Border.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.txtUploadMapPath.Border.CornerDiameter = 4;
            this.txtUploadMapPath.Border.CornerType = DevComponents.DotNetBar.eCornerType.Rounded;
            resources.ApplyResources(this.txtUploadMapPath, "txtUploadMapPath");
            this.txtUploadMapPath.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtUploadMapPath.Name = "txtUploadMapPath";
            this.txtUploadMapPath.Tag = "8";
            // 
            // chkIsEnableUploadProduct
            // 
            // 
            // 
            // 
            this.chkIsEnableUploadProduct.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.chkIsEnableUploadProduct.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.chkIsEnableUploadProduct.Checked = true;
            this.chkIsEnableUploadProduct.CheckSignSize = new System.Drawing.Size(20, 20);
            this.chkIsEnableUploadProduct.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkIsEnableUploadProduct.CheckValue = "Y";
            resources.ApplyResources(this.chkIsEnableUploadProduct, "chkIsEnableUploadProduct");
            this.chkIsEnableUploadProduct.Name = "chkIsEnableUploadProduct";
            this.chkIsEnableUploadProduct.Style = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
            this.chkIsEnableUploadProduct.TextVisible = false;
            // 
            // chkIsEnableUploadMap
            // 
            // 
            // 
            // 
            this.chkIsEnableUploadMap.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.chkIsEnableUploadMap.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.chkIsEnableUploadMap.Checked = true;
            this.chkIsEnableUploadMap.CheckSignSize = new System.Drawing.Size(20, 20);
            this.chkIsEnableUploadMap.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkIsEnableUploadMap.CheckValue = "Y";
            resources.ApplyResources(this.chkIsEnableUploadMap, "chkIsEnableUploadMap");
            this.chkIsEnableUploadMap.Name = "chkIsEnableUploadMap";
            this.chkIsEnableUploadMap.Style = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
            this.chkIsEnableUploadMap.TextVisible = false;
            // 
            // btnTestResultSelect01
            // 
            this.btnTestResultSelect01.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnTestResultSelect01.AntiAlias = true;
            this.btnTestResultSelect01.BackColor = System.Drawing.Color.Transparent;
            this.btnTestResultSelect01.ColorTable = DevComponents.DotNetBar.eButtonColor.Office2007WithBackground;
            resources.ApplyResources(this.btnTestResultSelect01, "btnTestResultSelect01");
            this.btnTestResultSelect01.Name = "btnTestResultSelect01";
            this.btnTestResultSelect01.Shape = new DevComponents.DotNetBar.RoundRectangleShapeDescriptor(4);
            this.btnTestResultSelect01.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnTestResultSelect01.Tag = "4";
            this.btnTestResultSelect01.Click += new System.EventHandler(this.btnTestResultSelect01_Click);
            // 
            // btnTestResultSelect02
            // 
            this.btnTestResultSelect02.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnTestResultSelect02.AntiAlias = true;
            this.btnTestResultSelect02.BackColor = System.Drawing.Color.Transparent;
            this.btnTestResultSelect02.ColorTable = DevComponents.DotNetBar.eButtonColor.Office2007WithBackground;
            resources.ApplyResources(this.btnTestResultSelect02, "btnTestResultSelect02");
            this.btnTestResultSelect02.Name = "btnTestResultSelect02";
            this.btnTestResultSelect02.Shape = new DevComponents.DotNetBar.RoundRectangleShapeDescriptor(4);
            this.btnTestResultSelect02.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnTestResultSelect02.Tag = "4";
            this.btnTestResultSelect02.Click += new System.EventHandler(this.btnTestResultSelect02_Click);
            // 
            // frmUploadRecipe
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImage = global::MPI.Tester.Gui.Properties.Resources.mini_hero_bg;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.btnTestResultSelect01);
            this.Controls.Add(this.btnTestResultSelect02);
            this.Controls.Add(this.chkIsEnableUploadMap);
            this.Controls.Add(this.chkIsEnableUploadProduct);
            this.Controls.Add(this.lblMapPath);
            this.Controls.Add(this.txtUploadMapPath);
            this.Controls.Add(this.lblPdPath);
            this.Controls.Add(this.txtUploadProductPath);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnUpload);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmUploadRecipe";
            this.Load += new System.EventHandler(this.frmUploadRecipe_Load);
            this.ResumeLayout(false);

		}
		#endregion

        private DevComponents.DotNetBar.ButtonX btnUpload;
        private DevComponents.DotNetBar.ButtonX btnExit;
        private DevComponents.DotNetBar.LabelX lblPdPath;
        private DevComponents.DotNetBar.Controls.TextBoxX txtUploadProductPath;
        private DevComponents.DotNetBar.LabelX lblMapPath;
        private DevComponents.DotNetBar.Controls.TextBoxX txtUploadMapPath;
        private DevComponents.DotNetBar.Controls.CheckBoxX chkIsEnableUploadProduct;
        private DevComponents.DotNetBar.Controls.CheckBoxX chkIsEnableUploadMap;
        private DevComponents.DotNetBar.ButtonX btnTestResultSelect01;
        private DevComponents.DotNetBar.ButtonX btnTestResultSelect02;

	}
}

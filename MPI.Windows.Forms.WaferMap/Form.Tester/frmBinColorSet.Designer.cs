namespace MPI.Windows.Forms
{
	partial class frmBinColorSet
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmBinColorSet));
			this.cobItem = new System.Windows.Forms.ComboBox();
			this.listboxItem = new System.Windows.Forms.ListBox();
			this.btnAdd = new System.Windows.Forms.Button();
			this.btnDelete = new System.Windows.Forms.Button();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.chkBoxOutOfRange = new System.Windows.Forms.CheckBox();
			this.txtMinVal = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.txtStepVal = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.btnSet = new System.Windows.Forms.Button();
			this.txtMaxVal = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.btnDefaultColor = new System.Windows.Forms.Button();
			this.colorDialog = new System.Windows.Forms.ColorDialog();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.txtMaxColor = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.txtMinColor = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.btnSave = new System.Windows.Forms.Button();
			this.btnExit = new System.Windows.Forms.Button();
			this.colorPickerButton1 = new DevComponents.DotNetBar.ColorPickerButton();
			this.groupBox4 = new System.Windows.Forms.GroupBox();
			this.label7 = new System.Windows.Forms.Label();
			this.lblBValue = new System.Windows.Forms.Label();
			this.lblGValue = new System.Windows.Forms.Label();
			this.lblRValue = new System.Windows.Forms.Label();
			this.btnSettingColor = new System.Windows.Forms.Button();
			this.sliderB = new DevComponents.DotNetBar.Controls.Slider();
			this.sliderG = new DevComponents.DotNetBar.Controls.Slider();
			this.sliderR = new DevComponents.DotNetBar.Controls.Slider();
			this.label6 = new System.Windows.Forms.Label();
			this.lblCurrentColor = new DevComponents.DotNetBar.LabelX();
			this.txtMapBackColor = new System.Windows.Forms.TextBox();
			this.labMapBackColor = new System.Windows.Forms.Label();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.groupBox4.SuspendLayout();
			this.SuspendLayout();
			// 
			// cobItem
			// 
			this.cobItem.Enabled = false;
			this.cobItem.FormattingEnabled = true;
			this.cobItem.Location = new System.Drawing.Point(15, 530);
			this.cobItem.Name = "cobItem";
			this.cobItem.Size = new System.Drawing.Size(121, 20);
			this.cobItem.TabIndex = 0;
			// 
			// listboxItem
			// 
			this.listboxItem.FormattingEnabled = true;
			this.listboxItem.ItemHeight = 12;
			this.listboxItem.Location = new System.Drawing.Point(20, 25);
			this.listboxItem.Name = "listboxItem";
			this.listboxItem.Size = new System.Drawing.Size(120, 328);
			this.listboxItem.TabIndex = 1;
			this.listboxItem.SelectedIndexChanged += new System.EventHandler(this.listboxItem_SelectedIndexChanged);
			// 
			// btnAdd
			// 
			this.btnAdd.Enabled = false;
			this.btnAdd.Location = new System.Drawing.Point(20, 570);
			this.btnAdd.Name = "btnAdd";
			this.btnAdd.Size = new System.Drawing.Size(115, 23);
			this.btnAdd.TabIndex = 2;
			this.btnAdd.Text = "新增";
			this.btnAdd.UseVisualStyleBackColor = true;
			this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
			// 
			// btnDelete
			// 
			this.btnDelete.Enabled = false;
			this.btnDelete.Location = new System.Drawing.Point(20, 605);
			this.btnDelete.Name = "btnDelete";
			this.btnDelete.Size = new System.Drawing.Size(115, 23);
			this.btnDelete.TabIndex = 2;
			this.btnDelete.Text = "刪除";
			this.btnDelete.UseVisualStyleBackColor = true;
			this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.chkBoxOutOfRange);
			this.groupBox1.Controls.Add(this.listboxItem);
			this.groupBox1.Controls.Add(this.txtMinVal);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.txtStepVal);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Controls.Add(this.btnSet);
			this.groupBox1.Controls.Add(this.txtMaxVal);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Location = new System.Drawing.Point(10, 10);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(160, 508);
			this.groupBox1.TabIndex = 3;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Item";
			// 
			// chkBoxOutOfRange
			// 
			this.chkBoxOutOfRange.AutoSize = true;
			this.chkBoxOutOfRange.Location = new System.Drawing.Point(24, 366);
			this.chkBoxOutOfRange.Name = "chkBoxOutOfRange";
			this.chkBoxOutOfRange.Size = new System.Drawing.Size(124, 16);
			this.chkBoxOutOfRange.TabIndex = 2;
			this.chkBoxOutOfRange.Text = "Enable Out Of Range";
			this.chkBoxOutOfRange.UseVisualStyleBackColor = true;
			this.chkBoxOutOfRange.CheckedChanged += new System.EventHandler(this.chkBoxOutOfRange_CheckedChanged);
			// 
			// txtMinVal
			// 
			this.txtMinVal.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtMinVal.Location = new System.Drawing.Point(84, 389);
			this.txtMinVal.Name = "txtMinVal";
			this.txtMinVal.Size = new System.Drawing.Size(50, 21);
			this.txtMinVal.TabIndex = 0;
			this.txtMinVal.Text = "1";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label3.Location = new System.Drawing.Point(27, 451);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(46, 15);
			this.label3.TabIndex = 1;
			this.label3.Text = "interval";
			// 
			// txtStepVal
			// 
			this.txtStepVal.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtStepVal.Location = new System.Drawing.Point(84, 449);
			this.txtStepVal.Name = "txtStepVal";
			this.txtStepVal.Size = new System.Drawing.Size(50, 21);
			this.txtStepVal.TabIndex = 0;
			this.txtStepVal.Text = "0.1";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(27, 391);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(28, 15);
			this.label1.TabIndex = 1;
			this.label1.Text = "min";
			// 
			// btnSet
			// 
			this.btnSet.BackColor = System.Drawing.Color.White;
			this.btnSet.Location = new System.Drawing.Point(32, 476);
			this.btnSet.Name = "btnSet";
			this.btnSet.Size = new System.Drawing.Size(88, 23);
			this.btnSet.TabIndex = 2;
			this.btnSet.Text = "Divide";
			this.btnSet.UseVisualStyleBackColor = false;
			this.btnSet.Click += new System.EventHandler(this.btnSet_Click);
			// 
			// txtMaxVal
			// 
			this.txtMaxVal.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtMaxVal.Location = new System.Drawing.Point(84, 419);
			this.txtMaxVal.Name = "txtMaxVal";
			this.txtMaxVal.Size = new System.Drawing.Size(50, 21);
			this.txtMaxVal.TabIndex = 0;
			this.txtMaxVal.Text = "5";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label2.Location = new System.Drawing.Point(27, 422);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(31, 15);
			this.label2.TabIndex = 1;
			this.label2.Text = "max";
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.btnDefaultColor);
			this.groupBox2.Location = new System.Drawing.Point(181, 67);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(233, 450);
			this.groupBox2.TabIndex = 4;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "groupBox2";
			// 
			// btnDefaultColor
			// 
			this.btnDefaultColor.BackColor = System.Drawing.Color.White;
			this.btnDefaultColor.Location = new System.Drawing.Point(14, 419);
			this.btnDefaultColor.Name = "btnDefaultColor";
			this.btnDefaultColor.Size = new System.Drawing.Size(88, 23);
			this.btnDefaultColor.TabIndex = 0;
			this.btnDefaultColor.Text = "Default Color";
			this.btnDefaultColor.UseVisualStyleBackColor = false;
			this.btnDefaultColor.Click += new System.EventHandler(this.btnDefaultColor_Click);
			// 
			// groupBox3
			// 
			this.groupBox3.Controls.Add(this.txtMaxColor);
			this.groupBox3.Controls.Add(this.label5);
			this.groupBox3.Controls.Add(this.txtMinColor);
			this.groupBox3.Controls.Add(this.label4);
			this.groupBox3.Location = new System.Drawing.Point(181, 11);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(233, 51);
			this.groupBox3.TabIndex = 5;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "Out Of Range";
			// 
			// txtMaxColor
			// 
			this.txtMaxColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtMaxColor.Font = new System.Drawing.Font("PMingLiU", 5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.txtMaxColor.Location = new System.Drawing.Point(195, 22);
			this.txtMaxColor.Name = "txtMaxColor";
			this.txtMaxColor.ReadOnly = true;
			this.txtMaxColor.Size = new System.Drawing.Size(15, 15);
			this.txtMaxColor.TabIndex = 1;
			this.txtMaxColor.Click += new System.EventHandler(this.txtMaxColor_Click);
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label5.Location = new System.Drawing.Point(134, 24);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(55, 13);
			this.label5.TabIndex = 0;
			this.label5.Text = "Max Color";
			this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// txtMinColor
			// 
			this.txtMinColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtMinColor.Font = new System.Drawing.Font("PMingLiU", 5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.txtMinColor.Location = new System.Drawing.Point(75, 22);
			this.txtMinColor.Name = "txtMinColor";
			this.txtMinColor.ReadOnly = true;
			this.txtMinColor.Size = new System.Drawing.Size(15, 15);
			this.txtMinColor.TabIndex = 1;
			this.txtMinColor.Click += new System.EventHandler(this.txtMinColor_Click);
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label4.Location = new System.Drawing.Point(20, 24);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(51, 13);
			this.label4.TabIndex = 0;
			this.label4.Text = "Min Color";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// btnSave
			// 
			this.btnSave.BackColor = System.Drawing.Color.YellowGreen;
			this.btnSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnSave.Location = new System.Drawing.Point(420, 481);
			this.btnSave.Name = "btnSave";
			this.btnSave.Size = new System.Drawing.Size(100, 37);
			this.btnSave.TabIndex = 6;
			this.btnSave.Text = "SAVE";
			this.btnSave.UseVisualStyleBackColor = false;
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			// 
			// btnExit
			// 
			this.btnExit.BackColor = System.Drawing.Color.LightCoral;
			this.btnExit.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnExit.Location = new System.Drawing.Point(523, 481);
			this.btnExit.Name = "btnExit";
			this.btnExit.Size = new System.Drawing.Size(100, 37);
			this.btnExit.TabIndex = 6;
			this.btnExit.Text = "EXIT";
			this.btnExit.UseVisualStyleBackColor = false;
			this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
			// 
			// colorPickerButton1
			// 
			this.colorPickerButton1.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
			this.colorPickerButton1.Checked = true;
			this.colorPickerButton1.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
			this.colorPickerButton1.Image = ((System.Drawing.Image)(resources.GetObject("colorPickerButton1.Image")));
			this.colorPickerButton1.Location = new System.Drawing.Point(112, 247);
			this.colorPickerButton1.Name = "colorPickerButton1";
			this.colorPickerButton1.SelectedColorImageRectangle = new System.Drawing.Rectangle(2, 2, 12, 12);
			this.colorPickerButton1.Size = new System.Drawing.Size(75, 20);
			this.colorPickerButton1.Style = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
			this.colorPickerButton1.TabIndex = 7;
			this.colorPickerButton1.SelectedColorChanged += new System.EventHandler(this.colorPickerButton1_SelectedColorChanged);
			// 
			// groupBox4
			// 
			this.groupBox4.Controls.Add(this.labMapBackColor);
			this.groupBox4.Controls.Add(this.txtMapBackColor);
			this.groupBox4.Controls.Add(this.label7);
			this.groupBox4.Controls.Add(this.lblBValue);
			this.groupBox4.Controls.Add(this.lblGValue);
			this.groupBox4.Controls.Add(this.lblRValue);
			this.groupBox4.Controls.Add(this.btnSettingColor);
			this.groupBox4.Controls.Add(this.sliderB);
			this.groupBox4.Controls.Add(this.sliderG);
			this.groupBox4.Controls.Add(this.sliderR);
			this.groupBox4.Controls.Add(this.label6);
			this.groupBox4.Controls.Add(this.lblCurrentColor);
			this.groupBox4.Controls.Add(this.colorPickerButton1);
			this.groupBox4.Location = new System.Drawing.Point(420, 11);
			this.groupBox4.Name = "groupBox4";
			this.groupBox4.Size = new System.Drawing.Size(204, 464);
			this.groupBox4.TabIndex = 6;
			this.groupBox4.TabStop = false;
			this.groupBox4.Text = "Color Setting";
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(13, 252);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(90, 12);
			this.label7.TabIndex = 17;
			this.label7.Text = "adv. Setting Color";
			// 
			// lblBValue
			// 
			this.lblBValue.AutoSize = true;
			this.lblBValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblBValue.Location = new System.Drawing.Point(164, 212);
			this.lblBValue.Name = "lblBValue";
			this.lblBValue.Size = new System.Drawing.Size(28, 13);
			this.lblBValue.TabIndex = 16;
			this.lblBValue.Text = "255";
			// 
			// lblGValue
			// 
			this.lblGValue.AutoSize = true;
			this.lblGValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblGValue.Location = new System.Drawing.Point(164, 170);
			this.lblGValue.Name = "lblGValue";
			this.lblGValue.Size = new System.Drawing.Size(28, 13);
			this.lblGValue.TabIndex = 15;
			this.lblGValue.Text = "255";
			// 
			// lblRValue
			// 
			this.lblRValue.AutoSize = true;
			this.lblRValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblRValue.Location = new System.Drawing.Point(164, 134);
			this.lblRValue.Name = "lblRValue";
			this.lblRValue.Size = new System.Drawing.Size(28, 13);
			this.lblRValue.TabIndex = 14;
			this.lblRValue.Text = "255";
			// 
			// btnSettingColor
			// 
			this.btnSettingColor.BackColor = System.Drawing.Color.WhiteSmoke;
			this.btnSettingColor.Location = new System.Drawing.Point(92, 300);
			this.btnSettingColor.Name = "btnSettingColor";
			this.btnSettingColor.Size = new System.Drawing.Size(100, 37);
			this.btnSettingColor.TabIndex = 13;
			this.btnSettingColor.Text = "Setting Color";
			this.btnSettingColor.UseVisualStyleBackColor = false;
			this.btnSettingColor.Click += new System.EventHandler(this.btnSettingColor_Click);
			// 
			// sliderB
			// 
			// 
			// 
			// 
			this.sliderB.BackgroundStyle.Class = "";
			this.sliderB.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
			this.sliderB.LabelWidth = 20;
			this.sliderB.Location = new System.Drawing.Point(14, 205);
			this.sliderB.Maximum = 255;
			this.sliderB.Name = "sliderB";
			this.sliderB.Size = new System.Drawing.Size(140, 21);
			this.sliderB.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
			this.sliderB.TabIndex = 12;
			this.sliderB.Text = "B";
			this.sliderB.TextColor = System.Drawing.Color.Blue;
			this.sliderB.Value = 0;
			this.sliderB.Click += new System.EventHandler(this.sliderRGB_Click);
			// 
			// sliderG
			// 
			// 
			// 
			// 
			this.sliderG.BackgroundStyle.Class = "";
			this.sliderG.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
			this.sliderG.LabelWidth = 20;
			this.sliderG.Location = new System.Drawing.Point(14, 168);
			this.sliderG.Maximum = 255;
			this.sliderG.Name = "sliderG";
			this.sliderG.Size = new System.Drawing.Size(140, 21);
			this.sliderG.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
			this.sliderG.TabIndex = 11;
			this.sliderG.Text = "G";
			this.sliderG.TextColor = System.Drawing.Color.Green;
			this.sliderG.Value = 0;
			this.sliderG.Click += new System.EventHandler(this.sliderRGB_Click);
			// 
			// sliderR
			// 
			// 
			// 
			// 
			this.sliderR.BackgroundStyle.Class = "";
			this.sliderR.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
			this.sliderR.LabelWidth = 20;
			this.sliderR.Location = new System.Drawing.Point(14, 132);
			this.sliderR.Maximum = 255;
			this.sliderR.Name = "sliderR";
			this.sliderR.Size = new System.Drawing.Size(140, 21);
			this.sliderR.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
			this.sliderR.TabIndex = 10;
			this.sliderR.Text = "R";
			this.sliderR.TextColor = System.Drawing.Color.Red;
			this.sliderR.Value = 0;
			this.sliderR.Click += new System.EventHandler(this.sliderRGB_Click);
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(24, 85);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(32, 12);
			this.label6.TabIndex = 9;
			this.label6.Text = "Color";
			// 
			// lblCurrentColor
			// 
			this.lblCurrentColor.BackColor = System.Drawing.Color.White;
			// 
			// 
			// 
			this.lblCurrentColor.BackgroundStyle.Class = "";
			this.lblCurrentColor.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
			this.lblCurrentColor.Location = new System.Drawing.Point(80, 81);
			this.lblCurrentColor.Name = "lblCurrentColor";
			this.lblCurrentColor.Size = new System.Drawing.Size(86, 21);
			this.lblCurrentColor.TabIndex = 8;
			this.lblCurrentColor.BackColorChanged += new System.EventHandler(this.lblCurrentColor_BackColorChanged);
			// 
			// txtMapBackColor
			// 
			this.txtMapBackColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtMapBackColor.Font = new System.Drawing.Font("PMingLiU", 5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.txtMapBackColor.Location = new System.Drawing.Point(112, 24);
			this.txtMapBackColor.Name = "txtMapBackColor";
			this.txtMapBackColor.ReadOnly = true;
			this.txtMapBackColor.Size = new System.Drawing.Size(15, 15);
			this.txtMapBackColor.TabIndex = 18;
			this.txtMapBackColor.Click += new System.EventHandler(this.txtMapBackColor_Click);
			// 
			// labMapBackColor
			// 
			this.labMapBackColor.AutoSize = true;
			this.labMapBackColor.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labMapBackColor.Location = new System.Drawing.Point(12, 24);
			this.labMapBackColor.Name = "labMapBackColor";
			this.labMapBackColor.Size = new System.Drawing.Size(80, 13);
			this.labMapBackColor.TabIndex = 2;
			this.labMapBackColor.Text = "Map Back Color";
			this.labMapBackColor.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// frmBinColorSet
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.WhiteSmoke;
			this.ClientSize = new System.Drawing.Size(636, 530);
			this.Controls.Add(this.btnExit);
			this.Controls.Add(this.groupBox4);
			this.Controls.Add(this.btnSave);
			this.Controls.Add(this.groupBox3);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.btnDelete);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.cobItem);
			this.Controls.Add(this.btnAdd);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "frmBinColorSet";
			this.Text = "Bin Color Set";
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.groupBox3.ResumeLayout(false);
			this.groupBox3.PerformLayout();
			this.groupBox4.ResumeLayout(false);
			this.groupBox4.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ComboBox cobItem;
		private System.Windows.Forms.ListBox listboxItem;
		private System.Windows.Forms.Button btnAdd;
		private System.Windows.Forms.Button btnDelete;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txtMinVal;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox txtMaxVal;
		private System.Windows.Forms.TextBox txtStepVal;
		private System.Windows.Forms.Button btnSet;
		private System.Windows.Forms.ColorDialog colorDialog;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.TextBox txtMinColor;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox txtMaxColor;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.CheckBox chkBoxOutOfRange;
		private System.Windows.Forms.Button btnSave;
		private System.Windows.Forms.Button btnExit;
		private DevComponents.DotNetBar.ColorPickerButton colorPickerButton1;
		private System.Windows.Forms.GroupBox groupBox4;
		private System.Windows.Forms.Label label6;
		private DevComponents.DotNetBar.LabelX lblCurrentColor;
		private DevComponents.DotNetBar.Controls.Slider sliderB;
		private DevComponents.DotNetBar.Controls.Slider sliderG;
		private DevComponents.DotNetBar.Controls.Slider sliderR;
		private System.Windows.Forms.Button btnSettingColor;
		private System.Windows.Forms.Label lblBValue;
		private System.Windows.Forms.Label lblGValue;
		private System.Windows.Forms.Label lblRValue;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Button btnDefaultColor;
		private System.Windows.Forms.Label labMapBackColor;
		private System.Windows.Forms.TextBox txtMapBackColor;
	}
}


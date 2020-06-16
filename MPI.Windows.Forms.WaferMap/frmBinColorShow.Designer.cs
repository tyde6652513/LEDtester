namespace MPI.Windows.Forms
{
	partial class frmBinColorShow
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
            this.StatisticsGroup = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.lblMaxColorPcs = new System.Windows.Forms.Label();
            this.lblMinColorPcs = new System.Windows.Forms.Label();
            this.txtMaxColor = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtMinColor = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.rdoPcs = new System.Windows.Forms.RadioButton();
            this.rdoPrs = new System.Windows.Forms.RadioButton();
            this.grpBoxInfo = new System.Windows.Forms.GroupBox();
            this.chkBoxOutOfRange = new System.Windows.Forms.CheckBox();
            this.txtMinVal = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtStepVal = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtMaxVal = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.grpBoxInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // StatisticsGroup
            // 
            this.StatisticsGroup.Location = new System.Drawing.Point(4, -6);
            this.StatisticsGroup.Name = "StatisticsGroup";
            this.StatisticsGroup.Size = new System.Drawing.Size(191, 400);
            this.StatisticsGroup.TabIndex = 5;
            this.StatisticsGroup.TabStop = false;
            // 
            // groupBox3
            // 
            this.groupBox3.Location = new System.Drawing.Point(226, 224);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(140, 85);
            this.groupBox3.TabIndex = 6;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Out Of Range";
            // 
            // lblMaxColorPcs
            // 
            this.lblMaxColorPcs.AutoSize = true;
            this.lblMaxColorPcs.Font = new System.Drawing.Font("Tahoma", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMaxColorPcs.Location = new System.Drawing.Point(100, 409);
            this.lblMaxColorPcs.Name = "lblMaxColorPcs";
            this.lblMaxColorPcs.Size = new System.Drawing.Size(12, 11);
            this.lblMaxColorPcs.TabIndex = 2;
            this.lblMaxColorPcs.Text = "0";
            // 
            // lblMinColorPcs
            // 
            this.lblMinColorPcs.AutoSize = true;
            this.lblMinColorPcs.Font = new System.Drawing.Font("Tahoma", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMinColorPcs.Location = new System.Drawing.Point(100, 396);
            this.lblMinColorPcs.Name = "lblMinColorPcs";
            this.lblMinColorPcs.Size = new System.Drawing.Size(12, 11);
            this.lblMinColorPcs.TabIndex = 2;
            this.lblMinColorPcs.Text = "0";
            // 
            // txtMaxColor
            // 
            this.txtMaxColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtMaxColor.Font = new System.Drawing.Font("PMingLiU", 5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.txtMaxColor.Location = new System.Drawing.Point(74, 408);
            this.txtMaxColor.Name = "txtMaxColor";
            this.txtMaxColor.Size = new System.Drawing.Size(12, 12);
            this.txtMaxColor.TabIndex = 1;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Tahoma", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(20, 409);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(36, 11);
            this.label5.TabIndex = 0;
            this.label5.Text = "> Max";
            // 
            // txtMinColor
            // 
            this.txtMinColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtMinColor.Font = new System.Drawing.Font("PMingLiU", 5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.txtMinColor.Location = new System.Drawing.Point(74, 395);
            this.txtMinColor.Name = "txtMinColor";
            this.txtMinColor.Size = new System.Drawing.Size(12, 12);
            this.txtMinColor.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Tahoma", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(20, 396);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(33, 11);
            this.label4.TabIndex = 0;
            this.label4.Text = "< Min";
            // 
            // rdoPcs
            // 
            this.rdoPcs.AutoSize = true;
            this.rdoPcs.Checked = true;
            this.rdoPcs.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdoPcs.ForeColor = System.Drawing.Color.MidnightBlue;
            this.rdoPcs.Location = new System.Drawing.Point(12, 425);
            this.rdoPcs.Name = "rdoPcs";
            this.rdoPcs.Size = new System.Drawing.Size(49, 18);
            this.rdoPcs.TabIndex = 7;
            this.rdoPcs.TabStop = true;
            this.rdoPcs.Text = "PCS";
            this.rdoPcs.UseVisualStyleBackColor = true;
            this.rdoPcs.CheckedChanged += new System.EventHandler(this.rdoPcs_CheckedChanged);
            // 
            // rdoPrs
            // 
            this.rdoPrs.AutoSize = true;
            this.rdoPrs.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdoPrs.ForeColor = System.Drawing.Color.MidnightBlue;
            this.rdoPrs.Location = new System.Drawing.Point(77, 425);
            this.rdoPrs.Name = "rdoPrs";
            this.rdoPrs.Size = new System.Drawing.Size(73, 18);
            this.rdoPrs.TabIndex = 7;
            this.rdoPrs.TabStop = true;
            this.rdoPrs.Text = "PCS(%)";
            this.rdoPrs.UseVisualStyleBackColor = true;
            this.rdoPrs.CheckedChanged += new System.EventHandler(this.rdoPrs_CheckedChanged);
            // 
            // grpBoxInfo
            // 
            this.grpBoxInfo.Controls.Add(this.chkBoxOutOfRange);
            this.grpBoxInfo.Controls.Add(this.txtMinVal);
            this.grpBoxInfo.Controls.Add(this.label3);
            this.grpBoxInfo.Controls.Add(this.txtStepVal);
            this.grpBoxInfo.Controls.Add(this.label1);
            this.grpBoxInfo.Controls.Add(this.txtMaxVal);
            this.grpBoxInfo.Controls.Add(this.label2);
            this.grpBoxInfo.Location = new System.Drawing.Point(213, 435);
            this.grpBoxInfo.Name = "grpBoxInfo";
            this.grpBoxInfo.Size = new System.Drawing.Size(140, 135);
            this.grpBoxInfo.TabIndex = 8;
            this.grpBoxInfo.TabStop = false;
            this.grpBoxInfo.Text = "Info.";
            // 
            // chkBoxOutOfRange
            // 
            this.chkBoxOutOfRange.AutoCheck = false;
            this.chkBoxOutOfRange.AutoSize = true;
            this.chkBoxOutOfRange.CausesValidation = false;
            this.chkBoxOutOfRange.Location = new System.Drawing.Point(8, 16);
            this.chkBoxOutOfRange.Name = "chkBoxOutOfRange";
            this.chkBoxOutOfRange.Size = new System.Drawing.Size(142, 18);
            this.chkBoxOutOfRange.TabIndex = 9;
            this.chkBoxOutOfRange.Text = "Enable Out Of Range";
            this.chkBoxOutOfRange.UseVisualStyleBackColor = false;
            this.chkBoxOutOfRange.Visible = false;
            // 
            // txtMinVal
            // 
            this.txtMinVal.Enabled = false;
            this.txtMinVal.Location = new System.Drawing.Point(65, 40);
            this.txtMinVal.Name = "txtMinVal";
            this.txtMinVal.Size = new System.Drawing.Size(35, 22);
            this.txtMinVal.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 100);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(48, 14);
            this.label3.TabIndex = 7;
            this.label3.Text = "Interval";
            // 
            // txtStepVal
            // 
            this.txtStepVal.Enabled = false;
            this.txtStepVal.Location = new System.Drawing.Point(65, 100);
            this.txtStepVal.Name = "txtStepVal";
            this.txtStepVal.Size = new System.Drawing.Size(35, 22);
            this.txtStepVal.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(25, 14);
            this.label1.TabIndex = 6;
            this.label1.Text = "Min";
            // 
            // txtMaxVal
            // 
            this.txtMaxVal.Enabled = false;
            this.txtMaxVal.Location = new System.Drawing.Point(65, 70);
            this.txtMaxVal.Name = "txtMaxVal";
            this.txtMaxVal.Size = new System.Drawing.Size(35, 22);
            this.txtMaxVal.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 70);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(28, 14);
            this.label2.TabIndex = 8;
            this.label2.Text = "Max";
            // 
            // frmBinColorShow
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(199, 446);
            this.ControlBox = false;
            this.Controls.Add(this.lblMaxColorPcs);
            this.Controls.Add(this.grpBoxInfo);
            this.Controls.Add(this.lblMinColorPcs);
            this.Controls.Add(this.rdoPrs);
            this.Controls.Add(this.txtMaxColor);
            this.Controls.Add(this.rdoPcs);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.txtMinColor);
            this.Controls.Add(this.StatisticsGroup);
            this.Controls.Add(this.label4);
            this.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmBinColorShow";
            this.Text = "frmBinColorShow";
            this.Load += new System.EventHandler(this.frmBinColorShow_Load);
            this.grpBoxInfo.ResumeLayout(false);
            this.grpBoxInfo.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.GroupBox StatisticsGroup;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.Label txtMaxColor;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label txtMinColor;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.RadioButton rdoPcs;
		private System.Windows.Forms.RadioButton rdoPrs;
		private System.Windows.Forms.GroupBox grpBoxInfo;
		private System.Windows.Forms.CheckBox chkBoxOutOfRange;
		private System.Windows.Forms.Label txtMinVal;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label txtStepVal;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label txtMaxVal;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label lblMaxColorPcs;
		private System.Windows.Forms.Label lblMinColorPcs;
	}
}
namespace MPI.Tester.Gui
{
    partial class frmPIVGraphComponent
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
            this.components = new System.ComponentModel.Container();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.chkIsShowIV = new System.Windows.Forms.CheckBox();
            this.chkIsShowSE = new System.Windows.Forms.CheckBox();
            this.chkIsShowRS = new System.Windows.Forms.CheckBox();
            this.chkIsShowPCE = new System.Windows.Forms.CheckBox();
            this.cmbSelectItem = new System.Windows.Forms.ComboBox();
            this.lblSelectCurve = new System.Windows.Forms.Label();
            this.zGraph = new ZedGraph.ZedGraphControl();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.chkIsShowIV);
            this.splitContainer1.Panel1.Controls.Add(this.chkIsShowSE);
            this.splitContainer1.Panel1.Controls.Add(this.chkIsShowRS);
            this.splitContainer1.Panel1.Controls.Add(this.chkIsShowPCE);
            this.splitContainer1.Panel1.Controls.Add(this.cmbSelectItem);
            this.splitContainer1.Panel1.Controls.Add(this.lblSelectCurve);
            this.splitContainer1.Panel1MinSize = 20;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.zGraph);
            this.splitContainer1.Panel2MinSize = 20;
            this.splitContainer1.Size = new System.Drawing.Size(715, 341);
            this.splitContainer1.SplitterDistance = 25;
            this.splitContainer1.TabIndex = 13;
            // 
            // chkIsShowIV
            // 
            this.chkIsShowIV.AutoSize = true;
            this.chkIsShowIV.Checked = true;
            this.chkIsShowIV.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkIsShowIV.Dock = System.Windows.Forms.DockStyle.Right;
            this.chkIsShowIV.Location = new System.Drawing.Point(528, 0);
            this.chkIsShowIV.Name = "chkIsShowIV";
            this.chkIsShowIV.Size = new System.Drawing.Size(49, 25);
            this.chkIsShowIV.TabIndex = 146;
            this.chkIsShowIV.Text = "I-V   ";
            this.chkIsShowIV.UseVisualStyleBackColor = true;
            // 
            // chkIsShowSE
            // 
            this.chkIsShowSE.AutoSize = true;
            this.chkIsShowSE.Dock = System.Windows.Forms.DockStyle.Right;
            this.chkIsShowSE.Location = new System.Drawing.Point(577, 0);
            this.chkIsShowSE.Name = "chkIsShowSE";
            this.chkIsShowSE.Size = new System.Drawing.Size(46, 25);
            this.chkIsShowSE.TabIndex = 145;
            this.chkIsShowSE.Text = "SE   ";
            this.chkIsShowSE.UseVisualStyleBackColor = true;
            // 
            // chkIsShowRS
            // 
            this.chkIsShowRS.AutoSize = true;
            this.chkIsShowRS.Dock = System.Windows.Forms.DockStyle.Right;
            this.chkIsShowRS.Location = new System.Drawing.Point(623, 0);
            this.chkIsShowRS.Name = "chkIsShowRS";
            this.chkIsShowRS.Size = new System.Drawing.Size(47, 25);
            this.chkIsShowRS.TabIndex = 144;
            this.chkIsShowRS.Text = "RS   ";
            this.chkIsShowRS.UseVisualStyleBackColor = true;
            // 
            // chkIsShowPCE
            // 
            this.chkIsShowPCE.AutoSize = true;
            this.chkIsShowPCE.Dock = System.Windows.Forms.DockStyle.Right;
            this.chkIsShowPCE.Location = new System.Drawing.Point(670, 0);
            this.chkIsShowPCE.Name = "chkIsShowPCE";
            this.chkIsShowPCE.Size = new System.Drawing.Size(45, 25);
            this.chkIsShowPCE.TabIndex = 140;
            this.chkIsShowPCE.Text = "PCE";
            this.chkIsShowPCE.UseVisualStyleBackColor = true;
            // 
            // cmbSelectItem
            // 
            this.cmbSelectItem.Dock = System.Windows.Forms.DockStyle.Left;
            this.cmbSelectItem.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSelectItem.FormattingEnabled = true;
            this.cmbSelectItem.Location = new System.Drawing.Point(80, 0);
            this.cmbSelectItem.Name = "cmbSelectItem";
            this.cmbSelectItem.Size = new System.Drawing.Size(75, 20);
            this.cmbSelectItem.TabIndex = 139;
            // 
            // lblSelectCurve
            // 
            this.lblSelectCurve.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblSelectCurve.Font = new System.Drawing.Font("Arial", 9F);
            this.lblSelectCurve.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblSelectCurve.Location = new System.Drawing.Point(0, 0);
            this.lblSelectCurve.Name = "lblSelectCurve";
            this.lblSelectCurve.Size = new System.Drawing.Size(80, 25);
            this.lblSelectCurve.TabIndex = 138;
            this.lblSelectCurve.Text = "Select Item";
            this.lblSelectCurve.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // zGraph
            // 
            this.zGraph.Dock = System.Windows.Forms.DockStyle.Fill;
            this.zGraph.Location = new System.Drawing.Point(0, 0);
            this.zGraph.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.zGraph.Name = "zGraph";
            this.zGraph.ScrollGrace = 0D;
            this.zGraph.ScrollMaxX = 0D;
            this.zGraph.ScrollMaxY = 0D;
            this.zGraph.ScrollMaxY2 = 0D;
            this.zGraph.ScrollMinX = 0D;
            this.zGraph.ScrollMinY = 0D;
            this.zGraph.ScrollMinY2 = 0D;
            this.zGraph.Size = new System.Drawing.Size(715, 312);
            this.zGraph.TabIndex = 13;
            // 
            // frmPIVGraphComponent
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(715, 341);
            this.Controls.Add(this.splitContainer1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmPIVGraphComponent";
            this.Text = "PIV Graph";
            this.VisibleChanged += new System.EventHandler(this.frmPIVGraphComponent_VisibleChanged);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private ZedGraph.ZedGraphControl zGraph;
        private System.Windows.Forms.ComboBox cmbSelectItem;
        private System.Windows.Forms.Label lblSelectCurve;
        private System.Windows.Forms.CheckBox chkIsShowPCE;
        private System.Windows.Forms.CheckBox chkIsShowIV;
        private System.Windows.Forms.CheckBox chkIsShowSE;
        private System.Windows.Forms.CheckBox chkIsShowRS;

    }
}
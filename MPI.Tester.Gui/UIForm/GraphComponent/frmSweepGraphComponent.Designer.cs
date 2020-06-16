namespace MPI.Tester.Gui
{
    partial class frmSweepGraphComponent
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSweepGraphComponent));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.chkyLog = new System.Windows.Forms.CheckBox();
            this.cmbSelectItem = new System.Windows.Forms.ComboBox();
            this.lblSelectCurve = new System.Windows.Forms.Label();
            this.lblChannel = new System.Windows.Forms.Label();
            this.cmbChannel = new System.Windows.Forms.ComboBox();
            this.lblXAxis = new System.Windows.Forms.Label();
            this.cmbXAxis = new System.Windows.Forms.ComboBox();
            this.lblYAxis = new System.Windows.Forms.Label();
            this.cmbYAxis = new System.Windows.Forms.ComboBox();
            this.lblLinearEq = new System.Windows.Forms.Label();
            this.zGraph = new ZedGraph.ZedGraphControl();
            this.chkxLog = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.BackColor = System.Drawing.Color.WhiteSmoke;
            resources.ApplyResources(this.splitContainer1, "splitContainer1");
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.chkxLog);
            this.splitContainer1.Panel1.Controls.Add(this.chkyLog);
            this.splitContainer1.Panel1.Controls.Add(this.cmbSelectItem);
            this.splitContainer1.Panel1.Controls.Add(this.lblSelectCurve);
            this.splitContainer1.Panel1.Controls.Add(this.lblChannel);
            this.splitContainer1.Panel1.Controls.Add(this.cmbChannel);
            this.splitContainer1.Panel1.Controls.Add(this.lblXAxis);
            this.splitContainer1.Panel1.Controls.Add(this.cmbXAxis);
            this.splitContainer1.Panel1.Controls.Add(this.lblYAxis);
            this.splitContainer1.Panel1.Controls.Add(this.cmbYAxis);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.lblLinearEq);
            this.splitContainer1.Panel2.Controls.Add(this.zGraph);
            // 
            // chkyLog
            // 
            resources.ApplyResources(this.chkyLog, "chkyLog");
            this.chkyLog.Name = "chkyLog";
            this.chkyLog.UseVisualStyleBackColor = true;
            // 
            // cmbSelectItem
            // 
            resources.ApplyResources(this.cmbSelectItem, "cmbSelectItem");
            this.cmbSelectItem.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSelectItem.FormattingEnabled = true;
            this.cmbSelectItem.Name = "cmbSelectItem";
            // 
            // lblSelectCurve
            // 
            resources.ApplyResources(this.lblSelectCurve, "lblSelectCurve");
            this.lblSelectCurve.Name = "lblSelectCurve";
            // 
            // lblChannel
            // 
            resources.ApplyResources(this.lblChannel, "lblChannel");
            this.lblChannel.Name = "lblChannel";
            // 
            // cmbChannel
            // 
            resources.ApplyResources(this.cmbChannel, "cmbChannel");
            this.cmbChannel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbChannel.FormattingEnabled = true;
            this.cmbChannel.Name = "cmbChannel";
            // 
            // lblXAxis
            // 
            resources.ApplyResources(this.lblXAxis, "lblXAxis");
            this.lblXAxis.Name = "lblXAxis";
            // 
            // cmbXAxis
            // 
            resources.ApplyResources(this.cmbXAxis, "cmbXAxis");
            this.cmbXAxis.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbXAxis.FormattingEnabled = true;
            this.cmbXAxis.Name = "cmbXAxis";
            // 
            // lblYAxis
            // 
            resources.ApplyResources(this.lblYAxis, "lblYAxis");
            this.lblYAxis.Name = "lblYAxis";
            // 
            // cmbYAxis
            // 
            resources.ApplyResources(this.cmbYAxis, "cmbYAxis");
            this.cmbYAxis.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbYAxis.Name = "cmbYAxis";
            // 
            // lblLinearEq
            // 
            resources.ApplyResources(this.lblLinearEq, "lblLinearEq");
            this.lblLinearEq.BackColor = System.Drawing.Color.Transparent;
            this.lblLinearEq.Name = "lblLinearEq";
            // 
            // zGraph
            // 
            resources.ApplyResources(this.zGraph, "zGraph");
            this.zGraph.Name = "zGraph";
            this.zGraph.ScrollGrace = 0D;
            this.zGraph.ScrollMaxX = 0D;
            this.zGraph.ScrollMaxY = 0D;
            this.zGraph.ScrollMaxY2 = 0D;
            this.zGraph.ScrollMinX = 0D;
            this.zGraph.ScrollMinY = 0D;
            this.zGraph.ScrollMinY2 = 0D;
            // 
            // chkxLog
            // 
            resources.ApplyResources(this.chkxLog, "chkxLog");
            this.chkxLog.Name = "chkxLog";
            this.chkxLog.UseVisualStyleBackColor = true;
            // 
            // frmSweepGraphComponent
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmSweepGraphComponent";
            this.VisibleChanged += new System.EventHandler(this.frmSweepGraphComponent_VisibleChanged);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private ZedGraph.ZedGraphControl zGraph;
        private System.Windows.Forms.Label lblChannel;
        private System.Windows.Forms.ComboBox cmbChannel;
        private System.Windows.Forms.Label lblXAxis;
        private System.Windows.Forms.ComboBox cmbXAxis;
        private System.Windows.Forms.Label lblYAxis;
        private System.Windows.Forms.ComboBox cmbYAxis;
        private System.Windows.Forms.ComboBox cmbSelectItem;
        private System.Windows.Forms.Label lblSelectCurve;
        private System.Windows.Forms.Label lblLinearEq;
        private System.Windows.Forms.CheckBox chkyLog;
        private System.Windows.Forms.CheckBox chkxLog;

    }
}
namespace MPI.Tester.Gui
{
    partial class frmLCRGraphComponent
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmLCRGraphComponent));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.pnlSelectedAxis = new System.Windows.Forms.Panel();
            this.lblXAxis = new System.Windows.Forms.Label();
            this.cmbYAxis = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbXAxis = new System.Windows.Forms.ComboBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.cmbSelectItem = new System.Windows.Forms.ComboBox();
            this.lblSelectCurve = new System.Windows.Forms.Label();
            this.zGraph = new ZedGraph.ZedGraphControl();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.pnlSelectedAxis.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            resources.ApplyResources(this.splitContainer1, "splitContainer1");
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.pnlSelectedAxis);
            this.splitContainer1.Panel1.Controls.Add(this.btnSave);
            this.splitContainer1.Panel1.Controls.Add(this.cmbSelectItem);
            this.splitContainer1.Panel1.Controls.Add(this.lblSelectCurve);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.zGraph);
            // 
            // pnlSelectedAxis
            // 
            this.pnlSelectedAxis.Controls.Add(this.lblXAxis);
            this.pnlSelectedAxis.Controls.Add(this.cmbYAxis);
            this.pnlSelectedAxis.Controls.Add(this.label2);
            this.pnlSelectedAxis.Controls.Add(this.cmbXAxis);
            resources.ApplyResources(this.pnlSelectedAxis, "pnlSelectedAxis");
            this.pnlSelectedAxis.Name = "pnlSelectedAxis";
            // 
            // lblXAxis
            // 
            resources.ApplyResources(this.lblXAxis, "lblXAxis");
            this.lblXAxis.Name = "lblXAxis";
            // 
            // cmbYAxis
            // 
            this.cmbYAxis.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbYAxis.DropDownWidth = 100;
            this.cmbYAxis.FormattingEnabled = true;
            resources.ApplyResources(this.cmbYAxis, "cmbYAxis");
            this.cmbYAxis.Name = "cmbYAxis";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // cmbXAxis
            // 
            this.cmbXAxis.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbXAxis.DropDownWidth = 100;
            this.cmbXAxis.FormattingEnabled = true;
            resources.ApplyResources(this.cmbXAxis, "cmbXAxis");
            this.cmbXAxis.Name = "cmbXAxis";
            // 
            // btnSave
            // 
            resources.ApplyResources(this.btnSave, "btnSave");
            this.btnSave.Name = "btnSave";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // cmbSelectItem
            // 
            resources.ApplyResources(this.cmbSelectItem, "cmbSelectItem");
            this.cmbSelectItem.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSelectItem.DropDownWidth = 100;
            this.cmbSelectItem.FormattingEnabled = true;
            this.cmbSelectItem.Name = "cmbSelectItem";
            // 
            // lblSelectCurve
            // 
            resources.ApplyResources(this.lblSelectCurve, "lblSelectCurve");
            this.lblSelectCurve.Name = "lblSelectCurve";
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
            this.zGraph.VisibleChanged += new System.EventHandler(this.zGraph_VisibleChanged);
            // 
            // frmLCRGraphComponent
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmLCRGraphComponent";
            this.VisibleChanged += new System.EventHandler(this.frmLCRGraphComponent_VisibleChanged);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.pnlSelectedAxis.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Label lblSelectCurve;
        private System.Windows.Forms.ComboBox cmbSelectItem;
        private System.Windows.Forms.Label lblXAxis;
        private System.Windows.Forms.ComboBox cmbXAxis;
        private System.Windows.Forms.ComboBox cmbYAxis;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Panel pnlSelectedAxis;
        private ZedGraph.ZedGraphControl zGraph;

    }
}
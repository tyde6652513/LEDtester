namespace MPI.Tester.Gui
{
    partial class frmLIVGraphComponent
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmLIVGraphComponent));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.pnlSelectedAxis = new System.Windows.Forms.Panel();
            this.cmbYAxis = new System.Windows.Forms.ComboBox();
            this.lblYAxis = new System.Windows.Forms.Label();
            this.cmbXAxis = new System.Windows.Forms.ComboBox();
            this.lblXAxis = new System.Windows.Forms.Label();
            this.cmbSelectItem = new System.Windows.Forms.ComboBox();
            this.lblSelectCurve = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
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
            this.splitContainer1.BackColor = System.Drawing.Color.WhiteSmoke;
            resources.ApplyResources(this.splitContainer1, "splitContainer1");
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.pnlSelectedAxis);
            this.splitContainer1.Panel1.Controls.Add(this.cmbSelectItem);
            this.splitContainer1.Panel1.Controls.Add(this.lblSelectCurve);
            this.splitContainer1.Panel1.Controls.Add(this.btnSave);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.zGraph);
            // 
            // pnlSelectedAxis
            // 
            this.pnlSelectedAxis.Controls.Add(this.cmbYAxis);
            this.pnlSelectedAxis.Controls.Add(this.lblYAxis);
            this.pnlSelectedAxis.Controls.Add(this.cmbXAxis);
            this.pnlSelectedAxis.Controls.Add(this.lblXAxis);
            resources.ApplyResources(this.pnlSelectedAxis, "pnlSelectedAxis");
            this.pnlSelectedAxis.Name = "pnlSelectedAxis";
            // 
            // cmbYAxis
            // 
            resources.ApplyResources(this.cmbYAxis, "cmbYAxis");
            this.cmbYAxis.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbYAxis.Name = "cmbYAxis";
            // 
            // lblYAxis
            // 
            resources.ApplyResources(this.lblYAxis, "lblYAxis");
            this.lblYAxis.Name = "lblYAxis";
            // 
            // cmbXAxis
            // 
            resources.ApplyResources(this.cmbXAxis, "cmbXAxis");
            this.cmbXAxis.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbXAxis.FormattingEnabled = true;
            this.cmbXAxis.Name = "cmbXAxis";
            // 
            // lblXAxis
            // 
            resources.ApplyResources(this.lblXAxis, "lblXAxis");
            this.lblXAxis.Name = "lblXAxis";
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
            // btnSave
            // 
            resources.ApplyResources(this.btnSave, "btnSave");
            this.btnSave.Name = "btnSave";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
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
            // frmLIVGraphComponent
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmLIVGraphComponent";
            this.VisibleChanged += new System.EventHandler(this.frmLIVGraphComponent_VisibleChanged);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.pnlSelectedAxis.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private ZedGraph.ZedGraphControl zGraph;
        private System.Windows.Forms.Label lblXAxis;
        private System.Windows.Forms.ComboBox cmbXAxis;
        private System.Windows.Forms.Label lblYAxis;
        private System.Windows.Forms.ComboBox cmbYAxis;
        private System.Windows.Forms.ComboBox cmbSelectItem;
        private System.Windows.Forms.Label lblSelectCurve;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Panel pnlSelectedAxis;

    }
}
namespace MPI.Tester.Gui
{
    partial class frmSpectrumGraphComponent
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSpectrumGraphComponent));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.pnlSelectedYaxis = new System.Windows.Forms.Panel();
            this.rdbRelative = new System.Windows.Forms.RadioButton();
            this.rdbAbsoluate = new System.Windows.Forms.RadioButton();
            this.cmbChannel = new System.Windows.Forms.ComboBox();
            this.lblChannel = new System.Windows.Forms.Label();
            this.cmbSelectItem = new System.Windows.Forms.ComboBox();
            this.lblSelectCurve = new System.Windows.Forms.Label();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.zGraph = new ZedGraph.ZedGraphControl();
            this.btnSave = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.pnlSelectedYaxis.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
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
            this.splitContainer1.Panel1.Controls.Add(this.pnlSelectedYaxis);
            this.splitContainer1.Panel1.Controls.Add(this.cmbChannel);
            this.splitContainer1.Panel1.Controls.Add(this.lblChannel);
            this.splitContainer1.Panel1.Controls.Add(this.cmbSelectItem);
            this.splitContainer1.Panel1.Controls.Add(this.lblSelectCurve);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            // 
            // pnlSelectedYaxis
            // 
            this.pnlSelectedYaxis.Controls.Add(this.rdbRelative);
            this.pnlSelectedYaxis.Controls.Add(this.rdbAbsoluate);
            resources.ApplyResources(this.pnlSelectedYaxis, "pnlSelectedYaxis");
            this.pnlSelectedYaxis.Name = "pnlSelectedYaxis";
            // 
            // rdbRelative
            // 
            resources.ApplyResources(this.rdbRelative, "rdbRelative");
            this.rdbRelative.Name = "rdbRelative";
            this.rdbRelative.UseVisualStyleBackColor = true;
            // 
            // rdbAbsoluate
            // 
            resources.ApplyResources(this.rdbAbsoluate, "rdbAbsoluate");
            this.rdbAbsoluate.Checked = true;
            this.rdbAbsoluate.Name = "rdbAbsoluate";
            this.rdbAbsoluate.TabStop = true;
            this.rdbAbsoluate.UseVisualStyleBackColor = true;
            this.rdbAbsoluate.CheckedChanged += new System.EventHandler(this.rdbAbsoluate_CheckedChanged);
            // 
            // cmbChannel
            // 
            resources.ApplyResources(this.cmbChannel, "cmbChannel");
            this.cmbChannel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbChannel.FormattingEnabled = true;
            this.cmbChannel.Name = "cmbChannel";
            // 
            // lblChannel
            // 
            resources.ApplyResources(this.lblChannel, "lblChannel");
            this.lblChannel.Name = "lblChannel";
            // 
            // cmbSelectItem
            // 
            resources.ApplyResources(this.cmbSelectItem, "cmbSelectItem");
            this.cmbSelectItem.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSelectItem.FormattingEnabled = true;
            this.cmbSelectItem.Name = "cmbSelectItem";
            this.cmbSelectItem.VisibleChanged += new System.EventHandler(this.cmbSelectItem_VisibleChanged);
            // 
            // lblSelectCurve
            // 
            resources.ApplyResources(this.lblSelectCurve, "lblSelectCurve");
            this.lblSelectCurve.Name = "lblSelectCurve";
            // 
            // splitContainer2
            // 
            resources.ApplyResources(this.splitContainer2, "splitContainer2");
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.zGraph);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.btnSave);
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
            // btnSave
            // 
            resources.ApplyResources(this.btnSave, "btnSave");
            this.btnSave.Name = "btnSave";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // frmSpectrumGraphComponent
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmSpectrumGraphComponent";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.pnlSelectedYaxis.ResumeLayout(false);
            this.pnlSelectedYaxis.PerformLayout();
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ComboBox cmbSelectItem;
        private System.Windows.Forms.Label lblSelectCurve;
        private System.Windows.Forms.Panel pnlSelectedYaxis;
        private System.Windows.Forms.RadioButton rdbAbsoluate;
        private System.Windows.Forms.RadioButton rdbRelative;
        private System.Windows.Forms.ComboBox cmbChannel;
        private System.Windows.Forms.Label lblChannel;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private ZedGraph.ZedGraphControl zGraph;
        private System.Windows.Forms.Button btnSave;

    }
}
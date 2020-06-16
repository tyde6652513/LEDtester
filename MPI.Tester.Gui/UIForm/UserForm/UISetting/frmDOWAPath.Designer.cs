namespace MPI.Tester.Gui.UIForm.UserForm.UISetting
{
    partial class frmDowaPath
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
            MPI.Tester.GuiComponent.PathInfo pathInfo1 = new MPI.Tester.GuiComponent.PathInfo();
            this.pathUIComponent1 = new MPI.Tester.GuiComponent.PathUIComponent();
            this.pnlBinMapPath = new System.Windows.Forms.Panel();
            this.pnlBinMapPath.SuspendLayout();
            this.SuspendLayout();
            // 
            // pathUIComponent1
            // 
            this.pathUIComponent1.BackColor = System.Drawing.Color.Transparent;
            this.pathUIComponent1.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pathUIComponent1.IsChkEnableEditable = true;
            this.pathUIComponent1.IsEnableCheckEn = true;
            this.pathUIComponent1.IsShowType = true;
            this.pathUIComponent1.Location = new System.Drawing.Point(0, 3);
            this.pathUIComponent1.Name = "pathUIComponent1";
            pathInfo1.EnablePath = true;
            pathInfo1.FileExt = "csv";
            pathInfo1.FolderType = MPI.Tester.GuiComponent.ETesterResultCreatFolderType.None;
            pathInfo1.PathName = "Binマップ保存位置";
            pathInfo1.TestResultPath = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            this.pathUIComponent1.PathInfomation = pathInfo1;
            this.pathUIComponent1.PathName = "Binマップ保存位置";
            this.pathUIComponent1.Size = new System.Drawing.Size(700, 24);
            this.pathUIComponent1.TabIndex = 0;
            // 
            // pnlBinMapPath
            // 
            this.pnlBinMapPath.Controls.Add(this.pathUIComponent1);
            this.pnlBinMapPath.Location = new System.Drawing.Point(12, 12);
            this.pnlBinMapPath.Name = "pnlBinMapPath";
            this.pnlBinMapPath.Size = new System.Drawing.Size(700, 294);
            this.pnlBinMapPath.TabIndex = 1;
            // 
            // frmDowaPath
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.ClientSize = new System.Drawing.Size(734, 318);
            this.Controls.Add(this.pnlBinMapPath);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmDowaPath";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "frmWAVETEK00Path";
            this.pnlBinMapPath.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private GuiComponent.PathUIComponent pathUIComponent1;
        private System.Windows.Forms.Panel pnlBinMapPath;

    }
}
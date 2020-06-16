namespace MPI.Tester.GuiComponent
{
    partial class UnitA
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

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.cmbForceValueUnit = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.dinForceValue = new DevComponents.Editors.DoubleInput();
            ((System.ComponentModel.ISupportInitialize)(this.dinForceValue)).BeginInit();
            this.SuspendLayout();
            // 
            // cmbForceValueUnit
            // 
            this.cmbForceValueUnit.DisplayMember = "Text";
            this.cmbForceValueUnit.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbForceValueUnit.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbForceValueUnit.FormattingEnabled = true;
            this.cmbForceValueUnit.ItemHeight = 19;
            this.cmbForceValueUnit.Location = new System.Drawing.Point(132, 3);
            this.cmbForceValueUnit.Name = "cmbForceValueUnit";
            this.cmbForceValueUnit.Size = new System.Drawing.Size(52, 25);
            this.cmbForceValueUnit.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.cmbForceValueUnit.TabIndex = 2;
            this.cmbForceValueUnit.Text = "mA";
            this.cmbForceValueUnit.SelectedIndexChanged += new System.EventHandler(this.cmbForceValueUnit_SelectedIndexChanged);
            // 
            // dinForceValue
            // 
            // 
            // 
            // 
            this.dinForceValue.BackgroundStyle.Class = "DateTimeInputBackground";
            this.dinForceValue.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.dinForceValue.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.dinForceValue.DisplayFormat = "0.0000000000";
            this.dinForceValue.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold);
            this.dinForceValue.Increment = 1D;
            this.dinForceValue.InputHorizontalAlignment = DevComponents.Editors.eHorizontalAlignment.Left;
            this.dinForceValue.Location = new System.Drawing.Point(3, 3);
            this.dinForceValue.MaxValue = 10000D;
            this.dinForceValue.MinValue = 0D;
            this.dinForceValue.Name = "dinForceValue";
            this.dinForceValue.ShowUpDown = true;
            this.dinForceValue.Size = new System.Drawing.Size(123, 25);
            this.dinForceValue.TabIndex = 100;
            this.dinForceValue.Value = 0.01D;
            this.dinForceValue.ValueChanged += new System.EventHandler(this.dinForceValue_ValueChanged);
            // 
            // UnitA
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.Controls.Add(this.dinForceValue);
            this.Controls.Add(this.cmbForceValueUnit);
            this.Name = "UnitA";
            this.Size = new System.Drawing.Size(189, 32);
            ((System.ComponentModel.ISupportInitialize)(this.dinForceValue)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.Editors.DoubleInput dinForceValue;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cmbForceValueUnit;
    }
}

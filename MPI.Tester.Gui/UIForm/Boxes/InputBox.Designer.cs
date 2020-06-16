using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace MPI.Test.Gui
{
	/// <summary>
	/// InputBox class.
	/// </summary>
	public partial class InputBox
	{
		private System.ComponentModel.IContainer components;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
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
			this.grpInput = new DevComponents.DotNetBar.Controls.GroupPanel();
			this.txtInput = new DevComponents.DotNetBar.Controls.TextBoxX();
			this.btnCancel_I18N = new DevComponents.DotNetBar.ButtonX();
			this.btnOK_I18N = new DevComponents.DotNetBar.ButtonX();
			this.labPrompt = new DevComponents.DotNetBar.LabelX();
			this.grpInput.SuspendLayout();
			this.SuspendLayout();
			// 
			// grpInput
			// 
			this.grpInput.BackColor = System.Drawing.Color.Transparent;
			this.grpInput.CanvasColor = System.Drawing.SystemColors.Control;
			this.grpInput.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
			this.grpInput.Controls.Add(this.txtInput);
			this.grpInput.Controls.Add(this.btnCancel_I18N);
			this.grpInput.Controls.Add(this.btnOK_I18N);
			this.grpInput.Controls.Add(this.labPrompt);
			this.grpInput.Dock = System.Windows.Forms.DockStyle.Fill;
			this.grpInput.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.grpInput.IsShadowEnabled = true;
			this.grpInput.Location = new System.Drawing.Point(5, 1);
			this.grpInput.Name = "grpInput";
			this.grpInput.Size = new System.Drawing.Size(396, 146);
			// 
			// 
			// 
			this.grpInput.Style.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
			this.grpInput.Style.BackColorGradientAngle = 90;
			this.grpInput.Style.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
			this.grpInput.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
			this.grpInput.Style.BorderBottomWidth = 1;
			this.grpInput.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
			this.grpInput.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
			this.grpInput.Style.BorderLeftWidth = 1;
			this.grpInput.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
			this.grpInput.Style.BorderRightWidth = 1;
			this.grpInput.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
			this.grpInput.Style.BorderTopWidth = 1;
			this.grpInput.Style.Class = "";
			this.grpInput.Style.CornerDiameter = 4;
			this.grpInput.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded;
			this.grpInput.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center;
			this.grpInput.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
			this.grpInput.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near;
			// 
			// 
			// 
			this.grpInput.StyleMouseDown.Class = "";
			this.grpInput.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square;
			// 
			// 
			// 
			this.grpInput.StyleMouseOver.Class = "";
			this.grpInput.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square;
			this.grpInput.TabIndex = 86;
			this.grpInput.Text = "Input";
			// 
			// txtInput
			// 
			// 
			// 
			// 
			this.txtInput.Border.Class = "";
			this.txtInput.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
			this.txtInput.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtInput.Location = new System.Drawing.Point(24, 39);
			this.txtInput.Name = "txtInput";
			this.txtInput.Size = new System.Drawing.Size(364, 22);
			this.txtInput.TabIndex = 75;
			// 
			// btnCancel_I18N
			// 
			this.btnCancel_I18N.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
			this.btnCancel_I18N.BackColor = System.Drawing.Color.CornflowerBlue;
			this.btnCancel_I18N.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel_I18N.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnCancel_I18N.ForeColor = System.Drawing.Color.Black;
			this.btnCancel_I18N.Location = new System.Drawing.Point(300, 70);
			this.btnCancel_I18N.Name = "btnCancel_I18N";
			this.btnCancel_I18N.Size = new System.Drawing.Size(88, 38);
			this.btnCancel_I18N.TabIndex = 77;
			this.btnCancel_I18N.Text = "Cancel";
			this.btnCancel_I18N.Click += new System.EventHandler(this.btnCancel_I18N_Click);
			// 
			// btnOK_I18N
			// 
			this.btnOK_I18N.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
			this.btnOK_I18N.BackColor = System.Drawing.Color.CornflowerBlue;
			this.btnOK_I18N.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnOK_I18N.ForeColor = System.Drawing.Color.Black;
			this.btnOK_I18N.Location = new System.Drawing.Point(206, 70);
			this.btnOK_I18N.Name = "btnOK_I18N";
			this.btnOK_I18N.Size = new System.Drawing.Size(88, 38);
			this.btnOK_I18N.TabIndex = 76;
			this.btnOK_I18N.Text = "OK";
			this.btnOK_I18N.Click += new System.EventHandler(this.btnOK_I18N_Click);
			// 
			// labPrompt
			// 
			this.labPrompt.BackColor = System.Drawing.Color.LightSteelBlue;
			// 
			// 
			// 
			this.labPrompt.BackgroundStyle.Class = "";
			this.labPrompt.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
			this.labPrompt.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labPrompt.Location = new System.Drawing.Point(24, 15);
			this.labPrompt.Name = "labPrompt";
			this.labPrompt.Size = new System.Drawing.Size(364, 22);
			this.labPrompt.TabIndex = 72;
			this.labPrompt.Text = "Prompt";
			// 
			// InputBox
			// 
			this.AcceptButton = this.btnOK_I18N;
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.CancelButton = this.btnCancel_I18N;
			this.ClientSize = new System.Drawing.Size(406, 149);
			this.ControlBox = false;
			this.Controls.Add(this.grpInput);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "InputBox";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Input Box";
			this.TopMost = true;
			this.Load += new System.EventHandler(this.InputBox_Load);
			this.Activated += new System.EventHandler(this.InputBox_Activated);
			this.grpInput.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private DevComponents.DotNetBar.ButtonX btnOK_I18N;
		private DevComponents.DotNetBar.ButtonX btnCancel_I18N;
		private DevComponents.DotNetBar.LabelX labPrompt;
		private DevComponents.DotNetBar.Controls.GroupPanel grpInput;
		private DevComponents.DotNetBar.Controls.TextBoxX txtInput;
	}
}

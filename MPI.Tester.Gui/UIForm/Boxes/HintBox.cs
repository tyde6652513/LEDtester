using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using DevComponents.DotNetBar;

namespace MPI.Tester.Gui
{
	/// <summary>
	/// HintBox class.
	/// </summary>
	public class HintBox : DevComponents.DotNetBar.Office2007RibbonForm
	{
		// private member
		private DevComponents.DotNetBar.LabelX labPrompt;
		private DevComponents.DotNetBar.Controls.GroupPanel grpInput;

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public HintBox()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

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
			this.labPrompt = new DevComponents.DotNetBar.LabelX();
			this.SuspendLayout();
			// 
			// grpInput
			// 
			this.grpInput.BackColor = System.Drawing.Color.WhiteSmoke;
			this.grpInput.CanvasColor = System.Drawing.SystemColors.Control;
			this.grpInput.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
			this.grpInput.Controls.Add(this.labPrompt);
			this.grpInput.Dock = System.Windows.Forms.DockStyle.Fill;
			this.grpInput.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.grpInput.Location = new System.Drawing.Point(5, 1);
			this.grpInput.Name = "grpInput";
			this.grpInput.Size = new System.Drawing.Size(396, 93);
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
			this.grpInput.Style.CornerType = DevComponents.DotNetBar.eCornerType.Square;
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
			this.grpInput.Text = "Hint";
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
			this.labPrompt.Location = new System.Drawing.Point(20, 3);
			this.labPrompt.Name = "labPrompt";
			this.labPrompt.Size = new System.Drawing.Size(364, 25);
			this.labPrompt.TabIndex = 72;
			this.labPrompt.Text = "Prompt";
			// 
			// HintBox
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 15);
			this.BottomLeftCornerSize = 0;
			this.BottomRightCornerSize = 0;
			this.ClientSize = new System.Drawing.Size(406, 96);
			this.ControlBox = false;
			this.Controls.Add(this.grpInput);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "HintBox";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Hint Box";
			this.TopLeftCornerSize = 0;
			this.TopRightCornerSize = 0;
			this.ResumeLayout(false);

		}
		#endregion

		#region >>> Public Property <<<

		/// <summary>
		/// Prompt.
		/// </summary>
		public string Prompt
		{
			get { return this.labPrompt.Text; }
			set { this.labPrompt.Text = value; }
		}

		/// <summary>
		/// Title.
		/// </summary>
		public string Title
		{
			get { return this.Text; }
			set { this.Text = value; }
		}

		#endregion

		/// <summary>
		/// Display the form to user.
		/// </summary>
		public void Show( string prompt )
		{
			this.Prompt = prompt;
			this.Show();
		}
	}
}

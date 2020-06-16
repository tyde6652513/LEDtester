using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using MPI.Tester.Data;
using MPI.Windows.Forms;


namespace MPI.Tester.Gui
{
	public partial class frmWaferMapSetting : Form
	{
		private Dictionary<string, string> _dicBinItemNameDisp;
		private List<string> _enableItem;
		private BinGradeColor[] _binGradeColorAry;
		private Label[] _lblColAry;
		private Label[] _lblValAry;
		private Control _nowCtl;

		public frmWaferMapSetting()
		{
			InitializeComponent();
		}

		#region >>> Private Method <<<

		private void UpdateListView()
		{
			this.lstEnableItem.Items.Clear();

			this.lstMsrtItem.Items.Clear();

			//Update Enable Item to Listview		
			for (int i = 0; i < this._enableItem.Count; i++)
			{
				if (this._dicBinItemNameDisp.ContainsKey(this._enableItem[i]))
				{
                    this.lstEnableItem.Items.Add(this._dicBinItemNameDisp[this._enableItem[i]]);
				}
				else
				{
					this._enableItem.RemoveAt(i);

					i--;
				}
			}

			//Update Measurement Item to Listview
			foreach (var Item in this._dicBinItemNameDisp)
			{
				if (!this._enableItem.Contains(Item.Key))
				{
					this.lstMsrtItem.Items.Add(Item.Value);
				}
			}
		}

		private void ChangeBinGradeColor(BinGradeColor binGC)
		{
			float fltMinVal = float.Parse(txtMinVal.Text);

			float fltMaxVal = float.Parse(txtMaxVal.Text);

			float fltStepVal = float.Parse(txtStepVal.Text);

			float value = 0;

			Color[] colorAry = new Color[32];

			colorAry = binGC.GetColorItems();

			this.lblMinColor.Text = "<" + fltMinVal.ToString(binGC.DisplayFormat);

			for (int i = 0; i < this._lblColAry.Length; i++)
			{
				value = fltMinVal + (i * fltStepVal);

				if (value >= fltMaxVal)
				{
					value = fltMaxVal;
				}

				this._lblColAry[i].BackColor = colorAry[i];

				this._lblValAry[i].Text = value.ToString(binGC.DisplayFormat);

				this._lblValAry[i].Location = new System.Drawing.Point(this._lblColAry[i].Location.X - this._lblValAry[i].Width, this._lblColAry[i].Location.Y);
			}

			this.txtMinColor.BackColor = binGC.ColorMin;

			this.txtMaxColor.BackColor = binGC.ColorMax;

			this.lblMaxColor.Text = ">" + (value).ToString(binGC.DisplayFormat);
		}

		#endregion

		#region >>> UI Event Handler <<<

		private void frmWaferMapSetting_Load(object sender, EventArgs e)
		{
			DataCenter.LoadTaskSheet(DataCenter._uiSetting.TaskSheetFileName);

			this._dicBinItemNameDisp = new Dictionary<string, string>();

			this._enableItem = new List<string>();

			//////////////////////////////////////////////////////////
			// Initialize tabpEnableItem Componen
			//////////////////////////////////////////////////////////
			foreach (var item in DataCenter._mapData.WeferMapShowItem)
			{
				this._enableItem.Add(item as string);
			}

			//Get Measurement Item
			TestResultData[] testResults = DataCenter._conditionCtrl.MsrtResultArray;

            if (testResults == null || testResults.Length == 0)
            {
                this.gplMertItem.Enabled = false;

                this.gplSelectedItemColors.Enabled = false;

                this.gplDivideSetting.Enabled = false;

                this.gplColorSetting.Enabled = false;

                this.gplOtherSetting.Enabled = false;

                this.btnEnableItemSave.Enabled = false;

                this.btnColorSettingSave.Enabled = false;

                return;
            }

			this._dicBinItemNameDisp.Add(ESysResultItem.BIN.ToString(), ESysResultItem.BIN.ToString());

            foreach (TestResultData data in testResults)
            {
                if (data.IsVision == false || data.IsEnable == false)
                    continue;

                if (data.KeyName.Contains("LOP"))
                {
                    this._dicBinItemNameDisp.Add(data.KeyName, data.Name + "(mcd)");
                }
                else if (data.KeyName.Contains("WATT"))
                {
                    this._dicBinItemNameDisp.Add(data.KeyName, data.Name + "(mW)");
                }
                else if (data.KeyName.Contains("LM"))
                {
                    this._dicBinItemNameDisp.Add(data.KeyName, data.Name + "(lm)");
                }
                else
                {
                    this._dicBinItemNameDisp.Add(data.KeyName, data.Name);
                }
            }

			this.UpdateListView();

			//////////////////////////////////////////////////////////
			// Initialize tabpColorSetting Componen
			//////////////////////////////////////////////////////////
			if (_dicBinItemNameDisp == null || _dicBinItemNameDisp.Count == 0)
			{
				this.gplMertItem.Enabled = false;

				this.gplSelectedItemColors.Enabled = false;

				this.gplDivideSetting.Enabled = false;

				this.gplColorSetting.Enabled = false;

				this.gplOtherSetting.Enabled = false;

				this.btnEnableItemSave.Enabled = false;

                this.btnColorSettingSave.Enabled = false;

				return;
			}

			this._binGradeColorAry = new BinGradeColor[_dicBinItemNameDisp.Count];

			this.chkBoxOutOfRange.Checked = BinGradeColorSet.UseNGColor;

			//InstanceBinGradeColor
			this.lstColorSetMsrtItem.Items.Clear();

			int j = 0;

			foreach (KeyValuePair<string, string> item in _dicBinItemNameDisp)
			{
				this._binGradeColorAry[j] = BinGradeColorSet.GetColorItem(item.Key);

                this.lstColorSetMsrtItem.Items.Add(item.Value);

				j++;
			}

            if (this._binGradeColorAry[0] != null)
            {
                this.txtMaxVal.Text = this._binGradeColorAry[0].Max.ToString();

                this.txtMinVal.Text = this._binGradeColorAry[0].Min.ToString();

                this.txtStepVal.Text = this._binGradeColorAry[0].Step.ToString();
            }
			//draw component
			this._lblColAry = new Label[32];

			this._lblValAry = new Label[32];

			int intX = 75, intY = 0;

			for (int i = 0; i < this._lblColAry.Length; i++)
			{
				this._lblColAry[i] = new Label();

				this._lblValAry[i] = new Label();

				this.Controls.Add(this._lblColAry[i]);

				this.Controls.Add(this._lblValAry[i]);

				this.gplSelectedItemColors.Controls.Add(this._lblColAry[i]);

				this.gplSelectedItemColors.Controls.Add(this._lblValAry[i]);

				this._lblColAry[i].BorderStyle = System.Windows.Forms.BorderStyle.None;

				this._lblColAry[i].Font = new System.Drawing.Font("Tahoma", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
				
				this._lblColAry[i].Location = new System.Drawing.Point(intX, intY += 28);

				this._lblColAry[i].Size = new System.Drawing.Size(15, 15);

				this._lblColAry[i].Click += new EventHandler(this.OnClickChangeColor);

				this._lblColAry[i].BackColorChanged += new EventHandler(onBackColorChange);

				this._lblValAry[i].Location = new System.Drawing.Point(intX - 50, intY);

				this._lblValAry[i].Font = new System.Drawing.Font("Tahoma", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
				
				this._lblValAry[i].Size = new System.Drawing.Size(40, 15);
				
				this._lblValAry[i].AutoSize = true;

				if (i == 15)
				{
					intX += 120;

					intY = 1;
				}
			}

			//InitListBoxItem
			lstColorSetMsrtItem.SelectedIndex = 0;

			//Stanley
			this.txtMapBackColor.BackColor = ColorSettingData.ParseColor(DataCenter._mapData.MapBackColor);
		}

		private void frmWaferMapSetting_FormClosing(object sender, FormClosingEventArgs e)
		{
			DataCenter.loadMapGradeColor();

			FormAgent.TestResultForm.ApplyBinColor();

			Host.FireTestItemChange();
		}

		private void btnAdd_Click(object sender, EventArgs e)
		{
			if (this.lstMsrtItem.SelectedItems.Count < 0)
			{
				return;
			}

			List<string> name = new List<string>(); 

			foreach(var index in this.lstMsrtItem.SelectedIndices)
			{
				name.Add(this.lstMsrtItem.Items[(int)index] as string);
			}

			foreach (var selectedItem in name)
			{
				foreach (var item in this._dicBinItemNameDisp)
				{
					if (selectedItem == item.Value)
					{
						this._enableItem.Add(item.Key);

						break;
					}
				}
			}

			this.UpdateListView();
		}

		private void btnDelete_Click(object sender, EventArgs e)
		{
			if (this.lstEnableItem.SelectedItems.Count < 0)
			{
				return;
			}

			List<string> name = new List<string>();

			foreach (var index in this.lstEnableItem.SelectedIndices)
			{
				name.Add(this.lstEnableItem.Items[(int)index] as string);
			}

			foreach (var selectedItem in name)
			{
				foreach (var item in this._dicBinItemNameDisp)
				{
					if (selectedItem == item.Value)
					{
						if (this._enableItem.Contains(item.Key))
						{
							this._enableItem.Remove(item.Key);

							break;
						}
					}
				}
			}

			this.UpdateListView();
		}

		private void btnSave_Click(object sender, EventArgs e)
		{
			Console.WriteLine("[frmWaferMapSetting], btnSave_Click()");

			//////////////////////////////////////////////////////////
			// Save tabpEnableItem Data
			//////////////////////////////////////////////////////////
			DataCenter._mapData.WeferMapShowItem = this._enableItem;

			//////////////////////////////////////////////////////////
			// Save tabpColorSetting Data
			//////////////////////////////////////////////////////////
			BinGradeColor bgc = this._binGradeColorAry[this.lstColorSetMsrtItem.SelectedIndex];

			if(bgc==null)
			{
			  return;
			}


			bgc.Min = float.Parse(this.txtMinVal.Text);

			bgc.Max = float.Parse(this.txtMaxVal.Text);

			bgc.Step = float.Parse(this.txtStepVal.Text);

			bgc.ColorMax = this.txtMaxColor.BackColor;

			bgc.ColorMin = this.txtMinColor.BackColor;

			BinGradeColorSet.UseNGColor = this.chkBoxOutOfRange.Checked;

			BinGradeColorSet.Save();

			DataCenter._mapData.MapBackColor = ColorSettingData.ParseColor(this.txtMapBackColor.BackColor);
		}

		private void OnClickChangeColor(object sender, EventArgs e)
		{
			Label lblCtrl = sender as Label;

			this._nowCtl = sender as Control;

			this.lblCurrentColor.BackColor = this._nowCtl.BackColor;

			try
			{
				foreach (Label lbl in this._lblColAry)
				{
					lbl.BorderStyle = BorderStyle.None;
				}

				lblCtrl.BorderStyle = BorderStyle.FixedSingle;
			}
			catch
			{

			}
		}

		private void onBackColorChange(object sender, EventArgs e)
		{
			//SetColorItems
			Color[] colorAry = new Color[32];

			int i = 0;

			foreach (Control ctl in this._lblColAry)
			{
				colorAry[i] = ctl.BackColor;

				i++;
			}

            if (this._binGradeColorAry[lstColorSetMsrtItem.SelectedIndex] != null)
            {
               this._binGradeColorAry[lstColorSetMsrtItem.SelectedIndex].SetColorItems(colorAry);
            }
		}

		private void btnDivide_Click(object sender, EventArgs e)
		{
			Console.WriteLine("[frmWaferMapSetting], btnDivide_Click()");

			this.ChangeBinGradeColor(this._binGradeColorAry[lstColorSetMsrtItem.SelectedIndex]);
		}

		private void lstColorSetMsrtItem_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this.lstColorSetMsrtItem.SelectedIndex >= 0)
			{
                if (this._binGradeColorAry[this.lstColorSetMsrtItem.SelectedIndex] != null)
                {
                    this.txtMaxVal.Text = this._binGradeColorAry[this.lstColorSetMsrtItem.SelectedIndex].Max.ToString();

                    this.txtMinVal.Text = this._binGradeColorAry[this.lstColorSetMsrtItem.SelectedIndex].Min.ToString();

                    this.txtStepVal.Text = this._binGradeColorAry[this.lstColorSetMsrtItem.SelectedIndex].Step.ToString();

                    this.gplSelectedItemColors.Text = lstColorSetMsrtItem.SelectedItem.ToString();

                    this.ChangeBinGradeColor(this._binGradeColorAry[this.lstColorSetMsrtItem.SelectedIndex]);
                }
			}
		}

		private void chkBoxOutOfRange_CheckedChanged(object sender, EventArgs e)
		{
			BinGradeColorSet.UseNGColor = this.chkBoxOutOfRange.Checked;

			if (this.chkBoxOutOfRange.Checked)
			{
				this.lblMinColor.Enabled = true;

				this.lblMaxColor.Enabled = true;

				this.txtMinColor.Enabled = true;

				this.txtMaxColor.Enabled = true;
			}
			else
			{
				this.lblMinColor.Enabled = false;

				this.lblMaxColor.Enabled = false;

				this.txtMinColor.Enabled = false;

				this.txtMaxColor.Enabled = false;
			}
		}

		private void btnDefaultColor_Click(object sender, EventArgs e)
		{
			Console.WriteLine("[frmWaferMapSetting], btnDefaultColor_Click()");

			BinGradeColor binGC = this._binGradeColorAry[lstColorSetMsrtItem.SelectedIndex];

			Color[] colorAry = new Color[32];

			int R = 256;
			int G = 0;
			int B = 0;

			for (int i = 0; i < colorAry.Length; i++)
			{
				if (i > 0 && i <= 8)
				{
					G += 32;
				}
				else if (i > 9 && i <= 16)
				{
					R -= 32;
				}
				else if (i > 16 && i <= 24)
				{
					B += 32;
				}
				else if (i > 24 && i <= 32)
				{
					G -= 32;
				}

				colorAry[i] = Color.FromArgb((R - 1) > 0 ? (R - 1) : R,
											(G - 1) > 0 ? (G - 1) : G,
											(B - 1) > 0 ? (B - 1) : B
											);
			}

            Color[] newcolorAry = new Color[32];


            for (int i = 0; i < colorAry.Length; i++)
            {
                newcolorAry[i] = colorAry[colorAry.Length - 1-i];
            }

            binGC.SetColorItems(newcolorAry);

			this.btnDivide.PerformClick();
		}

		private void slider_Click(object sender, EventArgs e)
		{
			this.lblCurrentColor.BackColor = Color.FromArgb(this.sliderR.Value, this.sliderG.Value, this.sliderB.Value);
		}

		private void cpbAdvSettingColor_SelectedColorChanged(object sender, EventArgs e)
		{
			this.lblCurrentColor.BackColor = this.cpbAdvSettingColor.SelectedColor;
		}

		private void btnSettingColor_Click(object sender, EventArgs e)
		{
			if (this._nowCtl != null)
			{
				this._nowCtl.BackColor = this.lblCurrentColor.BackColor;
			}
		}

		private void lblCurrentColor_BackColorChanged(object sender, EventArgs e)
		{
			this.sliderR.Value = this.lblCurrentColor.BackColor.R;

			this.sliderG.Value = this.lblCurrentColor.BackColor.G;

			this.sliderB.Value = this.lblCurrentColor.BackColor.B;

			this.lblRValue.Text = this.sliderR.Value.ToString();

			this.lblGValue.Text = this.sliderG.Value.ToString();

			this.lblBValue.Text = this.sliderB.Value.ToString();

			this.cpbAdvSettingColor.SelectedColor = this.lblCurrentColor.BackColor;
		}

		private void btnClose_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		#endregion
	}
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using MPI.Tester.Data;

namespace MPI.Windows.Forms
{
    public partial class frmBinColorSet : Form
    {
		BinGradeColor[] binGradeColorAry;
		Label[] lblColAry;
		Label[] lblValAry;
		Dictionary<string, string> dtList;
		Control _NowCtl;
		string _mapBackColor;

		public frmBinColorSet(string DispMapBackColor)
		{
			InitializeComponent();

			InitBinGradeColorSet();

			this.txtMapBackColor.BackColor = ColorSettingData.ParseColor(DispMapBackColor);
			this._mapBackColor = DispMapBackColor;
		}

		private void InitBinGradeColorSet()
		{
			dtList = BinGradeColorSet.GetNameList();

			if (dtList == null || dtList.Count == 0)
			{
				this.groupBox1.Enabled = false;
				this.groupBox2.Enabled = false;
				this.groupBox3.Enabled = false;
				this.groupBox4.Enabled = false;
				this.btnSave.Enabled = false;
				return;
			}

			binGradeColorAry = new BinGradeColor[dtList.Count];
			chkBoxOutOfRange.Checked = BinGradeColorSet.UseNGColor;

			InstanceBinGradeColor();
			InitFirsItem();
			InitListBoxItem();
		}

		private void InstanceBinGradeColor()
		{
			int i = 0;

			foreach ( KeyValuePair<string, string> item in dtList )
			{
				binGradeColorAry[i] = BinGradeColorSet.GetColorItem( item.Key );
				i++;
			}
		}

		private void InitListBoxItem()
		{
			listboxItem.SelectedIndex = 0;
		}

		private void InitFirsItem()
		{
			foreach ( KeyValuePair<string, string> item in dtList )
			{
				listboxItem.Items.Add( item.Value );
			}

			txtMaxVal.Text = binGradeColorAry[0].Max.ToString();
			txtMinVal.Text = binGradeColorAry[0].Min.ToString();
			txtStepVal.Text = binGradeColorAry[0].Step.ToString();

			//draw component
			newColorCtlAry( binGradeColorAry[0] );
		}

		private void btnAdd_Click( object sender, EventArgs e )
		{
			if ( cobItem.SelectedIndex >= 0 )
			{
				if ( !listboxItem.Items.Contains( cobItem.SelectedItem ) )
				{
					listboxItem.Items.Add( cobItem.SelectedItem );
				}
			}
		}

		private void btnDelete_Click( object sender, EventArgs e )
		{
			if ( listboxItem.SelectedIndex >= 0 )
			{
				if ( listboxItem.Items.Count == 1 )
				{
					MessageBox.Show( "至少需有一個晶圓圖項目!", "晶圓圖項目設定" );
				}
				else
				{
					listboxItem.Items.Remove( listboxItem.SelectedItem );
				}
			}
		}

		private void listboxItem_SelectedIndexChanged( object sender, EventArgs e )
		{
			if ( listboxItem.SelectedIndex >= 0 )
			{
				groupBox2.Text = listboxItem.SelectedItem.ToString();

				txtMaxVal.Text = binGradeColorAry[listboxItem.SelectedIndex].Max.ToString();
				txtMinVal.Text = binGradeColorAry[listboxItem.SelectedIndex].Min.ToString();
				txtStepVal.Text = binGradeColorAry[listboxItem.SelectedIndex].Step.ToString();

				changeBinGradeColor( binGradeColorAry[listboxItem.SelectedIndex] );
			}
		}

		private void btnSet_Click( object sender, EventArgs e )
		{
			changeBinGradeColor( binGradeColorAry[listboxItem.SelectedIndex] );
		}

		private void saveBinGradeColor()
		{
			BinGradeColor bgc = binGradeColorAry[listboxItem.SelectedIndex];

			bgc.Min = float.Parse( txtMinVal.Text );
			bgc.Max = float.Parse( txtMaxVal.Text );
			bgc.Step = float.Parse( txtStepVal.Text );
			bgc.ColorMax = txtMaxColor.BackColor;
			bgc.ColorMin = txtMinColor.BackColor;

			BinGradeColorSet.UseNGColor = chkBoxOutOfRange.Checked;

			BinGradeColorSet.Save();

			this._mapBackColor = ColorSettingData.ParseColor(this.txtMapBackColor.BackColor);
		}

		public void changeBinGradeColor( BinGradeColor binGC )
		{
			refreshColorAry( binGC );
		}

		private void newColorCtlAry( BinGradeColor binGC )
		{
			lblColAry = new Label[32];
			lblValAry = new Label[32];

			int intX = 75, intY = 0;

			for ( int i = 0; i < lblColAry.Length; i++ )
			{
				lblColAry[i] = new Label();
				lblValAry[i] = new Label();

				this.Controls.Add( lblColAry[i] );
				this.Controls.Add( lblValAry[i] );
				this.groupBox2.Controls.Add( lblColAry[i] );
				this.groupBox2.Controls.Add( lblValAry[i] );

				lblColAry[i].BorderStyle = System.Windows.Forms.BorderStyle.None;
				lblColAry[i].Font = new System.Drawing.Font( "Tahoma", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ( ( byte ) ( 136 ) ) );
				lblColAry[i].Location = new System.Drawing.Point( intX, intY += 23 );
				lblColAry[i].Size = new System.Drawing.Size( 15, 15 );
				lblColAry[i].Click += new EventHandler( onClickChangeColor );
				lblColAry[i].BackColorChanged += new EventHandler(onBackColorChange);

				lblValAry[i].Location = new System.Drawing.Point( intX - 50, intY );
				lblValAry[i].Font = new System.Drawing.Font( "Tahoma", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ( ( byte ) ( 136 ) ) );
				lblValAry[i].Size = new System.Drawing.Size( 40, 15 );
				lblValAry[i].AutoSize = true;

				if ( i == 15 )
				{
					intX += 120;
					intY = 1;
				}
			}
		}        

		private void refreshColorAry( BinGradeColor binGC )
		{
			float fltMinVal = float.Parse(txtMinVal.Text);
			float fltMaxVal = float.Parse(txtMaxVal.Text);
			float fltStepVal = float.Parse(txtStepVal.Text);
			float value = 0;

			Color[] colorAry = new Color[32];

			colorAry = binGC.GetColorItems();

			this.label4.Text = "<" + fltMinVal.ToString(binGC.DisplayFormat);

			for ( int i = 0; i < lblColAry.Length; i++ )
			{
				value = fltMinVal + (i * fltStepVal);
				if (value >= fltMaxVal)
					value = fltMaxVal;

				lblColAry[i].BackColor = colorAry[i];
				lblValAry[i].Text = value.ToString(binGC.DisplayFormat);
				lblValAry[i].Location = new System.Drawing.Point(lblColAry[i].Location.X - lblValAry[i].Width, lblColAry[i].Location.Y);
			}

			txtMinColor.BackColor = binGC.ColorMin;
			txtMaxColor.BackColor = binGC.ColorMax;

			this.label5.Text = ">" + (value).ToString(binGC.DisplayFormat);

		}

		private void deleteDynCtl()
		{
			foreach ( Control ctl in lblColAry )
			{
				ctl.Dispose();
			}

			foreach ( Control ctl in lblValAry )
			{
				ctl.Dispose();
			}
		}

		private void txtMinColor_Click( object sender, EventArgs e )
		{
			onClickChangeColor( txtMinColor as object, e );
		}

		private void txtMaxColor_Click( object sender, EventArgs e )
		{
			onClickChangeColor( txtMaxColor as object, e );
		}

		private void txtMapBackColor_Click(object sender, EventArgs e)
		{
			onClickChangeColor(txtMapBackColor as object, e);
		}

		private void onClickChangeColor( object sender, EventArgs e )
		{
			Label lblCtrl = sender as Label;
			this._NowCtl = sender as Control;
			this.lblCurrentColor.BackColor = this._NowCtl.BackColor;

			try
			{
				foreach (Label lbl in lblColAry)
				{
					lbl.BorderStyle = BorderStyle.None;
				}

				lblCtrl.BorderStyle = BorderStyle.FixedSingle;
			}
			catch
			{ }
			//this.sliderR.Value = this._NowCtl.BackColor.R;
			//this.sliderG.Value = this._NowCtl.BackColor.G;
			//this.sliderB.Value = this._NowCtl.BackColor.B;
		}

		private void onBackColorChange( object sender, EventArgs e )
		{
			//SetColorItems
			Color[] colorAry = new Color[32];
			int i = 0;

			foreach ( Control ctl in lblColAry )
			{
				colorAry[i] = ctl.BackColor;
				i++;
			}

			binGradeColorAry[listboxItem.SelectedIndex].SetColorItems( colorAry );
		}

		private void chkBoxOutOfRange_CheckedChanged( object sender, EventArgs e )
		{
			BinGradeColorSet.UseNGColor = chkBoxOutOfRange.Checked;
		}

		private void btnSave_Click( object sender, EventArgs e )
		{
			saveBinGradeColor();
		}

		private void btnExit_Click( object sender, EventArgs e )
		{
			this.Close();
		}

		private void colorPickerButton1_SelectedColorChanged( object sender, EventArgs e )
		{
			this.lblCurrentColor.BackColor = this.colorPickerButton1.SelectedColor;
		}

		private void lblCurrentColor_BackColorChanged( object sender, EventArgs e )
		{
			this.sliderR.Value = this.lblCurrentColor.BackColor.R;
			this.sliderG.Value = this.lblCurrentColor.BackColor.G;
			this.sliderB.Value = this.lblCurrentColor.BackColor.B;

			this.lblRValue.Text = this.sliderR.Value.ToString();
			this.lblGValue.Text = this.sliderG.Value.ToString();
			this.lblBValue.Text = this.sliderB.Value.ToString();

			this.colorPickerButton1.SelectedColor = this.lblCurrentColor.BackColor;
		}

		private void sliderRGB_Click( object sender, EventArgs e )
		{
			this.lblCurrentColor.BackColor = Color.FromArgb( this.sliderR.Value, this.sliderG.Value, this.sliderB.Value );
		}

		private void btnSettingColor_Click( object sender, EventArgs e )
		{
			if ( this._NowCtl != null )
				_NowCtl.BackColor = this.lblCurrentColor.BackColor;
		}

		private void btnDefaultColor_Click( object sender, EventArgs e )
		{
			BinGradeColor binGC = binGradeColorAry[listboxItem.SelectedIndex];
			Color[] colorAry = new Color[32];

			int R = 256;
			int G = 0;
			int B = 0;

			for ( int i = 0; i < colorAry.Length; i++ )
			{
				if ( i > 0 && i <= 8 )
				{
					G += 32;
				}
				else if ( i > 9 && i <= 16 )
				{
					R -= 32;
				}
				else if ( i > 16 && i <= 24 )
				{
					B += 32;
				}
				else if ( i > 24 && i <= 32 )
				{
					G -= 32;
				}

				colorAry[i] = Color.FromArgb( ( R - 1 ) > 0 ? ( R - 1 ) : R,
											( G - 1 ) > 0 ? ( G - 1 ) : G,
											( B - 1 ) > 0 ? ( B - 1 ) : B
											);
			}

			binGC.SetColorItems( colorAry );

			btnSet.PerformClick();
		}

		public string MapBackColor()
		{
			return this._mapBackColor;
		}
    }

}
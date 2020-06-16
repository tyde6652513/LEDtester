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
//using MPI.Tester.Data;
using MPI.UCF.Forms.Domain;
using System.Threading;

namespace MPI.Tester.Gui
{
	public partial class frmPopWaferMap : System.Windows.Forms.Form
	{
		private delegate void WaferMapEventHandler(int row, int col);
		private frmBinColorShow _formStatistics;
		private bool _isShowColor;

		public frmPopWaferMap()
		{
			InitializeComponent();

			this.WaferMap.SetDatabase(frmWaferMap.WaferDB);

			this._isShowColor = false;
		}

		#region >>> private Method <<<
		private int _drawLock;

		public void OnUpdateStatusUI(object sender, ShowMapDataEventArgs e)
		{
			int row = e.Row;
			int col = e.Col;

			//DataCenter.ChangeMapRowColOfTester(ref col, ref row);

			if (Interlocked.Exchange(ref _drawLock, 1) == 0)
			{
				WaferMap.DrawWaferDie(row, col);
				Interlocked.Exchange(ref _drawLock, 0);
			}

			float value = frmWaferMap.WaferDB[row, col, this.WaferMap.SymbolId];
			int index = frmWaferMap.WaferDB.GetIndex(row, col);

			if (float.IsNaN(value) == false)
				lblValue.Text = value.ToString("0.00000");

			lblIndex.Text = index.ToString("000000");
			lblPosX.Text = col.ToString();
			lblPosY.Text = row.ToString();
		}

		private void OnClickUapateStatusUI(int row, int col)
		{
			WaferMap.DrawWaferDie(row, col);

			float value = frmWaferMap.WaferDB[row, col, this.WaferMap.SymbolId];
			int index = frmWaferMap.WaferDB.GetIndex(row, col);

			if (float.IsNaN(value) == false)
				lblValue.Text = value.ToString("0.00000");

			lblIndex.Text = index.ToString("000000");

			//DataCenter.ChangeMapRowColOfTester(ref col, ref row);

			lblPosX.Text = col.ToString();
			lblPosY.Text = row.ToString();
		}

		#endregion

		#region >>> Public Method <<<

        public void SetDataBase()
        {
            this.WaferMap.SetDatabase(frmWaferMap.WaferDB);
        }


		public void Reset()
		{
			this.WaferMap.Start(true);
			this.WaferMap.SetLayout(frmWaferMap.WaferDB.Boundary);
			this.WaferMap.ResetScale();
		}

		public void Reset(int Left, int Top, int right, int bottom)
		{
			this.WaferMap.Start(true);
			this.WaferMap.SetLayout(Rectangle.FromLTRB(Left, Top, right, bottom));
			this.WaferMap.ResetScale();
		}

		public void SetWaferMapSelectable(bool isEnable)
		{
			WaferMap.Selectable = isEnable;
		}

		public void ApplyColorSetting(BinGradeColor bgc)
		{
			if (!this.IsHandleCreated)
				return;

			this.WaferMap.MaxLevelValue = bgc.Max;
			this.WaferMap.MinLevelValue = bgc.Min;

			try
			{
				BinGradeColorSet.Reflash();
				if (this._formStatistics != null)
				{
					this._formStatistics.refreshColorAry(this._formStatistics.GetBinGradeColor(bgc.KeyName));
				}

				this.WaferMap.BackColor = ColorSettingData.ParseColor(DataCenter._mapData.MapBackColor);
				//this.WaferMap.DrawAllWaferDie();
			}
			catch
			{
				return;
			}

			this.Text = "Wafer Map:: " + bgc.Title;
		}

		#endregion

		#region >>> UI Event Handler <<<

		private void btnHide_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void btnSnapshot_Click(object sender, EventArgs e)
		{
			WaferMap.SaveToImage(string.Format(@"C:\WaferMap.{0}.png", this.WaferMap.SymbolId));
		}

		private void frmPopWaferMap_FormClosing(object sender, FormClosingEventArgs e)
		{
            e.Cancel = true;

            //this.WaferMap.Stop();

            //AppSystem.ShowMapDataEvent -= OnUpdateStatusUI;

            //if (Interlocked.Exchange(ref _drawLock, 1) == 0)
            //{
            //    return;
            //}


            //if (e.CloseReason == CloseReason.UserClosing)
            //{
            //    e.Cancel = true;
            //    this.Hide();

            //    new Thread(delegate()
            //    {

            //        while (Interlocked.Exchange(ref _drawLock, 1) == 1)
            //            Thread.Sleep(10);

            //        this.Close();
            //    }).Start();
            //}
		}

		private void btnZoomIn_Click(object sender, EventArgs e)
		{
			if (this.WaferMap.ZoomScale >= 1)
			{
				this.WaferMap.Zoom(this.WaferMap.ZoomScale + 1);
				this.btnZoomOut.Enabled = true;
			}
			else
			{
				this.btnZoomOut.Enabled = true;
			}
		}

		private void btnZoomOut_Click(object sender, EventArgs e)
		{
			this.WaferMap.ResetScale();

			//if (this.WaferMap.ZoomScale >= 2)
			//{
			//    this.WaferMap.Zoom(this.WaferMap.ZoomScale - 1);
			//}

			//if (this.WaferMap.ZoomScale <= 1)
			//{
			//    this.btnZoomOut.Enabled = false;
			//}
			//else
			//{
			//    this.btnZoomOut.Enabled = true;
			//}
		}

		private void frmPopWaferMap_Load(object sender, EventArgs e)
		{
			this.Reset();

			this._formStatistics = new frmBinColorShow();
			this._formStatistics.TopLevel = false;
			this._formStatistics.Top = 0;
			this._formStatistics.Left = this.WaferMap.Width + 1;
			this._formStatistics.Parent = this;
			this.Controls.Add(this._formStatistics);
			this._formStatistics.Show();
			this._formStatistics.Hide();

			if (WaferMap != null)
				this._formStatistics.Start(WaferMap.SymbolId);

			AppSystem.ShowMapDataEvent += OnUpdateStatusUI;

			this.WaferMap.IsSeamless = !DataCenter._machineConfig.Enable.IsEnableMapShowGap;

		}

		private void btnShowColor_Click(object sender, EventArgs e)
		{
			this._isShowColor = !this._isShowColor;

			if (this._isShowColor)
			{
				this.Width = 515;
				this.Height = 480;
				this.btnShowColor.Image = global::MPI.Tester.Gui.Properties.Resources.arrow_left;
				this.btnShowColor.ImageFixedSize = new System.Drawing.Size(15, 30);
				//this.btnShowColor.Location = new System.Drawing.Point(this.WaferMap.Width + this._formStatistics.Width + 5, 150);
                this.BringToFront();

				this._formStatistics.Show();
			}
			else
			{
				this.Width = 320;
				this.Height = 354;
				this.btnShowColor.Image = global::MPI.Tester.Gui.Properties.Resources.arrow_right;
				this.btnShowColor.ImageFixedSize = new System.Drawing.Size(15, 30);
				//this.btnShowColor.Location = new System.Drawing.Point(this.WaferMap.Width + 5, 150);

				this._formStatistics.Hide();
			}
		}

		#endregion

		#region >>> Public Property <<<

		public string WMSymbolId
		{
			get
			{
				return this.WaferMap.SymbolId;
			}

			set
			{
				if (this.WaferMap != null && this.WaferMap.SymbolId != value)
				{
					this.WaferMap.SymbolId = value;
					if (this._formStatistics != null)
						this._formStatistics.Start(value);
				}
			}
		}

		#endregion

        private void frnPopWaferMap_FormClosed(object sender, FormClosedEventArgs e)
        {
            AppSystem.ShowMapDataEvent -= OnUpdateStatusUI;
        }

        public void RemoveEvent()
        {
            AppSystem.ShowMapDataEvent -= OnUpdateStatusUI;
        }


	}
}
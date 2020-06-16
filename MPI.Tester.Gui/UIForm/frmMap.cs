using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using MPI.Windows.Forms;
using MPI.Tester.Data;
using MPI.UCF.Forms.Domain;
using System.Threading;

namespace MPI.Tester.Gui
{
	public partial class frmWaferMap : System.Windows.Forms.Form
	{
		public delegate void WaferMapDieClickEventHandler(int row, int col);
		protected delegate void WaferMapEventHandler( int row, int col );

		#region >>> Sharing Field <<<

		public static int BinCounter;
		public static WaferDatabase WaferDB;
		public static frmBinColorShow _formStatistics;
		public event WaferMapDieClickEventHandler OnWaferMapDieClickEvent;
		protected static WaferMapEventHandler OnWaferMapEvent;

		#endregion

		#region >>> Public Property <<<

		public string WMSymbolId
		{
			get
			{
				return WaferMap.SymbolId;
			}

			set
			{
				if ( WaferMap != null && WaferMap.SymbolId != value )
				{
					WaferMap.SymbolId = value;
					if ( _formStatistics != null )
						_formStatistics.Start( value );
				}
			}
		}

		public bool DynamicMode
		{
			get
			{
				return this.WaferMap.DynamicMode;
			}

			set
			{
				this.WaferMap.DynamicMode = value;
			}


		}

		public bool isEnableShowMap
		{
			get;
			set;
		}

		#endregion

		#region >>> Constructor / Disposor <<<

		public frmWaferMap()
		{
			Console.WriteLine( "[frmWaferMap], frmWaferMap()" );

			InitializeComponent();

			//if (this.GetType() == typeof(frmWaferMap))
			//{
			//    WaferDB = new WaferDatabase();
			//}

			//WaferMap.SetDatabase( WaferDB );

         WaferMap.OwnerDraw = false;
		}

		#endregion

		#region >>> Public Method <<<

		public virtual void Reset()
		{
			WaferDB.Reset();

			BinCounter = 0;
			BinGradeColorSet.ResetCounter();

			this.WaferMap.Start( true );
		}

		public virtual void ApplyColorSetting( BinGradeColor bgc )
		{
			if ( this.Created == false )
				return;

			this.WaferMap.MaxLevelValue = bgc.Max;
			this.WaferMap.MinLevelValue = bgc.Min;

			try
			{
				//this.WaferMap.DrawAllWaferDie();
				BinGradeColorSet.Reflash();
				if ( _formStatistics != null )
				{
					_formStatistics.refreshColorAry( _formStatistics.GetBinGradeColor( bgc.KeyName ) );
				}

				this.WaferMap.BackColor = ColorSettingData.ParseColor( DataCenter._mapData.MapBackColor );
				this.WaferMap.DrawAllWaferDie();
			}
			catch
			{
				return;
			}
		}

		public void WaferMapPrepare()
		{
			if ( this.Created )
				this.prepare();
		}

		public void SaveToImage( string path )
		{
			this.WaferMap.SaveToImage( path );
		}

		public void AddWaferDieFromFile(int row, int col, Dictionary<string, float> result)
		{
			if ( this.Disposing || this.IsDisposed )
				return;

			BinCounter++;

			WaferMap.AddWaferDie(row, col, BinCounter, result);

			BinGradeColorSet.CounterItem(result);
		}

		public void DrawWaferDie()
		{
			WaferMap.DrawWaferDie();
		}

		public void DrawAllWaferDie()
		{
			WaferMap.DrawAllWaferDie();
		}

		public void SetWaferMapBoundary(int left, int top, int right, int bottom, int growthDirection)
		{

            Console.WriteLine("[frmMap], SetWaferMapBoundary()left/top/right/bottom : " + left.ToString() + " , " + top.ToString() + " , " + right.ToString() + " , " + bottom.ToString());
            if (left > right)
            {
                int temp = left;
                left = right;
                right = temp;
            }
            //if (top > bottom)
            //{
            //    int temp = top;
            //    top = bottom;
            //    bottom = temp;
            //}

            if (this.IsHandleCreated)
				WaferMap.SetLayout(Rectangle.FromLTRB(left, top, right, bottom));

			switch ( growthDirection )
			{
				case 0:
				WaferMap.GrowthDirection = EGrowthDirection.Downward;
				break;
				case 1:
				WaferMap.GrowthDirection = EGrowthDirection.Upward;
				break;
				case 2:
				WaferMap.GrowthDirection = EGrowthDirection.Rightward;
				break;
				case 3:
				WaferMap.GrowthDirection = EGrowthDirection.Leftward;
				break;
			}

		}

		public void SetWaferMapSelectable( bool isEnable )
		{
			WaferMap.Selectable = isEnable;
		}

		#endregion

		#region >>> Private Method <<<

		protected virtual void prepare()
		{
			if ( this.GetType() == typeof( frmWaferMap ) )
			{
				WaferDB = new WaferDatabase();
			}

			WaferMap.SetDatabase( WaferDB );

			this.WaferMap.Start( true );

			BinCounter = 0;

			OnWaferMapEvent += this.OnUpdateStatusUI;

			this.Tag = "Main";
		}

		#endregion

		#region >>> Event Handler <<<

		protected void registerMapEvent( bool register )
		{
			if ( register )
				OnWaferMapEvent += this.OnUpdateStatusUI;
			else
				OnWaferMapEvent -= this.OnUpdateStatusUI;
		}

      private int _drawLock;
		protected void OnUpdateStatusUI( int row, int col )
		{
            if (Interlocked.Exchange(ref _drawLock, 1) == 0)
            { 
                WaferMap.DrawWaferDie(row, col);
                Interlocked.Exchange(ref _drawLock, 0);
            }

			float value = WaferDB[row, col, WaferMap.SymbolId];
			if ( float.IsNaN( value ) == false )
				lblValue.Text = value.ToString( "0.00000" );

			lblIndex.Text = BinCounter.ToString( "000000" );

			//DataCenter.ChangeMapRowColOfTester( ref col, ref row );

			lblPosX.Text = col.ToString();
			lblPosY.Text = row.ToString();
		}

		/// <summary>
		/// Map Event From Tester
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public void MsrtProcess_OnMapDataEvent( object sender, ShowMapDataEventArgs e )
		{
			if ( this.Disposing || this.IsDisposed || !this.IsHandleCreated )
				return;

			if ( !this.isEnableShowMap )
				return;

			int row = e.Row;

			int col = e.Col;

			//DataCenter.ChangeMapRowColOfTester( ref col, ref row );

			BinCounter++;

			Dictionary<string, float> data = new Dictionary<string, float>();

			foreach ( var item in e.Values )
			{
				if ( DataCenter._mapData.WeferMapShowItem.Contains( item.Key ) )
				{
					data.Add( item.Key, item.Value );
				}
			}

			WaferMap.AddWaferDie( row, col, BinCounter, data );

			BinGradeColorSet.CounterItem( data );

			if ( this.Created )
			{
				//follow UI BeginInvoke
				this.BeginInvoke( ( WaferMapEventHandler ) OnWaferMapEvent, row, col );
			}
		}

		private void WaferMap_DieClick( object sender, DieLocationEventArgs e )
		{
			int row = e.RowColLocation.Row;
			int col = e.RowColLocation.Col;

			this.OnClickUapateStatusUI( row, col );
		}

		private void OnClickUapateStatusUI( int row, int col )
		{
            WaferMap.DrawWaferDie(row, col);

			float value = WaferDB[row, col, WaferMap.SymbolId];
			int index = WaferDB.GetIndex( row, col );
			if ( float.IsNaN( value ) == false )
				lblValue.Text = value.ToString( "0.00000" );

			lblIndex.Text = index.ToString( "000000" );

			//DataCenter.ChangeMapRowColOfTester( ref col, ref row );

			lblPosX.Text = col.ToString();
			lblPosY.Text = row.ToString();

			if(this.OnWaferMapDieClickEvent != null)
			{
				this.OnWaferMapDieClickEvent(row, col);
			}
		}

		#endregion

		#region >>> Form Event Handler <<<

		private void frmWaferMap_FormClosed( object sender, FormClosedEventArgs e )
		{
			OnWaferMapEvent -= this.OnUpdateStatusUI;
		}

		private void frmWaferMap_FormClosing( object sender, FormClosingEventArgs e )
		{
			if ( this.GetType() == typeof( frmWaferMap ) )
			{
				if ( _formStatistics != null )
					_formStatistics.Stop();
			}

			if ( e.CloseReason == CloseReason.UserClosing )
			{
				this.Hide();
				e.Cancel = true;    // this cancels the close event.
			}
		}

		private void frmWaferMap_Load( object sender, EventArgs e )
		{
			this.prepare();

			if ( this.GetType() != typeof( frmWaferMap ) )
				return;

			if ( _formStatistics == null )
			{
				_formStatistics = new frmBinColorShow();
				_formStatistics.TopLevel = false;
				_formStatistics.Top = -2;
				_formStatistics.Left = ControlPanel.Right;
				_formStatistics.Parent = this;
				this.Controls.Add( _formStatistics );
				_formStatistics.Show();

				if ( WaferMap != null )
					_formStatistics.Start( WaferMap.SymbolId );
			}

			WaferMap.IsSeamless = !DataCenter._machineConfig.Enable.IsEnableMapShowGap;
		}

		#endregion

		#region >>> UI Event Handler <<<

		private void btnZoomIn_Click( object sender, EventArgs e )
		{
			if ( WaferMap.ZoomScale >= 1 )
			{
				this.WaferMap.Zoom( WaferMap.ZoomScale + 1 );
				this.btnZoomOut.Enabled = true;
			}
			else
			{
				this.btnZoomOut.Enabled = true;
			}
		}

		private void btnZoomOut_Click( object sender, EventArgs e )
		{
			this.WaferMap.ResetScale();

			//if (WaferMap.ZoomScale >= 2)
			//{
			//    this.WaferMap.Zoom(WaferMap.ZoomScale - 1);
			//}

			//if (WaferMap.ZoomScale <= 1)
			//{
			//    this.btnZoomOut.Enabled = false;
			//}
			//else
			//{
			//    this.btnZoomOut.Enabled = true;
			//}
		}

		#endregion
	}
}
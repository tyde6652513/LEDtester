using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MPI.Windows.Forms
{
	public partial class frmBinColorShow : Form
	{
		private BinGradeColor[] binGradeColorAry;
		private Dictionary<string, int> dtBGC = new Dictionary<string, int>();
		private Label[] lblColAry;
		private Label[] lblValAry;
		private Label[] lblPcsAry;
		private int intCurrentIndex;
		private Int32[] intCntItemAry = new Int32[32];
		private Int32 intTotalPcs;
		private Timer _updateTimer;
		private bool _isPcsRdoBoxFlag;
		private Dictionary<string, string> dtList;
		
		private string _currentKey;

		public int RefreshInterval
		{
			get
			{
				return _updateTimer.Interval;
			}
			set
			{
				_updateTimer.Interval = value;
			}
		}

		public frmBinColorShow()
		{
			InitializeComponent();

			_updateTimer = new Timer();
			_updateTimer.Interval = 200;
			_updateTimer.Tick += new EventHandler( UpdateTimer_Tick );
			_updateTimer.Enabled = false;
			_currentKey = string.Empty;
			_isPcsRdoBoxFlag = true;
		}

		#region >>> Public Method <<<
		public void Start( string keyName )
		{
			if (!BinGradeColorSet.IsReady)
				return;

			if(this.Created)
			{
				InitBinGradeColorSet();
				InstanceBinGradeColor();
			}

			if ( _currentKey == keyName )
				return;

			if ( dtBGC.ContainsKey( keyName ) == false )
			{
				_currentKey = keyName;
				return;
			}

			_currentKey = keyName;

			_updateTimer.Stop();
			InitFirstItem( keyName );
			_updateTimer.Start();
		}

		public void Stop()
		{
			_updateTimer.Stop();
		}

		public void Continue()
		{
			this.Start( _currentKey );
		}

		public BinGradeColor GetBinGradeColor(string strKeyName)
		{
			this.Start(strKeyName);

			if (!dtBGC.ContainsKey(strKeyName))
				return null;

			int CurrentIndex = dtBGC[strKeyName];

			if(CurrentIndex >= binGradeColorAry.Length)
				return null;

			return binGradeColorAry[CurrentIndex];
		}

		public void refreshColorAry(BinGradeColor binGC)
		{
			float fltMinVal = binGC.Min;
			float fltMaxVal = binGC.Max;
			float fltStepVal = binGC.Step;
			float fltStartVal = fltMinVal;

			Color[] colorAry = binGC.GetColorItems();

			txtMinColor.BackColor = binGC.ColorMin;
			txtMaxColor.BackColor = binGC.ColorMax;
			txtMinVal.Text = binGC.Min.ToString();
			txtMaxVal.Text = binGC.Max.ToString();
			txtStepVal.Text = binGC.Step.ToString();
			chkBoxOutOfRange.Checked = BinGradeColorSet.UseNGColor;
			lblMinColorPcs.Text = "0";
			lblMaxColorPcs.Text = "0";
			this.label4.Text = "<" + binGC.Min.ToString();
			this.label5.Text = ">" + binGC.Max.ToString();
			for (int i = 0; i < lblColAry.Length; i++)
			{
				lblColAry[i].BackColor = colorAry[i];
				lblValAry[i].Text = fltStartVal.ToString(binGC.DisplayFormat);
				lblPcsAry[i].Text = "0";
				lblValAry[i].Location = new System.Drawing.Point(lblColAry[i].Location.X - lblValAry[i].Width, lblColAry[i].Location.Y);

				if ((fltStartVal += fltStepVal) > fltMaxVal)
                    fltStartVal = fltMaxVal;
			}
		}

		#endregion

		#region >>> Form Event Handler <<<
		private void frmBinColorShow_Load( object sender, EventArgs e )
		{
		}
		#endregion

		#region >>> Private Method <<<
		private void InitBinGradeColorSet()
		{
			dtList = BinGradeColorSet.GetNameList();
			binGradeColorAry = new BinGradeColor[dtList.Count];
		}

		private void InstanceBinGradeColor()
		{
			int i = 0;
			dtBGC.Clear();

			foreach ( KeyValuePair<string, string> item in dtList )
			{
				binGradeColorAry[i] = BinGradeColorSet.GetColorItem( item.Key );
				dtBGC.Add( item.Key, i );

				i++;
			}
		}

		private void InitFirstItem( string strKeyName )
		{
			intCurrentIndex = dtBGC[strKeyName];

			BinGradeColor bgc = binGradeColorAry[intCurrentIndex];
			if ( _isColorCellsInit == false )
			{
				newColorCtlAry( bgc );
				_isColorCellsInit = true;
			}

			refreshColorAry( bgc );
		}

		#endregion

		#region >>> Event Handler <<<
		private void UpdateTimer_Tick( object sender, EventArgs e )
		{
			intCntItemAry = BinGradeColorSet.GetCounterItem( _currentKey );
			if ( intCntItemAry == null || intCntItemAry.Length == 0 )
				return;

			int[] out_of_range = BinGradeColorSet.GetOutOfRangeCounter( _currentKey );
			lblMinColorPcs.Text = out_of_range[0].ToString( "" );
			lblMaxColorPcs.Text = out_of_range[1].ToString( "" );

			intTotalPcs = countTotalPcs( intCntItemAry );
			intTotalPcs += out_of_range[0];
			intTotalPcs += out_of_range[1];

			showPcs();
		}

		#endregion

		private bool _isColorCellsInit;
		private void newColorCtlAry( BinGradeColor binGC )
		{
			lblColAry = new Label[32];
			lblValAry = new Label[32];
			lblPcsAry = new Label[32];

			int intX = 70+20, intY = 0;

			StatisticsGroup.SuspendLayout();
			for ( int i = 0; i < lblColAry.Length; i++ )
			{
				Label lbl = new Label();
				lbl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
				lbl.Font = new System.Drawing.Font( "Tahoma", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ( ( byte ) ( 136 ) ) );
				lbl.Location = new System.Drawing.Point( intX, intY += 12 );
				lbl.Size = new System.Drawing.Size( 12, 12 );
				this.StatisticsGroup.Controls.Add( lbl );
				lblColAry[i] = lbl;
				lbl = null;

				lbl = new Label();
				lbl.Location = new System.Drawing.Point( intX - 12, intY );
				lbl.Font = new System.Drawing.Font( "Tahoma", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ( ( byte ) ( 136 ) ) );
				lbl.Size = new System.Drawing.Size( 40, 12 );
				this.StatisticsGroup.Controls.Add( lbl );
				lbl.AutoSize = true;
				lblValAry[i] = lbl;
				lbl = null;

				lbl = new Label();
				lbl.Location = new System.Drawing.Point( intX + 12, intY );
				lbl.Font = new System.Drawing.Font( "Tahoma", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ( ( byte ) ( 136 ) ) );
				lbl.Size = new System.Drawing.Size( 40, 12 );
				this.StatisticsGroup.Controls.Add( lbl );
				lblPcsAry[i] = lbl;

                //if ( i == 15 )
                //{
                //    intX += 125;
                //    intY = 1;
                //}
			}

			StatisticsGroup.ResumeLayout();
		}

		#region >>> UI Event Handler <<<
		private void onClickChangeColor( object sender, EventArgs e )
		{
			Control ctl = sender as Control;

			ctl.BackColor = BinGradeColorSet.ShowColorPicker( ctl );
		}

		#endregion

		private void rdoPcs_CheckedChanged( object sender, EventArgs e )
		{
			_isPcsRdoBoxFlag = true;
		}

		private void rdoPrs_CheckedChanged( object sender, EventArgs e )
		{
			_isPcsRdoBoxFlag = false;
		}

		private void showPcs()
		{
			if ( _isPcsRdoBoxFlag ) // show pcs
			{
				for ( int i = 0; i < 32; i++ )
				{
					lblPcsAry[i].Text = intCntItemAry[i].ToString();
				}
			}
			else // show pcs %
			{
				for ( int i = 0; i < 32; i++ )
				{
					double dblTemp;

					if ( intTotalPcs <= 0 )
						dblTemp = 0;
					else
						dblTemp = intCntItemAry[i] / (double)intTotalPcs;

					//lblPcsAry[i].Text = string.Format( "0.0000%", dblTemp );
                    lblPcsAry[i].Text = dblTemp.ToString("0.00%");//string.Format("{0.00%}", dblTemp);
				}
			}
		}

		private Int32 countTotalPcs( Int32[] intCntItmAry )
		{
			Int32 iTotalPcs = 0;
			intTotalPcs = 0;

			for ( int i = 0; i < 32; i++ )
			{
				iTotalPcs += intCntItmAry[i];
			}

			return iTotalPcs;
		}
	}
}
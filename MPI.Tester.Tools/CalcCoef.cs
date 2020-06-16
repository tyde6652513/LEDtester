using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using MPI.Tester.Data;
using MPI.Tester.Maths;

namespace MPI.Tester.Tools
{
	public class CalcCoef
	{
		private object _lockObj;

		private int _number;

		private int _maxWave;
		private int _minWave;

        private ECalBaseWave _calBaseWave;

		private double[] _wavelength;
		private double [][] _stdData;			// [col := items ] [ row := data ]
		private double [][] _msrtData;
		private double [][] _compareData;

		private CalcGainOffset[]  _gainOffsetArray;
		private double[][] _coefParamArray;
		private string[] _titleName;

		private int _extWaveMode;
		private int _extWavePoint;
		private double _extWaveStart;
		private double _extWaveEnd;

		private int _calcCoefMode;
		private int _filterStdevCount;
		private int _lookTableByStdMsrt;

		private Statistic _stsDataPint;

		public CalcCoef()
		{
			this._lockObj = new object();

			this._minWave = 380;
			this._maxWave = 780;
			this._calBaseWave = ECalBaseWave.By_WLD;
			this._titleName = new string[6] { "WLP1", "WLD1", "WLC1", "LOP1", "WATT1", "LM1" };

			this._extWaveMode = 0;
			this._extWavePoint = 5;
			this._extWaveStart = 430.0d;
			this._extWaveEnd = 470.0d;

			this._calcCoefMode = 0;
			this._filterStdevCount = 6;
			this._lookTableByStdMsrt = 1;

			this._stsDataPint = new Statistic();
		}

		public CalcCoef(int number, double[][] stdData, double[][] msrtData, ECalBaseWave calBaseWave, string[] titleName) : this()
		{
			this._number = number;
			this._stdData = stdData;
			this._msrtData = msrtData;
			this._calBaseWave = calBaseWave;
			this._titleName = titleName;

            this.CreateGainOffset();
			this._coefParamArray = new double[7][] {	new double[] { 0.0d },		// 6-item , WLP, WLC, WLD, LOP, WATT, LM
																	new double[] { 0.0d },
																	new double[] { 0.0d },
																	new double[] { 1.0d },
																	new double[] { 1.0d },
																	new double[] { 1.0d },
                                                                    new double[] { 0.0d }};
		}

		#region >>> Public Property <<<

        public int Number
        {
            get { return this._number; }
            set { this._number = value; }
        }

		public int MinWave
		{
			get { return this._minWave; }
		}

		public int MaxWave
		{
			get { return this._maxWave; }
		}

		public double[][] CoefParamArray
		{
			get { return this._coefParamArray; }
		}

		public CalcGainOffset[] GainOffsetArray
		{
			get { return this._gainOffsetArray; }
		}

		public double[][] CompareData
		{
			get{ return this._compareData; }
		}

		public double[] TableBaseWave
		{
			get{ return this._wavelength; }
		}

		public ECalBaseWave CalBaseWave
		{
			get { return this._calBaseWave; }
			set { this._calBaseWave = value; }
		}

		public int ExtWaveMode
		{
			get { return this._extWaveMode; }
			set { this._extWaveMode = value; }
		}

		public int ExtWavePoint
		{
			get { return this._extWavePoint; }
			set { this._extWavePoint = value; }
		}

		public double ExtWaveStart
		{
			get { return this._extWaveStart; }
			set { this._extWaveStart = value; }
		}

		public double ExtWaveEnd
		{
			get { return this._extWaveEnd; }
			set { this._extWaveEnd = value; }
		}

		public int FilterStdevCount
		{
			get { return this._filterStdevCount; }
			set { this._filterStdevCount = value; }
		}

		public int CalcCoefMode
		{
			get { return this._calcCoefMode; }
			set { this._calcCoefMode = value; }
		}

		public int LookTableByStdMsrt
		{
			get { return this._lookTableByStdMsrt; }
			set { this._lookTableByStdMsrt = value; }
		}

		public string KeyName
		{
			get { return ("Coef_" + Number.ToString()); }
		}

		#endregion 

       #region >>> Private Method <<<

        private void CreateGainOffset()
		{
                int index = this._number;
                this._gainOffsetArray = new CalcGainOffset[7];
                this._gainOffsetArray[0] = new CalcGainOffset("WLP_" + index.ToString(), this._titleName[0], this._msrtData[0], this._stdData[0]);
				this._gainOffsetArray[0].CalcType = EGainOffsetType.Offset;
                this._gainOffsetArray[1] = new CalcGainOffset("WLD_" + index.ToString(), this._titleName[1], this._msrtData[1], this._stdData[1]);
				this._gainOffsetArray[1].CalcType = EGainOffsetType.Offset;
                this._gainOffsetArray[2] = new CalcGainOffset("WLC_" + index.ToString(), this._titleName[2], this._msrtData[2], this._stdData[2]);
				this._gainOffsetArray[2].CalcType = EGainOffsetType.Offset;

                this._gainOffsetArray[3] = new CalcGainOffset("LOP_" + index.ToString(), this._titleName[3], this._msrtData[3], this._stdData[3]);
				this._gainOffsetArray[3].CalcType = EGainOffsetType.Gain;
                this._gainOffsetArray[4] = new CalcGainOffset("WATT_" + index.ToString(), this._titleName[4], this._msrtData[4], this._stdData[4]);
				this._gainOffsetArray[4].CalcType = EGainOffsetType.Gain;
                this._gainOffsetArray[5] = new CalcGainOffset("LM_" + index.ToString(), this._titleName[5], this._msrtData[5], this._stdData[5]);
				this._gainOffsetArray[5].CalcType = EGainOffsetType.Gain;

                this._gainOffsetArray[6] = new CalcGainOffset("HW_" + index.ToString(), this._titleName[6], this._msrtData[6], this._stdData[6]);
				this._gainOffsetArray[6].CalcType = EGainOffsetType.Offset;
		}

		private void AssignWavelength()
		{
			//-------------------------------------------------
			//  Assign Data To Wavelengths By Selected Item
			//-------------------------------------------------
			switch (this._calBaseWave)
			{
				case ECalBaseWave.By_WLP:
					if (this._lookTableByStdMsrt == 1)
					{
						this._wavelength = this._msrtData[0];	// WLP, Msrt data as wave segments
					}
					else
					{
						this._wavelength = this._stdData[0];	// WLP, Std data as wave segments
					}
					break;
				//------------------------------------------------------------------
				case ECalBaseWave.By_WLD:
					if (this._lookTableByStdMsrt == 1)
					{
						this._wavelength = this._msrtData[1];	// WLD, Msrt data as wave segments
					}
					else
					{
						this._wavelength = this._stdData[1];	// WLD, Std data as wave segments
					}
					break;
				//------------------------------------------------------------------
				case ECalBaseWave.By_WLC:
					if (this._lookTableByStdMsrt == 1)
					{
						this._wavelength = this._msrtData[2];	// WLC, Msrt data as wave segments
					}
					else
					{
						this._wavelength = this._stdData[2];	// WLC, Std data as wave segments
					}
					break;
				//------------------------------------------------------------------
				default:
					this._wavelength = this._msrtData[1];	// WLD, msrt data as wave segments
					break;
			}
		}

        private void ReCeateCompareData()
        {
            for (int col = 0; col < this._compareData.Length; col++)  //7
            {
                if (this._compareData[col].Length == 0)
                    break;

                if (this._gainOffsetArray[col].CalcType != EGainOffsetType.GainAndOffest)
                {
                    continue;
                }
             
                for (int row = 0; row < this._compareData[0].Length; row++)
                {
                    if (this._stdData[col][row] == 0.0d && this._msrtData[col][row] == 0.0d)
                    {
                        this._compareData[col][row] = 1.0d;
                    }
                    else if (this._stdData[col][row] == 0.0d && this._msrtData[col][row] != 0.0d)
                    {
                        this._compareData[col][row] = 0.0d;
                    }
                    else if (this._stdData[col][row] != 0.0d && this._msrtData[col][row] == 0.0d)
                    {
                        this._compareData[col][row] = double.MaxValue;
                    }
                    else
                    {
                       double msrtValue = (this._gainOffsetArray[col].Gain * this._msrtData[col][row]) + this._gainOffsetArray[col].Offset;
                           
                        this._compareData[col][row] = (double)this._stdData[col][row] / msrtValue;
                    }
                }
            }
        }

        //public void SetChannelFactorToCompareData(Dictionary<string, CalcGainOffset[]> calcData)
        //{
        //    for (int col = 0; col < this._compareData.Length; col++)  //7
        //    {
        //        if (this._compareData[col].Length == 0)
        //            break;

        //        if (calcData.ContainsKey(this._gainOffsetArray[col].KeyName))
        //        {
        //            for (int row = 0; row < this._compareData[0].Length; row++)
        //            {
        //                if (this._stdData[col][row] == 0.0d && this._msrtData[col][row] == 0.0d)
        //                {
        //                    this._compareData[col][row] = 1.0d;
        //                }
        //                else if (this._stdData[col][row] == 0.0d && this._msrtData[col][row] != 0.0d)
        //                {
        //                    this._compareData[col][row] = 0.0d;
        //                }
        //                else if (this._stdData[col][row] != 0.0d && this._msrtData[col][row] == 0.0d)
        //                {
        //                    this._compareData[col][row] = double.MaxValue;
        //                }
        //                else
        //                {
        //                    double msrtValue = (this._gainOffsetArray[col].Gain * this._msrtData[col][row]) + this._gainOffsetArray[col].Offset;

        //                    this._compareData[col][row] = (double)this._stdData[col][row] / msrtValue;
        //                }
        //            }
        //        }
        //    }
        //}


		private void CreateCompareData()
		{
			int[] minWave = new int[3];
			int[] maxWave = new int[3];

			this._compareData = new double[this._stdData.Length][];
			for (int col = 0; col < this._stdData.Length; col++)
			{
				this._compareData[col] = new double[this._stdData[0].Length];

				if ( this._compareData[col].Length == 0 )
					break;

				if (col < 3)				// WLP, WLD, WLC
				{
					minWave[col] = (int)Math.Round(MPI.Maths.Statistic.Min(this._stdData[col]), 0);
					maxWave[col] = (int)Math.Round(MPI.Maths.Statistic.Max(this._stdData[col]), 0);
					for (int row = 0; row < this._stdData[0].Length; row++)
					{
						this._compareData[col][row] = (double)this._stdData[col][row] - (double)this._msrtData[col][row];
					}
				}
                else if (col == 6)			// HW
                {
                    for (int row = 0; row < this._stdData[0].Length; row++)
                    {
                        this._compareData[col][row] = (double)this._stdData[col][row] - (double)this._msrtData[col][row];
                    }
                }
                else						// LOP, WATT, LM
                {
                    for (int row = 0; row < this._stdData[0].Length; row++)
                    {
                        if (this._stdData[col][row] == 0.0d && this._msrtData[col][row] == 0.0d)
                        {
                            this._compareData[col][row] = 1.0d;
                        }
                        else if (this._stdData[col][row] == 0.0d && this._msrtData[col][row] != 0.0d)
                        {
                            this._compareData[col][row] = 0.0d;
                        }
                        else if (this._stdData[col][row] != 0.0d && this._msrtData[col][row] == 0.0d)
                        {
                            this._compareData[col][row] = double.MaxValue;
                        }
                        else
                        {
                            if (this._gainOffsetArray[col].CalcType == EGainOffsetType.GainAndOffest)
                            {
                                double msrtValue = (this._gainOffsetArray[col].Gain * this._msrtData[col][row]) + this._gainOffsetArray[col].Offset;
                                this._compareData[col][row] = (double)this._stdData[col][row] / msrtValue;
                            }
                            else
                            {
                                this._compareData[col][row] = (double)this._stdData[col][row] / (double)this._msrtData[col][row];
                            }
                        }
                    }
                }
			}

			switch (this._calBaseWave)
			{ 
				case ECalBaseWave.By_WLP:
					this._minWave = minWave[0];
					this._maxWave = maxWave[0];
					break;
				//-------------------------------------------------
				case ECalBaseWave.By_WLD:
					this._minWave = minWave[1];
					this._maxWave = maxWave[1];
					break;
				//-------------------------------------------------
				case ECalBaseWave.By_WLC:
					this._minWave = minWave[2];
					this._maxWave = maxWave[2];
					break;
				//-------------------------------------------------
				default:
					this._minWave = minWave[1];
					this._maxWave = maxWave[1];
					break;
			}

			//this._minWave = minWave[(int)this._calBaseWave];
			//this._maxWave = maxWave[0];
			//for ( int i = 0; i < maxWave.Length; i++ )
			//{
			//    if ( minWave[i] < this._minWave )
			//    {
			//        this._minWave = minWave[i];
			//    }

			//    if ( maxWave[i] > this._maxWave )
			//    {
			//        this._maxWave = maxWave[i];
			//    }
			//}

			 
		}

        private double[] CaculateAllCoef(Dictionary<int, List<double>> compreDataByWave) //data Of Interval
		{     
            Dictionary<int, double> calculatedCoeff = new Dictionary<int, double>(compreDataByWave.Count);

            foreach (int key in compreDataByWave.Keys) //oneIntervalData
            {
				double calcCompensatedCoeff = this.CalcCompensatedCoef(compreDataByWave[key]);
                calculatedCoeff.Add(key, calcCompensatedCoeff);
            }


            //---------------------------------------------------------
            // Interpolate : interpolate the nonexistence wavelength
            //---------------------------------------------------------

            double[] interpolatedCoeffs;
            this.InterpolateNonexistenceWavelength(calculatedCoeff, out interpolatedCoeffs, out this._minWave, out this._maxWave);
            
			return interpolatedCoeffs;
		}

        private double CalcCompensatedCoef(List<double>dataInOneIntervals)
        {
            double calcCompensatedCoeff=0;
			int lowIndex = 0;
			int highIndex = 0;
			List<double> lstNewDataPoint = new List<double>(dataInOneIntervals.Count);

            switch (this._calcCoefMode)
            {
                case 0:
					//=============================
					// Calc Mode = Average
					//=============================
					//double singleIntervalStdev = MPI.Maths.Statistic.StandardDeviation(dataInOneIntervals.ToArray());
					//double average= MPI.Maths.Statistic.Average(dataInOneIntervals.ToArray());

					//for (int i = 0; i < dataInOneIntervals.Count; i++)
					//{
					//    double delta = average - dataInOneIntervals[i];
					//    //------------------------------------------------------------------
					//    // delta >= this._filterStdevCount * stdev / 2.0d ==> Filter it
					//    // this.__filterStdevCount = 6   ==> 3 Standard deviation
					//    //------------------------------------------------------------------
					//    if (Math.Abs(delta) > (this._filterStdevCount * singleIntervalStdev / 2.0d))  
					//    {
					//        dataInOneIntervals.RemoveAt(i);			//remove over Spec. range data
					//    }
					//}
					//calcCompensatedCoeff = MPI.Maths.Statistic.Average(dataInOneIntervals.ToArray());

					this._stsDataPint.Clear();
					this._stsDataPint.PushAll(dataInOneIntervals.ToArray());
					double delta = 0.0d;
					lstNewDataPoint.Clear();
					for( int i = 0; i< dataInOneIntervals.Count; i++ )
					{
					    delta = Math.Abs( dataInOneIntervals[i] - this._stsDataPint.Mean);
					    if ( delta <= (this._filterStdevCount * this._stsDataPint.STDEV / 2.0d) )
					    {
					        lstNewDataPoint.Add(dataInOneIntervals[i]);
					    }
					}					
					calcCompensatedCoeff = MPI.Tester.Maths.Statistic.CalcMean(dataInOneIntervals.ToArray());
					break;
				//-------------------------------------------------------------------
				case 1:
					//=============================
					// Calc Mode = Median
					//=============================
					dataInOneIntervals.Sort();
					lstNewDataPoint.Clear();
					if (dataInOneIntervals.Count < this._filterStdevCount)
					{
						//dataInOneIntervals.RemoveAll(); // .RemoveRange(0, deleteCounter); //lower
					}
					else if (this._filterStdevCount == 2)
					{
						calcCompensatedCoeff = dataInOneIntervals[(int)(dataInOneIntervals.Count / 2.0d)];
					}
					else if (this._filterStdevCount >= 4)
					{
						lowIndex = (int)(dataInOneIntervals.Count / this._filterStdevCount) + 1;
						highIndex = (int)(this._filterStdevCount - 1) * dataInOneIntervals.Count / this._filterStdevCount + 1;

						for (int i = 0; i < dataInOneIntervals.Count; i++)
						{
							if (i >= lowIndex && i < highIndex)
							{
								lstNewDataPoint.Add(dataInOneIntervals[i]);
							}
						}
						calcCompensatedCoeff = MPI.Tester.Maths.Statistic.CalcMean(lstNewDataPoint.ToArray());
					}
					break;
				//-------------------------------------------------------------------
                case 2:
					//=============================
					// Calc Mode = Centroid
					//=============================
					int deleteCounter = 0;				
					dataInOneIntervals.Sort();							

					// min chips = 6
					if (dataInOneIntervals.Count < this._filterStdevCount )
					{
						deleteCounter = 0;
					}
					else if ((dataInOneIntervals.Count - this._filterStdevCount) < 6)
					{
						deleteCounter = dataInOneIntervals.Count - 6;
					}
					else
					{ 
						deleteCounter = this._filterStdevCount;
					}
					dataInOneIntervals.RemoveRange(dataInOneIntervals.Count - deleteCounter, deleteCounter); //up
					dataInOneIntervals.RemoveRange(0, deleteCounter); //lower
					calcCompensatedCoeff = (dataInOneIntervals[0] + dataInOneIntervals[dataInOneIntervals.Count - 1]) * 0.5;
					break;
				//-------------------------------------------------------------------
                default :
					calcCompensatedCoeff = 0;
					break;
            }

            return calcCompensatedCoeff;

        }

        private void InterpolateNonexistenceWavelength(Dictionary<int, double> calculatedCoeff, out double[] interpolatedCoeffs,out int minWavelength,out int maxWavelength)
        {
            List<int> waveIndex = new List<int>(calculatedCoeff.Keys);
            waveIndex.Sort();

            int step = waveIndex[1] - waveIndex[0];
            int stepCount = 1;
            double range = 0.0d;
            double addValue = 0.0d;
            for (int i = 0; i < (waveIndex.Count - 1); i++)
            {
                stepCount = (waveIndex[i + 1] - waveIndex[i]) / step;
                if (stepCount > 1)
                {
                    range = calculatedCoeff[waveIndex[i + 1]] - calculatedCoeff[waveIndex[i]];
                    for (int k = 1; k < stepCount; k++)
                    {
                        addValue = calculatedCoeff[waveIndex[i]] + (range * k / (double)stepCount);
                        calculatedCoeff.Add(waveIndex[i] + step * k, addValue);
                    }
                }
            }


            waveIndex = new List<int>(calculatedCoeff.Keys);

            interpolatedCoeffs = new double[waveIndex.Count];

            waveIndex.Sort();

            for (int i = 0; i < waveIndex.Count; i++)
            {
                interpolatedCoeffs[i] = calculatedCoeff[waveIndex[i]];
            }

            minWavelength = waveIndex[0];
            maxWavelength = waveIndex[waveIndex.Count - 1];

        }

        private double InterpolationExtendCoeff(double x1,double y1, double x2,double y2,double x3) // interpolation
        {
            double y3 = 0;

            double dd = (y2 - y1) / (x2 - x1);
            
            //(x3-x1)dd=(y3-y1);

             y3=(x3-x1)*dd+y1;

             return y3;
        }

       #endregion

       #region >>> Public Method <<<

		public void RunCalculateBigGainOffset()	
        {
			this.CreateCompareData();
			this.AssignWavelength();

			double[][] coefParamArray = new double[8][];

			for (int i = 0; i < coefParamArray.Length; i++)
			{
				coefParamArray[i] = new double[2];
			}

			coefParamArray[0][0] = this._minWave;
			coefParamArray[0][1] = this._maxWave;

			//------------------------------------------------------------
			// WLP  ->  WLD  ->  WLC  ->  LOP  ->  WATT  ->  LM  ->  HW
			//------------------------------------------------------------
            for (int i = 0; i < this._gainOffsetArray.Length; i++)
            {
                if (this._gainOffsetArray[i] != null)
                {
                    this._gainOffsetArray[i].RunCalculate(); //this._msrtData[i], this._stdData[i], gainOffsetMode);
					
					if ( i < 3 )					// WLP, WLD, WLC
					{
					coefParamArray[i+1][0] = this._gainOffsetArray[i].Offset;
					coefParamArray[i+1][1] = coefParamArray[i+1][0];
					}
					else if ( i>=3 && i<=5 )		// LOP, WATT, LM
					{
						coefParamArray[i+1][0] = this._gainOffsetArray[i].Gain;
						coefParamArray[i+1][1] = coefParamArray[i+1][0];
					}
					else							// HW
					{
						coefParamArray[i+1][0] = this._gainOffsetArray[i].Offset;
						coefParamArray[i+1][1] = coefParamArray[i+1][0];
					}
                }
            }

			this._coefParamArray = coefParamArray;
        }

        public void RunCalculateCoefTable(int type)
        {
            if (this._stdData == null || this._msrtData == null)
                return;

            if (this._stdData.Length != this._msrtData.Length ||this._stdData[0].Length != this._msrtData[0].Length)
                return;

            this._wavelength = null;

			//----------------------------------------------------------
			// (1) Calculate the BIG Gain and Offset, and
			//     Includes [a] Create comparedData 
			//				[b] Assign wavelength
			//----------------------------------------------------------
			this.RunCalculateBigGainOffset();
            this.ReCeateCompareData();

            //if (isType3)
            //{
            //    ReCeateCompareData();
            //}
            //----------------------------------------------------------
            //  (2) Adjust the compared data by BIG gain and offset
            //----------------------------------------------------------
            for (int col = 0; col < this._compareData.Length; col++)
            {
                if (this._gainOffsetArray[col] == null)
                    continue;

                if (col < 3 || col == 6)		// WLP = 0, WLD = 1, WLC = 2 HW=6,
                {
                    for (int row = 0; row < this._compareData[0].Length; row++)	//Find Max Min _wavelength
                    {
                        this._compareData[col][row] = this._compareData[col][row] - this._gainOffsetArray[col].Offset;
                    }
                }
                else							// LOP = 3, WATT = 4, LM = 5,
                {
                    for (int row = 0; row < this._compareData[0].Length; row++)	//Find Max Min _wavelength
                    {
                        if (this._gainOffsetArray[col].Gain == 0.0d)
                        {
                            this._compareData[col][row] = double.MaxValue;
                        }
                        else
                        {
                            if (this._gainOffsetArray[col].CalcType == EGainOffsetType.Gain)
                            {
                                this._compareData[col][row] = this._compareData[col][row] / this._gainOffsetArray[col].Gain;
                            }
                        }
                    }
                }
            }           

            int keyWave;
			Dictionary<int, List<double>> compareDataByWave = new Dictionary<int, List<double>>();
            List<double> accumlateCoeff;
			double[][] coefParamArray = new double[8][];

            for (int col = 0; col < 7; col++)
            {
                compareDataByWave.Clear();

                for (int row = 0; row < this._wavelength.Length; row++)
                {
                    keyWave = (int)Math.Round(this._wavelength[row], 0);		// Math.Round(4.5); //Returns 4.0.  ; Math.Round(4.6); //Returns 5.0  

                    accumlateCoeff = new List<double>();

					//-----------------------------------------------
                    // Assign all data To Grade By KeyWave
					//-----------------------------------------------
                    if (compareDataByWave.ContainsKey(keyWave))
                    {
                        accumlateCoeff = compareDataByWave[keyWave];
                        compareDataByWave.Remove(keyWave);
                        accumlateCoeff.Add(this._compareData[col][row]);
                        compareDataByWave.Add(keyWave, accumlateCoeff);
                    }
                    else
                    {
                        accumlateCoeff.Add(this._compareData[col][row]);
                        compareDataByWave.Add(keyWave, accumlateCoeff);
                    }
                }

                //-----------------------------------------------
                // Add extend wavelength to dictionary.
                // Extend coef. table 
                //-----------------------------------------------
                if (compareDataByWave.Keys.Count == 0)
                    return;

                int coefMinWLD = compareDataByWave.Keys.Min();
                int coefMaxWLD = compareDataByWave.Keys.Max();
				int minWLDStartWL = coefMinWLD + 1;
				int maxWLDStartWL = coefMaxWLD - 1;
				int minWLExtPoint = 0;
				int maxWLExtPoint = 0;

				if (this._extWaveMode == 0)
				{
					minWLExtPoint = this._extWavePoint;
					maxWLExtPoint = this._extWavePoint;
				}
				else
				{
					if ( this._extWaveEnd > coefMaxWLD )
					{
						maxWLExtPoint = (int)this._extWaveEnd - coefMaxWLD + 1;
					}
					else
					{
						maxWLExtPoint = 0;
					}

					if ( this._extWaveStart < coefMinWLD )
					{
						minWLExtPoint = coefMinWLD - (int)this._extWaveStart - 1;
					}
					else
					{
						minWLExtPoint = 0;
					}
				}

				if ((coefMaxWLD - coefMinWLD) <= 1)		// ex.  445, 446, 
				{
					//============================================
					// Extend the high wavelength (max Wave)
					//============================================
					for (int ext = 1; ext <= maxWLExtPoint; ext++)
					{
						accumlateCoeff = new List<double>(compareDataByWave[coefMaxWLD]);
						compareDataByWave.Add(coefMaxWLD + ext, accumlateCoeff);
					}

					//============================================
					// Extend the low wavelength (min Wave)
					//============================================
					for (int ext = 1; ext <= minWLExtPoint; ext++)
					{
						accumlateCoeff = new List<double>(compareDataByWave[coefMinWLD]);
						compareDataByWave.Add(coefMinWLD - ext, accumlateCoeff);
					}
				}
				else
				{
					//============================================
					// Extend the high wavelength (max Wave)
					//============================================

                    switch (type)
                    {
                        case 0 :
                            for (int ext = 1; ext <= maxWLExtPoint; ext++)
                            {
                                if (compareDataByWave.Keys.Contains(maxWLDStartWL))
                                {
                                    accumlateCoeff = new List<double>(compareDataByWave[maxWLDStartWL]);
                                    compareDataByWave.Remove(coefMaxWLD);
                                    compareDataByWave.Add(maxWLDStartWL + ext, accumlateCoeff);
                                }
                                else
                                {
                                    accumlateCoeff = new List<double>(compareDataByWave[coefMaxWLD]);
                                    compareDataByWave.Add(coefMaxWLD + ext, accumlateCoeff);
                                }

                                //if (minWLDStartWL == (coefMaxWLD + ext))
                                //    continue;

                                //if (compareDataByWave.Keys.Contains(minWLDStartWL))
                                //{
                                //    accumlateCoeff = new List<double>(compareDataByWave[minWLDStartWL]);
                                //    compareDataByWave.Remove(coefMinWLD);
                                //    compareDataByWave.Add(minWLDStartWL - ext, accumlateCoeff);
                                //}
                                //else
                                //{
                                //    accumlateCoeff = new List<double>(compareDataByWave[coefMinWLD]);
                                //    compareDataByWave.Add(coefMinWLD - ext, accumlateCoeff);
                                //}
                            }

                            //============================================
                            // Extend the low wavelength (min Wave)
                            //============================================
                            for (int ext = 1; ext <= minWLExtPoint; ext++)
                            {
                                if (compareDataByWave.Keys.Contains(minWLDStartWL))
                                {
                                    accumlateCoeff = new List<double>(compareDataByWave[minWLDStartWL]);
                                    compareDataByWave.Remove(coefMinWLD);
                                    compareDataByWave.Add(minWLDStartWL - ext, accumlateCoeff);
                                }
                                else
                                {
                                    accumlateCoeff = new List<double>(compareDataByWave[coefMinWLD]);
                                    compareDataByWave.Add(coefMinWLD - ext, accumlateCoeff);
                                }
                            }

                            break;

                        case 1:

                            //============================================
                            // Extend the High wavelength (min Wave)
                            //============================================
                            for (int ext = 1; ext <= maxWLExtPoint; ext++)
                            {
                                if (compareDataByWave.Keys.Contains(coefMaxWLD))
                                {
                                    accumlateCoeff = new List<double>(compareDataByWave[coefMaxWLD]);
                                    compareDataByWave.Add(coefMaxWLD + ext, accumlateCoeff);
                                }
                            }

                            //============================================
                            // Extend the low wavelength (min Wave)
                            //============================================
                            for (int ext = 1; ext <= minWLExtPoint; ext++)
                            {
                                if (compareDataByWave.Keys.Contains(coefMinWLD))
                                {
                                    accumlateCoeff = new List<double>(compareDataByWave[coefMinWLD]);
                                    compareDataByWave.Add(coefMinWLD - ext, accumlateCoeff);
                                }
                            }
                            break;
                    }
                 }
				coefParamArray[col + 1] = this.CaculateAllCoef(compareDataByWave);
            }
            coefParamArray[0] = new double[coefParamArray[1].Length];

			for (int row = 0; row < coefParamArray[0].Length; row++)
			{
				coefParamArray[0][row] = (double)this._minWave + 1.0d * row;	// step = 1nm;
			}

			//int digit = 0;

			//for (int col = 0; col < coefParamArray.Length; col++)
			//{
			//    for (int row = 0; row < coefParamArray[0].Length; row++)
			//    {					
			//        if (col == 0)		// Wave
			//        {
			//            coefParamArray[col][row] = (double)this._minWave + 1.0d * row;	// step = 1nm;
			//            coefParamArray[col][row] = Math.Round(coefParamArray[col][row], 1, MidpointRounding.AwayFromZero);
			//        }
			//        else				// WLP, WLC, WLD, LOP, WATT, LM, HW
			//        {
			//            digit = this._gainOffsetArray[col - 1].Digit + this._gainOffsetArray[col - 1].ExtDigit;
			//            coefParamArray[col][row] = Math.Round(coefParamArray[col][row], digit, MidpointRounding.AwayFromZero);
			//        }
			//    }
			//}

            this._coefParamArray = coefParamArray;
        }

        public double GetUnderSpecPercent(int index, double specValue)
        {
            if (this._compareData == null)
                return 0.0d;

            if (index < 0 || index > this._compareData.Length)
                return 0.0d;

            double goodCount = 0.0d;

            if (this._compareData[index] == null)
                return 0.0d;

            for (int i = 0; i < this._compareData[index].Length; i++)
            {
                if (index < 3)
                {
                    if (Math.Abs(this._compareData[index][i]) < specValue + 0.001)
                    {
                        goodCount += 1.0d;
                    }
                }
                else
                {
                    if (Math.Abs(this._compareData[index][i] - 1.0d) < specValue + 0.001)
                    {
                        goodCount += 1.0d;
                    }
                }
            }

            return (goodCount / (double)this._compareData[index].Length);
        }

        #endregion

    }
}

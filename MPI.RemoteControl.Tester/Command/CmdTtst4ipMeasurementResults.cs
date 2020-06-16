using System;
using System.Collections.Generic;
using System.Text;

namespace MPI.RemoteControl.Tester.Command
{
	/// <summary>
	/// IS Tester Command: Ttst4ipMeasurementResults(10)
	/// 
	///	struct Ttst4ipMeasurementResults
	///	{
	///		UInt16 Version		// 2 bytes
	///		UInt16 Count			// 2 bytes
	///		MeasurementResult[]  MeasurementResults		// 68 bytes * MAX_MEASUREMENT_RESULT_LIST_COUNT = 65520 bytes
	/// 
	///		struct MeasurementResult
	///		{
	///			double X							// 8 bytes
	///			double Y							// 8 bytes
	///			double Z							// 8 bytes
	///			double U							// 8 bytes
	///			double v1976						// 8 bytes
	///			double v1960						// 8 bytes
	///			double Rad						// 8 bytes
	///			double Phot						// 8 bytes
	///			double PeakX						// 8 bytes
	///			double PeakY						// 8 bytes
	///			double Centroid					// 8 bytes
	///			double LambdaDom				// 8 bytes
	///			double Purity						// 8 bytes
	///			double Width50					// 8 bytes
	///			double CCT						// 8 bytes
	///			double Current					// 8 bytes
	///			double Voltage					// 8 bytes
	///			Int32 ADC						// 4 bytes
	///			float[] CRI						// 17 * 4 bytes = 68 bytes
	///			double Scotopic					// 8 bytes
	///			double TriX						// 8 bytes
	///			double TriY						// 8 bytes
	///			double TriZ						// 8 bytes
	///		}
	///	}
	/// </summary>
	public class CmdTtst4ipMeasurementResults : Ttst4ipCommand
	{
		// Command ID
		public const UInt16 COMMAND_ID = (UInt16)EISCommand.ID_TTST4IP_MEASUREMENT_RESULT;

		// Data Length
		public const Int32 DATA_LENGTH = 65524;		// Version(2) + Count(2) + MAX_MEASUREMENT_RESULT_LENGTH(240) * MAX_MEASUREMENT_RESULT_LIST_COUNT = 65524 bytes

		// Position 
		public const Int32 VERSION_POS = 0;
		public const Int32 COUNT_POS = VERSION_POS + sizeof(UInt16);
		public const Int32 MEASUREMENT_RESULT_LIST_POS = COUNT_POS + sizeof(UInt16);

		/// <summary>
		/// Constructor
		/// </summary>
		public CmdTtst4ipMeasurementResults()
			: base(COMMAND_ID, DATA_LENGTH)
		{
		}

		/// <summary>
		/// Version
		/// </summary>
		public UInt16 Version
		{
			get { return this.GetUInt16Data(VERSION_POS); }
			set { this.SetUInt16Data(VERSION_POS, value); }
		}

		/// <summary>
		/// Measurement Result Counts
		/// </summary>
		public UInt16 Count
		{
			get { return this.GetUInt16Data(COUNT_POS); }
			set { this.SetUInt16Data(COUNT_POS, value); }
		}

		/// <summary>
		/// Get Mesurement Result By Index
		/// </summary>
		public MeasurementResult GetMeasurementResult(int index)
		{
			if (index < 0 || index >= Convert.ToInt32(this.Count))
			{
				return null;
			}

			int StartPos = MEASUREMENT_RESULT_LIST_POS + index * MAX_MEASUREMENT_RESULT_LENGTH;

            byte[] tmp = this.GetByteData(StartPos, MAX_MEASUREMENT_RESULT_LENGTH);

            return new MeasurementResult(tmp);
		}

	}

	public class MeasurementResult
	{
        // Sub Position
        public const Int32 X_POS = 0;
        public const Int32 Y_POS = X_POS + sizeof(Double);
        public const Int32 Z_POS = Y_POS + sizeof(Double);
        public const Int32 U_POS = Z_POS + sizeof(Double);
        public const Int32 V1976_POS = U_POS + sizeof(Double);
        public const Int32 V1960_POS = V1976_POS + sizeof(Double);
        public const Int32 RAD_POS = V1960_POS + sizeof(Double);
        public const Int32 PHOT_POS = RAD_POS + sizeof(Double);
        public const Int32 PEAK_X_POS = PHOT_POS + sizeof(Double);
        public const Int32 PEAK_Y_POS = PEAK_X_POS + sizeof(Double);
        public const Int32 CENTROID_POS = PEAK_Y_POS + sizeof(Double);
        public const Int32 LAMBDA_DOM_POS = CENTROID_POS + sizeof(Double);
        public const Int32 PURITY_POS = LAMBDA_DOM_POS + sizeof(Double);
        public const Int32 WIDTH50_POS = PURITY_POS + sizeof(Double);
        public const Int32 CCT_POS = WIDTH50_POS + sizeof(Double);
        public const Int32 CURRENT_POS = CCT_POS + sizeof(Double);
        public const Int32 VOLTAGE_POS = CURRENT_POS + sizeof(Double);
        public const Int32 ADC_POS = VOLTAGE_POS + sizeof(Double);
        public const Int32 COMMON_CRI_POS = ADC_POS + sizeof(Int32);
        public const Int32 CRI_ARRAY_POS = COMMON_CRI_POS + sizeof(Single);
        public const Int32 SCOTOPIC_POS = CRI_ARRAY_POS + sizeof(Single) * Ttst4ipCommand.MAX_CRI_ARRAY_COUNT;
        public const Int32 TRI_X_POS = SCOTOPIC_POS + sizeof(Double);
        public const Int32 TRI_Y_POS = TRI_X_POS + sizeof(Double);
        public const Int32 TRI_Z_POS = TRI_Y_POS + sizeof(Double);

		private byte[] m_MeasureResult;

		/// <summary>
		/// Constructor
		/// </summary>
		public MeasurementResult()
		{
			this.m_MeasureResult = new byte[Ttst4ipCommand.MAX_MEASUREMENT_RESULT_LENGTH];
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="MeasureResult"></param>
		public MeasurementResult(byte[] MeasureResult)
		{
			this.m_MeasureResult = new byte[Ttst4ipCommand.MAX_MEASUREMENT_RESULT_LENGTH];

			if (MeasureResult != null && MeasureResult.Length >= Ttst4ipCommand.MAX_MEASUREMENT_RESULT_LENGTH)
			{
				Array.Copy(MeasureResult, 0, this.m_MeasureResult, 0, Ttst4ipCommand.MAX_MEASUREMENT_RESULT_LENGTH);
			}
		}

		public Double X
		{
			get { return this.GetDoubleData(X_POS); }
			set { this.SetDoubleData(X_POS, value); }
		}

		public Double Y
		{
			get { return this.GetDoubleData(Y_POS); }
			set { this.SetDoubleData(Y_POS, value); }
		}

		public Double Z
		{
			get { return this.GetDoubleData(Z_POS); }
			set { this.SetDoubleData(Z_POS, value); }
		}

		public Double U
		{
			get { return this.GetDoubleData(U_POS); }
			set { this.SetDoubleData(U_POS, value); }
		}

		public Double v1976
		{
			get { return this.GetDoubleData(V1976_POS); }
			set { this.SetDoubleData(V1976_POS, value); }
		}

		public Double v1960
		{
			get { return this.GetDoubleData(V1960_POS); }
			set { this.SetDoubleData(V1960_POS, value); }
		}

		public Double Rad
		{
			get { return this.GetDoubleData(RAD_POS); }
			set { this.SetDoubleData(RAD_POS, value); }
		}

		public Double Phot
		{
			get { return this.GetDoubleData(PHOT_POS); }
			set { this.SetDoubleData(PHOT_POS, value); }
		}

		public Double PeakX
		{
			get { return this.GetDoubleData(PEAK_X_POS); }
			set { this.SetDoubleData(PEAK_X_POS, value); }
		}

		public Double PeakY
		{
			get { return this.GetDoubleData(PEAK_Y_POS); }
			set { this.SetDoubleData(PEAK_Y_POS, value); }
		}

		public Double Centroid
		{
			get { return this.GetDoubleData(CENTROID_POS); }
			set { this.SetDoubleData(CENTROID_POS, value); }
		}

		public Double LambdaDom
		{
			get { return this.GetDoubleData(LAMBDA_DOM_POS); }
			set { this.SetDoubleData(LAMBDA_DOM_POS, value); }
		}

		public Double Purity
		{
			get { return this.GetDoubleData(PURITY_POS); }
			set { this.SetDoubleData(PURITY_POS, value); }
		}

		public Double Width50
		{
			get { return this.GetDoubleData(WIDTH50_POS); }
			set { this.SetDoubleData(WIDTH50_POS, value); }
		}

		public Double CCT
		{
			get { return this.GetDoubleData(CCT_POS); }
			set { this.SetDoubleData(CCT_POS, value); }
		}

		public Double Current
		{
			get { return this.GetDoubleData(CURRENT_POS); }
			set { this.SetDoubleData(CURRENT_POS, value); }
		}

		public Double Voltage
		{
			get { return this.GetDoubleData(VOLTAGE_POS); }
			set { this.SetDoubleData(VOLTAGE_POS, value); }
		}

		public Int32 ADC
		{
			get { return this.GetInt32Data(ADC_POS); }
			set { this.SetInt32Data(ADC_POS, value); }
		}

        public Single CommonCRI
        {
            get { return this.GetSingleData(COMMON_CRI_POS); }
            set { this.SetSingleData(COMMON_CRI_POS, value); }
        }

		public Single[] CRI
		{
			get
			{
				int counts = Ttst4ipCommand.MAX_CRI_ARRAY_COUNT;

				float[] tmp = new float[counts];

                for (int index = 0; index < counts; index++)
                {
                    float fVal = this.GetSingleData(CRI_ARRAY_POS + sizeof(float) * index);
                    tmp[index] = fVal;
                }

				return tmp;
			}

			set
			{
				int counts = value.Length;

				for (int index = 0; index < counts; index++)
				{
					this.SetSingleData(CRI_ARRAY_POS + sizeof(float) * index, value[index]);
				}
			}
		}

			public Double Scotopic
		{
			get { return this.GetDoubleData(SCOTOPIC_POS); }
			set { this.SetDoubleData(SCOTOPIC_POS, value); }
		}

		public Double TriX
		{
			get { return this.GetDoubleData(TRI_X_POS); }
			set { this.SetDoubleData(TRI_X_POS, value); }
		}

		public Double TriY
		{
			get { return this.GetDoubleData(TRI_Y_POS); }
			set { this.SetDoubleData(TRI_Y_POS, value); }
		}

		public Double TriZ
		{
			get { return this.GetDoubleData(TRI_Z_POS); }
			set { this.SetDoubleData(TRI_Z_POS, value); }
		}


		#region >>> Private Method <<<

		private Double GetDoubleData(int StartPos)
		{
			if ((this.m_MeasureResult != null) && (this.m_MeasureResult.Length >= StartPos + sizeof(Double)))
			{

				return BitConverter.ToDouble(this.m_MeasureResult, StartPos);
			}
			else
			{
				return 0;
			}
		}

        private Int16 GetInt16Data(int StartPos)
        {
            if ((this.m_MeasureResult != null) && (this.m_MeasureResult.Length >= StartPos + sizeof(Int16)))
            {
                return BitConverter.ToInt16(this.m_MeasureResult, StartPos);
            }
            else
            {
                return 0;
            }
        }

		private Int32 GetInt32Data(int StartPos)
		{
			if ((this.m_MeasureResult != null) && (this.m_MeasureResult.Length >= StartPos + sizeof(Int32)))
			{
				return BitConverter.ToInt32(this.m_MeasureResult, StartPos);
			}
			else
			{
				return 0;
			}
		}

		private Single GetSingleData(int StartPos)
		{
			if ((this.m_MeasureResult != null) && (this.m_MeasureResult.Length >= StartPos + sizeof(Single)))
			{
				return BitConverter.ToSingle(this.m_MeasureResult, StartPos);
			}
			else
			{
				return 0;
			}
		}

		private void SetDoubleData(int StartPos, Double value)
		{
			if ((this.m_MeasureResult != null) && (this.m_MeasureResult.Length >= StartPos + sizeof(Double)))
			{
				Array.Copy(BitConverter.GetBytes(value), 0, this.m_MeasureResult, StartPos, sizeof(Double));
			}
		}

        private void SetInt16Data(int StartPos, Int16 value)
        {
            if ((this.m_MeasureResult != null) && (this.m_MeasureResult.Length >= StartPos + sizeof(Int16)))
            {
                Array.Copy(BitConverter.GetBytes(value), 0, this.m_MeasureResult, StartPos, sizeof(Int16));
            }
        }

		private void SetInt32Data(int StartPos, Int32 value)
		{
			if ((this.m_MeasureResult != null) && (this.m_MeasureResult.Length >= StartPos + sizeof(Int32)))
			{
				Array.Copy(BitConverter.GetBytes(value), 0, this.m_MeasureResult, StartPos, sizeof(Int32));
			}
		}

		private void SetSingleData(int StartPos, Single value)
		{
			if ((this.m_MeasureResult != null) && (this.m_MeasureResult.Length >= StartPos + sizeof(Single)))
			{
				Array.Copy(BitConverter.GetBytes(value), 0, this.m_MeasureResult, StartPos, sizeof(Single));
			}
		}

		#endregion
	}
}

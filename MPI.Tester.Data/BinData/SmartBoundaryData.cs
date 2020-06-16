using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Drawing;

namespace MPI.Tester.Data
{
	[Serializable]
	public class SmartBoundaryData : ICloneable, IEnumerable<SmartBoundaryBase>
	{
		#region >>> Private Property <<<

		private string _name;
		private string _keyName;
		private string _format;
		private List<SmartBoundaryBase> _boundaryList;
        private EBinBoundaryRule _boundaryRule;
        

		#endregion

		#region >>> Constructor / Disposor <<<

		public SmartBoundaryData()
		{
			this._name = string.Empty;

			this._keyName = string.Empty;

			this._format = "0.00";

			this._boundaryList = new List<SmartBoundaryBase>();
		}

		#endregion

		#region >>> Public Method <<<

		public bool IsInBoundary(Dictionary<string, double> rowData, int binSortingRule, out string serilaNimber)
		{
			serilaNimber = string.Empty;

			//Find KeyName & raw Value
			double value = 0.0d;

			string cieXKey = string.Empty;

			string cieYKey = string.Empty;

			double cieX = 0.0d;

			double cieY = 0.0d;

			if (this._keyName.Contains(SmartBinning.CIExyKEY))
			{
				cieXKey = this._keyName.Replace("y", "");

				cieYKey = this._keyName.Replace("x", "");

				if (rowData.ContainsKey(cieXKey) && rowData.ContainsKey(cieYKey))
				{
					cieX = rowData[cieXKey];

					cieY = rowData[cieYKey];
				}
				else
				{
					return false;
				}
			}
			else if (this._keyName.Contains(SmartBinning.CIEupvpKEY))
			{
				cieXKey = this._keyName.Replace("CIEupvp", "Uprime");

				cieYKey = this._keyName.Replace("CIEupvp", "Vprime");

				if (rowData.ContainsKey(cieXKey) && rowData.ContainsKey(cieYKey))
				{
					cieX = rowData[cieXKey];

					cieY = rowData[cieYKey];
				}
				else
				{
					return false;
				}
			}
			else
			{
				if (rowData.ContainsKey(this._keyName))
				{
					value = rowData[this._keyName];
				}
				else
				{

                    //20190117 David for Emcore overload test binning
                    value = 0;
                    if (_boundaryList != null && _boundaryList.Count == 1 &&
                        (_boundaryList[0] is SmartLowUp))
                    {
                        SmartLowUp sup = _boundaryList[0] as SmartLowUp;
                        value = (sup.UpLimit + sup.LowLimit) / 2;
                    }
                    else 
                    {
                        return false;
                    }
					//return false;
				}
			}

			foreach (var item in this._boundaryList)
			{
				if (item is SmartLowUp)
				{
                    if (binSortingRule == 4)
                    {
                        if ((item as SmartLowUp).IsInRange(value, item.BoundaryRule))//variour
                        {
                            serilaNimber = item.SerialNumber;

                            return true;
                        }
                    }
                    else
                    {
                        if ((item as SmartLowUp).IsInRange(value, binSortingRule))//原來手法
                        {
                            serilaNimber = item.SerialNumber;

                            return true;
                        }
                    }
				}
				else if (item is SmartPolygon)
				{
					if ((item as SmartPolygon).IsInArea(cieX, cieY))
					{
						serilaNimber = item.SerialNumber;

						return true;
					}

				}
				else if (item is SmartEllipse)
				{
					if ((item as SmartEllipse).IsInArea(cieX, cieY))
					{
						serilaNimber = item.SerialNumber;

						return true;
					}
				}
			}

			return false;
		}

		public bool IsInBoundary(Dictionary<string, double> rowData, int binSortingRule, out string boundaryCode, out string serilaNimber)
		{
			boundaryCode = string.Empty;

			serilaNimber = string.Empty;

			//Find KeyName & raw Value
			double value = 0.0d;

			string cieXKey = string.Empty;

			string cieYKey = string.Empty;

			double cieX = 0.0d;

			double cieY = 0.0d;

			if (this._keyName.Contains(SmartBinning.CIExyKEY))
			{
				cieXKey = this._keyName.Replace("y", "");

				cieYKey = this._keyName.Replace("x", "");

				if (rowData.ContainsKey(cieXKey) && rowData.ContainsKey(cieYKey))
				{
					cieX = rowData[cieXKey];

					cieY = rowData[cieYKey];
				}
				else
				{
					return false;
				}
			}
			else if (this._keyName.Contains(SmartBinning.CIEupvpKEY))
			{
				cieXKey = this._keyName.Replace("CIEupvp", "Uprime");

				cieYKey = this._keyName.Replace("CIEupvp", "Vprime");

				if (rowData.ContainsKey(cieXKey) && rowData.ContainsKey(cieYKey))
				{
					cieX = rowData[cieXKey];

					cieY = rowData[cieYKey];
				}
				else
				{
					return false;
				}
			}
			else
			{
				if (rowData.ContainsKey(this._keyName))
				{
					value = rowData[this._keyName];
				}
				else
				{
                    //20190117 David for Emcore overload test binning
                    value = 0;
                    if (_boundaryList != null && _boundaryList.Count == 1 &&
                        (_boundaryList[0] is SmartLowUp))
                    {
                        SmartLowUp sup = _boundaryList[0] as SmartLowUp;
                        value = (sup.UpLimit + sup.LowLimit) / 2;
                    }
                    else
                    {
                        return false;
                    }
                    //return false;
				}
			}

			foreach (var item in this._boundaryList)
			{
				if (item is SmartLowUp)
				{
					if ((item as SmartLowUp).IsInRange(value, binSortingRule))
					{
						boundaryCode = item.BoundaryCode;

						serilaNimber = item.SerialNumber;

						return true;
					}
				}
				else if (item is SmartPolygon)
				{
					if ((item as SmartPolygon).IsInArea(cieX, cieY))
					{
						boundaryCode = item.BoundaryCode;

						serilaNimber = item.SerialNumber;

						return true;
					}

				}
				else if (item is SmartEllipse)
				{
					if ((item as SmartEllipse).IsInArea(cieX, cieY))
					{
						boundaryCode = item.BoundaryCode;

						serilaNimber = item.SerialNumber;

						return true;
					}
				}
			}

			return false;
		}

		public bool ContainsBoundaryCode(string boundaryCode)
		{
			for (int i = 0; i < this._boundaryList.Count; i++)
			{
				if (boundaryCode == this._boundaryList[i].BoundaryCode)
				{
					return true;
				}
			}

			return false;
		}

		public bool Add(SmartBoundaryBase boundary)
		{
			this._boundaryList.Add(boundary);

			return true;
		}

		public void Clear()
		{
			this._boundaryList.Clear();
		}

		public void Remove(int index)
		{
			if (index >= 0 && index < this._boundaryList.Count)
			{
				this._boundaryList.RemoveAt(index);
			}
		}


		public object Clone()
		{
			SmartBoundaryData cloneObj = new SmartBoundaryData();

			cloneObj._name = this._name;

			cloneObj._keyName = this._keyName;

			cloneObj._format = this._format;

			foreach (var item in this._boundaryList)
			{
				cloneObj._boundaryList.Add(item.DeepClone() as SmartBoundaryBase);
			}

			return cloneObj;
		}

		#endregion

		#region >>> Public Property <<<

		public SmartBoundaryBase this[int Index]
		{
			get
			{
				if (Index >= 0 && Index < this._boundaryList.Count)
				{
					return this._boundaryList[Index];
				}
				else
				{
					return null;
				}
			}

			set
			{
				if (Index >= 0 && Index < this._boundaryList.Count)
				{
					this._boundaryList[Index] = value;

					return;
				}
			}
		}

		public string Name
		{
			get { return this._name; }
			set { { this._name = value; } }
		}

		public string KeyName
		{
			get { return this._keyName; }
			set { { this._keyName = value; } }
		}

		public string Format
		{
			get { return this._format; }
			set { { this._format = value; } }
		}

		public int MaxDataLength
		{
			get 
			{
				int length = 0;

				foreach (var item in this._boundaryList)
				{
					if (item is SmartLowUp && length < 2)
					{
						length = 2;
					}
					else if (item is SmartPolygon)
					{
						int count = (item as SmartPolygon).Coord.Count * 2;

						if (length < count)
						{
							length = count;
						}
					}
					else if (item is SmartEllipse && length < 5)
					{
						length = 5;
					}
				}

				return length;
			}
		}

		public int Count
		{
			get { return this._boundaryList.Count; }
		}

        public EBinBoundaryRule BoundaryRule
        {
            get { return this._boundaryRule; }
        }

		#endregion

		#region >>> IEnumerator Interface <<<

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return (System.Collections.IEnumerator)(new BoundaryBaseEnum(this._boundaryList));
		}

		IEnumerator<SmartBoundaryBase> IEnumerable<SmartBoundaryBase>.GetEnumerator()
		{
			return (IEnumerator<SmartBoundaryBase>)(new BoundaryBaseEnum(this._boundaryList));
		}

		#endregion

		#region >>> BoundaryBaseEnum Class <<<

		private class BoundaryBaseEnum : IEnumerator<SmartBoundaryBase>
		{
			#region >>> Private Property <<<

			private int _position;
			private SmartBoundaryBase _data;
			private List<SmartBoundaryBase> _dataList;

			#endregion

			#region >>> Constructor / Disposor <<<

			public BoundaryBaseEnum(List<SmartBoundaryBase> dataList)
			{
				this._position = -1;

				this._data = default(SmartBoundaryBase);

				this._dataList = dataList;
			}

			#endregion

			#region >>> Interface Property <<<

			object System.Collections.IEnumerator.Current
			{
				get { return Current; }
			}

			#endregion

			#region >>> Public Method <<<

			public SmartBoundaryBase Current
			{
				get { return this._data; }
			}

			public bool MoveNext()
			{
				if (++this._position >= this._dataList.Count)
				{
					return false;
				}
				else
				{
					this._data = this._dataList[this._position];
				}

				return true;
			}

			public void Reset()
			{
				this._position = -1;
			}

			public void Dispose()
			{
			}

			#endregion

		}

		#endregion
	}

	[Serializable]
	public class SmartBoundaryBase
	{
		#region >>> Private Property <<<

		protected object _lockObj;
		private string _boundaryCode;
		private string _serialNumber;
        private EBinBoundaryRule _boundaryRule;
		#endregion

		#region >>> Constructor / Disposor <<<

		public SmartBoundaryBase()
		{
			this._lockObj = new object();


			this._serialNumber = Guid.NewGuid().ToString().Replace("-", "");

            _boundaryRule = EBinBoundaryRule.LeValL;
		}

		#endregion

		#region >>> Public Method <<<

		public SmartBoundaryBase DeepClone()
		{
			SmartBoundaryBase boundary = null;

			if (this is SmartLowUp)
			{
				boundary = new SmartLowUp();

				(boundary as SmartLowUp).LowLimit = (this as SmartLowUp).LowLimit;

				(boundary as SmartLowUp).UpLimit = (this as SmartLowUp).UpLimit;
			}
			else if (this is SmartPolygon)
			{
				boundary = new SmartPolygon();

				(boundary as SmartPolygon).Coord.Clear();

				for (int i = 0; i < (this as SmartPolygon).Coord.Count; i++)
				{
					float x = (this as SmartPolygon).Coord[i].X;

					float y = (this as SmartPolygon).Coord[i].Y;

					(boundary as SmartPolygon).Coord.Add(new PointF(x, y));
				}
			}
			else if (this is SmartEllipse)
			{
				boundary = new SmartEllipse((this as SmartEllipse).Type);

				(boundary as SmartEllipse).X = (this as SmartEllipse).X;

				(boundary as SmartEllipse).Y = (this as SmartEllipse).Y;

				(boundary as SmartEllipse).a = (this as SmartEllipse).a;

				(boundary as SmartEllipse).b = (this as SmartEllipse).b;

				(boundary as SmartEllipse).Theta = (this as SmartEllipse).Theta;
			}
			else
			{
				boundary = new SmartBoundaryBase();
			}

			boundary._boundaryCode = this._boundaryCode;

			boundary._serialNumber = this._serialNumber;

            boundary._boundaryRule = this._boundaryRule;

			return boundary;
		}

		#endregion

		#region >>> Public Property <<<

		public string BoundaryCode
		{
			get { return this._boundaryCode; }
			set { lock (this._lockObj) { this._boundaryCode = value; } }
		}

		public string SerialNumber
		{
			get { return this._serialNumber; }
		}

        public EBinBoundaryRule BoundaryRule
        {
            get { return this._boundaryRule; }
            set { this._boundaryRule = value; }
        }

		#endregion
	}

	[Serializable]
	public class SmartLowUp : SmartBoundaryBase
	{
		#region >>> Private Property <<<

		private double _lowLimit;
		private double _upLimit;

		#endregion

		#region >>> Constructor / Disposor <<<

		public SmartLowUp()
			: base()
		{
			this._lowLimit = 0.0d;
			this._upLimit = 0.0d;
		}

		#endregion

		#region >>> Public Property <<<

		public double LowLimit
		{
			get { return this._lowLimit; }
			set { lock (this._lockObj) { this._lowLimit = value; } }
		}

		public double UpLimit
		{
			get { return this._upLimit; }
			set { lock (this._lockObj) { this._upLimit = value; } }
		}

		#endregion

		#region >>> Public Method <<<

		public bool IsInRange(double value, int BinSortingRule)
		{

            switch(BinSortingRule)
            {
                default:
                case 0:
                    {
                        if (this._lowLimit <= value && value < this._upLimit)
                        {
                            return true;
                        }
                    }
                    break;
                case 1:
                    {
                        if (this._lowLimit <= value && value <= this._upLimit)
                        {
                            return true;
                        }
                    }
                    break;
                case 2:
                    {
                        if (this._lowLimit < value && value <= this._upLimit)
                        {
                            return true;
                        }
                    }
                    break;
                case 3:
                    {
                        if (this._lowLimit < value && value < this._upLimit)
                        {
                            return true;
                        }
                    }
                    break;
            }
			

			return false;
		}

        public bool IsInRange(double value, EBinBoundaryRule BinSortingRule)
        {

            switch (BinSortingRule)
            {
                default:
                case EBinBoundaryRule.LeValL:
                    {
                        if (this._lowLimit <= value && value < this._upLimit)
                        {
                            return true;
                        }
                    }
                    break;
                case EBinBoundaryRule.LeValLe:
                    {
                        if (this._lowLimit <= value && value <= this._upLimit)
                        {
                            return true;
                        }
                    }
                    break;
                case EBinBoundaryRule.LValLe:
                    {
                        if (this._lowLimit < value && value <= this._upLimit)
                        {
                            return true;
                        }
                    }
                    break;
                case EBinBoundaryRule.LValL:
                    {
                        if (this._lowLimit < value && value < this._upLimit)
                        {
                            return true;
                        }
                    }
                    break;
            }


            return false;
        }
		#endregion
	}

	[Serializable]
	public class SmartPolygon : SmartBoundaryBase
	{
		#region >>> Private Property <<<

		private List<PointF> _coord;

		#endregion

		#region >>> Constructor / Disposor <<<

		public SmartPolygon()
			: base()
		{
			this._coord = new List<PointF>();
		}

		#endregion

		#region >>> Public Method <<<

		public void CreadDefaultCoord()
		{
			this._coord.Add(new PointF(0, 0));
			this._coord.Add(new PointF(0, 0));
			this._coord.Add(new PointF(0, 0));
			this._coord.Add(new PointF(0, 0));
		}

		#endregion

		#region >>> Public Property <<<

		public List<PointF> Coord
		{
			get { return this._coord; }
			set { lock (this._lockObj) { this._coord = value; } }
		}

		#endregion

		#region >>> Public Method <<<

		public bool IsInArea(double pX, double pY)
		{
			if (this._coord == null || this._coord.Count <= 2)
			{
				return false;
			}

			float[] X = new float[this._coord.Count + 1];

			float[] Y = new float[this._coord.Count + 1];

			for (int i = 0; i < this._coord.Count; i++)
			{
				X[i] = this._coord[i].X;

				Y[i] = this._coord[i].Y;
			}

			X[this._coord.Count] = this._coord[0].X;

			Y[this._coord.Count] = this._coord[0].Y;

			int cn = 0;

			for (int i = 0; i < this._coord.Count; i++)
			{
				// point at Line
				if (pX == X[i] + (pY - Y[i]) * (X[i + 1] - X[i]) / (Y[i + 1] - Y[i]))
				{
					if (pX >= X[i + 1] && pX <= X[i + 1] || pX <= X[i] && pX >= X[i + 1] &&
						pY >= Y[i] && pY <= Y[i + 1] || pY <= Y[i] && pY >= Y[i + 1])
					{
						return true;
					}
				}

				if (((Y[i] <= pY) && (Y[i + 1] > pY)) || ((Y[i] > pY) && (Y[i + 1] <= pY)))
				{
					double vt = (pY - Y[i]) / (Y[i + 1] - Y[i]);

					if (pX < X[i] + vt * (X[i + 1] - X[i]))
					{
						++cn;
					}
				}
			}

			if (cn % 2 == 0)
			{
				return false;
			}
			else
			{
				return true;
			}
		}

		#endregion
	}

	[Serializable]
	public class SmartEllipse : SmartBoundaryBase
	{
		#region >>> Private Property <<<

		private double _x;
		private double _y;
		private double _a;
		private double _b;
		private double _theta;
		private ECIEType _type;

		#endregion

		#region >>> Constructor / Disposor <<<

		private SmartEllipse()
			: base()
		{
			this._x = 0.0d;

			this._y = 0.0d;

			this._a = 0.0d;

			this._b = 0.0d;

			this._theta = 0.0d;

			this._type = ECIEType.CIE1931;
		}

		public SmartEllipse(ECIEType type)
			: this()
		{
			this._type = type;
		}

		#endregion

		#region >>> Public Method <<<

		public bool IsInArea(double pX, double pY)
		{
			double pointXT, pointYT;

			double thetaT = (this._theta / 180 - (int)(this._theta / 180)) * Math.PI;

			// coordinate transformation
			pointXT = pX - this._x;

			pointYT = pY - this._y;

			double tempX = pointXT;

			pointXT = pointXT * Math.Cos(thetaT) + pointYT * Math.Sin(thetaT);

			pointYT = pointYT * Math.Cos(thetaT) - tempX * Math.Sin(thetaT);

			// Judge
			double judgeValue = Math.Pow(pointXT, 2.0) / Math.Pow(this._a, 2.0) + Math.Pow(pointYT, 2.0) / Math.Pow(this._b, 2.0);

			if (judgeValue > 1)
			{
				return false;
			}
			else
			{
				return true;
			}
		}

		#endregion

		#region >>> Public Property <<<

		public double X
		{
			get { return this._x; }
			set { lock (this._lockObj) { this._x = value; } }
		}

		public double Y
		{
			get { return this._y; }
			set { lock (this._lockObj) { this._y = value; } }
		}

		public double a
		{
			get { return this._a; }
			set { lock (this._lockObj) { this._a = value; } }
		}

		public double b
		{
			get { return this._b; }
			set { lock (this._lockObj) { this._b = value; } }
		}

		public double Theta
		{
			get { return this._theta; }
			set { lock (this._lockObj) { this._theta = value; } }
		}

		public ECIEType Type
		{
			get { return this._type; }
			set { lock (this._lockObj) { this._type = value; } }
		}

		#endregion

		public enum ECIEType
		{
			CIE1931 = 1,
			CIE1976 = 4,
		}
	}
}

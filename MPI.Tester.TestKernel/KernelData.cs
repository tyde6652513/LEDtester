using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MPI.Tester.TestKernel
{
	//-----------------------------------------------------------------------------
	// Generic, parameterized (indexed) "property" template
	//-----------------------------------------------------------------------------
	public class MyProperty<T>
	{
		// The internal property value
		private object _lockObj;
		private T[] _propertyVal;
		private int _length;

		public MyProperty()
		{
			this._lockObj = new object();
			this._propertyVal = new T[1] { default(T) };

		}

		public MyProperty(int length)
			: this()
		{
			this._length = length;
			this._propertyVal = new T[length];

			for (int i = 0; i < length; i++)
			{
				this._propertyVal[i] = default(T);
			}
		}
		// The indexed property get/set accessor 
		// (Property<T>[index] = newvalue; value = Property<T>[index];)
		//public T this[object key]
		//{
		//    get { return _propertyVal; }  // Get the value
		//    set { _propertyVal = value; } // Set the value
		//}

		public T this[uint index]
		{
			get
			{
				if (index >= 0 && index < this._length)
				{
					return this._propertyVal[index];
				}
				else
				{
					return default(T);
				}
			}

			set
			{
				lock (this._lockObj)
				{
					if (index >= 0 && index < this._length)
					{
						this._propertyVal[index] = value;
					}
				}
			}
		}

		public void Clear()
		{
			for (int i = 0; i < this._length; i++)
			{
				this._propertyVal[i] = default(T);
			}
		}
	}

	public class KernelCmdData
	{
		// Parameterized properties
		private object _lockObj;

		private MyProperty<int> _intProp;
		private MyProperty<double> _doubleProp;
		private MyProperty<string> _strProp;
		private int _cmdID;

		public KernelCmdData()
		{
			this._lockObj = new object();

			this._intProp = new MyProperty<int>(1);
			this._doubleProp = new MyProperty<double>(1);
			this._strProp = new MyProperty<string>(1);
			this._cmdID = 0;

		}

		public KernelCmdData(int length)
			: this()
		{
			this._intProp = new MyProperty<int>(length);
			this._doubleProp = new MyProperty<double>(length);
			this._strProp = new MyProperty<string>(length);
		}

		// Parameterized int property accessor for client access
		// (ex: KernelCmdData.IntData[index])
		public MyProperty<int> IntData
		{
			get { return this._intProp; }
		}

		// Parameterized string property accessor for client access
		// (ex: KernelCmdData.StringData[index])
		public MyProperty<string> StringData
		{
			get { return this._strProp; }
		}

		// Parameterized double property accessor for client access
		// (ex: KernelCmdData.DoubleData[index])
		public MyProperty<double> DoubleData
		{
			get { return this._doubleProp; }
		}

		public int CmdID
		{
			get { return this._cmdID; }
			set { lock (this._lockObj) { this._cmdID = value; } }
		}

		#region >>> Public Method <<<

		public void ClearAllData()
		{
			this._cmdID = 0;
			this._intProp.Clear();
			this._doubleProp.Clear();
			this._strProp.Clear();
		}

		#endregion
	}

	public class ProberDataArray
	{
		private object _lockObj;

		private double[] _data;
		private uint _dataLength;

		public ProberDataArray()
		{
			this._lockObj = new object();
			this._data = new double[1];
		}

		public ProberDataArray(uint dataLength)
			: this()
		{
			this._dataLength = dataLength;
			this._data = new double[dataLength];
		}

		public double[] AllData
		{
			get { return this._data; }
			set { lock (this._lockObj) { this._data = value; } }
		}

		public double this[uint index]
		{
			get
			{
				if (index >= 0 && index < this._dataLength)
				{
					return this._data[index];
				}
				else
				{
					return 0.0d;
				}
			}
			set
			{
				if (index >= 0 && index < this._dataLength)
				{
					lock (this._lockObj) { this._data[index] = value; }
				}
			}
		}
	}
	
}

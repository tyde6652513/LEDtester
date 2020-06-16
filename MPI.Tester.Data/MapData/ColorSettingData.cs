using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Drawing;

namespace MPI.Tester.Data
{

	[Serializable]
	public class ColorSetting
	{
		#region >>> Private Property <<<

		private object _lockObj;

		private bool _enableColorOutOfRange;

		private List<ColorSettingData> _dataList;

		#endregion

		#region >>> Constructor / Disposor <<<

		public ColorSetting()
		{
			this._lockObj = new object();

			this._enableColorOutOfRange = false;

			this._dataList = new List<ColorSettingData>();
		}

		#endregion

		#region >>> Private Method <<<

		public bool ContainsKey(string keyName)
		{
			foreach (var item in this._dataList)
			{
				if (item.KeyName == keyName)
				{
					return true;
				}
			}

			return false;
		}

		public bool Remove(string keyName)
		{
			foreach (var item in this._dataList)
			{
				if (item.KeyName == keyName)
				{
					this._dataList.Remove(item);

					return true;
				}
			}

			return false;
		}

		#endregion

		#region >>> Public Property <<<

		public ColorSettingData this[string keyName]
		{
			get
			{
				foreach (var item in this._dataList)
				{
					if (item.KeyName == keyName)
					{
						return item;
					}
				}

				return null;
			}

		}

		public bool EnableColorOutOfRange
		{
			get { return this._enableColorOutOfRange; }
			set { lock (this._lockObj) { this._enableColorOutOfRange = value; } }
		}

		public List<ColorSettingData> DataList
		{
			get { return this._dataList; }
			set { lock (this._lockObj) { this._dataList = value; } }
		}

		#endregion
	}

	[Serializable]
	public class ColorSettingData : ICloneable
	{
		#region >>> Private Property <<<

		public const int MAX_COLOR_LEVEL = 32;

		private object _lockObj;
		private bool _isEnable;
		private string _keyName;
		private string _name;
		private string _formate;
		private float _min;
		private float _step;
		private float _max;
		private string _minColor;
		private string _maxColor;
		private List<ColorLevel> _colorLevelList;

		#endregion

		#region >>> Constructor / Disposor <<<

		public ColorSettingData()
		{
			this._lockObj = new object();
			this._min = 0.0f;
			this._step = 1.0f;
			this._max = ColorSettingData.MAX_COLOR_LEVEL;
			this._minColor = ColorSettingData.ParseColor(Color.White);
			this._maxColor = ColorSettingData.ParseColor(Color.Purple);
			this._colorLevelList = new List<ColorLevel>();
		}

		#endregion

		#region >>> Public static Method <<<

		public static Color ParseColor(string value)
		{
			if (value == null || value.Length == 0)
				return Color.Empty;

			return Color.FromArgb((int)Convert.ToUInt32(value, 16));

		}

		public static string ParseColor(Color value)
		{
			if (value == null)
				return String.Format("{0:X}", value.ToArgb()).ToString();

			return String.Format("{0:X}", value.ToArgb()).ToString();
		}

		#endregion

		#region >>> Public Property <<<

		public float Min
		{
			get { return this._min; }
			set { lock (this._lockObj) { this._min = value; } }
		}

		public float Step
		{
			get { return this._step; }
			set { lock (this._lockObj) { this._step = value; } }
		}

		public float Max
		{
			get { return this._max; }
			set { lock (this._lockObj) { this._max = value; } }
		}

		public string MinColor
		{
			get { return this._minColor; }
			set { lock (this._lockObj) { this._minColor = value; } }
		}

		public string MaxColor
		{
			get { return this._maxColor; }
			set { lock (this._lockObj) { this._maxColor = value; } }
		}

		public bool IsEnable
		{
			get { return this._isEnable; }
			set { lock (this._lockObj) { this._isEnable = value; } }
		}

		public string KeyName
		{
			get { return this._keyName; }
			set { lock (this._lockObj) { this._keyName = value; } }
		}

		public string Name
		{
			get { return this._name; }
			set { lock (this._lockObj) { this._name = value; } }
		}

		public string Formate
		{
			get { return this._formate; }
			set { lock (this._lockObj) { this._formate = value; } }
		}

		public List<ColorLevel> ColorLevelList
		{
			get { return this._colorLevelList; }
			set { lock (this._lockObj) { this._colorLevelList = value; } }
		}

		public object Clone()
		{
			ColorSettingData data = this.MemberwiseClone() as ColorSettingData;

			data.ColorLevelList = new List<ColorLevel>();

			foreach (var item in this._colorLevelList)
			{
				data.ColorLevelList.Add(item.Clone() as ColorLevel);
			}

			return data;
		}

		#endregion
	}

	[Serializable]
	public class ColorLevel : ICloneable
	{
		#region >>> Private Property <<<

		private object _lockObj;
		private float _levelValue;
		private string _levelcolor;

		#endregion

		#region >>> Constructor / Disposor <<<

		public ColorLevel()
		{
			this._lockObj = new object();
			this._levelValue = 0.0f;
			this._levelcolor = "";
		}

		public ColorLevel(float LevelValue, string Levelcolor)
			: this()
		{
			this._levelValue = LevelValue;
			this._levelcolor = Levelcolor;
		}

		#endregion

		#region >>> Public Property <<<

		public float LevelValue
		{
			get { return this._levelValue; }
			set { lock (this._lockObj) { this._levelValue = value; } }
		}

		public string Levelcolor
		{
			get { return this._levelcolor; }
			set { lock (this._lockObj) { this._levelcolor = value; } }
		}

		public object Clone()
		{
			return this.MemberwiseClone();
		}

		#endregion
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Collections;
using System.Runtime.Serialization;

namespace MPI.Tester.Data
{
	[Serializable]
	public class AutoColorFilterSpec : ICloneable
	{
        private int _updateChipCount;
		private bool _isAutoBinColorGrade;
		private bool _isEnableSkipBadDie;
		private bool _isEnableNormalStatistic;
        private bool _isEnableAutoStartPDFfile;
        private bool _isEnableChannelGainSetting;
		private bool _statisticAvgFilterChecked;
		private bool _statisticMaxMinFilterChecked;
		private bool _statisticStdevFilterChecked;        
		private double _statisticAvgUpLimit;
		private double _statisticAvgDownLimit;
		private double _statisticMaxLimit;
		private double _statisticMinLimit;
		private double _statisticStdevUpLimit;
		private double _statisticStdevDownLimit;
		private List<AutoColorFilterData> _dataList;

		public AutoColorFilterSpec()
		{
            this._updateChipCount = 1;

			this._isEnableSkipBadDie = false;

			this._isEnableNormalStatistic = false;

			this._isAutoBinColorGrade = false;

            this._isEnableAutoStartPDFfile = false;

            this._isEnableChannelGainSetting = false;

			this._statisticAvgFilterChecked = false;

			this._statisticMaxMinFilterChecked = false;

			this._statisticStdevFilterChecked = false;

			this._statisticAvgUpLimit = 95;

			this._statisticAvgDownLimit = 5;

			this._statisticMaxLimit = 95;

			this._statisticMinLimit = 5;

			this._statisticStdevUpLimit = 95;

			this._statisticStdevDownLimit = 5;

			this._dataList = new List<AutoColorFilterData>();
		}

		public bool IsHaveAutoColorItem
		{
			get
			{
				foreach (var item in this._dataList)
				{
					if (item.IsAutoCalculateItem)
					{
						return true;
					}
				}

				return false;
			}
		}

		public List<AutoColorFilterData> DataList
		{
			get { return this._dataList; }
		}

		public AutoColorFilterData this[string keyName]
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

        public int UpdateChipCount
        {
            get { return this._updateChipCount; }
            set { this._updateChipCount = value; if (this._updateChipCount < 1) this._updateChipCount = 1; }
        }

		public bool IsAutoBinColorGrade
		{
			get { return this._isAutoBinColorGrade; }
			set { this._isAutoBinColorGrade = value; }
		}

		public bool IsEnableSkipBadDie
		{
			get { return this._isEnableSkipBadDie; }
			set { this._isEnableSkipBadDie = value; }
		}

		public bool IsEnableNormalStatistic
		{
			get { return this._isEnableNormalStatistic; }
			set { this._isEnableNormalStatistic = value; }
		}

        public bool IsEnableChannelGainSetting
        {
            get { return this._isEnableChannelGainSetting; }
            set { this._isEnableChannelGainSetting = value; }
        }

        public bool IsEnableAutoStartPDFfile
        {
            get { return this._isEnableAutoStartPDFfile; }
            set { this._isEnableAutoStartPDFfile = value; }
        }

		public bool StatisticAvgFilterChecked
		{
			get { return this._statisticAvgFilterChecked; }
			set { this._statisticAvgFilterChecked = value; }
		}

		public bool StatisticMaxMinFilterChecked
		{
			get { return this._statisticMaxMinFilterChecked; }
			set { this._statisticMaxMinFilterChecked = value; }
		}

		public bool StatisticStdevFilterChecked
		{
			get { return this._statisticStdevFilterChecked; }
			set { this._statisticStdevFilterChecked = value; }
		}       

		public double StatisticAvgUpLimit
		{
			get { return this._statisticAvgUpLimit; }
			set { this._statisticAvgUpLimit = value; }
		}

		public double StatisticAvgDownLimit
		{
			get { return this._statisticAvgDownLimit; }
			set { this._statisticAvgDownLimit = value; }
		}

		public double StatisticMaxLimit
		{
			get { return this._statisticMaxLimit; }
			set { this._statisticMaxLimit = value; }
		}

		public double StatisticMinLimit
		{
			get { return this._statisticMinLimit; }
			set { this._statisticMinLimit = value; }
		}

		public double StatisticStdevUpLimit
		{
			get { return this._statisticStdevUpLimit; }
			set { this._statisticStdevUpLimit = value; }
		}

		public double StatisticStdevDownLimit
		{
			get { return this._statisticStdevDownLimit; }
			set { this._statisticStdevDownLimit = value; }
		}       

		public List<string> EnableItems
		{
			get
			{
				List<string> items = new List<string>();

				foreach (var item in this._dataList)
				{
					if (item.IsEnable)
					{
						items.Add(item.KeyName);
					}
				}

				return items;
			}
		}

		public List<string> SkipBadDieItems
		{
			get
			{
				List<string> items = new List<string>();

				foreach (var item in this._dataList)
				{
					if (item.IsSkipBadDieItem)
					{
						items.Add(item.KeyName);
					}
				}

				return items;
			}
		}

		public List<string> NormalStatisticItems
		{
			get
			{
				List<string> items = new List<string>();

				foreach (var item in this._dataList)
				{
					if (item.IsNormalStatisticItem)
					{
						items.Add(item.KeyName);
					}
				}

				return items;
			}
		}

		public bool ContainsKey(string keyName)
		{
			for (int i = 0; i < this._dataList.Count; i++)
			{
				if (this._dataList[i].KeyName == keyName)
				{
					return true;
				}
			}

			return false;
		}

		public void Remove(string keyName)
		{
			for (int i = 0; i < this._dataList.Count; i++)
			{
				if (this._dataList[i].KeyName == keyName)
				{
					this._dataList.RemoveAt(i);

					return;
				}
			}
		}

		public object Clone()
		{
			AutoColorFilterSpec cloneObj = new AutoColorFilterSpec();

			foreach (var item in this._dataList)
			{
				cloneObj._dataList.Add(item.Clone() as AutoColorFilterData);
			}

            cloneObj._updateChipCount = this._updateChipCount;

			cloneObj._isAutoBinColorGrade = this._isAutoBinColorGrade;

			cloneObj._isEnableSkipBadDie = this._isEnableSkipBadDie;

			cloneObj._isEnableNormalStatistic = this._isEnableNormalStatistic;

			cloneObj._statisticAvgFilterChecked = this._statisticAvgFilterChecked;

			cloneObj._statisticMaxMinFilterChecked = this._statisticMaxMinFilterChecked;

			cloneObj._statisticStdevFilterChecked = this._statisticStdevFilterChecked;

			cloneObj._statisticAvgUpLimit = this._statisticAvgUpLimit;

			cloneObj._statisticAvgDownLimit = this._statisticAvgDownLimit;

			cloneObj._statisticMaxLimit = this._statisticMaxLimit;

			cloneObj._statisticMinLimit = this._statisticMinLimit;

			cloneObj._statisticStdevUpLimit = this._statisticStdevUpLimit;

			cloneObj._statisticStdevDownLimit = this._statisticStdevDownLimit;           

            cloneObj._isEnableAutoStartPDFfile = this._isEnableAutoStartPDFfile;

			return cloneObj;
		}
	}

	[Serializable]
	public class AutoColorFilterData : ICloneable
	{
		private string _keyName;
		private string _name;
        private string _direction;

		private bool _isEnable;
		private bool _isSkipBadDieItem;
		private bool _isNormalStatisticItem;
        private bool _isAutoCalculateItem;        
        private bool _isAvgRangeMode;
        private bool _isChanelGainSetting;
        private bool _isAllRange;

		private double _skipBadDieMin;
        private double _skipBadDieMax;
        private double _autoCalculateMin;
        private double _autoCalculateMax;
        private double _chGainSetting;

        private int _chNumSetting;        
        private int _startPosX;
        private int _startPosY;
        private int _endPosX;
        private int _endPosY;

		public AutoColorFilterData()
		{
			this._keyName = string.Empty;
			this._name = string.Empty;
			this._skipBadDieMin = 0.0d;
            this._skipBadDieMax = 0.0d;
			this._isEnable = false;
			this._isSkipBadDieItem = false;
			this._isNormalStatisticItem = false;
            this._chGainSetting = 0.0d;
            this._chNumSetting = 0;
            this._startPosX = 0;
            this._startPosY = 0;
            this._endPosX = 0;
            this._endPosY = 0;
            this._direction = "Left To Right";
		}

		public AutoColorFilterData(string keyName, string name)
			: this()
		{
			this._keyName = keyName;

			this._name = name;
		}

		public string KeyName
		{
			get { return this._keyName; }
			set { this._keyName = value; }
		}

		public string Name
		{
			get { return this._name; }
			set { this._name = value; }
		}

        public string Direction
        {
            get { return this._direction; }
            set { this._direction = value; }
        }

		public bool IsEnable
		{
			get { return this._isEnable; }
			set { this._isEnable = value; }
		}

        public double SkipBadDieMin
		{
			get { return this._skipBadDieMin; }
			set { this._skipBadDieMin = value; }
		}

        public double SkipBadDieMax
		{
            get { return this._skipBadDieMax; }
            set { this._skipBadDieMax = value; }
		}

        public double AutoCalculateMin
        {
            get { return this._autoCalculateMin; }
            set { this._autoCalculateMin = value; }
        }

        public double AutoCalculateMax
        {
            get { return this._autoCalculateMax; }
            set { this._autoCalculateMax = value; }
        }

        public int ChannelNumSetting
        {
            get { return this._chNumSetting; }
            set { this._chNumSetting = value; }
        }

        public double ChannelGainSetting
        {
            get { return this._chGainSetting; }
            set { this._chGainSetting = value; }
        }

        public int StartPosX
        {
            get { return this._startPosX; }
            set { this._startPosX = value; }
        }

        public int StartPosY
        {
            get { return this._startPosY; }
            set { this._startPosY = value; }
        }

        public int EndPosX
        {
            get { return this._endPosX; }
            set { this._endPosX = value; }
        }

        public int EndPosY
        {
            get { return this._endPosY; }
            set { this._endPosY = value; }
        }

		public bool IsSkipBadDieItem
		{
			get { return this._isSkipBadDieItem; }
			set { this._isSkipBadDieItem = value; }
		}

		public bool IsNormalStatisticItem
		{
			get { return this._isNormalStatisticItem; }
			set { this._isNormalStatisticItem = value; }
		}

        public bool IsAutoCalculateItem
        {
            get { return this._isAutoCalculateItem; }
            set { this._isAutoCalculateItem = value; }
        }

        public bool IsChanelGainSetting
        {
            get { return this._isChanelGainSetting; }
            set { this._isChanelGainSetting = value; }
        }

        public bool IsAvgRangeMode
        {
            get { return this._isAvgRangeMode; }
            set { this._isAvgRangeMode = value; }
        }

        public bool IsAllRange
        {
            get { return this._isAllRange; }
            set { this._isAllRange = value; }
        }

		public object Clone()
		{
			AutoColorFilterData filterData = new AutoColorFilterData();

			filterData._keyName = this._keyName;

			filterData._name = this._name;

			filterData._isEnable = this._isEnable;

			filterData._skipBadDieMin = this._skipBadDieMin;

            filterData._skipBadDieMax = this._skipBadDieMax;

            filterData._autoCalculateMax = this._autoCalculateMax;

            filterData._autoCalculateMin = this._autoCalculateMin;

            filterData._chGainSetting = this._chGainSetting;

            filterData._chNumSetting = this._chNumSetting;

			filterData._isEnable = this._isEnable;

			filterData._isSkipBadDieItem = this._isSkipBadDieItem;

			filterData._isNormalStatisticItem = this._isNormalStatisticItem;

            filterData._isAutoCalculateItem = this._isAutoCalculateItem;            

            filterData._isAvgRangeMode = this._isAvgRangeMode;

            filterData._isChanelGainSetting = this._isChanelGainSetting;

            filterData._startPosX = this._startPosX;

            filterData._startPosY = this._startPosY;

            filterData._endPosX = this._endPosX;

            filterData._endPosY = this._endPosY;

            filterData._direction = this._direction;

            filterData._isAllRange = this._isAllRange;

			return filterData;
		}
	}
}

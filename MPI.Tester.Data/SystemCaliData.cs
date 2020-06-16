using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using MPI.Xml;

using MPI.Tester.DeviceCommon;

namespace MPI.Tester.Data
{
	public class SystemCali : ICloneable
	{
        private SystemCaliData _data;       

		public SystemCali()
		{
			this._data = new SystemCaliData();
            
		}

        #region >>> Public Property <<<
        public SystemCaliData SystemCaliData
        {
            get { return this._data; }
            set { this._data = value; }
        }

        #endregion

        #region >>> Public Method <<<

        public bool Save(string fullFileName)
		{
			if (!Directory.Exists(Path.GetDirectoryName(fullFileName)))
			{
				Directory.CreateDirectory(Path.GetDirectoryName(fullFileName));
			}

			try
			{
				XmlFileSerializer.Serialize(this._data, fullFileName);

				return true;
			}
			catch
			{
				return false;
			}
		}

		public bool Open(string fullFileName)
		{
			if (!File.Exists(fullFileName))
			{
				return false;
			}

			try
			{
				this._data = XmlFileSerializer.Deserialize(typeof(SystemCaliData), fullFileName) as SystemCaliData;

				if (this._data == null)
				{
					return false;
				}
				else
				{
					return true;
				}
			}
			catch
			{
				return false;
			}
		}

		public object Clone()
		{
			SystemCali cloneObj = new SystemCali();

			cloneObj._data = this._data.Clone() as SystemCaliData;

            //cloneObj._lcrCaliData = this._lcrCaliData.Clone() as LCRCaliData;

			return cloneObj;
        }

        #endregion
    }


	[Serializable]
	public class SystemCaliData : ICloneable
	{
        private List<GainOffsetData> _dataList;

        private LCRCaliData _lcrCaliData;

		public SystemCaliData()
		{
            this._dataList = new List<GainOffsetData>();

            this._lcrCaliData = new LCRCaliData();
		}

        #region >>> Public Property <<<

        public List<GainOffsetData> ToolFactor
        {
            get { return this._dataList; }
            set { this._dataList = value; }
        }

        public GainOffsetData this[string keyName]
        {
            get
            {
                if (this._dataList == null || this._dataList.Count == 0)
                {
                    return null;
                }

                foreach (GainOffsetData item in this._dataList)
                {
                    if (item.KeyName == keyName)
                    {
                        return item;
                    }
                }

                return null;
            }
        }

        public LCRCaliData LCRCaliData
        {
            get { return this._lcrCaliData; }
            set { this._lcrCaliData = value;  }
        }

        #endregion

        #region >>> Public Method <<<

        public bool ContainsKey(string keyName)
        {
            if (this._dataList == null || this._dataList.Count == 0)
            {
                return false;
            }

            foreach (var data in this._dataList)
            {
                if (data.KeyName == keyName)
                {
                    return true;
                }
            }

            return false;
        }

        public object Clone()
		{
            SystemCaliData cloneObj = new SystemCaliData();

            cloneObj._dataList = new List<GainOffsetData>();

            foreach (GainOffsetData item in this._dataList)
            {
                cloneObj._dataList.Add(item.Clone() as GainOffsetData);
            }

			return cloneObj;
        }

        #endregion
    }


}

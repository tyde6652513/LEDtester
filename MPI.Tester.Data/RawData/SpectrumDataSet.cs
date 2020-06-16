using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Collections;

namespace MPI.Tester.Data
{
    public class SpectrumDataSet : ICloneable, IEnumerable<SpectrumData>
    {
        #region >>> Private Property <<<

        private object _lockObj;
        private List<SpectrumData> _dataList;
        private int _channelCount;
        #endregion

        #region >>> Constructor / Disposor <<<

        public SpectrumDataSet()
        {
            this._lockObj = new object();

            this._dataList = new List<SpectrumData>();
        }

        #endregion

        #region >>> Public Property <<<

        public SpectrumData this[uint channel, string keyName]
        {
            get
            {
                foreach (var item in this._dataList)
                {
                    if (item.KeyName == keyName && item.Channel == channel)
                    {
                        return item;
                    }
                }

                return null;
            }

            set
            {
                lock (this._lockObj)
                {
                    for (int i = 0; i < this._dataList.Count; i++)
                    {
                        if (this._dataList[i].KeyName == keyName)
                        {
                            this._dataList[i].Channel = channel;
                            
                            this._dataList[i] = value;
                        }
                    }
                }
            }
        }

        public SpectrumData this[int index]
        {
            get { return this._dataList[index]; }
        }

        public string[] Keys
        {
            get
            {
                string[] keys = new string[this._dataList.Count];

                for (int i = 0; i < this._dataList.Count; i++)
                {
                    keys[i] = this._dataList[i].KeyName;
                }

                return keys;
            }
        }

        public int ChannelCount
        {
            get { return this._channelCount; }
            set { lock (_lockObj) { this._channelCount = value; } }
        }

        public int Count
        {
            get { return this._dataList.Count; }
        }

        #endregion

        #region >>> Public Method <<<

        public void Add(SpectrumData data)
        {
            this._dataList.Add(data);
        }

        public void Clear()
        {
            this._dataList.Clear();
        }

        public void Overwrite(SpectrumDataSet data)
        {
            for (int i = 0; i < this._dataList.Count; i++)
            {
                if (this._dataList[i].IsEnable)
                {
                    data[i].Channel = this._dataList[i].Channel;

                    data[i].IsEnable = this._dataList[i].IsEnable;

                    data[i].KeyName = this._dataList[i].KeyName;
                    
                    data[i].Name = this._dataList[i].Name;

                    //-----------------------------------------------------------------------------
                    if (this._dataList[i].Wavelength.Length != data[i].Wavelength.Length)
                    {
                        data[i].Wavelength = new double[this._dataList[i].Wavelength.Length];
                    }

                    Array.Copy(this._dataList[i].Wavelength, data[i].Wavelength, this._dataList[i].Wavelength.Length);

                    //-----------------------------------------------------------------------------
                    if (this._dataList[i].Intensity.Length != data[i].Intensity.Length)
                    {
                        data[i].Intensity = new double[this._dataList[i].Intensity.Length];
                    }

                    Array.Copy(this._dataList[i].Intensity, data[i].Intensity, this._dataList[i].Intensity.Length);

                    //-----------------------------------------------------------------------------
                    if (this._dataList[i].Absoluate.Length != data[i].Absoluate.Length)
                    {
                        data[i].Absoluate = new double[this._dataList[i].Absoluate.Length];
                    }

                    Array.Copy(this._dataList[i].Absoluate, data[i].Absoluate, this._dataList[i].Absoluate.Length);

                    //-----------------------------------------------------------------------------
                    if (this._dataList[i].Dark.Length != data[i].Dark.Length)
                    {
                        data[i].Dark = new double[this._dataList[i].Dark.Length];
                    }

                    Array.Copy(this._dataList[i].Dark, data[i].Dark, this._dataList[i].Dark.Length);
                }
            }
        }

        public object Clone()
        {
            SpectrumDataSet cloneObj = new SpectrumDataSet();

            cloneObj._channelCount = this._channelCount;

            foreach (var item in this._dataList)
            {
                cloneObj._dataList.Add(item.Clone() as SpectrumData);
            }

            return cloneObj;
        }

        #endregion

        #region >>> IEnumerator Interface <<<

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)(new SpectrumDataEnum(this._dataList));
        }

        IEnumerator<SpectrumData> IEnumerable<SpectrumData>.GetEnumerator()
        {
            return (IEnumerator<SpectrumData>)(new SpectrumDataEnum(this._dataList));
        }

        #endregion

        #region >>> SpectrumDataEnum Class <<<

        private class SpectrumDataEnum : IEnumerator<SpectrumData>
        {
            #region >>> Private Property <<<

            private int _position;
            private SpectrumData _data;
            private List<SpectrumData> _dataList;

            #endregion

            #region >>> Constructor / Disposor <<<

            public SpectrumDataEnum(List<SpectrumData> dataList)
            {
                this._position = -1;

                this._data = default(SpectrumData);

                this._dataList = dataList;
            }

            #endregion

            #region >>> Interface Property <<<

            object IEnumerator.Current
            {
                get { return Current; }
            }

            #endregion

            #region >>> Public Method <<<

            public SpectrumData Current
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
}

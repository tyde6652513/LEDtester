using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using MPI.Tester;
using MPI.Tester.Tools;
using MPI.Tester.Data;

namespace MPI.Tester.Tools
{
    public class GainAndFilterDara
    {
        public GainAndFilterDara(string key)
        {
            _key = key;
        }

        public FilterData _filterData {set;get;}
        public GainOffset _gainData { set; get; }
        public string _key { set; get; } 
    }

    public class ChannelCompCore
    {
        protected Dictionary<string, string> dicStdTestData = new Dictionary<string, string>();

        protected Dictionary<string, string> dicMsrtTestData = new Dictionary<string, string>();

        protected Dictionary<string, List<string>> dicCompare = new Dictionary<string, List<string>>();

        protected Dictionary<string, List<string>> dicByChannelCompare = new Dictionary<string, List<string>>();

        protected Dictionary<string, List<double>> dicChannelGain = new Dictionary<string, List<double>>();

        protected Dictionary<string, List<double>> dicChannelOffSet = new Dictionary<string, List<double>>();

        protected Dictionary<string, double> dicChannelGainCompareResult = new Dictionary<string, double>();

        protected Dictionary<string, double> dicChannelOffSetCompareResult = new Dictionary<string, double>();

        //　Channel Data。

        protected CalcGainOffset[] _channelGainOffset = null;

        protected Dictionary<string, List<double>> dicChStandard = new Dictionary<string, List<double>>();

        protected Dictionary<string, List<double>> dicChMeasure = new Dictionary<string, List<double>>();

        protected string[] _stdTitle;

        protected string[] _msrtTitle;

        protected List<double> channelGainData;

        protected List<double> channelOffSetData;

        private static string[] SYS_RESULT_ITEM_NAME = new string[13] 
        {  
            "Device Number","X (Chip)","Y (Chip)","X (Optical)","Y (Optical)","Theta (Optical)","X (AOI)","Y (AOI)","P/F (Optical)","Bin (Optical)","Cat (Optical)","Mark(AOI)","Cat(AOI)"
        };

        protected Dictionary<string, int> dictTitleIndex = new Dictionary<string, int>();

        private Dictionary<string, GainAndFilterDara> _filterDic = new Dictionary<string, GainAndFilterDara>();

        private List<string> _availableItemName = new List<string>();

        public ChannelCompCore()
        {

        }

        #region >>> Public Methods <<<

        public virtual void LoadStdFile(string path)
        {
            StreamReader sr = new StreamReader(path, Encoding.Default);

            bool isRawData = false;

            dicStdTestData.Clear();

            while (sr.Peek() >= 0)
            {
                string line = sr.ReadLine();

                string[] rawData = line.Split(',');

                if (rawData[0] == CsvQuoteMark.PlusMark("Device Number"))
                {
                    isRawData = true;

                    _stdTitle = EscapeArrayQuoteMark(rawData);

                    continue;
                }
                else
                {

                }

                if (isRawData)
                {
                    string colrowKey = CsvQuoteMark.Unescape(rawData[1]) + "_" + CsvQuoteMark.Unescape(rawData[2]);
                    //=============================
                    // 2014.11.12 
                    // 修正Row/Col 重複讀檔案造成錯誤，在此只讀最後一筆Row/Col資料。
                    //=============================

                    string ds = string.Empty;

                    for (int i = 0; i < rawData.Length; i++)
                    {
                        ds += CsvQuoteMark.Unescape(rawData[i]);

                        if (i != rawData.Length - 1)
                        {
                            ds += ",";
                        }
                    }

                    if (dicStdTestData.ContainsKey(colrowKey))
                    {
                        dicStdTestData[colrowKey] = ds;
                    }
                    else
                    {
                        dicStdTestData.Add(colrowKey, ds);
                    }
                }
            }
        }

        public virtual void LoadMsrtFile(string path)
        {
            StreamReader sr = new StreamReader(path, Encoding.Default);

            bool isRawData = false;

            dicMsrtTestData.Clear();

            while (sr.Peek() >= 0)
            {
                string line = sr.ReadLine();

                string[] rawData = line.Split(',');

                if (rawData[0] == CsvQuoteMark.PlusMark("Device Number"))
                {
                    isRawData = true;

                    _msrtTitle = EscapeArrayQuoteMark(rawData);

                }
                else
                {
                    //continue;
                }

                if (isRawData)
                {
                    string colrowKey = CsvQuoteMark.Unescape(rawData[1]) + "_" + CsvQuoteMark.Unescape(rawData[2]);
                    //=============================
                    //=============================
                    // 2014.11.12 
                    // 修正Row/Col 重複讀檔案造成錯誤，在此只讀最後一筆Row/Col資料。
                    //=============================

                    string ds = string.Empty;

                    for (int i = 0; i < rawData.Length; i++)
                    {
                        ds += CsvQuoteMark.Unescape(rawData[i]);

                        if (i != rawData.Length - 1)
                        {
                            ds += ",";
                        }
                    }

                    if (dicMsrtTestData.ContainsKey(colrowKey))
                    {
                        dicMsrtTestData[colrowKey] = ds;
                    }
                    else
                    {
                        dicMsrtTestData.Add(colrowKey, ds);
                    }
                }
            }

            // dicMsrtTestDataTable.Add(dicMsrtTestData);
        }

        public virtual void SortingByChannel(int maxCH, GainAndFilterDara item, string normalizeCH)
        {
            this._channelGainOffset = new CalcGainOffset[maxCH];

            int channelIndex = -1;

            int itemHeaderIndex = -1;

            double channelGain = -1;

            double channelOffSet = -1;

            if (dictTitleIndex.ContainsKey("CH"))
            {
                channelIndex = dictTitleIndex["CH"];
            }
            else
            {
                return;
            }

            string itemHeader = item._key;

            if (dictTitleIndex.ContainsKey(itemHeader))
            {
                itemHeaderIndex = dictTitleIndex[itemHeader];
            }

            this.dicChannelGain.Clear();

            this.dicChannelOffSet.Clear();

            dicChStandard.Clear();

            dicChMeasure.Clear();

            for (int ch = 0; ch < maxCH; ch++)
            {
                this.channelGainData = new List<double>();

                this.channelOffSetData = new List<double>();

                List<double> standardList = new List<double>();

                List<double> measureList = new List<double>();

                foreach (var comp in dicCompare)
                {
                    string[] std = comp.Value[0].Split(',');

                    string[] mrst = comp.Value[1].Split(',');

                    string sChannel = std[channelIndex];

                    string mChannel = mrst[channelIndex];

                    if (sChannel == "" || mChannel == "")
                    {
                        continue;
                    }

                    int channel = -1;

                    int.TryParse(mChannel, out channel);

                    if (channel <= 0)
                    {
                        continue;
                    }

                    channel = channel - 1;

                    if (channel == ch)
                    {
                        double stdValue = 0.0d;

                        double msrtValue = 0.0d;

                        double.TryParse(std[itemHeaderIndex], out stdValue);

                        double.TryParse(mrst[itemHeaderIndex], out msrtValue);

                        standardList.Add(stdValue);

                        measureList.Add(msrtValue);

                        channelGain = double.Parse(std[itemHeaderIndex]) / double.Parse(mrst[itemHeaderIndex]);

                        channelOffSet = double.Parse(std[itemHeaderIndex]) - double.Parse(mrst[itemHeaderIndex]);

                        this.channelGainData.Add(channelGain);

                        this.channelOffSetData.Add(channelOffSet);
                    }
                }

                CalcGainOffset calcGainOffset = new CalcGainOffset((ch+1).ToString(), "CH" + (ch+1).ToString());

                calcGainOffset.Format = "0.00000";

                calcGainOffset.RunCalculate(measureList.ToArray(), standardList.ToArray(), item._gainData.CalcType);

                calcGainOffset.CalcType = item._gainData.CalcType;

                this._channelGainOffset[ch] = calcGainOffset;

                if (channelGainData.Count != 0)
                {
                    this.dicChannelGain.Add(ch.ToString(), this.channelGainData);

                    this.dicChannelOffSet.Add(ch.ToString(), this.channelOffSetData);

                    dicChStandard.Add(ch.ToString(), standardList);

                    dicChMeasure.Add(ch.ToString(), measureList);
                }
            }

            int normalizeCHIndex= int.Parse(normalizeCH);

            this.normalizeCH(normalizeCHIndex);

          //  normalizeCH(normalizeCHIndex);

           // ChannelGainComp(item._gainData.CalcType,normalizeCH);
        }

        private void normalizeCH(int normalizeCH)
        {
            normalizeCH = normalizeCH - 1;

            double baseGain = this._channelGainOffset[normalizeCH].Gain;

            double baseOffset = this._channelGainOffset[normalizeCH].Offset;

            for (int ch = 0; ch < this._channelGainOffset.Length; ch++)
            {
                if (this._channelGainOffset[ch].CalcType == EGainOffsetType.Gain)
                {
                    this._channelGainOffset[ch].Gain = this._channelGainOffset[ch].Gain / baseGain;
                }
                else if (this._channelGainOffset[ch].CalcType == EGainOffsetType.Offset)
                {
                    this._channelGainOffset[ch].Offset = this._channelGainOffset[ch].Offset -baseOffset;
                }
                else
                {

                }
            }
        }

        public virtual void CompareData()
        {
            this.CreateAvaiableGainAndFilterData();

            if (this._stdTitle == null)
            {
                return;
            }

            dicCompare.Clear();

            foreach (var data in dicStdTestData)
            {
                if (dicMsrtTestData.ContainsKey(data.Key))
                {
                    List<string> values = new List<string>(2);

                    values.Add(data.Value);

                    values.Add(dicMsrtTestData[data.Key]);

                    dicCompare.Add(data.Key, values);
                }
            }

            dictTitleIndex.Clear();

            int index = 0;

            foreach (string s in this._stdTitle)
            {
                if (dictTitleIndex.ContainsKey(s))
                {
                    dictTitleIndex[s] = index;
                }
                else
                {
                    dictTitleIndex.Add(s, index);
                }
                index++;
            }
        }

        public void Filter()
        {
            foreach (var fd in this._filterDic)
            {
                if (fd.Value._filterData.IsEnable)
                {
                    Filter(fd.Value._filterData.Name, fd.Value._filterData.Min, fd.Value._filterData.Max);
                }
            }
        }

        public double[] GetItem(string name)
        {
            if (this.dictTitleIndex.ContainsKey(name))
            {
                int titleindex = this.dictTitleIndex[name];

                List<double> data = new List<double>();

                int index = 0;

                foreach (var comp in dicCompare)
                {
                    string[] s = comp.Value[0].Split(',');

                    string sData = s[titleindex];

                    double sValue = 0.0d;

                    double.TryParse(sData, out sValue);

                    data.Add(sValue);
                }
                return data.ToArray();
            }

            return null;
        }

        #endregion

        #region >>> Private Methods <<<

        private void CreateAvaiableGainAndFilterData()
        {
            this._availableItemName.Clear();

            foreach (string s in this._stdTitle)
            {
                string sname = CsvQuoteMark.Unescape(s);

                if (!SYS_RESULT_ITEM_NAME.Contains(sname) && sname != "P/F" && sname != "NULL")
                {
                    this._availableItemName.Add(sname);
                }
            }

            this._filterDic.Clear();

            foreach (string s in this._availableItemName)
            {
                FilterData fd = new FilterData(s, s);

                GainOffset gd = new GainOffset(s, s);

                if (s.Contains("IR"))
                {
                    fd.IsEnable = true;

                    fd.Min = 0;

                    fd.Max = 1;

                    gd.CalcType = EGainOffsetType.Offset;
                }
                else if (s.Contains("PO(mW)"))
                {
                    fd.IsEnable = true;

                    fd.Min = 10;

                    fd.Max = 9999;

                    gd.CalcType = EGainOffsetType.Gain;
                }
                else if (s.Contains("WD"))
                {
                    fd.IsEnable = true;

                    fd.Min = 380;

                    fd.Max = 780;

                    gd.CalcType = EGainOffsetType.Offset;
                }
                else
                {
                    fd.IsEnable = false;

                    fd.Min = 0;

                    fd.Max = 9999;

                    gd.CalcType = EGainOffsetType.Offset;
                }


                GainAndFilterDara data=new GainAndFilterDara(s);

                data._filterData=fd;

                data._gainData=gd;

                this._filterDic.Add(s, data);
            }
        }

        private void Filter(string name, double minvalue, double maxvalue)
        {
            int filterIndex = -1;

            for (int i = 0; i < this._stdTitle.Length; i++)
            {
                if (CsvQuoteMark.Unescape(this._stdTitle[i]).Contains(name))
                {
                    filterIndex = i;
                }
            }

            if (filterIndex < 0)
            {
                return;
            }

            List<string> removeKey = new List<string>();

            foreach (var comp in dicCompare)
            {
                string[] s = comp.Value[0].Split(',');

                string[] m = comp.Value[1].Split(',');

                string sData = s[filterIndex];

                string mData = m[filterIndex];

                double sValue = 0;

                double mValue = 0;

                double.TryParse(sData, out sValue);

                double.TryParse(mData, out mValue);

                if (sValue < minvalue || mValue < minvalue ||
                    sValue > maxvalue || mValue > maxvalue)
                {
                    removeKey.Add(comp.Key);
                }
            }

            foreach (string s in removeKey)
            {
                dicCompare.Remove(s);
            }
        }

        private string[] EscapeArrayQuoteMark(string[] array)
        {
            string[] data = new string[array.Length];

            for (int i = 0; i < data.Length; i++)
            {
                data[i] = CsvQuoteMark.Unescape(array[i]);
            }

            return data;
        }

        private void ChannelGainComp(EGainOffsetType type, string normalizeCH)
        {
            double chRatio = 1.0d;

            double chOffSet = 0.0d;

            this.dicChannelGainCompareResult.Clear();

            this.dicChannelOffSetCompareResult.Clear();

            if (type == EGainOffsetType.Gain)
            {
                double normalizeGainValue = dicChannelGain[normalizeCH].Average();

                foreach (var CH in dicChannelGain)
                {
                    chRatio = CH.Value.Average() / normalizeGainValue;

                    dicChannelGainCompareResult.Add(CH.Key, chRatio);
                }

                foreach (var CH in dicChannelOffSet)
                {
                    dicChannelOffSetCompareResult.Add(CH.Key, chOffSet);
                }

            }
            else if (type == EGainOffsetType.Offset)
            {
                double normalizeOffSetValue = dicChannelOffSet[normalizeCH].Average();

                foreach (var CH in dicChannelGain)
                {
                    dicChannelGainCompareResult.Add(CH.Key, chRatio);
                }

                foreach (var CH in dicChannelOffSet)
                {
                    chOffSet = CH.Value.Average() - normalizeOffSetValue;

                    dicChannelOffSetCompareResult.Add(CH.Key, chOffSet);
                }
            }
        }

        #endregion 

        #region >>> Public Property <<<

        public List<string> ChannelName
        {
            get
            {
                List<string> rtn = new List<string>();

                foreach (var var in dicChStandard)
                {
                    rtn.Add("SCH-" + var.Key);
                }

                foreach (var var in dicChMeasure)
                {
                    rtn.Add("MCH-" + var.Key);
                }

                return rtn;
            }

        }

        public List<double[]> ChValue
        {
            get
            {
                List<double[]> rtn = new List<double[]>();

                foreach (var var in dicChStandard)
                {
                    rtn.Add(var.Value.ToArray());
                }

                foreach (var var in dicChMeasure)
                {
                    rtn.Add(var.Value.ToArray());
                }

                return rtn;
            }

        }

        public string[] MsrtItemNameArray
        {
            get
            {
                //List<string> name = new List<string>();

                //foreach (string s in this._stdTitle)
                //{
                //    string sname = CsvQuoteMark.Unescape(s);

                //    if (!SYS_RESULT_ITEM_NAME.Contains(sname) && sname != "P/F" && sname != "NULL")
                //    {
                //        name.Add(sname);
                //    }

                //}

                return _availableItemName.ToArray();
            }
        }

        public int TestCount
        {
            get
            {
                return this.dicCompare.Count;
            }
        }

        public string[] StdTitleData
        {
            get
            {
                return this._stdTitle;
            }
        }

        public string[] MsrtTitleData
        {
            get
            {
                // string[] rtn = new string[_msrtTitle.Length];

                return this._msrtTitle;
            }
        }

        public Dictionary<string, double> DicChannelGainCompareResult
        {
            get
            {
                return dicChannelGainCompareResult;
            }
        }

        public Dictionary<string, double> DicChannelOffSetCompareResult
        {
            get
            {
                return dicChannelOffSetCompareResult;
            }
        }

        public Dictionary<string, List<string>> DicCompareData
        {
            get { return dicCompare; }
        }

        public Dictionary<string, GainAndFilterDara> DicFilterData
        {
            get
            {
                return _filterDic;
            }
        }

        public CalcGainOffset[] CalcChannelGainOffset
        {
            get
            {
                return _channelGainOffset;
            }
        }

        #endregion
    }
}

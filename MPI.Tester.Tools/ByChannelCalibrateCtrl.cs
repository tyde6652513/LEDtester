using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;
using System.Xml;

using MPI.Tester.Data;

namespace MPI.Tester.Tools
{
    public class ByChannelCalibrateCtrl : FilesCompare
    {
        private Dictionary<string, CalcGainOffset[]> _calcGainOffsetData = new Dictionary<string, CalcGainOffset[]>();

        private Dictionary<string, double[]> _compData = new Dictionary<string, double[]>();

        private int _reportFileChannelCol = -1;

        private Dictionary<int, List<DataRow>> _sortedDataByChannel = new Dictionary<int, List<DataRow>>();

        private int _numbersOfChannel = 0;

        public ByChannelCalibrateCtrl()
        {

        }

        #region >>> Public Property <<< <<<

        public Dictionary<string, CalcGainOffset[]> DicCalcGainOffset
        {
            get { return this._calcGainOffsetData; }
        }

        public Dictionary<string, double[]> DicCompareData
        {
            get { return this._compData; }
        }

        public int ChannelIndexOfReportFile
        {
            get { return this._reportFileChannelCol; }
        }

        #endregion

        #region >>> Private Methods <<<

        private void SortDataByChannel()
        {
            this._sortedDataByChannel.Clear();

            for (int targetChannel = 0; targetChannel < this._numbersOfChannel; targetChannel++)
            {
                List<DataRow> dataRows = new List<DataRow>();

                for (int row = 0; row < this._dtCompare.Rows.Count; row++)
                {
                    double channelIndex = (double)this._dtCompare.Rows[row][this._reportFileChannelCol * 3 + 2];	// msrt index = 2, 5, 8,

                    if ((channelIndex - 1) == targetChannel)
                    {
                        dataRows.Add(this._dtCompare.Rows[row]);
                    }
                }

                this._sortedDataByChannel.Add(targetChannel, dataRows);
            }
        }

        private bool GetChannelPositionOfReportFile()
        {
            foreach (var data in this._titleIndex)
            {
                if (data.Key == "CHANNEL")
                {
                    _reportFileChannelCol = data.Value;

                    return true;
                }
            }

            return false;
        }

        #endregion

        #region >>> Public Methods <<<

        public void CreateGainOffset(TestItemData[] testItems)
        {
            if (this._titleName == null)
                return;

            string str = string.Empty;
            string strNum = string.Empty;
            string[] keyNames = new string[this._titleName.Keys.Count];

            Array.Copy(this._titleName.Keys.ToArray(), keyNames, keyNames.Length);

            if (keyNames == null || keyNames.Length == 0)
                return;

            this._gainOffsetIndex = new Dictionary<string, int>();

            this._gainOffsetIndex.Clear();

            for (int i = 0; i < keyNames.Length; i++)
            {
                if (keyNames[i].IndexOf("_") >= 0)
                {
                    str = keyNames[i].Remove(keyNames[i].IndexOf("_"));
                    strNum = keyNames[i].Substring(keyNames[i].IndexOf("_") + 1);

                    switch (str)
                    {
                        //case "MTHYVP":
                        //case "MTHYVD":
                        //case "MVF":
                        //case "MVFLA":
                        //case "MVZ":
                        //case "MIR":
                        //case "MIF":
                        //case "CIEx":
                        //case "CIEy":
                        //case "CCT":
                        //case "CRI":
                        //case "PURITY":
                        ////AC Item
                        //case "ACMIF":
                        //case "ACPOWER":
                        //case "ACAPPRARENT":
                        //case "ACPF":
                        //case "ACFREQUENCY":
                        //case "ACPEAK":
                        //case "ACPEAKMAX":
                        //case "ACMIFL":
                        //case "ACPOWERL":
                        //case "ACAPPRARENTL":
                        //case "ACPFL":
                        //case "ACFREQUENCYL":
                        //case "ACPEAKL":
                        //case "ACPEAKMAXL":
                        case "WATT":
                        case "LM":
                        case "LOP":
                       // case "WLP":
                       // case "WLD":
                       // case "CIEx":
                        //case "CIEy":
                            this._gainOffsetIndex.Add(keyNames[i], i);
                            break;
                        //---------------------------------------------------------
                        case "TEST":
                        case "BIN":
                        case "CONTA":
                        case "CONTC":
                        case "POLAR":
                        case "CIEz":
                        case "ST":
                        case "INT":
                        case "ROW":
                        case "COL":
                        case "CHUCKX":
                        case "CHUCKY":
                        case "CHUCKZ":
                            break;
                        //---------------------------------------------------------
                        default:
                            // Gilbert 20120801
                            // this._gainOffsetIndex.Add(keyNames[i], i);
                            break;
                    }

                }
            }

            if (testItems != null)
            {
                foreach (TestItemData data in testItems)
                {
                    if (data.GainOffsetSetting != null && data.GainOffsetSetting.Length > 0)
                    {
                        for (int i = 0; i < data.GainOffsetSetting.Length; i++)
                        {
                            if (this._titleInfor.ContainsKey(data.GainOffsetSetting[i].KeyName))
                            {
                                this._titleInfor[data.GainOffsetSetting[i].KeyName].GainOffsetType = data.GainOffsetSetting[i].Type;

                                this._titleInfor[data.GainOffsetSetting[i].KeyName].IsEnable = true;
                            }
                        }
                    }
                }
            }

        }

        public void SetData(int channelCount)
        {
            this._numbersOfChannel = channelCount;

            this.GetChannelPositionOfReportFile();

            this.SortDataByChannel();
        }

        public void Calculate()
        {
            this._calcGainOffsetData.Clear();

            this._compData.Clear();

            foreach (var data in this._gainOffsetIndex)
            {
                CalcGainOffset[] calcGainOffset = new CalcGainOffset[this._sortedDataByChannel.Count];

                TitleData titleData = null;

                if (this._titleInfor.ContainsKey(data.Key))
                {
                    titleData = this._titleInfor[data.Key];
                }

                if (titleData.IsEnable == false)
                {
                    continue;
                }

                int channel = 0;

                foreach (var ch in this._sortedDataByChannel)
                {
                    double[] xIn = new double[ch.Value.Count];

                    double[] yOut = new double[ch.Value.Count];

                    double[] delta = new double[ch.Value.Count];

                    for (int i = 0; i < ch.Value.Count; i++)
                    {
                        xIn[i] = (double)ch.Value[i][data.Value * 3 + 2];

                        yOut[i] = (double)ch.Value[i][data.Value * 3 + 1];

                        delta[i] = (double)ch.Value[i][data.Value * 3 + 3];
                    }

                    string key = string.Format("{0}@{1}", data.Key, ch.Key.ToString());

                    calcGainOffset[channel] = new CalcGainOffset(key, titleData.Name, xIn, yOut);

                    calcGainOffset[channel].Format = titleData.Format;

                    calcGainOffset[channel].CalcType = titleData.GainOffsetType;

                    calcGainOffset[channel].RunCalculate();

                    this._compData.Add(key, delta);

                    channel++;

                }

                this._calcGainOffsetData.Add(data.Key, calcGainOffset);
            }

            NormalizeFactor();
        }

        private void NormalizeFactor()
        {
            Dictionary<string, CalcGainOffset[]> calcgainOffset = new Dictionary<string, CalcGainOffset[]>();

            foreach (var item in this._calcGainOffsetData)
            {
                CalcGainOffset[] gainOffsetArray = item.Value;

                double gain = gainOffsetArray[0].Gain;

                double offset = gainOffsetArray[0].Offset;

                for (int i = 0; i < gainOffsetArray.Length; i++)
                {
                    gainOffsetArray[i].Gain = gainOffsetArray[i].Gain / gain;
                }
            }
        }

        #endregion
    }
}

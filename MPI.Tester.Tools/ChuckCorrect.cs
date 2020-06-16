using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Data;
using MPI.Tester.Data;
using System.IO;

namespace MPI.Tester.Tools
{
    public class ChuckCorrectionCtrl : FilesCompare
    {
        private const int SPEC_LENGTH = 7;
        private Dictionary<string, StdSpec> _itemSpec = new Dictionary<string, StdSpec>();
        private ChuckIndexState[] _calcChuckIndex;
        private DateTime _time;
        private string chuckIndexKeyName="";

        public ChuckCorrectionCtrl()
        {

        }

        public ChuckCorrectionCtrl(int numbersOfChuck)
            : this()
        {
            this._calcChuckIndex = new ChuckIndexState[numbersOfChuck];
        }

        #region >>> Public Property <<<

        public int NumbersOfChuck
        {
            get { return this._calcChuckIndex.Length; }
        }

        public Dictionary<string, StdSpec> ItemSpec
        {
            get { return this._itemSpec; }
            set { lock (this._lockObj) { this._itemSpec = value; } }
        }

        public ChuckIndexState[] CalcChuckIndex
        {
            get { return this._calcChuckIndex; }
        }

        public string ChuckIndexKeyName
        {
            get { return this.chuckIndexKeyName; }
        }

        public DateTime Time
        {
            get { return this._time; }
        }

        #endregion

        #region >>> Public Nethod <<<

        public bool LoadDailyWatchStdFile(string pathAndFile)
        {
            this.IsArrangeByRowCol = false;

            if (ParseSpecFromFile(pathAndFile) == false)
            {
                return false;
            }

            if(this.LoadStdFromFile(pathAndFile, 0)==EErrorCode.NONE)
            {
                return true;
            }
            return false;
        }

        public bool LoadDailyWatchMsrtFile(string pathAndFile)
        {
            this.IsArrangeByRowCol = false;
            if (this.LoadMsrtFromFile(pathAndFile, 0) == EErrorCode.NONE)
            {
                return true;
            }
            return false;
        }

        public bool CalcChuckFactor(int referChipIndex)
        {
            if (this._dtStd == null)
            {
                return false;
            }

            for (int i = 0; i < this._calcChuckIndex.Length; i++)
            {
                this._calcChuckIndex[i] = new ChuckIndexState(i + 1, "Chuck" + (i + 1).ToString(), this._itemSpec.Count);
                this._calcChuckIndex[i].StdData = this._dtStd.Rows[referChipIndex];
            }

            if (referChipIndex > this._dtStd.Rows.Count)
            {
                return false;
            }

            if (this._dtMsrt.Rows.Count < 8)
            {
                return false;
            }

            foreach (StdSpec spec in this._itemSpec.Values)
            {
                spec.StdData = (double)this._dtStd.Rows[referChipIndex][this._itemSpec[spec.KeyName].DataColIndex + 1];
            }

            for (int Idx = 0; Idx < 8; Idx++)  //chuck 1-8
            {
                int itemIndex = 0;
                foreach (StdSpec spec in this._itemSpec.Values)
                {
                    double[] x = new double[1];
                    double[] y = new double[1];
                    x[0] = (double)this._dtMsrt.Rows[Idx][this._itemSpec[spec.KeyName].DataColIndex + 1];
                    y[0] = spec.StdData;
                    this._calcChuckIndex[Idx].CalcGainOffset[itemIndex] = new DailyGainOffset(spec.KeyName, spec.Name, x, y, spec.DailyWatchSpec,spec.ReCalibSpec);
                    this._calcChuckIndex[Idx].CalcGainOffset[itemIndex].RunCalcAndGetState(EGainOffsetType.None);			//Power
                    this._calcChuckIndex[Idx].MsrtData = this._dtMsrt.Rows[Idx];
                    itemIndex++;
                }
            }
            this._time = DateTime.Now;
            return true;
        }

        #endregion

        #region >>> Private Nethod <<<

        private bool ParseSpecFromFile(string pathAndFile)
        {
            this._itemSpec.Clear();
            List<string[]> temp = MPI.Tester.CSVUtil.ReadCSV(pathAndFile);
            if (temp == null)
            {
                return false;
            }
            temp.RemoveAt(0);
            int titleStartIndex = -1;
            string[] titleStrName = this._titleName.Values.ToArray();

            for (int idx = 0; idx < temp.Count; idx++)
            {
                if (temp[idx][0] == titleStrName[0])
                {
                    titleStartIndex = idx;
                    break;
                }
            }

            if (titleStartIndex == -1)
            {
                return false;
            }

            for (int i = 0; i < titleStartIndex; i++)
            {
                string[] itemNameAndSpec = temp[i];
                string itemName = itemNameAndSpec[0];

                foreach (KeyValuePair<string, string> kvp in this._titleName)
                {
                    if (kvp.Value == itemName)
                    {
                        StdSpec spec = new StdSpec(kvp.Key, kvp.Value);
                        double watchSpec = -1;
                        double reCalibSpec = -1;
                        double.TryParse(itemNameAndSpec[1], out watchSpec);
                        double.TryParse(itemNameAndSpec[2], out reCalibSpec);
                        spec.DailyWatchSpec = watchSpec;
                        spec.ReCalibSpec = reCalibSpec;

                        if (this._titleIndex.ContainsKey(kvp.Key))
                        {
                            spec.DataColIndex = this._titleIndex[kvp.Key];
                        }
                        this._itemSpec.Add(kvp.Key, spec);
                        break;
                    }
                }
            }
            return true;
        }

        #endregion

    }

    public class ChuckIndexState
    {
        private object _lockObj;
        private int _index;
        private string _name;
        private DailyGainOffset[] _calcGainOffsetArray;
        private EDailyWacthState _state;
        private DataRow _stdData;
        private DataRow _msrtData;

        public ChuckIndexState()
        {
            this._lockObj = new object();
            this._name = string.Empty;
            this._index=-1;
            this._state = EDailyWacthState.PASS;
        }

        public ChuckIndexState(int chuckIndex, string name, int numbers)
            : this()
        {
            this._index = chuckIndex;
            this._name = name;
            this._calcGainOffsetArray = new DailyGainOffset[numbers];
            for (int i = 0; i < _calcGainOffsetArray.Length; i++)
            {
                this._calcGainOffsetArray[i] = new DailyGainOffset();
            }
        }

        #region >>> Public Property <<<


        public string Name
        {
            get { return this._name; }
            set { lock (this._lockObj) { this._name = value; } }
        }
        public int Index
        {
            get { return this._index; }
            set { lock (this._lockObj) { this._index = value; } }
        }
        public DailyGainOffset[] CalcGainOffset
        {
            get { return this._calcGainOffsetArray; }
            set { lock (this._lockObj) { this._calcGainOffsetArray = value; } }
        }

        public EDailyWacthState EState
        {
            get { return  this._state; }
            set { lock (this._lockObj) { this._state = value; } }
        }

        public DataRow StdData
        {
            get { return this._stdData; }
            set { lock (this._lockObj) { this._stdData = value; } }
        }


        public DataRow MsrtData
        {
            get { return this._msrtData; }
            set { lock (this._lockObj) { this._msrtData = value; } }
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;

namespace MPI.Tester.Tools
{

    public class ResultData
    {
        private object _lockObj;
        private string _keyName;
        private string _name;
        private double _avgBias;
        private int _mode;

        public ResultData()
        {
            _lockObj = new object();
            this._keyName = "";
            this._name = "";
            this._avgBias = 0.0d;
            this._mode = 0;      
        }

        #region >>> Public Property <<<

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

        public double AvgBias
        {
            get { return this._avgBias; }
            set { lock (this._lockObj) { this._avgBias = value; } }
        }

        public int Mode
        {
            get { return this._mode; }
            set { lock (this._lockObj) { this._mode = value; } }
        }

        #endregion

    }

    public class LogDailyItem
    {
        private object _lockObj;
        private string _recipeFileName;
        private string _checkFileName;
        private string _dateTime;
        private double _voltOffset;
        private double _wLOffset;
        private double _powerGain;
        private bool _isCheckResult;
        private int _mode;
        private List<ResultData> _data;

        public LogDailyItem()
        {
            _lockObj = new object();
            this._recipeFileName = "";
            this._checkFileName = "";
            this._voltOffset = 0;
            this._wLOffset = 0;
            this._mode = 1;
            this._isCheckResult = false;
            this._data = new List<ResultData>();
        }

        public LogDailyItem(string dateTime)
            : this()
        {
            this._dateTime = dateTime;
        }

        #region >>> Public Property <<<

        public string RecipeFileName
        {
            get { return this._recipeFileName; }
            set { lock (this._lockObj) { this._recipeFileName = value; } }
        }

        public string CheckFileName
        {
            get { return this._checkFileName; }
            set { lock (this._lockObj) { this._checkFileName = value; } }
        }


        public string CheckTime
        {
            get { return this._dateTime; }
            set { lock (this._lockObj) { this._dateTime = value; } }
        }

        public bool IsCheckResult
        {
            get { return this._isCheckResult; }
            set { lock (this._lockObj) { this._isCheckResult = value; } }
        }

        public double VoltOffset
        {
            get { return this._voltOffset; }
            set { lock (this._lockObj) { this._voltOffset = value; } }
        }

        public double WLOffset
        {
            get { return this._wLOffset; }
            set { lock (this._lockObj) { this._wLOffset = value; } }
        }
        public double PowerGain
        {
            get { return this._powerGain; }
            set { lock (this._lockObj) { this._powerGain = value; } }
        }

        public int Mode
        {
            get { return this._mode; }
            set { lock (this._lockObj) { this._mode = value; } }
        }
        public List<ResultData> Data
        {
            get { return this._data; }
            set { lock (this._lockObj) { this._data = value; } }
        }

        #endregion
    }

    public class LogData
    {
        private object _lockObj;
        private List<LogDailyItem> _data;

        public LogData()
        {
            this._lockObj = new object();
            this._data = new List<LogDailyItem>();
        }

        public List<LogDailyItem> Items
        {
            get { return this._data; }
        }

        public void Add(LogDailyItem data)
        {
            if (this._data.Count > 1000)
            {
                this._data.RemoveRange(0, 1);
            }
            this._data.Add(data);
        }
    }


    public class LogDailyCheck
    {
        private LogData _log;
        private const string RecordPathAndFile = @"C:\MPI\LEDTester\Tools\Record";
        private string _recipeName;

        public LogDailyCheck(string reciepe)
        {
            _recipeName = reciepe;
            this.Load(reciepe);
        }

        public bool Load(string reciepe)
        {
            string pathAndFileName = Path.Combine(RecordPathAndFile, reciepe + ".xml");

            if (File.Exists(pathAndFileName) == false)
            {
                _log = new LogData();
                return false;
            }

            this._log = MPI.Xml.XmlFileSerializer.Deserialize(typeof(LogData), pathAndFileName) as LogData;
            return true;
        }

        public void Add(LogDailyItem data)
        {
            if(this._log==null)
                return;

            this._log.Items.Add(data);
        }

        public void Save()
        {
            string pathAndFileName = Path.Combine(RecordPathAndFile, this._recipeName + ".xml");
            MPI.Xml.XmlFileSerializer.Serialize(this._log, pathAndFileName);
        }

        public LogData LogData
        {
            get { return this._log; }
        }
    }
}

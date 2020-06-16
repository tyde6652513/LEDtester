using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace MPI.Tester.Tools
{
    public class DailyCheckSpecInfo
    {
        private object _lockObj;
        private Dictionary<string, DailyWatchSpec> _data;
        private DailyWatchSpec[] _itemData;
        private string[] _itemName;
        private int _minCountAccept;
        private int _toleranceOutSpecCount;
        private int _alreadyOutSpecCount;
        private string _recipeName;
        private string[]  _theOtherAcceptRecipes;  

        public DailyCheckSpecInfo()
        {
            this._lockObj = new object();
            _data = new Dictionary<string, DailyWatchSpec>();
            _minCountAccept = 0;
            _toleranceOutSpecCount = 0;
            _alreadyOutSpecCount = 0;
            _recipeName = string.Empty;
            _theOtherAcceptRecipes = new string[1] { _recipeName };
        }

        public DailyWatchSpec[] ItemData
        {
            get { return this._data.Values.ToArray(); }
            set { lock (this._lockObj) { this._itemData = value; } }
        }
        public string[] Name
        {
            get { return this._data.Keys.ToArray(); }
            set { lock (this._lockObj) { this._itemName = value; } }
        }

        public int MinCountAccept
        {
            get { return this._minCountAccept; }
            set { lock (this._lockObj) { this._minCountAccept = value; } }
        }

        public int ToleranceOutSpecCount
        {
            get { return this._toleranceOutSpecCount; }
            set { lock (this._lockObj) { this._toleranceOutSpecCount = value; } }
        }

        public int AlreadyOutSpecCount
        {
            get { return this._alreadyOutSpecCount; }
            set { lock (this._lockObj) { this._alreadyOutSpecCount = value; } }
        }

        public string RecipeName
        {
            get { return this._recipeName; }
            set { lock (this._lockObj) { this._recipeName = value; } }
        }

        public string[] TheOtherAcceptRecipes
        {
            get { return this._theOtherAcceptRecipes; }
            set { lock (this._lockObj) { this._theOtherAcceptRecipes = value; } }
        }

        [XmlIgnore]
        public Dictionary<string, DailyWatchSpec> Data
        {
            get { return this._data; }
            set { lock (this._lockObj) { this._data = value; } }
        }

        public void PushData(Dictionary<string, string> title)
        {
            this._data = new Dictionary<string, DailyWatchSpec>();

            for (int i = 0; i < this._itemData.Length; i++)
            {
                if (title.ContainsKey(this._itemData[i].KeyName))
                {
                   this. _data.Add(_itemName[i], _itemData[i]);
                }
            }
        }
    }

    public class StdSpec
    {
        private object _lockObj;
        private string _keyName;
        private string _name;
        private double _reCalibSpec;   //  must to be recalib
        private double _dailyWatchSpec;   // auto tune spec
        private bool _isEnableFilter;
        private double _minValue;
        private double _maxValue;
        private int _dataColIndex = -1;
        private double _stdData = 0;

        public StdSpec()
        {
            this._lockObj = new object();
            this._keyName = string.Empty;
            this._name = string.Empty;
            this._reCalibSpec = 0.0d;
            this._dailyWatchSpec = 0.0d;
            this._minValue = 0.0d;
            this._maxValue = 0.0d;
            this._isEnableFilter = false;
        }

        public StdSpec(string keyName, string name)
            : this()
        {
            this._keyName = keyName;
            this._name = name;
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

        public double ReCalibSpec
        {
            get { return this._reCalibSpec; }
            set { lock (this._lockObj) { this._reCalibSpec = value; } }
        }

        public double DailyWatchSpec
        {
            get { return this._dailyWatchSpec; }
            set { lock (this._lockObj) { this._dailyWatchSpec = value; } }
        }

        public bool IsEnbaleFilter
        {
            get { return this._isEnableFilter; }
            set { lock (this._lockObj) { this._isEnableFilter = value; } }
        }

        public double MinValue
        {
            get { return this._minValue; }
            set { lock (this._lockObj) { this._minValue = value; } }
        }

        public double MaxValue
        {
            get { return this._maxValue; }
            set { lock (this._lockObj) { this._maxValue = value; } }
        }

        public int DataColIndex
        {
            get { return this._dataColIndex; }
            set { lock (this._lockObj) { this._dataColIndex = value; } }
        }

        public double StdData
        {
            get { return this._stdData; }
            set { lock (this._lockObj) { this._stdData = value; } }
        }
        #endregion

    }

    public class DailyWatchSpec : StdSpec
    {
        private object _lockObj;
        private double _everDieHighSpec;
        private double _everDieLowerSpec;
        private int _criteriaType;
        private bool _isEnable;
        private string _criteriaUnit;

        public DailyWatchSpec()
        {
            this._lockObj = new object();
            this._everDieHighSpec = 0;
            this._everDieLowerSpec = 0;
            this. _criteriaType = 0;
            this._criteriaUnit=string.Empty;
        }

        public DailyWatchSpec(string keyName, string name) : this()
        {
            this.KeyName = keyName;
            this.Name = name;
            this._isEnable = false;
        }
        #region >>> Public Property <<<

        public double EverDieHighSpec
        {
            get { return this._everDieHighSpec; }
            set { lock (this._lockObj) { this._everDieHighSpec = value; } }
        }
        public double EverDieLowerSpec
        {
            get { return this._everDieLowerSpec; }
            set { lock (this._lockObj) { this._everDieLowerSpec = value; } }
        }

        public int CriteriaType
        {
            get { return this._criteriaType; }
            set { lock (this._lockObj) { this._criteriaType = value; } }
        }

        public bool IsEnable
        {
            get { return this._isEnable; }
            set { lock (this._lockObj) { this._isEnable = value; } }
        }
        public string CriteriaUnit
        {
            get { return this._criteriaUnit; }
            set { lock (this._lockObj) { this._criteriaUnit = value; } }
        }

        #endregion
    }

    public class DcSpecRecipeManger
    {
        private object _lockObj;
        private List<string> _sourceRecipes;
        private List<string> _targetRecipe;
        private string _currentRecipe;

        public DcSpecRecipeManger()
        {
            this._lockObj = new object();
            _sourceRecipes = new List<string>();   
            _targetRecipe = new List<string>();
            _currentRecipe = string.Empty;
        }

        public void Create(string currentRecipe, string[] systemCurrentRecipe, string[] acceptRecipes)
        {
            _currentRecipe = currentRecipe;
            _sourceRecipes.Clear();
            _targetRecipe.Clear();
            _sourceRecipes.AddRange(systemCurrentRecipe);
            _targetRecipe.AddRange(acceptRecipes);
            Check();
        }


        private void Check()
        {
            for (int i = 0; i < this._targetRecipe.Count; i++)
            {
                if (this._targetRecipe[i] == string.Empty)
                {
                    this._targetRecipe.Remove(this._targetRecipe[i]);
                }
            }


            if (this._sourceRecipes.Contains(this._currentRecipe))
            {
                this._sourceRecipes.Remove(this._currentRecipe);
            }


            for (int i = 0; i < this._sourceRecipes.Count; i++)
            {
                if (!this._targetRecipe.Contains(this._sourceRecipes[i]))
                {
                    this._targetRecipe.Remove(this._sourceRecipes[i]);
                }
            }

            for (int i = 0; i < this._targetRecipe.Count; i++)
            {
                if (this._sourceRecipes.Contains(this._targetRecipe[i]))
                {
                    this._sourceRecipes.Remove(this._targetRecipe[i]);
                }
            }
        }

        public void RenewTarget(string[] acceptRecipes)
        {
            _targetRecipe.Clear();
            _targetRecipe.AddRange(acceptRecipes);
            Check();
        }

        public void Add(string recipename)
        {
            if (this._sourceRecipes.Contains(recipename))
            {
                this._sourceRecipes.Remove(recipename);
                this._targetRecipe.Add(recipename);
            }
        }

        public void Delete(string recipename)
        {
            if (this._targetRecipe.Contains(recipename))
            {
                this._targetRecipe.Remove(recipename);
                this._sourceRecipes.Add(recipename);
            }
        }

        #region >>> Public Property <<<

        public string[] SourceRecipes
        {
            get { return this._sourceRecipes.ToArray(); }
        }
        public string[] TargetRecipes
        {
            get { return this._targetRecipe.ToArray(); }
        }


        #endregion

    }
}

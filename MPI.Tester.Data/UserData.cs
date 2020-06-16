using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

using System.Xml;
using System.IO;

namespace MPI.Tester.Data
{
	[Serializable]
    public class UserData
    {
        private object _lockObj;

		private uint _id;
		private string _name;
		private string _version;
		private int _dataIndexShift;
		private XmlDocument _xmlUserDefine;
		private string[] _formatNames;
		private string[] _outputFileNameFormat01;
		private string[] _outputFileNameFormat02;

		private Dictionary<string, string> _testItemNameDic;
		private Dictionary<string, string> _forceItemNameDic;
		private Dictionary<string, string> _binItemNameDic;
		private Dictionary<string, string> _resultItemNameDic;
        private Dictionary<string, string> _resultItemExtNameDic;
		private Dictionary<string, TestResultData> _msrtDisplayItemDic;

		private string[] _lopItemSelectList;

        private int _opUIControlIndex = 0;
        private bool _isEnableShowCalibrateFileLink;
        private int _dailyCheckControlsMode = 0;
        private List<string> _dailyCheckFormat;
        private List<string> _dailyCheckSaveLogFormat;

        private string _dailyCheckingSaveReportName;

		private bool _isEnableLimitMode;
        private bool _isLDT1ASoftwareClamp;
        private bool _isEnableSptAdvSetting;
        private bool _isEnableSkipOptiMsrt;
        private bool _isEnableCorrectSpectometerParam;

		private bool _isEnableHighAccuracyMode;

        private bool _isEnableSafetyClamp;

        private bool _isAutoReNameVfItemOfLOPWL;

        private bool _isPermitSetMsrtItemName;

        private bool _isPermitSetMsrtItemUnit;

        private bool _isEnableAutoOrderResultItem;

        private bool _isShowPassRateChecking;

        private bool _isShowAdjacentChecking;

        private bool _isShowPreSamplingChecking;

		private int _reportTempFileKeepDays;

		private int _testSTartIndex;

        private bool _isShowReportInterpolation;

        private bool _isShowEnableTestGroup;

        public UserData()
        {
            this._lockObj = new object();

            this._id = (int)EUserID.MPI;
            this._name = string.Empty;
			this._version = string.Empty;
			this._xmlUserDefine = null;

			this._testItemNameDic = new Dictionary<string, string>(100);
			this._forceItemNameDic = new Dictionary<string, string>(100);
			this._binItemNameDic = new Dictionary<string, string>(100);
			this._resultItemNameDic = new Dictionary<string, string>(100);
            this._resultItemExtNameDic = new Dictionary<string, string>(100);

			this._msrtDisplayItemDic = new Dictionary<string, TestResultData>(100);

			this._lopItemSelectList = new string[2] {"mcd" , "Watt"};
			this._outputFileNameFormat01 = new string[4] { "WaferNum", " ", "StartTime" ,"#",};
			this._outputFileNameFormat02 = new string[4] { "WaferNum", " ", "StartTime", "#", };

            this._opUIControlIndex = 0;
            this._dailyCheckControlsMode = 0;
            this._isEnableShowCalibrateFileLink = false;
            this._dailyCheckFormat = new List<string>(10);
            this._dailyCheckSaveLogFormat = new List<string>(10);

            this._dailyCheckingSaveReportName = "TestFileName_#_MachineName_#_Time";

			this._isEnableLimitMode = true;
            this._isLDT1ASoftwareClamp = false;
            this._isEnableSptAdvSetting = false;
            this._isEnableSkipOptiMsrt = false;

            this._isEnableHighAccuracyMode = false;

            this._isEnableSafetyClamp = false;

            this._isAutoReNameVfItemOfLOPWL = false;

            this._isPermitSetMsrtItemName = false;

            this._isPermitSetMsrtItemUnit = false;

			this._isEnableAutoOrderResultItem = false;

			this._isShowPassRateChecking = false;

			this._isShowAdjacentChecking = false;

			this._isShowPreSamplingChecking = false;

			this._reportTempFileKeepDays = 30;

			this._testSTartIndex = 1;
            this._isShowReportInterpolation = false;

            this._isShowEnableTestGroup = false;
        }

        #region >>> Public Property <<<

        public uint ID
        {
            get { return this._id; }
        }

        public string Name
        {
            get { return this._name; }      
        }

		public string Version
		{
			get { return this._version; }
		}

		public int DataIndexShift
		{
			get { return this._dataIndexShift; }
		}

        public XmlDocument XmlUserDefine
        {
            get { return this._xmlUserDefine; }
        }

        public string[] FormatNames
        {
            get { return this._formatNames; }
        }

		public Dictionary<string, string> TestItemNameDic
		{
			get { return this._testItemNameDic; }
		}

		public Dictionary<string, string> ForceItemNameDic
		{
			get { return this._forceItemNameDic; }
		}

		public Dictionary<string, string> BinItemNameDic
		{
			get 
			{
				if (this._binItemNameDic.Count == 0)
				{
					this.CreateDefaultBinItemName();
				}

				return this._binItemNameDic; 
			}
		}

        public Dictionary<string, string> ResultItemNameDic
        {
            get { return this._resultItemNameDic; }
        }

        public Dictionary<string, string> ResultItemExtNameDic
        {
            get { return this._resultItemExtNameDic; }
        }

		public TestResultData this[string keyName]
		{
			get
			{
				if (this._msrtDisplayItemDic.ContainsKey(keyName))
				{
					return this._msrtDisplayItemDic[keyName];
				}
				else
				{
					return null;
				}
			}
		}

		public Dictionary<string, string> MsrtDisplayItemDic
		{
			get
			{
				Dictionary<string, string> msrtDisplayItemDic = new Dictionary<string, string>();

				if (this._msrtDisplayItemDic == null)
					return msrtDisplayItemDic;

				foreach(var item in this._msrtDisplayItemDic)
				{
					msrtDisplayItemDic.Add(item.Value.KeyName, item.Value.Name);
				}

				return msrtDisplayItemDic;
			}
		}

		public string[] LOPItemSelectList
		{
			get { return this._lopItemSelectList; }
		}

		public string[] OutputFileNameFormat01
		{
			get { return this._outputFileNameFormat01; }
		}

		public string[] OutputFileNameFormat02
		{
			get { return this._outputFileNameFormat02; }
		}

        public int OpUIControlIndex
        {
            get { return this._opUIControlIndex; }
        }

        public bool IsEnableShowCalibrateFileLink
        {
            get { return this._isEnableShowCalibrateFileLink; }
        }

        public int DailyCheckControlsMode
        {
            get { return this._dailyCheckControlsMode; }
        }

        public string[] DCheckFormat
        {
            get { return this._dailyCheckFormat.ToArray(); }
        }

        public string[] DCheckSaveLogFormat
        {
            get { return this._dailyCheckSaveLogFormat.ToArray(); }
        }

        public string DailyCheckingSaveReportName
        {
            get { return this._dailyCheckingSaveReportName; }
        }

		public bool IsEnableLimitMode
		{
			get { return this._isEnableLimitMode; }
		}

        public bool IsLDT1ASoftwareClamp
        {
            get { return this._isLDT1ASoftwareClamp; }
        }

        public bool IsEnableSptAdvSetting
        {
            get { return this._isEnableSptAdvSetting; }
        }

        public bool IsEnableSkipOptiMsrt
        {
            get { return this._isEnableSkipOptiMsrt; }
        }

        public bool IsEnableCorrectSpectometerParam
        {
            get { return this._isEnableCorrectSpectometerParam; }
        }

		public bool IsEnableHighAccuracyMode
        {
            get { return this._isEnableHighAccuracyMode; }
        }

        public bool IsEnableSafetyClamp
        {
            get { return this._isEnableSafetyClamp; }
        }

        public bool IsPermitSetMsrtItemName
        {
            get { return this._isPermitSetMsrtItemName; }
        }

        public bool IsPermitSetMsrtItemUnit
        {
            get { return this._isPermitSetMsrtItemUnit; }
        }
        
        public bool IsAutoReNameVfItemOfLOPWL
        {
            get { return this._isAutoReNameVfItemOfLOPWL; }
        }

        public bool IsEnableAutoOrderResultItem
        {
            get { return this._isEnableAutoOrderResultItem; }
        }

        public bool IsShowAdjacentChecking
        {
            get { return this._isShowAdjacentChecking ; }
        }

        public bool IsShowPassRateChecking
        {
            get { return this._isShowPassRateChecking; }
        }

        public bool IsShowPreSamplingChecking
        {
            get { return this._isShowPreSamplingChecking; }
        }

		  public int TestStartIndex
		  {
			  get { return this._testSTartIndex; }
		  }

		public int ReportTempFileKeepDays
		{
			get { return this._reportTempFileKeepDays; }
		}


        public bool IsShowReportInterpolation
		{
            get { return this._isShowReportInterpolation; }
		}

        public bool IsShowEnableTestGroup
        {
            get { return this._isShowEnableTestGroup; }
        }
        #endregion

		#region >>> private Methods <<<

		private void CreateDefaultBinItemName()
		{
			//Creat Default Item
			if (!this._binItemNameDic.ContainsKey("CIExy_1"))
			{
				this._binItemNameDic.Add("CIExy_1", "XY1");
			}
			if (!this._binItemNameDic.ContainsKey("CIExy_2"))
			{
				this._binItemNameDic.Add("CIExy_2", "XY2");
			}
			if (!this._binItemNameDic.ContainsKey("CIExy_3"))
			{
				this._binItemNameDic.Add("CIExy_3", "XY3");
			}
			if (!this._binItemNameDic.ContainsKey("LowLabel"))
			{
				this._binItemNameDic.Add("LowLabel", "_L");
			}
			if (!this._binItemNameDic.ContainsKey("UpLabel"))
			{
				this._binItemNameDic.Add("UpLabel", "_H");
			}
			if (!this._binItemNameDic.ContainsKey("CIENameLabel"))
			{
				this._binItemNameDic.Add("CIENameLabel", "(Name)");
			}
			if (!this._binItemNameDic.ContainsKey("Px1"))
			{
				this._binItemNameDic.Add("Px1", "_X1(x)");
			}
			if (!this._binItemNameDic.ContainsKey("Py1"))
			{
				this._binItemNameDic.Add("Py1", "_Y1(y)");
			}
			if (!this._binItemNameDic.ContainsKey("Px2"))
			{
				this._binItemNameDic.Add("Px2", "_X2(a)");
			}
			if (!this._binItemNameDic.ContainsKey("Py2"))
			{
				this._binItemNameDic.Add("Py2", "_Y2(b)");
			}
			if (!this._binItemNameDic.ContainsKey("Px3"))
			{
				this._binItemNameDic.Add("Px3", "_X3(Theta)");
			}
			if (!this._binItemNameDic.ContainsKey("Py3"))
			{
				this._binItemNameDic.Add("Py3", "_Y3(SDCM)");
			}

			//
			if (!this._binItemNameDic.ContainsKey("CIEupvp_1"))
			{
				this._binItemNameDic.Add("CIEupvp_1", "UpVp1");
			}
			if (!this._binItemNameDic.ContainsKey("CIEupvp_2"))
			{
				this._binItemNameDic.Add("CIEupvp_2", "UpVp2");
			}
			if (!this._binItemNameDic.ContainsKey("CIEupvp_3"))
			{
				this._binItemNameDic.Add("CIEupvp_3", "UpVp3");
			}
			if (!this._binItemNameDic.ContainsKey("Pup1"))
			{
				this._binItemNameDic.Add("Pup1", "_Up1(up)");
			}
			if (!this._binItemNameDic.ContainsKey("Pvp1"))
			{
				this._binItemNameDic.Add("Pvp1", "_Vp1(vp)");
			}
			if (!this._binItemNameDic.ContainsKey("Pup2"))
			{
				this._binItemNameDic.Add("Pup2", "_Up2(a)");
			}
			if (!this._binItemNameDic.ContainsKey("Pvp2"))
			{
				this._binItemNameDic.Add("Pvp2", "_Vp2(b)");
			}
			if (!this._binItemNameDic.ContainsKey("Pup3"))
			{
				this._binItemNameDic.Add("Pup3", "_Up3(Theta)");
			}
			if (!this._binItemNameDic.ContainsKey("Pvp3"))
			{
				this._binItemNameDic.Add("Pvp3", "_Vp3");
			}

			for (int i = 4; i < 100001; i++)
			{
				string index = i.ToString();

				string xLable = "Px" + index;

				string yLable = "Py" + index;

				string upLable = "Pup" + index;

				string vpLable = "Pvp" + index;

				if (!this._binItemNameDic.ContainsKey(xLable))
				{
					this._binItemNameDic.Add(xLable, "_X" + index);
				}
				if (!this._binItemNameDic.ContainsKey(yLable))
				{
					this._binItemNameDic.Add(yLable, "_Y" + index);
				}
				if (!this._binItemNameDic.ContainsKey(upLable))
				{
					this._binItemNameDic.Add(upLable, "_Up" + index);
				}
				if (!this._binItemNameDic.ContainsKey(vpLable))
				{
					this._binItemNameDic.Add(vpLable, "_Vp" + index);
				}
			}
		}

		#endregion

		#region >>> Public Methods <<<

		public bool LoadUserDefineData(EUserID user)
        {
            int i = 0;
            int id = 0;

            XmlDocument xmlDoc = new XmlDocument();
			string fileNameWithExt = string.Format("{0}{1}", "User", ((int)user).ToString("0000")) + ".xml";
			string pathAndFile = Path.Combine(Constants.Paths.USER_DIR, fileNameWithExt);

			if (File.Exists(pathAndFile))
			{
				xmlDoc.Load(pathAndFile);
			}
			else
			{
				this._xmlUserDefine = null;
				return false;
			}

            this._xmlUserDefine = xmlDoc;

            XmlNode root = xmlDoc.DocumentElement;
            XmlNodeList nodes = xmlDoc.SelectNodes("/UserDefine/Formats/*");

            this._name = (root as XmlElement).GetAttribute("name");
			this._version = (root as XmlElement).GetAttribute("version");
            this._formatNames = new string[nodes.Count];
            foreach ( XmlNode node in nodes )
            {
                this._formatNames[i] = (node as XmlElement).GetAttribute("name");
                i++;
            }

            if (int.TryParse((root as XmlElement).GetAttribute("id"), out id) == false)
            {
                return false;
            }
			else
			{
				this._id = (uint)id;
			}

			nodes = xmlDoc.SelectNodes("/UserDefine/UserData/DataIndexShift");
			if (nodes == null)
			{
				this._dataIndexShift = 0;
			}

			if (int.TryParse(nodes[0].InnerText, out this._dataIndexShift) == false)
			{
				this._dataIndexShift = 0;
			}

			nodes = xmlDoc.SelectNodes("/UserDefine/UserData/LOPSaveItem/*");
			if (nodes == null)
			{
				this._lopItemSelectList = null;
			}
			else
			{
				this._lopItemSelectList = new string[nodes.Count];
				for (int k = 0; k < nodes.Count; k++)
				{
					this._lopItemSelectList[k] = nodes[k].InnerText;
				}
			}

            nodes = xmlDoc.SelectNodes("/UserDefine/FileInfo/UserFileNameFormat/*");
            if (nodes == null)
            {
                this._outputFileNameFormat01 = null;
            }
            else
            {
                this._outputFileNameFormat01 = new string[nodes.Count];
                for (int k = 0; k < nodes.Count; k++)
                {
                    this._outputFileNameFormat01[k] = nodes[k].InnerText;
                }
            }

			nodes = xmlDoc.SelectNodes("/UserDefine/FileInfo/UserFileNameFormat02/*");
			if (nodes == null)
			{
				this._outputFileNameFormat02 = null;
			}
			else
			{
				this._outputFileNameFormat02 = new string[nodes.Count];
				for (int k = 0; k < nodes.Count; k++)
				{
					this._outputFileNameFormat02[k] = nodes[k].InnerText;
				}
			}


            nodes = xmlDoc.SelectNodes("/UserDefine/DCheckingFormat/UIControlMode");
            if (nodes == null | nodes.Count == 0) 
            {
                this._dailyCheckControlsMode = 0;
            }
            else
            {
                if (int.TryParse(nodes[0].InnerText, out this._dailyCheckControlsMode) == false)
                {
                    this._dailyCheckControlsMode = 0;
                }
            }

            //-----------------------------------------------------------------------------------
            //  20130528 Paul
            //  LDT1A Software Clamp Setting  揚州中科需求
            //-----------------------------------------------------------------------------------

            nodes = xmlDoc.SelectNodes("/UserDefine/UserData/IsLDT1ASoftwareClamp");
            if (nodes == null | nodes.Count == 0)
            {
                this._isLDT1ASoftwareClamp = false;
            }
            else
            {
                if (bool.TryParse(nodes[0].InnerText, out this._isLDT1ASoftwareClamp) == false)
                {
                    this._isLDT1ASoftwareClamp = false;
                }
            }

            //-----------------------------------------------------------------------------------
            //  20130530
            // 
            //-----------------------------------------------------------------------------------

            nodes = xmlDoc.SelectNodes("/UserDefine/UserData/IsEnableSkipOptiMsrt");
            if (nodes == null | nodes.Count == 0)
            {
                this._isEnableSkipOptiMsrt = false;
            }
            else
            {
                if (bool.TryParse(nodes[0].InnerText, out this._isEnableSkipOptiMsrt) == false)
                {
                    this._isEnableSkipOptiMsrt = false;
                }
            }

            //-----------------------------------------------------------------------------------
            //  20140506
            // 
            //-----------------------------------------------------------------------------------

            nodes = xmlDoc.SelectNodes("/UserDefine/UserData/ShowCalibrateFile");
            if (nodes == null | nodes.Count == 0)
            {
                this._isEnableShowCalibrateFileLink = false;
            }
            else
            {
                if (bool.TryParse(nodes[0].InnerText, out this._isEnableShowCalibrateFileLink) == false)
                {
                    this._isEnableShowCalibrateFileLink = false;
                }
            }

            nodes = xmlDoc.SelectNodes("/UserDefine/DCheckingFormat/SaveReportName");
            if (nodes == null | nodes.Count == 0)
            {
                //this._isEnableShowCalibrateFileLink = false;
            }
            else
            {
                this._dailyCheckingSaveReportName = nodes[0].InnerText;
            }

            //-----------------------------------------------------------------------------------
            //  20130528 Paul
            //  新廣聯軟體需求 BaseLine BoxCar 
            //-----------------------------------------------------------------------------------

            nodes = xmlDoc.SelectNodes("/UserDefine/UserData/IsEnableSptAdvSetting");
            if (nodes == null | nodes.Count == 0)
            {
                this._isEnableSptAdvSetting = false;
            }
            else
            {
                if (bool.TryParse(nodes[0].InnerText, out this._isEnableSptAdvSetting) == false)
                {
                    this._isEnableSptAdvSetting = false;
                }
            }


            //-----------------------------------------------------------------------------------
            //  20130926 Paul
            //  新廣聯軟體需求 EnableCorrectSpectometerParam
            //-----------------------------------------------------------------------------------

            nodes = xmlDoc.SelectNodes("/UserDefine/UserData/IsEnableCorrectSpectometerParam");
            if(nodes == null | nodes.Count == 0)
            {
                this._isEnableCorrectSpectometerParam = false;
            }
            else
            {
                if(bool.TryParse(nodes[0].InnerText, out this._isEnableCorrectSpectometerParam) == false)
                {
                    this._isEnableCorrectSpectometerParam = false;
                }
            }

            nodes = xmlDoc.SelectNodes("/UserDefine/UserData/IsEnableHighAccuracyMode");
          
            if (nodes == null | nodes.Count == 0)
            {
                this._isEnableHighAccuracyMode = false;
            }
            else
            {
                if (bool.TryParse(nodes[0].InnerText, out this._isEnableHighAccuracyMode) == false)
                {
                    this._isEnableHighAccuracyMode = false;
                }
            }

            nodes = xmlDoc.SelectNodes("/UserDefine/UserData/IsEnableSafetyClamp");
          
            if (nodes == null | nodes.Count == 0)
            {
                this._isEnableSafetyClamp = false;
            }
            else
            {
                if (bool.TryParse(nodes[0].InnerText, out this._isEnableSafetyClamp) == false)
                {
                    this._isEnableSafetyClamp = false;
                }
            }

			nodes = xmlDoc.SelectNodes("/UserDefine/UserData/ReportTempFileKeepDays");
			if (nodes == null | nodes.Count == 0)
			{
				this._reportTempFileKeepDays = 30;
			}
			else
			{
				if (int.TryParse(nodes[0].InnerText, out this._reportTempFileKeepDays) == false)
				{
					this._reportTempFileKeepDays = 30;
				}
				else
				{
					if (this._reportTempFileKeepDays <= 0)
					{
						this._reportTempFileKeepDays = 30;
					}
				}
			}

			nodes = xmlDoc.SelectNodes("/UserDefine/UserData/TestStartIndex");
			if (nodes == null | nodes.Count == 0)
			{
				this._testSTartIndex = 1;
			}
			else
			{
				if (int.TryParse(nodes[0].InnerText, out this._testSTartIndex) == false)
				{
					this._testSTartIndex = 1;
				}
			}

            this._isEnableAutoOrderResultItem=this.GetNodeInnerTextToBool(xmlDoc,"/UserDefine/UserData/EnableAutoOrderResultItem");

            this._isAutoReNameVfItemOfLOPWL = this.GetNodeInnerTextToBool(xmlDoc, "/UserDefine/UserData/IsAutoReNameVfItemOfLOPWL");

            this._isEnableAutoOrderResultItem = this.GetNodeInnerTextToBool(xmlDoc, "/UserDefine/UserData/EnableAutoOrderResultItem");

            this._isShowPassRateChecking = this.GetNodeInnerTextToBool(xmlDoc, "/UserDefine/UIDefine/IsEnablePassRatechecking");

            this._isShowPreSamplingChecking = this.GetNodeInnerTextToBool(xmlDoc, "/UserDefine/UIDefine/IsEnableSamplingRateChecing");

            this._isShowAdjacentChecking = this.GetNodeInnerTextToBool(xmlDoc, "/UserDefine/UIDefine/IsEnableAdjacetChecking");

            this._isPermitSetMsrtItemName = this.GetNodeInnerTextToBool(xmlDoc, "/UserDefine/UIDefine/IsPermitSetMsrtItemName");

            this._isPermitSetMsrtItemUnit = this.GetNodeInnerTextToBool(xmlDoc, "/UserDefine/UIDefine/IsPermitSetMsrtItemUnit");
		//	this._isShowEnableTestGroup = this.GetNodeInnerTextToBool(xmlDoc, "/UserDefine/UIDefine/IsEnableShowTestGroup");

            this._isShowReportInterpolation = this.GetNodeInnerTextToBool(xmlDoc, "/UserDefine/UIDefine/IsEnableReportInterpolation");

            this._isShowEnableTestGroup = this.GetNodeInnerTextToBool(xmlDoc, "/UserDefine/UIDefine/IsEnableShowTestGroup");
         
            this._isEnableLimitMode = true;

            // Load Daily Check Format

            this._dailyCheckFormat.Clear();
            XmlNodeList formatNodes = this._xmlUserDefine.SelectNodes("/UserDefine/DCheckingFormat/Display/*");
            if (formatNodes != null  || formatNodes.Count != 0)
            {
                foreach (XmlElement elem in formatNodes)
                {
                    this._dailyCheckFormat.Add(elem.InnerText);
                }
            }

            this._dailyCheckSaveLogFormat.Clear();
            XmlNodeList formatNodes2 = this._xmlUserDefine.SelectNodes("/UserDefine/DCheckingFormat/SaveLog/*");
            if (formatNodes2 != null || formatNodes2.Count != 0)
            {
                foreach (XmlElement elem in formatNodes2)
                {
                    this._dailyCheckSaveLogFormat.Add(elem.InnerText);
                }
            }

            return true;
        }

        private bool GetNodeInnerTextToBool(XmlDocument xmlDoc, string str)
        {
            bool rtn=true;

            XmlNodeList nodes = xmlDoc.SelectNodes(str);

            if (nodes == null | nodes.Count == 0)
            {
                rtn = false;
            }
            else
            {
                if (bool.TryParse(nodes[0].InnerText, out rtn) == false)
                {
                    rtn = false;
                }
            }
            return rtn;
        }

		public bool ResetUserDefinedName(string byFormatName)
		{
			this._testItemNameDic.Clear();
			this._forceItemNameDic.Clear();
			this._binItemNameDic.Clear();
			this._resultItemNameDic.Clear();
			this._resultItemExtNameDic.Clear();
			this._msrtDisplayItemDic.Clear();

			if (this._xmlUserDefine == null)
				return false;

			XmlNodeList formatNodes = this._xmlUserDefine.SelectNodes("/UserDefine/Formats/*");
			if (formatNodes == null)
			{
				return false;
			}

			foreach (XmlNode node in formatNodes)
			{
				if ((node as XmlElement).GetAttribute("name") == byFormatName)
				{
					//--------------------------------------------------------------------------------------------------
					// The xml node = /UserDefine/Formats/TestItem
					//--------------------------------------------------------------------------------------------------
					XmlNodeList subNodes = node.SelectNodes("TestItem/*");
					foreach (XmlElement elem in subNodes)
					{
						if (this._testItemNameDic.ContainsKey(elem.InnerText))
							return false;

						this._testItemNameDic.Add(elem.InnerText, elem.GetAttribute("name"));
					}
					//--------------------------------------------------------------------------------------------------
					// The xml node = /UserDefine/Formats/ForceElec
					//--------------------------------------------------------------------------------------------------
					subNodes = node.SelectNodes("ForceElec/*");
					foreach (XmlElement elem in subNodes)
					{
						if (this._forceItemNameDic.ContainsKey(elem.InnerText))
							return false;

						this._forceItemNameDic.Add(elem.InnerText, elem.GetAttribute("name"));
					}
					//--------------------------------------------------------------------------------------------------
					// The xml node = /UserDefine/Formats/MsrtDisplayItem
					//--------------------------------------------------------------------------------------------------
					subNodes = node.SelectNodes("MsrtDisplayItem/*");
					foreach (XmlElement elem in subNodes)
					{
						if (this._msrtDisplayItemDic.ContainsKey(elem.InnerText))
							return false;

						TestResultData result = new TestResultData();
						result.KeyName = elem.InnerText;
						result.Name = elem.GetAttribute("name");
						result.Formate = elem.GetAttribute("format");
						result.Unit = elem.GetAttribute("unit");
						if (elem.GetAttribute("enable") == "0")
						{
							result.IsEnable = false;
						}
						else
						{
							result.IsEnable = true;
						}
						this._msrtDisplayItemDic.Add(elem.InnerText, result);
					}

					//--------------------------------------------------------------------------------------------------
					// The xml node = /UserDefine/Formats/ResultItem
					//--------------------------------------------------------------------------------------------------
					subNodes = node.SelectNodes("ResultItem/*");
					foreach (XmlElement elem in subNodes)
					{
						if (this._resultItemNameDic.ContainsKey(elem.InnerText))
							return false;

						if (this._msrtDisplayItemDic.ContainsKey(elem.InnerText))
						{
							this._resultItemNameDic.Add(elem.InnerText, this._msrtDisplayItemDic[elem.InnerText].Name);
						}
						else
						{
							this._resultItemNameDic.Add(elem.InnerText, elem.GetAttribute("name"));
						}
					}

					//--------------------------------------------------------------------------------------------------
					// The xml node = /UserDefine/Formats/ExtResultItem
					//--------------------------------------------------------------------------------------------------
					subNodes = node.SelectNodes("ExtResultItem/*");
					foreach (XmlElement elem in subNodes)
					{
						if (this._resultItemExtNameDic.ContainsKey(elem.InnerText))
							return false;

						if (this._msrtDisplayItemDic.ContainsKey(elem.InnerText))
						{
							this._resultItemExtNameDic.Add(elem.InnerText, this._msrtDisplayItemDic[elem.InnerText].Name);
						}
						else
						{
							this._resultItemExtNameDic.Add(elem.InnerText, elem.GetAttribute("name"));
						}
					}

					//--------------------------------------------------------------------------------------------------
					// The xml node = /UserDefine/Formats/BinItem
					//--------------------------------------------------------------------------------------------------
					subNodes = node.SelectNodes("BinItem/*");
					foreach (XmlElement elem in subNodes)
					{
						if (this._binItemNameDic.ContainsKey(elem.InnerText))
						{
							return false;
						}

						this._binItemNameDic.Add(elem.InnerText, elem.GetAttribute("name"));
					}

					this.CreateDefaultBinItemName();

					break;
				}
			}

			return true;
		}

		public string[] GetResultItemKeyNamesByLopSaveItem(ELOPSaveItem lopSaveItem)
		{
			List<string> keyNames = new List<string>(50);
			keyNames.Clear();

			foreach (KeyValuePair<string, string> pair in this._resultItemNameDic)
			{

				switch (lopSaveItem)
				{
					case ELOPSaveItem.lm:
						if (UserData.ExtractKeyNameLetter(pair.Key) == "LOP" ||
								UserData.ExtractKeyNameLetter(pair.Key) == "WATT")
						{
							continue;
						}
						keyNames.Add(pair.Key);
						break;
					//--------------------------------------------------------------------------------------------------------------
					case ELOPSaveItem.mcd:
						if (UserData.ExtractKeyNameLetter(pair.Key) == "LM" ||
								UserData.ExtractKeyNameLetter(pair.Key) == "WATT")
						{
							continue;
						}
						keyNames.Add(pair.Key);
						break;
					//--------------------------------------------------------------------------------------------------------------
					case ELOPSaveItem.watt:
						if (UserData.ExtractKeyNameLetter(pair.Key) == "LOP" ||
								UserData.ExtractKeyNameLetter(pair.Key) == "LM")
						{
							continue;
						}
						keyNames.Add(pair.Key);
						break;
					//--------------------------------------------------------------------------------------------------------------
					case ELOPSaveItem.mcd_lm:
						if (UserData.ExtractKeyNameLetter(pair.Key) == "WATT")
						{
							continue;
						}
						keyNames.Add(pair.Key);
						break;
					//--------------------------------------------------------------------------------------------------------------
					case ELOPSaveItem.mcd_watt:
						if (UserData.ExtractKeyNameLetter(pair.Key) == "LM")
						{
							continue;
						}
						keyNames.Add(pair.Key);
						break;
					//--------------------------------------------------------------------------------------------------------------
					case ELOPSaveItem.watt_lm:
						if (UserData.ExtractKeyNameLetter(pair.Key) == "LOP")
						{
							continue;
						}
						keyNames.Add(pair.Key);
						break;
					//--------------------------------------------------------------------------------------------------------------
					case ELOPSaveItem.mcd_watt_lm:
						keyNames.Add(pair.Key);
						break;
					//--------------------------------------------------------------------------------------------------------------
					default:
						keyNames.Add(pair.Key);
						break;
				}
			}

			return keyNames.ToArray();
		}

		public string[] GetResultItemNamesByLopSaveItem(ELOPSaveItem lopSaveItem)
		{
			List<string> names = new List<string>(50);
			names.Clear();

			foreach (KeyValuePair<string, string> pair in this._resultItemNameDic)
			{
				switch (lopSaveItem)
				{
					case ELOPSaveItem.lm:
						if (UserData.ExtractKeyNameLetter(pair.Key) == "LOP" ||
								UserData.ExtractKeyNameLetter(pair.Key) == "WATT")
						{
							continue;
						}
						names.Add(pair.Value);
						break;
					//--------------------------------------------------------------------------------------------------------------
					case ELOPSaveItem.mcd:
						if (UserData.ExtractKeyNameLetter(pair.Key) == "LM" ||
								UserData.ExtractKeyNameLetter(pair.Key) == "WATT")
						{
							continue;
						}
						names.Add(pair.Value);
						break;
					//--------------------------------------------------------------------------------------------------------------
					case ELOPSaveItem.watt:
						if (UserData.ExtractKeyNameLetter(pair.Key) == "LOP" ||
								UserData.ExtractKeyNameLetter(pair.Key) == "LM")
						{
							continue;
						}
						names.Add(pair.Value);
						break;
					//--------------------------------------------------------------------------------------------------------------
					case ELOPSaveItem.mcd_lm:
						if (UserData.ExtractKeyNameLetter(pair.Key) == "WATT")
						{
							continue;
						}
						names.Add(pair.Value);
						break;
					//--------------------------------------------------------------------------------------------------------------
					case ELOPSaveItem.mcd_watt:
						if (UserData.ExtractKeyNameLetter(pair.Key) == "LM")
						{
							continue;
						}
						names.Add(pair.Value);
						break;
					//--------------------------------------------------------------------------------------------------------------
					case ELOPSaveItem.watt_lm:
						if (UserData.ExtractKeyNameLetter(pair.Key) == "LOP")
						{
							continue;
						}
						names.Add(pair.Value);
						break;
					//--------------------------------------------------------------------------------------------------------------
					case ELOPSaveItem.mcd_watt_lm:
						names.Add(pair.Value);
						break;
					//--------------------------------------------------------------------------------------------------------------
					default:
						names.Add(pair.Value);
						break;
				}
			}

			return names.ToArray();

		}

		static public string ExtractKeyNameLetter(string keyName)
		{
			if (keyName.IndexOf("_") >= 0)
			{
				return keyName.Remove(keyName.IndexOf("_"));
			}
			else
			{
				return keyName;
			}
		}

		static public int ExtractKeyNameNumber(string keyName)
		{
			if (keyName.IndexOf("_") >= 0)
			{
				return Convert.ToInt32(keyName.Substring(keyName.IndexOf("_") + 1));
			}
			else
			{
				return -1;
			}
		}

		#endregion

	}
}

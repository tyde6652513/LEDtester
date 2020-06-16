using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;
using System.Xml;

using MPI.Tester.Data;
using MPI.Tester;

namespace MPI.Tester.Tools
{
	public class FilesCompare
	{
        protected object _lockObj;
		private const int MAX_TITLE_SEARCH_NUM	= 500;

		protected Dictionary<string, TitleData> _titleInfor;

        protected Dictionary<string, string> _titleName;
        protected Dictionary<string, int> _titleIndex;
		private Dictionary<string,FilterData> _filterDic;
		private Dictionary<string, FilterData> _filterCompareDic;
        private Dictionary<string,int> _filterCounts;
		private Dictionary<string, string> _compTitleName;
		private Dictionary<string, int> _compTitleIndex;

		private int _titleStartIndex;
		private int _dataIndexShift;
        protected DataTable _dtStd;
        protected DataTable _dtMsrt;
        protected DataTable _dtCompare;

        protected Dictionary<string, int> _gainOffsetIndex;
		private Dictionary<string,int[]> _coefIndex;

        private ELOPSaveItem _LOPSaveItem;
		private ECalBaseWave _calBaseWave;
		private bool _isSingleLOPItem;

        private int _mutiFileColumnsNum = 0;
        private int _loadStdCount = 0;
        private int _loadMsrtCount = 0;
        private string[] _currentFileTitle;
        private bool _isArrangeByRowCol;
        private bool _isMultiFileMode;

		public FilesCompare()
		{
			this._lockObj = new object();

			this._titleName = new Dictionary<string,string>();
			this._titleIndex = new Dictionary<string,int>();
			this._titleStartIndex = 0;
			this._dataIndexShift = 0;

			this._dtStd = null;
			this._dtMsrt = null;
			this._dtCompare = null;
            this._LOPSaveItem = ELOPSaveItem.mcd;
			this._calBaseWave = ECalBaseWave.By_WLD;
			this._isSingleLOPItem = false;
            this._isArrangeByRowCol = true;
            this._isMultiFileMode = false;
		}


		#region >>> Public Property <<<

		public Dictionary<string, string> TitleName
		{
			get{ return this._titleName; }
		}

		public Dictionary<string, TitleData> TitleInfor
		{
			get { return this._titleInfor; }
		}

        public Dictionary<string, int> TitleIndex
        {
            get { return this._titleIndex; }
        }
		
		public  Dictionary<string,FilterData> FilterDic
		{
			get{ return this._filterDic; }
		}

		public Dictionary<string, FilterData> FilterCompareDic
		{
			get { return this._filterCompareDic; }
		}

        public Dictionary<string, int> FilterCounts
        {
            get { return this._filterCounts; }
        }

		public DataTable CompareTable
		{
			get{ return this._dtCompare; }
		}
	
        public DataTable StdTable
        {
            get { return this._dtStd; }
        }

        public DataTable MsrtTable
        {
            get { return this._dtMsrt; }
        }
				
		public int RawDataCount
		{
            get
            {
					if ( this._dtStd == null )
					{
						return 0;
					} 
					else
					{
						return this._dtStd.Rows.Count;
					}
				}
		}

		public int FilterDataCount
		{
			get
			{
				if (this._dtCompare == null)
				{
					return 0;
				}
				else
				{
					return this._dtCompare.Rows.Count;
				}
			}
		}

		public double FilterDataPercent
		{
			get
			{
				if ( this._dtStd == null || this._dtCompare == null || this._dtStd.Rows.Count == 0)
				{
					return 0.0d;
				}
				else
				{
					return ((double)this._dtCompare.Rows.Count / (double) (this._loadStdCount));
				}
			}
		}

        public ELOPSaveItem LOPSaveItem
		{
			get { return this._LOPSaveItem; }
			set 
			{ 
				this._LOPSaveItem = value;
				if (this._LOPSaveItem == ELOPSaveItem.mcd || this._LOPSaveItem == ELOPSaveItem.watt || this._LOPSaveItem == ELOPSaveItem.lm)
				{
					this._isSingleLOPItem = true;
				}
				else
				{
					this._isSingleLOPItem = false;
				}
			}
		}

		public ECalBaseWave CalBaseWave
		{
			get { return this._calBaseWave; }
			set { this._calBaseWave = value; }
		}
		
        public Dictionary<string, int[]> CoefTableIndex
         {
			get { return this._coefIndex; }
			set { this._coefIndex = value; }
		}

		public int CoefTableCount
		{
			get{ return this._coefIndex.Count; }
		}

        public int LoadStdDataCount
        {
            get { return this._loadStdCount; }
            set { this._loadStdCount = value; }
        }

        public int LoadMsrtDataCount
        {
            get { return this._loadMsrtCount; }
            set { this._loadMsrtCount = value; }
        }

        public bool IsArrangeByRowCol
        {
            get { return this._isArrangeByRowCol; }
            set { this._isArrangeByRowCol = value; }
        }

        public bool IsMultiFileMode
        {
            get { return this._isMultiFileMode; }
            set { this._isMultiFileMode = value; }
        }

		#endregion

        #region >>> Private Method <<<

        private Dictionary<string, FilterData> GetFilterSetting(XmlNodeList nodes)
        {
            Dictionary<string, FilterData> dicData = new Dictionary<string, FilterData>();

            dicData.Clear();

            double dd;
            string keyName = string.Empty;

            foreach (XmlNode node in nodes)
            {
                keyName = node.InnerText;

                if (this._titleName.ContainsKey(keyName))
                {
                    FilterData filterData = new FilterData(keyName, this._titleName[keyName]);

                    if ((node as XmlElement).GetAttribute("enable") == "1")
                    {
                        filterData.IsEnable = true;
                    }
                    else
                    {
                        filterData.IsEnable = false;
                    }

                    if (double.TryParse((node as XmlElement).GetAttribute("min"), out dd))
                    {
                        filterData.Min = dd;
                    }

                    if (double.TryParse((node as XmlElement).GetAttribute("max"), out dd))
                    {
                        filterData.Max = dd;
                    }
                    filterData.Unit = (node as XmlElement).GetAttribute("unit");

                    if (dicData.ContainsKey(keyName)==false)
                         dicData.Add(keyName, filterData);               
                }
                else
                {
                    FilterData filterData = new FilterData(keyName, "null");
                    dicData.Add(keyName, filterData);
                }
            }
            return dicData;
        }

        private string GetHeader(int idx)
        {
            string sHeader = "";
            int idx1 = 0;
            int idx2 = 0;
            do
            {
                idx1 = idx % 26;
                sHeader = Convert.ToChar('A' + idx1) + sHeader;
                idx2 = idx / 26;
                idx = idx2 - 1;
            } while (idx2 >= 1);
            return sHeader;
        }

        #endregion

        #region >>> Public Method <<<

        /// <summary>
        /// Load Current User Format Form User Data
        /// </summary>
		public bool LoadCurrentFormat( string pathAndFile,string formatName)
		{
            if (File.Exists(pathAndFile) == false)
                return false;

            XmlDocument xmlOutputFormat = new XmlDocument();

            xmlOutputFormat.Load(pathAndFile);
 
            XmlNode root = xmlOutputFormat.DocumentElement;

            XmlNode singlexmlNodeData = xmlOutputFormat.DocumentElement;

            singlexmlNodeData = xmlOutputFormat.SelectSingleNode("/UserDefine/UserData/DataIndexShift");

            if (singlexmlNodeData != null)
            {
                int.TryParse(singlexmlNodeData.InnerText, out this._dataIndexShift);
            }

            XmlNodeList nodes = xmlOutputFormat.SelectNodes("/UserDefine/Formats/*");

			this._titleInfor = new Dictionary<string, TitleData>(50);
            this._titleName = new Dictionary<string, string>(50);
            this._titleIndex = new Dictionary<string, int>(50);
            this._filterDic = new Dictionary<string, FilterData>(50);

            foreach (XmlNode node in nodes)
            {
                string name = (node as XmlElement).GetAttribute("name");

              	string keyname = string.Empty;

                if (name == formatName) //confirm User Format setting 
                {
                    this._titleName.Clear();
					this._titleInfor.Clear();

                    int i = 0;
                    XmlNodeList subNodes = node.SelectNodes("ResultItem/*");

                    foreach (XmlElement elem in subNodes)
                    {
                        keyname = elem.InnerText;

                        if (keyname.IndexOf("_") >= 0)
                        {
                            keyname = keyname.Remove(keyname.IndexOf("_"));

                            if (keyname == "LOP" || keyname == "WATT" || keyname == "LM")
                            {
                                switch (keyname)
                                {
                                    case "LOP":

										if (this.LOPSaveItem.ToString().Contains("mcd"))
										{
											this._titleName.Add(elem.InnerText, elem.GetAttribute("name"));
											this._titleInfor.Add(elem.InnerText, new TitleData(elem.InnerText));
										}
                                        break;
									//-----------------------------------------------------------------------
                                    case "WATT":
                                        if (this.LOPSaveItem.ToString().Contains("watt"))
                                        {
                                            this._titleName.Add(elem.InnerText, elem.GetAttribute("name"));
											this._titleInfor.Add(elem.InnerText, new TitleData(elem.InnerText));
	                                    }
                                        break;
									//-----------------------------------------------------------------------
                                    case "LM":
                                        if (this.LOPSaveItem.ToString().Contains("lm"))
                                        {
                                            this._titleName.Add(elem.InnerText, elem.GetAttribute("name"));
											this._titleInfor.Add(elem.InnerText, new TitleData(elem.InnerText));
                                        }
                                        break;
									//-----------------------------------------------------------------------
                                    default :
                                        break;
                                }
                            }
                            else
                            {
                                this._titleName.Add(elem.InnerText, elem.GetAttribute("name"));
                                this._titleInfor.Add(elem.InnerText, new TitleData(elem.InnerText));
                            }                  
                        }
                        else
                        {
                            this._titleName.Add(elem.InnerText, elem.GetAttribute("name"));
							this._titleInfor.Add(elem.InnerText, new TitleData(elem.InnerText));
                        }
						
						i++;
					}		

					// title Index
                    this._titleIndex.Clear();
                    this._filterDic.Clear();
                    int index=0;
                    foreach (KeyValuePair<string, string> kvp in this._titleName)
                    {
						this._titleIndex.Add(kvp.Key, index);
						this._titleInfor[kvp.Key].Index = index;
                        this._filterDic.Add(kvp.Key, new FilterData(kvp.Key,kvp.Value));
                        index++;
                    }

                    subNodes = node.SelectNodes("MsrtDisplayItem/*");
                    foreach (XmlElement elem in subNodes)
                    {
                        if (this._titleName.ContainsKey(elem.InnerText))
                        {
                            this._titleName[elem.InnerText] = elem.GetAttribute("name");
							this._titleInfor[elem.InnerText].Name = elem.GetAttribute("name");
							this._titleInfor[elem.InnerText].Format = elem.GetAttribute("format");
						}
                    }
				}
             }
				
			return true;
		}

        /// <summary>
        /// Load Filter Data Form Tools XML File
        /// </summary>
		public bool LoadFilterData(string pathAndFile)
		{
			if (File.Exists(pathAndFile) == false)
				return false;

			XmlDocument xmlStdFilter = new XmlDocument();
			xmlStdFilter.Load(pathAndFile);

			XmlNodeList nodes = xmlStdFilter.SelectNodes("/Filter/Std/*");

			this._filterDic = GetFilterSetting(nodes);

			nodes = xmlStdFilter.SelectNodes("/Filter/Compare/*");
			this._filterCompareDic = GetFilterSetting(nodes);

			if (this._filterDic == null || this._filterCompareDic == null)
			{
				return false;
			}
			else
			{
				return true;
			}
		}

        /// <summary>
        /// Load Filter Data Form TestItem and XML File
        /// </summary>
        public bool LoadFilterData(TestItemData[] testItems, string pathAndFile,bool isFellowRecipe)
        {
            if (testItems == null)
            {
                return false;
            }

            if (isFellowRecipe)
            {
                foreach (TestItemData item in testItems)
                {
                    if (item.MsrtResult != null)
                    {
                        foreach (TestResultData data in item.MsrtResult)
                        {
                            if (data.IsEnable && this._filterDic.ContainsKey(data.KeyName))
                            {
                                FilterData filterData = this._filterDic[data.KeyName];
                                filterData.IsEnable = data.IsVerify;
                                filterData.Min = data.MinLimitValue;
                                filterData.Max = data.MaxLimitValue;
                                filterData.Name = data.Name;
                            }
                        }
                    }
                }
            }
            else
            {
                if (File.Exists(pathAndFile) == false)
                    return false;

                XmlDocument xmlStdFilter = new XmlDocument();
                xmlStdFilter.Load(pathAndFile);
                XmlNodeList nodes = xmlStdFilter.SelectNodes("/Filter/Std/*");
                Dictionary<string, FilterData> filterDataByToolSetting = GetFilterSetting(nodes);

                //foreach (TestItemData item in testItems)
                //{
                //    if (item.MsrtResult != null)
                //    {
                //        foreach (TestResultData data in item.MsrtResult)
                //        {
                //            if (this._filterDic.ContainsKey(data.KeyName))
                //            {
                //                if (data.IsEnable)
                //                {
                //                    if (filterDataByToolSetting.ContainsKey(data.KeyName))
                //                    {
                //                        FilterData filterData = filterDataByToolSetting[data.KeyName];

                //                        this._filterDic[data.KeyName].IsEnable = filterData.IsEnable;
                //                        this._filterDic[data.KeyName].Min = filterData.Min;
                //                        this._filterDic[data.KeyName].Max = filterData.Max;
                //                        this._filterDic[data.KeyName].Name = filterData.Name;
                //                    }
                //                }
                //                else
                //                {
                //                    this._filterDic[data.KeyName].IsEnable = false;
                //                }
                //            }
                //        }
                //    }
                //}

                // 保留原本使用的Filter Dic 
                // 即使 TestItem Disable後，仍會保留數據
                // 這方法會預到一個問題點

                foreach (KeyValuePair<string, FilterData> kvp in this._filterDic)
                {
                    if (filterDataByToolSetting.ContainsKey(kvp.Key))
                    {
                        kvp.Value.IsEnable = filterDataByToolSetting[kvp.Key].IsEnable;
                        kvp.Value.Max = filterDataByToolSetting[kvp.Key].Max;
                        kvp.Value.Min = filterDataByToolSetting[kvp.Key].Min;
                        kvp.Value.Name = filterDataByToolSetting[kvp.Key].Name;
                    }
                }
            }
            return true;
        }

		public void ParseTitleDataIndex( TestItemData[] testItems)
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
			this._coefIndex = new Dictionary<string, int[]>();

			this._gainOffsetIndex.Clear();
			this._coefIndex.Clear();

			for (int i = 0; i < keyNames.Length; i++)
			{
				if (keyNames[i].IndexOf("_") >= 0)
				{
					str = keyNames[i].Remove(keyNames[i].IndexOf("_"));
					strNum = keyNames[i].Substring(keyNames[i].IndexOf("_") + 1);

					if (str == "LOP" || str == "WATT" || str == "LM" ||
                        str == "WLD" || str == "WLP" || str == "WLC" || str == "HW")
					{
						if (this._coefIndex.ContainsKey(strNum) == false)
						{
							this._coefIndex.Add(strNum, new int[7] { -1, -1, -1, -1, -1, -1, -1 });
						}
					}

					switch (str)
					{
						case "WLP":
							this._coefIndex[strNum][0] = i;
							break;
						//---------------------------------------------------------
						case "WLD":
							this._coefIndex[strNum][1] = i;
							break;
						//---------------------------------------------------------
						case "WLC":
							this._coefIndex[strNum][2] = i;
							break;
						//---------------------------------------------------------
						case "LOP":
							if (this._isSingleLOPItem)
							{
								if (this._LOPSaveItem == ELOPSaveItem.mcd)
								{
									this._coefIndex[strNum][3] = i;
								}
								else if (this._LOPSaveItem == ELOPSaveItem.watt)
								{
									this._coefIndex[strNum][4] = i;
								}
								else if (this._LOPSaveItem == ELOPSaveItem.lm)
								{
									this._coefIndex[strNum][5] = i;
								}
							}
							else
							{
								this._coefIndex[strNum][3] = i;
							}
							break;
						//---------------------------------------------------------
						case "WATT":
							this._coefIndex[strNum][4] = i;
							break;
						//---------------------------------------------------------
						case "LM":
							this._coefIndex[strNum][5] = i;
							break;
						//---------------------------------------------------------
						case "HW":
							this._coefIndex[strNum][6] = i;
							break;
						//---------------------------------------------------------
						case "MTHYVP":
						case "MTHYVD":
						case "MVF":
						case "MVFLA":
						case "MVZ":
						case "MIR":
						case "MIF":
						case "CIEx":
						case "CIEy":						
						case "CCT":
						case "CRI":
						case "PURITY":
						case "R01":
						case "R02":
						case "R03":
						case "R04":
						case "R05":
						case "R06":
						case "R07":
						case "R08":
						case "R09":
						case "R10":
						case "R11":
						case "R12":
						case "R13":
						case "R14":
						case "R15":
						//AC Item
						case "ACMIF":
						case "ACPOWER":
						case "ACAPPRARENT":
						case "ACPF":
						case "ACFREQUENCY":
						case "ACPEAK":
						case "ACPEAKMAX":
                        case "ACMIFL":
                        case "ACPOWERL":
                        case "ACAPPRARENTL":
                        case "ACPFL":
                        case "ACFREQUENCYL":
                        case "ACPEAKL":
                        case "ACPEAKMAXL":
						case "MVFMA":
						case "MVFMB":
						case "MVFMC":
						case "MVFMD":
                        case "PDMVF":
                        case "PDWATT": // 20160420, Roy 是否要多波段校正? 再討論
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
							}
						}
					}
				}
			}

		}

        public void SaveFilterDataToFile(string pathAndFileName)
        {
          //  string pathAndFileName = Path.Combine(Constants.Paths.TOOLS_DIR, "FilterCriteria.xml");
            XmlDocument xmlFilter = new XmlDocument();
            string str = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                             "<?xml-stylesheet type=\"text/xsl\"" +
                             " href=\"FormatA.xslt\"?><Filter></Filter>";

            xmlFilter.LoadXml(str);
            XmlNode rootNode = xmlFilter.DocumentElement;

            //----------------------------------------------------------
            // Append "ItemData" element to XML
            //----------------------------------------------------------

            XmlElement stdElemSet = xmlFilter.CreateElement("Std");
            rootNode.AppendChild(stdElemSet);

            //XmlElement compareElemSet = xmlFilter.CreateElement("Compare");
            //rootNode.AppendChild(compareElemSet);

            //----------------------------------------------------------
            // Write Filter Setting To XML
            //-------------------------------------------------------------
            int index = 0;
            foreach (string keyName in this.FilterDic.Keys)
            {
                XmlElement stdElem = xmlFilter.CreateElement(this.GetHeader(index));
                stdElem.InnerText = keyName;
                FilterData filterData = this.FilterDic[keyName];
                stdElem.SetAttribute("name", filterData.Name.ToString());

                if (filterData.IsEnable)
                {
                    stdElem.SetAttribute("enable", "1");
                }
                else
                {
                    stdElem.SetAttribute("enable", "0");
                }
                stdElem.SetAttribute("max", filterData.Max.ToString());
                stdElem.SetAttribute("min", filterData.Min.ToString());
                stdElemSet.AppendChild(stdElem);
                index++;
            }
            XmlTextWriter xtw = new XmlTextWriter(pathAndFileName, null);
            xtw.Formatting = System.Xml.Formatting.Indented;
            xmlFilter.Save(xtw);
            xtw.Close();
        }

        public string[] GetStdAndCurrentFileTitleName()
        {
            if(this._currentFileTitle==null)
            {
                return new string[1]{"NoCurrentData"};
            }
            string[] stdAndCurrentFileTitle=new string[this._titleName.Count];

            int index=0;

            foreach( KeyValuePair<string,string> kvp in this._titleName )
			{
                string result ="PASS";

                 if(kvp.Value!=this._currentFileTitle[index])
                 {
                     result="NO";
                 }
                 stdAndCurrentFileTitle[index] = kvp.Value + " , " + this._currentFileTitle[index] + " , " + result;
                 index++;
            }
                return stdAndCurrentFileTitle;
        }        		

        public EErrorCode LoadStdFromFile(string pathAndFile, int index, bool isIgnoreTitleMatching = false)
        {
            if (File.Exists(pathAndFile) == false)
                return EErrorCode.Tools_LoadStdDataFail;

            if (!this._titleIndex.ContainsKey("ROW") && !this._titleIndex.ContainsKey("COL")
                && this._isArrangeByRowCol)
            {
                return EErrorCode.Tools_UserDataNotDefineRowCol;
            }
            else if (!this._titleIndex.ContainsKey("TEST") && this._isArrangeByRowCol==false)
            {
                return EErrorCode.Tools_UserDataNotDefineSeq;
            }

            bool isFindTitle = false;
            List<string[]> strData = MPI.Tester.CSVUtil.ReadCSV(pathAndFile);

            if (strData == null)
                return EErrorCode.Tools_LoadStdDataFail;

            int Index = 0;
            string[] compareStrArray = new string[this._titleName.Count];
            string[] titleStr01 = this._titleName.Values.ToArray();
            this._titleStartIndex = 0;

            for (Index = 0; Index < MAX_TITLE_SEARCH_NUM && Index < strData.Count; Index++)
            {
                if (strData[Index].Length < compareStrArray.Length)
                {
                    Array.Copy(strData[Index], compareStrArray, strData[Index].Length);
                }
                else
                {
                    Array.Copy(strData[Index], compareStrArray, compareStrArray.Length);
                }

                if (isFindTitle == false && string.Equals(titleStr01[0].Replace(" ", ""), compareStrArray[0].Replace(" ", "")))
                {
                    this._currentFileTitle = compareStrArray;

                    if (isIgnoreTitleMatching)
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            if (titleStr01[i].Replace(" ", "") != compareStrArray[i].Replace(" ", ""))
                            {
                                return EErrorCode.Tools_UserDataStdTitleIsNotMatch;
                            }
                        }
                    }
                    else
                    {
                    for (int i = 0; i < titleStr01.Length - 10; i++)
                    {
                        if (titleStr01[i].Replace(" ", "") != compareStrArray[i].Replace(" ", ""))
                        {
                            return EErrorCode.Tools_UserDataStdTitleIsNotMatch;
                        }
                    }
                    }

                    isFindTitle = true;
                    this._titleStartIndex = Index+1;

                    break;
                }
            }
            // Can't find the title of the std file for the current format
            if (isFindTitle == false)
                return EErrorCode.Tools_UserDataStdTitleIsNotMatch;

            if (index == 1 || index == 0)
            {
                this._dtStd = new DataTable("StdData");
                DataColumn[] dtColArray = new DataColumn[this._titleName.Count + 1];
                _mutiFileColumnsNum = this._titleName.Count + 1;
                dtColArray[0] = new DataColumn("KeyID", System.Type.GetType("System.String"));
                dtColArray[0].Caption = "KeyID";
                dtColArray[0].Unique = true;

                for (int j = 1; j < dtColArray.Length; j++)
                {
                    dtColArray[j] = new DataColumn(this._titleName.Keys.ToArray()[j - 1], System.Type.GetType("System.Double"));
                    dtColArray[j].Caption = this._titleName.Values.ToArray()[j - 1];
                }
                this._dtStd.Columns.AddRange(dtColArray);
                this._dtStd.PrimaryKey = new DataColumn[] { this._dtStd.Columns["KeyID"] };
            }
            
            double dd;
            for (int m = (this._titleStartIndex + this._dataIndexShift); m < strData.Count; m++)
            {
                DataRow rowData = this._dtStd.NewRow();

                if (this._isArrangeByRowCol == true )
                {
                    if (this._titleIndex.ContainsKey("ROW") && this._titleIndex.ContainsKey("COL"))
                    {
                        if (index == 0) // Index=0 Single Data Compare
                        {
                            rowData[0] = "X" + strData[m][this._titleIndex["COL"]].Replace(" ", "") + "Y" + strData[m][this._titleIndex["ROW"]].Replace(" ", "");	// primaryKey = (string) X____Y____;
                        }
                        else
                        {
                            rowData[0] = "X" + (index * 1000) + strData[m][this._titleIndex["COL"]].Replace(" ", "") + "Y" + strData[m][this._titleIndex["ROW"]].Replace(" ", "");	// primaryKey = (string) X____Y____;
                        }
                    }
                    else
                    {
                        rowData[0] = string.Empty;
                    }
                }
                else
                {
                    if (index == 0)
                    {
                        rowData[0] = "TEST" + strData[m][this._titleIndex["TEST"]].Replace(" ", ""); // primaryKey = (string) X____Y____;
                    }
                    else
                    {
                        rowData[0] = "TEST" + strData[m][this._titleIndex["TEST"]].Replace(" ", "") + "_" + (index); // primaryKey = (string) X____Y____;
                    }
                    // rowData[0] = "X" + strData[m][this._titleIndex["TEST"]].Replace(" ", "") + "Y" + strData[m][this._titleIndex["TEST"]].Replace(" ", ""); // primaryKey = (string) X____Y____;
                }
               // rowData[0] = "X" + (index*1000)+strData[m][this._titleIndex["COL"]] + "Y" + strData[m][this._titleIndex["ROW"]];	// primaryKey = (string) X____Y____;

                for (int n = 1; n < _mutiFileColumnsNum; n++)
                {
                    if (n > strData[m].Length)
                    {
                        rowData[n] = 0.0d;
                        continue;
                    }

                    if (Double.TryParse(strData[m][n - 1].Replace(" ", ""), out dd))
                    {
                        rowData[n] = dd;
                    }
                    else
                    {
                        rowData[n] = 0.0d;
                    }
                }

                if (this._dtStd.Rows.Contains(rowData["KeyID"]) || rowData[0] == string.Empty)
                {
                    continue;
                }
                else
                {
                    this._dtStd.Rows.Add(rowData);
                    this._loadStdCount++;
                }
            }

            return EErrorCode.NONE;

        }

        public EErrorCode LoadMsrtFromFile(string pathAndFile, int index, bool isIgnoreTitleMatching = false)
        {
            if (File.Exists(pathAndFile) == false)
                return EErrorCode.Tools_LoadMsrtDataFail;

            if (!this._titleIndex.ContainsKey("ROW") && !this._titleIndex.ContainsKey("COL")
            && this._isArrangeByRowCol)
            {
                return EErrorCode.Tools_UserDataNotDefineRowCol;
            }
            else if (!this._titleIndex.ContainsKey("TEST") && this._isArrangeByRowCol == false)
            {
                return EErrorCode.Tools_UserDataNotDefineSeq;
            }

            bool isFindTitle = false;
            List<string[]> msrtData = CSVUtil.ReadCSV(pathAndFile);

            if (msrtData == null)
                return EErrorCode.Tools_LoadMsrtDataFail;

            int Index = 0;
            string[] compareStrArray = new string[this._titleName.Count];
            string[] titleStr01 = this._titleName.Values.ToArray();
            this._titleStartIndex = 0;

            for (Index = 0; Index < MAX_TITLE_SEARCH_NUM && Index < msrtData.Count; Index++)
            {
                if (msrtData[Index].Length < compareStrArray.Length)
                {
                    Array.Copy(msrtData[Index], compareStrArray, msrtData[Index].Length);
                }
                else
                {
                    Array.Copy(msrtData[Index], compareStrArray, compareStrArray.Length);
                }

                if (isFindTitle == false && string.Equals(titleStr01[0].Replace(" ", ""), compareStrArray[0].Replace(" ", "")) && string.Equals(titleStr01[1].Replace(" ", ""), compareStrArray[1].Replace(" ", "")))
                {
                    this._currentFileTitle = compareStrArray;

                    if (isIgnoreTitleMatching)
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            if (titleStr01[i].Replace(" ", "") != compareStrArray[i].Replace(" ", ""))
                            {
                                return EErrorCode.Tools_UserDataStdTitleIsNotMatch;
                            }
                        }
                    }
                    else
                    {
                    for (int i = 0; i < titleStr01.Length-10; i++)
                    {
                        if (titleStr01[i].Replace(" ", "") != compareStrArray[i].Replace(" ", ""))
                        {
                            return  EErrorCode.Tools_UserDataMsrtTitleIsNotMatch;
                        }
                    }
                    }
                    isFindTitle = true;
                    this._titleStartIndex = Index+1;

                    break;
                }
            }

            // Can't find the title of the std file for the current format
            if (isFindTitle == false)
                return   EErrorCode.Tools_UserDataMsrtTitleIsNotMatch;

            if (index == 1 || index==0)
            {
                this._dtMsrt = new DataTable("MsrtData");
                DataColumn[] dtColArray = new DataColumn[this._titleName.Count + 1];

                dtColArray[0] = new DataColumn("KeyID", System.Type.GetType("System.String"));
                dtColArray[0].Caption = "KeyID";
                dtColArray[0].Unique = true;

                for (int j = 1; j < dtColArray.Length; j++)
                {
                    dtColArray[j] = new DataColumn(this._titleName.Keys.ToArray()[j - 1], System.Type.GetType("System.Double"));
                    dtColArray[j].Caption = this._titleName.Values.ToArray()[j - 1];
                }

                this._dtMsrt.Columns.AddRange(dtColArray);
                this._dtMsrt.PrimaryKey = new DataColumn[] { this._dtMsrt.Columns["KeyID"] };
            }

            double dd = 0;

            for (int m = (this._titleStartIndex + this._dataIndexShift); m < msrtData.Count; m++)
            {
                DataRow rowData = this._dtMsrt.NewRow();

                if (this._isArrangeByRowCol == true)
                {
                    if (this._titleIndex.ContainsKey("COL") && this._titleIndex.ContainsKey("ROW"))
                    {
                        if (index == 0)
                        {
                            rowData[0] = "X" + msrtData[m][this._titleIndex["COL"]].Replace(" ", "") + "Y" + msrtData[m][this._titleIndex["ROW"]].Replace(" ", "");	// primaryKey = (string) X____Y____;
                        }
                        else
                        {
                            rowData[0] = "X" + (index * 1000) + msrtData[m][this._titleIndex["COL"]].Replace(" ", "") + "Y" + msrtData[m][this._titleIndex["ROW"]].Replace(" ", "");	// primaryKey = (string) X____Y____;
                        }
                    }
                }
                else
                {
                    if (index == 0)
                    {
                        rowData[0] = "TEST" + msrtData[m][this._titleIndex["TEST"]].Replace(" ", ""); // primaryKey = (string) X____Y____;
                    }
                    else
                    {
                        rowData[0] = "TEST" + msrtData[m][this._titleIndex["TEST"]].Replace(" ", "") + "_" + (index); // primaryKey = (string) X____Y____;
                    }
                    // rowData[0] = "X" + strData[m][this._titleIndex["TEST"]].Replace(" ", "") + "Y" + strData[m][this._titleIndex["TEST"]].Replace(" ", ""); // primaryKey = (string) X____Y____;
                }
               
              //  rowData[0] = "X" + (1000*index)+msrtData[m][this._titleIndex["COL"]] + "Y" + msrtData[m][this._titleIndex["ROW"]]; // primaryKey = (string) X____Y____;
                for (int n = 1; n < this._mutiFileColumnsNum; n++)
                {
                    if (n > msrtData[m].Length)
                    {
                        rowData[n] = 0.0d;
                        continue;
                    }

                    if (Double.TryParse(msrtData[m][n - 1].Replace(" ", ""), out dd))
                    {
                        rowData[n] = dd;
                    }
                    else
                    {
                        rowData[n] = 0.0d;
                    }
                }

                if (this._dtMsrt.Rows.Contains(rowData["KeyID"]) || rowData[0] == string.Empty)
                {
                    continue;
                }
                else
                {
                    this._dtMsrt.Rows.Add(rowData);
                    this._loadMsrtCount++;
                }
            }

            return EErrorCode.NONE;
      
        }

        public EErrorCode LoadMsrtFromFile(string pathAndFile, int index, Dictionary<string, CalcGainOffset[]> calcData, bool isIgnoreTitleMatching = false)
        {
            if (File.Exists(pathAndFile) == false)
                return EErrorCode.Tools_LoadMsrtDataFail;

            if (!this._titleIndex.ContainsKey("ROW") && !this._titleIndex.ContainsKey("COL")
            && this._isArrangeByRowCol)
            {
                return EErrorCode.Tools_UserDataNotDefineRowCol;
            }
            else if (!this._titleIndex.ContainsKey("TEST") && this._isArrangeByRowCol == false)
            {
                return EErrorCode.Tools_UserDataNotDefineSeq;
            }

            Dictionary<int, CalcGainOffset[]> comoensateData = new Dictionary<int, CalcGainOffset[]>();

            foreach (var item in calcData)
            {
                if (this.TitleIndex.ContainsKey(item.Key))
                {
                    comoensateData.Add(this.TitleIndex[item.Key], item.Value);
                }
            }

            int chIndex=0;

            if(this.TitleIndex.ContainsKey("CHANNEL"))
            {
                chIndex=this.TitleIndex["CHANNEL"];
            }

            bool isFindTitle = false;

            List<string[]> msrtData = CSVUtil.ReadCSV(pathAndFile);

            if (msrtData == null)
                return EErrorCode.Tools_LoadMsrtDataFail;

            int Index = 0;

            string[] compareStrArray = new string[this._titleName.Count];

            string[] titleStr01 = this._titleName.Values.ToArray();

            this._titleStartIndex = 0;

            for (Index = 0; Index < MAX_TITLE_SEARCH_NUM && Index < msrtData.Count; Index++)
            {
                if (msrtData[Index].Length < compareStrArray.Length)
                {
                    Array.Copy(msrtData[Index], compareStrArray, msrtData[Index].Length);
                }
                else
                {
                    Array.Copy(msrtData[Index], compareStrArray, compareStrArray.Length);
                }

                if (isFindTitle == false && string.Equals(titleStr01[0].Replace(" ", ""), compareStrArray[0].Replace(" ", "")) && string.Equals(titleStr01[1].Replace(" ", ""), compareStrArray[1].Replace(" ", "")))
                {
                    this._currentFileTitle = compareStrArray;
                    if (isIgnoreTitleMatching)
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            if (titleStr01[i].Replace(" ", "") != compareStrArray[i].Replace(" ", ""))
                            {
                                return EErrorCode.Tools_UserDataStdTitleIsNotMatch;
                            }
                        }
                    }
                    else
                    {
                    for (int i = 0; i < titleStr01.Length - 10; i++)
                    {
                        if (titleStr01[i].Replace(" ", "") != compareStrArray[i].Replace(" ", ""))
                        {
                            return EErrorCode.Tools_UserDataMsrtTitleIsNotMatch;
                        }
                    }
                    }

                    isFindTitle = true;
                    this._titleStartIndex = Index + 1;

                    break;
                }
            }

            // Can't find the title of the std file for the current format
            if (isFindTitle == false)
                return EErrorCode.Tools_UserDataMsrtTitleIsNotMatch;

            if (index == 1 || index == 0)
            {
                this._dtMsrt = new DataTable("MsrtData");

                DataColumn[] dtColArray = new DataColumn[this._titleName.Count + 1];

                dtColArray[0] = new DataColumn("KeyID", System.Type.GetType("System.String"));
                dtColArray[0].Caption = "KeyID";
                dtColArray[0].Unique = true;

                for (int j = 1; j < dtColArray.Length; j++)
                {
                    dtColArray[j] = new DataColumn(this._titleName.Keys.ToArray()[j - 1], System.Type.GetType("System.Double"));
                    dtColArray[j].Caption = this._titleName.Values.ToArray()[j - 1];
                }

                this._dtMsrt.Columns.AddRange(dtColArray);
                this._dtMsrt.PrimaryKey = new DataColumn[] { this._dtMsrt.Columns["KeyID"] };
            }

            double dd = 0;

            for (int m = (this._titleStartIndex + this._dataIndexShift); m < msrtData.Count; m++)
            {
                DataRow rowData = this._dtMsrt.NewRow();

                if (this._isArrangeByRowCol == true)
                {
                    if (this._titleIndex.ContainsKey("COL") && this._titleIndex.ContainsKey("ROW"))
                    {
                        if (index == 0)
                        {
                            rowData[0] = "X" + msrtData[m][this._titleIndex["COL"]].Replace(" ", "") + "Y" + msrtData[m][this._titleIndex["ROW"]].Replace(" ", "");	// primaryKey = (string) X____Y____;
                        }
                        else
                        {
                            rowData[0] = "X" + (index * 1000) + msrtData[m][this._titleIndex["COL"]].Replace(" ", "") + "Y" + msrtData[m][this._titleIndex["ROW"]].Replace(" ", "");	// primaryKey = (string) X____Y____;
                        }
                    }
                }
                else
                {
                    if (index == 0)
                    {
                        rowData[0] = "TEST" + msrtData[m][this._titleIndex["TEST"]].Replace(" ", ""); // primaryKey = (string) X____Y____;
                    }
                    else
                    {
                        rowData[0] = "TEST" + msrtData[m][this._titleIndex["TEST"]].Replace(" ", "") + "_" + (index); // primaryKey = (string) X____Y____;
                    }
                    // rowData[0] = "X" + strData[m][this._titleIndex["TEST"]].Replace(" ", "") + "Y" + strData[m][this._titleIndex["TEST"]].Replace(" ", ""); // primaryKey = (string) X____Y____;
                }

                //  rowData[0] = "X" + (1000*index)+msrtData[m][this._titleIndex["COL"]] + "Y" + msrtData[m][this._titleIndex["ROW"]]; // primaryKey = (string) X____Y____;
                for (int n = 1; n < this._mutiFileColumnsNum; n++)
                {
                    if (n > msrtData[m].Length)
                    {
                        rowData[n] = 0.0d;
                        continue;
                    }

                    if (Double.TryParse(msrtData[m][n - 1].Replace(" ", ""), out dd))
                    {
                        if (comoensateData.ContainsKey(n-1))
                        {
                            int chipChannelIndex = -1;

                            int.TryParse(msrtData[m][chIndex], out chipChannelIndex);

                            chipChannelIndex = chipChannelIndex - 1;

                            if (chipChannelIndex >= 0)
                            {
                                dd = dd * comoensateData[n-1][chipChannelIndex].Gain + comoensateData[n-1][chipChannelIndex].Offset;
                            }
                        }

                        rowData[n] = dd;
                    }
                    else
                    {
                        rowData[n] = 0.0d;
                    }
                }

                if (this._dtMsrt.Rows.Contains(rowData["KeyID"]) || rowData[0] == string.Empty)
                {
                    continue;
                }
                else
                {
                    this._dtMsrt.Rows.Add(rowData);
                    this._loadMsrtCount++;
                }
            }

            return EErrorCode.NONE;

        }

        public void FilterOrignalData(TestItemData[] items)
		{
			string filterExpress = string.Empty;
			DataRow[] rows = null;

			if (this._filterDic == null || this._dtStd== null || this._dtMsrt == null )
				return;

            Dictionary<string, bool> itemEnableDic = new Dictionary<string, bool>();

            foreach (TestItemData item in items)
            {
                if(item.MsrtResult!=null)
                {
                    foreach(TestResultData data in item.MsrtResult)
                    {
                        itemEnableDic.Add(data.KeyName, data.IsEnable);
                    }

                }
            }

            _filterCounts = new Dictionary<string, int>(this._filterDic.Count);

			foreach( KeyValuePair<string,FilterData> kvp in this._filterDic )
			{
                int stdFilterCounts = 0;
                int msrtFilterCounts = 0;

                if (!itemEnableDic.ContainsKey(kvp.Key))
                {
                    continue;
                }

				if ( kvp.Value.IsEnable == true )
				{
					filterExpress = kvp.Key + "<" + kvp.Value.Min.ToString() + " or " + kvp.Key + ">" + kvp.Value.Max.ToString();

					// filter std. data 
					rows = this._dtStd.Select(filterExpress);
					if ( rows != null )
					{
						for( int i = 0; i < rows.Length; i++ )
						{
							rows[i].Delete();
                            stdFilterCounts++;
						}
					}
					
					// filter Msrt. Data
					rows = this._dtMsrt.Select(filterExpress);
					if (rows != null)
					{
						for (int i = 0; i < rows.Length; i++)
						{
							rows[i].Delete();
                            msrtFilterCounts++;
						}
					}
				}
                this._filterDic[kvp.Key].StdFilterCounts = stdFilterCounts;
                this._filterDic[kvp.Key].MsrtFilterCounts = msrtFilterCounts;
			}

			this._dtStd.AcceptChanges();
			this._dtMsrt.AcceptChanges();	
		}
		
		public void CompareStdAndMsrt()
		{
			if ( this._dtStd == null || this._dtMsrt == null ) 
				return;

			// Create Compare data table
			this._dtCompare = new DataTable("CompData");
			DataColumn[] dtColArray = new DataColumn[ ( this._dtStd.Columns.Count - 1 ) * 3+ 1 ];

			dtColArray[0] = new DataColumn("KeyID", System.Type.GetType("System.String"));				// index = 0, primaryKey
			dtColArray[0].Caption = "KeyID";
			dtColArray[0].Unique = true;

			int index = 0;
			this._compTitleName = new Dictionary<string,string>();
			this._compTitleIndex = new Dictionary<string,int>();
			this._compTitleName.Clear();
			this._compTitleIndex.Clear();

			for (int j = 1; j < this._dtStd.Columns.Count; j++)
			{
				string newkey = "S_" + this._dtStd.Columns[j].ColumnName;				
				index = (j - 1) * 3 + 1;
				dtColArray[index] = new DataColumn(newkey, System.Type.GetType("System.Double"));		// index = 1, 4, 7, ...
				dtColArray[index].Caption = "S_" + this._dtStd.Columns[j].Caption; ;
				this._compTitleName.Add(newkey, "S_" + this._dtStd.Columns[j].Caption);
				this._compTitleIndex.Add(newkey, index);

				index = (j - 1) * 3 + 2;
				newkey = "M_" + this._dtMsrt.Columns[j].ColumnName;
				dtColArray[index] = new DataColumn(newkey, System.Type.GetType("System.Double"));		// index = 2, 5, 8, ...
				dtColArray[index].Caption = "M_" + this._dtMsrt.Columns[j].Caption;
				this._compTitleName.Add(newkey, "M_" + this._dtMsrt.Columns[j].Caption);
				this._compTitleIndex.Add(newkey, index);

				index = (j - 1) * 3 + 3;
				newkey = "D_" + this._dtMsrt.Columns[j].ColumnName;
				dtColArray[index] = new DataColumn(newkey, System.Type.GetType("System.Double"));		// index = 3, 6, 9, ...
				dtColArray[index].Caption = "D_" + this._dtMsrt.Columns[j].Caption;
				this._compTitleName.Add(newkey, "D_" + this._dtMsrt.Columns[j].Caption);
				this._compTitleIndex.Add(newkey, index);
			}

			this._dtCompare.Columns.AddRange(dtColArray);
			this._dtCompare.PrimaryKey = new DataColumn[] { this._dtCompare.Columns["KeyID"] };

			string gainKeyName;

			for (int i = 0; i < this._dtStd.Rows.Count; i++)
			{
				DataRow rowData = this._dtCompare.NewRow();
				rowData[0] = this._dtStd.Rows[i]["KeyID"];
				if ( this._dtMsrt.Rows.Contains(this._dtStd.Rows[i]["KeyID"]) )
				{
					for( int j = 1; j < this._dtStd.Columns.Count; j++ )
					{
						rowData[(j - 1) * 3 + 1] = this._dtStd.Rows[i][j];																	// index = 1, 4, 7, ...
						rowData[(j - 1) * 3 + 2] = this._dtMsrt.Rows.Find(rowData[0])[j];												// index = 2, 5, 8, ...

						gainKeyName = this._dtStd.Columns[j].ColumnName;
						
						if ( gainKeyName.IndexOf("_") >= 0 )
						{
							gainKeyName = gainKeyName.Remove(gainKeyName.IndexOf("_"));
						}

                        if (gainKeyName == "LOP" || gainKeyName == "WATT" || gainKeyName == "LM" || 
                            gainKeyName == "PDWATT" || gainKeyName == "PDMCD" ||
                            gainKeyName == "PfA" || gainKeyName == "PfB" || gainKeyName == "PfC" ||
                            gainKeyName == "PceA" || gainKeyName == "PceB" || gainKeyName == "PceC")    // 20160420, Roy : 新增LD校正項目 PDWATT
						{
							if (  (double)rowData[(j - 1) * 3 + 1] == 0.0d)
							{
								rowData[(j - 1) * 3 + 3] = 0.0d;
							}
							else if (  (double)rowData[(j - 1) * 3 + 2] == 0.0d )
							{
								rowData[(j - 1) * 3 + 3] = double.MaxValue;
							}
							else
							{
								rowData[(j - 1) * 3 + 3] = (double)rowData[(j - 1) * 3 + 1] / (double)rowData[(j - 1) * 3 + 2];
							}
						}
						else
						{
							rowData[(j - 1) * 3 + 3] = (double)rowData[(j - 1) * 3 + 1] - (double)rowData[(j - 1) * 3 + 2];		// index = 3, 6, 9, ...  Offset = ( Std- Msrt )
						}
					}

					if (this._dtCompare.Rows.Contains(rowData["KeyID"]))
					{
						break;
					}
					else
					{
						this._dtCompare.Rows.Add(rowData);
					}
				}
			}

			
		}

		public bool SaveCompareTable(string pathAndFileName)
		{
			if ( this._dtCompare == null )
				return false;

			List<string[]> ls = new List<string[]>(this._dtCompare.Rows.Count);

			string[] rowStr;
			rowStr = new string [this._dtCompare.Columns.Count];
			for(int i = 0; i < this._dtCompare.Columns.Count; i++ )
			{
				rowStr[i] = this._dtCompare.Columns[i].Caption;
			}
			ls.Add(rowStr);

			for( int i = 0; i < this._dtCompare.Rows.Count; i++)
			{
				rowStr = new string[ this._dtCompare.Columns.Count ];
				rowStr[0] = (string)this._dtCompare.Rows[i]["KeyID"];
				for( int j = 1; j< this._dtCompare.Columns.Count; j++ )
				{
					rowStr[j] = ((double)this._dtCompare.Rows[i][j]).ToString("0.00000");
				}

				ls.Add(rowStr);
				
			}

			return CSVUtil.WriteCSV(pathAndFileName, ls);
		}

		public void FilterCompareData()
		{
			string filterExpress = string.Empty;
			DataRow[] rows = null;

			double lowBond;
			double upBond; 

			if (this._filterCompareDic == null)
				return;

			int titleItemIndex = 0;
			int index = 0;
			string dtCompColKeyName;
			string itemName = string.Empty;
				
			foreach (KeyValuePair<string, FilterData> kvp in this._filterCompareDic)
			{
                if (kvp.Value.Name == "null")
                {
                    continue;
                }

				itemName = kvp.Key;

				if (itemName.IndexOf("_") > 0)
				{
					itemName = itemName.Remove(itemName.IndexOf("_"));
				}

				titleItemIndex = this._titleIndex[kvp.Key];
				dtCompColKeyName = this._dtCompare.Columns[ ( this._titleIndex[kvp.Key] + 1 ) * 3 ].ColumnName;
				if (kvp.Value.IsEnable == true)
				{
					lowBond = Math.Abs(kvp.Value.Max) * (-1.0d);
					upBond = Math.Abs(kvp.Value.Max);

					if (itemName == "LOP" || itemName == "WATT" || itemName == "LM")
					{
						lowBond = 1.0d - Math.Abs(kvp.Value.Max);
						upBond = 1.0d + Math.Abs(kvp.Value.Max);
						filterExpress = dtCompColKeyName + "<" + lowBond.ToString() + " or " + dtCompColKeyName + ">" + upBond.ToString();
					}
					else
					{
						filterExpress = dtCompColKeyName + "<" + lowBond.ToString() + " or " + dtCompColKeyName + ">" + upBond.ToString();
					}

					// filter std. data 
					rows = this._dtCompare.Select(filterExpress);
					if (rows != null)
					{
						for (int i = 0; i < rows.Length; i++)
						{
							rows[i].Delete();
						}
					}
					
				}
				index++;
			}
			
			this._dtCompare.AcceptChanges();
		
		}

        public void CreateCalcObj(out CalcGainOffset[] gainOffsetArray, out CalcCoef[] coefArray)
		{
			if (this._titleInfor == null || this._gainOffsetIndex == null || this._coefIndex == null || this._dtCompare == null)
			{
				gainOffsetArray = null;
				coefArray = null;
				return;
			}

			gainOffsetArray = new CalcGainOffset[ this._gainOffsetIndex.Count ];
			coefArray = new CalcCoef[ this._coefIndex.Count ];

			//-----------------------------------------------------
			// Create "CalcGainOffset" Object
			//-----------------------------------------------------
			int index = 0;
			double[] xIn = null;
			double[] yOut = null;
			foreach( KeyValuePair<string,int> kvp in this._gainOffsetIndex )
			{
				xIn = new double[ this._dtCompare.Rows.Count ];
				yOut = new double[ this._dtCompare.Rows.Count ];
				for(int row = 0; row < this._dtCompare.Rows.Count; row++ )
				{
					xIn[row]  = (double)this._dtCompare.Rows[row][kvp.Value * 3 + 2 ];	// msrt index = 2, 5, 8,
					yOut[row] = (double)this._dtCompare.Rows[row][kvp.Value * 3 + 1 ];	// std index  = 1, 4, 7,
				}
				gainOffsetArray[index] = new CalcGainOffset(kvp.Key, this.TitleName[kvp.Key] , xIn, yOut);
				gainOffsetArray[index].CalcType = this._titleInfor[kvp.Key].GainOffsetType;
				gainOffsetArray[index].Format = this._titleInfor[kvp.Key].Format;
				index++;
			}

			//-----------------------------------------------------
			// Create "Coef. Table" Object
			//-----------------------------------------------------
			index = 0;
			foreach( KeyValuePair<string,int[]> kvp in this._coefIndex )
			{
				//=============================================================
				// (1) New one menory space and Create default "Coef. Table" 
				//=============================================================
				double[][] stdRawData = new double[7][];
				double[][] msrtRawData = new double[7][];

				for (int col = 0; col < 7; col++)
				{
					stdRawData[col] = new double[this._dtCompare.Rows.Count];
					msrtRawData[col] = new double[this._dtCompare.Rows.Count];
					if (col >= 3 && col<=5)
					{
						for (int row = 0; row < this._dtCompare.Rows.Count; row++)
						{
							stdRawData[col][row] = 1.0d;
							msrtRawData[col][row] = 1.0d;
						}
					}
				}

				//=============================================================
				// (2) Fill the "Coef. Table"
				//=============================================================
				for( int row= 0; row< this._dtCompare.Rows.Count; row++ )
				{
					for ( int col=0; col < 7; col++ )
					{
						if ( kvp.Value[col] > 0 ) // Have the item
						{
							if ( col < 3 )	// for WLP, WLD, WLC
							{
								stdRawData[col][row] = (double)this._dtCompare.Rows[row][kvp.Value[col] * 3 + 1 ];		// std index  = 1, 4, 7,
								msrtRawData[col][row] = (double)this._dtCompare.Rows[row][kvp.Value[col] * 3 + 2 ];		// msrt index = 2, 5, 8,
							}
							else if ( col == 3 )
							{

                                if (this._LOPSaveItem == ELOPSaveItem.mcd && _isSingleLOPItem == true)
                                {
                                    stdRawData[3][row] = (double)this._dtCompare.Rows[row][kvp.Value[3] * 3 + 1];			// std index  = 1, 4, 7,
                                    msrtRawData[3][row] = (double)this._dtCompare.Rows[row][kvp.Value[3] * 3 + 2];			// msrt index = 2, 5, 8,
                                }
                                else
                                {
                                    stdRawData[3][row] = (double)this._dtCompare.Rows[row][kvp.Value[3] * 3 + 1];			// std index  = 1, 4, 7,
                                    msrtRawData[3][row] = (double)this._dtCompare.Rows[row][kvp.Value[3] * 3 + 2];	
                                }
							}
							else if ( col == 4 )
							{
                                if (this._isSingleLOPItem)
                                {
                                    if (this._LOPSaveItem == ELOPSaveItem.mcd)
                                    {
                                        stdRawData[3][row] = (double)this._dtCompare.Rows[row][kvp.Value[col] * 3 + 1];			// std index  = 1, 4, 7,
                                        msrtRawData[3][row] = (double)this._dtCompare.Rows[row][kvp.Value[col] * 3 + 2];		// msrt index = 2, 5, 8,
                                    }

                                    if (this._LOPSaveItem == ELOPSaveItem.watt)
                                    {
                                        stdRawData[4][row] = (double)this._dtCompare.Rows[row][kvp.Value[col] * 3 + 1];			// std index  = 1, 4, 7,
                                        msrtRawData[4][row] = (double)this._dtCompare.Rows[row][kvp.Value[col] * 3 + 2];		// msrt index = 2, 5, 8,
                                    }                       
                                }
                                else
                                {
                                    stdRawData[4][row] = (double)this._dtCompare.Rows[row][kvp.Value[col] * 3 + 1];				// std index  = 1, 4, 7,
                                    msrtRawData[4][row] = (double)this._dtCompare.Rows[row][kvp.Value[col] * 3 + 2];			// msrt index = 2, 5, 8,
                                }
							}
							else if ( col == 5 )
							{
								stdRawData[5][row] = (double)this._dtCompare.Rows[row][kvp.Value[col] * 3 + 1];					// std index  = 1, 4, 7,
								msrtRawData[5][row] = (double)this._dtCompare.Rows[row][kvp.Value[col] * 3 + 2];				// msrt index = 2, 5, 8,
							}
                            else if (col == 6)
                            {
                                stdRawData[6][row] = (double)this._dtCompare.Rows[row][kvp.Value[col] * 3 + 1];					// std index  = 1, 4, 7,
                                msrtRawData[6][row] = (double)this._dtCompare.Rows[row][kvp.Value[col] * 3 + 2];				// msrt index = 2, 5, 8,
                            }

						}
					}
				}

				string[] coefKeyNames = new string[7] { "WLP_", "WLD_", "WLC_", "LOP_","WATT_", "LM_","HW_"};
				string[] coefNames = new string[7] { "WLP1", "WLD1", "WLC1", "LOP1", "WATT1", "LM", "HW1" };
                int num = int.Parse(kvp.Key);
				string key = string.Empty;

				for (int i = 0; i < coefKeyNames.Length; i++)
				{
					key = coefKeyNames[i] + num.ToString();
					if ( this._titleName.ContainsKey(key)  )
					{
						coefNames[i] = this._titleName[coefKeyNames[i] + num.ToString()];
					}
					else
					{
						coefNames[i] = "NONE";
					}
				}

                // paul 2011-12-26
				coefArray[index] = new CalcCoef(num, stdRawData, msrtRawData, this._calBaseWave, coefNames);				
				foreach (CalcGainOffset oneCalcGainOffset in coefArray[index].GainOffsetArray)
				{
					if (this._titleInfor.ContainsKey(oneCalcGainOffset.KeyName))
					{
						oneCalcGainOffset.CalcType = this._titleInfor[oneCalcGainOffset.KeyName].GainOffsetType;
						oneCalcGainOffset.Format = this._titleInfor[oneCalcGainOffset.KeyName].Format;
					}
				}
				
				index++;
			}

        }

		public void ClearData()
		{
			if (this._dtStd != null)
			{
				this._dtStd.Clear();

                this._dtStd = null;
			}

			if (this._dtMsrt != null)
			{
				this._dtMsrt.Clear();

                this._dtMsrt = null;
			}

			if (this._dtCompare != null)
			{
				this._dtCompare.Clear();

                this._dtCompare = null;
			}
		}

        public EErrorCode ConvertFloatReportToFixedReport(string pathAndFile, string outFile)
        {
            if (File.Exists(pathAndFile) == false)
            {
                return EErrorCode.Tools_LoadStdDataFail;
            }

            if (!this._titleIndex.ContainsKey("ROW") && !this._titleIndex.ContainsKey("COL")
                && this._isArrangeByRowCol)
            {
                return EErrorCode.Tools_UserDataNotDefineRowCol;
            }
            else if (!this._titleIndex.ContainsKey("TEST") && this._isArrangeByRowCol == false)
            {
                return EErrorCode.Tools_UserDataNotDefineSeq;
            }

            List<string[]> file = MPI.Tester.CSVUtil.ReadCSV(pathAndFile);

            if (file == null)
            {
                return EErrorCode.Tools_LoadStdDataFail;
            }

            Dictionary<string, int> dataIndex = new Dictionary<string, int>();

            for (int i = 0; i < file.Count; i++)
            {
                if (file[i].Length > 2)
                {
                    if (file[i][1] == "PosX" && file[i][2] == "PosY")
                    {
                        int index = 0;

                        foreach (var item in file[i])
                        {
                            if (!dataIndex.ContainsKey(item))
                            {
                                dataIndex.Add(item, index);

                                index++;
                            }
                            else
                            {
                                return EErrorCode.Tools_UserDataMsrtTitleIsNotMatch;
                            }
                        }
                    }
                }
            }

            List<string[]> fixedFile = new List<string[]>();

            bool isRawData = false;

            for (int i = 0; i < file.Count; i++)
            {
                if (isRawData)
                {
                    string[] line = new string[this._titleName.Values.Count];

                    int index = 0;

                    foreach (var item in this._titleName.Values)
                    {
                        if (dataIndex.ContainsKey(item))
                        {
                            line[index] = file[i][dataIndex[item]];
                        }
                        else
                        {
                            line[index] = string.Empty;
                        }

                        index++;
                    }

                    fixedFile.Add(line);
                }
                else
                {
                    if (file[i].Length > 2)
                    {
                        if (file[i][1] == "PosX" && file[i][2] == "PosY")
                        {
                            isRawData = true;

                            string[] line = new string[this._titleName.Values.Count];

                            int index = 0;

                            foreach (var item in this._titleName.Values)
                            {
                                line[index] = item;

                                index++;
                            }

                            fixedFile.Add(line);
                        }
                        else
                        {
                            fixedFile.Add(file[i]);
                        }
                    }
                }
            }

            CSVUtil.WriteCSV(outFile, fixedFile);

            return EErrorCode.NONE;
        }

        #endregion
    }
}

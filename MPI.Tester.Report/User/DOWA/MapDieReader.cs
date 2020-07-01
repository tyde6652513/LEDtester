using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Linq;

using MPI.Tester.Report;
using MPI.Tester.Report.BaseMethod.HeaderFinder;
using MPI.Tester.Report.BaseMethod.PosKeyMaker;

namespace MPI.Tester.Report.User.DOWA
{
    public interface IMapItem
    {
        bool SetRowData(string str, List<string> refColList);
    }

    public class MapDieReader<T> where T : IMapItem, new()
    {
        public List<string> PosKeyLsit = new List<string>();//給定的key會作為keyPos的構成元素
        public List<int> PosColList = new List<int>();//較為舊版的tmap可能沒有檔頭，因此要允許手動給定
        public List<string> HeaderItemList = new List<string>();
        public Dictionary<string, T> PosDieDict = new Dictionary<string, T>();

        public bool IsDefineDataType = false;
        HeaderFinder _headerFinder;
        PosKeyMakerBase _posMaker;
        char _splitChar = ',';
        string posPAttern = "^[xyXY][+-]\\d";
        Regex _subDieRegex;
        #region
        public MapDieReader(HeaderFinder hf,PosKeyMakerBase posMaker,char splitChar = ',')
        {
            _subDieRegex = new Regex(posPAttern);
            _headerFinder = hf;
            _posMaker = posMaker;
            _splitChar = splitChar;

            PosKeyLsit = new List<string>();
            PosKeyLsit.Add("X");
            PosKeyLsit.Add("Y");
        }

        public MapDieReader(HeaderFinder hf, PosKeyMakerBase posMaker, List<string> posKeyList, char splitChar = ','):
            this(hf,posMaker,splitChar)
        {
            PosKeyLsit = new List<string>();
            PosKeyLsit.AddRange(posKeyList.ToArray());
        }

        public virtual Dictionary<string, T >ReadMapFromFile(string srcPath, bool deletSrcFile = true,Encoding encoding = null)
        {
            if (encoding == null)
            {
                encoding = Encoding.Default;
            }
            if (File.Exists(srcPath))
            {
                using (StreamReader sr = new StreamReader(srcPath, encoding))
                {
                    bool isRawData = false;
                    while (sr.Peek() >= 0)
                    {
                        string line = sr.ReadLine();


                        if (isRawData)
                        {
                            string[] rawData = line.Split(this._splitChar);
                            T dut = new T();
                            dut.SetRowData(line, HeaderItemList);
                            string pos = _posMaker.GetPosKey(rawData);
                            if (PosDieDict.ContainsKey(pos))
                            {
                                PosDieDict[pos] = dut;
                            }
                            else
                            {
                                PosDieDict.Add(pos, dut);
                            }
                        }
                        else
                        {
                            if (_headerFinder.CheckIfRowData(line))
                            {
                                isRawData = true;
                            }

                            if (_headerFinder.IsFitTarStr(line))
                            {
                                string[] rawData = line.Split(this._splitChar);
                                HeaderItemList = new List<string>();
                                for (int i = 0; i < rawData.Length; ++i)
                                {
                                    HeaderItemList.Add(rawData[i].ToUpper());
                                }                                    HeaderItemList.AddRange(rawData);

                                if (PosKeyLsit.Count == 0)
                                {
                                    PosKeyLsit = CollectPosKeys(HeaderItemList);
                                }

                                for (int i = 0; i < PosKeyLsit.Count; ++i)
                                {
                                    int col = HeaderItemList.IndexOf(PosKeyLsit[i]);
                                    if (col >= 0)
                                    {
                                        PosColList.Add(col);
                                    }
                                }
                            }

                        }
                    }
                }


                if (deletSrcFile)
                {
                    MPIFile.DeleteFile(srcPath);//為避免誤認前次的tmap
                }
            }

            return PosDieDict;
            
        }
        #endregion

        #region
        private List<string> CollectPosKeys(List<string> keyList) // X,Y, X+1
        {
            List<string> xyList = (from key in keyList
                                   where (key == "X" || key == "Y")
                                   orderby key == "X"?1:0
                                   select key).ToList();
            char[] splitArr = {'X','Y'};
            List<string> subxyList = (from key in keyList
                                      where (_subDieRegex.IsMatch(key))
                                      orderby key.StartsWith("X")//X先
                                      orderby (int.Parse(key.Split(splitArr)[0]))//層數  +1會被當作 1嗎?
                                      select key).ToList();//越後面執行優先度越高

            xyList.AddRange(subxyList.ToArray());
            return xyList;

        }
        #endregion
    }



    //public class MapDieBase:IMapItem//最基本的紀錄型別
    //{
    //    public string State;

    //    public MapDieBase(  )
    //    {            
    //    }

    //    public void SetRowData(string str,List<int> posColList)
    //    {
    //        State = str;
    //    }

    //}


}

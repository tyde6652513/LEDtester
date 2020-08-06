using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;


using MPI.Tester.Report;
using MPI.Tester.Report.BaseMethod.HeaderFinder;
using MPI.Tester.Report.BaseMethod.PosKeyMaker;

using MPI.Tester.Report.BaseMethod.MapReader;

namespace MPI.Tester.Report.User.DOWA
{
    public class MapReader_DOWA : MapDieReader<DOWA_AOISignItem> //S6 Prober尚未改成新的tmap格式，因此先override
    {

        public MapReader_DOWA(HeaderFinderBase hf, PosKeyMakerBase posMaker, char splitChar = ','):
            base( hf,  posMaker,  splitChar)
        {
            _subDieRegex = new Regex(posPattern);
            _headerFinder = hf;
            _posMaker = posMaker;
            _splitChar = splitChar;

            PosKeyLsit = new List<string>();
            PosKeyLsit.Add("X");
            PosKeyLsit.Add("Y");
        }

        public MapReader_DOWA(HeaderFinderBase hf, PosKeyMakerBase posMaker, List<string> posKeyList, char splitChar = ',') :
            this(hf,posMaker,splitChar)
        {
            PosKeyLsit = new List<string>();
            PosKeyLsit.AddRange(posKeyList.ToArray());
        }

        protected override List<string> CollectPosKeys(List<string> keyList) // X,Y, X+1
        {
            List<string> xyList = (from key in keyList
                                   where (key == "X" || key == "Y")
                                   orderby key == "X" ? 1 : 0
                                   select key).ToList();
            char[] splitArr = { 'X', 'Y' };
            List<string> subxyList = (from key in keyList
                                      where (_subDieRegex.IsMatch(key))
                                      orderby key.StartsWith("X")//X先
                                      orderby (int.Parse(key.Split(splitArr)[0]))//層數  +1會被當作 1嗎?
                                      select key).ToList();//越後面執行優先度越高

            xyList.AddRange(subxyList.ToArray());
            return xyList;

        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using MPI.Tester.Data;

using MPI.Tester.Report.BaseMethod.HeaderFinder;
using MPI.Tester.Report.BaseMethod.PosKeyMaker;
using MPI.Tester.Report.BaseMethod.MapReader;
using MPI.Tester.Report.BaseMethod.Merge;

namespace MPI.Tester.Report.User.Accelink
{
    public class ReportMerger : ReportMergerBase
    {

        #region
        private Dictionary<string, Dictionary<int, bool>> posFilecnt_passDic = new Dictionary<string, Dictionary<int, bool>>();
        private int _passIndex;
        #endregion
        public ReportMerger(UISetting uiset, HeaderFinderBase hf, ResultTitleInfo rti, PosKeyMakerBase posMaker):
            base (uiset,  hf,  rti,  posMaker)
        {
            _passIndex = rti.IsAllPassIndex;
        }



        protected override EParsingState ParseMsrtData(string line)
        {
            EParsingState state = EParsingState.MsrtData;

            string[] rawData = line.Split(this.SpiltChar);

            string colrowKey = _crKeyMaker.GetPosKey(rawData);

            if (!_posStrArrDic.ContainsKey(colrowKey))
            {
                _posStrArrDic.Add(colrowKey, rawData);
            }
            else
            {
                string[] tempArr = _posStrArrDic[colrowKey];
                int minLen = Math.Min(rawData.Length, tempArr.Length);
                for (int i = 0; i < minLen; ++i)
                {
                    if (rawData[i] != null)
                    {
                        if (rawData[i] != "")
                        {
                            

                            if (i == _passIndex)
                            {
                                bool isPass = ParseColISALLPASS(rawData, colrowKey, i);

                                tempArr[i] = isPass?"1":"0";
                            }
                            else
                            {
                                tempArr[i] = rawData[i];
                            }

                           
                        }


                    }
                }
                _posStrArrDic[colrowKey] = tempArr;
            }

            return state;
        }

        private bool ParseColISALLPASS(string[] rawData, string colrowKey, int i)
        {
            bool isPass = true;
            if (!posFilecnt_passDic.ContainsKey(colrowKey))
            {
                posFilecnt_passDic.Add(colrowKey, new Dictionary<int, bool>());
            }

            Dictionary<int, bool> cntPassDic = posFilecnt_passDic[colrowKey];
            bool nowIsPass = rawData[i].Trim() == "1";
            if (!cntPassDic.ContainsKey(_parsingFileCnt))
            {
                cntPassDic.Add(_parsingFileCnt, nowIsPass);
            }
            else
            {
                cntPassDic[_parsingFileCnt] = nowIsPass;
            }

            isPass = !cntPassDic.Any(x => x.Value == false);

            return isPass;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace MPI.Tester.Data
{
    public  class DecoderBase
    {
        LinkData _LData;
        SStack sST;
        VarDic vD;

        public DecoderBase( LinkData ldata)
        {
            _LData = ldata;
            sST = _LData.sStack;
            vD = _LData.VarDic;
        }
        virtual public void Decode(string str,List<TestResultData> rList = null)
        {
            //Symble sTemp;
            
            List<string> strList = str.Split(new char[] {' ','\t','\n'}).ToList().Where( x => x != " ").ToList();
            List<int> shLi = GetShiftList(strList);
            SymbleList sList = GetStack(shLi);

            switch (strList[0])
            {
                case "ASSIGN":
                    {
                        if (!vD.ContainsKey(strList[shLi[0]]))
                        {
                            vD.Add(strList[shLi[0]], 0);
                        }
                        vD[strList[1]] = sList[shLi[0]].val;

                        //return Decode
                    }

                    break;
                case "RETURN":
                    {
                        _LData.ReturnVal = sList[shLi[0]].val;    
                    }
                    break;
                case "ADD":
                    {
                        Symble s = new Symble("Expr");
                        s.val = sList[shLi[0]].val + sList[shLi[1]].val;
                        sST.Push(s);
                    }
                    break;
                case "SUB":
                    {
                        Symble s = new Symble("Expr");
                        s.val = sList[shLi[0]].val - sList[shLi[1]].val;
                        sST.Push(s);
                    }
                    break;
                case "MULTI":
                    {
                        Symble s = new Symble("Term");
                        s.val = sList[shLi[0]].val * sList[shLi[1]].val;
                        sST.Push(s);
                    }
                    break;
                case "DIVIDE":
                    {
                        Symble s = new Symble("Term");
                        if (sList[shLi[1]].val == 0)
                        {
                            if (sList[shLi[0]].val < 0)
                            {
                                s.val = double.NegativeInfinity;
                            }
                            else 
                            {
                                s.val = double.PositiveInfinity;
                            }
                        }
                        else
                        {
                            s.val = sList[shLi[0]].val / sList[shLi[1]].val;
                        }
                        sST.Push(s);
                    }
                    break;
                case "VAR":
                    {
                        if (vD.ContainsKey(strList[1]))
                        {
                            Symble s = new Symble("Factor");
                            s.val = vD[strList[1]];
                            sST.Push(s);
                        }
                        else
                        {
                            Symble s = new Symble("ERR");
                            s.str = "VAR \"" + strList[1] + "\" not defined!";
                            sST.Push(s);
                        }
                    }
                    break;
                case "REF_NUM":
                    {
                        double? rVal = _LData.GetValD.Invoke(strList[1], rList);
                        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                        if (rVal != null)
                        {
                            if (!vD.ContainsKey(strList[1]))
                            {
                                vD.Add(strList[1], (double)rVal);
                            }

                            Symble s = new Symble("Factor");
                            s.val = vD[strList[1]];
                            sST.Push(s);
                        }
                        else
                        {
                            Symble s = new Symble("ERR");
                            s.str = "RRF_VAR \"" + strList[1] + "\" Search Fail!";
                            sST.Push(s);
                        }
                    }

                    break;
                case "NUM":
                    {
                        Symble s = new Symble("Factor");
                        s.val = double.Parse(strList[1]);
                        sST.Push(s);
                    }
                    break;
            }
            //strList = 

            //List<string> strList = ((str.Split(new char[] {' '})).ToList().Select(x => new {x != ""}));

            
        }

        protected List<int> GetShiftList(List<string> strList)
        {
            List<int> shList = new List<int>();
            Regex reg = new Regex("\\$\\d");
            foreach (string str in strList)
            {
                if (reg.IsMatch(str))
                {
                    int tempNum;
                    string sTemp = str.Substring(1);
                    if (int.TryParse(sTemp, out tempNum))
                    {
                        shList.Add(tempNum);
                    }     
                }
            }
            return shList;
        }
        protected SymbleList GetStack(List<int>  shList)
        {
            int popQty = -1;
            foreach (int num in shList)
            {
                if (num > popQty)
                {
                    popQty = num;
                }
            }
            return new SymbleList(sST, popQty + 1);
        }

        protected SymbleList GetStack(List<string> strList)
        {
            int popQty = -1;
            Regex reg = new Regex("\\$\\d");
            foreach (string str in strList)
            {
                if (reg.IsMatch(str))
                {
                    int tempNum;
                    string sTemp = str.Substring(1);
                    if (int.TryParse(sTemp, out tempNum) &&
                        tempNum > popQty)
                    {
                        popQty = tempNum;
                    }                    
                }                
            }
            return new SymbleList(sST, popQty + 1);
        }
    }

    class Decoder : DecoderBase
    {
        public Decoder(LinkData ldata)
            : base(ldata)
        { }
    }
}

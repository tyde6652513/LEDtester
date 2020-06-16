using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace MPI.Tester.Data
{

    public delegate string ActionDelegate(string str,double val);
    public delegate double? GetValDelegate(string varName,List<TestResultData> resultList);//使用nullable作為回傳手段，回傳null以辨識取值失敗的狀況

    public class SymbleList : List<Symble> 
    {
        public SymbleList()
            : base()
        { }
        public SymbleList(SStack sst, int qty)
            : base()
        {
            for (int i = 0; i < qty; ++i)
            {
                this.Add(sst.Pop());
            }
            this.Reverse();

        }
        public void AddFromStack( SStack sst,int qty)
        {
            for(int i=0;i<qty;++i)
            {
                this.Add(sst.Pop());
            }
            this.Reverse();
        }
    };//建立單一Rule用

    public class TokenQue : Queue<TokenInfo> {
        public TokenQue():base()
        { }

        public TokenQue(List<TokenInfo> tList):this()
        {
            this.Clear();

            foreach (TokenInfo ti in tList)
            {
                this.Enqueue(ti.Clone() as TokenInfo);
            }
        }
    };
    //

    public class Parser
    {
        public TokenQue _tQueue;
        private Grama _grama;
        public  Priority _pTool;
        public LinkData _linkData;

        public Parser(LinkData ld)
        {
            _tQueue = new TokenQue();
            _pTool = new Priority();
            _linkData = ld;
            _grama = new Grama(ref _linkData);
            //CreateGramar();
        }

        public string  Parse()//(List<TokenInfo> tList )
        {
            _linkData.sStack.Clear();
            _tQueue = _linkData.tQueue;
            int count = _tQueue.Count;

            for (int i = 0; i < count * count  ; ++i)//(Tqueue.Peek().TType != ETType.EOL)
            {
                if (!TryReduce())
                {
                    TryShift();
                }

                if (_linkData.sStack.Peek().SymType == "statement")
                {
                    break;
                }
                else if (_linkData.sStack.Peek().SymType == "ERR")
                {
                    return _linkData.sStack.Peek().str;
                }
            }

            return "";

        }

        private bool TryReduce()//嘗試將符合語法的Symble進行處理
        {
            bool result = false;

            SStack sST = _linkData.sStack;
            //VarDic vD = _linkData.VarDic;
            Rule r = null;
            foreach (KeyValuePair<string, Symble> pair in _grama)
            {
                Symble symble = pair.Value;

                r = symble.GetMatchRule(sST);

                if (r != null)
                {
                    KeyValuePair<int, string> ori = new KeyValuePair<int, string>(_pTool.GetPriority(symble.SymType), _pTool.GetLR(symble.SymType));//r.GetPriority(_pTool);
                    KeyValuePair<int, string> preV = PreView();

                    if(ori.Key > preV.Key)
                    {
                        //_linkData.OutPutDelegate += r.OutDel;
                        ActionPack AP = new ActionPack();
                        AP.OutPutDelegate = r.OutDel;
                        AP.Str = sST.Peek().str;
                        AP.Val = sST.Peek().val;
                        _linkData.ActList.Add(AP);
                        string str = r.InnerDel.Invoke("",0);

#if DEBUG
                        using (StreamWriter sw = new StreamWriter(@"..\..\..\..\..\log.txt", true, Encoding.Default))
                        {                            
                            sw.WriteLine(str);
                        }
#endif
                        return true;
                    }
                    else if (ori.Key < preV.Key)
                    {
                        return false;
                    }
                    else 
                    {
                        if (ori.Value == "L")
                        {
                            ActionPack AP = new ActionPack();
                            AP.OutPutDelegate = r.OutDel;
                            AP.Str = sST.Peek().str;
                            AP.Val = sST.Peek().val;
                            _linkData.ActList.Add(AP);
                            string str = r.InnerDel.Invoke("", 0);
#if DEBUG
                            using (StreamWriter sw = new StreamWriter(@"..\..\..\..\..\log.txt", true, Encoding.Default))
                            {
                                sw.WriteLine(str);
                            }
#endif
                            return true;                            
                        }
                        return false;

                    }
                    //
                } 
            }

            return result;
        }

        private KeyValuePair<int, string> PreView()//看下一個Symble優先度是否較高
        {
            KeyValuePair<int, string> pri = new KeyValuePair<int, string>(0, "R");

            if (_tQueue != null & _tQueue.Count > 0)
            {
                Symble s = Token2Symble(_tQueue.Peek());
                int priVal = _pTool.GetPriority(s.SymType);
                string priLR = _pTool.GetLR(s.SymType);
                pri = new KeyValuePair<int, string>(priVal, priLR);
            }

            return pri;
        }

        private bool TryShift()//若無法Reduce，再往Stack塞入一個symble
        {
            bool result = false;

            Symble s = Token2Symble(_tQueue.Dequeue());
            _linkData.sStack.Push(s);

            return result;
 
        }

        private bool CheckGramaMatch()
        {
            bool isMatch = false;

            SStack tempStk = new SStack();

            SStack lStk = _linkData.sStack;

            return isMatch;
        }

        private Symble Token2Symble( TokenInfo T)
        {
            Symble sym = new Symble(T.TType.ToString());
            sym.val = T.val;
            sym.str = T.Str;
            return sym;
 
        }

        public class Priority
            {
                Dictionary<int, string> priorityDic;//先暫定數字越大優先度越高

                Dictionary<string, string> LRDic;//在偷看下一個運算子的時候，若優先度一樣要決定誰先處理
                //如 5-2-1會變成 (5-2)-1或是5-(2-1)
                //預設為右方優先
                public Priority()
                {
                    priorityDic = new Dictionary<int, string>();
                    LRDic = new Dictionary<string, string>();
                    MakeDic();
                }

                private void MakeDic()
                {
                    priorityDic = new Dictionary<int, string >();
                    int pri = 1;

                    priorityDic.Add(pri++, "statement");
                    priorityDic.Add(pri++, "EOL");
                    priorityDic.Add(pri++, "RETURN");                    
                    priorityDic.Add(pri++, "R_BRACKETS");//括號內處理完再結束
                    priorityDic.Add(pri++, "ADD");
                    priorityDic.Add(pri++, "SUB");
                    priorityDic.Add(pri++, "Expr");
                    priorityDic.Add(pri++, "MULTI");
                    priorityDic.Add(pri++, "DIVIDE");//現乘除後加減
                    priorityDic.Add(pri++, "Term");
                    priorityDic.Add(pri++, "L_BRACKETS");//括號內優先
                    priorityDic.Add(pri++, "Factor");

                    priorityDic.Add(pri++, "NUM");
                    priorityDic.Add(pri++, "VAR");
                    priorityDic.Add(pri++, "REF_NUM");
                    priorityDic.Add(pri++, "ASSIGN");

                    
                    priorityDic.Add(pri++, "ERR");

                    LRDic = new Dictionary<string, string>();

                    LRDic.Add("+", "L");
                    LRDic.Add("-", "L");
                    //LRDic.Add("=", "r");//a=b =1
                }

                public int GetPriority(string symble)
                {
                    foreach (KeyValuePair<int, string> psPair in priorityDic)
                    {
                        if (psPair.Value == symble)
                        {
                            return psPair.Key;
                        }
                    }

                    return 0;
                }

                public int ComparePriority(string operator1, string operator2)
                {
                    int higher = 2;//1代表operator1優先

                    if (GetPriority(operator1) > (GetPriority(operator1)))
                    {
                        higher = 1;
                    }
                    else if (GetPriority(operator1) < (GetPriority(operator1)))
                    {
                        higher = -1;
                    }
                    else
                    {
                        higher = 0;
                    }
                    return higher;

                }

                public string GetLR(string symbele)
                {
                    if (LRDic.ContainsKey(symbele))
                    {
                        return LRDic[symbele];
                    }
                    return "R";
                }
            }

    }

    public class Rule
    {
        public SymbleList SL;
        public ActionDelegate InnerDel;
        public ActionDelegate OutDel;
        //public hand

        public Rule()
        {
            SL = new SymbleList();
        }
        public Rule(SymbleList slist)
        {
            SL = slist;
        }
        public Rule(List<string> strlist):this()
        {
            foreach (string str in strlist)
            {
                SL.Add(new Symble(str));
            }
        }
        public Rule(List<string> strlist, ActionDelegate del)
            : this(strlist)
        {
            InnerDel = del;
        }

        public Rule(List<string> strlist, ActionDelegate idel, ActionDelegate odel)
            : this(strlist, idel)
        {
            OutDel = odel;
        }

        public KeyValuePair<int, string> GetPriority(Parser.Priority pTool)
        {
            int priority = 0;
            string LR = "R";//default is right first

            foreach (Symble sym in SL)
            {
                int pri = pTool.GetPriority(sym.SymType);
                if (pri > priority)
                {
                    priority = pri;
                    LR = pTool.GetLR(sym.SymType);
                }
            }
            return new KeyValuePair<int, string>(priority, LR);
        }

        public bool CheckIfMatch(SStack lStk)
        {
            bool isMatch = true;

            SymbleList testlist = new SymbleList();
            if (lStk.Count < 1 || lStk.Count < SL.Count())
            {
                return false;
            }
            int length = SL.Count();
            for (int i = 0; i < length && lStk.Count > 0; ++i)
            {
                testlist.Add(lStk.Pop());
                isMatch &= testlist[i].SymType == SL[length - i -1].SymType;
            }

            if (isMatch)
            {
                string outStr = "";

                foreach (Symble s in SL)
                {
                    outStr += s.SymType;
                }
            }
            //SymbleList testlistR = testlist.Reverse();
            testlist.Reverse();
            foreach (Symble s in testlist)
            {
                lStk.Push(s);
            }

            return isMatch;
        }

    }

    public class Symble
    {
        public string str = "";//token所回傳文字
        public string SymType = "";
        public List<Rule> ruleList = new List<Rule>();
        public bool Isterminal = false;
        public double val = 0.0;

        public Symble(string name)
        {
            SymType = name;
        }
        public void AddRule(List<string> symNameList)
        {
            if (ruleList == null)
            {
                ruleList = new List<Rule>();
            }
            Rule r = new Rule(symNameList);
            //ruleList.Add();
        }

        public void AddRule(List<string> symNameList, ActionDelegate iDel)
        {
            if (ruleList == null)
            {
                ruleList = new List<Rule>();
            }
            Rule r = new Rule(symNameList, iDel);
            ruleList.Add(r);
        }

        public void AddRule(List<string> symNameList, ActionDelegate iDel, ActionDelegate oDel)
        {
            if (ruleList == null)
            {
                ruleList = new List<Rule>();
            }
            Rule r = new Rule(symNameList, iDel, oDel);
            ruleList.Add(r);
        }

        public Rule GetMatchRule(SStack lStk)
        {
            Rule matchRule = null;

            foreach (Rule r in ruleList)
            {
                if (r.CheckIfMatch(lStk))
                {
                    return r;
                }
            }
            return matchRule; 
        }

       // public 
    }



    
}

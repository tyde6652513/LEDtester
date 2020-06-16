using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;
using System.Xml;

//using System.Windows.Forms;

namespace MPI.Tester.Data
{
    public class SimpleCompiler
    {
        LinkData _linkData;
        Scanner _scanner;
        Parser _parser;
        List<string> strList;

        public SimpleCompiler()
        {
            strList = new List<string>();
            //tNode = new TreeNode("root");//treeView1.Nodes.Add("Websites");
            _linkData = new LinkData();
            _scanner = new Scanner(_linkData);
            _parser = new Parser(_linkData);
            
        }
        
        public string JustCompileIt(string str)
        {


            _linkData.Refresh();
            //////////////////////////////////////////////////////////////////////////////////////////////////////////
#if DEBUG
            using (StreamWriter sw = new StreamWriter(@"..\..\..\..\..\log.txt", false, Encoding.Default))
            {
                sw.Write("");
            }
#endif
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////
            string outStr = "";
            List<TokenInfo> tList = new List<TokenInfo>();
            strList = str.Split(new char[] { '\n' }).ToList();//以換行作為分割

            for (int i = 0; i < strList.Count(); ++i)
            {
                bool scannerPass = false;
                if (strList[i] == "")//空行
                {
                    continue;
                }

                List<TokenInfo> ptList = _scanner.Scan(strList[i], i);

                if (ptList != null && ptList.Count > 0)
                {
                    if (ptList[0].TType != ETType.ERR &&
                        ptList[0].TType != ETType.NONE)
                    {
                        _linkData.tQueue = new TokenQue(ptList);
                        //outStr += "Ln:" + i.ToString() + "Scanned\n";

                        scannerPass = true;
                    }
                }

                if (scannerPass)
                {
                    try
                    {
                        string parseResult = _parser.Parse();
                        if (parseResult == "")
                        {
                            //outStr += "Ln:" + i.ToString() + "Parsed\n";
                        }
                        else
                        {
                            return outStr + parseResult;
                        }

                    }
                    catch (Exception e)
                    {
                        _linkData.OutPutStr = "ERR Parser Error :" + e.Message;
                        return "Parser Error : " + e.Message;
                    }
                }
                else
                {
                    //若沒有成功，直接fail
                    string str1 = "ERR Ln:" + i.ToString() + ",Error Text " + ptList[0].Str + "\n";
                    _linkData.OutPutStr = str1;
                    return str1;
                }

            }

            //outStr +=  _linkData.ReturnVal.ToString();
            outStr = "Compile Successed";
            return outStr;

        }//開始編譯
        //public double RunCode()
        //{
        //    _linkData.RefreshAtRunCode();
        //    double outStr = double.NaN;
        //    Decoder dec = new Decoder(_linkData);

        //    List<string> cmdList = (_linkData.OutPutStr.Split(new char[] { '\n' })).ToList();
        //    foreach (string str in cmdList)
        //    {
        //        dec.Decode(str);
        //    }

        //    if (_linkData.ReturnVal != null)
        //    {
        //        outStr = (double)_linkData.ReturnVal;
        //    }

        //    return outStr;

        //} 
    
        public double RunCode(string codeStr = "", List<TestResultData> rsultList = null)
        {
            _linkData.RefreshAtRunCode();
            double outStr = double.NaN;
            Decoder dec = new Decoder(_linkData);
            

            List<string> cmdList ;
            if (codeStr != "")
            {
                cmdList = (codeStr.Split(new char[] { '\n' })).ToList();
            }
            else
            {
                cmdList = (_linkData.OutPutStr.Split(new char[] { '\n' })).ToList();
            }
            foreach (string str in cmdList)
            {
                dec.Decode(str, rsultList);
            }

            if (_linkData.ReturnVal != null)
            {
                outStr = (double)_linkData.ReturnVal;
            }

            return outStr;

        } //執行編譯結果

        public void ClearFindDelegate()
        {
            _linkData.GetValD = null; ;
        }
        public void AssignFindDelegate( GetValDelegate gvd)//註冊"向測試機取值"的函式
        {
            _linkData.GetValD += gvd;
        }

        public string GetCompiledCode()
        {
            if (_linkData.OutPutStr != null)
            {
                return _linkData.OutPutStr;
            }
            else
            {
                return ""; 
            }
        }
    }

    public class VarDic : Dictionary<string, double> //: Dictionary<string,double> 
    {

        public void Add(string key, double val)
        {
            if (base.Keys.Contains(key))
            {
                base[key] = val;
            }
            else
            {
                base.Add(key, val);
            }
        }

        public double this[string key]
        {
            get
            {
                if (base.ContainsKey(key))
                {
                    return base[key];
                }
                else
                {
                    return double.NaN;
                }
            }
            set 
            {
                base[key] = value;
            }
        }

    }
    
    public class BinaryNode<T>//T is the data structor of token
    {
        public BinaryNode<T> ParantNode;//同階層上一個
        public BinaryNode<T> LNode;//同階層下一個
        public BinaryNode<T> RNode;//下一階
        //public int level = 0;

        //  PNode
        //  |
        //  this
        //  |       \
        //  LNode   RNode
        //

        public T Data;

        public BinaryNode()
        {
            Data = default(T);
        }
        public BinaryNode(T data )
        {
            Data = data;
        }
        //public BinaryNode(T data ,BinaryNode<T> parentNode):this( data )
        //{
        //    ParantNode = parentNode;
        //    level = ParantNode.level + 1;
        //}
        public BinaryNode<T> Pop()
        {
            ParantNode.LNode = this.LNode;
            return this;
        }

        public void Delet()
        {
            ParantNode = null;
            LNode = null;
            RNode = null;
            Data = default(T);
        }

        public void AppendLNode(BinaryNode<T> lNode)
        {
            LNode = lNode;
            lNode.ParantNode = this;
        }

        public void AppendRNode(BinaryNode<T> rNode)
        {
            RNode = rNode;
            rNode.ParantNode = this;
            //rNode.level = this.level + 1;
        }
 
    }

    public class TokenInfo:ICloneable
    {
        public string Str;
        public int Line;
        public double val;
        public ETType TType;

        public TokenInfo()
        {
            Str = "";
            Line = -1;
            val = 0;
            TType = ETType.NONE;
        }

        public TokenInfo(ETType type,string name):this()
        {
            Str = name;
            TType = type;
        }

        public TokenInfo(ETType type, string name,int line)
            : this(type,name)
        {
            Line = line;
        }

        public object Clone()
        {
            TokenInfo obj = new TokenInfo();
            obj = this.MemberwiseClone() as TokenInfo;
            return obj;
 
        }
        //
 
    }

    public enum ETType//token type
    {
        NUM = 1,
        VAR = 2,
        //OPERATOR = 3,
        ASSIGN = 4,
        REF_NUM = 5, // 從tester 輸入參數/輸出結果取值
        EOL = 6,
        EOF = 7,
        L_BRACKETS = 8,
        R_BRACKETS = 9,
        ADD = 10,
        SUB = 11,
        MULTI = 12,
        DIVIDE = 13,
        RETURN = 90,

        ERR = 998,
        NONE = 999,
        
    }


    public class SStack : Stack<Symble> { };

    public class ActionPack
    {
        public ActionDelegate OutPutDelegate;
        public double Val = double.PositiveInfinity;
        public string Str = "";
        public ActionPack()
        { 
        }

        ~ActionPack()
        {
            OutPutDelegate = null;
        } 
    }
            
    public class LinkData
    {
        public List<ActionPack> ActList;
        //public ActionDelegate OutPutDelegate;      //compiler
        public GetValDelegate GetValD;
        //public Dictionary<string, double> VarDic;
        public VarDic VarDic;
        public string OutPutStr = "";
        public double? ReturnVal = null;
        public SStack sStack;
        //public double Result = double.PositiveInfinity;
        public TokenQue tQueue;
        //public Queue<string> AssignNameQueue;
        
        //public 

        public LinkData()
        {
            this.VarDic = new VarDic();
            this.sStack = new SStack();
            this.tQueue = new TokenQue();
            this.ActList = new List<ActionPack>();
            //this.AssignNameQueue = new Queue<string>();
        }

        public void Refresh()
        {
            this.VarDic.Clear();
            this.sStack.Clear();
            this.tQueue.Clear();
            this.OutPutStr = "";
            //this.AssignNameQueue = new Queue<string>();
            //OutPutStr = "";
            ReturnVal = null;
        }

        public void RefreshAtRunCode()
        {
            this.VarDic.Clear();
            this.sStack.Clear();
            this.tQueue.Clear();
            this.ActList.Clear();
            //OutPutDelegate = null;
            //GetValD = null;
        }
        

    }


}

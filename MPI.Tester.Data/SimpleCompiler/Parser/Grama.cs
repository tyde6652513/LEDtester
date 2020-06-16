using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace MPI.Tester.Data
{
    class Grama: Dictionary<string,Symble>//建立語法用
    {
        private LinkData _linkData;

        public Grama(ref LinkData lData)
        {
            _linkData = lData;
            SStack sST = _linkData.sStack;
            VarDic vD = _linkData.VarDic;
            //Queue<string> AQueue = _linkData.AssignNameQueue;
            Symble sTemp;
            SymbleList sList;

            //string OutStr = _linkData.OutPutStr;

            //statement -> Var = Expr  
            //          | return Expr
            #region >>statement<<

            this.Add("statement", new Symble("statement"));
            sTemp = this["statement"];
            sTemp.AddRule((new string[] { "VAR", "ASSIGN", "Expr", "EOL" }).ToList(),
                new ActionDelegate(delegate(string str,double val)
                {
                    sList = new SymbleList(sST, 4);

                    if (!vD.ContainsKey(sList[0].str))
                    {
                        vD.Add(sList[0].str, sList[0].val);
                    }
                    vD[str] = sList[0].val;

                    sST.Push(new Symble("statement"));

                    _linkData.OutPutStr += "ASSIGN " + sList[0].str + " $0\n";

                    return "statement -> Var = Expr EOL";
                })

                );
            sTemp.AddRule((new string[] { "RETURN", "Expr", "EOL" }).ToList(),
                    new ActionDelegate(delegate(string str, double val)
                {
                    sList = new SymbleList(sST, 3);

                    sST.Push(new Symble("statement"));

                    _linkData.OutPutStr += "RETURN  $0\n";

                    return "statement -> RETURN Expr EOL";
                })

                );

            #endregion
            //Expr -> Expr + Term
            //      | Expr - Term
            //      | Trem     
            #region>>Expr<<
            this.Add("Expr", new Symble("Expr"));
            sTemp = this["Expr"];
            sTemp.AddRule((new string[] { "Expr", "ADD", "Term" }).ToList(),
                new ActionDelegate(delegate(string str, double val)
                {
                    sList = new SymbleList(sST, 3);
                    Symble s = new Symble("Expr");
                    sST.Push(s);
                    _linkData.OutPutStr += "ADD $0 $1\n";
                    return "Expr ->Expr + Term";
                })

                );
            sTemp.AddRule((new string[] { "Expr", "SUB", "Term" }).ToList(),
                 new ActionDelegate(delegate(string str, double val)
                 {
                     sList = new SymbleList(sST, 3);
                     Symble s = new Symble("Expr");
                     sST.Push(s);
                     _linkData.OutPutStr += "SUB $0 $1\n";
                     return "Expr ->Expr - Term";
                 })

                 );
            sTemp.AddRule((new string[] { "Term" }).ToList(),
                 new ActionDelegate(delegate(string str, double val)
                 {
                     sList = new SymbleList(sST, 1);
                     Symble s = new Symble("Expr");
                     sST.Push(s);
                     return "Expr -> Term";
                 })

                 );
            #endregion

            //Term -> Term * Factor
            //      | Term / Factor
            //      | Factor  
            #region>>Term<<

            this.Add("Term", new Symble("Term"));
            sTemp = this["Term"];
            sTemp.AddRule((new string[] { "Term", "MULTI", "Factor" }).ToList(),
                new ActionDelegate(delegate(string str, double val)
                {
                    sList = new SymbleList(sST, 3);
                    Symble s = new Symble("Term");
                    s.val = sList[0].val * sList[2].val;
                    sST.Push(s);
                    _linkData.OutPutStr += "MULTI $0 $1\n";
                    return "Term ->Term * Factor";
                })

                );
            sTemp.AddRule((new string[] { "Term", "DIVIDE", "Factor" }).ToList(),
                 new ActionDelegate(delegate(string str, double val)
                 {
                     sList = new SymbleList(sST, 3);
                     Symble s = new Symble("Term");
                     sST.Push(s);
                     _linkData.OutPutStr += "DIVIDE  $0 $1\n";
                     return "Term ->Term / Factor";
                 })

                 );
            sTemp.AddRule((new string[] { "Factor" }).ToList(),
                 new ActionDelegate(delegate(string str, double val)
                 {
                     sList = new SymbleList(sST, 1);
                     Symble s = new Symble("Term");
                     sST.Push(s);
                     return "Term -> Factor";
                 })

                 );

            #endregion

            //Factor-> ( Expr )
            //      | ID
            //      | VAR
            //      | REF_NUM
            //      | NUM
            #region>>Factor<<

            this.Add("Factor", new Symble("Factor"));
            sTemp = this["Factor"];
            sTemp.AddRule((new string[] { "L_BRACKETS", "Expr", "R_BRACKETS" }).ToList(),
                new ActionDelegate(delegate(string str, double val)
                {
                    sList = new SymbleList(sST, 3);
                    Symble s = new Symble("Factor");
                    sST.Push(s);
                    return "Factor -> ( Expr )";
                })

                );
            sTemp.AddRule((new string[] { "VAR" }).ToList(),
                new ActionDelegate(delegate(string str, double val)
                {
                    sList = new SymbleList(sST, 1);

                    if (vD.ContainsKey(sList[0].str))
                    {
                        Symble s = new Symble("Factor");
                        sST.Push(s);
                        _linkData.OutPutStr += "VAR " + sList[0].str + "\n";
                        return "Factor -> VAR";
                    }
                    else
                    {
                        Symble s = new Symble("ERR");
                        s.str = "ERR:VAR \"" + sList[0].str + "\" not defined!";
                        //OutStr += "VAR " + sList[0].str + "\n";
                        sST.Push(s);
                        return s.str;
                    }

                })

                );
            sTemp.AddRule((new string[] { "REF_NUM" }).ToList(),
                new ActionDelegate(delegate(string str, double val)
                {
                    sList = new SymbleList(sST, 1);
                    //Symble s = new Symble("Factor");
                    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    //Get value from Tester DataCenter
                    double? rVal = _linkData.GetValD.Invoke(sList[0].str,null);
                    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    if (rVal != null)
                    {
                        if (!vD.ContainsKey(sList[0].str))
                        {
                            vD.Add(sList[0].str, 0);
                        }
                        vD[sList[0].str] = (double)rVal;

                        Symble s = new Symble("Factor");
                        sST.Push(s);
                        _linkData.OutPutStr += "REF_NUM " + sList[0].str + "\n";
                        return "Factor -> REF_NUM";
                    }
                    else 
                    {
                        Symble s = new Symble("ERR");
                        s.str = "ERR:RRF_VAR \"" + sList[0].str + "\" Search Fail!";
                        sST.Push(s);
                        return s.str;
                    }
                })

                );
            sTemp.AddRule((new string[] { "NUM" }).ToList(),
                new ActionDelegate(delegate(string str, double val)
                {
                    sList = new SymbleList(sST, 1);
                    Symble s = new Symble("Factor");
                    sST.Push(s);
                    _linkData.OutPutStr += "NUM " + sList[0].val + "\n";
                    return "Factor -> NUM";
                })

                );
            #endregion
        }
    }
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace MPI.Tester.Data
{
    

    class Scanner
    {
        const string ID = "^[a-zA-Z_]+[a-zA-Z_0-9]*$";
        //const string digit = "^[0-9]+[.][]*";//
        const string Num = "^(-?\\d+)(\\.\\d+)?$";//"^(-?\\d+)(.\\d+)?(\s|\n)";
        //const string Num = "^(-?[0-9])+(.[0-9]+)?[^0-9]";
       // const string Num = "^[-?[0-9]]+[.[0-9]+]?[^a-zA-Z_]";
        //const string oper = "[+|-|*|/|(|)|=]";
        //const string oper = "[+|-|*|/]$";
        //const string add = "[+]$";
        //const string sub = "[-]$";
        //const string multi = "[*]$";
        //const string devide = "[/]$"; 
        const string refVar = "^[F|M]:[a-zA-Z_]+[a-zA-Z_0-9]*$";

        List<string> _strList = new List<string> ();
        Dictionary<ETType, Regex> TypeRegDic = new Dictionary<ETType, Regex>();
        LinkData _linkData;


        public Scanner()
        {
            _strList = new List<string>();
            CreateRegDic();
        }

        public Scanner(LinkData ld):this()
        {
            _linkData = ld;
        }
       
        public List<TokenInfo> Scan(string str , int line = 0)
        {
            List<TokenInfo> tList = new List<TokenInfo>();
            //int listLength = str.Length;

            //for (int i = 0; i < listLength; ++i)
            //{
            string[] strArr = str.Split(new char[]{'\t',' ','\n'});
            int length = strArr.Length;
            for (int i = 0; i < length  ; ++i)
            {
                if (strArr[i] == " " || strArr[i] == "")
                {
                    continue;
                }
                string nowStr = strArr[i].Trim();

                ETType type = TryMatch(nowStr);

                if (type != ETType.NONE)
                {
                    tList.Add(MakeToken(type, ref nowStr, line));
                }
                else
                {
                    tList = new List<TokenInfo>();
                    TokenInfo ti = new TokenInfo(ETType.ERR, nowStr, line);
                    tList.Add(ti);
                    return tList;
                }

                //}

            }

            TokenInfo tiEnd = new TokenInfo(ETType.EOL, "", line);

            tList.Add(tiEnd);


            return tList;
        }

        private TokenInfo MakeToken(ETType type,ref string  nowStr,int line  = 0)
        {
            nowStr = nowStr.Trim();
            Match match1 = TypeRegDic[type].Match(nowStr);
            string matchStr = match1.Value;
            string oriStr;
            oriStr= string.Copy(nowStr);
            nowStr = nowStr.Substring(matchStr.Length);

            TokenInfo tokenInfo = new TokenInfo(type, matchStr, line);

            switch (type)
            {
                case ETType.NUM:
                    
                    if (!TypeRegDic[ETType.VAR].IsMatch(nowStr) &&
                        !TypeRegDic[ETType.REF_NUM].IsMatch(nowStr))
                    {
                        tokenInfo.val = double.Parse(matchStr);
                    }
                    else 
                    {
                        tokenInfo.TType = ETType.ERR;
                        tokenInfo.Str = oriStr;
                    }
                    
                    break;
                default:
                    break;
            }

            return tokenInfo;
        }

        private void CreateRegDic()
        {
            //Regex r
            RegexOptions opt = new RegexOptions();
            opt =  RegexOptions.Singleline;

            //越前面越優先判定，因此return 要在VAR前面
            TypeRegDic.Add(ETType.RETURN, new Regex("^return$", opt));            
            TypeRegDic.Add(ETType.REF_NUM, new Regex(refVar, opt));
            TypeRegDic.Add(ETType.VAR, new Regex(ID, opt));
            TypeRegDic.Add(ETType.NUM, new Regex(Num, opt));
            //TypeRegDic.Add(ETType.OPERATOR, new Regex(oper, opt));
            TypeRegDic.Add(ETType.ADD, new Regex("[+]$", opt));
            TypeRegDic.Add(ETType.SUB, new Regex("[-]$", opt));
            TypeRegDic.Add(ETType.MULTI, new Regex("[*]$", opt));
            TypeRegDic.Add(ETType.DIVIDE, new Regex("[/]$", opt));
            TypeRegDic.Add(ETType.EOL, new Regex("^[\n|\r]$", opt));
            TypeRegDic.Add(ETType.ASSIGN, new Regex("^=$", opt));
            TypeRegDic.Add(ETType.L_BRACKETS, new Regex("^[(]$", opt));
            TypeRegDic.Add(ETType.R_BRACKETS, new Regex("^[)]$", opt));

 
        }

        private ETType TryMatch(string str)
        {
            string tStr = str.Trim();
            
            foreach (KeyValuePair<ETType, Regex> trp in TypeRegDic)
            {
                if (trp.Value.IsMatch(tStr))
                {
                    return trp.Key;
                }
            }
            return ETType.NONE;
        }
        
    }



}

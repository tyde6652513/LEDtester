using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using MPI.Tester.Data;

namespace MPI.Tester.Gui
{
    public partial class frmItemSettingCALC : Form, IConditionUICtrl
    {
        private object _lockObj;

        private CALCTestItem _item;
        private string localAssemble;
        private SimpleCompiler SC;

        public frmItemSettingCALC()
        {
            InitializeComponent();
            
            this._lockObj = new object();

            this._item = new CALCTestItem();

            this.cmbCalcAddItemA.SelectedIndexChanged += new System.EventHandler(this.UpdateDataEventHandler);
            this.cmbCalcAddItemB.SelectedIndexChanged += new System.EventHandler(this.UpdateDataEventHandler);
            this.rdSubtract.CheckedChanged += new System.EventHandler(this.UpdateDataEventHandler);
            this.rdDivide.CheckedChanged += new System.EventHandler(this.UpdateDataEventHandler);
            this.rdAdd.CheckedChanged += new System.EventHandler(this.UpdateDataEventHandler);
            this.rdMultiple.CheckedChanged += new System.EventHandler(this.UpdateDataEventHandler);
            this.chbUseValB.CheckedChanged += new System.EventHandler(this.UpdateDataEventHandler);
            this.chbUseValA.CheckedChanged += new System.EventHandler(this.UpdateDataEventHandler);
            //localAssemble = "";
            RefreshUI();
            SC = new SimpleCompiler();
            
            //SC.AssignFindDelegate(GetVal);
            SC.AssignFindDelegate(DataCenter._conditionCtrl.GetValue);
            //superTabControl1.SelectedTabIndex
        }

        public frmItemSettingCALC(TestItemDescription description):this()
        {
            UpdateItemBoudary( description);
            RefreshUI();
        }
        

        #region >>> Public Property <<<

        #endregion

        #region >>> Private Method <<<

        private void btnCompile_Click(object sender, EventArgs e)
        {
            rtbOutput.Clear();

            string outStr = SC.JustCompileIt(rtbInput.Text);

            if(!outStr.ToUpper().Contains("ERR"))
            {
                localAssemble = SC.GetCompiledCode();
            }
            
            rtbOutput.Text = outStr;
        }

        private void btnTestRun_Click(object sender, EventArgs e)
        {
            rtbOutput.Clear();
            string outStr = SC.JustCompileIt(rtbInput.Text);

            if (!outStr.ToUpper().Contains("ERR"))
            {
                localAssemble = SC.GetCompiledCode();
                if (localAssemble != "")
                {
                    outStr = "Get Num: " + SC.RunCode(localAssemble).ToString();
                }
            }
            rtbOutput.Text = outStr;
        }

        private void UpdateDataEventHandler(object sender, EventArgs e)
        {
            this.lblCalcFunc.Text = "Calc Function";

            if (this.cmbCalcAddItemA.Items.Count == 0 || this.cmbCalcAddItemB.Items.Count == 0)
            {
                return;
            }

            //this.gBxGain.Visible = true;
            if (this.gBxGain.Visible == true)
            {
                this.dinGain.Value = this._item.Gain;
            }
            else
            {
                this.dinGain.Value = 1;
            }

            if (this.cmbCalcAddItemA.SelectedIndex != -1 && this.cmbCalcAddItemB.SelectedIndex != -1)
            {               

                string itemA = this.cmbCalcAddItemA.SelectedItem.ToString();//(this.cmbCalcAddItemA.SelectedItem as TestResultData).Name;

                string itemB = this.cmbCalcAddItemB.SelectedItem.ToString(); //(this.cmbCalcAddItemB.SelectedItem as TestResultData).Name;

                string val2 = itemB;

                string val1 = itemA;

                if (chbUseValA.Checked)
                {
                    dinValA.Enabled = true;
                    val1 = dinValA.Value.ToString("0.##########");
                    this.cmbCalcAddItemA.Enabled = false;
                }
                else
                {
                    dinValA.Enabled = false;
                    this.cmbCalcAddItemA.Enabled = true;
                }


                if (chbUseValB.Checked )
                {
                    dinValB.Enabled = true;
                    val2 = dinValB.Value.ToString("0.##########");
                    this.cmbCalcAddItemB.Enabled = false;
                }
                else 
                {
                    dinValB.Enabled = false;
                    this.cmbCalcAddItemB.Enabled = true;
                }
                chbUseValA.Visible = true;
                dinValA.Visible = true;

                chbUseValB.Visible = true;
                dinValB.Visible = true;

                if (this.rdSubtract.Checked)
                {
                    RefreshUI(itemA, itemB);
                    //this.lblCalcFunc.Text = string.Format("({0}  -  {1}) * " + dinGain.Value.ToString("0.#####"), itemA, val2);
                    this.lblCalcFunc.Text = string.Format("({0}  -  {1})", val1, val2);

                    //this.lblCalcFunc.Text = string.Format("{0}  -  {1}", itemA, itemB);
                    //this.gBxGain.Visible = false;
                    
                }
                else if (this.rdDivide.Checked)
                {
                    RefreshUI(itemA, itemB);
                    //this.lblCalcFunc.Text = string.Format("({0}  /  {1}) * " + dinGain.Value.ToString("0.#####"), itemA, val2);
                    this.lblCalcFunc.Text = string.Format("({0}  /  {1}) ", val1, val2);
                }
                else if (this.rdAdd.Checked)
                {
                    RefreshUI(itemA, itemB);
                    //this.lblCalcFunc.Text = string.Format("({0}  +  {1}) * " + dinGain.Value.ToString("0.#####"), itemA, val2);
                    this.lblCalcFunc.Text = string.Format("({0}  +  {1})", val1, val2);
                    //this.lblCalcFunc.Text = string.Format("{0}  +  {1}", itemA, itemB);
                    //this.gBxGain.Visible = false;
                }
                else if (this.rdMultiple.Checked)
                {
                    RefreshUI(itemA, itemB);
                    //this.lblCalcFunc.Text = string.Format("({0}  *  {1}) * " + dinGain.Value.ToString("0.#####"), itemA, val2);
                    this.lblCalcFunc.Text = string.Format("{0}  *  {1}", val1, itemB);
                }
                else if (this.rdDeltaR.Checked)
                {
                    chbUseValA.Checked = false;
                    chbUseValB.Checked = false;

                    ReSetDeltaRUI(itemA, itemB);

                    string deltaX = string.Empty;

                    string deltaY = string.Empty;

                    //this.lblCalcFunc.Text = "(" + itemA.ToString() + " deltaR " + itemB.ToString() + ")*" + dinGain.Value.ToString("0.#####");
                    this.lblCalcFunc.Text = "(" + itemA.ToString() + " deltaR " + itemB.ToString() + ")";

                    chbUseValA.Visible = false;
                    chbUseValB.Visible = false;

                    dinValA.Visible = false;
                    dinValB.Visible = false;

                }

                if (this.gBxGain.Visible == true)
                {
                    string str = this.lblCalcFunc.Text;
                    this.lblCalcFunc.Text = str + " * " + dinGain.Value.ToString("0.#####");
                }
            }
        }

        private void RefreshUI(string itemA, string itemB)
        {
            if (!IsTheSame())
            {
                this.cmbCalcAddItemA.Items.Clear();
                this.cmbCalcAddItemB.Items.Clear();

                List<string> nameList = MakeNameList(this._item.MsrtResult[0].Name);
                foreach (string str in nameList)
                {
                    this.cmbCalcAddItemA.Items.Add(str);
                    this.cmbCalcAddItemB.Items.Add(str);
                }

                SelectItems(itemA, itemB);
            }
        }

        private void UpdateItemBoudary(TestItemDescription description)
        {
            foreach (var data in description.Property)
            {
                EItemDescription keyName = (EItemDescription)Enum.Parse(typeof(EItemDescription), data.PropertyKeyName);

                switch (keyName)
                {
                    case EItemDescription.CALC_GAIN:
                        {
                            if (data.DefaultValue >0)
                            {
                                gBxGain.Visible = true;
                                chbUseValA.Visible = true;
                                dinValA.Visible = true;
                                chbUseValB.Visible = true;
                                dinValB.Visible = true;
                            }
                            else 
                            {
                                gBxGain.Visible = false;
                                dinGain.Value = 1;
                                dinValA.Visible = false;
                                dinValA.Value = 0;
                                chbUseValA.Visible = false;
                                chbUseValA.Checked = false;

                                dinValB.Visible = false;
                                dinValB.Value = 0;
                                chbUseValB.Visible = false;
                                chbUseValB.Checked = false;
                            }
                        }
                        break;

                    case EItemDescription.CALC_ADV:
                        {
                            if (data.DefaultValue > 0)
                            {
                                tbiAdv.Visible = true;
                                //tbiAdv.Enabled = true;
                            }
                            else 
                            {
                                tbiAdv.Visible = false;
                                //tbiAdv.Enabled = false;
                                superTabControl1.SelectedTabIndex = 0;
                            }
                        }
                        break;
                }
            }
        }

        private bool IsTheSame()
        {
            bool IsTheSame = true;

            List<string> nameList = MakeNameList(this._item.MsrtResult[0].Name);

            if (nameList.Count != this.cmbCalcAddItemA.Items.Count)
            {
                return false;
            }

            foreach (var item in this.cmbCalcAddItemA.Items)
            {
                if (!nameList.Contains(item.ToString()))
                {
                    return false;
                }
            }

            return IsTheSame;
        }

        private List<string> MakeNameList(string name = "")
        {
            List<string> nameList = new List<string>();
            if (DataCenter._product.TestCondition.TestItemArray != null)
            {
                foreach (TestItemData tc in DataCenter._product.TestCondition.TestItemArray)
                {
                    foreach (TestResultData result in tc.MsrtResult)
                    {
                        if (result.IsEnable == true && result.Name != name)
                        {
                            nameList.Add(result.Name);
                        }
                    }
                }
            }
            return nameList;
        }

        private void ReSetDeltaRUI(string itemA = "", string itemB = "")
        {
            if (!IsTheSame4DeltaR())
            {
                this.cmbCalcAddItemA.Items.Clear();
                this.cmbCalcAddItemB.Items.Clear();

                List<string> nameList = MmakeList4DeltaR(this._item.MsrtResult[0].Name);
                foreach (string str in nameList)
                {
                    this.cmbCalcAddItemA.Items.Add(str);
                    this.cmbCalcAddItemB.Items.Add(str);
                }

                SelectItems(itemA, itemB);
            }
        }

        private bool IsTheSame4DeltaR()
        {
            bool IsTheSame = true;

            List<string> nameList = MmakeList4DeltaR(this._item.MsrtResult[0].Name);

            foreach (var item in this.cmbCalcAddItemA.Items)
            {
                if (!nameList.Contains(item.ToString()))
                {
                    return false;
                }
            }
            foreach (var item in this.cmbCalcAddItemA.Items)
            {
                if (!nameList.Contains(item.ToString()))
                {
                    return false;
                }
            }
            return IsTheSame;
        }

        private List<string> MmakeList4DeltaR(string name = "")
        {
            List<string> nameList = new List<string>();

            if (DataCenter._product.TestCondition.TestItemArray != null)
            {
                foreach (TestItemData tc in DataCenter._product.TestCondition.TestItemArray)
                {
                    if (tc.Type == ETestType.IF ||
                        tc.Type == ETestType.VF||
                        tc.Type == ETestType.IZ||
                        tc.Type == ETestType.VR||
                        tc.Type == ETestType.LOPWL 
                        )
                    {
                        foreach (TestResultData result in tc.MsrtResult)
                        {
                            if (result.IsEnable == true && result.Name != name)
                            {
                                nameList.Add(result.Name);
                            }
                        }
                    }

                }
            }
            return nameList;
        }

        private void SelectItems(string itemA, string itemB)
        {
            if (this.cmbCalcAddItemA.Items.Contains(itemA))
            {
                cmbCalcAddItemA.SelectedIndex = this.cmbCalcAddItemA.Items.IndexOf(itemA);
            }
            else
            {
                cmbCalcAddItemA.SelectedIndex = 0;
            }

            if (this.cmbCalcAddItemB.Items.Contains(itemB))
            {
                cmbCalcAddItemB.SelectedIndex = this.cmbCalcAddItemB.Items.IndexOf(itemB);
            }
            else
            {
                cmbCalcAddItemB.SelectedIndex = 0;
            }
        }

        private string GetKeyByName(string name)
        {
            if (DataCenter._product.TestCondition.TestItemArray != null)
            {
                foreach (TestItemData td in DataCenter._product.TestCondition.TestItemArray)
                {
                    foreach (TestResultData rd in td.MsrtResult)
                    {
                        if (rd.Name == name)
                        {
                            return rd.KeyName;
                        }
                    }
                }
            }
            return null;
        }

        private bool CheckIfPass()
        {
            if (!localAssemble.ToUpper().Contains("ERR") &&
                localAssemble != "")
            {
                return true;
            }
            return false;
        }

        private string MakeAssemble()//一般模式下將UI設定轉換成Assemble
        {
            string itemA = this.cmbCalcAddItemA.SelectedItem.ToString();//(this.cmbCalcAddItemA.SelectedItem as TestResultData).Name;

            string itemB = this.cmbCalcAddItemB.SelectedItem.ToString(); //(this.cmbCalcAddItemB.SelectedItem as TestResultData).Name;

            double val = 0;
            //SC

            string assemlble = "";
            string itemAName = this.cmbCalcAddItemA.SelectedItem.ToString();
            string itemBName = this.cmbCalcAddItemB.SelectedItem.ToString();

            ECalcType calcType = ECalcType.Add;

            if (this.rdSubtract.Checked)
            {
                calcType = ECalcType.Subtract;
            }
            else if (this.rdDivide.Checked)
            {
                calcType = ECalcType.DivideBy;
            }
            else if (this.rdAdd.Checked)
            {
                calcType = ECalcType.Add;
            }
            else if (this.rdMultiple.Checked)
            {
                calcType = ECalcType.Multiple;
            }
            else if (this.rdDeltaR.Checked)
            {
                calcType = ECalcType.DeltaR;
            }

            if (calcType != ECalcType.DeltaR)
            {
                if (chbUseValA.Checked)
                {
                    double valueA = dinValA.Value;
                    assemlble += "NUM " + valueA.ToString() + "\n";
                }
                else
                {
                    assemlble += "REF_NUM M:" + itemAName + "\n";
                }

                if (chbUseValB.Checked)
                {
                    double valueB = dinValB.Value;
                    assemlble += "NUM " + valueB.ToString() + "\n";
                }
                else
                {
                    assemlble += "REF_NUM M:" + itemBName + "\n";
                }
                switch (calcType)
                {
                    case ECalcType.Subtract:
                        assemlble += "SUB $0 $1\n";
                        break;
                    case ECalcType.DivideBy:
                        assemlble += "DIVIDE $0 $1\n";
                        break;
                    case ECalcType.Add:
                        assemlble += "ADD $0 $1\n";
                        break;
                    case ECalcType.Multiple:
                        assemlble += "MULTI $0 $1\n";
                        break;

                }
            }
            else
            {
                assemlble += GetNumerator(GetKeyByName(this.cmbCalcAddItemA.SelectedItem.ToString()));
                assemlble += GetNumerator(GetKeyByName(this.cmbCalcAddItemB.SelectedItem.ToString()));
                assemlble += "SUB $0 $1\n";
                assemlble += GetDenominator(GetKeyByName(this.cmbCalcAddItemA.SelectedItem.ToString()));
                assemlble += GetDenominator(GetKeyByName(this.cmbCalcAddItemB.SelectedItem.ToString()));
                assemlble += "SUB $0 $1\n";
                assemlble += "DIVIDE $0 $1\n";

            }

            assemlble += "NUM " + dinGain.Value.ToString() + "\n";
            assemlble += "MULTI $0 $1\n";
            assemlble += "RETURN  $0\n";


            return assemlble;
        }

        private string GetNumerator(string keyName)//分子
        {
            double unitFactor = 1;
            string str = "";

            foreach (var subItem in DataCenter._product.TestCondition.TestItemArray)
            {
                foreach (var subResult in subItem.MsrtResult)
                {
                    if (keyName == subResult.KeyName)
                    {
                        if (subItem is IFTestItem ||
                            subItem is IZTestItem)
                        {
                            unitFactor = MPI.Tester.Maths.UnitMath.ToSIUnit(subItem.MsrtResult[0].Unit);
                            str += "REF_NUM M:" + subResult.Name + "\n";
                            str += "NUM " + unitFactor.ToString() + "\n";

                            break;
                        }
                        else if (subItem is VFTestItem ||
                            subItem is VRTestItem)
                        {
                            unitFactor = MPI.Tester.Maths.UnitMath.ToSIUnit(subItem.ElecSetting[0].ForceUnit);
                            str += "REF_NUM F:" + subResult.Name + "\n";
                            str += "NUM " + unitFactor.ToString() + "\n";

                        }
                    }
                }
            }
            str += "MULTI $0 $1\n";
            return str;
        }

        private string GetDenominator(string keyName)//分母
        {
            double unitFactor = 1;
            string str = "";

            foreach (var subItem in DataCenter._product.TestCondition.TestItemArray)
            {
                foreach (var subResult in subItem.MsrtResult)
                {
                    if (keyName == subResult.KeyName)
                    {
                        if (subItem is IFTestItem ||
                            subItem is IZTestItem)
                        {
                            unitFactor = MPI.Tester.Maths.UnitMath.ToSIUnit(subItem.ElecSetting[0].ForceUnit);
                            str += "REF_NUM F:" + subResult.Name + "\n";
                            str += "NUM " + unitFactor.ToString() + "\n";

                            break;
                        }
                        else if (subItem is VFTestItem ||
                            subItem is VRTestItem)
                        {
                            unitFactor = MPI.Tester.Maths.UnitMath.ToSIUnit(subItem.MsrtResult[0].Unit);
                            str += "REF_NUM M:" + subResult.Name + "\n";
                            str += "NUM " + unitFactor.ToString() + "\n";
                        }
                    }
                }
            }

            str += "MULTI $0 $1\n";
            return str;
        }
        #endregion

        #region >>> Public Method <<<

        public void RefreshUI()
        {
            superTabControl1.SelectedTabIndex = 0;

            this.cmbCalcAddItemA.Items.Clear();
            this.cmbCalcAddItemB.Items.Clear();

            List<string> nameList = MakeNameList(this._item.MsrtResult[0].Name);
            foreach (string str in nameList)
            {
                this.cmbCalcAddItemA.Items.Add(str);
                this.cmbCalcAddItemB.Items.Add(str);
            }
            if (cmbCalcAddItemA.Items != null &&
                cmbCalcAddItemA.Items.Count > 0)
            {
                cmbCalcAddItemA.SelectedIndex = 0;
            }
            if (cmbCalcAddItemB.Items != null &&
    cmbCalcAddItemB.Items.Count > 0)
            {
                cmbCalcAddItemB.SelectedIndex = 0;
            }

            if (DataCenter._uiSetting.UserID == EUserID.DOWA)
            {
                gbRemark.Visible = true;
                tbRemark.Text = _item.Remark;
            }
            else
            {
                gbRemark.Visible = false;
            }

            rtbOutput.Text = "";
            rtbInput.Text = "";
            localAssemble = "";

            
            //this.cmbCalcAddItemB.Items.Add(this._item.MsrtResult[0].Name);
        }
  
        public bool CheckUI(out string msg)
        {
            msg = string.Empty;

            return true;
        }

        public void UpdateCondtionDataToComponent(TestItemData data)
        {
            this._item = (data as CALCTestItem).Clone() as CALCTestItem;
            RefreshUI();

            if (_item.IsAdvanceMode)
            {
                superTabControl1.SelectedTabIndex = 1;
                localAssemble = _item.LocalAssemble;
            }
            else 
            {
                superTabControl1.SelectedTabIndex = 0;
            }

            rtbInput.Text = _item.UserCommand;

            //this.gBxGain.Visible = true;
            if (this.gBxGain.Visible == true)
            {
                this.dinGain.Value = this._item.Gain;
            }
            else 
            {
                this.dinGain.Value = 1;
            }

            this.chbUseValA.Checked = true;
            this.dinValA.Value = this._item.ValA;
            this.chbUseValA.Checked = this._item.IsAConst;
            
            
            this.chbUseValB.Checked = true;
            this.dinValB.Value = this._item.ValB;
            this.chbUseValB.Checked = this._item.IsBConst;

            if (DataCenter._uiSetting.UserID == EUserID.DOWA)
            {
                gbRemark.Visible = true;
                tbRemark.Text = _item.Remark;
            }
            else
            {
                gbRemark.Visible = false;                
            }

            switch (this._item.CalcType)
            {
                case ECalcType.Subtract:
                    this.rdSubtract.Checked = true;                    
                    break;
                case ECalcType.DivideBy:
                    this.rdDivide.Checked = true;
                    break;
                case ECalcType.Add:
                    this.rdAdd.Checked = true;
                    break;
                case ECalcType.Multiple:
                    this.rdMultiple.Checked = true;
                    break;
                case ECalcType.DeltaR:
                    this.rdDeltaR.Checked = true;
                    ReSetDeltaRUI();
                    break;
            }
            // item A
            if (this._item.ItemKeyNameA != null)
            {
                if (DataCenter._product.TestCondition.TestItemArray != null)
                {
                    foreach (TestItemData tc in DataCenter._product.TestCondition.TestItemArray)
                    {
                        if (tc.MsrtResult == null)
                            continue;

                        foreach (TestResultData result in tc.MsrtResult)
                        {
                            if (result.KeyName == this._item.ItemKeyNameA)
                            {
                                if (this.cmbCalcAddItemA.Items.Contains(result.Name))
                                {
                                    this.cmbCalcAddItemA.SelectedItem = result.Name;
                                }
                                else
                                {
                                    this.cmbCalcAddItemA.SelectedItem = null;
                                }
                                break;
                            }
                        }
                    }
                }
            }
            // item B
            if (this._item.ItemKeyNameB != null)
            {
                if (DataCenter._product.TestCondition.TestItemArray != null)
                {
                    foreach (TestItemData tc in DataCenter._product.TestCondition.TestItemArray)
                    {
                        if (tc.MsrtResult == null)
                            continue;

                        foreach (TestResultData result in tc.MsrtResult)
                        {
                            if (result.KeyName == this._item.ItemKeyNameB)
                            {
                                if (this.cmbCalcAddItemB.Items.Contains(result.Name))
                                {
                                    this.cmbCalcAddItemB.SelectedItem = result.Name;
                                }
                                else
                                {
                                    this.cmbCalcAddItemB.SelectedItem = null;
                                }
                                break;
                            }
                        }
                    }
                }
                else
                {
                    this.cmbCalcAddItemB.SelectedItem = null;
                }
            }

            
        }        

        public TestItemData GetConditionDataFromComponent()
        {
            this._item.ItemNameA = this.cmbCalcAddItemA.SelectedItem.ToString(); //(this.cmbCalcAddItemA.SelectedItem as TestResultData).Name;
            this._item.ItemNameB = this.cmbCalcAddItemB.SelectedItem.ToString();// (this.cmbCalcAddItemB.SelectedItem as TestResultData).Name;
            //this._item.ItemKeyNameA = (this.cmbCalcAddItemA.SelectedItem as TestResultData).KeyName;
            //this._item.ItemKeyNameB = (this.cmbCalcAddItemB.SelectedItem as TestResultData).KeyName;
            this._item.ItemKeyNameA = GetKeyByName(this.cmbCalcAddItemA.SelectedItem.ToString());
            this._item.ItemKeyNameB = GetKeyByName(this.cmbCalcAddItemB.SelectedItem.ToString());//(this.cmbCalcAddItemB.SelectedItem as TestResultData).KeyName;
            //GetKeyByName(string name)
            //this._item.MsrtResult[0].Unit = "";
            this._item.Gain = dinGain.Value;

            if (superTabControl1.SelectedTabIndex == 1)
            {
                _item.IsAdvanceMode = true;
                if (CheckIfPass())
                {
                    _item.LocalAssemble = localAssemble;
                }
            }
            else 
            {
                _item.IsAdvanceMode = false;
                _item.LocalAssemble = MakeAssemble();
            }
            

            this._item.UserCommand = rtbInput.Text;

            this._item.IsAConst = this.chbUseValA.Checked;

            if (this.dinValA.Value != double.NaN)
            {
                this._item.ValA = this.dinValA.Value;
            }
            
            this._item.IsBConst = this.chbUseValB.Checked;

            if (this.dinValB.Value != double.NaN)
            {
                this._item.ValB = this.dinValB.Value;
            }

            if (this.rdSubtract.Checked)
            {
                this._item.CalcType = ECalcType.Subtract;
            }
            else if (this.rdDivide.Checked)
            {
                this._item.CalcType = ECalcType.DivideBy;
            }
            else if (this.rdAdd.Checked)
            {
                this._item.CalcType = ECalcType.Add;
            }
            else if (this.rdMultiple.Checked)
            {
                this._item.CalcType = ECalcType.Multiple;
            }
            else if (this.rdDeltaR.Checked)
            {
                this._item.CalcType = ECalcType.DeltaR;
            }

            if (this._item.ItemKeyNameA != null || this._item.ItemKeyNameB != null)
            {
                if (this._item.ItemKeyNameA == string.Empty || this._item.ItemKeyNameB == string.Empty)
                {
                    this._item.IsUserSetEnable = false;
                    this._item.MsrtResult[0].IsEnable = false;
                }
                else
                {
                    this._item.IsUserSetEnable = true;
                    this._item.MsrtResult[0].IsEnable = true;
                }
            }
            else
            {
                this._item.IsUserSetEnable = false;
                this._item.MsrtResult[0].IsEnable = false;
            }

            if (DataCenter._uiSetting.UserID == EUserID.DOWA)
            {
                gbRemark.Visible = true;
                _item.Remark = tbRemark.Text;
            }



            return this._item;
        }

        double? GetVal(string varName)
        {
            string[] strArr = varName.Trim().Split(new char[] { ':' });

            foreach (TestItemData item in DataCenter._product.TestCondition.TestItemArray)
            {
                if (item.IsEnable == true && item.MsrtResult != null)
                {
                    foreach (TestResultData resultData in item.MsrtResult)
                    {
                        if (strArr[1].Trim() == resultData.Name)
                        {
                            switch (strArr[0].Trim())
                            {
                                case "F":
                                case "f":
                                    return item.ElecSetting[0].ForceValue;  
                                    break;
                                case "M":
                                case "m":
                                default:
                                    return resultData.Value;      
                                    break;
                            }
                            break;

                        }
                    }
                }
            }
            return null;
        }

        #endregion

    }
}

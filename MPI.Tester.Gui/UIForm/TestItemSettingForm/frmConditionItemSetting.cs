using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using MPI.Tester.Data;
using MPI.Tester.DeviceCommon;

namespace MPI.Tester.Gui
{
    public partial class frmConditionItemSetting : Form
    {
        public event EventHandler<EventArgs> TestItemDataChangeEvent;

        private IConditionUICtrl _subFormCtrl;
        
        private EBtnActionMode _operateMode;

        private int _operateIndex;

        private List<string> _lstTestItemDisplay;


        public frmConditionItemSetting()
        {
            InitializeComponent();

            //this.btnConfirm.DialogResult = DialogResult.OK;

            this.btnCancel.DialogResult = DialogResult.Cancel;

            //this.AcceptButton = this.btnConfirm;//20180122 David,若不註解掉,Calc在輸入文字指令時按下Enter就會直接送出

            this.CancelButton = this.btnCancel;

            this.TestItemDataChangeEvent += new EventHandler<EventArgs>(Host.TestItemDataChangeEventHandler);
        }

        #region >>> Private Method <<<

      
        private void ShowItemSettingFrom(ETestType testType)
        {
            this.SuspendLayout();
            
            TestItemDescription description = DataCenter._sysSetting.SpecCtrl.ItemDescription[testType.ToString()];

            if (this._subFormCtrl != null)
            {
                (this._subFormCtrl as Form).Dispose();
            }

            //--------------------------------------------------------------------------------------------
            // (1) _subFormCtrl Create Child Form
            //--------------------------------------------------------------------------------------------

            #region >>> Create Child Form <<<

            switch (testType)
            {
                case ETestType.IF:
                    {
                        this._subFormCtrl = new frmItemSettingIF(description);

                        break;
                    }
                case ETestType.VF:
                    {
                        this._subFormCtrl = new frmItemSettingVF(description);
                        
                        break;
                    }
                case ETestType.IZ:
                    {
                        this._subFormCtrl = new frmItemSettingIZ(description);
                        
                        break;
                    }
                case ETestType.VR:
                    {
                        this._subFormCtrl = new frmItemSettingVR(description);

                        break;
                    }
                case ETestType.IFH:
                    {
                        this._subFormCtrl = new frmItemSettingIFH(description);

                        break;
                    }
                case ETestType.DVF:
                    {
                        this._subFormCtrl = new frmItemSettingDVF(description);

                        break;
                    }
                case ETestType.LOP:
                    {
                        this._subFormCtrl = new frmItemSettingLOP(description);

                        break;
                    }
                case ETestType.LOPWL:
                    {
                        if (!DataCenter._sysSetting.SpecCtrl.IsAcTestItem)
                        {
                            this._subFormCtrl = new frmItemSettingLOPWLDC(description);
                        }
                        else
                        {
                            this._subFormCtrl = new frmItemSettingLOPWLAC(description);
                        }
                        break;
                    }
                case ETestType.IVSWEEP:
                    {
                        this._subFormCtrl = new frmItemSettingIVSWEEP(description);

                        break;
                    }
                case ETestType.VISWEEP:
                    {
                        this._subFormCtrl = new frmItemSettingVISWEEP(description);

                        break;
                    }
                case ETestType.THY:
                    {
                        this._subFormCtrl = new frmItemSettingTHY(description);

                        break;
                    }
                case ETestType.CALC:
                    {
                        this._subFormCtrl = new frmItemSettingCALC(description);
                        
                        break;
                    }
                case ETestType.DIB:
                    {
                        this._subFormCtrl = new frmItemSettingDIB();

                        break;
                    }
                case ETestType.POLAR:
                    {
                        this._subFormCtrl = new frmItemSettingPOLAR(description);
                        
                        break;
                    }
                case ETestType.VAC:
                    {
                        this._subFormCtrl = new frmItemSettingVAC(description);

                        break;
                    }
                case ETestType.R:
                    {
                        this._subFormCtrl = new frmItemSettingR(description);

                        break;
                    }
				case ETestType.ContactCheck:
					{
						this._subFormCtrl = new frmItemContactCheck(description);

						break;
					}
                case ETestType.RTH:
                    {
                        this._subFormCtrl = new frmItemSettingRTH(description);
                        
                        break;
                    }
                case ETestType.LIV:
                    {
                        this._subFormCtrl = new frmItemSettingLIV(description);

                        break;
                    }
                case ETestType.VLR:
                    {

                        break;
                    }
                case ETestType.ESD:
                    {
                        this._subFormCtrl = new frmItemSettingESD(description);

                        break;
                    }
                case ETestType.VISCAN:
                    {
                        this._subFormCtrl = new frmItemSettingVISCAN(description);

                        break;
                    }
                case ETestType.PIV:
                    {
                        this._subFormCtrl = new frmItemSettingPIV(description);

                        break;
                    }
                case ETestType.TRANSISTOR:
                    {
                        this._subFormCtrl = new frmItemSettingTransistor(description);

                        break;
                    }
                case ETestType.OSA:
                    {
                        this._subFormCtrl = new frmItemSettingOSA(description);

                        break;
                    }
				case ETestType.LCR:
					{
						this._subFormCtrl = new frmItemSettingLCR(description);

						break;
					}
                case ETestType.LCRSWEEP:
                    {
                        this._subFormCtrl = new frmItemSettingLCRSweep(description);

                        break;
                    }
                case ETestType.IO:
                    {
                        this._subFormCtrl = new frmItemSettingIO(description);
                        break;
                    }
                case ETestType.LaserSource:
                    {
                        this._subFormCtrl = new frmItemSettingLaserSource();
                        break;
                    }

            }

            #endregion

            if (this._subFormCtrl != null)
            {
                if (!(this._subFormCtrl as Form).IsDisposed)
                {
                    //--------------------------------------------------------------------------------------------
                    // (2) Update Property to _subFormCtrl
                    //--------------------------------------------------------------------------------------------

                    #region >>> Update Child form (IConditionElecCtrl) Property <<<

                    if (typeof(IConditionElecCtrl).IsAssignableFrom(this._subFormCtrl.GetType()))
                    {
                        (this._subFormCtrl as IConditionElecCtrl).IsVisibleNPLC = DataCenter._uiSetting.IsShowNPLCAndSGFilterSetting;
                       // (this._subFormCtrl as IConditionElecCtrl).IsVisibleFilterCount = DataCenter._uiSetting.IsShowNPLCAndSGFilterSetting;
                        (this._subFormCtrl as IConditionElecCtrl).IsVisibleFilterCount = false;
                        (this._subFormCtrl as IConditionElecCtrl).IsAutoSelectMsrtRange = false;
                       
                        if (DataCenter._machineConfig.SwitchSystemModel != ESwitchSystemModel.NONE && DataCenter._machineConfig.TesterFunctionType == ETesterFunctionType.Single_Die)
                        {
                            (this._subFormCtrl as IConditionElecCtrl).IsEnableSwitchChannel = true;
                            (this._subFormCtrl as IConditionElecCtrl).MaxSwitchingChannelCount = (uint)DataCenter._machineInfo.MaxSwitchingChannelCount;
                        }
                        else
                        {
                            (this._subFormCtrl as IConditionElecCtrl).IsEnableSwitchChannel = false;
                            (this._subFormCtrl as IConditionElecCtrl).MaxSwitchingChannelCount = 0;
                        }

                        (this._subFormCtrl as IConditionElecCtrl).IsEnableMsrtForceValue = DataCenter._sysSetting.IsEnableSrcMeterMsrtForceValue;
                    }

                    #endregion

                    #region >>> Update Child form (VF) Property <<<

                    if (this._subFormCtrl is frmItemSettingVF)
                    {
                        (this._subFormCtrl as frmItemSettingVF).IsEnableRetest = DataCenter._sysSetting.IsEnableRetestTestItem;
                    }

                    #endregion

                    #region >>> Update Child form (LOPWL) Property <<<

                    if (this._subFormCtrl is frmItemSettingLOPWLDC)
                    {
                        (this._subFormCtrl as frmItemSettingLOPWLDC).IsVisibleForceMode = DataCenter._rdFunc.RDFuncData.IsEnableForceMode;

                        Dictionary<string, string> tempDict = new Dictionary<string, string>();

                        if (DataCenter._conditionCtrl.Data.TestItemArray != null)
                        {
                            if (DataCenter._conditionCtrl.Data.TestItemArray.Length > 0)
                            {
                                foreach (var item in DataCenter._conditionCtrl.Data.TestItemArray)
                                {
                                    if (item.Type == ETestType.PIV)
                                    {
                                        foreach (var result in item.MsrtResult)
                                        {
                                            if (result.KeyName.Contains(ELaserMsrtType.Ith.ToString()) ||
                                                result.KeyName.Contains(ELaserMsrtType.Iop.ToString()))
                                            {
                                                tempDict.Add(result.KeyName, result.Name);
                                            }
                                        }   
                                    }
                                }
                            }
                        }

                        (this._subFormCtrl as frmItemSettingLOPWLDC).RefItem = tempDict;
                    }

                    #endregion

                    #region >>> Update Child form (LOP) Property <<<

                    if (this._subFormCtrl is frmItemSettingLOP)
                    {
                        (this._subFormCtrl as frmItemSettingLOP).IsVisibleForceMode = DataCenter._rdFunc.RDFuncData.IsEnableForceMode;

                        if (DataCenter._machineConfig.PDSensingMode == EPDSensingMode.SrcMeter_2nd)
                        {
                            if (DataCenter._machineInfo.SourceMeterSpec != null)
                            {
                              (this._subFormCtrl as frmItemSettingLOP).IsSupportedDetectorB = DataCenter._machineInfo.SourceMeterSpec.IsSupportedDualDetectorCH;
                            }
                        }
                    }

                    #endregion

                    #region >>> Update Child form (VR or IZ) Property <<<

                    if (this._subFormCtrl is frmItemSettingVR ||
                        this._subFormCtrl is frmItemSettingIZ)
                    {
                        Dictionary<string, string> tempDictVR = new Dictionary<string, string>();
                        Dictionary<string, string> tempDictIZ = new Dictionary<string, string>();

                        if (DataCenter._conditionCtrl.Data.TestItemArray != null)
                        {
                            if (DataCenter._conditionCtrl.Data.TestItemArray.Length > 0)
                            {
                                foreach (var item in DataCenter._conditionCtrl.Data.TestItemArray)
                                {
                                    if (item.Type == ETestType.IZ)
                                    {
                                        tempDictVR.Add(item.MsrtResult[0].KeyName, item.MsrtResult[0].Name);
                                    }
                                    if (item.Type == ETestType.VR)
                                    {
                                        tempDictIZ.Add(item.MsrtResult[0].KeyName, item.MsrtResult[0].Name);
                                    }
                                }
                            }
                        }
                        if (this._subFormCtrl is frmItemSettingVR)
                        {
                            (this._subFormCtrl as frmItemSettingVR).VzItem = tempDictVR;
                        }
                        else 
                        {
                            (this._subFormCtrl as frmItemSettingIZ).IrItem = tempDictIZ;
                        }
                    }

                    #endregion

                    #region >>> Update Child form (LIV) Property <<<

                    if (this._subFormCtrl is frmItemSettingLIV)
                    {
                        bool isEnableDetectorChannel = true;

                        if (DataCenter._machineConfig.PDSensingMode == EPDSensingMode.NONE)
                        {
                            isEnableDetectorChannel = false;
                        }
                        
                        (this._subFormCtrl as frmItemSettingLIV).IsVisibleForceMode = DataCenter._rdFunc.RDFuncData.IsEnableForceMode;

                        (this._subFormCtrl as frmItemSettingLIV).IsEnablePDDetector = isEnableDetectorChannel;
                    }

                    #endregion

                    #region >>> Update Child form (PIV) Property <<<

                    if (this._subFormCtrl is frmItemSettingPIV)
                    {
                        if (DataCenter._uiSetting.LoginID == "simulator")
                        {
                            (this._subFormCtrl as frmItemSettingPIV).Authority = EAuthority.Admin;
                        }
                        else
                        {
                            (this._subFormCtrl as frmItemSettingPIV).Authority = DataCenter._userManag.CurrentAuthority;
                        }

                        (this._subFormCtrl as frmItemSettingPIV).UserID = DataCenter._uiSetting.UserID;
                    }

                    #endregion

                    #region >>> Update Child form (Transistor) Property <<<

                    if (this._subFormCtrl is frmItemSettingTransistor)
                    {
                        bool isEnableDetectorChannel = true;
                        bool isEnableSpectrometer = true;

                        if (DataCenter._machineConfig.PDSensingMode == EPDSensingMode.NONE)
                        {
                            isEnableDetectorChannel = false;
                        }

                        if (DataCenter._machineConfig.SpectrometerModel == ESpectrometerModel.NONE)
                        {
                            isEnableSpectrometer = false;
                        }

                        (this._subFormCtrl as frmItemSettingTransistor).IsEnablePDDetector = isEnableDetectorChannel;
                        (this._subFormCtrl as frmItemSettingTransistor).IsEnableSpectrometer = isEnableSpectrometer;
                    }

                    #endregion

                    this._subFormCtrl.RefreshUI(); // 設定完 subform property 後, RefreshUI
                    
                    //--------------------------------------------------------------------------------------------
                    // (3) Include Item Setting From into Panel
                    //--------------------------------------------------------------------------------------------
                    (this._subFormCtrl as Form).TopLevel = false;
              
                    (this._subFormCtrl as Form).Parent = this.pnlCondition;

                    (this._subFormCtrl as Form).FormBorderStyle = FormBorderStyle.None;

                    (this._subFormCtrl as Form).Dock = DockStyle.Top;

                    // Resize PnlCondition
                    this.pnlCondition.Height = (this._subFormCtrl as Form).Height;

                    this.pnlCondition.Width = (this._subFormCtrl as Form).Width;

                    this.Height = this.pnlCondition.Height + 115;

                    this.Width = this.pnlCondition.Width + 30;

                    (this._subFormCtrl as Form).Show();
                }
            }

            System.Threading.Thread.Sleep(0);

            this.ResumeLayout();
        }

        private void UpdateItemSelectList()
        {
            this.cmbTestSelected.Items.Clear();

            this._lstTestItemDisplay = new List<string>(DataCenter._sysSetting.SpecCtrl.SupportedTestItems);

            //this._lstTestItemDisplay = new List<string>(Enum.GetNames(typeof(ETestType)));

            this.cmbTestSelected.Items.AddRange(this._lstTestItemDisplay.ToArray());

            if (this.cmbTestSelected.Items.Count > 0)
            {
                this.cmbTestSelected.SelectedIndex = 0;
            }
        }

        private void CheckItemEnableStatusBySettingData(TestItemData item)
        {
            //----------------------------------------------------------------------------
            //  Check the force time setting
            //  if ForceTime < 0.000001 ~ 0.0d , default is disable
            //----------------------------------------------------------------------------
            switch (item.Type)
            {
                case ETestType.IFH:
                case ETestType.IF:
                case ETestType.IZ:
                case ETestType.VF:
                case ETestType.VR:
                case ETestType.LOP:
                case ETestType.LOPWL:
                case ETestType.VAC:
                case ETestType.PIV:
                    {
                        if (item.ElecSetting[0].ForceTime < 0.000001)
                        {
                            item.IsUserSetEnable = false;
                        }
                        else
                        {
                            item.IsUserSetEnable = true;
                        }

                        break;
                    }
                //-----------------------------------------------------------
                case ETestType.DVF:
                    {
                        if ((item.ElecSetting[0].ForceTime < 0.000001) ||
                            (item.ElecSetting[1].ForceTime < 0.000001) ||
                            (item.ElecSetting[2].ForceTime < 0.000001))
                        {
                            item.IsUserSetEnable = false;
                        }
                        else
                        {
                            item.IsUserSetEnable = true;
                        }

                        break;
                    }
            
                //-----------------------------------------------------------
                case ETestType.CALC:
                    {
                        break;
                    }
                //-----------------------------------------------------------
                case ETestType.VISCAN:
                    {
                        if ((item.ElecSetting[0].SweepContCount == 0))
                        {
                            item.IsDeviceSetEnable = false;
                        }
                        else
                        {
                            item.IsDeviceSetEnable = true;
                        }

                        break;
                    }
                //-----------------------------------------------------------
                
                //---------------------------------------------------------------------
                case ETestType.IO:
                    {
                        item.IsDeviceSetEnable = true;

                        break;
                    }
                default:
                    {
                        item.IsDeviceSetEnable = true;

                        break;
                    }
            }
        }

        #endregion

        #region >>> Public Methods <<<

        public bool DialogControl(EBtnActionMode mode, int itemIndex)
        {
            this._operateMode = mode;

            this._operateIndex = itemIndex;

            //-------------------------------------------------------------------------------------------------
            // (1) 更新 Test select items 下拉式選單
            //-------------------------------------------------------------------------------------------------
            this.UpdateItemSelectList();

            //-------------------------------------------------------------------------------------------------
            // (2) UI 資料更新
            //-------------------------------------------------------------------------------------------------
            switch (this._operateMode)
            {
                case EBtnActionMode.NewTestItem:
                    {
                        this.cmbTestSelected.Enabled = true;

                        break;
                    }
                //---------------------------------------------------------------------------------------------
                case EBtnActionMode.InsertTestItem:
                    {
                        if (DataCenter._product.TestCondition.TestItemArray == null ||
                            this._operateIndex < 0 ||
                            this._operateIndex >= DataCenter._product.TestCondition.TestItemArray.Length)
                        {
                            return false;
                        }

                        this.cmbTestSelected.Enabled = true;

                        break;
                    }
                //---------------------------------------------------------------------------------------------
                case EBtnActionMode.UpdateTestItem:
                case EBtnActionMode.CopyTestItem:
                    {
                        if (DataCenter._product.TestCondition.TestItemArray == null ||
                            this._operateIndex < 0 ||
                            this._operateIndex >= DataCenter._product.TestCondition.TestItemArray.Length)
                        {
                            return false;
                        }

                        this.cmbTestSelected.Enabled = false;

                        // if the device not support the test item, and the GUI update will carsh
                        string selectedItem = DataCenter._product.TestCondition.TestItemArray[itemIndex].Type.ToString();

                        if (DataCenter._sysSetting.SpecCtrl.SupportedTestItems.Contains(selectedItem))
                        {
                            this.cmbTestSelected.SelectedItem = selectedItem;

                            TestItemData item = DataCenter._product.TestCondition.TestItemArray[this._operateIndex];

                            // 更新舊有資料 TestCondition 至 SubForm
                            this._subFormCtrl.UpdateCondtionDataToComponent(item);
                        }
                        else
                        {
                            return false;
                        }

                        break;
                    }
                //---------------------------------------------------------------------------------------------
                default:
                    {
                        return false;
                    }
            }

            DialogResult result = this.ShowDialog();

            if (!(this._subFormCtrl as Form).IsDisposed)
            {
                (this._subFormCtrl as Form).Dispose();
            }

            GC.Collect();

            if (result != DialogResult.OK)
            {
                return false;
            }

            return true;
        }

        public void Fire_TestItemDataChangeEvent()
        {
            if (this.TestItemDataChangeEvent != null)
            {
                this.TestItemDataChangeEvent(new object(), new EventArgs());
            }
        }

        #endregion

        #region >>> UI Event Handler <<<

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if (this._subFormCtrl == null)
            {
                this.DialogResult = DialogResult.Cancel;
                return;
            }
            
            if ((this._subFormCtrl as Form).IsDisposed)
            {
                this.DialogResult = DialogResult.Cancel;
                return;
            }

            string errMsg = string.Empty;

            if (!this._subFormCtrl.CheckUI(out errMsg))
            {
                MessageBox.Show(errMsg, "WARMING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            TestItemData newItem;

            bool oldItemEnableStatus = true;

            newItem = this._subFormCtrl.GetConditionDataFromComponent();

            if (this._operateMode == EBtnActionMode.UpdateTestItem)
            {
                oldItemEnableStatus = DataCenter._product.TestCondition.TestItemArray[this._operateIndex].IsEnable;
            }

            this.CheckItemEnableStatusBySettingData(newItem);

            newItem.IsUserSetEnable &= oldItemEnableStatus;

            switch (this._operateMode)
            {
                //case EBtnActionMode.NewTestItem:
                //case EBtnActionMode.CopyTestItem:
                //    {
                //        DataCenter._conditionCtrl.AddTestItem(newItem);

                //        break;
                //    }
                case EBtnActionMode.NewTestItem:
                    {
                        newItem.IsNewCreateItem = true;
                        DataCenter._conditionCtrl.AddTestItem(newItem);
                        break;
                    }
                case EBtnActionMode.CopyTestItem:
                    {
                        newItem.IsNewCreateItem = true;
                        DataCenter._conditionCtrl.AddTestItem(newItem);

                        break;
                    }
                case EBtnActionMode.InsertTestItem:
                    {
                        DataCenter._conditionCtrl.InsertTestItem(this._operateIndex, newItem);

                        break;
                    }
                case EBtnActionMode.UpdateTestItem:
                    {
                        DataCenter._conditionCtrl.UpdateTestItem(this._operateIndex, newItem);

                        break;
                    }
            }

            this.DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            // Click Event 訂閱在建構子 "this.CancelButton = this.btnCancel"

            //避免雷射設定完後按下cancle導致未切回預設通道
            if (this._subFormCtrl != null && this._subFormCtrl is frmItemSettingLaserSource)
            {
                AppSystem.Switch2D4OpticalCh();
            }
        }

        private void cmbTestSelected_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.cmbTestSelected.Items.Count == 0 || this.cmbTestSelected.SelectedIndex < 0)
            {
                return;
            }

            ETestType testType = (ETestType)Enum.Parse(typeof(ETestType), this.cmbTestSelected.SelectedItem.ToString());

            this.ShowItemSettingFrom(testType);
        }

        #endregion
    }
}

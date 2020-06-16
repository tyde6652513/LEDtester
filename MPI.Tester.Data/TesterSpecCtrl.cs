using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MPI.Tester.DeviceCommon;

namespace MPI.Tester.Data
{
    public class TesterSpecCtrl
    {
        private object _lockObj;
                                                 
        private ETesterConfigType _configType;
                       
        private List<string> _lstSupportedItems;  // 支援的Items, 可顯示於UI

        private List<string> _lstEnableItems;     // 顯示於UI的Items中, 可被啟用的項目

        private TestItemDescriptionCollections _descColl;

        public TesterSpecCtrl()
        {
            this._lockObj = new object();

            this._lstSupportedItems = new List<string>();

            this._lstEnableItems = new List<string>();

            this._configType = ETesterConfigType.LEDTester;
        }

        #region >>> Public Property <<<

        public string[] SupportedTestItems
        {
            get 
            {
                if (this._lstSupportedItems.Count == 0)
                {
                    foreach (var item in TesterSpecDescription.TESTER_LED_DEFAULT_ITEMS)
                    {
                        this._lstSupportedItems.Add(item.ToString());
                    }
                }
                
                return this._lstSupportedItems.ToArray(); 
            }
        }

        public bool IsSupportedSweepItem
        {
            get 
            {
                if (this._lstSupportedItems.Count != 0)
                {
                    if (this._lstSupportedItems.Contains(ETestType.IVSWEEP.ToString()) ||
                        this._lstSupportedItems.Contains(ETestType.VISWEEP.ToString())  ||
                         this._lstSupportedItems.Contains(ETestType.VISCAN.ToString()))
                    {
                        return true;
                    }
                }

                return false; 
            }
        }

        public bool IsSupportedLIVItem
        {
            get 
            {
                if (this._lstSupportedItems.Count != 0)
                {
                    if (this._lstSupportedItems.Contains(ETestType.LIV.ToString()))
                    {
                        return true;
                    }
                }

                return false; 
            }
        }

        //20171006 David for lcrSweep
        public bool IsSupportedLCRSweepItem
        {
            get
            {
                if (this._lstSupportedItems.Count != 0)
                {
                    if (this._lstSupportedItems.Contains(ETestType.LCRSWEEP.ToString()))
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        public bool IsSupportedPIVItem
        {
            get
            {
                if (this._lstSupportedItems.Count != 0)
                {
                    if (this._lstSupportedItems.Contains(ETestType.PIV.ToString()))
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        public bool IsSupportedLOPItem
        {
            get 
            {
                if (this._lstSupportedItems.Count != 0)
                {
					if (this._lstSupportedItems.Contains(ETestType.LOP.ToString()) || this._lstSupportedItems.Contains(ETestType.LIV.ToString()) || this._lstSupportedItems.Contains(ETestType.TRANSISTOR.ToString()))
                    {
                        return true;
                    }
                }

                return false;  
            }
        }

		public bool IsSupportedLCRItem
		{
			get
			{
				if (this._lstSupportedItems.Count != 0)
				{
					if (this._lstSupportedItems.Contains(ETestType.LCR.ToString()))
					{
						return true;
					}
				}

				return false;
			}
		}

        public bool IsSupportedWLOSAItem
        {
            get
            {
                if (this._lstSupportedItems.Count != 0)
                {
                    if (this._lstSupportedItems.Contains(ETestType.OSA.ToString()))
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        public bool IsSupportedTransistorItem
        {
            get
            {
                if (this._lstSupportedItems.Count != 0)
                {
                    if (this._lstSupportedItems.Contains(ETestType.TRANSISTOR.ToString()))
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        public bool IsAcTestItem
        {
            get
            {
                if (this._lstSupportedItems.Count != 0)
                {
                    if (this._lstSupportedItems.Contains(ETestType.VAC.ToString()))
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        public TestItemDescriptionCollections ItemDescription
        {
            get { return this._descColl; }
        }

        #endregion

        #region >>> Private Method <<<

        private void UpdateDataToSupportedItems(MachineConfig config, TestItemDescriptionCollections rdDefineData)
        {
            List<string> testTypeList = new List<string>(Enum.GetNames(typeof(ETestType)));

            List<string> userItems = new List<string>();

            // Add By User Data Enable Items
            if (rdDefineData != null)
            {
                foreach (var s in rdDefineData.TestTypeKeyNames)
                {
                    if (testTypeList.Contains(s))
                    {
                        userItems.Add(s);
                    }
                }
            }

            // check default items have been added.  // 改由 RDFunc 掌控
            //foreach (var defaultItem in TESTER_DEFAULT_ITEMS)
            //{
            //    if (!userItems.Contains(defaultItem.ToString()))
            //    {
            //        userItems.Add(defaultItem.ToString());
            //    }
            //}

            // Remove By Device support Items
            List<string> deviceSupportItem = TesterSpecDescription.GetDeviceSupportItmeList(config);

            for (int i = 0; i < userItems.Count; i++)
            {
                if (!deviceSupportItem.Contains(userItems[i]))
                {
                    userItems.RemoveAt(i);

                    i--;
                }
            }

            // 決定 Tester 能顯示的 Test Item List 並重新排序
            this._lstSupportedItems.Clear();

            foreach (var s in testTypeList)
            {
                if (userItems.Contains(s))
                {
                    this._lstSupportedItems.Add(s);
                }
            }
        }

        //private List<string> GetDeviceSupportItmeList(MachineConfig config)
        //{
        //    List<string> deviceSupportItem = new List<string>();

        //    switch (config.SourceMeterModel)
        //    {
        //        case ESourceMeterModel.LDT1A:
        //        case ESourceMeterModel.T2001L:
        //        case ESourceMeterModel.LDT3A200:
        //            {
        //                deviceSupportItem.Add(ETestType.IF.ToString());
        //                deviceSupportItem.Add(ETestType.VF.ToString());
        //                deviceSupportItem.Add(ETestType.VR.ToString());
        //                deviceSupportItem.Add(ETestType.IZ.ToString());
        //                deviceSupportItem.Add(ETestType.IFH.ToString());
        //                deviceSupportItem.Add(ETestType.DVF.ToString());
        //                deviceSupportItem.Add(ETestType.LOP.ToString());
        //                deviceSupportItem.Add(ETestType.LOPWL.ToString());
        //                deviceSupportItem.Add(ETestType.THY.ToString());
        //                deviceSupportItem.Add(ETestType.CALC.ToString());
        //                deviceSupportItem.Add(ETestType.POLAR.ToString());
        //                deviceSupportItem.Add(ETestType.IVSWEEP.ToString());
        //                deviceSupportItem.Add(ETestType.VISWEEP.ToString());
        //                deviceSupportItem.Add(ETestType.DIB.ToString());

        //                deviceSupportItem.Add(ETestType.ESD.ToString());
        //                break;
        //            }
        //        case ESourceMeterModel.N5700:
        //        case ESourceMeterModel.DR2000:
        //        case ESourceMeterModel.DSPHD:
        //            {
        //                deviceSupportItem.Add(ETestType.IF.ToString());
        //                deviceSupportItem.Add(ETestType.VF.ToString());
        //                deviceSupportItem.Add(ETestType.LOPWL.ToString());

        //                break;
        //            }
        //        case ESourceMeterModel.K2400:
        //            {
        //                deviceSupportItem.Add(ETestType.IF.ToString());
        //                deviceSupportItem.Add(ETestType.VF.ToString());
        //                deviceSupportItem.Add(ETestType.VR.ToString());
        //                deviceSupportItem.Add(ETestType.IZ.ToString());
        //                deviceSupportItem.Add(ETestType.LOPWL.ToString());
        //                deviceSupportItem.Add(ETestType.POLAR.ToString());
        //                deviceSupportItem.Add(ETestType.CALC.ToString());

        //                break;
        //            }
        //        case ESourceMeterModel.K2600:
        //            {
        //                deviceSupportItem.Add(ETestType.IF.ToString());
        //                deviceSupportItem.Add(ETestType.VF.ToString());
        //                deviceSupportItem.Add(ETestType.VR.ToString());
        //                deviceSupportItem.Add(ETestType.IZ.ToString());
        //                deviceSupportItem.Add(ETestType.IFH.ToString());
        //                deviceSupportItem.Add(ETestType.DVF.ToString());
        //                deviceSupportItem.Add(ETestType.LOP.ToString());
        //                deviceSupportItem.Add(ETestType.LOPWL.ToString());
        //                deviceSupportItem.Add(ETestType.THY.ToString());
        //                deviceSupportItem.Add(ETestType.CALC.ToString());
        //                deviceSupportItem.Add(ETestType.POLAR.ToString());
        //                deviceSupportItem.Add(ETestType.IVSWEEP.ToString());
        //                deviceSupportItem.Add(ETestType.VISWEEP.ToString());
        //                deviceSupportItem.Add(ETestType.R.ToString());
        //                deviceSupportItem.Add(ETestType.RTH.ToString());
        //                deviceSupportItem.Add(ETestType.LIV.ToString());
        //                deviceSupportItem.Add(ETestType.VLR.ToString());
        //                deviceSupportItem.Add(ETestType.DIB.ToString());

        //                deviceSupportItem.Add(ETestType.VISCAN.ToString());
        //                deviceSupportItem.Add(ETestType.PIV.ToString());
        //                deviceSupportItem.Add(ETestType.ESD.ToString());
        //                deviceSupportItem.Add(ETestType.TRANSISTOR.ToString());
        //                break;
        //            }
        //        case ESourceMeterModel.IT7321:
        //            {
        //                deviceSupportItem.Add(ETestType.VAC.ToString());
        //                deviceSupportItem.Add(ETestType.LOPWL.ToString());
        //                deviceSupportItem.Add(ETestType.CALC.ToString());
                       
        //                break;
        //            }
        //        default:
        //            {
        //                deviceSupportItem.Add(ETestType.IF.ToString());
        //                deviceSupportItem.Add(ETestType.VF.ToString());
        //                deviceSupportItem.Add(ETestType.VR.ToString());
        //                deviceSupportItem.Add(ETestType.IZ.ToString());
        //                deviceSupportItem.Add(ETestType.LOPWL.ToString());

        //                break;
        //            }
        //    }

        //    if (config.TesterFunctionType == ETesterFunctionType.Multi_Die)
        //    {
        //        if (deviceSupportItem.Contains(ETestType.DIB.ToString()))
        //        {
        //            deviceSupportItem.Remove(ETestType.DIB.ToString());
        //        }

        //        if (deviceSupportItem.Contains(ETestType.LIV.ToString()))
        //        {
        //            deviceSupportItem.Remove(ETestType.LIV.ToString());
        //        }

        //        if (deviceSupportItem.Contains(ETestType.R.ToString()))
        //        {
        //            deviceSupportItem.Remove(ETestType.R.ToString());
        //        }

        //        if (deviceSupportItem.Contains(ETestType.POLAR.ToString()))
        //        {
        //            deviceSupportItem.Remove(ETestType.POLAR.ToString());
        //        }

        //        if (deviceSupportItem.Contains(ETestType.ESD.ToString()))
        //        {
        //            deviceSupportItem.Remove(ETestType.ESD.ToString());
        //        }
        //    }

        //    return deviceSupportItem;
        //}

        private void UpdateDataByMachineInfo(MachineInfoData info, TestItemDescriptionCollections rdDefineData)
        {
            //--------------------------------------------------------------------
            // (1) 建立 Defualt 的 Test Item 描述資料
            //--------------------------------------------------------------------
            if (this._descColl != null)
            {
                this._descColl = null;
            }

            this._descColl = this.CreateAllDefaultItemsDescription();
            
            //--------------------------------------------------------------------
            // (2) 由 RdDefineData 更新 Test Item 描述資料
            //--------------------------------------------------------------------
            this.UpdateItemDescByDefineData(rdDefineData);

            //--------------------------------------------------------------------
            // (3) 由 Device Spec 的資訊，限制Test Item 描述資料的 Boundary
            //--------------------------------------------------------------------
            TesterSpecDescription.UpdateItemDescByDeviceSpec(this._descColl, info);
        }

        private void UpdateItemDescByDefineData(TestItemDescriptionCollections rdDefineData)
        {
            if (rdDefineData == null)
            {
                return;
            }

            double value;

            foreach (var item in rdDefineData.Items)
            {
                foreach (var prop in item.Property)
                {
                    ItemDescriptionBase defaultDescBase = this._descColl[item.KeyName][prop.PropertyKeyName];

                    // 若 CreateDefaultItemDesc() 未定義到 property description, 則不新增到現有資料結構中
                    if (defaultDescBase != null)
                    {
                        // check format
                        if(!double.TryParse(prop.Format, out value))
                        {
                            prop.Format = defaultDescBase.Format;
                        }

                        // Overwirte, rdData to defaultDesc
                        defaultDescBase.OverWrite(prop);
                    }
                }
            }
        }

        //private void UpdateItemDescByDeviceSpec(MachineInfoData info)
        //{
        //    string keyName = string.Empty;
            
        //    #region >>> SourceMeterSpec <<<

        //    if (info.SourceMeterSpec != null)
        //    {
        //        SourceMeterSpec srcSpec = info.SourceMeterSpec.Clone() as SourceMeterSpec;

        //        #region >>> Boundary Check <<<

        //        if (srcSpec.CurrentRange.Count != 0 && srcSpec.VoltageRange.Count != 0)
        //        {
        //            double minI = srcSpec.MinCurrentRange;  // unit : A
                    
        //            double maxI = srcSpec.MaxCurrentRange;  // unit : A

        //            double minV = srcSpec.MinVoltageRange;  // unit : V

        //            double maxV = srcSpec.MaxVoltageRange;  // unit : V

        //            if (minI != 0.0d && maxI != 0.0d && minV != 0.0d && maxV != 0.0d)
        //            {
        //                keyName = ETestType.IF.ToString();
        //                this._descColl[keyName][EItemDescription.ForceValue.ToString()].LimitBoundary(0.0d, maxI, EAmpUnit.A);
        //                this._descColl[keyName][EItemDescription.ForceTime.ToString()].LimitBoundary(0.0d, srcSpec.MaxForceTime);
        //                this._descColl[keyName][EItemDescription.MsrtRange.ToString()].LimitBoundary(minV, maxV, EVoltUnit.V);
        //                this._descColl[keyName][EItemDescription.MsrtClamp.ToString()].LimitBoundary(minV, maxV, EVoltUnit.V);

        //                keyName = ETestType.VF.ToString();
        //                this._descColl[keyName][EItemDescription.ForceValue.ToString()].LimitBoundary(0.0d, maxV, EVoltUnit.V);
        //                this._descColl[keyName][EItemDescription.ForceTime.ToString()].LimitBoundary(0.0d, srcSpec.MaxForceTime);
        //                this._descColl[keyName][EItemDescription.MsrtRange.ToString()].LimitBoundary(minI, maxI, EAmpUnit.A);
        //                this._descColl[keyName][EItemDescription.MsrtClamp.ToString()].LimitBoundary(minI, maxI, EAmpUnit.A);

        //                keyName = ETestType.IZ.ToString();
        //                this._descColl[keyName][EItemDescription.ForceValue.ToString()].LimitBoundary(0.0d, maxI, EAmpUnit.A);
        //                this._descColl[keyName][EItemDescription.ForceTime.ToString()].LimitBoundary(0.0d, srcSpec.MaxForceTime);
        //                this._descColl[keyName][EItemDescription.MsrtRange.ToString()].LimitBoundary(minV, maxV, EVoltUnit.V);
        //                this._descColl[keyName][EItemDescription.MsrtClamp.ToString()].LimitBoundary(minV, maxV, EVoltUnit.V);

        //                keyName = ETestType.VR.ToString();
        //                this._descColl[keyName][EItemDescription.ForceValue.ToString()].LimitBoundary(0.0d, maxV, EVoltUnit.V);
        //                this._descColl[keyName][EItemDescription.ForceTime.ToString()].LimitBoundary(0.0d, srcSpec.MaxForceTime);
        //                this._descColl[keyName][EItemDescription.MsrtRange.ToString()].LimitBoundary(minI, maxI, EAmpUnit.A);
        //                this._descColl[keyName][EItemDescription.MsrtClamp.ToString()].LimitBoundary(minI, maxI, EAmpUnit.A);

        //                keyName = ETestType.IFH.ToString();
        //                this._descColl[keyName][EItemDescription.ForceValue.ToString()].LimitBoundary(0.0d, maxI, EAmpUnit.A);
        //                this._descColl[keyName][EItemDescription.ForceTime.ToString()].LimitBoundary(0.0d, srcSpec.MaxForceTime);

        //                keyName = ETestType.LOP.ToString();
        //                this._descColl[keyName][EItemDescription.ForceValue.ToString()].LimitBoundary(0.0d, maxI, EAmpUnit.A);
        //                this._descColl[keyName][EItemDescription.ForceTime.ToString()].LimitBoundary(0.0d, srcSpec.MaxForceTime);
        //                this._descColl[keyName][EItemDescription.MsrtRange.ToString()].LimitBoundary(minV, maxV, EVoltUnit.V);
        //                this._descColl[keyName][EItemDescription.MsrtClamp.ToString()].LimitBoundary(minV, maxV, EVoltUnit.V);

        //                keyName = ETestType.LOPWL.ToString();
        //                this._descColl[keyName][EItemDescription.ForceValue.ToString()].LimitBoundary(0.0d, maxI, EAmpUnit.A);
        //                this._descColl[keyName][EItemDescription.ForceTime.ToString()].LimitBoundary(0.0d, srcSpec.MaxForceTime);
        //                this._descColl[keyName][EItemDescription.MsrtRange.ToString()].LimitBoundary(minV, maxV, EVoltUnit.V);
        //                this._descColl[keyName][EItemDescription.MsrtClamp.ToString()].LimitBoundary(minV, maxV, EVoltUnit.V);

        //                keyName = ETestType.IVSWEEP.ToString();
        //                this._descColl[keyName][EItemDescription.SweepStart.ToString()].LimitBoundary(-maxI, maxI, EAmpUnit.A);  // 零交越
        //                this._descColl[keyName][EItemDescription.SweepEnd.ToString()].LimitBoundary(-maxI, maxI, EAmpUnit.A);  // 零交越
        //                this._descColl[keyName][EItemDescription.ForceTime.ToString()].LimitBoundary(0.0d, srcSpec.MaxForceTime);
        //                this._descColl[keyName][EItemDescription.MsrtRange.ToString()].LimitBoundary(minV, maxV, EVoltUnit.V);
        //                this._descColl[keyName][EItemDescription.MsrtClamp.ToString()].LimitBoundary(minV, maxV, EVoltUnit.V);

        //                keyName = ETestType.VISWEEP.ToString();
        //                this._descColl[keyName][EItemDescription.SweepStart.ToString()].LimitBoundary(-maxV, maxV, EVoltUnit.V);
        //                this._descColl[keyName][EItemDescription.SweepEnd.ToString()].LimitBoundary(-maxV, maxV, EVoltUnit.V);
        //                this._descColl[keyName][EItemDescription.ForceTime.ToString()].LimitBoundary(0.0d, srcSpec.MaxForceTime);
        //                this._descColl[keyName][EItemDescription.MsrtRange.ToString()].LimitBoundary(minI, maxI, EAmpUnit.A);
        //                this._descColl[keyName][EItemDescription.MsrtClamp.ToString()].LimitBoundary(minI, maxI, EAmpUnit.A);

        //                keyName = ETestType.THY.ToString();
        //                this._descColl[keyName][EItemDescription.ForceValue.ToString()].LimitBoundary(0.0d, maxI, EAmpUnit.A);
        //                this._descColl[keyName][EItemDescription.ForceTime.ToString()].LimitBoundary(0.0d, srcSpec.MaxForceTime);
        //                this._descColl[keyName][EItemDescription.MsrtRange.ToString()].LimitBoundary(minV, maxV, EVoltUnit.V);
        //                this._descColl[keyName][EItemDescription.MsrtClamp.ToString()].LimitBoundary(minV, maxV, EVoltUnit.V);

        //                keyName = ETestType.POLAR.ToString();
        //                this._descColl[keyName][EItemDescription.ForceValue.ToString()].LimitBoundary(0.0d, maxI, EAmpUnit.A);
        //                this._descColl[keyName][EItemDescription.ForceTime.ToString()].LimitBoundary(0.0d, srcSpec.MaxForceTime);
        //                this._descColl[keyName][EItemDescription.MsrtRange.ToString()].LimitBoundary(minV, maxV, EVoltUnit.V);
        //                this._descColl[keyName][EItemDescription.MsrtClamp.ToString()].LimitBoundary(minV, maxV, EVoltUnit.V);

        //                keyName = ETestType.VAC.ToString();
        //                this._descColl[keyName][EItemDescription.ForceValue.ToString()].LimitBoundary(0.0d, maxV, EVoltUnit.V);
        //                this._descColl[keyName][EItemDescription.ForceTime.ToString()].LimitBoundary(0.0d, srcSpec.MaxForceTime);
        //                this._descColl[keyName][EItemDescription.MsrtRange.ToString()].LimitBoundary(minI, maxI, EAmpUnit.A);
        //                this._descColl[keyName][EItemDescription.MsrtClamp.ToString()].LimitBoundary(minI, maxI, EAmpUnit.A);

        //                keyName = ETestType.R.ToString();
        //                this._descColl[keyName][EItemDescription.ForceValue.ToString()].LimitBoundary(0.0d, maxI, EAmpUnit.A);
        //                this._descColl[keyName][EItemDescription.ForceTime.ToString()].LimitBoundary(0.0d, srcSpec.MaxForceTime);
        //                this._descColl[keyName][EItemDescription.MsrtRange.ToString()].LimitBoundary(minV, maxV, EVoltUnit.V);
        //                this._descColl[keyName][EItemDescription.MsrtClamp.ToString()].LimitBoundary(minV, maxV, EVoltUnit.V);

        //                keyName = ETestType.RTH.ToString();
        //                this._descColl[keyName][EItemDescription.ForceValue.ToString()].LimitBoundary(0.0d, maxI, EAmpUnit.A);
        //                this._descColl[keyName][EItemDescription.ForceTime.ToString()].LimitBoundary(0.0d, srcSpec.MaxForceTime);
        //                this._descColl[keyName][EItemDescription.MsrtRange.ToString()].LimitBoundary(minV, maxV, EVoltUnit.V);
        //                this._descColl[keyName][EItemDescription.MsrtClamp.ToString()].LimitBoundary(minV, maxV, EVoltUnit.V);

        //                keyName = ETestType.LIV.ToString();
        //                this._descColl[keyName][EItemDescription.SweepStart.ToString()].LimitBoundary(0.0d, maxI, EAmpUnit.A);
        //                this._descColl[keyName][EItemDescription.SweepStep.ToString()].LimitBoundary(0.0d, maxI, EAmpUnit.A);
        //                this._descColl[keyName][EItemDescription.SweepEnd.ToString()].LimitBoundary(0.0d, maxI, EAmpUnit.A);
        //                this._descColl[keyName][EItemDescription.ForceTime.ToString()].LimitBoundary(0.0d, srcSpec.MaxForceTime);
        //                this._descColl[keyName][EItemDescription.MsrtRange.ToString()].LimitBoundary(minV, maxV, EVoltUnit.V);
        //                this._descColl[keyName][EItemDescription.MsrtClamp.ToString()].LimitBoundary(minV, maxV, EVoltUnit.V);

        //                keyName = ETestType.PIV.ToString();
        //                this._descColl[keyName][EItemDescription.SweepStart.ToString()].LimitBoundary(0.0d, maxI, EAmpUnit.A);
        //                this._descColl[keyName][EItemDescription.SweepStep.ToString()].LimitBoundary(0.0d, maxI, EAmpUnit.A);
        //                this._descColl[keyName][EItemDescription.SweepEnd.ToString()].LimitBoundary(0.0d, maxI, EAmpUnit.A);
        //                this._descColl[keyName][EItemDescription.ForceTime.ToString()].LimitBoundary(0.0d, srcSpec.MaxForceTime);
        //                this._descColl[keyName][EItemDescription.MsrtRange.ToString()].LimitBoundary(minV, maxV, EVoltUnit.V);
        //                this._descColl[keyName][EItemDescription.MsrtClamp.ToString()].LimitBoundary(minV, maxV, EVoltUnit.V);
        //            }
        //        }

        //        #endregion

        //        #region >>> Property Supported/Visible Check <<<

        //        List<string> testTypeList = new List<string>(Enum.GetNames(typeof(ETestType)));

        //        string strType = string.Empty;

        //        foreach (var testType in testTypeList)
        //        {
        //            strType = testType.ToString();
                    
        //            if (this._descColl.ContainsKeyName(strType))
        //            {
        //                // IsAutoMsrtRange (MsrtRange 由 Clamp決定, 並隱藏MsrtRange顯示)
        //                if (this._descColl[strType].ContainsKeyName(EItemDescription.MsrtRange.ToString()) &&
        //                    this._descColl[strType].ContainsKeyName(EItemDescription.MsrtClamp.ToString()))
        //                {
        //                    this._descColl[strType][EItemDescription.MsrtRange.ToString()].IsEnable = true;
        //                    this._descColl[strType][EItemDescription.MsrtClamp.ToString()].IsEnable = true;

        //                    if (srcSpec.IsAutoMsrtRange)
        //                    {
        //                        this._descColl[strType][EItemDescription.MsrtRange.ToString()].IsVisible = false;
        //                        this._descColl[strType][EItemDescription.MsrtClamp.ToString()].IsVisible = true;
        //                    }
        //                    else
        //                    {
        //                        this._descColl[strType][EItemDescription.MsrtRange.ToString()].IsVisible = true;
        //                        this._descColl[strType][EItemDescription.MsrtClamp.ToString()].IsVisible = true;
        //                    }
        //                }
                        
        //                // NPLC Check
        //                if (this._descColl[strType].ContainsKeyName(EItemDescription.NPLC.ToString()))
        //                {
        //                    this._descColl[strType][EItemDescription.NPLC.ToString()].IsEnable = srcSpec.IsSupportedNPLC;
        //                }

        //                // MsrtFilter Check
        //                if (this._descColl[strType].ContainsKeyName(EItemDescription.FilterCount.ToString()))
        //                {
        //                    this._descColl[strType][EItemDescription.FilterCount.ToString()].IsEnable = srcSpec.IsSupportedMsrtFilter;
        //                }
        //            } 
        //        }

        //        #endregion
        //    }

        //    #endregion

        //    #region >>> SpectrometerSpec <<<

        //    if (info.SpectrometerSpec != null)
        //    {
        //        SpectrometerSpec sptSpec = info.SpectrometerSpec.Clone() as SpectrometerSpec;
        //    }

        //    #endregion

        //    #region >>> ESDSpec <<<

        //    if (info.ESDSpec != null)
        //    {
        //        ESDSpec esdSpec = info.ESDSpec.Clone() as ESDSpec;

        //        keyName = ETestType.ESD.ToString();

        //        this._descColl[keyName][EItemDescription.HBMVolt.ToString()].LimitBoundary(esdSpec.HBMVoltMinValue, esdSpec.HBMVoltMaxValue);
        //        this._descColl[keyName][EItemDescription.MMVolt.ToString()].LimitBoundary(esdSpec.MMVoltMinValue, esdSpec.MMVoltMaxValue);
        //        this._descColl[keyName][EItemDescription.ESDInterval.ToString()].LimitBoundary(esdSpec.ZapInvervalMinValue, esdSpec.ZapInvervalMaxValue);
        //        this._descColl[keyName][EItemDescription.ESDCount.ToString()].LimitBoundary(1.0d, esdSpec.ZapCountMaxValue);
        //    }

        //    #endregion
        //}

        private TestItemDescriptionCollections CreateAllDefaultItemsDescription()
        {
            List<string> testTypeList = new List<string>(Enum.GetNames(typeof(ETestType)));

            TestItemDescriptionCollections descColl = new TestItemDescriptionCollections();

            foreach (var testType in testTypeList)
            {
                descColl.Add(TesterSpecDescription.CreateDefaultItemDescription(testType.ToString()));
            }

            return descColl;
        }

        //private TestItemDescription CreateDefaultItemDescription(string keyName)
        //{
        //    TestItemDescription itemDescription = new TestItemDescription(keyName);
           
        //    ETestType testType = (ETestType)Enum.Parse(typeof(ETestType), keyName);

        //    switch (testType)
        //    {
        //        case ETestType.IF:
        //            {
        //                #region >>> IF <<<

        //                itemDescription.Add(new ItemDescriptionBase(EItemDescription.WaitTime.ToString(), 0.0d, 10000.0d, 0.0d, "ms", "0.0"));
        //                itemDescription.Add(new ItemDescriptionBase(EItemDescription.ForceValue.ToString(), 0.0d, 2000.0d, 0.0d, "mA", "0.000"));
        //                itemDescription.Add(new ItemDescriptionBase(EItemDescription.ForceTime.ToString(), 0.0d, 10000.0d, 1.0d, "ms", "0.0"));
        //                itemDescription.Add(new ItemDescriptionBase(EItemDescription.MsrtRange.ToString(), 2.0d, 100.0d, 8.0d, "V", "0.0"));
        //                itemDescription.Add(new ItemDescriptionBase(EItemDescription.MsrtClamp.ToString(), 2.0d, 100.0d, 8.0d, "V", "0.0"));

        //                itemDescription.Add(new ItemDescriptionBase(EItemDescription.FilterCount.ToString(), 0.0d, 20.0d, 5.0d, "cnt", "0"));
        //                itemDescription.Add(new ItemDescriptionBase(EItemDescription.NPLC.ToString(), 0.01d, 10.0d, 0.01d, "", "0.00"));

        //                #endregion

        //                break;
        //            }
        //        //-------------------------------------------------------------------------------------------------------------------------------------
        //        case ETestType.IZ:
        //            {
        //                #region >>> IZ <<<

        //                itemDescription.Add(new ItemDescriptionBase(EItemDescription.WaitTime.ToString(), 0.0d, 10000.0d, 0.0d, "ms", "0.0"));
        //                itemDescription.Add(new ItemDescriptionBase(EItemDescription.ForceValue.ToString(), 0.0d, 1000.0d, 0.0d, "uA", "0.0"));
        //                itemDescription.Add(new ItemDescriptionBase(EItemDescription.ForceTime.ToString(), 0.0d, 10000.0d, 1.0d, "ms", "0.0"));
        //                itemDescription.Add(new ItemDescriptionBase(EItemDescription.MsrtRange.ToString(), 10.0d, 100.0d, 40.0d, "V", "0.0"));
        //                itemDescription.Add(new ItemDescriptionBase(EItemDescription.MsrtClamp.ToString(), 10.0d, 100.0d, 40.0d, "V", "0.0"));

        //                itemDescription.Add(new ItemDescriptionBase(EItemDescription.FilterCount.ToString(), 0.0d, 20.0d, 5.0d, "cnt", "0"));
        //                itemDescription.Add(new ItemDescriptionBase(EItemDescription.NPLC.ToString(), 0.01d, 10.0d, 0.01d, "", "0.00"));

        //                #endregion

        //                break;
        //            }
        //        //-------------------------------------------------------------------------------------------------------------------------------------
        //        case ETestType.VF:
        //            {
        //                #region >>> VF <<<

        //                itemDescription.Add(new ItemDescriptionBase(EItemDescription.WaitTime.ToString(), 0.0d, 10000.0d, 0.0d, "ms", "0.0"));
        //                itemDescription.Add(new ItemDescriptionBase(EItemDescription.ForceValue.ToString(), 0.0d, 100.0d, 0.0d, "V", "0.0"));
        //                itemDescription.Add(new ItemDescriptionBase(EItemDescription.ForceTime.ToString(), 0.0d, 10000.0d, 1.0d, "ms", "0.0"));
        //                itemDescription.Add(new ItemDescriptionBase(EItemDescription.MsrtRange.ToString(), 0.0001d, 1000.0d, 100.0d, "mA", "0.0000"));
        //                itemDescription.Add(new ItemDescriptionBase(EItemDescription.MsrtClamp.ToString(), 0.0001d, 1000.0d, 100.0d, "mA", "0.0000"));

        //                itemDescription.Add(new ItemDescriptionBase(EItemDescription.FilterCount.ToString(), 0.0d, 20.0d, 5.0d, "cnt", "0"));
        //                itemDescription.Add(new ItemDescriptionBase(EItemDescription.NPLC.ToString(), 0.01d, 10.0d, 0.01d, "", "0.00"));

        //                #endregion

        //                break;
        //            }
        //        //-------------------------------------------------------------------------------------------------------------------------------------
        //        case ETestType.VR:
        //            {
        //                #region >>> VR <<<

        //                itemDescription.Add(new ItemDescriptionBase(EItemDescription.WaitTime.ToString(), 0.0d, 10000.0d, 0.0d, "ms", "0.0"));
        //                itemDescription.Add(new ItemDescriptionBase(EItemDescription.ForceValue.ToString(), 0.0d, 20.0d, 0.0d, "V", "0.0"));
        //                itemDescription.Add(new ItemDescriptionBase(EItemDescription.ForceTime.ToString(), 0.0d, 10000.0d, 1.0d, "ms", "0.0"));
        //                itemDescription.Add(new ItemDescriptionBase(EItemDescription.MsrtRange.ToString(), 0.1d, 100.0d, 4.0d, "uA", "0.0"));
        //                itemDescription.Add(new ItemDescriptionBase(EItemDescription.MsrtClamp.ToString(), 0.1d, 100.0d, 4.0d, "uA", "0.0"));

        //                itemDescription.Add(new ItemDescriptionBase(EItemDescription.FilterCount.ToString(), 0.0d, 20.0d, 5.0d, "cnt", "0"));
        //                itemDescription.Add(new ItemDescriptionBase(EItemDescription.NPLC.ToString(), 0.01d, 10.0d, 0.01d, "", "0.00"));

        //                #endregion

        //                break;
        //            }
        //        //-------------------------------------------------------------------------------------------------------------------------------------
        //        case ETestType.IFH:
        //            {
        //                #region >>> IFH <<<

        //                itemDescription.Add(new ItemDescriptionBase(EItemDescription.WaitTime.ToString(), 0.0d, 10000.0d, 0.0d, "ms", "0.0"));
        //                itemDescription.Add(new ItemDescriptionBase(EItemDescription.ForceValue.ToString(), 1.0d, 2000.0d, 0.0d, "mA", "0.000"));
        //                itemDescription.Add(new ItemDescriptionBase(EItemDescription.ForceTime.ToString(), 0.0d, 10000.0d, 1.0d, "ms", "0.0"));

        //                #endregion

        //                break;
        //            }
        //        //-------------------------------------------------------------------------------------------------------------------------------------
        //        case ETestType.DVF:
        //            {
        //                #region >>> DVF <<<

        //                itemDescription.Add(new ItemDescriptionBase(EItemDescription.WaitTime.ToString(), 0.0d, 10000.0d, 0.0d, "ms", "0.0"));
        //                itemDescription.Add(new ItemDescriptionBase(EItemDescription.ForceValue.ToString(), 0.0d, 2000.0d, 0.0d, "mA", "0.000"));
        //                itemDescription.Add(new ItemDescriptionBase(EItemDescription.ForceTime.ToString(), 0.0d, 10000.0d, 1.0d, "ms", "0.0"));
        //                itemDescription.Add(new ItemDescriptionBase(EItemDescription.MsrtRange.ToString(), 2.0d, 100.0d, 8.0d, "V", "0.0"));
        //                itemDescription.Add(new ItemDescriptionBase(EItemDescription.MsrtClamp.ToString(), 2.0d, 100.0d, 8.0d, "V", "0.0"));

        //                itemDescription.Add(new ItemDescriptionBase(EItemDescription.FilterCount.ToString(), 0.0d, 20.0d, 5.0d, "cnt", "0"));
        //                itemDescription.Add(new ItemDescriptionBase(EItemDescription.NPLC.ToString(), 0.01d, 10.0d, 0.01d, "", "0.00"));

        //                #endregion

        //                break;
        //            }
        //        //-------------------------------------------------------------------------------------------------------------------------------------
        //        case ETestType.LOP:
        //            {
        //                #region >>> LOP <<<

        //                itemDescription.Add(new ItemDescriptionBase(EItemDescription.WaitTime.ToString(), 0.0d, 10000.0d, 0.0d, "ms", "0.0"));
        //                itemDescription.Add(new ItemDescriptionBase(EItemDescription.ForceValue.ToString(), 0.0d, 2000.0d, 0.0d, "mA", "0.000"));
        //                itemDescription.Add(new ItemDescriptionBase(EItemDescription.ForceTime.ToString(), 0.0d, 10000.0d, 1.0d, "ms", "0.0"));
        //                itemDescription.Add(new ItemDescriptionBase(EItemDescription.MsrtRange.ToString(), 2.0d, 100.0d, 8.0d, "V", "0.0"));
        //                itemDescription.Add(new ItemDescriptionBase(EItemDescription.MsrtClamp.ToString(), 2.0d, 100.0d, 8.0d, "V", "0.0"));

        //                itemDescription.Add(new ItemDescriptionBase(EItemDescription.FilterCount.ToString(), 0.0d, 20.0d, 5.0d, "cnt", "0"));
        //                itemDescription.Add(new ItemDescriptionBase(EItemDescription.NPLC.ToString(), 0.01d, 10.0d, 0.01d, "", "0.00"));

        //                itemDescription.Add(new ItemDescriptionBase(EItemDescription.DetectorMsrtRange.ToString(), 0.001d, 100.0d, 1.0d, "mA", "0.000"));
        //                #endregion

        //                break;
        //            }
        //        //-------------------------------------------------------------------------------------------------------------------------------------
        //        case ETestType.LOPWL:
        //            {
        //                #region >>> LOPWL <<<

        //                itemDescription.Add(new ItemDescriptionBase(EItemDescription.WaitTime.ToString(), 0.0d, 10000.0d, 0.0d, "ms", "0.0"));
        //                itemDescription.Add(new ItemDescriptionBase(EItemDescription.ForceValue.ToString(), 0.0d, 2000.0d, 0.0d, "mA", "0.000"));
        //                itemDescription.Add(new ItemDescriptionBase(EItemDescription.ForceTime.ToString(), 0.0d, 10000.0d, 1.0d, "ms", "0.0"));
        //                itemDescription.Add(new ItemDescriptionBase(EItemDescription.MsrtRange.ToString(), 2.0d, 100.0d, 8.0d, "V", "0.0"));
        //                itemDescription.Add(new ItemDescriptionBase(EItemDescription.MsrtClamp.ToString(), 2.0d, 100.0d, 8.0d, "V", "0.0"));

        //                itemDescription.Add(new ItemDescriptionBase(EItemDescription.FilterCount.ToString(), 0.0d, 20.0d, 5.0d, "cnt", "0"));
        //                itemDescription.Add(new ItemDescriptionBase(EItemDescription.NPLC.ToString(), 0.01d, 10.0d, 0.01d, "", "0.00"));

        //                itemDescription.Add(new ItemDescriptionBase(EItemDescription.IntTimeFix.ToString(), 0.0d, 500.0d, 10.0d, "ms", "0.0"));
        //                itemDescription.Add(new ItemDescriptionBase(EItemDescription.IntTimeLimit.ToString(), 0.0d, 500.0d, 10.0d, "ms", "0.0"));

        //                #endregion

        //                break;
        //            }
        //        //-------------------------------------------------------------------------------------------------------------------------------------
        //        case ETestType.IVSWEEP:
        //            {
        //                #region >>> IVSWEEP <<<

        //                itemDescription.Add(new ItemDescriptionBase(EItemDescription.WaitTime.ToString(), 0.0d, 10000.0d, 0.0d, "ms", "0.0"));
        //                itemDescription.Add(new ItemDescriptionBase(EItemDescription.SweepStart.ToString(), -2000.0d, 2000.0d, 0.0d, "mA", "0.0000")); // 零交越 Sweep
        //                itemDescription.Add(new ItemDescriptionBase(EItemDescription.SweepEnd.ToString(), -2000.0d, 2000.0d, 0.0d, "mA", "0.0000"));

        //                itemDescription.Add(new ItemDescriptionBase(EItemDescription.SweepTurnOffTime.ToString(), 0.0d, 10000.0d, 0.0d, "ms", "0.0"));

        //                itemDescription.Add(new ItemDescriptionBase(EItemDescription.SweepRiseCount.ToString(), 2.0d, 2000.0d, 2.0d, "cnt", "0"));

        //                itemDescription.Add(new ItemDescriptionBase(EItemDescription.ForceTime.ToString(), 0.0d, 10000.0d, 1.0d, "ms", "0.0"));
        //                itemDescription.Add(new ItemDescriptionBase(EItemDescription.MsrtRange.ToString(), 2.0d, 100.0d, 8.0d, "V", "0.0"));
        //                itemDescription.Add(new ItemDescriptionBase(EItemDescription.MsrtClamp.ToString(), 2.0d, 100.0d, 8.0d, "V", "0.0"));

        //                itemDescription.Add(new ItemDescriptionBase(EItemDescription.FilterCount.ToString(), 0.0d, 20.0d, 5.0d, "cnt", "0"));
        //                itemDescription.Add(new ItemDescriptionBase(EItemDescription.NPLC.ToString(), 0.01d, 10.0d, 0.01d, "", "0.00"));

        //                #endregion

        //                break;
        //            }
        //        //-------------------------------------------------------------------------------------------------------------------------------------
        //        case ETestType.VISWEEP:
        //            {
        //                #region >>> VISWEEP <<<

        //                itemDescription.Add(new ItemDescriptionBase(EItemDescription.WaitTime.ToString(), 0.0d, 10000.0d, 0.0d, "ms", "0.0"));
        //                itemDescription.Add(new ItemDescriptionBase(EItemDescription.SweepStart.ToString(), -100.0d, 100.0d, 0.0d, "V", "0.0")); // 零交越 Sweep
        //                itemDescription.Add(new ItemDescriptionBase(EItemDescription.SweepEnd.ToString(), -100.0d, 100.0d, 0.0d, "V", "0.0"));

        //                itemDescription.Add(new ItemDescriptionBase(EItemDescription.SweepTurnOffTime.ToString(), 0.0d, 10000.0d, 0.0d, "ms", "0.0"));

        //                itemDescription.Add(new ItemDescriptionBase(EItemDescription.SweepRiseCount.ToString(), 2.0d, 2000.0d, 2.0d, "cnt", "0"));

        //                itemDescription.Add(new ItemDescriptionBase(EItemDescription.ForceTime.ToString(), 0.0d, 10000.0d, 1.0d, "ms", "0.0"));
        //                itemDescription.Add(new ItemDescriptionBase(EItemDescription.MsrtRange.ToString(), 0.001d, 1000.0d, 100.0d, "mA", "0.000"));
        //                itemDescription.Add(new ItemDescriptionBase(EItemDescription.MsrtClamp.ToString(), 0.001d, 1000.0d, 100.0d, "mA", "0.000"));

        //                itemDescription.Add(new ItemDescriptionBase(EItemDescription.FilterCount.ToString(), 0.0d, 20.0d, 5.0d, "cnt", "0"));
        //                itemDescription.Add(new ItemDescriptionBase(EItemDescription.NPLC.ToString(), 0.01d, 10.0d, 0.01d, "", "0.00"));

        //                #endregion

        //                break;
        //            }
        //        //-------------------------------------------------------------------------------------------------------------------------------------
        //        case ETestType.THY:
        //            {
        //                #region >>> THY <<<

        //                itemDescription.Add(new ItemDescriptionBase(EItemDescription.WaitTime.ToString(), 0.0d, 10000.0d, 0.0d, "ms", "0.0"));
        //                itemDescription.Add(new ItemDescriptionBase(EItemDescription.ForceValue.ToString(), 0.0d, 100.0d, 0.0d, "mA", "0.000"));
        //                itemDescription.Add(new ItemDescriptionBase(EItemDescription.ForceTime.ToString(), 0.0d, 10000.0d, 1.0d, "ms", "0.0"));
        //                itemDescription.Add(new ItemDescriptionBase(EItemDescription.MsrtRange.ToString(), 2.0d, 100.0d, 8.0d, "V", "0.0"));
        //                itemDescription.Add(new ItemDescriptionBase(EItemDescription.MsrtClamp.ToString(), 2.0d, 100.0d, 8.0d, "V", "0.0"));

        //                itemDescription.Add(new ItemDescriptionBase(EItemDescription.SweepFlatCount.ToString(), 0.0d, 1200.0d, 100.0d, "cnt", "0"));
        //                itemDescription.Add(new ItemDescriptionBase(EItemDescription.FilterCount.ToString(), 0.0d, 20.0d, 5.0d, "cnt", "0"));
        //                itemDescription.Add(new ItemDescriptionBase(EItemDescription.NPLC.ToString(), 0.003d, 10.0d, 0.003d, "", "0.000"));

        //                itemDescription.Add(new ItemDescriptionBase(EItemDescription.SGFilterCount.ToString(), 0.0d, 20.0d, 0.0d, "cnt", "0"));
        //                itemDescription.Add(new ItemDescriptionBase(EItemDescription.MovingAvgWindowSize.ToString(), 0.0d, 10.0d, 5.0d, "cnt", "0"));

        //                #endregion

        //                break;
        //            }
        //        //-------------------------------------------------------------------------------------------------------------------------------------
        //        case ETestType.CALC:
        //            {
        //                #region >>> CALC <<<

        //                #endregion

        //                break;
        //            }
        //        //-------------------------------------------------------------------------------------------------------------------------------------
        //        case ETestType.DIB:
        //            {
        //                #region >>> DIB <<<

        //                #endregion

        //                break;
        //            }
        //        //-------------------------------------------------------------------------------------------------------------------------------------
        //        case ETestType.POLAR:
        //            {
        //                #region >>> POLAR <<<

        //                itemDescription.Add(new ItemDescriptionBase(EItemDescription.WaitTime.ToString(), 0.0d, 10000.0d, 0.0d, "ms", "0.0"));
        //                itemDescription.Add(new ItemDescriptionBase(EItemDescription.ForceValue.ToString(), 0.0d, 2000.0d, 0.0d, "mA", "0.000"));
        //                itemDescription.Add(new ItemDescriptionBase(EItemDescription.ForceTime.ToString(), 0.0d, 10000.0d, 1.0d, "ms", "0.0"));
        //                itemDescription.Add(new ItemDescriptionBase(EItemDescription.MsrtRange.ToString(), 2.0d, 100.0d, 8.0d, "V", "0.0"));
        //                itemDescription.Add(new ItemDescriptionBase(EItemDescription.MsrtClamp.ToString(), 2.0d, 100.0d, 8.0d, "V", "0.0"));

        //                itemDescription.Add(new ItemDescriptionBase(EItemDescription.FilterCount.ToString(), 0.0d, 20.0d, 5.0d, "cnt", "0"));
        //                itemDescription.Add(new ItemDescriptionBase(EItemDescription.NPLC.ToString(), 0.01d, 10.0d, 0.01d, "", "0.00"));

        //                #endregion

        //                break;
        //            }
        //        //-------------------------------------------------------------------------------------------------------------------------------------
        //        case ETestType.VAC:
        //            {
        //                #region >>> VAC <<<

        //                itemDescription.Add(new ItemDescriptionBase(EItemDescription.WaitTime.ToString(), 0.0d, 10000.0d, 0.0d, "ms", "0.0"));
        //                itemDescription.Add(new ItemDescriptionBase(EItemDescription.ForceValue.ToString(), 0.0d, 300.0d, 0.0d, "V", "0.0"));
        //                itemDescription.Add(new ItemDescriptionBase(EItemDescription.ForceTime.ToString(), 0.0d, 10000.0d, 1.0d, "ms", "0.0"));
        //                itemDescription.Add(new ItemDescriptionBase(EItemDescription.MsrtRange.ToString(), 1.0d, 6000.0d, 20.0d, "mA", "0.0"));
        //                itemDescription.Add(new ItemDescriptionBase(EItemDescription.MsrtClamp.ToString(), 1.0d, 6000.0d, 20.0d, "mA", "0.0"));

        //                itemDescription.Add(new ItemDescriptionBase(EItemDescription.FilterCount.ToString(), 0.0d, 20.0d, 5.0d, "cnt", "0"));

        //                itemDescription.Add(new ItemDescriptionBase(EItemDescription.ACFrequency.ToString(), 45.0d, 500.0d, 60.0d, "Hz", "0.0"));

        //                #endregion

        //                break;
        //            }
        //        //-------------------------------------------------------------------------------------------------------------------------------------
        //        case ETestType.R:
        //            {
        //                #region >>> R <<<

        //                itemDescription.Add(new ItemDescriptionBase(EItemDescription.WaitTime.ToString(), 0.0d, 10000.0d, 0.0d, "ms", "0.0"));
        //                itemDescription.Add(new ItemDescriptionBase(EItemDescription.ForceValue.ToString(), 0.0d, 100.0d, 0.0d, "mA", "0.000"));
        //                itemDescription.Add(new ItemDescriptionBase(EItemDescription.ForceTime.ToString(), 0.0d, 10000.0d, 1.0d, "ms", "0.0"));
        //                itemDescription.Add(new ItemDescriptionBase(EItemDescription.MsrtRange.ToString(), 2.0d, 100.0d, 8.0d, "V", "0.0"));
        //                itemDescription.Add(new ItemDescriptionBase(EItemDescription.MsrtClamp.ToString(), 2.0d, 100.0d, 8.0d, "V", "0.0"));

        //                itemDescription.Add(new ItemDescriptionBase(EItemDescription.FilterCount.ToString(), 0.0d, 20.0d, 5.0d, "cnt", "0"));
        //                itemDescription.Add(new ItemDescriptionBase(EItemDescription.NPLC.ToString(), 0.01d, 10.0d, 0.01d, "", "0.00"));

        //                #endregion

        //                break;
        //            }
        //        //-------------------------------------------------------------------------------------------------------------------------------------
        //        case ETestType.RTH:
        //            {
        //                #region >>> RTH <<<

        //                itemDescription.Add(new ItemDescriptionBase(EItemDescription.WaitTime.ToString(), 0.0d, 10000.0d, 0.0d, "ms", "0.0"));
        //                itemDescription.Add(new ItemDescriptionBase(EItemDescription.ForceValue.ToString(), 0.0d, 100.0d, 0.0d, "mA", "0.000"));
        //                itemDescription.Add(new ItemDescriptionBase(EItemDescription.ForceTime.ToString(), 0.0d, 10000.0d, 1.0d, "ms", "0.0"));
        //                itemDescription.Add(new ItemDescriptionBase(EItemDescription.MsrtRange.ToString(), 2.0d, 100.0d, 8.0d, "V", "0.0"));
        //                itemDescription.Add(new ItemDescriptionBase(EItemDescription.MsrtClamp.ToString(), 2.0d, 100.0d, 8.0d, "V", "0.0"));

        //                itemDescription.Add(new ItemDescriptionBase(EItemDescription.FilterCount.ToString(), 0.0d, 20.0d, 5.0d, "cnt", "0"));
        //                itemDescription.Add(new ItemDescriptionBase(EItemDescription.NPLC.ToString(), 0.01d, 10.0d, 0.01d, "", "0.00"));

        //                #endregion

        //                break;
        //            }
        //        //-------------------------------------------------------------------------------------------------------------------------------------
        //        case ETestType.LIV:
        //            {
        //                #region >>> LIV <<<

        //                itemDescription.Add(new ItemDescriptionBase(EItemDescription.WaitTime.ToString(), 0.0d, 10000.0d, 0.0d, "ms", "0.0"));
        //                itemDescription.Add(new ItemDescriptionBase(EItemDescription.SweepStart.ToString(), 0.0d, 2000.0d, 0.0d, "mA", "0.000"));
        //                itemDescription.Add(new ItemDescriptionBase(EItemDescription.SweepStep.ToString(), 0.0d, 2000.0d, 0.0d, "mA", "0.000"));
        //                itemDescription.Add(new ItemDescriptionBase(EItemDescription.SweepEnd.ToString(), 0.0d, 2000.0d, 0.0d, "mA", "0.000"));

        //                itemDescription.Add(new ItemDescriptionBase(EItemDescription.SweepTurnOffTime.ToString(), 0.0d, 10000.0d, 1.0d, "ms", "0.0"));

        //                itemDescription.Add(new ItemDescriptionBase(EItemDescription.SweepRiseCount.ToString(), 0.0d, 1000.0d, 1.0d, "cnt", "0"));
        //                itemDescription.Add(new ItemDescriptionBase(EItemDescription.SweepFlatCount.ToString(), 0.0d, 1000.0d, 1.0d, "cnt", "0"));

        //                itemDescription.Add(new ItemDescriptionBase(EItemDescription.ForceTime.ToString(), 0.0d, 10000.0d, 1.0d, "ms", "0.0"));
        //                itemDescription.Add(new ItemDescriptionBase(EItemDescription.MsrtRange.ToString(), 2.0d, 100.0d, 8.0d, "V", "0.0"));
        //                itemDescription.Add(new ItemDescriptionBase(EItemDescription.MsrtClamp.ToString(), 2.0d, 100.0d, 8.0d, "V", "0.0"));

        //                itemDescription.Add(new ItemDescriptionBase(EItemDescription.FilterCount.ToString(), 0.0d, 20.0d, 5.0d, "cnt", "0"));
        //                itemDescription.Add(new ItemDescriptionBase(EItemDescription.NPLC.ToString(), 0.01d, 10.0d, 0.01d, "", "0.00"));

        //                itemDescription.Add(new ItemDescriptionBase(EItemDescription.IntTimeFix.ToString(), 0.0d, 500.0d, 10.0d, "ms", "0.0"));
        //                itemDescription.Add(new ItemDescriptionBase(EItemDescription.IntTimeLimit.ToString(), 0.0d, 500.0d, 10.0d, "ms", "0.0"));

        //                itemDescription.Add(new ItemDescriptionBase(EItemDescription.DetectorMsrtRange.ToString(), 0.001d, 100.0d, 1.0d, "mA", "0.000"));

        //                #endregion

        //                break;
        //            }
        //        //-------------------------------------------------------------------------------------------------------------------------------------
        //        case ETestType.VLR:
        //            {
        //                #region >>> VLR <<<

        //                #endregion

        //                break;
        //            }
        //        //-------------------------------------------------------------------------------------------------------------------------------------
        //        case ETestType.ESD:
        //            {
        //                #region >>> ESD <<<

        //                itemDescription.Add(new ItemDescriptionBase(EItemDescription.HBMVolt.ToString(), 100.0d, 4000.0d, 100.0d, "V", "0"));
        //                itemDescription.Add(new ItemDescriptionBase(EItemDescription.MMVolt.ToString(), 50.0d, 500.0d, 50.0d, "V", "0"));
        //                itemDescription.Add(new ItemDescriptionBase(EItemDescription.ESDInterval.ToString(), 0.0d, 1000.0d, 0.0d, "ms", "0.0"));
        //                itemDescription.Add(new ItemDescriptionBase(EItemDescription.ESDCount.ToString(), 1.0d, 99.0d, 1.0d, "cnt", "0"));

        //                #endregion

        //                break;
        //            }
        //        //-------------------------------------------------------------------------------------------------------------------------------------
        //        case ETestType.VISCAN:
        //            {
        //                #region >>> VISCAN <<<

        //                itemDescription.Add(new ItemDescriptionBase(EItemDescription.WaitTime.ToString(), 0.0d, 10000.0d, 0.0d, "ms", "0.0"));
        //                itemDescription.Add(new ItemDescriptionBase(EItemDescription.ForceValue.ToString(), -100.0d, 100.0d, 0.0d, "V", "0.0")); // 零交越 Sweep

        //                itemDescription.Add(new ItemDescriptionBase(EItemDescription.SweepFlatCount.ToString(), 2.0d, 5000.0d, 2.0d, "cnt", "0"));

        //                itemDescription.Add(new ItemDescriptionBase(EItemDescription.ForceTime.ToString(), 0.0d, 10000.0d, 1.0d, "ms", "0.0"));
        //                itemDescription.Add(new ItemDescriptionBase(EItemDescription.MsrtRange.ToString(), 0.001d, 1000.0d, 100.0d, "uA", "0.000"));
        //                itemDescription.Add(new ItemDescriptionBase(EItemDescription.MsrtClamp.ToString(), 0.001d, 1000.0d, 100.0d, "uA", "0.000"));

        //                itemDescription.Add(new ItemDescriptionBase(EItemDescription.FilterCount.ToString(), 0.0d, 20.0d, 5.0d, "cnt", "0"));
        //                itemDescription.Add(new ItemDescriptionBase(EItemDescription.NPLC.ToString(), 0.01d, 10.0d, 0.01d, "", "0.00"));

        //                #endregion

        //                break;
        //            }
        //        //-------------------------------------------------------------------------------------------------------------------------------------
        //        case ETestType.PIV:
        //            {
        //                #region >>> PIV <<<

        //                itemDescription.Add(new ItemDescriptionBase(EItemDescription.WaitTime.ToString(),   0.0d, 10000.0d, 0.0d, "ms", "0.0"));
        //                itemDescription.Add(new ItemDescriptionBase(EItemDescription.SweepStart.ToString(), 0.0d, 2000.0d, 0.0d, "mA", "0.00")); // 零交越 Sweep
        //                itemDescription.Add(new ItemDescriptionBase(EItemDescription.SweepEnd.ToString(),   0.01d, 2000.0d, 0.0d, "mA", "0.00"));
        //                itemDescription.Add(new ItemDescriptionBase(EItemDescription.SweepStep.ToString(),  0.01d, 2000.0d, 0.5d, "mA", "0.00"));

        //                itemDescription.Add(new ItemDescriptionBase(EItemDescription.SweepTurnOffTime.ToString(), 0.0d, 10000.0d, 0.0d, "ms", "0.0"));

        //                itemDescription.Add(new ItemDescriptionBase(EItemDescription.ForceTime.ToString(), 0.0d, 10000.0d, 1.0d, "ms", "0.0"));
        //                itemDescription.Add(new ItemDescriptionBase(EItemDescription.MsrtRange.ToString(), 2.0d, 100.0d, 8.0d, "V", "0.0"));
        //                itemDescription.Add(new ItemDescriptionBase(EItemDescription.MsrtClamp.ToString(), 2.0d, 100.0d, 8.0d, "V", "0.0"));

        //                itemDescription.Add(new ItemDescriptionBase(EItemDescription.FilterCount.ToString(), 0.0d, 20.0d, 5.0d, "cnt", "0"));
        //                itemDescription.Add(new ItemDescriptionBase(EItemDescription.NPLC.ToString(),       0.01d, 10.0d, 0.01d, "", "0.000"));

        //                itemDescription.Add(new ItemDescriptionBase(EItemDescription.DetectorMsrtRange.ToString(), 0.001d, 100.0d, 1.0d, "mA", "0.000"));
        //                itemDescription.Add(new ItemDescriptionBase(EItemDescription.DetectorBiasValue.ToString(), -20.0d, 20.0d, 0.0d, "V", "0.0"));

        //                // Method
        //                itemDescription.Add(new ItemDescriptionBase(EItemDescription.OperationMethod.ToString(), 0.0, 1.0d, 0.0d, "", "0"));
        //                itemDescription.Add(new ItemDescriptionBase(EItemDescription.SeMethod.ToString(),        0.0, 1.0d, 0.0d, "", "0"));
        //                itemDescription.Add(new ItemDescriptionBase(EItemDescription.RsMethod.ToString(),        0.0, 1.0d, 0.0d, "", "0"));
        //                #endregion

        //                break;
        //            }
        //        //-------------------------------------------------------------------------------------------------------------------------------------
        //        case ETestType.TRANSISTOR:
        //            {
        //                #region >>> Transistor <<<

        //                #endregion

        //                break;
        //            }
        //        default:
        //            break;
        //    }

        //    return itemDescription;
        //}

        #endregion

        #region >>> Public Method <<<

        public void SetSupportedItems(MachineConfig config, TestItemDescriptionCollections rdDefineData, ETesterConfigType configType)
        {
            this._configType = configType;
            
            //----------------------------------------------------------------------------------------------------------
            // (1) 決定 Tester 能顯示的 Supported Test Items  (RDFunc Enable List & Device supoort Items 兩者交集)
            //----------------------------------------------------------------------------------------------------------
            this.UpdateDataToSupportedItems(config, rdDefineData);

            //----------------------------------------------------------------------------------------------------------
            // (2) 由 _lstSupportedItems 與 儀器資訊 ModelName = None 決定能啟動的測試條件 _lstEnableItems
            //----------------------------------------------------------------------------------------------------------
            this._lstEnableItems = TesterSpecDescription.GetTestItemEnableList(this._lstSupportedItems, config);
        }

        public void SetSpecBoundary(MachineInfoData info, TestItemDescriptionCollections rdDefineData)
        {
            //----------------------------------------------------------------------------------------------------------
            // (1) 從 Kernel 初始化後 回傳 Machine Info 的 Device Spec, 決定各 Test Item 的 Boundary
            //----------------------------------------------------------------------------------------------------------
            this.UpdateDataByMachineInfo(info, rdDefineData);
        }

        //public void SetSpecDescription(MachineConfig config, MachineInfoData info, TestItemDescriptionCollections rdDefineData)
        //{
        //    //----------------------------------------------------------------------------------------------------------
        //    // (1) 決定 Tester 能顯示的 Supported Test Items  (RDFunc Enable List & Device supoort Items 兩者交集)
        //    //----------------------------------------------------------------------------------------------------------
        //    this.UpdateDataToSupportedItems(config, rdDefineData);

        //    //----------------------------------------------------------------------------------------------------------
        //    // (2) 移除硬體 ModelName = None 所對應的 Enable Test Items
        //    //----------------------------------------------------------------------------------------------------------
        //    foreach (var strType in this._lstSupportedItems)
        //    {
        //        ETestType testType = (ETestType)Enum.Parse(typeof(ETestType), strType);

        //        switch (testType)
        //        {
        //            case ETestType.ESD:
        //                {
        //                    if (config.ESDModel == EESDModel.NONE)
        //                    {
        //                        continue;
        //                    }

        //                    break;
        //                }
        //            //--------------------------------------------------------------------------------------------
        //            case ETestType.LOP:
        //            case ETestType.PIV:
        //                {
        //                    if (config.PDSensingMode == EPDSensingMode.NONE)
        //                    {
        //                        continue;
        //                    }

        //                    break;
        //                }
        //            //--------------------------------------------------------------------------------------------
        //            case ETestType.LOPWL:
        //            case ETestType.LIV:
        //                {
        //                    if (config.SpectrometerModel == ESpectrometerModel.NONE)
        //                    {
        //                        continue;
        //                    }

        //                    break;
        //                }
        //            //--------------------------------------------------------------------------------------------
        //            default:
        //                {
        //                    if (config.SourceMeterModel == ESourceMeterModel.NONE)
        //                    {
        //                        continue;
        //                    }

        //                    break;
        //                }
        //        }

        //        this._lstEnableItems.Add(strType);
        //    }
            
        //    //----------------------------------------------------------------------------------------------------------
        //    // (3) 從 Kernel 初始化後 回傳 Machine Info 的 Device Spec, 決定各 Test Item 的 Boundary
        //    //----------------------------------------------------------------------------------------------------------
        //    this.UpdateDataByMachineInfo(info, rdDefineData);
        //}

        public bool CheckTestItemTypeEnable(string type)
        {
            if (this._lstEnableItems.Count != 0)
            {
                return this._lstEnableItems.Contains(type);
            }

            return false;
        }

        public TestItemDescription GetDefaultDescription(string keyName)
        {
            return TesterSpecDescription.CreateDefaultItemDescription(keyName);
        }

        public string[] GetDefaultItems(ETesterConfigType type)
        {
            List<string> lstTypes = new List<string>();

            ETestType[] testTypes;

            switch (type)
            {
                case ETesterConfigType.LDTester:
                    {
                        testTypes = TesterSpecDescription.TESTER_LD_DEFAULT_ITEMS;
                        break;
                    }
                case ETesterConfigType.PDTester:
                    {
                        testTypes = TesterSpecDescription.TESTER_PD_DEFAULT_ITEMS;
                        break;
                    }
                default:
                    {
                        testTypes = TesterSpecDescription.TESTER_LED_DEFAULT_ITEMS;
                        break;
                    }
            }

            foreach (var eType in testTypes)
            {
                lstTypes.Add(eType.ToString());
            }

            return lstTypes.ToArray();
        }

        #endregion
    }

    /// <summary>
    /// 若有新增之Test Item, 請編輯此物件
    /// </summary>
    internal class TesterSpecDescription
    {
        public static ETestType[] TESTER_LED_DEFAULT_ITEMS = new ETestType[] { ETestType.IF,    ETestType.VF,   ETestType.IZ,   ETestType.VR, ETestType.LOPWL,
                                                                           ETestType.IFH, ETestType.CALC, ETestType.THY };

        public static ETestType[] TESTER_LD_DEFAULT_ITEMS  = new ETestType[] { ETestType.IF,    ETestType.VF,   ETestType.IZ,   ETestType.VR, ETestType.LOP,
                                                                               ETestType.LOPWL, ETestType.PIV };

        public static ETestType[] TESTER_PD_DEFAULT_ITEMS  = new ETestType[] { ETestType.IF,    ETestType.VF,   ETestType.IZ,   ETestType.VR, ETestType.VISWEEP };

        /// <summary>
        /// 各儀器(SMU, SPT, ESD)可支援的測試條件
        /// </summary>
        public static List<string> GetDeviceSupportItmeList(MachineConfig config)
        {
            List<string> deviceSupportItem = new List<string>();

            switch (config.SourceMeterModel)
            {
                case ESourceMeterModel.LDT1A:
                case ESourceMeterModel.T2001L:
                case ESourceMeterModel.LDT3A200:
                    {
                        deviceSupportItem.Add(ETestType.IF.ToString());
                        deviceSupportItem.Add(ETestType.VF.ToString());
                        deviceSupportItem.Add(ETestType.VR.ToString());
                        deviceSupportItem.Add(ETestType.IZ.ToString());
                        deviceSupportItem.Add(ETestType.IFH.ToString());
                        deviceSupportItem.Add(ETestType.DVF.ToString());
                        deviceSupportItem.Add(ETestType.LOP.ToString());
                        deviceSupportItem.Add(ETestType.LOPWL.ToString());
                        deviceSupportItem.Add(ETestType.THY.ToString());
                        deviceSupportItem.Add(ETestType.CALC.ToString());
                        deviceSupportItem.Add(ETestType.POLAR.ToString());
                        deviceSupportItem.Add(ETestType.IVSWEEP.ToString());
                        deviceSupportItem.Add(ETestType.VISWEEP.ToString());
                        deviceSupportItem.Add(ETestType.DIB.ToString());

                        deviceSupportItem.Add(ETestType.ESD.ToString());
                        break;
                    }
                case ESourceMeterModel.N5700:
                case ESourceMeterModel.DR2000:
                case ESourceMeterModel.DSPHD:
                    {
                        deviceSupportItem.Add(ETestType.IF.ToString());
                        deviceSupportItem.Add(ETestType.VF.ToString());
                        deviceSupportItem.Add(ETestType.LOPWL.ToString());

                        break;
                    }
                case ESourceMeterModel.K2400:
                    {
                        deviceSupportItem.Add(ETestType.IF.ToString());
                        deviceSupportItem.Add(ETestType.VF.ToString());
                        deviceSupportItem.Add(ETestType.VR.ToString());
                        deviceSupportItem.Add(ETestType.IZ.ToString());
                        deviceSupportItem.Add(ETestType.LOPWL.ToString());
                        deviceSupportItem.Add(ETestType.POLAR.ToString());
                        deviceSupportItem.Add(ETestType.CALC.ToString());

                        break;
                    }
                case ESourceMeterModel.Persona:
                case ESourceMeterModel.K2600:
                    {
                        deviceSupportItem.Add(ETestType.IF.ToString());
                        deviceSupportItem.Add(ETestType.VF.ToString());
                        deviceSupportItem.Add(ETestType.VR.ToString());
                        deviceSupportItem.Add(ETestType.IZ.ToString());
                        deviceSupportItem.Add(ETestType.IFH.ToString());
                        deviceSupportItem.Add(ETestType.DVF.ToString());
                        deviceSupportItem.Add(ETestType.LOP.ToString());
                        deviceSupportItem.Add(ETestType.LOPWL.ToString());
                        deviceSupportItem.Add(ETestType.THY.ToString());
                        deviceSupportItem.Add(ETestType.CALC.ToString());
                        deviceSupportItem.Add(ETestType.POLAR.ToString());
                        deviceSupportItem.Add(ETestType.IVSWEEP.ToString());
                        deviceSupportItem.Add(ETestType.VISWEEP.ToString());
                        deviceSupportItem.Add(ETestType.R.ToString());
						deviceSupportItem.Add(ETestType.ContactCheck.ToString());
                        deviceSupportItem.Add(ETestType.RTH.ToString());
                        deviceSupportItem.Add(ETestType.LIV.ToString());
                        deviceSupportItem.Add(ETestType.VLR.ToString());
                        deviceSupportItem.Add(ETestType.DIB.ToString());

                        deviceSupportItem.Add(ETestType.VISCAN.ToString());
                        deviceSupportItem.Add(ETestType.PIV.ToString());
                        deviceSupportItem.Add(ETestType.ESD.ToString());
                        deviceSupportItem.Add(ETestType.TRANSISTOR.ToString());

                        deviceSupportItem.Add(ETestType.OSA.ToString());
                        deviceSupportItem.Add(ETestType.IO.ToString());
                        break;
                    }
                case ESourceMeterModel.IT7321:
                    {
                        deviceSupportItem.Add(ETestType.VAC.ToString());
                        deviceSupportItem.Add(ETestType.LOPWL.ToString());
                        deviceSupportItem.Add(ETestType.CALC.ToString());

                        break;
                    }
				case ESourceMeterModel.RM3542:
					{
						deviceSupportItem.Add(ETestType.R.ToString());

						break;
					}
                case ESourceMeterModel.B2900A:
                    {
                        deviceSupportItem.Add(ETestType.IF.ToString());
                        deviceSupportItem.Add(ETestType.VF.ToString());
                        deviceSupportItem.Add(ETestType.VR.ToString());
                        deviceSupportItem.Add(ETestType.IZ.ToString());
                        deviceSupportItem.Add(ETestType.LOP.ToString());
                        deviceSupportItem.Add(ETestType.LOPWL.ToString());
                        deviceSupportItem.Add(ETestType.PIV.ToString());
                        deviceSupportItem.Add(ETestType.OSA.ToString());

                        break;
                    }
                case ESourceMeterModel.K2520:
                case ESourceMeterModel.SS400:
                    {
                        deviceSupportItem.Add(ETestType.IF.ToString());
                        deviceSupportItem.Add(ETestType.LOP.ToString());
                        deviceSupportItem.Add(ETestType.LOPWL.ToString());
                        deviceSupportItem.Add(ETestType.PIV.ToString());
                        deviceSupportItem.Add(ETestType.VR.ToString());
                        deviceSupportItem.Add(ETestType.IZ.ToString());
                        break;
                    }
                default:
                    {
                        deviceSupportItem.Add(ETestType.IF.ToString());
                        deviceSupportItem.Add(ETestType.VF.ToString());
                        deviceSupportItem.Add(ETestType.VR.ToString());
                        deviceSupportItem.Add(ETestType.IZ.ToString());
                        deviceSupportItem.Add(ETestType.LOPWL.ToString());
                        break;
                    }
            }

            if (config.TesterFunctionType == ETesterFunctionType.Multi_Die)
            {
                if (deviceSupportItem.Contains(ETestType.DIB.ToString()))
                {
                    deviceSupportItem.Remove(ETestType.DIB.ToString());
                }

                if (deviceSupportItem.Contains(ETestType.LIV.ToString()))
                {
                    deviceSupportItem.Remove(ETestType.LIV.ToString());
                }

                if (deviceSupportItem.Contains(ETestType.R.ToString()))
                {
                    deviceSupportItem.Remove(ETestType.R.ToString());
                }

				if (deviceSupportItem.Contains(ETestType.ContactCheck.ToString()))
				{
					deviceSupportItem.Remove(ETestType.ContactCheck.ToString());
				}

                if (deviceSupportItem.Contains(ETestType.POLAR.ToString()))
                {
                    deviceSupportItem.Remove(ETestType.POLAR.ToString());
                }

                if (deviceSupportItem.Contains(ETestType.PIV.ToString()))
                {
                    deviceSupportItem.Remove(ETestType.PIV.ToString());
                }
                if (deviceSupportItem.Contains(ETestType.IVSWEEP.ToString()))
                {
                    deviceSupportItem.Remove(ETestType.IVSWEEP.ToString());
                }
                if (deviceSupportItem.Contains(ETestType.VISWEEP.ToString()))
                {
                    deviceSupportItem.Remove(ETestType.VISWEEP.ToString());
                }
                if (deviceSupportItem.Contains(ETestType.LCR.ToString()))
                {
                    deviceSupportItem.Remove(ETestType.LCR.ToString());
                }
                if (deviceSupportItem.Contains(ETestType.LCRSWEEP.ToString()))
                {
                    deviceSupportItem.Remove(ETestType.LCRSWEEP.ToString());
                }

            }
            else if (config.TesterFunctionType == ETesterFunctionType.Multi_Terminal)
            {
                if (deviceSupportItem.Contains(ETestType.TRANSISTOR.ToString()))
                {
                    deviceSupportItem.Clear();

                    deviceSupportItem.Add(ETestType.TRANSISTOR.ToString());
                }
                else
                {
                    deviceSupportItem.Clear();
                }
            }

            //config.TesterFunctionType == ETesterFunctionType.Single_Die
            if (config.LCRModel != ELCRModel.NONE && config.TesterFunctionType == ETesterFunctionType.Single_Die)
            {
                deviceSupportItem.Add(ETestType.LCR.ToString());

                deviceSupportItem.Add(ETestType.LCRSWEEP.ToString());
 
            }

            if (config.LaserSrcSysConfig != null && config.LaserSrcSysConfig.ChConfigList != null
                && config.LaserSrcSysConfig.ChConfigList.Count > 0)
            {
                deviceSupportItem.Add(ETestType.LaserSource.ToString());
            }
            return deviceSupportItem;
        }

        public static List<string> GetTestItemEnableList(List<string> supportItemList, MachineConfig config)
        {
            List<string> enableTestItemList = new List<string>();

            foreach (var strType in supportItemList)
            {
                ETestType testType = (ETestType)Enum.Parse(typeof(ETestType), strType);

                switch (testType)
                {
                    case ETestType.ESD:
                        {
                            if (config.ESDModel == EESDModel.NONE)
                            {
                                continue;
                            }

                            break;
                        }
                    //--------------------------------------------------------------------------------------------
                    case ETestType.LOP:
                    case ETestType.PIV:
                        {
                            if (config.PDSensingMode == EPDSensingMode.NONE)
                            {
                                continue;
                            }

                            break;
                        }
                    //--------------------------------------------------------------------------------------------
                    case ETestType.LOPWL:         
                        {
                            if (config.SpectrometerModel == ESpectrometerModel.NONE)
                            {
                                continue;
                            }

                            break;
                        }
                    //--------------------------------------------------------------------------------------------
                    case ETestType.LCR:
                        {
                            if (config.LCRModel == ELCRModel.NONE)
                            {
                                continue;
                            }

                            break;
                        }
                    //--------------------------------------------------------------------------------------------
                    case ETestType.LCRSWEEP:
                        {
                            if (config.LCRModel == ELCRModel.NONE)
                            {
                                continue;
                            }

                            break;
                        }
                    //--------------------------------------------------------------------------------------------
                    default:
                        {
                            if (config.SourceMeterModel == ESourceMeterModel.NONE)
                            {
                                continue;
                            }

                            break;
                        }
                }

                enableTestItemList.Add(strType);
            }

            return enableTestItemList;
        }

        /// <summary>
        /// 測試條件 item Description 的 Default設定 
        /// </summary>
        public static TestItemDescription CreateDefaultItemDescription(string keyName)
        {
            TestItemDescription itemDescription = new TestItemDescription(keyName);
           
            ETestType testType = (ETestType)Enum.Parse(typeof(ETestType), keyName);

            switch (testType)
            {
                case ETestType.IF:
                    {
                        #region >>> IF <<<

                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.WaitTime.ToString(), 0.0d, 10000.0d, 0.0d, "ms", "0.0"));
                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.ForceValue.ToString(), 0.0d, 2000.0d, 0.0d, "mA", "0.000"));
                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.ForceTime.ToString(), 0.0d, 10000.0d, 1.0d, "ms", "0.0"));
                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.MsrtRange.ToString(), 2.0d, 100.0d, 8.0d, "V", "0.0"));
                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.MsrtClamp.ToString(), 2.0d, 100.0d, 8.0d, "V", "0.0"));

                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.FilterCount.ToString(), 0.0d, 20.0d, 5.0d, "cnt", "0"));
                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.NPLC.ToString(), 0.01d, 10.0d, 0.01d, "", "0.00"));

                        #endregion

                        break;
                    }
                //-------------------------------------------------------------------------------------------------------------------------------------
                case ETestType.IZ:
                    {
                        #region >>> IZ <<<

                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.WaitTime.ToString(), 0.0d, 10000.0d, 0.0d, "ms", "0.0"));
                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.ForceValue.ToString(), 0.0d, 1000.0d, 0.0d, "uA", "0.0"));
                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.ForceTime.ToString(), 0.0d, 10000.0d, 1.0d, "ms", "0.0"));
                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.MsrtRange.ToString(), 10.0d, 100.0d, 40.0d, "V", "0.0"));
                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.MsrtClamp.ToString(), 10.0d, 100.0d, 40.0d, "V", "0.0"));

                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.FilterCount.ToString(), 0.0d, 20.0d, 5.0d, "cnt", "0"));
                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.NPLC.ToString(), 0.01d, 10.0d, 0.01d, "", "0.00"));

                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.EnableFloatForceValue.ToString(), 0.0d, 1.0d, 0.0d, "", "0"));  // bool
                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.FloatFactor.ToString(), 0.01d, 2.00d, 1.00d, "", "0.00"));  // bool
                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.FloatOffset.ToString(), 0.00d, 20.00d, 0.00d, "", "0.00"));  // bool
                        #endregion

                        break;
                    }
                //-------------------------------------------------------------------------------------------------------------------------------------
                case ETestType.VF:
                    {
                        #region >>> VF <<<

                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.WaitTime.ToString(), 0.0d, 10000.0d, 0.0d, "ms", "0.0"));
                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.ForceValue.ToString(), 0.0d, 100.0d, 0.0d, "V", "0.0"));
                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.ForceTime.ToString(), 0.0d, 10000.0d, 1.0d, "ms", "0.0"));
                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.MsrtRange.ToString(), 0.0001d, 1000.0d, 100.0d, "mA", "0.0000"));
                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.MsrtClamp.ToString(), 0.0001d, 1000.0d, 100.0d, "mA", "0.0000"));

                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.FilterCount.ToString(), 0.0d, 20.0d, 5.0d, "cnt", "0"));
                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.NPLC.ToString(), 0.01d, 10.0d, 0.01d, "", "0.00"));

                        #endregion

                        break;
                    }
                //-------------------------------------------------------------------------------------------------------------------------------------
                case ETestType.VR:
                    {
                        #region >>> VR <<<

                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.WaitTime.ToString(), 0.0d, 10000.0d, 0.0d, "ms", "0.0"));
                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.ForceValue.ToString(), 0.0d, 20.0d, 0.0d, "V", "0.0"));
                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.ForceTime.ToString(), 0.0d, 10000.0d, 1.0d, "ms", "0.0"));
                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.MsrtRange.ToString(), 0.1d, 100.0d, 4.0d, "uA", "0.0"));
                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.MsrtClamp.ToString(), 0.1d, 100.0d, 4.0d, "uA", "0.0"));

                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.FilterCount.ToString(), 0.0d, 20.0d, 1.0d, "cnt", "0"));
                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.NPLC.ToString(), 0.01d, 10.0d, 0.01d, "", "0.00"));

                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.EnableFloatForceValue.ToString(), 0.0d, 1.0d, 0.0d, "", "0"));  // bool
                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.FloatFactor.ToString(), 0.01d, 2.00d, 1.00d, "", "0.00"));  // bool
                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.FloatOffset.ToString(), 0.00d, 20.00d, 0.00d, "", "0.00"));  // bool

                        #endregion

                        break;
                    }
                //-------------------------------------------------------------------------------------------------------------------------------------
                case ETestType.IFH:
                    {
                        #region >>> IFH <<<

                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.WaitTime.ToString(), 0.0d, 10000.0d, 0.0d, "ms", "0.0"));
                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.ForceValue.ToString(), 1.0d, 2000.0d, 0.0d, "mA", "0.000"));
                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.ForceTime.ToString(), 0.0d, 10000.0d, 1.0d, "ms", "0.0"));

                        #endregion

                        break;
                    }
                //-------------------------------------------------------------------------------------------------------------------------------------
                case ETestType.DVF:
                    {
                        #region >>> DVF <<<

                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.WaitTime.ToString(), 0.0d, 10000.0d, 0.0d, "ms", "0.0"));
                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.ForceValue.ToString(), 0.0d, 2000.0d, 0.0d, "mA", "0.000"));
                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.ForceTime.ToString(), 0.0d, 10000.0d, 1.0d, "ms", "0.0"));
                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.MsrtRange.ToString(), 2.0d, 100.0d, 8.0d, "V", "0.0"));
                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.MsrtClamp.ToString(), 2.0d, 100.0d, 8.0d, "V", "0.0"));

                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.FilterCount.ToString(), 0.0d, 20.0d, 5.0d, "cnt", "0"));
                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.NPLC.ToString(), 0.01d, 10.0d, 0.01d, "", "0.00"));

                        #endregion

                        break;
                    }
                //-------------------------------------------------------------------------------------------------------------------------------------
                case ETestType.LOP:
                    {
                        #region >>> LOP <<<

                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.WaitTime.ToString(), 0.0d, 10000.0d, 0.0d, "ms", "0.0"));
                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.ForceValue.ToString(), 0.0d, 2000.0d, 0.0d, "mA", "0.000"));
                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.ForceTime.ToString(), 0.0d, 10000.0d, 1.0d, "ms", "0.0"));
                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.MsrtRange.ToString(), 2.0d, 100.0d, 8.0d, "V", "0.0"));
                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.MsrtClamp.ToString(), 2.0d, 100.0d, 8.0d, "V", "0.0"));

                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.FilterCount.ToString(), 0.0d, 20.0d, 5.0d, "cnt", "0"));
                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.NPLC.ToString(), 0.01d, 10.0d, 0.01d, "", "0.00"));

                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.DetectorMsrtRange.ToString(), 0.001d, 100.0d, 1.0d, "mA", "0.000"));

                        // Pulse Mode
                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.IsPulseMode.ToString(), 0, 1, 0, "", "0"));
                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.PulseValue.ToString(), 1500.0d, 10000.0d, 0.0d, "mA", "0.00"));
                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.PulseWidth.ToString(), 0.5d, 1.0d, 0.5d, "ms", "0.0"));
                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.PulseDuty.ToString(), 0.1d, 2.2d, 1.0d, "%", "0.0"));
                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.PulseMsrtRange.ToString(), 2.0d, 6.0d, 5.0d, "V", "0.0"));
                        #endregion

                        break;
                    }
                //-------------------------------------------------------------------------------------------------------------------------------------
                case ETestType.LOPWL:
                    {
                        #region >>> LOPWL <<<

                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.WaitTime.ToString(), 0.0d, 10000.0d, 0.0d, "ms", "0.0"));
                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.ForceValue.ToString(), 0.0d, 2000.0d, 0.0d, "mA", "0.000"));
                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.ForceTime.ToString(), 0.0d, 10000.0d, 1.0d, "ms", "0.0"));
                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.MsrtRange.ToString(), 2.0d, 100.0d, 8.0d, "V", "0.0"));
                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.MsrtClamp.ToString(), 2.0d, 100.0d, 8.0d, "V", "0.0"));

                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.FilterCount.ToString(), 0.0d, 20.0d, 5.0d, "cnt", "0"));
                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.NPLC.ToString(), 0.01d, 10.0d, 0.01d, "", "0.00"));

                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.IntTimeFix.ToString(), 0.0d, 500.0d, 10.0d, "ms", "0.0"));
                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.IntTimeLimit.ToString(), 0.0d, 500.0d, 10.0d, "ms", "0.0"));

                        // Pulse Mode
                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.IsPulseMode.ToString(), 0, 1, 0, "", "0"));
                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.PulseValue.ToString(), 1500.0d, 10000.0d, 0.0d, "mA", "0.00"));
                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.PulseWidth.ToString(), 0.5d, 1.0d, 0.5d, "ms", "0.0"));
                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.PulseDuty.ToString(), 0.1d, 2.2d, 1.0d, "%", "0.0"));
                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.PulseMsrtRange.ToString(), 2.0d, 6.0d, 5.0d, "V", "0.0"));

                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.EnableFloatForceValue.ToString(), 0.0d, 1.0d, 0.0d, "", "0"));  // bool
                        #endregion

                        break;
                    }
                //-------------------------------------------------------------------------------------------------------------------------------------
                case ETestType.IVSWEEP:
                    {
                        #region >>> IVSWEEP <<<

                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.WaitTime.ToString(), 0.0d, 10000.0d, 0.0d, "ms", "0.0"));
                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.SweepStart.ToString(), -2000.0d, 2000.0d, 0.0d, "mA", "0.0000")); // 零交越 Sweep
                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.SweepEnd.ToString(), -2000.0d, 2000.0d, 0.0d, "mA", "0.0000"));

                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.SweepTurnOffTime.ToString(), 0.0d, 10000.0d, 0.0d, "ms", "0.0"));

                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.SweepRiseCount.ToString(), 2.0d, 2000.0d, 2.0d, "cnt", "0"));

                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.ForceTime.ToString(), 0.0d, 10000.0d, 1.0d, "ms", "0.0"));
                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.MsrtRange.ToString(), 2.0d, 100.0d, 8.0d, "V", "0.0"));
                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.MsrtClamp.ToString(), 2.0d, 100.0d, 8.0d, "V", "0.0"));

                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.FilterCount.ToString(), 0.0d, 20.0d, 5.0d, "cnt", "0"));
                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.NPLC.ToString(), 0.01d, 10.0d, 0.01d, "", "0.00"));
                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.SweepAdvanceMode.ToString(), 0, 1, 0, "", "0"));

                        #endregion

                        break;
                    }
                //-------------------------------------------------------------------------------------------------------------------------------------
                case ETestType.VISWEEP:
                    {
                        #region >>> VISWEEP <<<

                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.WaitTime.ToString(), 0.0d, 10000.0d, 0.0d, "ms", "0.0"));
                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.SweepStart.ToString(), -100.0d, 100.0d, 0.0d, "V", "0.0")); // 零交越 Sweep
                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.SweepEnd.ToString(), -100.0d, 100.0d, 0.0d, "V", "0.0"));

                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.SweepTurnOffTime.ToString(), 0.0d, 10000.0d, 0.0d, "ms", "0.0"));

                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.SweepRiseCount.ToString(), 2.0d, 2000.0d, 2.0d, "cnt", "0"));

                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.ForceTime.ToString(), 0.0d, 10000.0d, 1.0d, "ms", "0.0"));
                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.MsrtRange.ToString(), 0.001d, 100000.0d, 100.0d, "uA", "0.0"));
                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.MsrtClamp.ToString(), 0.001d, 100000.0d, 100.0d, "uA", "0.0"));

                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.FilterCount.ToString(), 0.0d, 20.0d, 5.0d, "cnt", "0"));
                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.NPLC.ToString(), 0.01d, 10.0d, 0.01d, "", "0.00"));
                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.SweepAdvanceMode.ToString(), 0, 1, 0, "", "0"));
                        #endregion

                        break;
                    }
                //-------------------------------------------------------------------------------------------------------------------------------------
                case ETestType.THY:
                    {
                        #region >>> THY <<<

                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.WaitTime.ToString(), 0.0d, 10000.0d, 0.0d, "ms", "0.0"));
                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.ForceValue.ToString(), 0.0d, 100.0d, 0.0d, "mA", "0.000"));
                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.ForceTime.ToString(), 0.0d, 10000.0d, 1.0d, "ms", "0.0"));
                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.MsrtRange.ToString(), 2.0d, 100.0d, 8.0d, "V", "0.0"));
                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.MsrtClamp.ToString(), 2.0d, 100.0d, 8.0d, "V", "0.0"));

                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.SweepFlatCount.ToString(), 0.0d, 1200.0d, 100.0d, "cnt", "0"));
                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.FilterCount.ToString(), 0.0d, 20.0d, 5.0d, "cnt", "0"));
                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.NPLC.ToString(), 0.003d, 10.0d, 0.003d, "", "0.000"));

                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.SGFilterCount.ToString(), 0.0d, 20.0d, 0.0d, "cnt", "0"));
                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.MovingAvgWindowSize.ToString(), 0.0d, 10.0d, 5.0d, "cnt", "0"));

                        #endregion

                        break;
                    }
                //-------------------------------------------------------------------------------------------------------------------------------------
                case ETestType.CALC:
                    {
                        #region >>> CALC <<<
                        {
                            itemDescription.Add(new IOItemDescription(EItemDescription.CALC_GAIN.ToString(), 0.0d, 999d, 0.0d, "", "0"));
                            itemDescription.Add(new IOItemDescription(EItemDescription.CALC_ADV.ToString(), 0.0d, 999d, 0.0d, "", "0"));
                        }
                        #endregion

                        break;
                    }
                //-------------------------------------------------------------------------------------------------------------------------------------
                case ETestType.DIB:
                    {
                        #region >>> DIB <<<

                        #endregion

                        break;
                    }
                //-------------------------------------------------------------------------------------------------------------------------------------
                case ETestType.POLAR:
                    {
                        #region >>> POLAR <<<

                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.WaitTime.ToString(), 0.0d, 10000.0d, 0.0d, "ms", "0.0"));
                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.ForceValue.ToString(), 0.0d, 2000.0d, 0.0d, "mA", "0.000"));
                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.ForceTime.ToString(), 0.0d, 10000.0d, 1.0d, "ms", "0.0"));
                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.MsrtRange.ToString(), 2.0d, 100.0d, 8.0d, "V", "0.0"));
                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.MsrtClamp.ToString(), 2.0d, 100.0d, 8.0d, "V", "0.0"));

                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.FilterCount.ToString(), 0.0d, 20.0d, 5.0d, "cnt", "0"));
                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.NPLC.ToString(), 0.01d, 10.0d, 0.01d, "", "0.00"));

                        #endregion

                        break;
                    }
                //-------------------------------------------------------------------------------------------------------------------------------------
                case ETestType.VAC:
                    {
                        #region >>> VAC <<<

                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.WaitTime.ToString(), 0.0d, 10000.0d, 0.0d, "ms", "0.0"));
                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.ForceValue.ToString(), 0.0d, 300.0d, 0.0d, "V", "0.0"));
                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.ForceTime.ToString(), 0.0d, 10000.0d, 1.0d, "ms", "0.0"));
                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.MsrtRange.ToString(), 1.0d, 6000.0d, 20.0d, "mA", "0.0"));
                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.MsrtClamp.ToString(), 1.0d, 6000.0d, 20.0d, "mA", "0.0"));

                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.FilterCount.ToString(), 0.0d, 20.0d, 5.0d, "cnt", "0"));

                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.ACFrequency.ToString(), 45.0d, 500.0d, 60.0d, "Hz", "0.0"));

                        #endregion

                        break;
                    }
                //-------------------------------------------------------------------------------------------------------------------------------------
                case ETestType.R:
                    {
                        #region >>> R <<<

						itemDescription.Add(new ItemDescriptionBase(EItemDescription.WaitTime.ToString(), 0.0d, 10000.0d, 0.0d, "ms", "0.0"));
                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.RTestItemRange.ToString(), 0.1d, 100e6, 0.0d, "Ohm", "0.0"));

                        #endregion

                        break;
                    }
                //-------------------------------------------------------------------------------------------------------------------------------------
				case ETestType.ContactCheck:
					{
						#region >>> R <<<

						#endregion

						break;
					}
				//-------------------------------------------------------------------------------------------------------------------------------------
				case ETestType.RTH:
                    {
                        #region >>> RTH <<<

                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.WaitTime.ToString(), 0.0d, 10000.0d, 0.0d, "ms", "0.0"));
                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.ForceValue.ToString(), 0.0d, 100.0d, 0.0d, "mA", "0.000"));
                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.ForceTime.ToString(), 0.0d, 10000.0d, 1.0d, "ms", "0.0"));
                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.MsrtRange.ToString(), 2.0d, 100.0d, 8.0d, "V", "0.0"));
                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.MsrtClamp.ToString(), 2.0d, 100.0d, 8.0d, "V", "0.0"));

                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.FilterCount.ToString(), 0.0d, 20.0d, 5.0d, "cnt", "0"));
                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.NPLC.ToString(), 0.01d, 10.0d, 0.01d, "", "0.00"));

                        #endregion

                        break;
                    }
                //-------------------------------------------------------------------------------------------------------------------------------------
                case ETestType.LIV:
                    {
                        #region >>> LIV <<<

                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.WaitTime.ToString(), 0.0d, 10000.0d, 0.0d, "ms", "0.0"));
                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.SweepStart.ToString(), 0.0d, 2000.0d, 0.0d, "mA", "0.000"));            
                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.SweepEnd.ToString(), 0.0d, 2000.0d, 0.0d, "mA", "0.000"));

                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.SweepTurnOffTime.ToString(), 0.0d, 10000.0d, 0.0d, "ms", "0.0"));

                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.SweepRiseCount.ToString(), 2.0d, 500.0d, 2.0d, "cnt", "0"));

                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.ForceTime.ToString(), 0.0d, 10000.0d, 1.0d, "ms", "0.0"));
                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.MsrtRange.ToString(), 2.0d, 100.0d, 8.0d, "V", "0.0"));
                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.MsrtClamp.ToString(), 2.0d, 100.0d, 8.0d, "V", "0.0"));

                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.FilterCount.ToString(), 0.0d, 20.0d, 5.0d, "cnt", "0"));
                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.NPLC.ToString(), 0.01d, 10.0d, 0.01d, "", "0.00"));

                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.IntTimeFix.ToString(), 0.0d, 500.0d, 10.0d, "ms", "0.0"));
                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.IntTimeLimit.ToString(), 0.0d, 500.0d, 10.0d, "ms", "0.0"));

                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.DetectorMsrtRange.ToString(), 0.001d, 100.0d, 1.0d, "mA", "0.000"));

                        #endregion

                        break;
                    }
                //-------------------------------------------------------------------------------------------------------------------------------------
                case ETestType.VLR:
                    {
                        #region >>> VLR <<<

                        #endregion

                        break;
                    }
                //-------------------------------------------------------------------------------------------------------------------------------------
                case ETestType.ESD:
                    {
                        #region >>> ESD <<<

                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.HBMVolt.ToString(), 100.0d, 4000.0d, 100.0d, "V", "0"));
                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.MMVolt.ToString(), 50.0d, 500.0d, 50.0d, "V", "0"));
                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.ESDInterval.ToString(), 0.0d, 1000.0d, 0.0d, "ms", "0.0"));
                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.ESDCount.ToString(), 1.0d, 99.0d, 1.0d, "cnt", "0"));

                        #endregion

                        break;
                    }
                //-------------------------------------------------------------------------------------------------------------------------------------
                case ETestType.VISCAN:
                    {
                        #region >>> VISCAN <<<

                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.WaitTime.ToString(), 0.0d, 10000.0d, 0.0d, "ms", "0.0"));
                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.ForceValue.ToString(), -100.0d, 100.0d, 0.0d, "V", "0.0")); // 零交越 Sweep

                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.SweepFlatCount.ToString(), 2.0d, 5000.0d, 2.0d, "cnt", "0"));

                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.ForceTime.ToString(), 0.0d, 10000.0d, 1.0d, "ms", "0.0"));
                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.MsrtRange.ToString(), 0.001d, 1000.0d, 100.0d, "uA", "0.0000"));
                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.MsrtClamp.ToString(), 0.001d, 1000.0d, 100.0d, "uA", "0.0000"));

                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.FilterCount.ToString(), 0.0d, 20.0d, 5.0d, "cnt", "0"));
                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.NPLC.ToString(), 0.01d, 10.0d, 0.01d, "", "0.00"));

                        #endregion

                        break;
                    }
                //-------------------------------------------------------------------------------------------------------------------------------------
                case ETestType.PIV:
                    {
                        #region >>> PIV <<<

                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.WaitTime.ToString(), 0.0d, 10000.0d, 0.0d, "ms", "0.0"));
                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.SweepStart.ToString(), 0.0d, 2000.0d, 0.0d, "mA", "0.00")); // 零交越 Sweep
                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.SweepEnd.ToString(), 0.01d, 2000.0d, 0.0d, "mA", "0.00"));
                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.SweepStep.ToString(), 0.01d, 2000.0d, 0.5d, "mA", "0.00"));

                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.SweepTurnOffTime.ToString(), 0.0d, 10000.0d, 0.0d, "ms", "0.0"));

                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.ForceTime.ToString(), 0.0d, 10000.0d, 1.0d, "ms", "0.0"));
                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.MsrtRange.ToString(), 2.0d, 100.0d, 8.0d, "V", "0.0"));
                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.MsrtClamp.ToString(), 2.0d, 100.0d, 8.0d, "V", "0.0"));

                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.FilterCount.ToString(), 0.0d, 20.0d, 5.0d, "cnt", "0"));
                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.NPLC.ToString(), 0.01d, 10.0d, 0.01d, "", "0.000"));

                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.DetectorMsrtRange.ToString(), 0.001d, 100.0d, 1.0d, "mA", "0.000"));
                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.DetectorBiasValue.ToString(), -20.0d, 20.0d, 0.0d, "V", "0.0"));

                        // Method
                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.OperationMethod.ToString(), 0.0, 4.0d, 0.0d, "", "0"));
                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.SeMethod.ToString(), 0.0, 4.0d, 0.0d, "", "0"));
                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.RsMethod.ToString(), 0.0, 4.0d, 0.0d, "", "0"));
                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.ThresholdMethod.ToString(), 0.0, 4.0d, 0.0d, "", "0"));
                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.KinkMethod.ToString(), 0.0, 4.0d, 0.0d, "", "0"));
                        // Pulse Mode
                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.IsPulseMode.ToString(), 0, 1, 0, "", "0"));
                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.PulseValue.ToString(), 1500.0d, 10000.0d, 0.0d, "mA", "0.00"));
                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.PulseWidth.ToString(), 0.5d, 1.0d, 0.5d, "ms", "0.0"));
                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.PulseDuty.ToString(), 0.1d, 2.2d, 1.0d, "%", "0.0"));
                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.PulseMsrtRange.ToString(), 2.0d, 6.0d, 5.0d, "V", "0.0"));

                        #endregion

                        break;
                    }
                //-------------------------------------------------------------------------------------------------------------------------------------
                case ETestType.TRANSISTOR:
                    {
                        #region >>> Transistor <<<

                        #endregion

                        break;
                    }
                //-------------------------------------------------------------------------------------------------------------------------------------
                case ETestType.OSA:
                    {
                        #region >>> LOPWL <<<

                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.WaitTime.ToString(), 0.0d, 10000.0d, 0.0d, "ms", "0.0"));
                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.ForceValue.ToString(), 0.0d, 2000.0d, 0.0d, "mA", "0.000"));
                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.ForceTime.ToString(), 0.0d, 10000.0d, 1.0d, "ms", "0.0"));
                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.MsrtRange.ToString(), 2.0d, 100.0d, 8.0d, "V", "0.0"));
                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.MsrtClamp.ToString(), 2.0d, 100.0d, 8.0d, "V", "0.0"));

                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.FilterCount.ToString(), 0.0d, 20.0d, 5.0d, "cnt", "0"));
                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.NPLC.ToString(), 0.01d, 10.0d, 0.01d, "", "0.00"));

                        #endregion

                        break;
                    }
				//-------------------------------------------------------------------------------------------------------------------------------------
				case ETestType.LCR:
                case ETestType.LCRSWEEP:
					{
						#region >>> LCR <<<

						itemDescription.Add(new LCRItemDescription(EItemDescription.LCR_IsProvideSignalLevelV.ToString(), 0.0d, 0.0d, 0.0d, "", "0"));
						itemDescription.Add(new LCRItemDescription(EItemDescription.LCR_IsProvideSignalLevelI.ToString(), 0.0d, 0.0d, 0.0d, "", "0"));
						itemDescription.Add(new LCRItemDescription(EItemDescription.LCR_IsProvideDCBiasV.ToString(), 0.0d, 0.0d, 0.0d, "", "0"));
						itemDescription.Add(new LCRItemDescription(EItemDescription.LCR_IsProvideDCBiasI.ToString(), 0.0d, 0.0d, 0.0d, "", "0"));
						itemDescription.Add(new LCRItemDescription(EItemDescription.LCR_MsrtSpeed.ToString(), 0.0d, 0.0, 0.0d, "", "0"));
						itemDescription.Add(new LCRItemDescription(EItemDescription.LCR_TestType.ToString(), 0.0d, 0.0, 0.0d, "", "0"));

						itemDescription.Add(new LCRItemDescription(EItemDescription.WaitTime.ToString(), 0.0d, 10000.0d, 0.0d, "ms", "0.0"));
						itemDescription.Add(new LCRItemDescription(EItemDescription.LCR_SignalLevelV.ToString(), 0.0d, 100.0d, 0.5d, "V", "0.000"));
						itemDescription.Add(new LCRItemDescription(EItemDescription.LCR_SignalLevelI.ToString(), 0.0d, 1000d, 0.005d, "mA", "0.000"));
						itemDescription.Add(new LCRItemDescription(EItemDescription.LCR_Frequency.ToString(), 0.02d, 3000000, 20, "Hz", "0.000"));
						itemDescription.Add(new LCRItemDescription(EItemDescription.LCR_DCBiasV.ToString(), -200.0d, 200.0d, 0.0d, "V", "0.0"));
						itemDescription.Add(new LCRItemDescription(EItemDescription.LCR_DCBiasI.ToString(), -1000, 1000, 0.0d, "mA", "0"));

                        itemDescription.Add(new LCRItemDescription(EItemDescription.LCR_CaliDataQty.ToString(), -1000, 1000, 0.0d, "", "0"));
                        itemDescription.Add(new LCRItemDescription(EItemDescription.LCR_CableLength.ToString(), -1000, 1000, 0.0d, "", "0"));
                        itemDescription.Add(new LCRItemDescription(EItemDescription.LCR_CaliType.ToString(), -1000, 1000, 0.0d, "", "0"));

                        itemDescription.Add(new LCRItemDescription(EItemDescription.LCR_SignalLevelV.ToString(), 0.0d, 100.0d, 0.5d, "V", "0.000"));


                        itemDescription.Add(new LCRItemDescription(EItemDescription.LCR_Cali_UI_Ver.ToString(), 0, 10, 2, "", "0"));
                        itemDescription.Add(new LCRItemDescription(EItemDescription.LCR_Enable_Open_Cali.ToString(), 0, 1, 1, "", "0"));
                        itemDescription.Add(new LCRItemDescription(EItemDescription.LCR_Enable_Short_Cali.ToString(), 0, 1, 0, "", "0"));
                        itemDescription.Add(new LCRItemDescription(EItemDescription.LCR_Enable_Load_Cali.ToString(), 0, 1, 0, "", "0"));

                        

						#endregion

						break;
					}
                //-------------------------------------------------------------------------------------------------------------------------------------
                case ETestType.IO:
                    {
                        itemDescription.Add(new IOItemDescription(EItemDescription.IO_Qty.ToString(), 0.0d, 64.0d, 0.0d, "", "0"));
                        itemDescription.Add(new IOItemDescription(EItemDescription.IO_State.ToString(), 0.0d, 0.0d, 0.0d, "", "0"));
                        itemDescription.Add(new IOItemDescription(EItemDescription.IO_Mode.ToString(), 0.0d, 0.0d, 0.0d, "", "0"));
                        itemDescription.Add(new IOItemDescription(EItemDescription.IO_Inverse.ToString(), 0.0d, 1.0d, 0.0d, "", "0"));
                    }
                    break;
                    //-----------------------------------------------------
                case ETestType.LaserSource:
                    {
                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.ATT_Power_Range.ToString(), -50d, 20, 0.0d, "dBm", "0.00"));
                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.ATT_Attenuat_Range.ToString(), 0, 40, 0.0d, "dB", "0.00"));
                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.ATT_AvgTime.ToString(), 0.002, 1, 1d, "S", "0.000"));

                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.ATT_TransitionSpeed.ToString(), 0.1, 1000, 0.1d, "dB/s", "0.0"));
                        itemDescription.Add(new ItemDescriptionBase(EItemDescription.ATT_WavelengthRange.ToString(), 300, 2000, 1550d, "nm", "0.0"));

                    }
                    break;


            }

            return itemDescription;
        }

        /// <summary>
        /// item Description 與 Device Spec 的交集描述
        /// </summary>
		public static void UpdateItemDescByDeviceSpec(TestItemDescriptionCollections descColl, MachineInfoData info)
		{
			string keyName = string.Empty;

			#region >>> SourceMeterSpec <<<

			if (info.SourceMeterSpec != null)
			{
				SourceMeterSpec srcSpec = info.SourceMeterSpec.Clone() as SourceMeterSpec;

				#region >>> Boundary Check <<<

				if (srcSpec.CurrentRange.Count != 0 && srcSpec.VoltageRange.Count != 0)
				{
                    double minI = srcSpec.MinCurrentRange;  // unit : A

					double maxI = srcSpec.MaxCurrentRange;  // unit : A

					double minV = srcSpec.MinVoltageRange;  // unit : V

					double maxV = srcSpec.MaxVoltageRange;  // unit : V

                    #region >>> DC Boundary <<<

                    if (minI != 0.0d && maxI != 0.0d && minV != 0.0d && maxV != 0.0d)
					{
						keyName = ETestType.IF.ToString();
						descColl[keyName][EItemDescription.ForceValue.ToString()].LimitBoundary(0.0d, maxI, EAmpUnit.A);
						descColl[keyName][EItemDescription.ForceTime.ToString()].LimitBoundary(0.0d, srcSpec.MaxForceTime);
						descColl[keyName][EItemDescription.MsrtRange.ToString()].LimitBoundary(minV, maxV, EVoltUnit.V);
						descColl[keyName][EItemDescription.MsrtClamp.ToString()].LimitBoundary(minV, maxV, EVoltUnit.V);

						keyName = ETestType.VF.ToString();
						descColl[keyName][EItemDescription.ForceValue.ToString()].LimitBoundary(0.0d, maxV, EVoltUnit.V);
						descColl[keyName][EItemDescription.ForceTime.ToString()].LimitBoundary(0.0d, srcSpec.MaxForceTime);
						descColl[keyName][EItemDescription.MsrtRange.ToString()].LimitBoundary(minI, maxI, EAmpUnit.A);
						descColl[keyName][EItemDescription.MsrtClamp.ToString()].LimitBoundary(minI, maxI, EAmpUnit.A);

						keyName = ETestType.IZ.ToString();
						descColl[keyName][EItemDescription.ForceValue.ToString()].LimitBoundary(0.0d, maxI, EAmpUnit.A);
						descColl[keyName][EItemDescription.ForceTime.ToString()].LimitBoundary(0.0d, srcSpec.MaxForceTime);
						descColl[keyName][EItemDescription.MsrtRange.ToString()].LimitBoundary(minV, maxV, EVoltUnit.V);
						descColl[keyName][EItemDescription.MsrtClamp.ToString()].LimitBoundary(minV, maxV, EVoltUnit.V);

						keyName = ETestType.VR.ToString();
						descColl[keyName][EItemDescription.ForceValue.ToString()].LimitBoundary(0.0d, maxV, EVoltUnit.V);
						descColl[keyName][EItemDescription.ForceTime.ToString()].LimitBoundary(0.0d, srcSpec.MaxForceTime);
                        //descColl[keyName][EItemDescription.ForceTime.ToString()].LimitBoundary(0.0d, 10000);//20180612 David
						descColl[keyName][EItemDescription.MsrtRange.ToString()].LimitBoundary(minI, maxI, EAmpUnit.A);
						descColl[keyName][EItemDescription.MsrtClamp.ToString()].LimitBoundary(minI, maxI, EAmpUnit.A);

						keyName = ETestType.IFH.ToString();
						descColl[keyName][EItemDescription.ForceValue.ToString()].LimitBoundary(0.0d, maxI, EAmpUnit.A);
						descColl[keyName][EItemDescription.ForceTime.ToString()].LimitBoundary(0.0d, srcSpec.MaxForceTime);

						keyName = ETestType.LOP.ToString();
						descColl[keyName][EItemDescription.ForceValue.ToString()].LimitBoundary(0.0d, maxI, EAmpUnit.A);
						descColl[keyName][EItemDescription.ForceTime.ToString()].LimitBoundary(0.0d, srcSpec.MaxForceTime);
						descColl[keyName][EItemDescription.MsrtRange.ToString()].LimitBoundary(minV, maxV, EVoltUnit.V);
						descColl[keyName][EItemDescription.MsrtClamp.ToString()].LimitBoundary(minV, maxV, EVoltUnit.V);

						keyName = ETestType.LOPWL.ToString();
						descColl[keyName][EItemDescription.ForceValue.ToString()].LimitBoundary(0.0d, maxI, EAmpUnit.A);
						descColl[keyName][EItemDescription.ForceTime.ToString()].LimitBoundary(0.0d, srcSpec.MaxForceTime);
						descColl[keyName][EItemDescription.MsrtRange.ToString()].LimitBoundary(minV, maxV, EVoltUnit.V);
						descColl[keyName][EItemDescription.MsrtClamp.ToString()].LimitBoundary(minV, maxV, EVoltUnit.V);

						keyName = ETestType.IVSWEEP.ToString();
						descColl[keyName][EItemDescription.SweepStart.ToString()].LimitBoundary(-maxI, maxI, EAmpUnit.A);  // 零交越
						descColl[keyName][EItemDescription.SweepEnd.ToString()].LimitBoundary(-maxI, maxI, EAmpUnit.A);  // 零交越
						descColl[keyName][EItemDescription.ForceTime.ToString()].LimitBoundary(0.0d, srcSpec.MaxForceTime);
						descColl[keyName][EItemDescription.MsrtRange.ToString()].LimitBoundary(minV, maxV, EVoltUnit.V);
						descColl[keyName][EItemDescription.MsrtClamp.ToString()].LimitBoundary(minV, maxV, EVoltUnit.V);

						keyName = ETestType.VISWEEP.ToString();
						descColl[keyName][EItemDescription.SweepStart.ToString()].LimitBoundary(-maxV, maxV, EVoltUnit.V);
						descColl[keyName][EItemDescription.SweepEnd.ToString()].LimitBoundary(-maxV, maxV, EVoltUnit.V);
						descColl[keyName][EItemDescription.ForceTime.ToString()].LimitBoundary(0.0d, srcSpec.MaxForceTime);
						descColl[keyName][EItemDescription.MsrtRange.ToString()].LimitBoundary(minI, maxI, EAmpUnit.A);
						descColl[keyName][EItemDescription.MsrtClamp.ToString()].LimitBoundary(minI, maxI, EAmpUnit.A);

						keyName = ETestType.THY.ToString();
						descColl[keyName][EItemDescription.ForceValue.ToString()].LimitBoundary(0.0d, maxI, EAmpUnit.A);
						descColl[keyName][EItemDescription.ForceTime.ToString()].LimitBoundary(0.0d, srcSpec.MaxForceTime);
						descColl[keyName][EItemDescription.MsrtRange.ToString()].LimitBoundary(minV, maxV, EVoltUnit.V);
						descColl[keyName][EItemDescription.MsrtClamp.ToString()].LimitBoundary(minV, maxV, EVoltUnit.V);

						keyName = ETestType.POLAR.ToString();
						descColl[keyName][EItemDescription.ForceValue.ToString()].LimitBoundary(0.0d, maxI, EAmpUnit.A);
						descColl[keyName][EItemDescription.ForceTime.ToString()].LimitBoundary(0.0d, srcSpec.MaxForceTime);
						descColl[keyName][EItemDescription.MsrtRange.ToString()].LimitBoundary(minV, maxV, EVoltUnit.V);
						descColl[keyName][EItemDescription.MsrtClamp.ToString()].LimitBoundary(minV, maxV, EVoltUnit.V);

						keyName = ETestType.VAC.ToString();
						descColl[keyName][EItemDescription.ForceValue.ToString()].LimitBoundary(0.0d, maxV, EVoltUnit.V);
						descColl[keyName][EItemDescription.ForceTime.ToString()].LimitBoundary(0.0d, srcSpec.MaxForceTime);
						descColl[keyName][EItemDescription.MsrtRange.ToString()].LimitBoundary(minI, maxI, EAmpUnit.A);
						descColl[keyName][EItemDescription.MsrtClamp.ToString()].LimitBoundary(minI, maxI, EAmpUnit.A);

						keyName = ETestType.R.ToString();
                       // descColl[keyName][EItemDescription.RTestItemRange.ToString()].LimitBoundary(0.1d, 100e6);

						keyName = ETestType.RTH.ToString();
						descColl[keyName][EItemDescription.ForceValue.ToString()].LimitBoundary(0.0d, maxI, EAmpUnit.A);
						descColl[keyName][EItemDescription.ForceTime.ToString()].LimitBoundary(0.0d, srcSpec.MaxForceTime);
						descColl[keyName][EItemDescription.MsrtRange.ToString()].LimitBoundary(minV, maxV, EVoltUnit.V);
						descColl[keyName][EItemDescription.MsrtClamp.ToString()].LimitBoundary(minV, maxV, EVoltUnit.V);

						keyName = ETestType.LIV.ToString();
						descColl[keyName][EItemDescription.SweepStart.ToString()].LimitBoundary(0.0d, maxI, EAmpUnit.A);
						descColl[keyName][EItemDescription.SweepEnd.ToString()].LimitBoundary(0.0d, maxI, EAmpUnit.A);
						descColl[keyName][EItemDescription.ForceTime.ToString()].LimitBoundary(0.0d, srcSpec.MaxForceTime);
						descColl[keyName][EItemDescription.MsrtRange.ToString()].LimitBoundary(minV, maxV, EVoltUnit.V);
						descColl[keyName][EItemDescription.MsrtClamp.ToString()].LimitBoundary(minV, maxV, EVoltUnit.V);

						keyName = ETestType.PIV.ToString();
						descColl[keyName][EItemDescription.SweepStart.ToString()].LimitBoundary(0.0d, maxI, EAmpUnit.A);
						descColl[keyName][EItemDescription.SweepStep.ToString()].LimitBoundary(0.0d, maxI, EAmpUnit.A);
						descColl[keyName][EItemDescription.SweepEnd.ToString()].LimitBoundary(0.0d, maxI, EAmpUnit.A);
						descColl[keyName][EItemDescription.ForceTime.ToString()].LimitBoundary(0.0d, srcSpec.MaxForceTime);
						descColl[keyName][EItemDescription.MsrtRange.ToString()].LimitBoundary(minV, maxV, EVoltUnit.V);
						descColl[keyName][EItemDescription.MsrtClamp.ToString()].LimitBoundary(minV, maxV, EVoltUnit.V);
                    }

                    #endregion

                    #region >>> Pulse Boundary  <<<

                    if (srcSpec.PulseSpec.Count > 0)
                    {
                        double maxPulseCurrentRange = srcSpec.PulseSpec.Region.Max(T => T.PulseCurrentRange);

                        int maxCurrentIndex = srcSpec.PulseSpec.Region.FindIndex(T => T.PulseCurrentRange == maxPulseCurrentRange);

                        if (maxCurrentIndex >= 0)
                        {
                            double maxPulseI = srcSpec.PulseSpec.Region[maxCurrentIndex].PulseCurrentRange;

                            double maxPulseWidth = srcSpec.PulseSpec.Region[maxCurrentIndex].PulseWidth;

                            double maxPulseDuty = srcSpec.PulseSpec.Region[maxCurrentIndex].PulseDuty;

                            double maxPulseMsrtRange = srcSpec.PulseSpec.Region[maxCurrentIndex].PulseVoltageRange;

                            keyName = ETestType.PIV.ToString();
                            descColl[keyName][EItemDescription.PulseValue.ToString()].LimitBoundary(maxI, maxPulseI, EAmpUnit.A);
                            descColl[keyName][EItemDescription.PulseWidth.ToString()].LimitBoundary(0.0d, maxPulseWidth);
                            descColl[keyName][EItemDescription.PulseDuty.ToString()].LimitBoundary(0.1d, maxPulseDuty, true);
                            descColl[keyName][EItemDescription.PulseMsrtRange.ToString()].LimitBoundary(minV, maxPulseMsrtRange, EVoltUnit.V, true);

                            keyName = ETestType.LOP.ToString();
                            descColl[keyName][EItemDescription.PulseValue.ToString()].LimitBoundary(maxI, maxPulseI, EAmpUnit.A);
                            descColl[keyName][EItemDescription.PulseWidth.ToString()].LimitBoundary(0.0d, maxPulseWidth);
                            descColl[keyName][EItemDescription.PulseDuty.ToString()].LimitBoundary(0.1d, maxPulseDuty, true);
                            descColl[keyName][EItemDescription.PulseMsrtRange.ToString()].LimitBoundary(minV, maxPulseMsrtRange, EVoltUnit.V, true);

                            keyName = ETestType.LOPWL.ToString();
                            descColl[keyName][EItemDescription.PulseValue.ToString()].LimitBoundary(maxI, maxPulseI, EAmpUnit.A);
                            descColl[keyName][EItemDescription.PulseWidth.ToString()].LimitBoundary(0.0d, maxPulseWidth);
                            descColl[keyName][EItemDescription.PulseDuty.ToString()].LimitBoundary(0.1d, maxPulseDuty, true);
                            descColl[keyName][EItemDescription.PulseMsrtRange.ToString()].LimitBoundary(minV, maxPulseMsrtRange, EVoltUnit.V, true);
                        }
                    }

                    #endregion
                }

				#endregion

                #region >>> Property Supported/Visible Check <<<

                List<string> testTypeList = new List<string>(Enum.GetNames(typeof(ETestType)));

				string strType = string.Empty;

				foreach (var testType in testTypeList)
				{
					strType = testType.ToString();

					if (descColl.ContainsKeyName(strType))
					{
						// IsAutoMsrtRange (MsrtRange 由 Clamp決定, 並隱藏MsrtRange顯示)
						if (descColl[strType].ContainsKeyName(EItemDescription.MsrtRange.ToString()) &&
							descColl[strType].ContainsKeyName(EItemDescription.MsrtClamp.ToString()))
						{
							descColl[strType][EItemDescription.MsrtRange.ToString()].IsEnable = true;
							descColl[strType][EItemDescription.MsrtClamp.ToString()].IsEnable = true;

							if (srcSpec.IsAutoMsrtRange)
							{
								descColl[strType][EItemDescription.MsrtRange.ToString()].IsVisible = false;
								descColl[strType][EItemDescription.MsrtClamp.ToString()].IsVisible = true;
							}
							else
							{
								descColl[strType][EItemDescription.MsrtRange.ToString()].IsVisible = true;
								descColl[strType][EItemDescription.MsrtClamp.ToString()].IsVisible = true;
							}
						}

						// NPLC Check
						if (descColl[strType].ContainsKeyName(EItemDescription.NPLC.ToString()))
						{
							descColl[strType][EItemDescription.NPLC.ToString()].IsEnable = srcSpec.IsSupportedNPLC;
						}

						// MsrtFilter Check
						if (descColl[strType].ContainsKeyName(EItemDescription.FilterCount.ToString()))
						{
							descColl[strType][EItemDescription.FilterCount.ToString()].IsEnable = srcSpec.IsSupportedMsrtFilter;
						}
					}
				}


                (descColl[ETestType.IO.ToString()][EItemDescription.IO_Qty.ToString()] as IOItemDescription).IOQty = srcSpec.IOQty;


                List<EIOTrig_Mode> eList = (from p in Enum.GetNames(typeof(EIOTrig_Mode))
                                            select ((EIOTrig_Mode)Enum.Parse(typeof(EIOTrig_Mode), p))).ToList();

                foreach (EIOTrig_Mode mode in eList)
                {
                    (descColl[ETestType.IO.ToString()][EItemDescription.IO_Mode.ToString()] as IOItemDescription).ModeList.Add(mode);
                }

                List<EIOState> sList = (from p in Enum.GetNames(typeof(EIOState))
                                        select ((EIOState)Enum.Parse(typeof(EIOState), p))).ToList();

                foreach (EIOState state in sList)
                {
                    (descColl[ETestType.IO.ToString()][EItemDescription.IO_State.ToString()] as IOItemDescription).StateList.Add(state);
                }

				#endregion
			}

			#endregion

			#region >>> SpectrometerSpec <<<

			if (info.SpectrometerSpec != null)
			{
				SpectrometerSpec sptSpec = info.SpectrometerSpec.Clone() as SpectrometerSpec;
			}

			#endregion

			#region >>> ESDSpec <<<

			if (info.ESDSpec != null)
			{
				ESDSpec esdSpec = info.ESDSpec.Clone() as ESDSpec;

				keyName = ETestType.ESD.ToString();

				descColl[keyName][EItemDescription.HBMVolt.ToString()].LimitBoundary(esdSpec.HBMVoltMinValue, esdSpec.HBMVoltMaxValue);
				descColl[keyName][EItemDescription.MMVolt.ToString()].LimitBoundary(esdSpec.MMVoltMinValue, esdSpec.MMVoltMaxValue);
				descColl[keyName][EItemDescription.ESDInterval.ToString()].LimitBoundary(esdSpec.ZapInvervalMinValue, esdSpec.ZapInvervalMaxValue);
				descColl[keyName][EItemDescription.ESDCount.ToString()].LimitBoundary(1.0d, esdSpec.ZapCountMaxValue);
			}

			#endregion

			#region >>> LCRMeterSpec <<<

			if (info.LCRMeterSpec != null)
			{
				LCRMeterSpec lcrSpec = info.LCRMeterSpec.Clone() as LCRMeterSpec;

                GetLcrSpec(descColl[ETestType.LCR.ToString()], ETestType.LCRSWEEP.ToString(), lcrSpec, info);

                GetLcrSpec(descColl[ETestType.LCRSWEEP.ToString()], ETestType.LCRSWEEP.ToString(), lcrSpec, info);
                
			}

			#endregion

            #region >>> LaserSourceSpec <<<

            if (info.AttSpec != null)
            {
                AttenuatorSpec AttSpec = info.AttSpec.Clone() as AttenuatorSpec;

                GetAttenuatorSpec(descColl[ETestType.LaserSource.ToString()], ETestType.LaserSource.ToString(), AttSpec, info);
            }

            #endregion

		}

        private static void GetLcrSpec(TestItemDescription td, string keyName, LCRMeterSpec lcrSpec, MachineInfoData info)
        {

            td[EItemDescription.LCR_IsProvideSignalLevelV.ToString()].IsEnable &= lcrSpec.IsProvideSignalLevelV;

            td[EItemDescription.LCR_IsProvideSignalLevelI.ToString()].IsEnable &= lcrSpec.IsProvideSignalLevelI;

            td[EItemDescription.LCR_IsProvideDCBiasV.ToString()].IsEnable &= lcrSpec.IsProvideDCBiasV;

            td[EItemDescription.LCR_IsProvideDCBiasI.ToString()].IsEnable &= lcrSpec.IsProvideDCBiasI;

            td[EItemDescription.LCR_SignalLevelV.ToString()].LimitBoundary(lcrSpec.SignalLevelVMin, lcrSpec.SignalLevelVMax, EVoltUnit.V);

            td[EItemDescription.LCR_SignalLevelI.ToString()].LimitBoundary(lcrSpec.SignalLevelIMin, lcrSpec.SignalLevelIMax, EAmpUnit.A);

            td[EItemDescription.LCR_Frequency.ToString()].LimitBoundary(lcrSpec.FrequencyMin, lcrSpec.FrequencyMax, EFreqUnit.Hz);

            td[EItemDescription.LCR_DCBiasV.ToString()].LimitBoundary(lcrSpec.DCBiasVMin, lcrSpec.DCBiasVMax, EVoltUnit.V);

            td[EItemDescription.LCR_DCBiasI.ToString()].LimitBoundary(lcrSpec.DCBiasIMin, lcrSpec.DCBiasIMax, EAmpUnit.A);

            if (info.DCBiasSpec != null)
            {
                td[EItemDescription.LCR_DCBiasV.ToString()].LimitBoundary(info.DCBiasSpec.MinVoltageRange, info.DCBiasSpec.MaxVoltageRange, EVoltUnit.V);

                td[EItemDescription.LCR_DCBiasI.ToString()].LimitBoundary(info.DCBiasSpec.MinCurrentRange, info.DCBiasSpec.MaxCurrentRange, EAmpUnit.A);
            }

            foreach (var item in lcrSpec.MsrtSpeedList)
            {
                (td[EItemDescription.LCR_MsrtSpeed.ToString()] as LCRItemDescription).SupportMsrtSpeed.Add(item);
            }

            foreach (var item in lcrSpec.TestTypeList)
            {
                (td[EItemDescription.LCR_TestType.ToString()] as LCRItemDescription).SupportTestItemList.Add(item);
            }

            (td[EItemDescription.LCR_CaliDataQty.ToString()] as LCRItemDescription).CaliDataQty = lcrSpec.CaliDataQty;


            foreach (string item in lcrSpec.CableLenList)
            {
                LCRItemDescription temp = td[EItemDescription.LCR_CableLength.ToString()] as LCRItemDescription;
                (td[EItemDescription.LCR_CableLength.ToString()] as LCRItemDescription).CableLengthList.Add(item);
            }

            foreach (ELCRTestType item in lcrSpec.CaliTypeList)
            {
                switch (item)
                {
                    case ELCRTestType.CPD:
                    case ELCRTestType.CSD:
                        (td[EItemDescription.LCR_CaliType.ToString()] as LCRItemDescription).CaliTypeList.Add(item);
                        break;
                }
            }
        }

        private static void GetAttenuatorSpec(TestItemDescription td, string keyName, AttenuatorSpec attSpec, MachineInfoData info)
        {
            SetSpecInRange(td[EItemDescription.ATT_Attenuat_Range.ToString()], attSpec.AttenuationRange);

            SetSpecInRange(td[EItemDescription.ATT_Power_Range.ToString()], attSpec.PowerRange);

            SetSpecInRange(td[EItemDescription.ATT_AvgTime.ToString()], attSpec.AveragingTime);

            SetSpecInRange(td[EItemDescription.ATT_TransitionSpeed.ToString()], attSpec.TransitionSpeed);

            SetSpecInRange(td[EItemDescription.ATT_WavelengthRange.ToString()], attSpec.WavelengthRange);

        }

        private static void SetSpecInRange(ItemDescriptionBase desc, MinMaxValuePair<double> range)
        {
            desc.MaxValue = range.SetInRange(desc.MaxValue);
        }
       
    }
}

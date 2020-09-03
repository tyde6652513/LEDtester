using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Collections;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

using MPI.Tester.Data;
using MPI.Tester.DeviceCommon;
using MPI.Tester.Gui.UIForm.UserForm.Condition;
using MPI.Tester.Gui.UIForm.Dialog.HardWareSetting;
using MPI.Tester.Maths;

namespace MPI.Tester.Gui
{
	public partial class frmCondition : System.Windows.Forms.Form
	{
		public event EventHandler<EventArgs> TestItemDataChangeEvent;

		private delegate void UpdateDataHandler();

        public frmDailyVerify _frmDailyWath = new frmDailyVerify();
   
        public frmDailyCheckSetting _frmDailyCheckSetting;

        public frmDeviceVerify _deviceVerify;

        private IfrmCusConditin customerFrm;

        private frmLCRCaliVer2 _frmLcrCali = null;

        private bool _isRefreshEnd = true;

		public frmCondition()
		{
			Console.WriteLine("[frmCondition], frmCondition()");

			InitializeComponent();

            this.InitUiComponent();
			this.InitItemTableDGV();
			this.InitMsrtItemDGV();

			this.TestItemDataChangeEvent += new EventHandler<EventArgs>(Host.TestItemDataChangeEventHandler);

            this.IncludeChannelConditionFrm();
		}

		#region >>> Private Method <<<

        private void InitUiComponent()
		{
            //---------------------------------------
            // UI component initialization
            //---------------------------------------
            this.cmbPolarity.Items.Clear();
            string[] strPolarity = Enum.GetNames(typeof(EPolarity));
            this.cmbPolarity.Items.Add(strPolarity[0]);
            this.cmbPolarity.Items.Add(strPolarity[1]);

            this.cmbFilterPosition.Items.Clear();
            string[] attenuatorItems = new string[] { "1", "2", "3", "4", "5" };
            this.cmbFilterPosition.Items.AddRange(attenuatorItems);

            this.cmbStage.Items.Clear();
            this.cmbStage.Items.Add(ETestStage.IV.ToString());
            this.cmbStage.Items.Add(ETestStage.LCR.ToString());
            this.cmbStage.Items.Add(ETestStage.Sampling.ToString());
            this.cmbStage.Items.Add(ETestStage.Sampling1.ToString());
            this.cmbStage.Items.Add(ETestStage.Sampling2.ToString());

            if (DataCenter._machineConfig.OSAModel != EOSAModel.NONE)
            {
                this.btnOsaCoupling.Visible = true;
            }

            if (DataCenter._uiSetting.UserID == EUserID.Emcore &&
                DataCenter._machineConfig.LaserSrcSysConfig!= null &&
                DataCenter._machineConfig.LaserSrcSysConfig.Attenuator != null &&
                DataCenter._machineConfig.LaserSrcSysConfig.Attenuator.AttenuatorModel != ELaserAttenuatorModel.NONE)
            {
                this.btnAttenuator.Visible = true;
            }

            if (DataCenter._rdFunc.RDFuncData.TesterConfigType == ETesterConfigType.PDTester)
            {
                pnlOptical.Visible = false;
            }

            SetCustomerUI();
            //SetLCRUI();會比LCr init 還早被執行，因此移到別處
		}

		private void InitItemTableDGV()
		{
			//-----------------------------------------------------------------------------
			// DataGridView Setting for Test Items Setting Table
			//-----------------------------------------------------------------------------
			this.dgvItemTable.DataSource = null;

            this.dgvItemTable.Columns["colTestItemEnable"].ValueType = System.Type.GetType("System.bool");
            this.dgvItemTable.Columns["colGroup1"].ValueType = System.Type.GetType("System.bool");
            this.dgvItemTable.Columns["colGroup2"].ValueType = System.Type.GetType("System.bool");
            this.dgvItemTable.Columns["colGroup3"].ValueType = System.Type.GetType("System.bool");
            this.dgvItemTable.Columns["colChannel"].ValueType = System.Type.GetType("System.String");
            this.dgvItemTable.Columns["colItemName"].ValueType = System.Type.GetType("System.String");
            this.dgvItemTable.Columns["colForceValue"].ValueType = System.Type.GetType("System.String");
            this.dgvItemTable.Columns["colForceTime"].ValueType = System.Type.GetType("System.String");
            this.dgvItemTable.Columns["colMsrtRange"].ValueType = System.Type.GetType("System.String");
            this.dgvItemTable.Columns["colOthers"].ValueType = System.Type.GetType("System.String");

			this.dgvItemTable.ReadOnly = true;
			this.dgvItemTable.AllowUserToAddRows = false;
			this.dgvItemTable.AllowUserToDeleteRows = false;
			this.dgvItemTable.AllowUserToResizeRows = false;
			//this.dgvItemTable.AllowUserToResizeColumns = false;
			this.dgvItemTable.AllowUserToOrderColumns = false;

            this.UpdateItemTableLayout();

		}

		private void InitMsrtItemDGV()
		{
			//--------------------------------------------------
			// DataGridView Setting for GainOffset Table
			//--------------------------------------------------
			this.dgvMsrtItem.DataSource = null;

			for ( int i = 0; i < this.dgvMsrtItem.Columns.Count; i++ )
			{
				this.dgvMsrtItem.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
             //   this.dgvMsrtItem2.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
			}

            for (int i = 0; i < this.dgvMsrtItem2.Columns.Count; i++)
            {
                //this.dgvMsrtItem.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                this.dgvMsrtItem2.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            this.dgvMsrtItem.Columns[0].ValueType = System.Type.GetType("System.String");
			this.dgvMsrtItem.Columns[1].ValueType = System.Type.GetType("System.bool");
			this.dgvMsrtItem.Columns[2].ValueType = System.Type.GetType("System.bool");
			this.dgvMsrtItem.Columns[3].ValueType = System.Type.GetType("System.bool");
			this.dgvMsrtItem.Columns[4].ValueType = System.Type.GetType("System.String");
			this.dgvMsrtItem.Columns[5].ValueType = System.Type.GetType("System.Double");
			this.dgvMsrtItem.Columns[6].ValueType = System.Type.GetType("System.Double");
            this.dgvMsrtItem.Columns["colUnit"].ValueType = System.Type.GetType("System.String");

            this.dgvMsrtItem2.Columns[0].ValueType = System.Type.GetType("System.bool");
            this.dgvMsrtItem2.Columns[1].ValueType = System.Type.GetType("System.bool");
            this.dgvMsrtItem2.Columns[2].ValueType = System.Type.GetType("System.bool");
            this.dgvMsrtItem2.Columns[3].ValueType = System.Type.GetType("System.String");
            this.dgvMsrtItem2.Columns[4].ValueType = System.Type.GetType("System.Double");
            this.dgvMsrtItem2.Columns[5].ValueType = System.Type.GetType("System.Double");
 

			this.dgvMsrtItem.AllowUserToAddRows = false;
			this.dgvMsrtItem.AllowUserToDeleteRows = false;
			this.dgvMsrtItem.AllowUserToResizeRows = false;
			this.dgvMsrtItem.AllowUserToResizeColumns = false;
			this.dgvMsrtItem.AllowUserToOrderColumns = false;

            this.dgvMsrtItem2.AllowUserToAddRows = false;
            this.dgvMsrtItem2.AllowUserToDeleteRows = false;
            this.dgvMsrtItem2.AllowUserToResizeRows = false;
            this.dgvMsrtItem2.AllowUserToResizeColumns = false;
            this.dgvMsrtItem2.AllowUserToOrderColumns = false;

            this.dgvBoundaryRule.Columns[0].ValueType = System.Type.GetType("System.String");
            this.dgvBoundaryRule.Columns[1].ValueType = System.Type.GetType("System.String");
            this.dgvBoundaryRule.Columns[2].ValueType = System.Type.GetType("System.String");

		}

		private void ChangeAuthority()
		{
			switch (DataCenter._uiSetting.AuthorityLevel)
			{ 
				case EAuthority.Operator :
				case EAuthority.QC:
                    //this.gplLeft.Enabled = false;
                    this.gplLeft.Enabled = true;
                    this.dgvItemTable.Enabled = false;
                    this.tabControlPanel4.Enabled = false;
                    this.tabControlPanel6.Enabled = true;
                    this.tabControlPanel5.Enabled = false;

                    this.btnConfirm.Enabled = false;
                    this.chkEnableAllMsrtItem.Enabled = false;
                    this.tabpGainOffset.Enabled = false;
                    this.tabpChuck.Enabled = false;

                    this.btnDailyCheckSetting.Enabled = false;
                    this.btnDailyWatch.Enabled = true;
                    this.btnDailayCheckSpec.Enabled = false;
                    this.pnlConditionEdit.Enabled = false;
                    this.gplRight.Enabled = false;
					break;
				//-------------------------------------------------------------------
				case EAuthority.Engineer :
				case EAuthority.Admin :
				case EAuthority.Super :
                    this.gplLeft.Enabled = true;
                    this.dgvItemTable.Enabled = true;
                    this.tabControlPanel4.Enabled = true;
                    this.tabControlPanel6.Enabled = true;
                    this.tabControlPanel5.Enabled = true;

                    this.btnConfirm.Enabled = true;
                    this.chkEnableAllMsrtItem.Enabled = true;
                    this.tabpGainOffset.Enabled = true;
                    this.tabpChuck.Enabled = true;

                    this.btnDailyCheckSetting.Enabled = true;
                    this.btnDailyWatch.Enabled = true;
                    this.btnDailayCheckSpec.Enabled = true;
                    this.pnlConditionEdit.Enabled = true;
                    this.gplRight.Enabled = true;
					break;
				default:
                    this.gplLeft.Enabled = false;
                    this.btnConfirm.Enabled = true;
                    this.chkEnableAllMsrtItem.Enabled = true;
                    this.btnConfirm.Enabled = false;
                    this.chkEnableAllMsrtItem.Enabled = false;
                    this.tabpGainOffset.Enabled = false;
                    this.tabpChuck.Enabled = false;
                    this.tabControlPanel5.Enabled = true;

                    this.btnDailyCheckSetting.Enabled = false;
                    this.btnDailyWatch.Enabled = true;
                    this.btnDailayCheckSpec.Enabled = true;
                    this.pnlConditionEdit.Enabled = true;
                    this.gplRight.Enabled = true;
					break;
			}
		}

		private void UpdateDataToControls()
		{
			this.lblLOPSelect.Text = DataCenter._product.LOPSaveItem.ToString();

            //this.cmbPolarity.SelectedItem = DataCenter._product.TestCondition.ChipPolarity.ToString();

            //this.cmbFilterPosition.SelectedIndex = (int)DataCenter._product.ProductFilterWheelPos;

			this.ReCheckItemEnableForDevice();


            SetLCRUI();

            if (customerFrm != null)
            {
                customerFrm.Refresh();
            }
			//------------------------------------------------------------------------------------------
			// Update the data grid view of "setting table" and "gain offset table"
			//------------------------------------------------------------------------------------------
			this.UpdateSettingAndMsrtItemDGV();

            bool enable = false;

            switch (DataCenter._uiSetting.UIOperateMode)
            {
                case (int)EUIOperateMode.Idle:
                  //  this.gplLeft.Enabled = true;
                    this.ChangeAuthority();
                    enable = true;
                    break;
                //-----------------------------------------------------------------------------
                case (int)EUIOperateMode.AutoRun:
                case (int)EUIOperateMode.ManulRun:
                    this.gplLeft.Enabled = false;
                    this.btnConfirm.Enabled = false;
                    this.chkEnableAllMsrtItem.Enabled = false;
                    this.tabpGainOffset.Enabled = false;
                    this.tabpChuck.Enabled = false;
                    this.btnDailyCheckSetting.Enabled = false;
                    this.btnDailyWatch.Enabled = false;
                    enable = false;
                    break;
                //-----------------------------------------------------------------------------
                //case (int)EUIOperateMode.ManulRun:
                //    this.gplLeft.Enabled = false;
                //    this.ChangeAuthority();
                //    enable = false;
                //    break;
                //-----------------------------------------------------------------------------
                default:
                    this.gplLeft.Enabled = true;
					this.ChangeAuthority();
                    enable = true;
                    break;
            }

            //給ChangeAuthority()管理
            if (enable)// && (int)DataCenter._uiSetting.AuthorityLevel >= 20)
            {
                this.gplLeft.Enabled = true;
                this.btnConfirm.Enabled = true;
            }
            else
            {
                this.gplLeft.Enabled = false;
                this.btnConfirm.Enabled = false;
            }

            //---------------------------------------------------------------------------------------------------
            // If Dialog is Open , Close the Dialog //
            if (this._frmDailyWath != null || !this._frmDailyWath.IsDisposed)
            {
                if (this._frmDailyWath.isAlreadyShow)
                {
                    this._frmDailyWath.Close();
                    this._frmDailyWath.Dispose();
                }
            }

            if (DataCenter._uiSetting.IsRunDailyCheckMode && DataCenter._uiSetting.IsShowDailyCheckUI
                && DataCenter._toolConfig.DCheck.AutoRunLevel >= 1)
            {
                this.AutoPopDaliyCheckUI();
                DataCenter._uiSetting.IsShowDailyCheckUI = false;
            }

            if (DataCenter._uiSetting.UserDefinedData.DCheckFormat.Length != 0)
            {
                this.plDailyCheck.Visible = true;
            }
            else
            {
                this.plDailyCheck.Visible = false;
            }
            //---------------------------------------------------------------------------------------------------

            if (DataCenter._sysSetting.IsCheckSpec2)
            {
                tabiGainOffset2.Visible = true;
            }
            else
            {
                tabiGainOffset2.Visible = false;
            }

            //===================
            // update Cond. Para
            //===================
            this.cmbPolarity.SelectedItem = DataCenter._product.TestCondition.ChipPolarity.ToString();

            this.cmbStage.SelectedItem = DataCenter._product.TestCondition.TestStage.ToString();
          
            this.cmbFilterPosition.SelectedIndex = (int)DataCenter._product.ProductFilterWheelPos;

            this.dinPdDetectorFactor.Value = DataCenter._product.PdDetectorFactor;

            this.txtProductType.Text = DataCenter._product.ProductName;

            this.cmbLopSaveItem.Items.Clear();
            this.cmbLopSaveItem.Items.AddRange(DataCenter._uiSetting.UserDefinedData.LOPItemSelectList);
            this.cmbLopSaveItem.SelectedItem = DataCenter._product.LOPSaveItem.ToString();

            this.chkEnableTestGroup.Checked = DataCenter._product.CustomerizedSetting.IsEnableTestGroup;
            if (this.cmbLopSaveItem.SelectedIndex == -1)
            {
                this.cmbLopSaveItem.SelectedIndex = 0;
                Host.SetErrorCode(EErrorCode.LOPSaveItemNotMatch);
                DataCenter._product.LOPSaveItem = (ELOPSaveItem)Enum.Parse(typeof(ELOPSaveItem), this.cmbLopSaveItem.SelectedItem.ToString(), true);     // 0-base
                DataCenter._conditionCtrl.ResetLOPVisionProperty(DataCenter._product.LOPSaveItem);
            }

            this.chkIsTVSTesting.Checked = DataCenter._product.IsTVSProduct;

          

            if (DataCenter._machineConfig.TesterFunctionType == ETesterFunctionType.Multi_Die)
            {
                this.tabiMutliDie.Visible = true;
            }
            else
            {
                this.tabiMutliDie.Visible = false;
            }


            dgvMsrtItem.Columns[4].ReadOnly = !DataCenter._uiSetting.UserDefinedData.IsPermitSetMsrtItemName;

            dgvMsrtItem.Columns["colUnit"].ReadOnly = !DataCenter._uiSetting.UserDefinedData.IsPermitSetMsrtItemUnit;

            string info = "Recipe Channel Setting:";

            // (1) check TestCondition length and channelConditionData length
            if (DataCenter._product.TestCondition.TestItemArray != null)
            {
                info = string.Format("Recipe Channel Setting: X = {0}, Y = {1}", DataCenter._product.TestCondition.ChannelConditionTable.ColXCount,
                                                                                 DataCenter._product.TestCondition.ChannelConditionTable.RowYCount);
            }

            this.lblRecipeCondiTableInfo.Text = info;

            if (DataCenter._machineConfig.TesterFunctionType == ETesterFunctionType.Multi_Die)
            {
                this.lblRecipeCondiTableInfo.Visible = true;
                this.pnlDutChDisplay.Visible = true;
            }
            else
            {
                this.lblRecipeCondiTableInfo.Visible = false;
                this.pnlDutChDisplay.Visible = false;
            }

            this.SetChannelStatusUI(DataCenter._machineConfig.ChannelConfig.ColXCount, DataCenter._machineConfig.ChannelConfig.RowYCount);

            if (DataCenter._machineConfig.SwitchSystemModel != ESwitchSystemModel.NONE)
            {
                this.dgvItemTable.Columns["colChannel"].Visible = true;
            }
            else
            {
                this.dgvItemTable.Columns["colChannel"].Visible = false;
            }

            // Roy, 20140327, show frmChannelCondition
            //this.IncludeChannelConditionFrm();
            FormAgent.ChannelCondition.Show();

            this.UpdateItemTableLayout();
		}
		
		private void UpdateSettingAndMsrtItemDGV()
		{
            string msrtKeyName = string.Empty;

			this.dgvItemTable.Rows.Clear();
			this.dgvMsrtItem.Rows.Clear();
            this.dgvMsrtItem2.Rows.Clear();
            this.dgvBoundaryRule.Rows.Clear();

			if (DataCenter._product.TestCondition == null || DataCenter._product.TestCondition.TestItemArray == null)
			{
				this.dgvItemTable.Update();
				this.dgvMsrtItem.Update();
				return;
			}

			int testItemIndex = 0;
            int resultItemIndex = 0;
			int msrtRowIndex = 0;

			TestItemData[] testItems = DataCenter._product.TestCondition.TestItemArray;
			string[] setRowData = new string[6];

			this.dgvItemTable.AllowUserToAddRows = true;
			this.dgvMsrtItem.AllowUserToAddRows = true;

            string strSrcDelayTime = string.Empty;

            if (DataCenter._sysSetting.BinSortingRule == EBinBoundaryRule.Various)
            {
                dgvBoundaryRule.ReadOnly = false;
            }
            else
            {
                dgvBoundaryRule.ReadOnly = true;
            }

            _isRefreshEnd = false;

			foreach (TestItemData item in testItems)
			{
                if (DataCenter._machineConfig.SourceMeterModel == ESourceMeterModel.N5700 ||
					DataCenter._machineConfig.SourceMeterModel == ESourceMeterModel.DR2000 ||
                    DataCenter._machineConfig.SourceMeterModel == ESourceMeterModel.DSPHD || 
					DataCenter._machineConfig.SourceMeterModel == ESourceMeterModel.IT7321 ||
					DataCenter._machineConfig.SourceMeterModel == ESourceMeterModel.K2400 ||
                    DataCenter._machineConfig.SourceMeterModel == ESourceMeterModel.K2600 ||
                    DataCenter._machineConfig.SourceMeterModel == ESourceMeterModel.K2520 ||
                    DataCenter._machineConfig.SourceMeterModel == ESourceMeterModel.B2900A ||
                    DataCenter._machineConfig.SourceMeterModel == ESourceMeterModel.SS400)
                {
					if (item.ElecSetting != null && item.ElecSetting.Length > 0)
					{
						item.ElecSetting[0].MsrtRange = item.ElecSetting[0].MsrtProtection;
                    }
                }

                #region >>> Update Items Setting data into DGV <<<

                strSrcDelayTime = string.Empty;

                setRowData[0] = "1";

				switch (item.Type)
				{
                    case ETestType.POLAR:
                        {
                            setRowData[1] = item.Name.ToString();

                            if (item.ElecSetting != null)
                            {
                                setRowData[2] = Math.Abs(item.ElecSetting[0].ForceValue).ToString() + item.ElecSetting[0].ForceUnit;
                                setRowData[3] = item.ElecSetting[0].ForceTime.ToString() + item.ElecSetting[0].ForceTimeUnit.ToString();
                                setRowData[4] = item.ElecSetting[0].MsrtRange.ToString() + item.ElecSetting[0].MsrtUnit;

                                if (item.ElecSetting[0].MsrtType == EMsrtType.POLAR)
                                {
                                    setRowData[5] = "W=" + item.ElecSetting[0].ForceDelayTime.ToString() + item.ElecSetting[0].ForceTimeUnit + " / " +
                                                            "TH=" + (item as PolarTestItem).ElecSetting[0].PolarThresholdVoltage + item.ElecSetting[0].MsrtUnit;
                                }
                            }
                            break;
                        }
					//-------------------------------------------------------------------------------------------
                    case ETestType.DVF:
                        {
                            setRowData[1] = item.Name.ToString();
                            setRowData[2] = Math.Abs(item.ElecSetting[0].ForceValue).ToString() + " / " +
                                                       Math.Abs(item.ElecSetting[1].ForceValue).ToString() + " / " +
                                                       Math.Abs(item.ElecSetting[2].ForceValue).ToString() + item.ElecSetting[0].ForceUnit;
                            setRowData[3] = item.ElecSetting[0].ForceTime.ToString() + " / " +
                                                        item.ElecSetting[1].ForceTime.ToString() + " / " +
                                                        item.ElecSetting[2].ForceTime.ToString() + item.ElecSetting[0].ForceTimeUnit;
                            setRowData[4] = item.ElecSetting[0].MsrtRange.ToString() + item.ElecSetting[0].MsrtUnit;
                            setRowData[5] = "W=" + item.ElecSetting[0].ForceDelayTime.ToString() + item.ElecSetting[0].ForceTimeUnit;
                           
                            break;
                        }
                    case ETestType.LOP:
                        {
                            if (item.ElecSetting != null)
                            {
                                string strMsrtForceValue = string.Empty;
                                string srcFunc = string.Empty;
                                string srcPulseCnt = string.Empty;

                                if (item.ElecSetting[0].IsEnableMsrtForceValue)
                                {
                                    strMsrtForceValue = "*";
                                }
                                
                                setRowData[0] = (item.SwitchingChannel + 1).ToString();
                                setRowData[1] = item.Name.ToString();

                                double forceValue = Math.Abs(item.ElecSetting[0].ForceValue);
                                string unit = item.ElecSetting[0].ForceUnit;

                                if (unit == "mA" && forceValue > 1000.0d) // 大於 1000mA, UI顯示單位轉 A
                                {
                                    forceValue = Math.Round((forceValue / 1000.0d), 6, MidpointRounding.AwayFromZero);
                                    unit = "A";
                                }

                                setRowData[2] = forceValue.ToString() + unit + strMsrtForceValue;

                                setRowData[3] = item.ElecSetting[0].ForceTime.ToString() + item.ElecSetting[0].ForceTimeUnit.ToString();
                                setRowData[4] = item.ElecSetting[0].MsrtProtection.ToString() + item.ElecSetting[0].MsrtUnit;

                                switch (item.ElecSetting[0].SourceFunction)
                                {
                                    case ESourceFunc.CW:
                                        {
                                            srcFunc = "[C] ";
                                            break;
                                        }
                                    case ESourceFunc.PULSE:
                                        {
                                            srcFunc = "[P] ";
                                            break;
                                        }
                                    case ESourceFunc.QCW:
                                        {
                                            srcFunc = "[Q] ";
                                            srcPulseCnt = string.Format("cnt={0}", item.ElecSetting[0].PulseCount);
                                            setRowData[3] += string.Format(" / D:{0}%", (item.ElecSetting[0].Duty * 100.0d).ToString("0.0"));
                                            break;
                                        }
                                }

                                string detectorStatus = string.Empty;

                                if (DataCenter._machineConfig.PDSensingMode == EPDSensingMode.SrcMeter_2nd)
                                {
                                    if (DataCenter._machineInfo.SourceMeterSpec != null)
                                    {
                                        if (DataCenter._machineInfo.SourceMeterSpec.IsSupportedDualDetectorCH)
                                        {
                                            if (item.ElecSetting[0].IsTrigDetector && item.ElecSetting[0].IsTrigDetector2)
                                            {
                                                detectorStatus = ", CH-A & B";
                                            }
                                            else if (item.ElecSetting[0].IsTrigDetector)
                                            {
                                                detectorStatus = ", CH-A";
                                            }
                                            else if (item.ElecSetting[0].IsTrigDetector2)
                                            {
                                                detectorStatus = ", CH-B";
                                            }
                                        }
                                    }
                                }

                                if (item.ElecSetting[0].ForceDelayTime != 0.0d)
                                {
                                    strSrcDelayTime = "W=" + item.ElecSetting[0].ForceDelayTime.ToString() + item.ElecSetting[0].ForceTimeUnit + ", ";
                                }

                                setRowData[5] = srcFunc + strSrcDelayTime + srcPulseCnt + detectorStatus;
                            }

                            break;
                        }
					//-------------------------------------------------------------------------------------------
					case ETestType.LOPWL:
						{
                            setRowData[0] = (item.SwitchingChannel + 1).ToString();
							setRowData[1] = item.Name.ToString();

                            double forceValue = Math.Abs(item.ElecSetting[0].ForceValue);
                            string unit = item.ElecSetting[0].ForceUnit;
                            string srcFunc = string.Empty;
                            string duty = string.Empty;

                            if ((item as LOPWLTestItem).IsUseMsrtAsForceValue)
                            {
                                string strApply = string.Empty;
                                string applyUnit = item.ElecSetting[0].ForceUnit;
                                string refMsrtName = (item as LOPWLTestItem).RefMsrtKeyName.Split('_')[0];

                                if ((item as LOPWLTestItem).Offset == 0.0d)
                                {
                                    strApply = refMsrtName;
                                }
                                else
                                {
                                    strApply = refMsrtName + " + " + ((item as LOPWLTestItem).Offset * 1000.0d).ToString() + applyUnit;
                                }

                                setRowData[2] = strApply;
                            }
                            else
                            {
                                if (unit == "mA" && forceValue >= 1000.0d) // 大於 1000mA, UI顯示單位轉 A
                            {
                                forceValue = Math.Round((forceValue / 1000.0d), 6, MidpointRounding.AwayFromZero);
                                unit = "A";
                            }

                            setRowData[2] = forceValue.ToString() + unit;
                            }

                            switch (item.ElecSetting[0].SourceFunction)
                            {
                                case ESourceFunc.CW:
                                    {
                                        srcFunc = "[C] ";
                                        break;
                                    }
                                case ESourceFunc.PULSE:
                                    {
                                        srcFunc = "[P] ";
                                        break;
                                    }
                                case ESourceFunc.QCW:
                                    {
                                        srcFunc = "[Q] ";
                                        duty = string.Format("/D:{0}%", (item.ElecSetting[0].Duty * 100.0d).ToString("0.0"));
                                        break;
                                    }
                            }

							if ((item as LOPWLTestItem).OptiSetting.SensingMode == ESensingMode.Fixed)
							{
								setRowData[3] = item.ElecSetting[0].ForceTime.ToString() + "/F_" +
                                                            (item as LOPWLTestItem).OptiSetting.FixIntegralTime.ToString() + item.ElecSetting[0].ForceTimeUnit + duty;
							}
                            else if ((item as LOPWLTestItem).OptiSetting.SensingMode == ESensingMode.Limit)
							{
								setRowData[3] = item.ElecSetting[0].ForceTime.ToString() + "/L_" +
                                                            (item as LOPWLTestItem).OptiSetting.LimitIntegralTime.ToString() + item.ElecSetting[0].ForceTimeUnit + duty;
							}
                            else if ((item as LOPWLTestItem).OptiSetting.SensingMode == ESensingMode.Limit02)
                            {
                                setRowData[3] = item.ElecSetting[0].ForceTime.ToString() + "/L2_" +
                                                            (item as LOPWLTestItem).OptiSetting.LimitIntegralTime.ToString() + item.ElecSetting[0].ForceTimeUnit + duty;
                            }

                            setRowData[4] = item.ElecSetting[0].MsrtRange.ToString() + item.ElecSetting[0].MsrtUnit;

                            if (item.ElecSetting[0].ForceDelayTime != 0.0d)
                            {
                                strSrcDelayTime = "W=" + item.ElecSetting[0].ForceDelayTime.ToString() + item.ElecSetting[0].ForceTimeUnit;
                            }

                            setRowData[5] = srcFunc + strSrcDelayTime;

                            break;
						}
					//-------------------------------------------------------------------------------------------
					case ETestType.THY:
						{
							setRowData[1] = item.Name.ToString();
							setRowData[2] = item.ElecSetting[0].SweepStart.ToString() + " / " +
														item.ElecSetting[0].SweepStep.ToString() + " / " +
														item.ElecSetting[0].SweepStop.ToString() + item.ElecSetting[0].ForceUnit;
							setRowData[3] = "";
							setRowData[4] = item.ElecSetting[0].MsrtRange.ToString() + item.ElecSetting[0].MsrtUnit;

                            if (item.ElecSetting[0].ForceDelayTime != 0.0d)
                            {
                                strSrcDelayTime = "W=" + item.ElecSetting[0].ForceDelayTime.ToString() + item.ElecSetting[0].ForceTimeUnit + " / " ;
                            }

                            setRowData[5] = strSrcDelayTime + "cnt=" + (item.ElecSetting[0].SweepRiseCount + item.ElecSetting[0].SweepContCount).ToString();

                            break;

						}
                    //-------------------------------------------------------------------------------------------
                    case ETestType.IVSWEEP:
                    case ETestType.VISWEEP:
                        {
                            string mode = string.Empty;

                            switch (item.ElecSetting[0].SweepMode)
                            {
                                case ESweepMode.Linear:
                                    {
                                        mode = "LIN";

                                        break;
                                    }
                                case ESweepMode.Log:
                                    {
                                        mode = "LOG";

                                        break;
                                    }
                                case ESweepMode.Custom:
                                    {
                                        mode = "CUST";

                                        break;
                                    }
                            }

                            setRowData[0] = (item.SwitchingChannel + 1).ToString();

                            setRowData[1] = item.Name.ToString();

                            setRowData[2] = item.ElecSetting[0].SweepStart.ToString() + " -> " +
                                            item.ElecSetting[0].SweepStop.ToString() + item.ElecSetting[0].ForceUnit + " / " + mode;


							setRowData[3] =	item.ElecSetting[0].ForceTime.ToString() +
														item.ElecSetting[0].ForceTimeUnit;


                            if (item.ElecSetting[0].IsSweepAutoMsrtRange)
                            {
                                setRowData[4] = "Auto";
                            }
                            else
                            {
                                setRowData[4] = item.ElecSetting[0].MsrtRange.ToString() + item.ElecSetting[0].MsrtUnit;
                            }

                            if (item.ElecSetting[0].ForceDelayTime != 0.0d)
                            {
                                strSrcDelayTime = "W=" + item.ElecSetting[0].ForceDelayTime.ToString() + item.ElecSetting[0].ForceTimeUnit + " / ";
                            }

                            setRowData[5] = strSrcDelayTime + "cnt=" + (item.ElecSetting[0].SweepRiseCount + item.ElecSetting[0].SweepContCount).ToString();

                            break;
                        }
					//-------------------------------------------------------------------------------------------
                    case ETestType.IZ:
                        {
                            string strMsrtForceValue = string.Empty;

                            if (item.ElecSetting[0].IsEnableMsrtForceValue)
                            {
                                strMsrtForceValue = "*";
                            }

                            setRowData[1] = item.Name.ToString();

                            if (item.ElecSetting != null)
                            {
                                setRowData[0] = (item.SwitchingChannel + 1).ToString();

                                if ((item as IZTestItem).IsUseIrAsForceValue)
                                {
                                    string strApply = string.Empty;
                                    string applyUnit = item.ElecSetting[0].ForceUnit;

                                    if ((item as IZTestItem).Factor == 1.0d )
                                    {
                                        strApply = (item as IZTestItem).RefIrName;
                                    }
                                    else if ((item as IZTestItem).Factor != 1.0d )
                                    {
                                        strApply = (item as IZTestItem).RefIrName + " * " + (item as IZTestItem).Factor.ToString("F2") + applyUnit;
                                    }


                                    if ((item as IZTestItem).Offset != 0.0d)
                                    {
                                        if ((item as IZTestItem).Offset < 0)
                                        {
                                            strApply += " + " + Math.Abs((item as IZTestItem).Offset).ToString("F1") + applyUnit;
                                        }
                                        else
                                        {
                                            strApply += " - " + (item as IZTestItem).Offset.ToString("F1") + applyUnit;
                                        }
                                    }

                                    setRowData[2] = strApply + strMsrtForceValue;
                                    setRowData[3] = (item.ElecSetting[0].ForceTime + item.ElecSetting[0].ForceTimeVR).ToString() + item.ElecSetting[0].ForceTimeUnit.ToString();
                                    setRowData[4] = item.ElecSetting[0].MsrtProtection.ToString() + item.ElecSetting[0].MsrtUnit;

                                }
                                else
                                {
                                    setRowData[2] = Math.Abs(item.ElecSetting[0].ForceValue).ToString() + item.ElecSetting[0].ForceUnit + strMsrtForceValue;
                                    setRowData[3] = (item.ElecSetting[0].ForceTime + item.ElecSetting[0].ForceTimeVR).ToString() + item.ElecSetting[0].ForceTimeUnit.ToString();
                                    setRowData[4] = item.ElecSetting[0].MsrtProtection.ToString() + item.ElecSetting[0].MsrtUnit;

                                }

                                if (item.ElecSetting[0].ForceDelayTime != 0.0d)
                                {
                                    strSrcDelayTime = "W=" + item.ElecSetting[0].ForceDelayTime.ToString() + item.ElecSetting[0].ForceTimeUnit;
                                }

                                setRowData[5] = strSrcDelayTime;
                            }

                            break;
                        }
                        break;
					//-------------------------------------------------------------------------------------------
                    case ETestType.ESD:
                        {
                            ESDTestItem esdItem = (item as ESDTestItem);

                            // ESD Test Item					
                            setRowData[1] = esdItem.Name.ToString();

                            string strMode = string.Empty;
                            string strVolt = string.Empty;
                            string strPolar = string.Empty;

                            setRowData[2] = string.Format("{0}, {1}V, {2}", esdItem.EsdSetting.Mode.ToString(), esdItem.EsdSetting.ZapVoltage.ToString(), esdItem.EsdSetting.Polarity.ToString());

                            setRowData[3] = "Cnt=" + esdItem.EsdSetting.Count + ", Delay=" + esdItem.EsdSetting.IntervalTime;

                            if ((item as ESDTestItem).IsEnableJudgeItem)
                            {
                                setRowData[4] = item.ElecSetting[0].MsrtRange.ToString() + item.ElecSetting[0].MsrtUnit;

                                setRowData[5] = "W=" + item.ElecSetting[0].ForceDelayTime.ToString() + item.ElecSetting[0].ForceTimeUnit
                                                + " / " + Math.Abs(item.ElecSetting[0].ForceValue).ToString() + item.ElecSetting[0].ForceUnit
                                                + " / " + item.ElecSetting[0].ForceTime.ToString() + item.ElecSetting[0].ForceTimeUnit;
                            }
                            else
                            {
                                setRowData[4] = "-";

                                setRowData[5] = "No Judge";
                            }

                            break;
                        }
					//-------------------------------------------------------------------------------------------
                    case ETestType.VR:
                        {
                            string strMsrtForceValue = string.Empty;

                            if (item.ElecSetting[0].IsEnableMsrtForceValue)
                            {
                                strMsrtForceValue = "*";
                            }
                            
                            setRowData[1] = item.Name.ToString();

                            if (item.ElecSetting != null)
                            {
                                setRowData[0] = (item.SwitchingChannel + 1).ToString();
                                
                                if ((item as VRTestItem).IsUseVzAsForceValue)
                                {
                                    string strApply = string.Empty;
                                    string applyUnit = item.ElecSetting[0].ForceUnit;

                                    if ((item as VRTestItem).Factor == 1.0d)
                                    {
                                        strApply = (item as VRTestItem).RefVzName;
                                    }
                                    else if ((item as VRTestItem).Factor != 1.0d)
                                    {
                                        strApply = (item as VRTestItem).RefVzName + " * " + (item as VRTestItem).Factor.ToString("F2") + applyUnit;
                                    }


                                    if ((item as VRTestItem).Offset != 0.0d)
                                    {
                                        if ((item as VRTestItem).Offset < 0)
                                        {
                                            strApply += " + " + Math.Abs((item as VRTestItem).Offset).ToString("F1") + applyUnit;
                                        }
                                        else
                                        {
                                            strApply += " - " + (item as VRTestItem).Offset.ToString("F1") + applyUnit;
                                        }
                                    }

                                    setRowData[2] = strApply + strMsrtForceValue;
                                    setRowData[3] = (item.ElecSetting[0].ForceTime + item.ElecSetting[0].ForceTimeVR).ToString() + item.ElecSetting[0].ForceTimeUnit.ToString();
                                    setRowData[4] = item.ElecSetting[0].MsrtProtection.ToString() + item.ElecSetting[0].MsrtUnit;
                                   
                                }
                                else
                                {
                                    setRowData[2] = Math.Abs(item.ElecSetting[0].ForceValue).ToString() + item.ElecSetting[0].ForceUnit + strMsrtForceValue;
                                    setRowData[3] = (item.ElecSetting[0].ForceTime + item.ElecSetting[0].ForceTimeVR).ToString() + item.ElecSetting[0].ForceTimeUnit.ToString();
                                    setRowData[4] = item.ElecSetting[0].MsrtProtection.ToString() + item.ElecSetting[0].MsrtUnit;
                                  
                                }

                                if (item.ElecSetting[0].ForceDelayTime != 0.0d)
                                {
                                    strSrcDelayTime = "W=" + item.ElecSetting[0].ForceDelayTime.ToString() + item.ElecSetting[0].ForceTimeUnit;
                                }

                                setRowData[5] = strSrcDelayTime;
                            }

                            break;
                        }
					//-------------------------------------------------------------------------------------------
                    case ETestType.IFH:
                        {
                            setRowData[1] = item.Name.ToString();
                            if (item.ElecSetting != null)
                            {
                                setRowData[2] = Math.Abs(item.ElecSetting[0].ForceValue).ToString() + item.ElecSetting[0].ForceUnit;
                                setRowData[3] = item.ElecSetting[0].ForceTime.ToString() + item.ElecSetting[0].ForceTimeUnit.ToString();
                                setRowData[4] = item.ElecSetting[0].MsrtRange.ToString() + item.ElecSetting[0].MsrtUnit;

                                  if (item.ElecSetting[0].ForceDelayTime != 0.0d)
                                  {
                                      strSrcDelayTime = "W=" + item.ElecSetting[0].ForceDelayTime.ToString() + item.ElecSetting[0].ForceTimeUnit;
                                  }

                                  setRowData[5] = strSrcDelayTime;
                            }

                            break;
                        }
					//-------------------------------------------------------------------------------------------
                    case ETestType.CALC:
                        {
                            CALCTestItem calcItem = (item as CALCTestItem);

                            setRowData[1] = calcItem.Name.ToString();

                            StringBuilder sb = new StringBuilder();

                            if (calcItem.IsAdvanceMode)
                            {
                                sb.Append("Text:" + calcItem.UserCommand.Substring(0,5) + " ...");
                                setRowData[2] = sb.ToString();
                            }
                            else
                            {
                                sb.Append("(");

                                if (calcItem.IsAConst)
                                {
                                    sb.Append(calcItem.ValB.ToString("E1"));
                                }
                                else
                                {
                                    if (DataCenter._uiSetting.UserDefinedData.MsrtDisplayItemDic.ContainsKey(calcItem.ItemKeyNameA))
                                    {
                                        sb.Append(DataCenter._uiSetting.UserDefinedData.MsrtDisplayItemDic[calcItem.ItemKeyNameA]);
                                    }
                                }

                                switch (calcItem.CalcType)
                                {
                                    case ECalcType.Add:
                                        sb.Append(" + ");
                                        break;
                                    case ECalcType.Subtract:
                                        sb.Append(" - ");
                                        break;
                                    case ECalcType.Multiple:
                                        sb.Append(" x ");
                                        break;
                                    case ECalcType.DivideBy:
                                        sb.Append(" / ");
                                        break;
                                }
                                //sb.Append(calcItem.ItemNameB);
                                if (calcItem.IsBConst)
                                {
                                    sb.Append(calcItem.ValB.ToString("E1"));
                                }
                                else
                                {
                                    if (DataCenter._uiSetting.UserDefinedData.MsrtDisplayItemDic.ContainsKey(calcItem.ItemKeyNameB))
                                    {
                                        sb.Append(DataCenter._uiSetting.UserDefinedData.MsrtDisplayItemDic[calcItem.ItemKeyNameB]);
                                    }
                                }
                                sb.Append(")");

                                if (calcItem.Gain != 1)
                                {
                                    sb.Append(")");
                                    sb.Append(calcItem.Gain.ToString("0.#####"));
                                }

                                setRowData[2] = sb.ToString();

                                if (calcItem.CalcType == ECalcType.DeltaR)
                                {
                                    setRowData[2] = "Delta R";
                                }
                            }

                            //if (item.ElecSetting != null)
                            {
                                // setRowData[1] = Math.Abs(item.ElecSetting[0].ForceValue).ToString() + item.ElecSetting[0].ForceUnit;
                                setRowData[3] = "";// item.ElecSetting[0].ForceTime.ToString() + item.ElecSetting[0].ForceTimeUnit.ToString();
                                setRowData[4] = "";// item.ElecSetting[0].MsrtRange.ToString() + item.ElecSetting[0].MsrtUnit;
                                setRowData[5] = "";// "W=" + item.ElecSetting[0].ForceDelayTime.ToString() + item.ElecSetting[0].ForceTimeUnit;
                            }
                            break;
                        }
                    //-------------------------------------------------------------------------------------------
                    case ETestType.DIB:
                        {
                            DIBTestItem dibItem = (item as DIBTestItem);
                            setRowData[1] = dibItem.Name.ToString();

                            if (item.ElecSetting != null)
                            {
                                setRowData[2] = Math.Abs(item.ElecSetting[0].ForceValue).ToString() + item.ElecSetting[0].ForceUnit;
                                setRowData[3] = string.Empty;
                                setRowData[4] = item.ElecSetting[0].MsrtRange.ToString() + item.ElecSetting[0].MsrtUnit;
                                setRowData[5] = dibItem.FilterBase.ToString() + "/" + dibItem.FilterSpec.ToString() + " V" + " cnt =" + dibItem.FlatCount.ToString();
                            }
                          
                            break;
                        }
                    //-------------------------------------------------------------------------------------------
                    case ETestType.LIV:
                        {
                            setRowData[0] = (item.SwitchingChannel + 1).ToString();
                            setRowData[1] = item.Name.ToString();

                            string mode = string.Empty;

                            switch ((item as LIVTestItem).LIVSweepMode)
                            {
                                case ESweepMode.Linear:
                                    {
                                        mode = "LIN";
                                        break;
                                    }
                                case ESweepMode.Log:
                                    {
                                        mode = "LOG";
                                        break;
                                    }
                                case ESweepMode.Custom:
                                    {
                                        mode = "CUST";
                                        break;
                                    }
                            }

                            setRowData[2] = (item as LIVTestItem).LIVStartValue.ToString() + " -> " +
                                            (item as LIVTestItem).LIVStopValue.ToString() + (item as LIVTestItem).LIVForceUnit + " / " + mode;

                            if ((item as LIVTestItem).LIVSensingMode == ESensingMode.Fixed)
                            {
                                setRowData[3] = (item as LIVTestItem).LIVForceTime.ToString() + " / Fix=" + (item as LIVTestItem).LIVFixIntegralTime.ToString() + (item as LIVTestItem).LIVForceTimeUnit;
                            }
                            else
                            {
                                setRowData[3] = (item as LIVTestItem).LIVForceTime.ToString() + " / LT=" + (item as LIVTestItem).LIVLimitIntegralTime.ToString() + (item as LIVTestItem).LIVForceTimeUnit;
                            }

                            setRowData[4] = (item as LIVTestItem).LIVMsrtRange.ToString() + (item as LIVTestItem).LIVMsrtUnit;
                            setRowData[5] = "W=" + (item as LIVTestItem).LIVForceDelayTime.ToString() + (item as LIVTestItem).LIVForceTimeUnit +
														" / cnt=" + ((item as LIVTestItem).LIVSweepPoints).ToString();

                            break;
                        }
                    //-------------------------------------------------------------------------------------------
                    case ETestType.VISCAN:
                        {
                            setRowData[1] = item.Name.ToString();
                            setRowData[2] = item.ElecSetting[0].ForceValue.ToString() + item.ElecSetting[0].ForceUnit;
                            setRowData[4] = item.ElecSetting[0].MsrtProtection.ToString() + item.ElecSetting[0].MsrtUnit;

                            if (item.ElecSetting[0].ForceDelayTime != 0.0d)
                            {
                                strSrcDelayTime = "W=" + item.ElecSetting[0].ForceDelayTime.ToString() + item.ElecSetting[0].ForceTimeUnit + " / ";
                            }

                            setRowData[5] = strSrcDelayTime + " cnt=" + item.ElecSetting[0].SweepContCount.ToString();
                            
                            break;
                        }
                    case ETestType.PIV:
                        {
                            double start = Math.Abs(item.ElecSetting[0].SweepStart);
                            double step = Math.Abs(item.ElecSetting[0].SweepStep);
                            double end = Math.Abs(item.ElecSetting[0].SweepStop);
                            string unit = item.ElecSetting[0].ForceUnit;
                            string srcFunc = string.Empty;

                            setRowData[0] = (item.SwitchingChannel + 1).ToString();
                            setRowData[1] = item.Name.ToString();

                            if (unit == "mA" && end >= 1000.0d) // 大於 1000mA, UI顯示單位轉 A
                            {
                                start = Math.Round((start / 1000.0d), 6, MidpointRounding.AwayFromZero);
                                step = Math.Round((step / 1000.0d), 6, MidpointRounding.AwayFromZero);
                                end = Math.Round((end / 1000.0d), 6, MidpointRounding.AwayFromZero);
                                unit = "A"; 
                            }

                            setRowData[2] = start.ToString() + " / " + step.ToString() + " / " + end.ToString() + unit;
                            //-------------------------------------------------------------------------------------------
                            string strCount = string.Empty;

                            switch (item.ElecSetting[0].SourceFunction)
                            {
                                case ESourceFunc.CW:
                                    {
                                        srcFunc = "[C] ";
                                        setRowData[3] = item.ElecSetting[0].ForceTime.ToString() +
                                                    item.ElecSetting[0].ForceTimeUnit;
                                        strCount = "cnt=" + (item.ElecSetting[0].SweepRiseCount).ToString();
                                        break;
                                    }
                                case ESourceFunc.PULSE:
                                    {
                                        srcFunc = "[P] ";
                                        setRowData[3] = string.Format("{0}{1} / D:{2}%",
                                            item.ElecSetting[0].ForceTime.ToString(), item.ElecSetting[0].ForceTimeUnit,
                                            (item.ElecSetting[0].Duty * 100.0d).ToString("0.0"));
                                        strCount = "cnt=" + (item.ElecSetting[0].SweepRiseCount).ToString();
                                        break;
                                    }
                                case ESourceFunc.QCW:
                                    {
                                        srcFunc = "[Q] ";
                                        setRowData[3] = string.Format("{0}{1} / D:{2}%",
                                            item.ElecSetting[0].ForceTime.ToString(), item.ElecSetting[0].ForceTimeUnit,
                                            (item.ElecSetting[0].Duty * 100.0d).ToString("0.0"));
                                        strCount = string.Format("cnt={0}*{1}", (item.ElecSetting[0].SweepRiseCount), (item.ElecSetting[0].PulseCount));
                                        break;
                                    }
                            }

                            setRowData[4] = item.ElecSetting[0].MsrtRange.ToString() + item.ElecSetting[0].MsrtUnit;

                            if (item.ElecSetting[0].ForceDelayTime != 0.0d)
                            {
                                strSrcDelayTime = "W=" + item.ElecSetting[0].ForceDelayTime.ToString() + item.ElecSetting[0].ForceTimeUnit + " / ";
                            }

                            setRowData[5] = srcFunc + strSrcDelayTime + strCount;

                            break;
                        }
                    case ETestType.TRANSISTOR:
                        {
                            setRowData[1] = item.Name.ToString();
                            setRowData[2] = "-";

                            string forceTime = (item as TransistorTestItem).TRForceTime.ToString();
                            string forceTimeUnit = (item as TransistorTestItem).TRForceTimeUnit.ToString();
                            string opticalTime = string.Empty;
							string trunOffTime = string.Empty;

                            if ((item as TransistorTestItem).TRIsTestOptical)
                            {
                                if ((item as TransistorTestItem).TRSensingMode == ESensingMode.Fixed)
                                {
									opticalTime = "Fix=" + (item as TransistorTestItem).TRFixIntegralTime.ToString() + " "  +forceTimeUnit;
                                }
                                else
                                {
									opticalTime = "LT=" + (item as TransistorTestItem).TRLimitIntegralTime.ToString() + forceTimeUnit;
                                }
                                }


                            if ((item as TransistorTestItem).TRTurnOffTime > 0.0d)
							{
								trunOffTime = " /Toff=" + (item as TransistorTestItem).TRTurnOffTime.ToString();
                            }

							setRowData[3] = string.Format("F={0}{1} {2}", forceTime, trunOffTime, forceTimeUnit);

							setRowData[4] = opticalTime;

                            setRowData[5] = "W=" + (item as TransistorTestItem).TRProcessDelayTime.ToString() + (item as TransistorTestItem).TRForceTimeUnit +
                                           " / cnt=" + (item as TransistorTestItem).TRSweepPoints.ToString();
                            break;
                        }
					//-------------------------------------------------------------------------------------------
					case ETestType.R:
						{
							if (item.ElecSetting != null)
							{
								setRowData[1] = item.Name.ToString();
								//setRowData[2] = Math.Abs(item.ElecSetting[0].ForceValue).ToString() + item.ElecSetting[0].ForceUnit;
								setRowData[3] = item.ElecSetting[0].MsrtRTestItemSpeed.ToString();
								setRowData[4] = item.ElecSetting[0].MsrtRTestItemRange.ToString() + "Ohm";
								setRowData[5] = "W=" + item.ElecSetting[0].ForceDelayTime.ToString() + item.ElecSetting[0].ForceTimeUnit;
							}

							break;
						}
					//-------------------------------------------------------------------------------------------
                    case ETestType.VF:
                        {
                            if (item.ElecSetting != null)
                            {
                                string strMsrtForceValue = string.Empty;
                                string strRetest = string.Empty;

                                if (item.ElecSetting[0].IsEnableMsrtForceValue)
                                {
                                    strMsrtForceValue = "*";
                                }

                                if ((item as VFTestItem).IsEnableRetest)
                                {
                                    strRetest = string.Format(", Th={0}V, Cr={1}", (item as VFTestItem).RetestThresholdV, (item as VFTestItem).RetestCount);
                                }

                                setRowData[0] = (item.SwitchingChannel + 1).ToString();
                                setRowData[1] = item.Name.ToString();
                                setRowData[2] = Math.Abs(item.ElecSetting[0].ForceValue).ToString() + item.ElecSetting[0].ForceUnit + strMsrtForceValue;
                                setRowData[3] = item.ElecSetting[0].ForceTime.ToString() + item.ElecSetting[0].ForceTimeUnit.ToString();
                                setRowData[4] = item.ElecSetting[0].MsrtProtection.ToString() + item.ElecSetting[0].MsrtUnit;

                                if (item.ElecSetting[0].ForceDelayTime != 0.0d)
                                {
                                    strSrcDelayTime = "W=" + item.ElecSetting[0].ForceDelayTime.ToString() + item.ElecSetting[0].ForceTimeUnit;
                                }

                                setRowData[5] = strSrcDelayTime + strRetest;

                            }

                            break;
                        }
                    //-------------------------------------------------------------------------------------------
                    case ETestType.LCR:
                        {
                            if (item.ElecSetting != null)
                            {
                                string strMsrtForceValue = string.Empty;
                                string strRetest = string.Empty;


                                setRowData[0] = (item.SwitchingChannel + 1).ToString();
                                setRowData[1] = item.Name.ToString();
                                setRowData[2] = "Bias:"+(item as LCRTestItem).LCRSetting.DCBiasV.ToString() + "V";
                                setRowData[3] =  (item as LCRTestItem).LCRSetting.MsrtSpeed.ToString() ; //item.ElecSetting[0].ForceTime.ToString() + item.ElecSetting[0].ForceTimeUnit.ToString();
                                setRowData[4] = (item as LCRTestItem).LCRSetting.Range + "Ohm";
                                if ((item as LCRTestItem).LCRSetting.Range == 0)
                                {
                                    setRowData[4] = "Auto";
                                }

                                if (item.ElecSetting[0].ForceDelayTime != 0.0d)
                                {
                                    strSrcDelayTime = "W=" + item.ElecSetting[0].ForceDelayTime.ToString() + item.ElecSetting[0].ForceTimeUnit;
                                }

                                setRowData[5] = strSrcDelayTime + strRetest;

                            }

                            break;
                        }
                    //-------------------------------------------------------------------------------------------

                    case ETestType.IO:
                        #region
                        {
                            if (item.ElecSetting != null)
                            {
                                string strMsrtForceValue = string.Empty;

                                if (item.ElecSetting[0].IsEnableMsrtForceValue)
                                {
                                    strMsrtForceValue = "*";
                                }

                                setRowData[0] = (item.SwitchingChannel + 1).ToString();
                                setRowData[1] = item.Name.ToString();
                                setRowData[2] = "";
                                setRowData[3] = "";
                                setRowData[4] = "";

                                if (item.ElecSetting[0].ForceDelayTime != 0.0d)
                                {
                                    strSrcDelayTime = "W=" + item.ElecSetting[0].ForceDelayTime.ToString() + item.ElecSetting[0].ForceTimeUnit;
                                }

                                setRowData[5] = strSrcDelayTime;
                            }

                            break;
                        }
                        #endregion
                    //-------------------------------------------------------------------------------------------
                    case ETestType.LaserSource:
                        #region
                        {
                            LaserSourceTestItem lsItem = item as LaserSourceTestItem;
                            if(lsItem.LaserSourceSet != null)
                            {
                                string strMsrtForceValue = string.Empty;
                                string strRetest = string.Empty;

                                setRowData[0] = (item.SwitchingChannel + 1).ToString();
                                setRowData[1] = item.Name.ToString();
                                setRowData[2] = "Ch:" + lsItem.LaserSourceSet.ChName;
                                setRowData[3] = "";
                                setRowData[4] = "";
                                setRowData[5] = "";
                            }

                            break;
                        }
                        #endregion
                    //-------------------------------------------------------------------------------------------
                    default:
                        {
                            if (item.ElecSetting != null)
                            {
                                string strMsrtForceValue = string.Empty;

                                if (item.ElecSetting[0].IsEnableMsrtForceValue)
                                {
                                    strMsrtForceValue = "*";
                                }
                                
                                setRowData[0] = (item.SwitchingChannel + 1).ToString();
                                setRowData[1] = item.Name.ToString();
                                setRowData[2] = Math.Abs(item.ElecSetting[0].ForceValue).ToString() + item.ElecSetting[0].ForceUnit + strMsrtForceValue;
                                setRowData[3] = item.ElecSetting[0].ForceTime.ToString() + item.ElecSetting[0].ForceTimeUnit.ToString();
                                setRowData[4] = item.ElecSetting[0].MsrtProtection.ToString() + item.ElecSetting[0].MsrtUnit;

                                if (item.ElecSetting[0].ForceDelayTime != 0.0d)
                                {
                                    strSrcDelayTime = "W=" + item.ElecSetting[0].ForceDelayTime.ToString() + item.ElecSetting[0].ForceTimeUnit;
                                }

                                setRowData[5] = strSrcDelayTime;
                            }

                            break;
                        }
				}

				this.dgvItemTable.Rows.Add();
                this.dgvItemTable.Rows[testItemIndex].Cells[0].Value = item.IsEnable;
                this.dgvItemTable.Rows[testItemIndex].Cells[1].Value = item.TestGroupCtrl[0];
                this.dgvItemTable.Rows[testItemIndex].Cells[2].Value = item.TestGroupCtrl[1];
                this.dgvItemTable.Rows[testItemIndex].Cells[3].Value = item.TestGroupCtrl[2];

				for (int j = 0; j < setRowData.Length; j++)
				{
					this.dgvItemTable.Rows[testItemIndex].Cells[j + 4].Value = setRowData[j];
				}

                this.dgvItemTable.Rows[testItemIndex].HeaderCell.Value = (testItemIndex + 1).ToString();	// 1-base 

                if ((testItemIndex % 2) == 0)
				{
                    this.dgvItemTable.Rows[testItemIndex].DefaultCellStyle.BackColor = Color.White;
				}
				else
				{
                    this.dgvItemTable.Rows[testItemIndex].DefaultCellStyle.BackColor = Color.WhiteSmoke;
                }

                #endregion

                //--------------------------------------------------------------------------------
                // Update ESD Show Judge IR
                //--------------------------------------------------------------------------------
                if (item is ESDTestItem)
                {
                    for (int i = 0; i < item.MsrtResult.Length; i++)
                    {
                        //item.MsrtResult[i].IsVision = false;

                        //item.MsrtResult[i].IsEnable = (item as ESDTestItem).IsEnableJudgeItem;

                        item.MsrtResult[i].IsEnable |= (item as ESDTestItem).IsEnableJudgeItem;//為避免忘記關掉ESD_IR造成輸出錯誤，沒關掉ESD_IR時ESD強制啟動
                    }
                }

                //--------------------------------------------------------------------------------
				// Update Msrt Items Table data
				//--------------------------------------------------------------------------------
				if (item.IsEnable == true && item.MsrtResult != null)
				{
					for (int i = 0; i < item.MsrtResult.Length; i++)
					{
                        if (item.MsrtResult[i].IsVision == false)
							continue;

						if (item.MsrtResult[i].KeyName.IndexOf('_') > 0)
						{
							msrtKeyName = item.MsrtResult[i].KeyName.Remove(item.MsrtResult[i].KeyName.IndexOf('_'));
						}

						this.dgvMsrtItem.Rows.Add();

                        this.dgvMsrtItem.Rows[msrtRowIndex].Cells[0].Value = (item.Order +1).ToString();
						this.dgvMsrtItem.Rows[msrtRowIndex].Cells[1].Value = item.MsrtResult[i].IsEnable;
						this.dgvMsrtItem.Rows[msrtRowIndex].Cells[2].Value = item.MsrtResult[i].IsVerify;
						this.dgvMsrtItem.Rows[msrtRowIndex].Cells[3].Value = item.MsrtResult[i].IsSkip;

                        this.dgvMsrtItem2.Rows.Add();
                        this.dgvMsrtItem2.Rows[msrtRowIndex].Cells[0].Value = item.MsrtResult[i].IsEnable;
                        this.dgvMsrtItem2.Rows[msrtRowIndex].Cells[1].Value = item.MsrtResult[i].IsVerify;
                        this.dgvMsrtItem2.Rows[msrtRowIndex].Cells[2].Value = item.MsrtResult[i].IsSkip;


                        this.dgvBoundaryRule.Rows.Add();
                        this.dgvBoundaryRule.Rows[msrtRowIndex].Cells[0].Value = (item.Order + 1).ToString();
                        this.dgvBoundaryRule.Rows[msrtRowIndex].Cells[1].Value = item.MsrtResult[i].Name;
                        (this.dgvBoundaryRule.Rows[msrtRowIndex].Cells[2]as DataGridViewComboBoxCell).Value
                             = (this.dgvBoundaryRule.Rows[msrtRowIndex].Cells[2] as DataGridViewComboBoxCell).Items[(int)item.MsrtResult[i].BoundaryRule];        

                        //if ( msrtKeyName == "LOP" )
                        //{
                        //    this.dgvMsrtItem.Rows[msrtRowIndex].Cells[4].Value = item.MsrtResult[i].Name + "(mcd)";
                        //    this.dgvMsrtItem2.Rows[msrtRowIndex].Cells[3].Value = item.MsrtResult[i].Name + "(mcd)";
                        //}
                        //else if ( msrtKeyName == "WATT" )
                        //{
                        //    this.dgvMsrtItem.Rows[msrtRowIndex].Cells[4].Value = item.MsrtResult[i].Name + "(mW)";
                        //    this.dgvMsrtItem2.Rows[msrtRowIndex].Cells[3].Value = item.MsrtResult[i].Name + "(mW)";						
                        //}
                        //else if (msrtKeyName == "LM")
                        //{
                        //    this.dgvMsrtItem.Rows[msrtRowIndex].Cells[4].Value = item.MsrtResult[i].Name + "(lm)";
                        //    this.dgvMsrtItem2.Rows[msrtRowIndex].Cells[3].Value = item.MsrtResult[i].Name + "(mW)";						
                        //}
                        //else
                        //{
                        //    this.dgvMsrtItem.Rows[msrtRowIndex].Cells[4].Value = item.MsrtResult[i].Name;
                        //    this.dgvMsrtItem2.Rows[msrtRowIndex].Cells[3].Value = item.MsrtResult[i].Name;
                        //}
                        this.dgvMsrtItem.Rows[msrtRowIndex].Cells[4].Value = item.MsrtResult[i].Name;
                        this.dgvMsrtItem2.Rows[msrtRowIndex].Cells[3].Value = item.MsrtResult[i].Name;


						this.dgvMsrtItem.Rows[msrtRowIndex].Cells[5].Value = Convert.ToDouble(item.MsrtResult[i].MinLimitValue); //.ToString(item.MsrtResult[i].Formate));
						this.dgvMsrtItem.Rows[msrtRowIndex].Cells[6].Value = Convert.ToDouble(item.MsrtResult[i].MaxLimitValue); //.ToString(item.MsrtResult[i].Formate));
                        this.dgvMsrtItem2.Rows[msrtRowIndex].Cells[4].Value = Convert.ToDouble(item.MsrtResult[i].MinLimitValue2); //.ToString(item.MsrtResult[i].Formate));
                        this.dgvMsrtItem2.Rows[msrtRowIndex].Cells[5].Value = Convert.ToDouble(item.MsrtResult[i].MaxLimitValue2); //.ToString(item.MsrtResult[i].Formate));

                        ////////////////////////////////////////////////////////////////////////////
                        string unitName = item.MsrtResult[i].Unit;
                        DataGridViewComboBoxCell Dvcomboxcell = (DataGridViewComboBoxCell)this.dgvMsrtItem.Rows[msrtRowIndex].Cells["colUnit"];
                        Dvcomboxcell.Items.Clear();
                        List<string> unitList = UnitMath.GetUnitList(item.MsrtResult[i].Unit);
                        foreach (string unitStr in unitList)
                        {
                            Dvcomboxcell.Items.Add(unitStr);
                        }
                        if (Dvcomboxcell.Items.Contains(unitName))
                        {
                            Dvcomboxcell.Value = unitName;

                        }
                        ////////////////////////////////////////////////

						if ((resultItemIndex % 2) == 0)
						{
							this.dgvMsrtItem.Rows[msrtRowIndex].DefaultCellStyle.BackColor = Color.AliceBlue;
						}
						else
						{
							this.dgvMsrtItem.Rows[msrtRowIndex].DefaultCellStyle.BackColor = Color.Ivory;
						}
						msrtRowIndex++;
					}
					resultItemIndex++;
				}
                testItemIndex++;
			}

            _isRefreshEnd = true;
            //paul
			this.dgvItemTable.AllowUserToAddRows = false;
			this.dgvMsrtItem.AllowUserToAddRows = false;

            //if (customerFrm != null)
            //{
            //    customerFrm.Reftesh();
            //}

			this.dgvItemTable.Update();
			this.dgvMsrtItem.Update();

			this.dgvItemTable.Refresh();
			this.dgvMsrtItem.Refresh();
		}

		private void ReCheckItemEnableForDevice()
		{
            if (DataCenter._product.TestCondition == null || DataCenter._product.TestCondition.TestItemArray == null)
                return;

			foreach (TestItemData item in DataCenter._product.TestCondition.TestItemArray)
			{
				switch (item.Type)
				{ 
					case ETestType.ESD :
						if (DataCenter._machineConfig.ESDModel == EESDModel.NONE)
						{
                            item.IsDeviceSetEnable = false;
						}
						break;
                    //--------------------------------------------------------------------------------------------
					case ETestType.LOP:
                        if (DataCenter._machineConfig.PDSensingMode == EPDSensingMode.NONE)
                        {
                            item.IsDeviceSetEnable = false;
                        }
                        break;
					//--------------------------------------------------------------------------------------------
                    case ETestType.LOPWL:
                        {
                            if (DataCenter._machineConfig.SpectrometerModel == ESpectrometerModel.NONE)
                            {
                                item.IsDeviceSetEnable = false;
                            }

                            break;
                        }
                    case ETestType.LIV:

                        if ((item as LIVTestItem).LIVIsTestOptical && DataCenter._machineConfig.SpectrometerModel == ESpectrometerModel.NONE)
                        {
                            item.IsDeviceSetEnable = false;
                        }


                        if ((item as LIVTestItem).LIVIsEnableDetector && DataCenter._machineConfig.PDSensingMode == EPDSensingMode.NONE)
                        {
                            item.IsDeviceSetEnable = false;
                        }

						break;
					//--------------------------------------------------------------------------------------------
                    case ETestType.LCR:
                    case ETestType.LCRSWEEP:
                        if (DataCenter._machineConfig.LCRModel == ELCRModel.NONE ||DataCenter._machineConfig.TesterFunctionType != ETesterFunctionType.Single_Die)
                        {
                            item.IsDeviceSetEnable = false;
                        }
                        break;
                    //--------------------------------------------------------------------------------------------
					default :
						if (DataCenter._machineConfig.SourceMeterModel == ESourceMeterModel.NONE)
						{
                            item.IsDeviceSetEnable = false;
						}
						break;
				}

                if (DataCenter._machineConfig.SwitchSystemModel != ESwitchSystemModel.NONE)
                {
                    if (item.SwitchingChannel >= DataCenter._machineInfo.MaxSwitchingChannelCount)
                    {
                        item.IsUserSetEnable = false;
                    }
                }

			}
		}

        private void IncludeChannelConditionFrm()
        {
            FormAgent.ChannelCondition.TopLevel = false;
            FormAgent.ChannelCondition.Parent = this.pnlChannelCondi;
            FormAgent.ChannelCondition.Dock = DockStyle.Top;
            FormAgent.ChannelCondition.FormBorderStyle = FormBorderStyle.None;

            FormAgent.ChannelCondition.SetFormSize(DataCenter._machineConfig.TesterFunctionType);

            FormAgent.ChannelCondition.Show();

            switch (DataCenter._machineConfig.TesterFunctionType)
            {
                case ETesterFunctionType.Single_Die:
                case ETesterFunctionType.Multi_Terminal:
                    this.pnlChannelCondi.Visible = false;
                    break;
                case ETesterFunctionType.Multi_Die:

                   // this.dgvItemTable.Size = new Size(715, 458);
                    //this.pnlChannelCondi.Location = new Point(3, 530);
                    //this.pnlChannelCondi.Size = new Size(715, 290);

                    break;
                case ETesterFunctionType.Multi_Pad:

                 // this.dgvItemTable.Size = new Size(715, 388);
                    //this.pnlChannelCondi.Location = new Point(3, 460);
                    //this.pnlChannelCondi.Size = new Size(715, 360);

                    break;
                default:
                    this.pnlChannelCondi.Visible = false;
                    break;
            }
        }

        private void AutoPopDaliyCheckUI()
        {
            DataCenter._uiSetting.IsRunDailyCheckMode = false;
            DataCenter._uiSetting.IsShowDailyCheckUI = false;

            if (this._frmDailyWath == null || this._frmDailyWath.IsDisposed)
            {
                this._frmDailyWath = new frmDailyVerify();
                this._frmDailyWath.ShowDialog();
            }
            else
            {
                if (!this._frmDailyWath.isAlreadyShow)
                {
                    this._frmDailyWath.ShowDialog();
                }
            }

        }

        private void SetChannelStatusUI(int colMax, int rowMax)
        {
            Button[,] channelButton;

            int width = 48;
            int pitch = 10;
            int fontSize = 16;
            int count = 0;

            this.pnlDutChDisplay.Controls.Clear();

            channelButton = new Button[colMax, rowMax];

            int pnlHight = this.pnlDutChDisplay.Size.Height;

            int pnlWidth = this.pnlDutChDisplay.Size.Width;

            // 計算 Location & Size
            int locationX = (int)(((double)pnlWidth / 2) - (((double)colMax / 2) * (width + pitch)));

            int locationY = (int)(((double)pnlHight / 2) - (((double)rowMax / 2) * (width + pitch)));

            locationX = locationX > 0 ? locationX : 0;

            locationY = locationY > 0 ? locationY : 0;

            if (rowMax * (width + pitch) > pnlHight || colMax * (width + pitch) > pnlWidth)
            {
                double ratio = (double)pnlHight / (rowMax * (width + pitch));

                width = (int)(ratio * width);

                pitch = (int)(ratio * pitch);

                fontSize = (int)(ratio * fontSize);
            }

            // 畫出 物件

            for (int row = 0; row < rowMax; row++)
            {
                for (int col = 0; col < colMax; col++)
                {
                    channelButton[col, row] = new Button();

                    channelButton[col, row].Left = locationX + col * (width + pitch);
                    channelButton[col, row].Top = locationY / 2 + row * (width + pitch);
                    channelButton[col, row].Name = "btnChannel" + count.ToString();

                  //  this._channelStatusTip.SetToolTip(channelButton[col, row], "Click to Enable / Disable");

                  //  channelButton[col, row].Click += new EventHandler(ChannelStatusChangeEvent);

                    channelButton[col, row].Width = width;
                    channelButton[col, row].Height = width;

                    channelButton[col, row].Text = (count + 1).ToString();
                    channelButton[col, row].TextAlign = ContentAlignment.MiddleCenter;
                    channelButton[col, row].Font = new Font("Verdana", fontSize, FontStyle.Bold);

                    channelButton[col, row].BackColor = Color.LightGreen;

                    if (DataCenter._product.TestCondition.ChannelConditionTable.Channels.Length > count)
                    {
                        if (!DataCenter._product.TestCondition.ChannelConditionTable.Channels[count].IsEnable)
                        {
                            channelButton[col, row].BackColor = Color.LightGray;
                        }
                    }
                    this.pnlDutChDisplay.Controls.Add(channelButton[col, row]);

                    count++;
                }
            }
        }

        public void ConditionTableCtrl(EBtnActionMode mode, int selectedIndex)
        {
            switch (mode)
            {
                case EBtnActionMode.NewTestItem:
                case EBtnActionMode.InsertTestItem:
                case EBtnActionMode.UpdateTestItem:
                case EBtnActionMode.CopyTestItem:
                    {
                        if (!FormAgent.ConditionItemSetting.DialogControl(mode, selectedIndex))
                        {
                            return;
                        }

                        DataCenter._conditionCtrl.ResetLOPVisionProperty(DataCenter._product.LOPSaveItem);

                        break;
                    }
                case EBtnActionMode.DeleteTestItem:
                    {
                        DataCenter._conditionCtrl.RemoveTestItemAt(selectedIndex);
                        
                        break;
                    }
                case EBtnActionMode.ChangeTestItemOrder_Up:
                    {
                        DataCenter._conditionCtrl.ChangeOrder(selectedIndex, -1);

                        break;
                    }
                case EBtnActionMode.ChangeTestItemOrder_Down:
                    {
                        DataCenter._conditionCtrl.ChangeOrder(selectedIndex, 1);

                        break;
                    }
                case EBtnActionMode.ChangeTestItemEnable:
                    {
                        if (!DataCenter._uiSetting.IsEnableConditionFormEnCheckBox)
                        {
                            return;
                        }
                        
                        TestItemData item = DataCenter._product.TestCondition.TestItemArray[selectedIndex];


                        DataCenter._conditionCtrl.UpdateTestItem(selectedIndex, item);
                        //item.IsEnable = !item.IsEnable;

                        // 20160322 先拿掉, Fire_TestItemDataChangeEvent 也會執行 SaveProductFile()
                        // DataCenter.SaveProductFile();   
                        
                        break;
                    }
            }

            DataCenter.ConfirmTestItemIsEnable();

            this.Fire_TestItemDataChangeEvent();
        }

        private void UpdateItemTableLayout()
        {
            if (!DataCenter._uiSetting.UserDefinedData.IsShowEnableTestGroup)
            {
                DataCenter._sysSetting.IsEnableTestGroup = false;
            }


            if (DataCenter._uiSetting.UserDefinedData.IsShowEnableTestGroup & DataCenter._product.CustomerizedSetting.IsEnableTestGroup)
            //if (DataCenter._uiSetting.UserDefinedData.IsShowEnableTestGroup & DataCenter._sysSetting.IsEnableTestGroup)
            {
                this.dgvItemTable.Columns["colGroup1"].Visible = true;

                this.dgvItemTable.Columns["colGroup2"].Visible = true;

                this.dgvItemTable.Columns["colGroup3"].Visible = false;

                this.dgvItemTable.Columns["colTestItemEnable"].Width = 30;

                this.dgvItemTable.Columns["colGroup1"].Width = 30;

                this.dgvItemTable.Columns["colGroup2"].Width = 30;

                this.dgvItemTable.Columns["colGroup3"].Width = 30;

                this.dgvItemTable.Columns["colChannel"].Width = 30;

                this.dgvItemTable.Columns["colItemName"].Width = 100;

                this.dgvItemTable.Columns["colForceValue"].Width =121;

                this.dgvItemTable.Columns["colForceTime"].Width = 145;

                this.dgvItemTable.Columns["colMsrtRange"].Width = 80;

                this.dgvItemTable.Columns["colOthers"].Width = 80;
            }
            else
            {
                this.dgvItemTable.Columns[1].Visible = false;

                this.dgvItemTable.Columns[2].Visible = false;

                this.dgvItemTable.Columns[3].Visible = false;

                this.dgvItemTable.Columns["colTestItemEnable"].Width = 30;

                this.dgvItemTable.Columns["colGroup1"].Width = 0;

                this.dgvItemTable.Columns["colGroup2"].Width = 0;

                this.dgvItemTable.Columns["colGroup3"].Width = 0;

                this.dgvItemTable.Columns["colChannel"].Width = 0;

                this.dgvItemTable.Columns["colItemName"].Width = 100;

                this.dgvItemTable.Columns["colForceValue"].Width = 121;

                this.dgvItemTable.Columns["colForceTime"].Width = 140;

                this.dgvItemTable.Columns["colMsrtRange"].Width = 100;

                this.dgvItemTable.Columns["colOthers"].Width = 150;
        
            }
        }

        private void SetCustomerUI()
        {

            List<EUserID> enableIdList = new List<EUserID>();
            enableIdList.Add(EUserID.DOWA);
            enableIdList.Add(EUserID.Accelink);

            switch (DataCenter._uiSetting.UserID)
            {
                case EUserID.WAVETEK00:
                    {
                        customerFrm = new frmWAVETEC00Condition();  
                    }
                    break;
                case EUserID.Accelink:
                    {
                        customerFrm = new frmAccelinkCondition();
                    }
                    break;
                default:
                    {
                        tabCustomer.Visible = false;
                    }
                    break;
            }

            if (enableIdList.Contains(DataCenter._uiSetting.UserID))
            {
                tabCustomer.Visible = true;

                (customerFrm as Form).TopLevel = false;

                tabControlPanel6.Controls.Add((customerFrm as Form));

                (customerFrm as Form).Dock = DockStyle.Fill;

                (customerFrm as Form).Size = tabControlPanel6.Size;

                (customerFrm as Form).Show();

                tabcCond.SelectedTabIndex = 4;
            }
            //tabControlPanel6
        }

        private void SetLCRUI()
        {
            //_frmLcrCali
            //tabControlPanel8

            tabLCR.Visible = false;
            btnLCRCali.Visible = false;
            if ( DataCenter._sysSetting.SpecCtrl.IsSupportedLCRItem &&
                DataCenter._machineConfig.LCRModel != ELCRModel.NONE)
            {
                switch (DataCenter._uiSetting.UserID)
                {
                    case EUserID.Emcore:
                    case EUserID.CHT:
                    case EUserID.TYNTE:
                    case EUserID.EpiStar:
                        {
                            tabLCR.Visible = false;
                            btnLCRCali.Visible = true;
                        }
                        break;
                    default:
                        {
                            if (
                                DataCenter._sysSetting.SpecCtrl.ItemDescription != null && 
                                DataCenter._sysSetting.SpecCtrl.ItemDescription.ContainsKeyName(ETestType.LCR.ToString()) )
                            {
                                
                                tabLCR.Visible = true;
                                btnLCRCali.Visible = false;

                                if (_frmLcrCali == null)
                                {
                                    TestItemDescription description = DataCenter._sysSetting.SpecCtrl.ItemDescription[ETestType.LCR.ToString()];
                                    this._frmLcrCali = new frmLCRCaliVer2(description);
                                    _frmLcrCali.TopLevel = false;
                                    tabControlPanel8.Controls.Add(_frmLcrCali as Form);
                                    _frmLcrCali.Dock = DockStyle.Fill;
                                    _frmLcrCali.Show();
                                }
                            }

                        }
                        break;
                }
            }
        }
		#endregion

		#region >>> Public Method <<<

		public void Fire_TestItemDataChangeEvent()
		{
			if (this.TestItemDataChangeEvent != null)
			{
				this.TestItemDataChangeEvent(new object(), new EventArgs());
			}
		}

		//------------------------------------------------------------------------
		// Parameters of condition will havee add, insert, remove, and update
		// action. These actions will fire event, then call this function
		// to update data to this form.
		//-------------------------------------------------------------------------
		public void UpdateDataToUIForm()
		{
			if (this.InvokeRequired && this.IsHandleCreated)
			{
				this.BeginInvoke(new UpdateDataHandler(UpdateDataToControls), null);		// Run at other TestServer Thread
			}
			else if (this.IsHandleCreated)
			{
				this.UpdateDataToControls();			// Run at Main Thread
			}
		}

		#endregion

		#region >>> UI Event Handler <<<

		private void frmCondition_Load( object sender, EventArgs e )
		{
            this.UpdateDataToControls();

			// ----------------------------------------------------------------------------
			// For a Form class, instantiation of an instance (via the constructor), form loading and form visibility are 
			// three different things, and don't need to happen at the same time.
			// Form.Show() method will trigger Form_Load event and Form_Show event at firts time display. 
			//-----------------------------------------------------------------------------
            //FormAgent.DataSettingForm.Show();
            //FormAgent.DataSettingForm.Hide();
		}
		
		private void btnConfirm_Click( object sender, EventArgs e )
		{
            UILog.Log(this, sender, "btnConfirm_Click");

            if (this.chkEnableAllMsrtItem.Checked)
            {
                for (int i = 0; i < this.dgvMsrtItem.Rows.Count; i++)
                {
                    this.dgvMsrtItem[1, i].Value = true;
                    this.dgvMsrtItem2[1, i].Value = true;
                }
            }

			if (DataCenter._product.TestCondition == null || DataCenter._product.TestCondition.TestItemArray == null)
				return;

            int row = 0;
            TestResultData[] resultArray = DataCenter._conditionCtrl.MsrtResultArray;

            TestItemData[] testItems = DataCenter._product.TestCondition.TestItemArray;

            foreach (TestItemData item in testItems)
            {
                item.IsNewCreateItem = false;
                if (item.IsEnable == true && item.MsrtResult != null)
                {
                    for (int i = 0; i < item.MsrtResult.Length; i++)
                    {
                        if (item.MsrtResult[i].IsVision == false)
                            continue;

                        item.MsrtResult[i].IsEnable = (bool)this.dgvMsrtItem[1, row].Value;
                        item.MsrtResult[i].IsVerify = (bool)this.dgvMsrtItem[2, row].Value;

                        if (item.Type != ETestType.LOPWL)
                        {
                            item.MsrtResult[i].IsSkip = (bool)this.dgvMsrtItem[3, row].Value;
                        }

                        item.MsrtResult[i].Name = this.dgvMsrtItem[4, row].Value.ToString();
                        if (item.GainOffsetSetting != null)
                        {
                            foreach (var gItem in item.GainOffsetSetting)
                            {
                                if (gItem.KeyName == item.MsrtResult[i].KeyName)
                                {
                                    gItem.Name = item.MsrtResult[i].Name;
                                }
                            }
                        }

                        //item.MsrtResult[i].Name = (string)this.dgvMsrtItem[3, row].Value;
                        item.MsrtResult[i].MinLimitValue = Convert.ToDouble(this.dgvMsrtItem[5, row].Value);
                        item.MsrtResult[i].MaxLimitValue = Convert.ToDouble(this.dgvMsrtItem[6, row].Value);
                        if (DataCenter._sysSetting.IsSpecBinTableSync)
                        {
                            foreach (var ngB in DataCenter._smartBinning.NGBin)
                            {
                                if (ngB.KeyName == item.MsrtResult[i].KeyName)
                                {
                                    ngB.NGUpLimit = item.MsrtResult[i].MaxLimitValue;
                                    ngB.NGLowLimit = item.MsrtResult[i].MinLimitValue;
                                    ngB.IsEnable = item.MsrtResult[i].IsVerify;
                                }
                            }
                        }

                        if (this.dgvMsrtItem["colUnit", row].Value != null)
                        {
                            item.MsrtResult[i].Unit = this.dgvMsrtItem["colUnit", row].Value.ToString();
                        }
                        else
                        {
                            item.MsrtResult[i].Unit = "";
                        }

                        // LOPWL is not allow to use skip function 
                        if (item.MsrtResult[i].IsSkip == true)
                        {
                            item.MsrtResult[i].IsVerify = true;
                        }
                        
                        item.MsrtResult[i].MinLimitValue2 = Convert.ToDouble(this.dgvMsrtItem2[4, row].Value);
                        item.MsrtResult[i].MaxLimitValue2 = Convert.ToDouble(this.dgvMsrtItem2[5, row].Value);

                        int selectedIndex = (dgvBoundaryRule.Columns[2] as DataGridViewComboBoxColumn).Items.IndexOf
(dgvBoundaryRule.Rows[row].Cells[2].Value);

                        item.MsrtResult[i].BoundaryRule = (EBinBoundaryRule)selectedIndex;
                        this.dgvBoundaryRule.Rows.Add();

                        //if (item.MsrtResult[i].IsSkip == true)
                        //{
                        //    item.MsrtResult[i].IsVerify = true;
                        //}

                        row++;
                    }
                }
            }

           // DataCenter._product.TestCondition.ChipPolarity = (EPolarity)Enum.Parse(typeof(EPolarity), this.cmbPolarity.SelectedItem.ToString(), true);

            //DataCenter._product.ProductFilterWheelPos = (uint)this.cmbFilterPosition.SelectedIndex;

            DataCenter._conditionCtrl.AutoOrderMsrtItemIndex();

            DataCenter._conditionCtrl.AutoRenameMsrtItemAndGianItem();

            DataCenter._conditionCtrl.CopyTestItemArrayToEachChannel();

            if (customerFrm != null)
            {
                customerFrm.Save();
            }
			// Fire event
            DataCenter.SaveProductFile();

            if (DataCenter._sysSetting.IsSpecBinTableSync)
            {
                DataCenter.SaveBinFile();
            }


			this.Fire_TestItemDataChangeEvent();
		}
		
		private void btnNewItem_Click( object sender, EventArgs e )
		{
            UILog.Log(this, sender, "btnNewItem_Click");

            this.ConditionTableCtrl(EBtnActionMode.NewTestItem, 0);
		}

		private void btnInsertItem_Click( object sender, EventArgs e )
		{
			if ( this.dgvItemTable.CurrentRow == null )
				return;

            DialogResult result = TopMessageBox.ShowMessage((int)EMessageCode.InsrtTestItem, "Would you insert one new test item? ", "Insert Test Item", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
			
            if ( result != DialogResult.OK )
				return;

            this.ConditionTableCtrl(EBtnActionMode.InsertTestItem, this.dgvItemTable.CurrentRow.Index);
		}

		private void btnUpdateItem_Click( object sender, EventArgs e )
		{
			if (this.dgvItemTable.CurrentRow == null)
				return;

            this.ConditionTableCtrl(EBtnActionMode.UpdateTestItem, this.dgvItemTable.CurrentRow.Index);
		}

		private void btnRemoveItem_Click( object sender, EventArgs e )
		{
			if (this.dgvItemTable.CurrentRow == null)
				return;

            DialogResult result = TopMessageBox.ShowMessage((int)EMessageCode.DeleteTestItem, "Would you delete this test item? ", "Delete Test Item", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
			
            if ( result != DialogResult.OK )
				return;

            this.ConditionTableCtrl(EBtnActionMode.DeleteTestItem, this.dgvItemTable.CurrentRow.Index);
		}

        private void btnCopyTestItem_Click(object sender, EventArgs e)
        {
            if (this.dgvItemTable.CurrentCell == null)
            {
                return;
            }

            this.ConditionTableCtrl(EBtnActionMode.CopyTestItem, this.dgvItemTable.CurrentRow.Index);

            this.dgvItemTable.CurrentCell = this.dgvItemTable[0, this.dgvItemTable.Rows.Count - 1];
        }

		private void dgvMsrtItem_CellEnter(object sender, DataGridViewCellEventArgs e)
		{
            //if ( false == this.dgvMsrtItem.Columns[e.ColumnIndex].ReadOnly )
            //    this.dgvMsrtItem[e.ColumnIndex, e.RowIndex].Style.BackColor = Color.Red;
		}

		private void dgvMsrtItem_CellLeave(object sender, DataGridViewCellEventArgs e)
		{
			this.dgvMsrtItem[e.ColumnIndex, e.RowIndex].Style.BackColor = Color.Empty;
		}

		private void dgvMsrtItem_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
		{
            if (this.dgvMsrtItem.Rows[e.RowIndex].IsNewRow)
                return;

            this.dgvMsrtItem.Rows[e.RowIndex].ErrorText = "";

            double inputValue = 0.0d;
            double minValue = 0.0d;
            double maxValue = 0.0d;

            if (!(e.ColumnIndex==5 ||e.ColumnIndex==6))	// not Min Value or Max Value , the return;
                return;

            if (double.TryParse(e.FormattedValue.ToString(), out inputValue) == false)
            {
                // Gilbert								
                DialogResult Result = MessageBox.Show("Data Formate Error", "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                e.Cancel = true;
                this.dgvMsrtItem.Rows[e.RowIndex].ErrorText = " Data Format Error";
            }


            if (e.ColumnIndex == 5)
            {
                minValue = inputValue;
                maxValue = Convert.ToDouble(this.dgvMsrtItem[6, e.RowIndex].Value);
            }
            else if (e.ColumnIndex == 6)
            {
                minValue = Convert.ToDouble(this.dgvMsrtItem[5, e.RowIndex].Value);
                maxValue = inputValue;
            }

            if (e.ColumnIndex == 5 || e.ColumnIndex == 6)
            {
                if (minValue > maxValue)
                {
                    // Gilbert 
                    DialogResult Result = MessageBox.Show("The value is not correct", "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                    e.Cancel = true;
                    this.dgvMsrtItem.Rows[e.RowIndex].ErrorText = " Data Format Error";
                }
            }			

		}

		private void frmCondition_VisibleChanged(object sender, EventArgs e)
		{
			if (this.Visible)
			{
				this.UpdateDataToControls();
			}
		}

		//private void dgvMsrtItem_DataError(object sender, DataGridViewDataErrorEventArgs e)
		//{
		//    MessageBox.Show(e.Exception.Message );

		//}

		private void dgvItemTable_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			DataGridView dgv = (DataGridView)sender;

			if (e.Button == MouseButtons.Left)
			{
				if ( dgv.CurrentRow == null )
 					return;

				DataGridView.HitTestInfo hit = dgv.HitTest(e.X, e.Y);

				if (hit.RowIndex < 0)
					return;

                this.ConditionTableCtrl(EBtnActionMode.UpdateTestItem, dgv.CurrentRow.Index);
			}
		}

		private void btnUp_Click(object sender, EventArgs e)
		{
			if (this.dgvItemTable.CurrentRow == null)
				return;
			
			int selectRowIndex = this.dgvItemTable.CurrentRow.Index;
			int selectColumnIndex = this.dgvItemTable.CurrentCell.ColumnIndex;

			if (selectRowIndex == 0 )
				return;

            this.ConditionTableCtrl(EBtnActionMode.ChangeTestItemOrder_Up, selectRowIndex);

			this.dgvItemTable.ClearSelection();

			if (selectRowIndex >= 1)
			{
				this.dgvItemTable.CurrentCell = this.dgvItemTable[selectColumnIndex, selectRowIndex - 1];
				this.dgvItemTable.Rows[selectRowIndex - 1].Selected = true;
			}
		}

		private void btnDown_Click(object sender, EventArgs e)
		{
			if (this.dgvItemTable.CurrentRow == null)
				return;

			int selectRowIndex = this.dgvItemTable.CurrentRow.Index;

			int selectColumnIndex = this.dgvItemTable.CurrentCell.ColumnIndex;

			if (selectRowIndex == (this.dgvItemTable.Rows.Count - 1))
				return;

            this.ConditionTableCtrl(EBtnActionMode.ChangeTestItemOrder_Down, selectRowIndex);

			this.dgvItemTable.ClearSelection();

			if ( selectRowIndex < (this.dgvItemTable.Rows.Count - 1))
			{
				this.dgvItemTable.CurrentCell = this.dgvItemTable[selectColumnIndex, selectRowIndex + 1];
				this.dgvItemTable.Rows[selectRowIndex + 1].Selected = true;
			}
		}

		private void dgvItemTable_CellContentClick(object sender, DataGridViewCellEventArgs e)
		{
            if (!DataCenter._uiSetting.IsEnableConditionFormEnCheckBox)
                return;

            if (e.ColumnIndex < 0 || e.ColumnIndex > 3 || e.RowIndex < 0)
				return;

            TestItemData item = DataCenter._product.TestCondition.TestItemArray[e.RowIndex];

            if (e.ColumnIndex == 0)
            {
                item.IsUserSetEnable = !item.IsEnable;
            }
            else
            {
                item.TestGroupCtrl[e.ColumnIndex - 1] = !item.TestGroupCtrl[e.ColumnIndex - 1];
            }


            this.ConditionTableCtrl(EBtnActionMode.ChangeTestItemEnable, e.RowIndex);
		}

		#endregion		

        private void cmbPolarity_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (this.cmbPolarity.SelectedIndex > 0)
            //{
            //    DataCenter._product.TestCondition.ChipPolarity = (EPolarity)Enum.Parse(typeof(EPolarity), this.cmbPolarity.SelectedItem.ToString(), true);

            //    DataCenter._product.ProductFilterWheelPos = (uint)this.cmbFilterPosition.SelectedIndex;
            //}
        }

        private void btnCreateTask_Click(object sender, EventArgs e)
        {
            UILog.Log(this, sender, "btnConfirm_Click");

            if (this.chkEnableAllMsrtItem.Checked)
            {
                for (int i = 0; i < this.dgvMsrtItem.Rows.Count; i++)
                {
                    this.dgvMsrtItem[0, i].Value = true;
                    this.dgvMsrtItem2[0, i].Value = true;
                }
            }

            if (DataCenter._product.TestCondition == null || DataCenter._product.TestCondition.TestItemArray == null)
                return;

            int row = 0;
            TestResultData[] resultArray = DataCenter._conditionCtrl.MsrtResultArray;

            TestItemData[] testItems = DataCenter._product.TestCondition.TestItemArray;

            foreach (TestItemData item in testItems)
            {
                if (item.IsEnable == true && item.MsrtResult != null)
                {
                    for (int i = 0; i < item.MsrtResult.Length; i++)
                    {
                        if (item.MsrtResult[i].IsVision == false)
                            continue;

                        item.MsrtResult[i].IsEnable = (bool)this.dgvMsrtItem[1, row].Value;
                        item.MsrtResult[i].IsVerify = (bool)this.dgvMsrtItem[2, row].Value;

                        if (item.Type != ETestType.LOPWL)
                        {
                            item.MsrtResult[i].IsSkip = (bool)this.dgvMsrtItem[3, row].Value;
                        }

                        item.MsrtResult[i].Name = this.dgvMsrtItem[4, row].Value.ToString();

                        //item.MsrtResult[i].Name = (string)this.dgvMsrtItem[3, row].Value;
                        item.MsrtResult[i].MinLimitValue = Convert.ToDouble(this.dgvMsrtItem[5, row].Value);
                        item.MsrtResult[i].MaxLimitValue = Convert.ToDouble(this.dgvMsrtItem[6, row].Value);

                        // LOPWL is not allow to use skip function 
                        if (item.MsrtResult[i].IsSkip == true)
                        {
                            item.MsrtResult[i].IsVerify = true;
                        }
                        //    spec 2
                        //item.MsrtResult[i].IsEnable = (bool)this.dgvMsrtItem2[0, row].Value;
                        //item.MsrtResult[i].IsVerify = (bool)this.dgvMsrtItem2[1, row].Value;
                        //item.MsrtResult[i].IsSkip = (bool)this.dgvMsrtItem2[2, row].Value;

                        //item.MsrtResult[i].Name = (string)this.dgvMsrtItem[3, row].Value;
                        item.MsrtResult[i].MinLimitValue2 = Convert.ToDouble(this.dgvMsrtItem2[4, row].Value);
                        item.MsrtResult[i].MaxLimitValue2 = Convert.ToDouble(this.dgvMsrtItem2[5, row].Value);

                        //if (item.MsrtResult[i].IsSkip == true)
                        //{
                        //    item.MsrtResult[i].IsVerify = true;
                        //}

                        row++;
                    }
                }
            }

          //  DataCenter._product.TestCondition.ChipPolarity = (EPolarity)Enum.Parse(typeof(EPolarity), this.cmbPolarity.SelectedItem.ToString(), true);

          //  DataCenter._product.ProductFilterWheelPos = (uint)this.cmbFilterPosition.SelectedIndex;

            DataCenter._conditionCtrl.AutoOrderMsrtItemIndex();

            DataCenter._conditionCtrl.AutoRenameMsrtItemAndGianItem();

            DataCenter._conditionCtrl.CopyTestItemArrayToEachChannel();

            // Fire event
            DataCenter.SaveProductFile();

            this.Fire_TestItemDataChangeEvent();
        }

        private void cmbFilterPosition_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void buttonX1_Click(object sender, EventArgs e)
        {
            if (customerFrm != null)
            {
                customerFrm.Save();
            }
            
            DataCenter._product.TestCondition.ChipPolarity = (EPolarity)Enum.Parse(typeof(EPolarity), this.cmbPolarity.SelectedItem.ToString(), true);

            DataCenter._product.ProductFilterWheelPos = (uint)this.cmbFilterPosition.SelectedIndex;

            DataCenter._product.TestCondition.TestStage = (ETestStage)Enum.Parse(typeof(ETestStage), this.cmbStage.SelectedItem.ToString(), true);

            if (DataCenter._product.LOPSaveItem.ToString() != this.cmbLopSaveItem.SelectedItem.ToString())
            {
                DataCenter._product.LOPSaveItem = (ELOPSaveItem)Enum.Parse(typeof(ELOPSaveItem), this.cmbLopSaveItem.SelectedItem.ToString(), true);     // 0-base
                DataCenter._conditionCtrl.ResetLOPVisionProperty(DataCenter._product.LOPSaveItem);

                WMOperate.WM_ReadCalibrateParamFromSetting();
                //Host.UpdateDataToAllUIForm();
            }

            DataCenter._product.CustomerizedSetting.IsEnableTestGroup = this.chkEnableTestGroup.Checked;

            DataCenter._product.IsTVSProduct = this.chkIsTVSTesting.Checked;

            DataCenter._product.PdDetectorFactor = this.dinPdDetectorFactor.Value;

            DataCenter._product.ProductName = this.txtProductType.Text;

            DataCenter.SaveProductFile();

			if (DataCenter._uiSetting.ImportCalibrateFileName != "")
			{
				DataCenter.ExportCalibrateData(DataCenter._uiSetting.ProductPath02, DataCenter._uiSetting.ImportCalibrateFileName + ".cal");
			}


            AppSystem.SetDataToSystem();

            AppSystem.CheckMachineHW();

            AppSystem.Fire_SwitchUIEvent((int)EBaseFormDisplayUI.ConditionForm);

            Host.UpdateDataToAllUIForm();
        }

        private void btnDailyWatch_Click(object sender, EventArgs e)
        {
            UILog.Log(this, sender, "btnDailyWatch_Click");
            //DialogResult result = TopMessageBox.ShowMessage(202, "", "Run Daily Check ? ");
            //if (result!=DialogResult.OK)
            //    return;
            AutoPopDaliyCheckUI();
        }

        private void btnDailyCheckSetting_Click(object sender, EventArgs e)
        {
            if (this._frmDailyCheckSetting == null || this._frmDailyCheckSetting.IsDisposed)
            {
                this._frmDailyCheckSetting = new frmDailyCheckSetting();
                // this._frmDailyCheckSetting.InitialFormType(false);
                this._frmDailyCheckSetting.ShowDialog();
            }
            else
            {
                //   this._frmDailyCheckSetting.InitialFormType(false);
                this._frmDailyCheckSetting.ShowDialog();
            }
        }

        private void btnDailayCheckSpec_Click(object sender, EventArgs e)
        {
            if (this._frmDailyCheckSetting == null || this._frmDailyCheckSetting.IsDisposed)
            {
                this._frmDailyCheckSetting = new frmDailyCheckSetting();
                //    this._frmDailyCheckSetting.InitialFormType(true);
                this._frmDailyCheckSetting.ShowDialog();
            }
            else
            {
                //     this._frmDailyCheckSetting.InitialFormType(true);
                this._frmDailyCheckSetting.ShowDialog();
            }
        }

        private void btnDeviceCheck_Click(object sender, EventArgs e)
        {
            if (this._deviceVerify == null || this._deviceVerify.IsDisposed)
            {
                this._deviceVerify = new frmDeviceVerify();

                this._deviceVerify.ShowDialog();
            }
            else
            {
                this._deviceVerify.ShowDialog();
            }
        }

        private void btnDefaultValueSetting_Click(object sender, EventArgs e)
        {
            frmDefaultValueSetting frmDefaultSetting = new frmDefaultValueSetting();

            frmDefaultSetting.ShowDialog();

            frmDefaultSetting.Dispose();

            frmDefaultSetting.Close();

            Host.UpdateDataToAllUIForm();
        }

        private void btnAutoChannelEnd_Click(object sender, EventArgs e)
        {
            AppSystem._autoCalibChannelGain.End();

            AppSystem.Fire_PopUIEvent((int)EPopUpUIForm.AutoChannelCalibrationForm);
        }

        private void btnAutoChannelStart_Click(object sender, EventArgs e)
        {
            AppSystem._autoCalibChannelGain.Start((uint)DataCenter._machineConfig.ChannelConfig.ChannelCount, DataCenter._product);
        }

        private void btnLCRCali_Click(object sender, EventArgs e)
        {
            TestItemDescription description = DataCenter._sysSetting.SpecCtrl.ItemDescription[ETestType.LCR.ToString()];
            frmLCRCali frmLCRCali = new frmLCRCali(description);
            frmLCRCali.ShowDialog();
            frmLCRCali.Dispose();

            frmLCRCali.Close();
        }

        private void btnOsaCoupling_Click(object sender, EventArgs e)
        {
            frmOsaCoupling frm = new frmOsaCoupling(DataCenter._machineInfo);
            frm.ShowDialog();
            frm.Dispose();
            frm.Close();
        }

        private void btnAttenuator_Click(object sender, EventArgs e)
        {
            TestItemDescription description = DataCenter._sysSetting.SpecCtrl.ItemDescription[ETestType.LaserSource.ToString()];


            Form frmAtt = null;

            //if (DataCenter._uiSetting.UserID == EUserID.Emcore)
            //{
                if (DataCenter._product.LaserSrcSetting.AttenuatorData != null)
                {
                    frmAtt = new UIForm.Dialog.HardWareSetting.frmAttenuator(description,
                        DataCenter._product.LaserSrcSetting.AttenuatorData);
                }
                else
                {
                    frmAtt = new UIForm.Dialog.HardWareSetting.frmAttenuator(description);
                }
            //}
            //else
            //{
            //    frmAtt = new UIForm.Dialog.HardWareSetting.frmOpticalPowerSet();
            //}

            frmAtt.ShowDialog();

            frmAtt.Dispose();

            frmAtt.Close();
        }

        private void dgvMsrtItem_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (this.dgvMsrtItem.Rows != null &&
                e.RowIndex >= 0 && e.RowIndex < this.dgvMsrtItem.Rows.Count)
            {
                if (!_isRefreshEnd || this.dgvMsrtItem.Rows[e.RowIndex].IsNewRow)
                {
                    this.dgvMsrtItem[e.ColumnIndex, e.RowIndex].Style.BackColor = Color.Empty;
                }
                else
                {
                    this.dgvMsrtItem[e.ColumnIndex, e.RowIndex].Style.BackColor = Color.DarkOrange;
                }

            }

        }

        private void btnDeviceRelayInfo_Click(object sender, EventArgs e)
        {
            frmDevRelay frmDevRelay = null;

            if (DataCenter._machineInfo.SNDeviceRelayDic != null)
            {
                frmDevRelay = new UIForm.Dialog.HardWareSetting.frmDevRelay(AppSystem.LoadDevRelayInfo());

                if (frmDevRelay.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    var devLogDic = frmDevRelay.GetDevRelayInfo();
                    if (devLogDic != null)
                    {
                        Dictionary<string, object> nameObj = new Dictionary<string, object>();
                        foreach (var p in devLogDic)
                        {
                            nameObj.Add(p.Key, p.Value);
                        }
                        AppSystem.SaveDevRelayInfo(nameObj);
                    }
                    
                }

                frmDevRelay.Dispose();

                frmDevRelay.Close();
            }


            
        }
	}
}
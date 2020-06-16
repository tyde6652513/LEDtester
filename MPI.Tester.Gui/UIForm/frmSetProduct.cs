using System;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;

using MPI.Tester.Data;
using MPI.Tester.DeviceCommon;

namespace MPI.Tester.Gui
{
	public partial class frmSetProduct : System.Windows.Forms.Form
	{
		private delegate void UpdateDataHandler();
		public event EventHandler<EventArgs> TestItemDataChangeEvent;

		public frmSetProduct()
		{
			Console.WriteLine("[frmSetProduct], frmSetProduct()");

			InitializeComponent();
			this.TestItemDataChangeEvent += new EventHandler<EventArgs>(Host.TestItemDataChangeEventHandler);
			this.InitParamAndCompData();
		}

		#region >>> Private Method <<<

		private void InitParamAndCompData()
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

			this.cmbMonitorCheckMode.Items.Clear();
			string[] monitorCheckMode = new string[] { "Value Type", "Gradient Type" };
			this.cmbMonitorCheckMode.Items.AddRange(monitorCheckMode);

			this.cmbPassRateCheckMode.Items.Clear();
			this.cmbPassRateCheckMode.Items.AddRange(Enum.GetNames(typeof(EPassRateCheckNGMode)));
		}

		private void ChangeAuthority()
		{
			switch (DataCenter._uiSetting.AuthorityLevel)
			{
				case EAuthority.Operator:
				case EAuthority.QC:
					this.Enabled = false;
					break;
				//-------------------------------------------------------------------
				case EAuthority.Engineer:
				case EAuthority.Admin:
				case EAuthority.Super:
					this.Enabled = true;
					break;
				default:
					this.Enabled = false;
					break;
			}
		}

		public void UpdateDataToControls()
		{
			this.cmbPolarity.SelectedItem = DataCenter._product.TestCondition.ChipPolarity.ToString();

			this.cmbFilterPosition.SelectedIndex = (int)DataCenter._product.ProductFilterWheelPos;

			this.dinConsecutiveError.Value = DataCenter._product.AdjacentConsecutiveErrorCount;

			this.dinLopWLSkipCount.Value = DataCenter._product.LOPWLSkipCount;

			this.chkIsTVSTesting.Checked = DataCenter._product.IsTVSProduct;

			this.chkIsAllPassChecking.Checked = DataCenter._product.IsAdjacentAllItemPassCheck;

			this.dinSamplingConsecutiveError.Value = DataCenter._product.SamplingMonitorConsecutiveErrCount;

			this.cmbMonitorCheckMode.SelectedIndex = (int)DataCenter._product.SamplingMonitorMode;

			this.chkIsEnablePassRateCheck.Checked = DataCenter._sysSetting.IsEnablePassRateCheck;

			this.cmbPassRateCheckMode.SelectedItem = DataCenter._sysSetting.PassRateCheckMode.ToString();

			this.dinAdjacentStartingCount.Value = DataCenter._product.AdjacentStartingCount;

            this.dinAdjacentEndCount.Value = DataCenter._product.AdjacentStopCount;

			if (DataCenter._uiSetting.UserDefinedData.IsEnableSkipOptiMsrt)
			{
				this.dinLopWLSkipCount.Visible = true;
				this.lblLopWLSkipCount.Visible = true;
			}
			else
			{
				this.dinLopWLSkipCount.Visible = false;
				this.lblLopWLSkipCount.Visible = false;
			}

			this.cmbLopSaveItem.Items.Clear();
			this.cmbLopSaveItem.Items.AddRange(DataCenter._uiSetting.UserDefinedData.LOPItemSelectList);
			this.cmbLopSaveItem.SelectedItem = DataCenter._product.LOPSaveItem.ToString();

			if (this.cmbLopSaveItem.SelectedIndex == -1)
			{
				this.cmbLopSaveItem.SelectedIndex = 0;
				Host.SetErrorCode(EErrorCode.LOPSaveItemNotMatch);
				DataCenter._product.LOPSaveItem = (ELOPSaveItem)Enum.Parse(typeof(ELOPSaveItem), this.cmbLopSaveItem.SelectedItem.ToString(), true);     // 0-base
				DataCenter._conditionCtrl.ResetLOPVisionProperty(DataCenter._product.LOPSaveItem);
			}

			switch (DataCenter._uiSetting.UIOperateMode)
			{
				case (int)EUIOperateMode.Idle:
					this.ChangeAuthority();
					break;
				//-----------------------------------------------------------------------------
				case (int)EUIOperateMode.AutoRun:
                case (int)EUIOperateMode.ManulRun:
					this.Enabled = false;
					break;
				//-----------------------------------------------------------------------------
                //case (int)EUIOperateMode.ManulRun:
                //    this.ChangeAuthority();
                //    break;
				//-----------------------------------------------------------------------------
				default:
					this.ChangeAuthority();
					break;
			}

			this.UpdateAdjacentAndSamplingSetting();

			if (DataCenter._sysSetting.IsEnableAdjacentError)
			{
				this.lblAdjacentError.Visible = true;
				this.gbAdjacentError.Visible = true;
			}
			else
			{
				this.lblAdjacentError.Visible = true;
				this.gbAdjacentError.Visible = true;
			}


			if (DataCenter._uiSetting.UserDefinedData.IsShowAdjacentChecking)
			{
				gbAdjacentError.Visible = true;
			}
			else
			{
				gbAdjacentError.Visible = false;
			}

			if (DataCenter._uiSetting.UserDefinedData.IsShowPreSamplingChecking)
			{
				gbSamplingMonitor.Visible = true;
			}
			else
			{
				gbSamplingMonitor.Visible = false;
			}

			if (DataCenter._uiSetting.UserDefinedData.IsShowPassRateChecking)
			{
				this.gbPassRateChecking.Visible = true;
			}
			else
			{
				gbPassRateChecking.Visible = false;
			}

			if (DataCenter._machineConfig.TesterFunctionType == ETesterFunctionType.Single_Die ||
                DataCenter._machineConfig.TesterFunctionType == ETesterFunctionType.Multi_Terminal)
			{
				this.lblAdjacentError.Visible = true;

				this.lblChannelError.Visible = false;
			}
			else
			{
				this.lblAdjacentError.Visible = false;

				this.lblChannelError.Visible = true;
			}
		}

		private void SaveDataToFile()
		{
			DataCenter._product.TestCondition.ChipPolarity = (EPolarity)Enum.Parse(typeof(EPolarity), this.cmbPolarity.SelectedItem.ToString(), true);

			DataCenter._product.ProductFilterWheelPos = (uint)this.cmbFilterPosition.SelectedIndex;

			DataCenter._product.AdjacentConsecutiveErrorCount = (uint)this.dinConsecutiveError.Value;

			DataCenter._product.LOPWLSkipCount = (uint)this.dinLopWLSkipCount.Value;

			DataCenter._product.IsAdjacentAllItemPassCheck = this.chkIsAllPassChecking.Checked;

			DataCenter._product.IsTVSProduct = this.chkIsTVSTesting.Checked;

			DataCenter._product.SamplingMonitorMode = (uint)this.cmbMonitorCheckMode.SelectedIndex;

			DataCenter._product.SamplingMonitorConsecutiveErrCount = (uint)this.dinSamplingConsecutiveError.Value;

			DataCenter._sysSetting.IsEnablePassRateCheck = this.chkIsEnablePassRateCheck.Checked;

			DataCenter._sysSetting.PassRateCheckMode = (EPassRateCheckNGMode)Enum.Parse(typeof(EPassRateCheckNGMode), this.cmbPassRateCheckMode.SelectedItem.ToString(), true);

			DataCenter._product.AdjacentStartingCount = (int)this.dinAdjacentStartingCount.Value;

            DataCenter._product.AdjacentStopCount = (int)this.dinAdjacentEndCount.Value;

			if (DataCenter._product.LOPSaveItem.ToString() != this.cmbLopSaveItem.SelectedItem.ToString())
			{
				DataCenter._product.LOPSaveItem = (ELOPSaveItem)Enum.Parse(typeof(ELOPSaveItem), this.cmbLopSaveItem.SelectedItem.ToString(), true);     // 0-base
				DataCenter._conditionCtrl.ResetLOPVisionProperty(DataCenter._product.LOPSaveItem);

				WMOperate.WM_ReadCalibrateParamFromSetting();
				//Host.UpdateDataToAllUIForm();
			}
		}

		public void Fire_TestItemDataChangeEvent()
		{
			if (this.TestItemDataChangeEvent != null)
			{
				this.TestItemDataChangeEvent(new object(), new EventArgs());
			}
		}

		private void UpdateAdjacentAndSamplingSetting()
		{
			string msrtKeyName = string.Empty;

			this.dgvAdjacent.Rows.Clear();

			this.dgvSortPageReTest.Rows.Clear();

			this.dgvSamplingMonitor.Rows.Clear();

			this.dgvPassRateCheck.Rows.Clear();

			if (DataCenter._product.TestCondition == null || DataCenter._product.TestCondition.TestItemArray == null)
			{
				this.dgvAdjacent.Update();
				this.dgvSortPageReTest.Update();
				this.dgvSamplingMonitor.Update();
				return;
			}

			int testItemIndex = 0;
			int resultItemIndex = 0;
			int msrtRowIndex = 0;

			TestItemData[] testItems = DataCenter._product.TestCondition.TestItemArray;
			string[] setRowData = new string[5];
			string[] setRowData2 = new string[5];

			this.dgvAdjacent.AllowUserToAddRows = true;

			foreach (TestItemData item in testItems)
			{
				//if (DataCenter._machineConfig.SourceMeterModel == ESourceMeterModel.N5700 ||
				//    DataCenter._machineConfig.SourceMeterModel == ESourceMeterModel.DR2000 ||
				//    DataCenter._machineConfig.SourceMeterModel == ESourceMeterModel.DSPHD ||
				//    DataCenter._machineConfig.SourceMeterModel == ESourceMeterModel.K2400 ||
				//    DataCenter._machineConfig.SourceMeterModel == ESourceMeterModel.IT7321)
				//{
				//    if (item.ElecSetting != null && item.ElecSetting.Length > 0)
				//    {
				//        item.ElecSetting[0].MsrtRange = item.ElecSetting[0].MsrtProtection;
				//    }
				//}

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

						this.dgvAdjacent.Rows.Add();

						this.dgvAdjacent.Rows[msrtRowIndex].Cells[0].Value = item.MsrtResult[i].EnableAdjacent;

						this.dgvSamplingMonitor.Rows.Add();

						this.dgvSamplingMonitor.Rows[msrtRowIndex].Cells[0].Value = item.MsrtResult[i].EnableSamplingMonitor;

						this.dgvPassRateCheck.Rows.Add();

						this.dgvPassRateCheck.Rows[msrtRowIndex].Cells[0].Value = item.MsrtResult[i].IsEnablePassRateCheck;

						//      this.dgvSortPageReTest.Rows.Add();
						//      this.dgvSortPageReTest.Rows[msrtRowIndex].Cells[0].Value = item.MsrtResult[i].SortPageData.IsEnable;


						if (msrtKeyName == "LOP")
						{
							this.dgvAdjacent.Rows[msrtRowIndex].Cells[1].Value = item.MsrtResult[i].Name + "(mcd)";

							this.dgvSamplingMonitor.Rows[msrtRowIndex].Cells[1].Value = item.MsrtResult[i].Name + "(mcd)";

							this.dgvPassRateCheck.Rows[msrtRowIndex].Cells[1].Value = item.MsrtResult[i].Name + "(mcd)";

							//            this.dgvSortPageReTest.Rows[msrtRowIndex].Cells[1].Value = item.MsrtResult[i].Name + "(mcd)";
						}
						else if (msrtKeyName == "WATT")
						{
							this.dgvAdjacent.Rows[msrtRowIndex].Cells[1].Value = item.MsrtResult[i].Name + "(mW)";

							this.dgvSamplingMonitor.Rows[msrtRowIndex].Cells[1].Value = item.MsrtResult[i].Name + "(mW)";

							this.dgvPassRateCheck.Rows[msrtRowIndex].Cells[1].Value = item.MsrtResult[i].Name + "(mW)";


							//             this.dgvSortPageReTest.Rows[msrtRowIndex].Cells[1].Value = item.MsrtResult[i].Name + "(mW)";
						}
						else if (msrtKeyName == "LM")
						{
							this.dgvAdjacent.Rows[msrtRowIndex].Cells[1].Value = item.MsrtResult[i].Name + "(lm)";

							this.dgvSamplingMonitor.Rows[msrtRowIndex].Cells[1].Value = item.MsrtResult[i].Name + "(lm)";

							this.dgvPassRateCheck.Rows[msrtRowIndex].Cells[1].Value = item.MsrtResult[i].Name + "(lm)";

							//            this.dgvSortPageReTest.Rows[msrtRowIndex].Cells[1].Value = item.MsrtResult[i].Name + "(lm)";
						}
						else
						{
							this.dgvAdjacent.Rows[msrtRowIndex].Cells[1].Value = item.MsrtResult[i].Name;

							this.dgvSamplingMonitor.Rows[msrtRowIndex].Cells[1].Value = item.MsrtResult[i].Name;

							this.dgvPassRateCheck.Rows[msrtRowIndex].Cells[1].Value = item.MsrtResult[i].Name;


							//             this.dgvSortPageReTest.Rows[msrtRowIndex].Cells[1].Value = item.MsrtResult[i].Name;
						}

						this.dgvAdjacent.Rows[msrtRowIndex].Cells[2].Value = Convert.ToDouble(item.MsrtResult[i].AdjacentType); //.ToString(item.MsrtResult[i].Formate));
						this.dgvAdjacent.Rows[msrtRowIndex].Cells[3].Value = Convert.ToDouble(item.MsrtResult[i].AdjacentRange); //.ToString(item.MsrtResult[i].Formate));


						this.dgvSamplingMonitor.Rows[msrtRowIndex].Cells[2].Value = Convert.ToDouble(item.MsrtResult[i].SamplingMonitorType); //.ToString(item.MsrtResult[i].Formate));
						this.dgvSamplingMonitor.Rows[msrtRowIndex].Cells[3].Value = Convert.ToDouble(item.MsrtResult[i].SamplingMonitorRange); //.ToString(item.MsrtResult[i].Formate));


						this.dgvPassRateCheck.Rows[msrtRowIndex].Cells[2].Value = item.MsrtResult[i].MinPassRatePercent;

						//this.dgvSortPageReTest.Rows[msrtRowIndex].Cells[3].Value = Convert.ToInt32(item.MsrtResult[i].SortPageData.Type); //.ToString(item.MsrtResult[i].Formate));
						//this.dgvSortPageReTest.Rows[msrtRowIndex].Cells[3].Value = Convert.ToDouble(item.MsrtResult[i].SortPageData.LowValue); //.ToString(item.MsrtResult[i].Formate));
						//this.dgvSortPageReTest.Rows[msrtRowIndex].Cells[4].Value = Convert.ToDouble(item.MsrtResult[i].SortPageData.HighValue); //.ToString(item.MsrtResult[i].Formate));
						//this.dgvSortPageReTest.Rows[msrtRowIndex].Cells[3].Value = Convert.ToDouble(item.MsrtResult[i].SortPageData.LowExtValue); //.ToString(item.MsrtResult[i].Formate));
						//this.dgvSortPageReTest.Rows[msrtRowIndex].Cells[4].Value = Convert.ToDouble(item.MsrtResult[i].SortPageData.HighExtValue); //.ToString(item.MsrtResult[i].Formate));


						if ((resultItemIndex % 2) == 0)
						{
							this.dgvAdjacent.Rows[msrtRowIndex].DefaultCellStyle.BackColor = Color.WhiteSmoke;

							this.dgvSamplingMonitor.Rows[msrtRowIndex].DefaultCellStyle.BackColor = Color.WhiteSmoke;
							// this.dgvSortPageReTest.Rows[msrtRowIndex].DefaultCellStyle.BackColor = Color.WhiteSmoke;
						}
						else
						{
							this.dgvAdjacent.Rows[msrtRowIndex].DefaultCellStyle.BackColor = Color.White;

							this.dgvSamplingMonitor.Rows[msrtRowIndex].DefaultCellStyle.BackColor = Color.WhiteSmoke;
							// this.dgvSortPageReTest.Rows[msrtRowIndex].DefaultCellStyle.BackColor = Color.WhiteSmoke;
						}

						//if (msrtKeyName == "WATT" || msrtKeyName == "LOP" || msrtKeyName == "LM" || msrtKeyName == "WLD" || msrtKeyName == "MVF")
						//{
						//   this.dgvAdjacent.Rows[msrtRowIndex].Visible = true;
						//  //  this.dgvSortPageReTest.Rows[msrtRowIndex].Visible = true;
						//}
						//else
						//{
						//   this.dgvAdjacent.Rows[msrtRowIndex].Visible = false;
						// //   this.dgvSortPageReTest.Rows[msrtRowIndex].Visible = false;
						//}

						msrtRowIndex++;
					}
					resultItemIndex++;
				}
				testItemIndex++;
			}

			this.dgvAdjacent.AllowUserToAddRows = false;
			this.dgvAdjacent.Update();
			this.dgvAdjacent.Refresh();

			this.dgvSamplingMonitor.AllowUserToAddRows = false;
			this.dgvSamplingMonitor.Update();
			this.dgvSamplingMonitor.Refresh();


			this.dgvPassRateCheck.AllowUserToAddRows = false;
			this.dgvPassRateCheck.Update();
			this.dgvPassRateCheck.Refresh();

		}


		private void SetAdjacentAndSamplingSetting()
		{
			//this.UpdateAdjacentSetting();

			if (DataCenter._product.TestCondition.TestItemArray == null)
				return;

			int row = 0;
			TestItemData[] testItems = DataCenter._product.TestCondition.TestItemArray;

			List<string> data = new List<string>();

			foreach (TestItemData item in testItems)
			{
				if (item.IsEnable == true && item.MsrtResult != null)
				{
					for (int i = 0; i < item.MsrtResult.Length; i++)
					{
						if (item.MsrtResult[i].IsVision == false)
							continue;

						data.Add(item.MsrtResult[i].KeyName);

						//if (item.MsrtResult[i].KeyName == "WATT" || item.MsrtResult[i].KeyName == "LOP" || item.MsrtResult[i].KeyName == "LM" || item.MsrtResult[i].KeyName == "WLD" || item.MsrtResult[i].KeyName == "MVF")
						//{
						//    continue; ;
						//}

						item.MsrtResult[i].EnableAdjacent = (bool)this.dgvAdjacent[0, row].Value;

						item.MsrtResult[i].AdjacentType = Convert.ToInt32(this.dgvAdjacent[2, row].Value);

						item.MsrtResult[i].AdjacentRange = Convert.ToDouble(this.dgvAdjacent[3, row].Value);


						item.MsrtResult[i].EnableSamplingMonitor = (bool)this.dgvSamplingMonitor[0, row].Value;

						item.MsrtResult[i].SamplingMonitorType = Convert.ToInt32(this.dgvSamplingMonitor[2, row].Value);

						item.MsrtResult[i].SamplingMonitorRange = Convert.ToDouble(this.dgvSamplingMonitor[3, row].Value);


						item.MsrtResult[i].IsEnablePassRateCheck = (bool)this.dgvPassRateCheck[0, row].Value;

						item.MsrtResult[i].MinPassRatePercent = Convert.ToInt32(this.dgvPassRateCheck[2, row].Value);

						row++;
					}

				}
			}

			// Fire event
			// this.Fire_TestItemDataChangeEvent();
		}

		#endregion

		#region >>> Public Method <<<

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

		private void frmSetProduct_Load(object sender, EventArgs e)
		{
			//this.UpdateDataToControls();
		}

		private void frmSetProduct_VisibleChanged(object sender, EventArgs e)
		{
			if (this.Visible == true)
			{
				this.UpdateDataToControls();
			}
		}

		private void btnSave_Click(object sender, EventArgs e)
		{
			this.SetAdjacentAndSamplingSetting();
			this.SaveDataToFile();
			this.UpdateAdjacentAndSamplingSetting();
			this.SetAdjacentAndSamplingSetting();

			if (DataCenter._uiSetting.ImportCalibrateFileName != "")
			{
				DataCenter.ExportCalibrateData(DataCenter._uiSetting.ProductPath02, DataCenter._uiSetting.ImportCalibrateFileName + ".cal");
			}

			// Host._MPIStorage.SaveReportHeadToFile();
			//  AppSystem.SetDataToSystem();
			DataCenter.SaveProductFile();
			//   AppSystem.SetDataToSystem();
			Fire_TestItemDataChangeEvent();
		}

		private void btnWaferMapSetting_Click(object sender, EventArgs e)
		{
			frmWaferMapSetting frmWaferMapSetting = new frmWaferMapSetting();

			frmWaferMapSetting.ShowDialog();

			frmWaferMapSetting.Dispose();

			frmWaferMapSetting.Close();

			Host.UpdateDataToAllUIForm();
		}

		#endregion
	}
}

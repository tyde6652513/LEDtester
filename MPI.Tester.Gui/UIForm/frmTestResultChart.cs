using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using MPI.Tester.Data;
using MPI.Windows.Forms;
using MPI.Tester.Data;
using MPI.Tester.TestKernel;

namespace MPI.Tester.Gui
{  
    public partial class frmTestResultChart : Form
    {
        private Dictionary<string, frmPopWaferMap> PopWaferFormBag=new Dictionary<string,frmPopWaferMap>(); //Bag 袋/ 包

        private readonly Point[] multiMapArrangePosition = new Point[] { new Point(0, 1), new Point(320, 1), new Point(0, 358), new Point(320, 358) };

        private BinGradeColorSet _colorBagSet; // Color setting 的DB

        private Dictionary<string, string> _dicEnableWaferMap;

        private bool _isCreateForm = false;

        private int _dataFormWith = 0;

        private string _parentForm = string.Empty;

        private int _smallPanelWidth = 660;

        private int _bigPanelWidth = 850;

        private const int CP_NOCLOSE_BUTTON = 0x200;

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams myCp = base.CreateParams;
                myCp.ClassStyle = myCp.ClassStyle | CP_NOCLOSE_BUTTON;
                return myCp;
            }
        } 

        public frmTestResultChart()
        {
            InitializeComponent();

           AppSystem.OnAppSystemRun += new EventHandler(this.AppSystem_OnAppSystemRun);

            Host.OnTestItemChangeEvent += new EventHandler(this.OnRefreshMapSetting);

            _dicEnableWaferMap = new Dictionary<string, string>();

            _colorBagSet = new BinGradeColorSet();

            //this.Size = Screen.PrimaryScreen.WorkingArea.Size;

        }

        private void frmPopResult_Load(object sender, EventArgs e)
        {
            string version = Host.GetProgramVersion();

            this.Text = "MPI Tester" + "  [ " + version + " ]";

            this.ResetMapSetting();

			this.CreateEnableWaferMap();

		    this.tmrUpdate.Enabled = true;

            _isCreateForm = true;

            UpateDataToControls();

            this.chkIsEnableShowData.Checked = true;

            includeTestDataForm();
        }

        public void RegisterUpdateEvent(EventHandler e)
        {
            this.tmrUpdate.Tick += e;
        }

        #region >>> Public Property <<<

        public bool IsCreate
        {
            get { return this._isCreateForm; }
        }

        #endregion

        #region >>>  Private Method <<<

        private void OnRefreshMapSetting(object o, EventArgs e)
        {
            this.UpateDataToControls();

            UpdateMultiMap(); 
        }

        public void AppSystem_OnAppSystemRun(object sender, EventArgs e)
        {
            if (this.IsHandleCreated == false)
                return;

				// UpdateMultiMap();

            int lift = DataCenter._uiSetting.WaferMapLeft;
            int top = DataCenter._uiSetting.WaferMapTop;
            int right = DataCenter._uiSetting.WaferMapRight;
            int bottom = DataCenter._uiSetting.WaferMapBottom;

           FormAgent.TestResultForm.SetWaferMap();

            foreach (frmPopWaferMap form in PopWaferFormBag.Values)
            {
                if (form.IsDisposed || form.Disposing)
                    continue;

               // form.SetDataBase();

                form.Reset(lift, top, right, bottom);
            }

            this.UpateDataToControls();
        }

        private void CreateEnableWaferMap()
        {
            foreach (var map in this._dicEnableWaferMap)
            {
                frmPopWaferMap form=null;

                if (!this.PopWaferFormBag.ContainsKey(map.Key))            
                {
                    form = new frmPopWaferMap();

                    string symbol = map.Key;

                    form.WMSymbolId = symbol;

                    form.TopLevel = false;

                    form.Parent = this.plResult;

                    form.Dock = DockStyle.None;

                    //form.Location = multiMapArrangePosition[index];

                    //index++;

                    form.Show();

						  //int lift = DataCenter._uiSetting.WaferMapLift;
						  //int top = DataCenter._uiSetting.WaferMapTop;
						  //int right = DataCenter._uiSetting.WaferMapRight;
						  //int bottom = DataCenter._uiSetting.WaferMapBottom;

						  //form.Reset(lift, top, right, bottom);

                    PopWaferFormBag.Add(symbol, form);

                    if (this._colorBagSet.ContainsKey(symbol))
                    {
                        form.ApplyColorSetting(this._colorBagSet[symbol]);
                    }
                }
            }

            this.autoArrangeFourMapLoaction();
        }

        private void autoArrangeFourMapLoaction()
        {
            List<string> disposed_list = new List<string>();

            int idx = 0;

            foreach (KeyValuePair<string, frmPopWaferMap> item in PopWaferFormBag)
            {
                Form form = item.Value;

                if (form.IsDisposed || form.Disposing)
                {
                    disposed_list.Add(item.Key);

                    continue;
                }

                if (idx >= 4)
                    break;

                Point location = this.multiMapArrangePosition[idx];

                location.X = location.X + this._dataFormWith;


                location.Y = this.multiMapArrangePosition[idx].Y;


                form.Location = location;

                idx++;
            }
        }

        private void DisposeDisableWaferMap()
        {
            // DisPose 以 PopMap 繞回圈
            // 把這次沒有要show的關掉,Dispose去除，並移除Event handler.

            List<string> disposed_list = new List<string>();

            foreach (var item in PopWaferFormBag)
            {
                if (!this._dicEnableWaferMap.ContainsKey(item.Key))
                {
                    frmPopWaferMap form = null;

                    string symbol = item.Key;

                    form = PopWaferFormBag[symbol];

                    form.RemoveEvent();

                    form.Close();

                    form.Dispose();

                    form = null;

                    disposed_list.Add(item.Key);
                }
                else
                {
              
                }
            }

            foreach (string s in disposed_list)
            {
                PopWaferFormBag.Remove(s);
            }
        }

        private void applyMapColor()
        {
            foreach (KeyValuePair<string, frmPopWaferMap> item in PopWaferFormBag)
            {
                if (this._colorBagSet.ContainsKey(item.Key))
                {
                    frmPopWaferMap form = item.Value;
                    form.ApplyColorSetting(this._colorBagSet[item.Key]);
                }
            }
        }

        private void ResetMapSetting()
        {
            if (DataCenter._mapData.WeferMapShowItem.Count == 0)
            {
                _colorBagSet.Clear();

                this._dicEnableWaferMap.Clear();

                return;
            }
            _colorBagSet.Clear();

            this._dicEnableWaferMap.Clear();

            // 把新增的Item加進來 ，當有要show map時
            foreach (TestItemData item in DataCenter._product.TestCondition.TestItemArray)
            {
                if (item.IsEnable == false)
                {
                    continue;
                }

                if (item.MsrtResult == null)
                {
                    continue;
                }

                foreach (TestResultData data in item.MsrtResult)
                {
                    if (!data.IsEnable)
                        continue;
                    if (DataCenter._mapData.WeferMapShowItem.Contains(data.KeyName))
                    {
                        BinGradeColor bgc = new BinGradeColor();

                        bgc.Title = data.Name;

                        bgc.KeyName = data.KeyName;

                        if (bgc.KeyName.Contains("LOP"))
                        {
                            bgc.Title += "(mcd)";
                        }
                        else if (bgc.KeyName.Contains("WATT"))
                        {
                            bgc.Title += "(mW)";
                        }
                        else if (bgc.KeyName.Contains("LM"))
                        {
                            bgc.Title += "(lm)";
                        }

                        bgc.DisplayFormat = data.Formate;

                        if (data.MaxLimitValue >= data.MinLimitValue)
                        {
                            bgc.Max = (float)data.MaxLimitValue;

                            bgc.Min = (float)data.MinLimitValue;
                        }

                        else if (data.MaxLimitValue < data.MinLimitValue)
                        {
                            bgc.Max = (float)data.MinLimitValue;

                            bgc.Min = (float)data.MaxLimitValue;
                        }

                        this._colorBagSet.Add(bgc.KeyName, bgc);

                        _dicEnableWaferMap.Add(data.KeyName, data.Name);
                    }
                }
            }
        }

        private void includeSpectrumForm()
        {
            frmTestResultSpectrum form = (frmTestResultSpectrum)FormAgent.RetrieveForm(typeof(frmTestResultSpectrum));

            form.TopLevel = false;

            form.Parent = this.plResult;

            form.Dock = DockStyle.None;

            // form.Location = SpectrumLocation;

            form.FormBorderStyle = FormBorderStyle.FixedToolWindow;

            form.Show();

        }

        private void UpdateMultiMap()
        {
            if (this.IsHandleCreated == false)
                return;

            this.ResetMapSetting();

            this.DisposeDisableWaferMap();

            this.CreateEnableWaferMap();

            this.applyBinColor();

            foreach (var item in PopWaferFormBag)
            {
                frmPopWaferMap form = null;

                string symbol = item.Key;

                form = PopWaferFormBag[symbol];

                form.Show();
            }
        }

        private void UpateDataToControls()
        {

            txtLotID.Text = DataCenter._uiSetting.LotNumber;

            txtWaferID.Text = DataCenter._uiSetting.WaferNumber;

            lblBaseTaskSheet.Text = DataCenter._uiSetting.TaskSheetFileName;

           // this.chkEnableMultiMap.Checked = DataCenter._uiSetting.IsEanleShowMultiMap;

            lblDateTime.Text = "Start Time : "+DataCenter._sysSetting.StartTestTime.ToString();

				if (DataCenter._machineConfig.Enable.IsCheckFilterWheel)
				{

					if (GlobalData.HwFilterWheelPos == 7
						 || GlobalData.HwFilterWheelPos == 0
						 || GlobalData.HwFilterWheelPos == 6)
					{
						lblHWFilterSelect.Text = "Error_" + GlobalData.HwFilterWheelPos;
					}
					else
					{
						this.lblHWFilterSelect.Text = GlobalData.HwFilterWheelPos.ToString();
					}

				}
				else
				{
					this.lblHWFilterSelect.Text = "Disable";
				}

				this.txtSWNDFilter.Text = (DataCenter._product.ProductFilterWheelPos + 1).ToString();

            SetAuthorityLevel();

            switch (DataCenter._uiSetting.UIOperateMode)
            {
                case (int)EUIOperateMode.Idle:
                 //   this.tsMain.Enabled = true;
                    btnColorMapSetting.Enabled= true;

                    //  this.cmbTaskSheet.Enabled = true;
                    break;
                //-----------------------------------------------------------------------------
                case (int)EUIOperateMode.AutoRun:
                case (int)EUIOperateMode.ManulRun:
                //    this.tsMain.Enabled = false;
                    btnColorMapSetting.Enabled = false;
                    //  this.cmbTaskSheet.Enabled = false;
                    break;
                //-----------------------------------------------------------------------------
                //case (int)EUIOperateMode.ManulRun:
                ////    this.tsMain.Enabled = false;
                //    btnColorMapSetting.Enabled = false;
                //    //  this.cmbTaskSheet.Enabled = false;
                //    break;
                //-----------------------------------------------------------------------------
                default:
                  //  this.tsMain.Enabled = true;
                    btnColorMapSetting.Enabled = true;
                    //     this.cmbTaskSheet.Enabled = true;
                    break;
            }
        }

        private void SetAuthorityLevel()
        {
            btnColorMapSetting.Enabled = true;
            this.tsMain.Enabled = true;
            this.btnExit.Enabled = true;

            switch (DataCenter._userManag.CurrentAuthority)
            {
                case EAuthority.Operator:
                case EAuthority.QC:
                    btnColorMapSetting.Enabled = false;
                    this.tsMain.Enabled = false;
                    this.btnExit.Enabled = false;
							//this.lblTestTimeData.Visible=false;
							//lblTestTime.Visible = false;

                    break;
                //  ------------------------------------------------------------
                case EAuthority.Engineer:
						  //this.lblTestTimeData.Visible = false;
						  //lblTestTime.Visible = false;
						  break;
                case EAuthority.Admin:
						  this.lblTestTimeData.Visible = true;
						  lblTestTime.Visible = true;
                    break;
                //   ------------------------------------------------------------
                case EAuthority.Super:
						  this.lblTestTimeData.Visible = true;
						  lblTestTime.Visible=false;
                    break;
                // ------------------------------------------------------------
                default:
                    break;
            }
        }

        private void DisposeWaferMap()
        {
            List<string> disposed_list = new List<string>();

            foreach (var item in PopWaferFormBag)
            {
                frmPopWaferMap form = null;

                string symbol = item.Key;

                form = PopWaferFormBag[symbol];

                form.Close();

                form.Dispose();

                form = null;

                disposed_list.Add(item.Key);
            }

            foreach (string s in disposed_list)
            {
                PopWaferFormBag.Remove(s);
            }

        }

        private void applyBinColor()
        {
            foreach (KeyValuePair<string, frmPopWaferMap> item in PopWaferFormBag)
            {
                if (this._colorBagSet.ContainsKey(item.Key))
                {
                    frmPopWaferMap form = item.Value;
                    form.ApplyColorSetting(this._colorBagSet[item.Key]);
                }
            }
        }

        private void AddScrollBar()
        {
            ScrollBar vScrollBar1 = new VScrollBar();
            vScrollBar1.Dock = DockStyle.Right;
            vScrollBar1.Scroll += (sender, e) => { this.plResult.VerticalScroll.Value = vScrollBar1.Value; };
            plResult.Controls.Add(vScrollBar1);

            ScrollBar vScrollBar2 = new VScrollBar();
            vScrollBar2.Dock = DockStyle.Bottom;
            vScrollBar2.Scroll += (sender, e) => { this.plResult.HorizontalScroll.Value = vScrollBar2.Value; };
            plResult.Controls.Add(vScrollBar2);

        }

        private void includeTestDataForm()
        {
            switch (DataCenter._machineConfig.TesterFunctionType)
            {
                case ETesterFunctionType.Single_Die:
                case ETesterFunctionType.Multi_Terminal:
                    {
                        if (this.chkIsEnableShowData.Checked == true)
                        {
                            includeDataFormInfo();
                        }
                        else
                        {
                            frmTestResultData form = (frmTestResultData)FormAgent.RetrieveForm(typeof(frmTestResultData));

                            this._dataFormWith = 0;

                            form.Hide();
                        }
                        
                        break;
                    }
                case ETesterFunctionType.Multi_Die:
                case ETesterFunctionType.Multi_Pad:
                    {
                        if (this.chkIsEnableShowData.Checked == true)
                        {
                            includeChannelDataFormInfo();
                        }
                        else
                        {
                            frmTestResultChannelData form = (frmTestResultChannelData)FormAgent.RetrieveForm(typeof(frmTestResultChannelData));

                            this._dataFormWith = 0;

                            this.Width = 655;

                            form.Hide();
                        }
                        break;
                    }

            }
            
            this.autoArrangeFourMapLoaction();
        }

        private void includeDataFormInfo()
        {
            frmTestResultData form = (frmTestResultData)FormAgent.RetrieveForm(typeof(frmTestResultData));

            form.TopLevel = false;

            form.Parent = this.plResult;

            form.Dock = DockStyle.Left;

            //  form.Width = this.plResult.Width;

            //form.Height = plResult.Height - 60;


            //form.Location = SpectrumLocation;

            form.FormBorderStyle = FormBorderStyle.None;

            form.Show();

            _dataFormWith = form.Width;
        }

        private void includeChannelDataFormInfo()
        {
            frmTestResultChannelData form = (frmTestResultChannelData)FormAgent.RetrieveForm(typeof(frmTestResultChannelData));

            form.TopLevel = false;

            form.Parent = this.plResult;

            form.Dock = DockStyle.Left;

            //  form.Width = this.plResult.Width;

            form.Height = plResult.Height - 60;


            //form.Location = SpectrumLocation;

            form.FormBorderStyle = FormBorderStyle.None;

            form.Show();

            _dataFormWith = form.Width;

            this.Width = _dataFormWith + 700;

            plResult.Width = _dataFormWith + 700;

        }

        private void includeAnalysisForm()
        {
            frmTestResultAnalyze form = (frmTestResultAnalyze)FormAgent.RetrieveForm(typeof(frmTestResultAnalyze));

            form.TopLevel = false;

            form.Parent = this.plResult;

            form.Dock = DockStyle.Fill;

            //  form.Location = SpectrumLocation;

            form.FormBorderStyle = FormBorderStyle.None;

            form.Show();

        }

        #endregion

        #region >>> UI Event Handler <<<

        private void frmPopResult_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.DisposeWaferMap();
        }

        private void frmPopResult_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;

            this.tmrUpdate.Enabled = false;

            this.Hide();

        }

        private void btnAutoArrangeMultiMap_Click(object sender, EventArgs e)
        {
            this.autoArrangeFourMapLoaction();
        }

        private void chkIsEnableShowSpectrum_CheckedChanged(object sender, EventArgs e)
        {
            if (this.chkIsEnableShowSpectrum.Checked == true)
            {
                includeSpectrumForm();
            }
            else
            {
                FormAgent.TestResultSpectrum.Hide();

                this._dataFormWith = 0;
            }
            this.autoArrangeFourMapLoaction();
        }

        private void btnColorMapSetting_Click(object sender, EventArgs e)
        {
            frmWaferMapSetting frmWaferMapSetting = new frmWaferMapSetting();

            frmWaferMapSetting.ShowDialog();

            frmWaferMapSetting.Dispose();

            frmWaferMapSetting.Close();

           //Host.FireTestItemChange();
        }

        private void chkEnableMultiMap_CheckedChanged(object sender, EventArgs e)
        {
            UpdateMultiMap();
        }

        private void btnSingleTest_Click(object sender, EventArgs e)
        {
            AppSystem.Fire_SwitchUIEvent((int)EBaseFormDisplayUI.OperatorForm);
            this.Hide();
        }

        private void tmrUpdate_Tick(object sender, EventArgs e)
        {
				if(!this.Visible)
				return;

                if(DataCenter._userManag.CurrentAuthority == EAuthority.Admin || DataCenter._userManag.CurrentAuthority == EAuthority.Super)
                {
                    this.lblTestTimeData.Text = DataCenter._acquireData.ChipInfo.TestTime.ToString("0000.0");
                }
                else
                {
                    this.lblTestTimeData.Text = "0.0";
                }


            this.lblBinData.Text = DataCenter._acquireData.ChipInfo.BinGrade.ToString();
            this.lblColXData.Text = DataCenter._acquireData.ChipInfo.ColX.ToString();
            this.lblRowYData.Text = DataCenter._acquireData.ChipInfo.RowY.ToString();

            this.lblTestCountData.Text = DataCenter._acquireData.ChipInfo.TestCount.ToString();
            this.lblPassCountData.Text = DataCenter._acquireData.ChipInfo.GoodDieCount.ToString();
            this.lblFailCountData.Text = DataCenter._acquireData.ChipInfo.FailDieCount.ToString();
            this.lblPassRateData.Text = DataCenter._acquireData.ChipInfo.GoodRate.ToString("00.00") + "%";

            this.lblStatus.Text = AppSystem._MPITesterKernel.Status.State.ToString();

				//switch(AppSystem._MPITesterKernel.Status.State)
				//{
				//   case EKernelState.Running:
				//      if(btnColorMapSetting.Enabled==true)
				//      {
				//         btnColorMapSetting.Enabled=false;
				//      }

				//      if (this.btnCycleRun.Enabled == true)
				//      {
				//         btnCycleRun.Enabled = false;
				//      }
				//      break;

				//   case EKernelState.Ready:
				//      break;

				//      default:
				//      break;

				//}

            this.lblAuthority.Text = DataCenter._uiSetting.AuthorityLevel.ToString();

            this.lblUser.Text = "User:" + DataCenter._uiSetting.OperatorName;

            System.Windows.Forms.Application.DoEvents();

        }

        private void btnHide_Click(object sender, EventArgs e)
        {
            AppSystem.Fire_SwitchUIEvent((int)EBaseFormDisplayUI.ResultForm);
            this.Hide();
        }

        private void btnTestItem_Click(object sender, EventArgs e)
        {
            AppSystem.Fire_SwitchUIEvent((int)EBaseFormDisplayUI.ConditionForm);
            this.Hide();
        }

        private void btnCycleRun_Click(object sender, EventArgs e)
        {
            //UILog.Log(this, sender, "btnReSingleTest_Click");

            //DialogResult result = TopMessageBox.ShowMessage((int)EMessageCode.RunSingleReTest, "Test Result Output File Exist, Would you Run Test ? ", "  Run Single Retest");

            //if (result != DialogResult.OK)
            //    return;

            System.Threading.Thread.Sleep(300);

            GlobalFlag.IsReSingleTestMode = true;
            AppSystem.RunSingleRetest();
        }

        private void btnUISetting_Click(object sender, EventArgs e)
        {
            AppSystem.Fire_SwitchUIEvent((int)EBaseFormDisplayUI.UISettingForm);

            this.Hide();
        }

        private void chkIsEnableShowData_CheckedChanged(object sender, EventArgs e)
        {
            includeTestDataForm();
        }

        #endregion

        private void btnExit_Click(object sender, EventArgs e)
        {
           //FormAgent.MainForm.ExitApp();
        }

		private void frmTestresultChart_VisbkeChange(object sender, EventArgs e)
		{
			if ( this.Visible == true )
            {
                this.tmrUpdate.Start();
            }
            else
            {
                this.tmrUpdate.Stop();
            }
		}

        private void frmTestResultChart_SizeChanged(object sender, EventArgs e)
        {
            plResult.Width = this.Width - 5;
        }

        //private void frmTestResultChart_SizeChanged(object sender, EventArgs e)
        //{
        //    plResult.Width = this.Width - 5;
        //}
    }
}

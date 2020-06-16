using System;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;

using MPI.Tester.Data;
using MPI.Tester.DeviceCommon;

using DevComponents.DotNetBar.Controls;

namespace MPI.Tester.Gui
{
    public partial class frmChannelCondition : System.Windows.Forms.Form
    {
        private delegate void UpdateDataHandler();

        private Button[,] _channelStatusCtrl;

        private ToolTip _channelStatusTip;

        private Color _channelStatusHighLineColor;

        private Color _channelStatusDefaultColor;

        private ETesterFunctionType _funcType;

        public frmChannelCondition()
        {
            InitializeComponent();

            this.VisibleChanged += new System.EventHandler(this.UpdateDgvEventHandler);

            this._channelStatusTip = new ToolTip();

            this._channelStatusDefaultColor = Color.LightGray;

            this._channelStatusHighLineColor = Color.LightGreen;
        }

        #region >>> Private Method <<<

        private void SetChannelStatusUI(int colMax, int rowMax)
        {
            int width = 48;
            int pitch = 10;
            int fontSize = 16;
            int count = 0;

            this.pnlDutChDisplay.Controls.Clear();

            this._channelStatusCtrl = new Button[colMax, rowMax];

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
                    this._channelStatusCtrl[col, row] = new Button();

                    this._channelStatusCtrl[col, row].Left = locationX + col * (width + pitch);
                    this._channelStatusCtrl[col, row].Top = locationY / 2 + row * (width + pitch);
                    this._channelStatusCtrl[col, row].Name = "btnChannel" + count.ToString();

                    this._channelStatusTip.SetToolTip(this._channelStatusCtrl[col, row], "Click to Enable / Disable");

                    this._channelStatusCtrl[col, row].Click += new EventHandler(ChannelStatusChangeEvent);

                    this._channelStatusCtrl[col, row].Width = width;
                    this._channelStatusCtrl[col, row].Height = width;

                    this._channelStatusCtrl[col, row].Text = (count + 1).ToString();
                    this._channelStatusCtrl[col, row].TextAlign = ContentAlignment.MiddleCenter;
                    this._channelStatusCtrl[col, row].Font = new Font("Verdana", fontSize, FontStyle.Bold);

                    this._channelStatusCtrl[col, row].BackColor = this._channelStatusDefaultColor;

                    this.pnlDutChDisplay.Controls.Add(this._channelStatusCtrl[col, row]);

                    count++;
                }
            }
        }

        private void ChangeAuthority()
        {
            this.dgvChannelCondiSetting.ReadOnly = false;
        }

        private void UpdateToConditionTableDgv()
        {  
            DataGridViewRow dgvRow;
            int col = 0;
            int row = 0;
            int itemCnt;
            int reserveCnt = 2;

            this.dgvChannelCondiSetting.Columns.Clear();
            this.dgvChannelCondiSetting.SuspendLayout();

            this.dgvChannelCondiSetting.ReadOnly = false;
            this.dgvChannelCondiSetting.RowHeadersVisible = false;

            if (DataCenter._product.TestCondition.TestItemArray == null || !DataCenter._product.TestCondition.ChannelConditionTable.IsEnable)
            {
                this.dgvChannelCondiSetting.RowCount = reserveCnt;
            }
            else
            {
                this.dgvChannelCondiSetting.RowCount = DataCenter._product.TestCondition.TestItemArray.Length + reserveCnt;
            }

            ChannelConditionTable dataTable = DataCenter._product.TestCondition.ChannelConditionTable;

            //--------------------------------------------------------------------------------------------------------------------------------------
            // Create DGV Title
            //--------------------------------------------------------------------------------------------------------------------------------------
            this.dgvChannelCondiSetting[0, 0].Value = "Status";
            this.dgvChannelCondiSetting.Rows[0].ReadOnly = true;

            this.dgvChannelCondiSetting[0, 1].Value = "Order";
            this.dgvChannelCondiSetting.Rows[1].ReadOnly = false;

            this.dgvChannelCondiSetting.Columns[0].Width = 75;
            this.dgvChannelCondiSetting.Rows[0].Height = 30;

            this.dgvChannelCondiSetting.Columns[0].DefaultCellStyle.Font = new Font("Arial", 9, FontStyle.Regular);
            this.dgvChannelCondiSetting.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
            //this.dgvChannelCondiSetting.Columns[0].DefaultCellStyle.BackColor = Color.PowderBlue;

            for (int i = 0; i < dataTable.Count; i++)
            {
                col = i + 1;

                this.dgvChannelCondiSetting.Columns.Add(new DataGridViewTextBoxColumn());

                this.dgvChannelCondiSetting.Columns[col].SortMode = DataGridViewColumnSortMode.NotSortable;
                this.dgvChannelCondiSetting.Columns[col].HeaderText = "DUT-" + (i + 1).ToString("D2");
                this.dgvChannelCondiSetting.Columns[col].Width = 90;

                itemCnt = 0;

                if (DataCenter._product.TestCondition.TestItemArray != null && DataCenter._product.TestCondition.ChannelConditionTable.IsEnable)
                {
                    if (DataCenter._product.TestCondition.TestItemArray.Length != 0)
                    {
                        foreach (TestItemData item in DataCenter._product.TestCondition.TestItemArray)  // Row
                        {
                            row = reserveCnt + itemCnt;
                            dgvRow = this.dgvChannelCondiSetting.Rows[row];

                            dgvRow.ReadOnly = true;

                            dgvRow.Height = 32;

                            dgvRow.Cells[0].Value = item.Name;

                            itemCnt++;
                        }
                    }
                }
            }

            //--------------------------------------------------------------------------------------------------------------------------------------
            // Update Data to DGV
            //--------------------------------------------------------------------------------------------------------------------------------------
            string cellStr = string.Empty;

            for (int channel = 0; channel < dataTable.Count; channel++)
            {
                col = channel + 1;

                if (!dataTable.IsEnable)
                    continue;

                this.dgvChannelCondiSetting.Rows[1].Cells[col].Value = dataTable.Channels[channel].Order + 1;  // Order Display base = 1;

                // Update DUT-CH Enable to DGV
                if (dataTable.Channels[channel].IsEnable)
                {
                    this.dgvChannelCondiSetting[col, 0].Value = "Enable";
                    this.dgvChannelCondiSetting[col, 0].Style.ForeColor = Color.Black;
                }
                else
                {
                    this.dgvChannelCondiSetting[col, 0].Value = "Disable";
                    this.dgvChannelCondiSetting[col, 0].Style.ForeColor = Color.Red;
                  
                }
                // Update TestItem to DGV
                if (dataTable.Channels[channel].Conditions == null)
                    continue;

                if (dataTable.Channels[channel].Conditions.Count == 0)
                    continue;

                itemCnt = 0;

                foreach (TestItemData mainItem in DataCenter._product.TestCondition.TestItemArray)
                {
                    row = reserveCnt + itemCnt;
                    dgvRow = this.dgvChannelCondiSetting.Rows[row];
                    dgvRow.Height = 32;

                    if (mainItem.IsEnable)
                    {
                        if (DataCenter._product.TestCondition.ChannelConditionTable.Channels[channel].Conditions[itemCnt].IsEnable)
                        {
                            cellStr = this.GetTestItemInfo(mainItem);

                            dgvRow.Cells[col].Style.BackColor = Color.White;
                        }
                        else
                        {
                            cellStr = string.Empty;

                            dgvRow.Cells[col].Style.BackColor = Color.LightGray;
                        }
                    }
                    else
                    {                     
                        dgvRow.Visible = false;
                    }

                    dgvRow.Cells[col].Value = cellStr;

                    if (DataCenter._machineConfig.TesterFunctionType == ETesterFunctionType.Multi_Die)
                    {
                        dgvRow.Visible = false;
                    }

                    itemCnt++;
                }
            }

            this.dgvChannelCondiSetting.ResumeLayout();
        }

        private void UpdateToChannelStatusUI()
        {
            if (DataCenter._product.TestCondition.ChannelConditionTable.ColXCount != this._channelStatusCtrl.GetLength(0) ||
                DataCenter._product.TestCondition.ChannelConditionTable.RowYCount != this._channelStatusCtrl.GetLength(1))
            {

                switch (DataCenter._machineConfig.TesterFunctionType)
                {
                    case ETesterFunctionType.Multi_Die:
                        {
                            this.IsChannelStatusEnable(false);
                            
                            break;
                        }
                    case ETesterFunctionType.Multi_Pad:
                        {
                            this.CancelChannelStatusChangeEvent();

                            this.SetChannelStatusUI(DataCenter._product.TestCondition.ChannelConditionTable.ColXCount, DataCenter._product.TestCondition.ChannelConditionTable.RowYCount);

                            break;
                        }
                }
            }
                    
            int channel = 0;

            if (this._channelStatusCtrl == null)
                return;

            for (int row = 0; row < this._channelStatusCtrl.GetLength(1); row++)
            {
                for (int col = 0; col < this._channelStatusCtrl.GetLength(0); col++)
                {
                    this._channelStatusCtrl[col, row].BackColor = this._channelStatusDefaultColor;

                    if (DataCenter._product.TestCondition.ChannelConditionTable.IsEnable)
                    {
                        if (DataCenter._product.TestCondition.ChannelConditionTable.Channels.Length <= channel)
                            continue;
                        
                        if (DataCenter._product.TestCondition.ChannelConditionTable.Channels[channel].IsEnable)
                        {
                            this._channelStatusCtrl[col, row].BackColor = this._channelStatusHighLineColor;
                        }
                    }

                    channel++;
                }
            }
        }

        private void UpdateDataToControls()
        {
            switch (DataCenter._uiSetting.UIOperateMode)
            {
                case (int)EUIOperateMode.Idle:
                    this.btnConfirm.Enabled = true;
                    this.IsChannelStatusEnable(true);
                    break;
                //-----------------------------------------------------------------------------
                case (int)EUIOperateMode.AutoRun:
                case (int)EUIOperateMode.ManulRun:
                    this.btnConfirm.Enabled = false;
                    this.IsChannelStatusEnable(false);
                    break;
                //-----------------------------------------------------------------------------
                //case (int)EUIOperateMode.ManulRun:
                //    this.btnConfirm.Enabled = false;
                //    this.IsChannelStatusEnable(false);
                //    break;
                //-----------------------------------------------------------------------------
                default:
                    this.btnConfirm.Enabled = true;
                    this.IsChannelStatusEnable(true);
                    break;
            }

            string info = "Recipe Channel Setting:";

            // (1) check TestCondition length and channelConditionData length
            if (DataCenter._product.TestCondition.TestItemArray != null)
            {
                info = string.Format("Recipe Channel Setting: X = {0}, Y = {1}", DataCenter._product.TestCondition.ChannelConditionTable.ColXCount, 
                                                                                 DataCenter._product.TestCondition.ChannelConditionTable.RowYCount);
            }

            this.lblRecipeCondiTableInfo.Text = info;

            this.chkSamplingOpticalTest.Checked = DataCenter._sysSetting.IsEnableMultiDieOpticalSamplingTest;

            try
            {
                // (2) Update ConditionTable to Dgv
                this.UpdateToConditionTableDgv();

                this.UpdateToChannelStatusUI();
            }
            catch
            {

            }

            //// (2) Update ConditionTable to Dgv
            //this.UpdateToConditionTableDgv();

            //this.UpdateToChannelStatusUI();
        }

        private void UpdateDgvEventHandler(object sender, EventArgs e)
        {
            this.UpdateDataToControls();
        }

        private void ChannelStatusChangeEvent(object sender, EventArgs e)
        {
            Button btn = (Button)sender;

            if (btn.BackColor == this._channelStatusDefaultColor)
            {
                btn.BackColor = this._channelStatusHighLineColor;
            }
            else
            {
                btn.BackColor = this._channelStatusDefaultColor;
            }
        }

        private void CancelChannelStatusChangeEvent()
        {
            if (this._channelStatusCtrl != null)
            {
                for (int col = 0; col < this._channelStatusCtrl.GetLength(0); col++)
                {
                    for (int row = 0; row < this._channelStatusCtrl.GetLength(1); row++)
                    {
                        this._channelStatusCtrl[col, row].Click -= ChannelStatusChangeEvent;
                    }
                }
            }
        }

        private void IsChannelStatusEnable(bool isEnable)
        {
            if (this._channelStatusCtrl == null)
                return;

            for (int col = 0; col < this._channelStatusCtrl.GetLength(0); col++)
            {
                for (int row = 0; row < this._channelStatusCtrl.GetLength(1); row++)
                {
                    this._channelStatusCtrl[col, row].Enabled = isEnable;
                }
            }
        }

        private string GetTestItemInfo(TestItemData item)
        {
            string rtnStr = string.Empty;

            string forceValeStr = string.Empty;

            string forceTimeStr = string.Empty;

            string stTimeStr = string.Empty;

            switch (item.Type)
            {
                case ETestType.LOPWL:
                    {
                        forceValeStr = Math.Abs(item.ElecSetting[0].ForceValue).ToString() + item.ElecSetting[0].ForceUnit;

                        forceTimeStr = item.ElecSetting[0].ForceTime.ToString() + item.ElecSetting[0].ForceTimeUnit.ToString();

                        stTimeStr = (item as LOPWLTestItem).OptiSetting.FixIntegralTime.ToString();

                        rtnStr = string.Format("{0}/{1}/ST={2}", forceValeStr, forceTimeStr, stTimeStr);
                        
                        break;
                    }
                default:
                    {
                        if (item.ElecSetting != null)
                        {
                            forceValeStr = Math.Abs(item.ElecSetting[0].ForceValue).ToString() + item.ElecSetting[0].ForceUnit;

                            forceTimeStr = item.ElecSetting[0].ForceTime.ToString() + item.ElecSetting[0].ForceTimeUnit.ToString();

                            rtnStr = string.Format("{0} / {1}", forceValeStr, forceTimeStr);
                        }
                        break;
                    }

            }

            return rtnStr;
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

        public void SetFormSize(ETesterFunctionType type)
        {
            this._funcType = type;

            //if (type == ETesterFunctionType.Multi_Die)
            //{
            //    this.pnlDutChDisplay.Size = new Size(223, 210);
            //    this.lblRecipeCondiTableInfo.Location = new Point(12, 259);
            //    this.dgvChannelCondiSetting.Size = new Size(462, 262);
            //}

            this.SetChannelStatusUI(DataCenter._machineConfig.ChannelConfig.ColXCount, DataCenter._machineConfig.ChannelConfig.RowYCount);
        }

        #endregion

        #region >>> UI Event Handler <<<

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if (DataCenter._product.TestCondition == null || DataCenter._product.TestCondition.TestItemArray == null)
                return;

            if (!DataCenter._product.TestCondition.ChannelConditionTable.IsEnable)   // ConditionTable 關閉, 不執行 UI Update
                return;

            int channel = 0;

            //-----------------------------------------------------------------------------------------------------------------------------
            // 儲存 Channel 開啟/關閉狀態
            channel = 0;

            if (this._channelStatusCtrl != null)
            {
                for (int row = 0; row < this._channelStatusCtrl.GetLength(1); row++)
                {
                    for (int col = 0; col < this._channelStatusCtrl.GetLength(0); col++)
                    {
                        ChannelConditionData condition = DataCenter._product.TestCondition.ChannelConditionTable.Channels[channel];

                        if (this._channelStatusCtrl[col, row].BackColor == this._channelStatusHighLineColor)
                        {
                            condition.IsEnable = true;
                        }
                        else
                        {
                            condition.IsEnable = false;
                        }

                        channel++;
                    }
                }
            }

            //-----------------------------------------------------------------------------------------------------------------------------
            // 檢查 Order 是否重覆
            Host.SetErrorCode(EErrorCode.NONE);

            int order = 0;

            for (int col = 1; col < this.dgvChannelCondiSetting.ColumnCount; col++)
            {
                int.TryParse(this.dgvChannelCondiSetting[col, 1].Value.ToString(), out order);

                this.dgvChannelCondiSetting[col, 1].Style.BackColor = Color.Empty;

                for (int chkCol = col + 1; chkCol < this.dgvChannelCondiSetting.ColumnCount; chkCol++)
                {
                    int chkOrder = 0;

                    int.TryParse(this.dgvChannelCondiSetting[chkCol, 1].Value.ToString(), out chkOrder);

                    if (order == chkOrder)
                    {
                        this.dgvChannelCondiSetting[col, 1].Style.BackColor = Color.LightPink;

                        this.dgvChannelCondiSetting[chkCol, 1].Style.BackColor = Color.LightPink;

                        Host.SetErrorCode(EErrorCode.ChannelOrderSettingErr);

                        return;
                    }
                }
            }

            // 儲存 Channel Order
            channel = 0;

            for (int col = 1; col < this.dgvChannelCondiSetting.ColumnCount; col++)
            {
                ChannelConditionData condition = DataCenter._product.TestCondition.ChannelConditionTable.Channels[channel];

                int.TryParse(this.dgvChannelCondiSetting[col, 1].Value.ToString(), out order);

                condition.Order = (uint)(order - 1);  // UI Order, base: 1; ChannelConditionData Order, base: 0

                channel++;
            }
            //-----------------------------------------------------------------------------------------------------------------------------

            DataCenter._sysSetting.IsEnableMultiDieOpticalSamplingTest = this.chkSamplingOpticalTest.Checked;

            DataCenter._conditionCtrl.CopyTestItemArrayToEachChannel();

            AppSystem.SetDataToSystem();

            DataCenter.SaveProductFile();

            Host.UpdateDataToAllUIForm();
        }

        private void dgvChannelCondiSetting_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            e.CellStyle.SelectionBackColor = e.CellStyle.BackColor;

            e.CellStyle.SelectionForeColor = e.CellStyle.ForeColor;
        }

        private void dgvChannelCondiSetting_CellValidated(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;

            int order = 0;

            if (e.RowIndex == 1)  // Order Key In
            {
                if (int.TryParse(this.dgvChannelCondiSetting[e.ColumnIndex, e.RowIndex].Value.ToString(), out order))
                {
                    if (order >= this.dgvChannelCondiSetting.ColumnCount || order < 1)
                    {
                        this.dgvChannelCondiSetting[e.ColumnIndex, e.RowIndex].Value = e.ColumnIndex.ToString();
                    }
                }
                else
                {
                    this.dgvChannelCondiSetting[e.ColumnIndex, e.RowIndex].Value = e.ColumnIndex.ToString();
                }
            }
        }

        #endregion
    }
}

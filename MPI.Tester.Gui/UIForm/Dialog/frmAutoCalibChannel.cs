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
    public partial class frmAutoCalibChannel : Form
    {
        public frmAutoCalibChannel()
        {
            InitializeComponent();
        }

        private void UpdateProductChannelData()
        {
            this.dgvByChannelCoefTable.Rows.Clear();

            ChannelConditionTable conditionTable = DataCenter._product.TestCondition.ChannelConditionTable;

            if (conditionTable == null)
            {
                return;
            }

            if (conditionTable.Channels.Length == 0)
            {
                return;
            }

            // If Talbe Channel Row/Col Count is not match to Machine setting
            if (conditionTable.ColXCount != DataCenter._machineConfig.ChannelConfig.ColXCount || conditionTable.RowYCount != DataCenter._machineConfig.ChannelConfig.RowYCount)
            {
                return;
            }

            if (!conditionTable.IsEnable)
            {
                return;
            }

            TestItemData[] testItems = DataCenter._product.TestCondition.TestItemArray;

            if (testItems == null)
            {
                return;
            }

            this.dgvByChannelCoefTable.SuspendLayout();

            string gainKeyName = string.Empty;

            int rowCount = 0;

            int index = 0;

            bool isShow = false;

            foreach (TestItemData item in testItems)
            {
                if (item.GainOffsetSetting == null || item.GainOffsetSetting.Length == 0)
                    continue;

                foreach (GainOffsetData data in item.GainOffsetSetting)
                {
                    if (!data.IsEnable || !data.IsVision)
                        continue;

                    for (uint channel = 0; channel < conditionTable.Count; channel++)
                    {
                        GainOffsetData coef = conditionTable.Channels[channel].GetByChannelGainOffsetData(data.KeyName);

                        this.dgvByChannelCoefTable.Rows.Add();

                        this.dgvByChannelCoefTable.Rows[rowCount].Cells[0].Value = (rowCount + 1).ToString();

                        if ((item is LOPWLTestItem) && data.KeyName.IndexOf("_") > 0)
                        {
                            gainKeyName = data.KeyName.Remove(data.KeyName.IndexOf("_"));

                            switch (gainKeyName)
                            {
                                case "LOP":        // EOptiMsrtType.LOP :
                                    this.dgvByChannelCoefTable.Rows[rowCount].Cells[1].Value = coef.Name + " (mcd)";
                                    isShow = true;
                                    break;
                                //------------------------------------------------
                                case "WATT":       // EOptiMsrtType.WATT
                                    this.dgvByChannelCoefTable.Rows[rowCount].Cells[1].Value = coef.Name + " (mW)";
                                    isShow = true;
                                    break;
                                //------------------------------------------------
                                case "LM":         // EOptiMsrtType.LM
                                    this.dgvByChannelCoefTable.Rows[rowCount].Cells[1].Value = coef.Name + " (lm)";
                                    isShow = true;
                                    break;
                                //------------------------------------------------
                                default:
                                    this.dgvByChannelCoefTable.Rows[rowCount].Cells[1].Value = coef.Name;
                                    isShow = false;
                                    break;
                            }
                        }
                        else
                        {
                            this.dgvByChannelCoefTable.Rows[rowCount].Cells[1].Value = coef.Name;
                        }


                        this.dgvByChannelCoefTable.Rows[rowCount].Cells[2].Value = (channel + 1).ToString();
                        this.dgvByChannelCoefTable.Rows[rowCount].Cells[3].Value = coef.Type;
                        this.dgvByChannelCoefTable.Rows[rowCount].Cells[4].Value = coef.Square;
                        this.dgvByChannelCoefTable.Rows[rowCount].Cells[5].Value = coef.Gain;
                        this.dgvByChannelCoefTable.Rows[rowCount].Cells[6].Value = coef.Offset;
                        this.dgvByChannelCoefTable.Rows[rowCount].Cells[7].Value = coef.KeyName;
                        this.dgvByChannelCoefTable.Rows[rowCount].Cells[8].Value = 0.0;
                        this.dgvByChannelCoefTable.Rows[rowCount].Cells[9].Value = 0.0;
                        this.dgvByChannelCoefTable.Rows[rowCount].Visible = isShow;

                        //if ((index % 2) != 0)
                        //{
                        //    this.dgvByChannelCoefTable.Rows[rowCount].DefaultCellStyle.BackColor = Color.AliceBlue;
                        //}

                        rowCount++;
                    }

                    index++;
                }
            }

            this.dgvByChannelCoefTable.ResumeLayout();
        }

        private void dgvByChannelCoefTable_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void UpdateCalcChannelGain()
        {
            for (int i = 0; i < this.dgvByChannelCoefTable.Rows.Count; i++)
            {
                string keyName = this.dgvByChannelCoefTable.Rows[i].Cells[7].Value.ToString();

                string channelStr = this.dgvByChannelCoefTable.Rows[i].Cells[2].Value.ToString();

                int channel = -1;

                int.TryParse(channelStr, out channel);

                channel = channel - 1;

                if (AppSystem._autoCalibChannelGain.GainOffset.ContainsKey(keyName))
                {
                    MPI.Tester.Tools.GainOffset[] calcGainOffset=AppSystem._autoCalibChannelGain.GainOffset[keyName];

                    if (calcGainOffset != null)
                    {
                        if (channel >= 0)
                        {
                            if (channel < calcGainOffset.Length)
                            {
                                this.dgvByChannelCoefTable.Rows[i].Cells[8].Value = calcGainOffset[channel].Value;

                                this.dgvByChannelCoefTable.Rows[i].Cells[9].Value = calcGainOffset[channel].Gain;

                                this.dgvByChannelCoefTable.Rows[i].Cells[10].Value = calcGainOffset[channel].Offset;
                            }
                        }
                    }
                }
            }

            lblRowCol.Text = AppSystem._autoCalibChannelGain.CalibRowColKey;
        }

        private void frmAutoCalibChannel_Load(object sender, EventArgs e)
        {
            this.TopMost = true;

            UpdateProductChannelData();

            UpdateCalcChannelGain();

            //this.TopMost = false;
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            SaveByChannelCoefTable();

            DataCenter._conditionCtrl.CopyTestItemArrayToEachChannel();

            //  DataCenter.SaveProductFile();

            DataCenter.SaveProductFile();

            Host.UpdateDataToAllUIForm();

            this.Close();

            this.Dispose();
        }

        private void CombineCalcCoeff()
        {
            int row = 0;

            for (row = 0; row < this.dgvByChannelCoefTable.Rows.Count; row++)
            {
                string keyName = this.dgvByChannelCoefTable.Rows[row].Cells[7].Value.ToString();
                uint channel = Convert.ToUInt32(this.dgvByChannelCoefTable.Rows[row].Cells[2].Value.ToString()) - 1;

                //if (coef != null)
                //{
                //    coef.Gain = Convert.ToDouble(this.dgvByChannelCoefTable.Rows[row].Cells[5].Value);
                //    coef.Offset = Convert.ToDouble(this.dgvByChannelCoefTable.Rows[row].Cells[6].Value);
                //}
            }

        }

        private void SaveByChannelCoefTable()
        {
            if (DataCenter._product.TestCondition == null || DataCenter._product.TestCondition.TestItemArray == null )
                return;

            int row = 0;

            ChannelConditionTable saveTable = DataCenter._product.TestCondition.ChannelConditionTable;


            if (!saveTable.IsApplyByChannelCompensate)
            {
                return;
            }

            for (row = 0; row < this.dgvByChannelCoefTable.Rows.Count; row++)
            {
                string keyName = this.dgvByChannelCoefTable.Rows[row].Cells[7].Value.ToString();
                uint channel = Convert.ToUInt32(this.dgvByChannelCoefTable.Rows[row].Cells[2].Value.ToString()) - 1;

                GainOffsetData coef = saveTable.Channels[channel].GetByChannelGainOffsetData(keyName);

                if (coef != null)
                {
                    coef.Gain = Convert.ToDouble(this.dgvByChannelCoefTable.Rows[row].Cells[5].Value);
                    coef.Offset = Convert.ToDouble(this.dgvByChannelCoefTable.Rows[row].Cells[6].Value);
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
        }

        private void btnCombine_Click(object sender, EventArgs e)
        {
            int row = 0;

            for (row = 0; row < this.dgvByChannelCoefTable.Rows.Count; row++)
            {
                if (this.dgvByChannelCoefTable.Rows[row].Visible)
                {
                    string keyName = this.dgvByChannelCoefTable.Rows[row].Cells[7].Value.ToString();

                    uint channel = Convert.ToUInt32(this.dgvByChannelCoefTable.Rows[row].Cells[2].Value.ToString()) - 1;

                    double gain = (double)this.dgvByChannelCoefTable.Rows[row].Cells[5].Value;

                    string calcGainstr = this.dgvByChannelCoefTable.Rows[row].Cells[9].Value.ToString();

                    double calcGain = 1.0d;

                    double.TryParse(calcGainstr, out calcGain);

                    double targetGain = gain * calcGain;

                    this.dgvByChannelCoefTable.Rows[row].Cells[5].Value = targetGain;

                    this.dgvByChannelCoefTable.Rows[row].Cells[9].Value = 1.0d;

                    this.dgvByChannelCoefTable.Rows[row].Cells[5].Style.BackColor = Color.Yellow;

                    this.dgvByChannelCoefTable.Rows[row].Cells[9].Style.BackColor = Color.Yellow;
                }

            }
        }

    }
}

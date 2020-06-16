using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar.Controls;

using MPI.Tester.Data;
using MPI.Tester.DeviceCommon;

namespace MPI.Tester.GuiComponent.TestItemComponent
{
    public partial class CustomizeListComponent : UserControl
    {
        string _forceUnit = "mA";
        string _msrtUnit = "V";
        string _timeUnit = "ms";

        private List<SweepInfo> _sweepInfoList;
        public CustomizeListComponent()
        {
            InitializeComponent();
            SetUnit(_forceUnit, _forceUnit, _timeUnit);
            _sweepInfoList = new List<SweepInfo>();

            ColMode.Items.Add(ESweepMode.Linear);
            ColMode.Items.Add(ESweepMode.Log);
        }

        public void SetUnit(string fUnit, string mUnit,string tUnit)
        {
            _forceUnit = fUnit;
            _msrtUnit = mUnit;
            _timeUnit = tUnit;
            ColStartValue.HeaderText = "Start Value(" + _forceUnit + ")";
            ColEndValue.HeaderText = "End Value(" + _forceUnit + ")";
            ColClamp.HeaderText = "Clamp(" + _msrtUnit + ")";
            ColOffTime.HeaderText = "Off Time(" + _timeUnit + ")";
            ColForceTime.HeaderText = "Force Time(" + _timeUnit + ")";
        }

        #region>>public method<<

        public string ForceUnit
        {
            get  {return   _forceUnit; }
            set { _forceUnit = value;
            SetUnit(_forceUnit, _msrtUnit, _timeUnit);
            }
        }

        public string MsrtUnit
        {
            get { return _msrtUnit; }
            set { _msrtUnit = value;
            SetUnit(_forceUnit, _msrtUnit, _timeUnit);
            }
        }
        public string TimeUnit
        {
            get { return _timeUnit; }
            set
            {
                _timeUnit = value;
                SetUnit(_forceUnit, _msrtUnit, _timeUnit);
            }
        }

        public void SetSweepData(List<SweepInfo> sDataList)
        {
            foreach (var sData in sDataList)
            {
                AddRow(sData);
            }
            GetSweepListFromDGV();
            //SetRow(int rowNum, SweepInfo sData)
        }
        public List<SweepInfo> GetSweepData()
        {
            return GetSweepListFromDGV();
        }

        #endregion

        #region>>public property<<

        public DataGridViewDoubleInputColumn ColNPLC
        {
            get { return (dataGridViewX1.Columns["colNPLC"] as DataGridViewDoubleInputColumn); }
        }
        public DataGridViewCheckBoxColumn ColAutoRange
        {
            get { return (dataGridViewX1.Columns["colAutoRange"] as DataGridViewCheckBoxColumn); }
        }
        public DataGridViewDoubleInputColumn ColCnt
        {
            get { return (dataGridViewX1.Columns["colCnt"] as DataGridViewDoubleInputColumn); }
        }
        public DataGridViewComboBoxExColumn ColMode
        {
            get { return dataGridViewX1.Columns["colMode"] as DataGridViewComboBoxExColumn; }
        }
        public DataGridViewDoubleInputColumn ColStartValue
        {
            get { return (dataGridViewX1.Columns["colStart"] as DataGridViewDoubleInputColumn); }
        }
        public DataGridViewDoubleInputColumn ColEndValue
        {
            get { return (dataGridViewX1.Columns["colEnd"]as DataGridViewDoubleInputColumn); }
        }
        public DataGridViewDoubleInputColumn ColForceTime
        {
            get { return (dataGridViewX1.Columns["colFT"]as DataGridViewDoubleInputColumn); }
        }
        public DataGridViewDoubleInputColumn ColOffTime
        {
            get { return (dataGridViewX1.Columns["colOT"]as DataGridViewDoubleInputColumn); }
        }
        public DataGridViewDoubleInputColumn ColClamp
        {
            get { return (dataGridViewX1.Columns["colClamp"]as DataGridViewDoubleInputColumn); }
        }

        #endregion

        #region >>private method<<
        private void AddRow()
        {
            dataGridViewX1.Rows.Add();
            int rowQty = dataGridViewX1.RowCount;
            SweepInfo sInfo = new SweepInfo();
            SetRow(rowQty-1, sInfo);
            //rowData
        }
        private void AddRow(SweepInfo sInfo)
        {
            dataGridViewX1.Rows.Add();
            int rowQty = dataGridViewX1.RowCount;
            SetRow(rowQty - 1, sInfo);
            //rowData
        }

        private void DeletRow(int rowNum)
        {
            dataGridViewX1.Rows.RemoveAt(rowNum);//.Rows[id - 1];
        }

        private void SetRow(int rowNum, SweepInfo sData)
        {

            var rowData = dataGridViewX1.Rows[rowNum];
            (rowData.Cells["colID"] as DataGridViewTextBoxCell).Value = (rowNum+1).ToString();
            (rowData.Cells["colNPLC"] as DataGridViewDoubleInputCell).Value = sData.NPLC;
            (rowData.Cells["colAutoRange"] as DataGridViewCheckBoxCell).Value = sData.AutoMsrtRange;
            (rowData.Cells["colMode"] as DataGridViewTextBoxCell).Value = sData.Mode;

            (rowData.Cells["colStart"] as DataGridViewDoubleInputCell).Value = sData.StartValue;
            (rowData.Cells["colEnd"] as DataGridViewDoubleInputCell).Value = sData.EndValue;
            (rowData.Cells["colCnt"] as DataGridViewDoubleInputCell).Value = sData.Cnt;
            (rowData.Cells["colFT"] as DataGridViewDoubleInputCell).Value = sData.ForceTime;
            (rowData.Cells["colOT"] as DataGridViewDoubleInputCell).Value = sData.OffTime;
            (rowData.Cells["colClamp"] as DataGridViewDoubleInputCell).Value = sData.Clamp;
        }



        private SweepInfo GetRow(int rowNum)
        {
            SweepInfo sData = new SweepInfo();
            var rowData = dataGridViewX1.Rows[rowNum];
            sData.NPLC = double.Parse((rowData.Cells["colNPLC"] as DataGridViewDoubleInputCell).Value.ToString());
            sData.AutoMsrtRange = (bool)(rowData.Cells["colAutoRange"] as DataGridViewCheckBoxCell).Value;


            //使用 FormattedValue取值，否則會出現Value沒有被上下限卡下來的狀況
            ESweepMode mode = ESweepMode.Linear;
            string mStr = (rowData.Cells["colMode"] as DataGridViewTextBoxCell).FormattedValue.ToString();
            if(Enum.TryParse<ESweepMode>(mStr,out mode)) //(rowData.Cells["colMode"] as DataGridViewTextBoxCell).ToString(),out mode))
            {
                sData.Mode = mode;
            }

            sData.StartValue = double.Parse((rowData.Cells["colStart"] as DataGridViewDoubleInputCell).FormattedValue.ToString());
            sData.EndValue = double.Parse((rowData.Cells["colEnd"] as DataGridViewDoubleInputCell).FormattedValue.ToString());
            int cnt = int.Parse((rowData.Cells["colCnt"] as DataGridViewDoubleInputCell).FormattedValue.ToString());
            sData.Cnt = cnt;
            sData.ForceTime = double.Parse((rowData.Cells["colFT"] as DataGridViewDoubleInputCell).FormattedValue.ToString());
            sData.OffTime = double.Parse((rowData.Cells["colOT"] as DataGridViewDoubleInputCell).FormattedValue.ToString());
            sData.Clamp = double.Parse((rowData.Cells["colClamp"] as DataGridViewDoubleInputCell).FormattedValue.ToString());
            sData.ForceUnit = _forceUnit;
            sData.MsrtUnit = _msrtUnit;
            sData.TimeUnit = _timeUnit;
            return sData;
        }

        public  List<SweepInfo> GetSweepListFromDGV()
        {
            _sweepInfoList = new List<SweepInfo>();

            int rowCnt = dataGridViewX1.Rows.Count;
            for (int i = 0; i < rowCnt; ++i)
            {
                SweepInfo sInfo = GetRow(i);
                _sweepInfoList.Add(sInfo); 
            }
            return _sweepInfoList;
        }

        #endregion

        private void btnAdd_Click(object sender, EventArgs e)
        {
            AddRow();
            GetSweepListFromDGV();
        }

        private void btnDelet_Click(object sender, EventArgs e)
        {
            int rowQty =  dataGridViewX1.RowCount;
            if(rowQty > 0)
            {
                DeletRow(rowQty - 1);
            }
            GetSweepListFromDGV();
        }
    }

   
}

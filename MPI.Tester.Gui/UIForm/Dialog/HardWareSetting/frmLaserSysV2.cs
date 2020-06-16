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

using System.ComponentModel.Design;
using System.Drawing.Design;
using System.IO;
using System.Windows.Forms.Design;

using MPI.Tester.Data.LaserData.LaserSource;

namespace MPI.Tester.Gui.UIForm.Dialog.HardWareSetting
{
    public partial class frmLaserSysV2 : Form
    {
        private LaserSrcSysConfig _laserSrcSysConfig;
        int _nowRow = 0;

        public frmLaserSysV2()
        {
            InitializeComponent();
            _laserSrcSysConfig = new LaserSrcSysConfig();
            btnSave.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        public frmLaserSysV2(LaserSrcSysConfig ldfg):this()
        {
            _laserSrcSysConfig = ldfg.Clone() as LaserSrcSysConfig;
            DataToUI(_laserSrcSysConfig);
        }

        public LaserSrcSysConfig GetLaserSysConfig()
        {
            return _laserSrcSysConfig;
        }
        #region
        private void DataToUI(LaserSrcSysConfig lscfg)
        {
            if (lscfg != null && lscfg.ChConfigList != null)
            {

                dgvLaserSys.Rows.Clear();

                int rowCnt = 0;
                foreach (var chData in lscfg.ChConfigList)
                {
                    var cData = lscfg.ChConfigList[rowCnt];

                    dgvLaserSys.Rows.Add();
                    DataGridViewRow r = dgvLaserSys.Rows[rowCnt];

                    r.Cells["colSysCh"].Value = chData.SysChannel.ToString();
                    r.Cells["colDefault"].Value = cData.IsDefauleChannel;
                    r.Cells["colName"].Value = cData.ChannelName;
                    rowCnt++;
                }

                //chkAutoAttInPreheat.Checked = lscfg.IsAutoAttInPreheat;
                numAutoAttPerCheck.Value = lscfg.AutoAttPerCntCheck;
            }
        }

        #endregion


        private void btnAdd_Click(object sender, EventArgs e)
        {
            LaserSrcChConfig lsc =new LaserSrcChConfig();
            if (_laserSrcSysConfig.ChConfigList.Count > 0)
            {
                lsc.IsDefauleChannel = false;
            }
            _laserSrcSysConfig.ChConfigList.Add(lsc);

            DataToUI(_laserSrcSysConfig);
        }

        private void btnDelet_Click(object sender, EventArgs e)
        {
            int row = _nowRow;
            try{
                if(row >=0 &&row < _laserSrcSysConfig.ChConfigList.Count)
                {
                    _laserSrcSysConfig.ChConfigList.RemoveAt(row);
                    DataToUI(_laserSrcSysConfig);
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("[frmLaserSysV2],Exception " + ex.Message);
            }           

        }

        private void dgvLaserSys_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            _nowRow = e.RowIndex;
            int col = e.ColumnIndex;
            List<String> colNameList = new List<string>();
            for (int i = 0; i < dgvLaserSys.Columns.Count; ++i)
            { colNameList.Add(dgvLaserSys.Columns[i].Name); }
            string key = "";
            if(col >=0 && col < colNameList.Count)
            {
                key = colNameList[col];
            }

            switch (key)
            {
                case "colDefault":
                    {
                        int d4Row = 0;
                        if (!_laserSrcSysConfig.ChConfigList[_nowRow].IsDefauleChannel)
                        {
                            d4Row = _nowRow;
                        }
                        for (int i = 0; i < _laserSrcSysConfig.ChConfigList.Count; ++i)
                        {
                            if (i == d4Row)
                            {
                                _laserSrcSysConfig.ChConfigList[i].IsDefauleChannel = true;
                            }
                            else
                            { _laserSrcSysConfig.ChConfigList[i].IsDefauleChannel = false; }
                        }
                        DataToUI(_laserSrcSysConfig);
                    }
                    break;
                default:
                    {
                        if (_nowRow >= 0 && _laserSrcSysConfig.ChConfigList.Count >= _nowRow)
                        {
                            pgdChSet.SelectedObject = _laserSrcSysConfig.ChConfigList[_nowRow];
                        }
                    }
                    break;
            }
        }

        private void pgdChSet_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            DataToUI(_laserSrcSysConfig);
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog sf = new SaveFileDialog())
            {
                sf.FileName = "";
                sf.FilterIndex = 1;
                sf.Title = "Export Laser System Config";
                sf.Filter = "XML files (*.xml)|*.xml";

                if (sf.ShowDialog() == DialogResult.OK)
                {
                    LaserSrcSysConfig lsc = GetLaserSysConfig();

                    string pathAndFile = sf.FileName;
                    MPI.Xml.XmlFileSerializer.Serialize(lsc, pathAndFile);

                }
            }
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog of = new OpenFileDialog())
            {
                of.Title = "Import  Laser System Config";

                of.Filter = "XML files (*.xml)|*.xml";
                if (of.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    string pathAndFile = of.FileName;
                    LaserSrcSysConfig ldfg = MPI.Xml.XmlFileSerializer.Deserialize(typeof(LaserSrcSysConfig), pathAndFile) as LaserSrcSysConfig;
                    _laserSrcSysConfig = ldfg.Clone() as LaserSrcSysConfig;
                    DataToUI(_laserSrcSysConfig);
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            //_laserSrcSysConfig.IsAutoAttInPreheat = chkAutoAttInPreheat.Checked;
            _laserSrcSysConfig.AutoAttPerCntCheck =  (int)numAutoAttPerCheck.Value ;
        }



        
    }
}

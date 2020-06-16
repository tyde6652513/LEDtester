using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using MPI.Tester.Tools;
using MPI.Tester.DeviceCommon.DeviceLogger;

namespace MPI.Tester.Gui.UIForm.Dialog.HardWareSetting
{
    public partial class frmDevRelay : Form
    {
        Dictionary<string, DeviceRelayInfoBase> _snDevDic;

        public frmDevRelay()
        {
            _snDevDic = new Dictionary<string, DeviceRelayInfoBase>();
            InitializeComponent();
            btnSave.DialogResult = System.Windows.Forms.DialogResult.OK;

            if (DataCenter._uiSetting.AuthorityLevel == Data.EAuthority.Super)
            {
                dgvRealyCnt.Columns["colCnt"].ReadOnly = false;
                btnSave.Visible = true;
            }
            else
            {
                dgvRealyCnt.Columns["colCnt"].ReadOnly = true;
                btnSave.Visible = false;
            }
        }


        public frmDevRelay(Dictionary<string, object> SnDevDic):this()
        {
            if(SnDevDic != null)
            {
                foreach (var p in SnDevDic)
                {
                    if (p.Value is DeviceRelayInfoBase)
                    {
                        _snDevDic.Add(p.Key, p.Value as DeviceRelayInfoBase);
                    }
                }
                RefreshDGV(_snDevDic);
            }
        }
        #region
        private void RefreshDGV(Dictionary<string, DeviceRelayInfoBase> snDevDic)
        {
            if (snDevDic != null && _snDevDic != null)
            {
                dgvRealyCnt.Rows.Clear();
                foreach (var p in snDevDic)
                {
                    string sn = p.Key;
                    string devName = p.Key;
                    if (p.Value != null && p.Value.RelayCntDic != null)
                    {
                        foreach(var rcP in p.Value.RelayCntDic)//relay/Cnt
                        {
                            devName = p.Value.DeviceName;
                            string relay = rcP.Key;
                            long cnt = rcP.Value;
                            dgvRealyCnt.Rows.Add(new object[] { sn,devName, relay, cnt });
                          
                        }                        
                    }
                }
            }
        }

        private void GetDevRelayInfoFrmmDGV()
        {
            if (dgvRealyCnt != null)
            {
                for (int i = 0; i < dgvRealyCnt.RowCount; ++i)
                {
                    DataGridViewRow row = dgvRealyCnt.Rows[i];
                    string sn = dgvRealyCnt["colSN",i].Value as string;
                    string devName = dgvRealyCnt["colDeviceName", i].Value as string;
                    string relay = dgvRealyCnt["colRelay", i].Value as string;
                    long cnt = long.Parse((dgvRealyCnt["colCnt", i] as DevComponents.DotNetBar.Controls.DataGridViewDoubleInputCell).Value.ToString());

                    foreach (var p in _snDevDic)
                    {
                        if (p.Key == sn)
                        {
                            List<string> keyList = p.Value.RelayCntDic.Keys.ToList();
                            foreach (string rKey in keyList)//不能直接從KeyValPair去改數值，只好繞開從dictionary去處理
                            {
                                if (rKey == relay)
                                {
                                    p.Value.RelayCntDic[rKey] = cnt;
                                }                                
                            }
                        }
                    }
                }
 
            }
        }

        #endregion

        #region public method
        public Dictionary<string, DeviceRelayInfoBase> GetDevRelayInfo()
        {
            return _snDevDic;
        }
        #endregion

        private void btnSave_Click(object sender, EventArgs e)
        {
            GetDevRelayInfoFrmmDGV();
        }
    }
}

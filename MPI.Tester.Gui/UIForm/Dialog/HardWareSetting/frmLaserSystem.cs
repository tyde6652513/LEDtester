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
    public partial class frmLaserSystem : Form
    {
        private LaserSrcSysConfig _laserSrcSysConfig;
        public frmLaserSystem()
        {
            InitializeComponent();

            btnSave.DialogResult = System.Windows.Forms.DialogResult.OK;
            btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            _laserSrcSysConfig = DataCenter._machineConfig.LaserSrcSysConfig; ;
            pgdLaserSys.SelectedObject = _laserSrcSysConfig;

            pgdLaserSys.ExpandAllGridItems();
        }

        public frmLaserSystem(LaserSrcSysConfig lssCfg):this()
        {
            _laserSrcSysConfig = lssCfg.Clone() as  LaserSrcSysConfig;
            pgdLaserSys.SelectedObject = _laserSrcSysConfig;
            pgdLaserSys.ExpandAllGridItems();
        }


        public LaserSrcSysConfig GetLaserSysConfig()
        {
            return _laserSrcSysConfig; 
        }

        private void btnSave_Click(object sender, EventArgs e)
        {

        }
    }
    
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MPI.Tester.Data;

namespace MPI.Tester.Gui.UIForm.UIComponent
{
    public partial class frmSysPathSet : Form
    {
        UISetting _uisetting;

        frmPathInfo frmMapPath;

        frmPathInfo frmProductPath;

        frmPathInfo frmCoefTablePath;

        frmPathInfo frmCoefBackupPath;

        public frmSysPathSet()
        {
            InitializeComponent();
        }

        public frmSysPathSet(UISetting uiset):this()
        {

            _uisetting = uiset;

            frmProductPath = new frmPathInfo("Product Path", _uisetting.ProductPathInfo, false);

            frmCoefTablePath = new frmPathInfo("Coef Table Path", _uisetting.CoefTablePathInfo, false);

            frmCoefBackupPath = new frmPathInfo("Backup Coef Path", _uisetting.CoefBackupPathInfo, true);

            frmMapPath = new frmPathInfo("Map Path", _uisetting.MapPathInfo,true, true, false);


            SetFrmPos(frmProductPath, 0, 0);

            SetFrmPos(frmCoefTablePath, 0, 1);

            SetFrmPos(frmCoefBackupPath, 0, 2);

            SetFrmPos(frmMapPath, 0, 3);

        }
        public void Load(UISetting uiset)
        {
            _uisetting = uiset;

            frmProductPath.Load(_uisetting.ProductPathInfo);

            frmCoefTablePath.Load(_uisetting.CoefTablePathInfo);

            frmCoefBackupPath.Load(_uisetting.CoefBackupPathInfo);

            frmMapPath.Load(_uisetting.MapPathInfo);
        }

        public void Save()
        {
            frmMapPath.Save();

            frmCoefTablePath.Save();

            frmCoefBackupPath.Save();

            frmMapPath.Save();
        }

        private void SetFrmPos(frmPathInfo frm,int col, int row)
        {
            frm.TopLevel = false;

            tableLayoutPanel1.Controls.Add(frm, col, row);

            frm.Show();
        }
    }
}

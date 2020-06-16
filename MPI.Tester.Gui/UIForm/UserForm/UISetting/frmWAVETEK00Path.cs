using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel.Design;
using System.Drawing.Design;

using System.Reflection;
using MPI.Tester.Data;

namespace MPI.Tester.Gui.UIForm.UserForm.UISetting
{
    public partial class frmWAVETEK00Path : Form
    {
        const int PATH_QTY = 12;
        //PathArr _pArr;
        public frmWAVETEK00Path()
        {
            InitializeComponent();
        }

        public frmWAVETEK00Path(PathInfo[] pArr)
            : this()
        {
            SetData(pArr);

            //SetLabelColumnWidth(pgdPath, 10);
        }

        public void SetData(PathInfo[] pArr)
        {

            List<PathInfo> pList = new List<PathInfo>();

            //foreach (var data in pArr)
            //{
            //    pList.Add(data.Clone() as PathInfo);
            //}

            if (pArr != null)
            {
                for (int i = 0; i < PATH_QTY ; ++i)
                {
                    PathInfo p = new PathInfo();

                    if (i < pArr.Length)
                    {
                        p = pArr[i];
                    }

                    switch (i)
                    {
                        case 0:
                            p.PathName = "1_1.Wafer CP raw data (csv檔)";
                            break;
                        case 1:
                            p.PathName = "1_2.Wafer CP raw data to server (csv檔)";                            
                            break;
                        case 2:
                            p.PathName = "2_1.Wafer CP raw data (csv檔) WTK ";                            
                            break;
                        case 3:
                            p.PathName = "2_2.Wafer CP raw data to server (csv檔) WTK ";
                            break;
                        case 4:
                            p.PathName = "3_1.Wafer CP raw data (txt) ";
                            break;
                        case 5:
                            p.PathName = "3_2.Wafer CP raw data to server (txt) ";
                            break;
                        case 6:
                            p.PathName = "4_1.Wafer CP map file (CP1)";
                            break;
                        case 7:
                            p.PathName = "4_2.Wafer CP map file to server (CP1)";
                            break;
                        case 8:
                            p.PathName = "5_1.Wafer Inkless map file (WTK)";
                            break;
                        case 9:
                            p.PathName = "5_2.Wafer Inkless map file to server (WTK)";
                            break;
                        case 10:
                            p.PathName = "6_1.Wafer Inkless map file(csv)";
                            break;
                        case 11:
                            p.PathName = "6_2.Wafer Inkless map file  to server (csv)";
                            break;
                    }
                    pList.Add(p);
                }

                pgdPath.SelectedObject = pList.ToArray();
            }

        }

        public PathInfo[] GetPathArr()
        {

            PathInfo[] pArr = (pgdPath.SelectedObject as PathInfo[]).Clone() as PathInfo[];

            return pArr;
            //return 
        }


    }

}

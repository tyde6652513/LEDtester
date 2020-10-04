using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.IO;

using MPI.Tester.Report.User.Accelink;

namespace MPI.Tester.Gui.UIForm.UserForm.Condition
{
    public partial class frmAccelinkCondition : Form, IfrmCusConditin
    {

        const string EXTEND = "accMap";
        string loadFilePath = "";
        public frmAccelinkCondition()
        {
            InitializeComponent();
        }

        #region public method
        public bool Save()
        {
            DataCenter._product.CustomerizedSetting.IsMergeReport = chkMergeFile.Checked;
            string path = GetcustomizePath();
            DataCenter._product.CustomerizedSetting.CustomerSetFilePath = path;
            Console.WriteLine("[frmAccelinkCondition],Save(),IsMergeReport:" + chkMergeFile.Checked.ToString());
            Console.WriteLine("[frmAccelinkCondition],Save(),CustomerSetFilePath:" + path);
            DataCenter.SaveProductFile();


            DGVToCSV(path);
            return true;
        }

        public bool Refresh()
        {
            loadFilePath = DataCenter._product.CustomerizedSetting.CustomerSetFilePath;
            if (File.Exists(loadFilePath))
            {
                CSVToDGV(loadFilePath);
            }
            chkMergeFile.Checked = DataCenter._product.CustomerizedSetting.IsMergeReport;
            
            return true;
        }

        #endregion

        #region
        private void DGVToCSV(string tarFileName)
        {
            
            if (DGVsetting.Rows != null && DGVsetting.RowCount > 0)
            {
                using (StreamWriter sw = new StreamWriter(tarFileName))
                {
                    for (int i = 0; i < DGVsetting.RowCount; ++i)
                    {
                        try
                        {
                            string name = DGVsetting["colName", i].Value.ToString();
                            bool isPass = (bool)(DGVsetting["colShow", i]).Value;
                            string str = name + "," + isPass.ToString();
                            sw.WriteLine(str);
                            sw.Flush();


                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("[frmAccelinkCondition],DGVToCSV(),Exception:" + e.Message);
                        }
                    }
                }
            }
        }

        private void CSVToDGV(string srcFileName)
        {

            DGVsetting.Rows.Clear();
            try
            {
                using (StreamReader sr = new StreamReader(srcFileName))
                {
                    int row = 0;
                    while (sr.Peek() >= 0)
                    {
                        string str = sr.ReadLine();
                        string[] strArr = str.Split(',');
                        if (strArr != null && strArr.Length == 2)
                        {
                            bool isShow = true;
                            if (bool.TryParse(strArr[1], out isShow))
                            {
                                DGVsetting.Rows.Add("", false);
                                DGVsetting["colName", row].Value = strArr[0];
                                DGVsetting["colShow", row].Value = isShow;
                                row++;
                            }
                        }
                        
                    }

                }
            }
            catch (Exception e)
            {
                Console.WriteLine("[frmAccelinkCondition],CSVToDGV(),Exception:" + e.Message);
            }
        }


        private string GetcustomizePath()
        {
            string productFolder = DataCenter._uiSetting.ProductPath;

            string fileNameWithExt = DataCenter._uiSetting.ProductFileName + "." + EXTEND;

            string path = Path.Combine(productFolder, fileNameWithExt);
            return path;
        }

        private string SelctReadFile()
        {
            string filePath = "";

            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    filePath = ofd.FileName;
                }
            }
            return filePath;
        }

        private string SelctSaveFile()
        {
            string filePath = "";

            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "JPeg Image|*.jpg|Bitmap Image|*.bmp|PNG Image|*.png";
                if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    filePath = sfd.FileName;
                }
            }
            return filePath;
        }

        private List<SelectItem> DGVToList()
        {
            List<SelectItem> sList = new List<SelectItem>();
            if (DGVsetting.Rows != null && DGVsetting.RowCount > 0)
            {
                for (int i = 0; i < DGVsetting.RowCount; ++i)
                {
                    try
                    {
                        SelectItem sItem = new SelectItem();
                        sItem.Name = DGVsetting["colName", i].Value.ToString();
                        var ce1 = DGVsetting["colShow", i];
                        sItem.ShowOnMap = (bool)(DGVsetting["colShow", i]).Value;
                        sList.Add(sItem);
                    }
                    catch (Exception e)
                    { }
                }
            }

            return sList;
        }
        

        #endregion

        private void chkMergeFile_CheckStateChanged(object sender, EventArgs e)
        {
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            DGVsetting.Rows.Add("", false);

        }

        private void btnDelet_Click(object sender, EventArgs e)
        {
            int rowCnt = DGVsetting.RowCount;
            if (rowCnt >= 0)
            {
                DGVsetting.Rows.RemoveAt(rowCnt - 1);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Save();
        }

        private void btnConvert_Click(object sender, EventArgs e)
        {
            Save();
            string srcFile = SelctReadFile();
            string tarFile = SelctSaveFile();
            string settingFile = GetcustomizePath();
            MPI.Tester.Report.User.Accelink.AccelinkMapCreator.CreateAcceLinkWaferMap(srcFile, tarFile, settingFile);

        }

    }


    //public class SelectItem
    //{
    //    public string Name { get; set; }
    //    public bool ShowOnMap { get; set; }

    //    public SelectItem()
    //    {
    //        Name = "";
    //        ShowOnMap = false;
    //    }

    //    public SelectItem(string name, bool show)
    //    {
    //        Name = name;
    //        ShowOnMap = show;
    //    }

    //}
}

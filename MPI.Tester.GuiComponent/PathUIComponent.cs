using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;

//using MPI.Tester.Data;
namespace MPI.Tester.GuiComponent
{

    public partial class PathUIComponent : UserControl
    {
        

        PathInfo _pInfo;
        bool _isShowType;
        public PathUIComponent()
        {
            InitializeComponent();
            _isShowType = true;
            this.cmbFolder.Items.AddRange(Enum.GetNames(typeof(ETesterResultCreatFolderType)));

            //this.cmbFolder.SelectedIndex = (int)ETesterResultCreatFolderType.None;
        }


        public PathUIComponent(PathInfo pInfo)
            : this()
        {
            _pInfo = pInfo.Clone() as PathInfo;
        }


        private void button1_Click(object sender, EventArgs e)
        {
            string path = this.SelectPath("Test Output Path");
            if (path != string.Empty)
            {
                this.txtPath.Text = path;
            }
        }

        #region >>private method<<
        private string SelectPath(string title)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();

            folderBrowserDialog.Description = title;
            folderBrowserDialog.RootFolder = Environment.SpecialFolder.Desktop;
            //folderBrowserDialog.SelectedPath = Constants.Paths.ROOT;

            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                return folderBrowserDialog.SelectedPath;
            }
            else
            {
                return string.Empty;
            }
        }

        private void GetDataFromUI()
        {
            _pInfo = new PathInfo();
            _pInfo.EnablePath = chkEnable.Checked;
            _pInfo.FileExt = "csv";
            ETesterResultCreatFolderType fType = ETesterResultCreatFolderType.None;
            if (cmbFolder.SelectedItem != null && Enum.TryParse(cmbFolder.SelectedItem.ToString(), out fType))
            {
                _pInfo.FolderType = fType;
            }

            _pInfo.TestResultPath = txtPath.Text;
            _pInfo.PathName = lblPathName.Text;

        }

        private void SetDataToUI()
        {
            if (_pInfo != null)
            {
                if (chkEnable.Enabled)
                {
                    chkEnable.Checked = _pInfo.EnablePath;
                }
                else
                {
                    chkEnable.Checked = true;
                }

                this.cmbFolder.SelectedIndex = (int)_pInfo.FolderType;

                txtPath.Text = _pInfo.TestResultPath;
                lblPathName.Text = _pInfo.PathName;
                
            }
        }
        #endregion
        [Browsable(false)]
        public PathInfo PathInfomation
        {
            get
            {
                GetDataFromUI();
                return _pInfo;
            }
            set
            {
                string str = "";
                if (_pInfo != null)
                {
                    str = _pInfo.PathName;
                }                
                _pInfo = value.Clone() as PathInfo;
                if (str != "")
                {
                    _pInfo.PathName = str;
                }
                SetDataToUI();

            }
        }


        //[Editor(typeof(MyTypeEditor), typeof(UITypeEditor))]  
        public string PathName
        {
            get { return lblPathName.Text; }
            set
            {
                lblPathName.Text = value;
                if (_pInfo != null)
                {
                    _pInfo.PathName = value;
                }
            }
        }

        public bool IsChkEnableEditable
        {
            get { return chkEnable.Enabled; }
            set
            {
                chkEnable.Enabled = value;
                if (!value)
                {
                    chkEnable.Checked = true;
                }
            }
        }


        public bool IsEnableCheckEn
        {
            get { return chkEnable.Enabled; }
            set
            {
                chkEnable.Enabled = value;
                if (!value)
                {
                    chkEnable.Checked = true;
                }
            }
        }

        public bool IsShowType
        {
            get { return _isShowType; }
            set
            {
                _isShowType = value;
                if (_isShowType)
                { cmbFolder.Visible = true;
                this.Width = 700;
                }
                else
                {
                    cmbFolder.Visible = false;
                    cmbFolder.SelectedIndex = (int)ETesterResultCreatFolderType.None;
                    this.Width = 560;
                }
            }

        }


    }

    //使用x64編譯有問題
    //調用x64 MPI.Tester.Data也會有問題
    //只好在這邊建立一個local 端的，在UI層在另外做資料交換
    
    public class PathInfo
    {
        #region

        private string _path = "";
        public bool EnablePath { set; get; }

        //[EditorAttribute(typeof(System.Windows.Forms.Design.FolderNameEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public string TestResultPath {
            set
            {
                string tmpStr = value;//.TrimStart('\\');
                _path = tmpStr;
            }
            get { return _path; }
        }

        public ETesterResultCreatFolderType FolderType { set; get; }

        public string FileExt { set; get; }
        //[BrowsableAttribute(false)]
        public string PathName { set; get; }
        #endregion

        #region
        public PathInfo()
        {
            EnablePath = false;
            TestResultPath = "";//Constants.Paths.MPI_TEMP_DIR;
            FolderType = ETesterResultCreatFolderType.None;
            FileExt = ".csv";
            PathName = "Path";
        }
        public PathInfo(bool enable, string path, ETesterResultCreatFolderType fType = ETesterResultCreatFolderType.None, string fileExt = ".csv")
        {
            EnablePath = enable;
            TestResultPath = path;
            FolderType = fType;
            FileExt = fileExt;
        }

        #endregion

        public object Clone()
        {
            PathInfo obj = new PathInfo();
            obj = this.MemberwiseClone() as PathInfo;
            obj.FileExt = this.FileExt;
            obj.TestResultPath = this.TestResultPath;
            return obj;
        }
    }

    public enum ETesterResultCreatFolderType : int
    {
        [XmlEnum(Name = "0")]
        None = 0,

        [XmlEnum(Name = "1")]
        ByLotNumber = 1,

        [XmlEnum(Name = "2")]
        ByMachineName = 2,

        [XmlEnum(Name = "3")]
        ByDataTime = 3,

        [XmlEnum(Name = "4")]
        ByWaferID = 4,

        [XmlEnum(Name = "5")]
        ByLotNumber_WaferID = 5,
    }
    
}

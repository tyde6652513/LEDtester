using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.IO;

using MPI.Tester.Data;

namespace MPI.Tester.Gui
{
    public partial class frmDefaultValueSetting : Form
    {
        private Dictionary<string, string> _reultItemDefaultValue = new Dictionary<string, string>();

        private Dictionary<string, string> _dicMrstItemDefaultValue = new Dictionary<string, string>();


        public frmDefaultValueSetting()
        {
            InitializeComponent();

            this.InitailizeDGV();
        }


        private void InitailizeDGV()
        {
            DataGridViewCellStyle columnHeaderStyle = new DataGridViewCellStyle();
            DataGridViewCellStyle columnCellStyle = new DataGridViewCellStyle();

            columnHeaderStyle.BackColor = Color.Beige;
            columnHeaderStyle.Font = new Font("Verdana", 9, FontStyle.Bold);
            columnHeaderStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            columnCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;


            //------- ItemList --------

            this.dgvDefaultValue.ColumnHeadersDefaultCellStyle = columnHeaderStyle;
            //this.dgvDefaultValue.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            this.dgvDefaultValue.DataSource = null;
            this.dgvDefaultValue.ReadOnly = false;
            this.dgvDefaultValue.MultiSelect = false;
            this.dgvDefaultValue.AllowUserToAddRows = false;
            this.dgvDefaultValue.AllowUserToDeleteRows = false;
            this.dgvDefaultValue.AllowUserToResizeRows = false;
            this.dgvDefaultValue.AllowUserToResizeColumns = false;
            this.dgvDefaultValue.AllowUserToOrderColumns = false;
            this.dgvDefaultValue.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvDefaultValue.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvDefaultValue.Refresh();

            if (!this.LoadUserXml())
            {
                return;
            }

            this.GetResultItem();

            this.UpdateDGV();
                        
        }

        private void GetResultItem()
        {
            foreach(string keyName in DataCenter._uiSetting.UserDefinedData.ResultItemNameDic.Keys)
            {
                if (this._dicMrstItemDefaultValue.ContainsKey(keyName))
                {
                    this._reultItemDefaultValue.Add(keyName, this._dicMrstItemDefaultValue[keyName]);
                }
            }                        
        }

        private void SaveResultItemDefaultValue()
        {
            foreach (var resultItem in DataCenter._uiSetting.UserDefinedData.ResultItemNameDic)
            {
                for (int i = 0; i < this.dgvDefaultValue.Rows.Count; i++ )
                {
                    if (resultItem.Value == this.dgvDefaultValue.Rows[i].Cells[0].Value.ToString())
                    {
                        if (this.dgvDefaultValue.Rows[i].Cells[1].Value != null)
                        {
                            this._reultItemDefaultValue[resultItem.Key] = this.dgvDefaultValue.Rows[i].Cells[1].Value.ToString();
                        }
                        else
                        {
                            this._reultItemDefaultValue[resultItem.Key] = "";
                        }
                        break;
                    }
                }
                
            }
        }

        private bool LoadUserXml()
        {
            XmlDocument xmlDoc = new XmlDocument();
            string fileNameWithExt = string.Format("{0}{1}", "User", ((int)DataCenter._uiSetting.UserID).ToString("0000")) + ".xml";
            string pathAndFile = Path.Combine(Constants.Paths.USER_DIR, fileNameWithExt);

            if (File.Exists(pathAndFile))
            {
                xmlDoc.Load(pathAndFile);
            }
            else
            {                
                return false;
            }
                       
            XmlNodeList nodes = xmlDoc.SelectNodes("/UserDefine/Formats/*");

            int nodeCount = 1;

            foreach (XmlNode node in nodes)
            {
                if (DataCenter._uiSetting.FormatName == (node as XmlElement).GetAttribute("name")) 
                {
                    break;
                }

                nodeCount++;
            }

            XmlNodeList MsrtDisplayItem = xmlDoc.SelectNodes("/UserDefine/Formats/*[" + nodeCount.ToString() + "]" + "/MsrtDisplayItem/*");
                       
            foreach (XmlNode item in MsrtDisplayItem)
            {               
                this._dicMrstItemDefaultValue.Add(item.InnerText, (item as XmlElement).GetAttribute("nullValue"));
            }

            return true;
        }

        private bool SaveUserXml()
        {
            XmlDocument xmlDoc = new XmlDocument();
            string fileNameWithExt = string.Format("{0}{1}", "User", ((int)DataCenter._uiSetting.UserID).ToString("0000")) + ".xml";
            string pathAndFile = Path.Combine(Constants.Paths.USER_DIR, fileNameWithExt);

            if (File.Exists(pathAndFile))
            {
                xmlDoc.Load(pathAndFile);
            }
            else
            {
                return false;
            }

            XmlNodeList nodes = xmlDoc.SelectNodes("/UserDefine/Formats/*");

            int nodeCount = 1;

            foreach (XmlNode node in nodes)
            {
                if (DataCenter._uiSetting.FormatName == (node as XmlElement).GetAttribute("name"))
                {
                    break;
                }

                nodeCount++;
            }

            XmlNodeList MsrtDisplayItem = xmlDoc.SelectNodes("/UserDefine/Formats/*[" + nodeCount.ToString() + "]" + "/MsrtDisplayItem/*");

            foreach (var resultItem in this._reultItemDefaultValue)
            {
                foreach (XmlNode MrstItem in MsrtDisplayItem)
                {
                    if (MrstItem.InnerText == resultItem.Key)
                    {
                        if (MrstItem.Attributes.GetNamedItem("nullValue") == null)
                        {
                            return false;
                        }

                        MrstItem.Attributes["nullValue"].InnerText = resultItem.Value;
                    }                       

                }
            }

            xmlDoc.Save(pathAndFile);

            return true;
        }

        private void UpdateDGV()
        {
            this.dgvDefaultValue.Rows.Clear();

            int row = 0;

            string itemName;

            foreach (var resultItem in this._reultItemDefaultValue)
            {
                itemName = DataCenter._uiSetting.UserDefinedData.ResultItemNameDic[resultItem.Key];

                this.dgvDefaultValue.Rows.Add(itemName);

                this.dgvDefaultValue.Rows[row].Cells[1].Value = resultItem.Value;

                row++;
            }

        }
        
                
        private void btnSaveSetting_Click(object sender, EventArgs e)
        {
            this.SaveResultItemDefaultValue();

            this.SaveUserXml();

            this.Close();
        }
    }
}

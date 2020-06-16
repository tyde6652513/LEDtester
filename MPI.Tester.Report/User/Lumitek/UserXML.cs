using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MPI.Tester.Data;

namespace MPI.Tester.Report.User.Lumitek
{
    class UserXML
    {
        private string _formatName;
        private Dictionary<string, string> _xmlResultItem = new Dictionary<string, string>();
        private Dictionary<string, string> _xmlMsrtDisplayItem = new Dictionary<string, string>();

        public string FormatName
        {
            get { return this._formatName; }
            set { this._formatName = value; }
        }

        public Dictionary<string, string> XmlResultItem
        {
            get { return this._xmlResultItem; }
            set { this._xmlResultItem = value; }
        }

        public Dictionary<string, string> XmlMsrtDisplayItem
        {
            get { return this._xmlMsrtDisplayItem; }
            set { this._xmlMsrtDisplayItem = value; }
        }


        public void Clear()
        {
            this._formatName = "";
            this._xmlResultItem.Clear();
            this._xmlMsrtDisplayItem.Clear();
        }
    }
}

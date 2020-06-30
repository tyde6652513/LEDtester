using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace MPI.RemoteControl2.Tester.Command.TransferableData.Basic
{
    [Serializable]
    public class RefPoint
    {
        private static XmlDocument _xmlDocument = new XmlDocument();

        [XmlIgnore]
        private int _baseX;
        [XmlIgnore]
        private int _baseY;
        [XmlIgnore]
        private int _newX;
        [XmlIgnore]
        private int _newY;
        [XmlIgnore]
        private string _chipName;
        [XmlIgnore]
        private string _remark;

        [XmlElement]
        public int BaseX
        {
            get { return _baseX; }
            set { _baseX = value; }
        }
        [XmlElement]
        public int BaseY
        {
            get { return _baseY; }
            set { _baseY = value; }
        }
        [XmlElement]
        public int NewX
        {
            get { return _newX; }
            set { _newX = value; }
        }
        [XmlElement]
        public int NewY
        {
            get { return _newY; }
            set { _newY = value; }
        }
        [XmlElement]
        public string ChipName
        {
            get { return _chipName; }
            set { _chipName = value; }
        }
        [XmlElement]
        public string Remark
        {
            get { return _remark; }
            set { _remark = value; }
        }

        public RefPoint()
        {
            _baseX = 0;
            _baseY = 0;
            _newX = 0;
            _newY = 0;
            _chipName = "";
            _remark = "";
        }

        public RefPoint(int bx, int by, int nx, int ny, string chipName)
            : this()
        {
            _baseX = bx;
            _baseY = by;
            _newX = nx;
            _newY = ny;
            _chipName = chipName;
        }
    }

    [Serializable]
    public class RefPointsTable
    {
        private List<RefPoint> _table;

        [XmlElement("RefPointTable")]
        public List<RefPoint> Table
        {
            get { return _table; }
            set { _table = value; }
        }

        public RefPointsTable()
        {
            _table = new List<RefPoint>();
        }
    }
}

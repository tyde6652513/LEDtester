using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace MPI.Tester.DeviceCommon.DeviceLogger
{
    [Serializable]
    public class DeviceRelayInfoBase : ICloneable,IXmlSerializable
    {
        public DeviceRelayInfoBase()
        {
            Console.WriteLine("[DeviceRelayInfoBase],DeviceRelayInfoBase()");
            RelayCntDic = new Dictionary<string, long>();
            DeviceName = "";
        }
        /// <summary>
        /// //輸出channel = 可獨立切換的switch數
        /// </summary>
        /// <param name="outputChannel"> 1 base</param>
        public DeviceRelayInfoBase(string name,int outputChannel)
            : this()
        {
            DeviceName = name;
            for (int i = 1; i <= outputChannel; ++i)//1 base
            {
                RelayCntDic.Add(i.ToString(), 0);
            }
        }
        /// <summary>
        ///列舉Relay名稱
        /// </summary>
        /// <param name="strList">列舉Relay名稱</param>
        public DeviceRelayInfoBase(string name, List<string> strList)
            : this()
        {
            DeviceName = name;
            for (int i = 1;strList!= null&& i <= strList.Count; ++i)
            {
                RelayCntDic.Add(strList[i], 0);
            }
        }

        #region >>private object<<
        //List<int> _chCntList;
        object _lock = new object();
        Dictionary<string, long> _relayCntDic;//考量到後面可能會有High/Low Voltage或是其他非數字能表達的relay，因此直接使用string 作為識別
        #endregion

        #region >>public property<<
        public Dictionary<string, long> RelayCntDic { get { return _relayCntDic; } set { lock (_lock) { _relayCntDic = value; } } }
        public string DeviceName { get; set; }
        #endregion

        #region >>public method<<

        public XmlSchema GetSchema()
        { return null; }
        public void ReadXml(XmlReader reader)
        {
            RelayCntDic = new Dictionary<string, long>();
            reader.Read();
            if (reader.MoveToContent() == XmlNodeType.Element && reader.Name == "DeviceName")
            {
                DeviceName = reader.Name;
                while (reader.Read())
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "RelayCounter")
                    {
                        string key = reader.GetAttribute("name");
                        long cnt = reader.ReadElementContentAsLong();
                        if (!RelayCntDic.ContainsKey(key))
                        {
                            RelayCntDic.Add(key, 0);
                        }
                        RelayCntDic[key] = cnt;
                    }
            }
        }
        public void WriteXml(XmlWriter writer)
        {
            writer.WriteStartElement("DeviceName");
            writer.WriteValue(DeviceName);            
            
            foreach (var p in RelayCntDic)
            {
                writer.WriteStartElement("RelayCounter");
                writer.WriteAttributeString("name", p.Key);
                writer.WriteValue(p.Value);
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
        }
        public void ChCntAddOnce(int ch)
        {
            ChCntAddOnce(ch.ToString());
        }
        public void ChCntAddOnce(string ch)
        {
            if (RelayCntDic != null)
            {
                if (!RelayCntDic.ContainsKey(ch))
                {
                    RelayCntDic.Add(ch, 0);
                }
                RelayCntDic[ch]++;
            }
        }
        public object Clone()
        {
            DeviceRelayInfoBase obj = new DeviceRelayInfoBase();
            if (RelayCntDic != null)
            {
                foreach (var p in RelayCntDic)
                {
                    obj.RelayCntDic.Add(p.Key, p.Value);
                }
            }
            obj.DeviceName = DeviceName;
            return obj;

        }


        #endregion



    }
}

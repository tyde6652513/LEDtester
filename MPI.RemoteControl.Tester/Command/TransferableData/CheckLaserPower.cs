using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace MPI.RemoteControl.Tester
{
    public class CheckLaserPower : TransferableCommonObjectBase
    {
        private static XmlDocument s_document = new XmlDocument();
        private static XmlSerializer s_serializer = null;

#region public property
        public bool AutoSetLaserPower{set;get;}//自動調整強度，先給false
        public bool IsPowerCheckPass { set; get; }//T->P回應確認是否成功
        public string Remark { get; set; }//臨時擴充用，先填空就好
#endregion


        public CheckLaserPower()
            : base()
        {
            if (s_serializer == null)
                s_serializer = new XmlSerializer(this.GetType());

            this.InitializeData();
        }

        #region >>private/protected method<<

        private void InitializeData()
        {
            AutoSetLaserPower = false;
            IsPowerCheckPass = false;
            Remark = "";
        }

        protected override bool DeserializeCommonObject(string context)
        {
            using (StringReader sr = new StringReader(context))
            {
                try
                {
                    CheckLaserPower data = s_serializer.Deserialize(sr) as CheckLaserPower;
                    if (data != null)
                    {
                        InitializeData();

                        this.CopyFrom(data);

                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch
                {
                    return false;
                }
            }
        }

        protected override string SerializeCommonObject()
        {
            StringBuilder sb = new StringBuilder();
            using (StringWriter sw = new StringWriter(sb))
            {
                s_serializer.Serialize(sw, this);

                return sw.ToString();
            }
        }

        protected override int ObjectIdentifiedID
        {
            get
            {
                return (int)ETransferableCommonObject.CheckLaserPower;
            }
        }
        #endregion

        #region >>public method<<
        public CheckLaserPower Clone()
        {
            CheckLaserPower info = new CheckLaserPower();

            return this.CopyTo(info) ? info : null;
        }

        public bool CopyTo(CheckLaserPower info)
        {
            if (info == null) return false;

            info = this.MemberwiseClone() as CheckLaserPower ;

            return true;
        }

        public bool CopyFrom(CheckLaserPower info)
        {
            if (info == null) return false;

            this.AutoSetLaserPower = info.AutoSetLaserPower;
            this.IsPowerCheckPass = info.IsPowerCheckPass;
            this.Remark = info.Remark.ToString();
            return true;
        }
        #endregion 

    
    }
}

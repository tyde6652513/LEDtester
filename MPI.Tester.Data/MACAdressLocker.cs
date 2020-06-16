using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.IO;

using System.Net.NetworkInformation;

using MPI.Tester;

namespace MPI.Tester.Data
{
    public class MACAdressLocker
    {
        private Dictionary<string, MACLockerInfo> _macAdressTable = new Dictionary<string, MACLockerInfo>();

        private string path = @"C:\MPI\LEDTester\Data\MACdata.dat";

        private bool _isExist = false;

        public MACAdressLocker()
        {
            ReadBinaryFile();
        }

        public MACLockerInfo GetMacAdressStatus()
        {
            if (!_isExist)
            {
                return new MACLockerInfo("000000000000", 8.0d, 15.0d);
            }

            if (_macAdressTable.Count == 0)
            {
                return null;
            }

            MACLockerInfo mac = new MACLockerInfo("000000000000", 8.0d, 15.0d);

            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();

            List<string> macList = new List<string>();

            foreach (var nic in nics)
            {
                // 因為電腦中可能有很多的網卡(包含虛擬的網卡)，
                // 只需要 Ethernet 網卡的 MAC
                if (nic.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
                {
                    string PhysicalAddress = nic.GetPhysicalAddress().ToString();

                    if (this._macAdressTable.ContainsKey(PhysicalAddress))
                    {
                        mac = this._macAdressTable[PhysicalAddress];

                        return mac;
                    }
                }
            }

            return mac;
        }

        private void ReadBinaryFile()
        {
            if (!File.Exists(path))
            {
                this._isExist = false;
                return;
            }

            this._isExist = true;

            List<string> readData = new List<string>();

            _macAdressTable.Clear();

            try
            {
                using (StreamReader fileReader = new StreamReader(path, Encoding.Default))
                {
                    while (!fileReader.EndOfStream)
                    {
                        string strLine = fileReader.ReadLine();

                        readData.Add(MPIFile.BinaryToString(strLine));
                    }

                    fileReader.Close();
                }

                foreach (string s in readData)
                {
                    string[] row = s.Split(',');

                    if (row.Length > 0)
                    {
                        string macAdress = row[0];

                        double delay = 0.0d;

                        double.TryParse(row[2], out delay);

                        double ESDdelay = 0.0d;

                        double.TryParse(row[3], out ESDdelay);


                        double esdChangeDealy = 0.0d;

                        double.TryParse(row[6], out esdChangeDealy);


                        if (macAdress.Contains("-"))
                        {
                            macAdress = macAdress.Replace("-", "");
                        }

                        if (!_macAdressTable.ContainsKey(macAdress))
                        {
                            MACLockerInfo mci = new MACLockerInfo(macAdress, delay, ESDdelay);

                            mci.EsdVoltChangeDelay = esdChangeDealy;

                            _macAdressTable.Add(macAdress, mci);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }

    public class MACLockerInfo
    {
        private string _macAdressSN;
        private double _delay;
        private double _edelay;
        private bool _isESDHighSpeedMode;

        private double _esdVoltChangeDelay;

        public MACLockerInfo()
        {
            _macAdressSN = "000000000000";
            _delay = 8.0d;
            _edelay = 15.0d;
            _isESDHighSpeedMode = false;

            _esdVoltChangeDelay = 100.0d;
        }

        public MACLockerInfo(string macAdressSN, double delay, double edelay)
        {
            _macAdressSN = macAdressSN;
            _delay = delay;
            _edelay = edelay;

            if (_edelay < 15)
            {
                _isESDHighSpeedMode = true;
            }
        }

        public string MacAdressSN
        {
            get { return this._macAdressSN; }
            set { this._macAdressSN = value; }
        }

        public double Delay
        {
            get { return this._delay; }
            set { this._delay = value; }
        }


        public double ESDDeleay
        {
            get { return this._edelay; }
            set { this._edelay = value; }
        }

        public bool IsESDHighSpeedMode
        {
            get { return this._isESDHighSpeedMode; }
            set { this._isESDHighSpeedMode = value; }
        }

        public double EsdVoltChangeDelay
        {
            get { return this._esdVoltChangeDelay; }
            set { this._esdVoltChangeDelay = value; }
        }


    }
}

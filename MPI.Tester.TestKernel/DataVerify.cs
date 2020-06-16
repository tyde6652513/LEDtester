using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MPI.Tester.Data;
using MPI.Tester.CompoCommon;
using MPI.Tester.Compo.DIDOCard;
using MPI.Tester.Compo.ADCard;

using MPI.Tester.DeviceCommon;
using MPI.Tester.Device.SourceMeter;
using MPI.Tester.Device.SpectroMeter;
using MPI.Tester.Device.ESD;
using MPI.Tester.Maths;


namespace MPI.Tester.TestKernel
{
    public class DataVerify
    {
        private List<Dictionary<string, float>> _recordData;
        private const int CONSECUTIVE_ERROR_COUNT = 10;

        public DataVerify()
        {
            _recordData = new List<Dictionary<string, float>>(3);

        }

        public void Start()
        {
            _recordData.Clear();
        }

        public bool Push(TestItemData[] data)
        {
            Dictionary<string, float> result = new Dictionary<string, float>();

            foreach (TestItemData item in data)
            {
                if (!item.IsEnable)
                    continue;

                if (item.MsrtResult != null)
                {
                    for (int i = 0; i < item.MsrtResult.Length; i++)
                    {
                        result.Add(item.MsrtResult[i].KeyName, (float)item.MsrtResult[i].Value);
                    }
                }
            }

            if (this._recordData.Count >= CONSECUTIVE_ERROR_COUNT)
            {
                this._recordData.RemoveAt(0);
            }

            this._recordData.Add(result);

            return Run();

        }

        private bool Run()
        {
            List<float> ints = new List<float>();
           // List<float> vfs = new List<float>();

            foreach (var item in _recordData)
            {
                if (item.ContainsKey("INT_1"))
                {
                    ints.Add(item["INT_1"]);
                }
            }

            if (ints.Count < CONSECUTIVE_ERROR_COUNT)
            {
                return true;
            }

            float deltaInt = ints.Max() - ints.Min();

            if (deltaInt == 0.0f || ints.Max() == 0.0f)
            {
                this._recordData.Clear();
                return false;
            }

            //if (vfs.Average() == 0.0f)
            //{
            //    this._recordData.Clear();
            //    return false;
            //}

            //if (vfs.Average() < 4.0f && vfs.Max() - vfs.Min() == 0.0f)
            //{
            //    this._recordData.Clear();
            //    return false;
            //}
            return true;
        }


    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MPI.Tester.DeviceCommon;

namespace MPI.Tester.Data
{
    [Serializable]
    public class LOPWLParameter
    {
        private const int COEF_COUNT = 10;

        private string _keyName;

        private object _lockObj;

        private double[][] _coefTable;

        private double _coefWLResolution;

        private double _coefStartWL;

        private double _coefEndWL;

        private OptiSettingData _optiSetting;

        public LOPWLParameter()
        {
            this._lockObj = new object();

            this._coefStartWL = 200.0d;

            this._coefEndWL = 1200.0d;

            this._coefWLResolution = 1.0d;

            this.CreateTable();

            this._optiSetting = new OptiSettingData();
        }

        public LOPWLParameter(string key)
            : this()
        {
            this._keyName = key;
        }


        private void CreateTable()
        {
            double value = 0.0d;

            if (this._coefStartWL >= this._coefEndWL || this._coefStartWL < 0 || this._coefEndWL < 0)
            {
                return;
            }

            if (this._coefWLResolution < 0)
            {
                return;
            }

            UInt32 items = (UInt32)Math.Floor((this._coefEndWL - this._coefStartWL) / this._coefWLResolution) + 1;

            this._coefTable = new double[items][];

            for (int i = 0; i < items; i++)
            {
                value = this._coefStartWL + i * this._coefWLResolution;
                this._coefTable[i] = new double[COEF_COUNT] { value, 0.0d, 0.0d, 0.0d, 1.0d, 1.0d, 1.0d, 0.0d, 0.0d, 0.0d };
            }
        }

        public string KeyName
        {
            get
            {
                return this._keyName;
            }
            set
            {
                lock (this._lockObj)
                {
                    this._keyName = value;
                }
            }
        }

        public double[][] CoefTable
        {
            get
            {
                return this._coefTable;
            }
            set
            {
                lock (this._lockObj)
                {
                    this._coefTable = value;
                }
            }
        }

        public double CoefStartWL
        {
            get
            {
                return this._coefStartWL;
            }
            set
            {
                lock (this._lockObj)
                {
                    this._coefStartWL = value;
                }
            }
        }

        public double CoefEndWL
        {
            get
            {
                return this._coefEndWL;
            }
            set
            {
                lock (this._lockObj)
                {
                    this._coefEndWL = value;
                }
            }
        }

        public OptiSettingData OptiSetting
        {
            get { return this._optiSetting; }
            set { lock (this._lockObj) { this._optiSetting = value; } }
        }

        public double CoefWLResolution
        {
            get
            {
                return this._coefWLResolution;
            }
            set
            {
                lock (this._lockObj)
                {
                    this._coefWLResolution = value;
                }
            }
        }

    }
}

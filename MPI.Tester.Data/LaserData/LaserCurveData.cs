using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MPI.Tester.Data
{
    public class LaserCurveData
    {
        private object _lockObj;

        private List<double> _seArray;
        private List<double> _rsArray;
        private List<double> _pceArray;
        
        public LaserCurveData()
        {
            this._seArray = new List<double>();
            this._rsArray = new List<double>();
            this._pceArray = new List<double>();
        }

        #region >>> Public Property <<<

        public double[] SeData
        {
            get { return this._seArray.ToArray(); }
        }

        public double[] RsData
        {
            get { return this._rsArray.ToArray(); }
        }

        public double[] PceData
        {
            get { return this._pceArray.ToArray(); }
        }

        #endregion

        #region >>> Public Method <<<

        public void AddSeData(double[] array)
        {
            this._seArray.Clear();

            if (array != null)
            {
                foreach (var data in array)
                {
                    this._seArray.Add(data);
                }
            }
        }

        public void AddRsData(double[] array)
        {
            this._rsArray.Clear();

            if (array != null)
            {
                foreach (var data in array)
                {
                    this._rsArray.Add(data);
                }
            }
        }

        public void AddPceData(double[] array)
        {
            this._pceArray.Clear();

            if (array != null)
            {
                foreach (var data in array)
                {
                    this._pceArray.Add(data);
                }
            }
        }

        #endregion
    }
}

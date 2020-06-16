using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MPI.Tester.DeviceCommon
{
    [Serializable]
    public struct MinMaxValuePair<T>
    where T :System.IComparable<T>
    {
        public T Min;
        public T Max;
        
        public MinMaxValuePair(T min, T max)
        {
            if (min.CompareTo(max) > 0) //min > max
            {
                T tmp = min;
                min = max;
                max = min;
            }
            Min = min;
            Max = max;
        }

        public bool InRange(T val)
        {
            if((Min.CompareTo(val) > 0)  || 
                (Max.CompareTo(val) < 0))
            {
                return false;
            }
            return true;
        }

        public T SetInRange(T val)
        {
            if ((Min.CompareTo(val) > 0) )
            {
                return Min;
            }

            if ((Max.CompareTo(val) < 0))
            {
                return Max;
            }

            return val;
        }

    }
}

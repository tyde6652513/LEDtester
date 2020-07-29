using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MPI.Tester.Data.ChannelCoordTable
{
   /// <summary>
   /// Level 定義: 0為一般col row,往上groupX,Y為1,2,3...,往下SubX,Y為-1-2...
   /// </summary>
   /// <typeparam name="T"></typeparam>
    public class LevelShiftTable<T> : Dictionary<int, List<T>>, ICloneable//避免以後還有Z方向或是其他在同一層內必須擴充的變數
    {
        public LevelShiftTable()
            : base()
        {
            List<T> lt = new List<T>();
            dynamic val = 0;
            lt.Add(val);
            lt.Add(val);
            base.Add(0, lt);
        }

        #region >>public method<<
        public void Push(int level, T x, T y)
        {
            List<T> lt = new List<T>();
            lt.Add(x);
            lt.Add(y);
            if (!base.ContainsKey(level))
            {
                base.Add(level, lt);
            }
            else
            {
                base[level] = lt;
            }
        }

        public void Push(int level, List<T> lt)
        {
            List<T> lt1 = new List<T>();
            lt1.AddRange(lt.ToArray());
            if (!base.ContainsKey(level))
            {
                base.Add(level, lt1);
            }
            else
            {
                base[level] = lt1;
            }
        }

        public T GetX(int level)
        {
            return GetShift_In_List(level, 0);
        }
        public T GetY(int level)
        {
            return GetShift_In_List(level, 1);
        }
        public T GetZ(int level)
        {
            return GetShift_In_List(level, 2);
        }

        //public LevelShiftTable<T> ALL_Shift(LevelShiftTable<T> shiftTable)
        //{
        //    LevelShiftTable<T> tTable = new LevelShiftTable<T>();

        //    foreach (var lPair in this)
        //    {
        //        int level  =lPair.Key;
        //        int maxCnt = Math.Max(lPair.Value.Count, shiftTable[level].Count);
        //    }
        //    T a;
        //    T b;
        //    var c = a +b;
        //}

        #endregion

        #region >>public property<<
        public List<T> this[int level]
        {
            get
            {
                if (!base.ContainsKey(level))
                {
                    dynamic val = 0;
                    Push(level, val, val);
                }
                return base[level];
            }
            set
            {
                Push(level, value);
            }
        }

        public int BottomLayer
        {
            get
            {
                int layer = 0;
                if (this.Keys != null)
                {
                    return this.Keys.ToList().Min();
                }
                return layer;
        } }

        public object Clone()
        {
            LevelShiftTable<T> obj = new LevelShiftTable<T>();
            foreach (var p in this)
            {
                int lelev = p.Key;
                List<T> tList = new List<T>();
                tList.AddRange(p.Value.ToArray());
                obj.Push(lelev, tList);
            }
            return obj;
        }
        #endregion

        #region >>private property<<
        private T GetShift_In_List(int level, int id)
        {
            dynamic val = 0;
            if (base.ContainsKey(level) && base[level].Count > id)
            {
                val = base[level][id];
            }
            return val;
        }
        #endregion

        #region >>operator<<
        public static LevelShiftTable<T> operator +(LevelShiftTable<T> a, LevelShiftTable<T> b)
        {
            LevelShiftTable<T> tTable = new LevelShiftTable<T>();

            try
            {
                foreach (var lPair in a)
                {
                    int level = lPair.Key;
                    int maxCnt = Math.Max(lPair.Value.Count, b[level].Count);
                    for (int i = lPair.Value.Count; i < maxCnt; ++i)
                    {
                        dynamic val = 0;
                        lPair.Value.Add(val);
                    }
                    for (int i = b[level].Count; i < maxCnt; ++i)
                    {
                        dynamic val = 0;
                        b[level].Add(val);
                    }
                    List<T> lt = new List<T>();
                    for (int i = 0; i < maxCnt; ++i)
                    {
                        dynamic aa = lPair.Value[i];
                        dynamic bb = b[level][i];
                        dynamic val = aa + bb;
                        lt.Add(val);
                    }
                    tTable.Push(level, lt);

                }
            }
            catch (Exception e)
            {
                Console.WriteLine("[LevelShiftTable],operator + Exception " + e.Message);
                return new LevelShiftTable<T>();
            }
            return tTable;
        }
   
        public static LevelShiftTable<T> operator -(LevelShiftTable<T> a, LevelShiftTable<T> b)
        {
            LevelShiftTable<T> tTable = new LevelShiftTable<T>();

            try
            {
                foreach (var lPair in a)
                {
                    int level = lPair.Key;
                    int maxCnt = Math.Max(lPair.Value.Count, b[level].Count);
                    for (int i = lPair.Value.Count; i < maxCnt; ++i)
                    {
                        dynamic val = 0;
                        lPair.Value.Add(val);
                    }
                    for (int i = b[level].Count; i < maxCnt; ++i)
                    {
                        dynamic val = 0;
                        b[level].Add(val);
                    }
                    List<T> lt = new List<T>();
                    for (int i = 0; i < maxCnt; ++i)
                    {
                        dynamic aa = lPair.Value[i];
                        dynamic bb = b[level][i];
                        dynamic val = aa - bb;
                        lt.Add(val);
                    }
                    tTable.Push(level, lt);

                }
            }
            catch (Exception e)
            {
                Console.WriteLine("[LevelShiftTable],operator - Exception " + e.Message);
                return new LevelShiftTable<T>();
            }
            return tTable;
        }

        #endregion
    }
}

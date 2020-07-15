using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//using MPI.Tester.Tools;

namespace MPI.Tester.Data.ChannelCoordTable
{
    public class ChannelPosShiftTable<T> : Dictionary<int, LevelShiftTable<T>> where T : IConvertible 
    {
        #region >>constructor<<
        public ChannelPosShiftTable()
            : base()
        {
            LevelShiftTable<T> lst = new LevelShiftTable<T>();
            base.Add(0, lst);
            //base.Add(1, lst);//先不要亂塞
        }


        #endregion

        #region >>private property<<
        
        #endregion

        #region >>public property<<
        #endregion

        #region >>private method<<
        #endregion

        #region >>public method<<

        
        public void Push(int ch,int level, T x, T y)
        {
            List<T> coList = new List<T>();
            coList.Add(x);
            coList.Add(y);
            Push(ch, level, coList);
        }
        
        public void Push(int ch,int level, List<T> coList)
        {
            LevelShiftTable<T> lst = new LevelShiftTable<T>();
            lst.Push(level, coList);
            Push(ch, lst);
        }

        public void Push(int ch, LevelShiftTable<T> lt)
        {
            if (!base.ContainsKey(ch))
            {
                base.Add(ch, lt);
            }
            else
            {
                base[ch] = lt;
            }
        }
        #endregion

        
    }
}

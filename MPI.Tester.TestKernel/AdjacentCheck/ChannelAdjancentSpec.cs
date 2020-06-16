using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MPI.Tester.TestKernel
{
    public enum EPBMoveDirection
    {
        AxisX = 1,
        AxisY = 2,
    }

    class Point2D
    {
        private int _x;

        private int _y;

        public Point2D(int x, int y)
        {
            this._x = x;
            this._y = y;
        }

        public int X
        {
            get { return this._x; }
        }

        public int Y
        {
            get { return this._y; }
        }
    }

    public class ChannelAdjacentCheckSpec
    {
        private int _type;
        private double _criterion;
        private string _keyName;

        public ChannelAdjacentCheckSpec(string keyname, int type, double criterion)
        {
            this._type = type;

            this._keyName = keyname;

            this._criterion = criterion;
        }

        public int Type
        {
            get { return this._type; }
        }

        public double Criterion
        {
            get { return this._criterion; }
        }

        public string KeyName
        {
            get { return this._keyName; }
        }
    }
}

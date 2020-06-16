using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MPI.Tester.Data;
using MPI.Tester;

namespace MPI.Tester.Tools
{
    public class TitleData
    {
        private object _lockObj;

        private string _keyName;
        private string _name;
        private int _index;

        private string _format;
        private EGainOffsetType _gainOffsetType;

        private bool _isEnable;

        public TitleData()
        {
            this._lockObj = new object();

            this._keyName = string.Empty;
            this._name = string.Empty;

            this._isEnable = false;
        }

        public TitleData(string keyName)
            : this()
        {
            this._keyName = keyName;
        }

        public TitleData(string keyName, string name)
            : this(keyName)
        {
            this._name = name;
        }

        #region >>> Public Property <<<

        public string KeyName
        {
            get { return this._keyName; }
        }

        public string Name
        {
            get { return this._name; }
            set { this._name = value; }
        }

        public int Digit
        {
            get
            {
                int pointPos = this._format.IndexOf('.');

                if (pointPos < 0)
                {
                    return 0;
                }
                else
                {
                    return (this._format.Length - pointPos - 1);
                }
            }
        }

        public int Index
        {
            get { return this._index; }
            set { this._index = value; }
        }

        public string Format
        {
            get { return this._format; }
            set { this._format = value; }
        }

        public EGainOffsetType GainOffsetType
        {
            get { return this._gainOffsetType; }
            set { this._gainOffsetType = value; }
        }

        public bool IsEnable
        {
            get { return this._isEnable; }
            set { this._isEnable = value; }
        }

        #endregion
    }
}

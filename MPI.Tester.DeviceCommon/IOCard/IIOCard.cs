using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MPI.Tester.DeviceCommon
{
    public interface IIOCard
    {
        string SerialNumber { get; }
        string SoftwareVersion { get; }
        string HardwareVersion { get; }
        EDevErrorNumber ErrorNumber { get; }

        bool Init();
        void Close();
        void WriteDO(int[] pins, bool state);

        DI DI { get; }
        DO DO { get; }

    }

    public class IOCommand
    { 
        public delegate void IOInterrupt();
        public IOInterrupt StartOfTest;
        public IOInterrupt Calculate;
    }

    public class DI
    {
        public delegate bool DIEventOccur(int pin);

        #region >>> Private Method <<<

        private DIEventOccur _diEventOccur;

        #endregion

        public DI(int pinQuantity, DIEventOccur diCallBack)
        {
            this._diEventOccur = diCallBack;
        }

        # region >>> Public Proterties <<<

        public bool this[int pin]
        {
            get { return this._diEventOccur(pin); }
        }

        #endregion

        #region >>> Public Methods <<<

        #endregion
    }

    public class DO
    {
        public delegate void DOEventOccur(int pin, bool state);

        #region >>> Private Method <<<

        private DOEventOccur _doEventOccur;

        #endregion

        public DO(int pinQuantity, DOEventOccur doCallBack)
        {
            this._doEventOccur = doCallBack;
        }

        # region >>> Public Proterties <<<

        public bool this[int pin]
        {
            set { this._doEventOccur(pin, value); }
        }

        #endregion

        #region >>> Public Methods <<<

        #endregion
    }
}

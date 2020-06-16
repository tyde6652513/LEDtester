using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

using NationalInstruments.NI4882;
using MPI.Tester.DeviceCommon;

namespace MPI.Tester.Device
{
    // Ricky 0814
    class GPIBConnect3 
    {
        private NationalInstruments.NI4882.Device _device = null;

        private const int BOARD_DINDEX = 0;					// board Index
        private const int PRIMARY_ADDR = 24;                // pimary address of device
        private const int SECONDARY_ADDR = 0;               // secondary address of device

        private const int EOTMODE = 1;                      // enable the END message
        private const int EOSMODE = 0;                      // disable the EOS mode		

        private static string[] ErrorMnemonic = {"EDVR", "ECIC", "ENOL", "EADR", "EARG",          // Error codes
                                                    "ESAC", "EABO", "ENEB", "EDMA", "",
                                                    "EOIP", "ECAP", "EFSO", "", "EBUS",
                                                    "ESTB", "ESRQ", "", "", "", "ETAB"};

        //private static int TIMEOUT;                         // tmeout value

        private bool _isQueried;
        private string _readBuffer;
        private bool _isCompleteWrite;
        private GPIBSettingData _setting;


        //private AsyncCallback _writeCallBack;
        private AsyncCallback _readCallBack;
        MPI.PerformanceTimer pt = new PerformanceTimer();

        /// <summary>
        /// constructor
        /// </summary>
        /// 

        public GPIBConnect3(GPIBSettingData setting)
        {
            this._isQueried = false;
            this._setting = setting;
        }

        #region >>> Private Methods <<<


        private void OnWriteComplete(IAsyncResult result)
        {
            try
            {
                _device.EndWrite(result);
                if (result.IsCompleted == true)
                    this._isCompleteWrite = true;

            }
            catch (Exception ex)
            {
					MessageBox.Show(ex.Message);
            }
            //elementsTransferredTextBox.Text = device.LastCount.ToString();
            //lastIOStatusTextBox.Text = device.LastStatus.ToString();
        }

        private void OnReadComplete(IAsyncResult result)
        {
            double time;
            try
            {
                this._readBuffer = _device.EndReadString(result).TrimEnd('\n');
                //_device.SynchronizeCallbacks = false;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            //elementsTransferredTextBox.Text = _device.LastCount.ToString();
            //lastIOStatusTextBox.Text = _device.LastStatus.ToString();
            pt.Stop();
            time = pt.GetTimeSpan(ETimeSpanUnit.MilliSecond);
        }



        #endregion

        public bool Open(out string Information) 
        {
            int index = this._setting.DeviceNumber; 
            

            try
            {
                _device = new NationalInstruments.NI4882.Device(index, (byte)this._setting.PrimaryAddress, (byte)SECONDARY_ADDR);
                _device.IOTimeout = TimeoutValue.T1s;
                //buffersize = 50; // _device.DefaultBufferSize;

                //#if NETFX2_0
                //For .NET Framework 2.0, use SynchronizeCallbacks to specify that the object 
                //marshals callbacks across threads appropriately.
                _device.SynchronizeCallbacks = true;
                //#else
                //For .NET Framework 1.1, set SynchronizingObject to the Windows Form to specify 
                //that the object marshals callbacks across threads appropriately.
                //                _device.SynchronizingObject = this;
                //#endif
                //                SetupControlState(true);


                //this._writeCallBack = new AsyncCallback(OnWriteComplete);
                this._readCallBack = new AsyncCallback(OnReadComplete);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                //                Cursor.Current = Cursors.Default;
            }


            if (this.QueryCommand("*IDN?") == true)
            {
                return WaitAndGetData(out Information);
                 
            }
            else
            {
                Information = "Err. on GPIB";
                return false;
            }

        }

        public void Close()
        {
            try
            {
                _device.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public bool SendCommand(string command)
        {
            try
            {
                this._isCompleteWrite = false;
                //_device.BeginWrite(command, new AsyncCallback(OnWriteComplete), null);
                _device.Write(command);

                //do
                //{
                //    System.Threading.Thread.Sleep(1);
                //    System.Windows.Forms.Application.DoEvents();
                //}
                //while (this._isCompleteWrite == false);


                //_device.Write(command);


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }

            return true;
        }



        public bool QueryCommand(string command)
        {

            bool rtn = false;

            if (false == this._isQueried)
            {
                this._readBuffer = "";
                rtn = this.SendCommand(command);

                //_device.SynchronizeCallbacks = true;


                this._isQueried = true;
            }
            else
            {
                return true;
            }

            return rtn;
        }





        public  bool WaitAndGetData(out string result)
        {

            if (this._isQueried == false)
            {
                result = "";
                return false;
            }


            pt.Start();
            try
            {
                _device.BeginRead(this._readCallBack, null);
                //this._readBuffer = _device.ReadString().TrimEnd('\n');
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


            do
            {
                System.Threading.Thread.Sleep(1);
                System.Windows.Forms.Application.DoEvents();
            }
            while (_readBuffer == "");

            this._isQueried = false;


            result = this._readBuffer;
            return true;



        }
    }



}

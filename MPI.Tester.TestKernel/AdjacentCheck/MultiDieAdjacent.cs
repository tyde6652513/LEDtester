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
    class MultiDieAdjacentCheck : AdjacentCheck
    {
        private Dictionary<string, ChannelAdjacentCheckSpec> _channelAdjancentSpec = new Dictionary<string, ChannelAdjacentCheckSpec>();

        private double[][] _channelDataBuffer = null;

        private int _channelCount;

        private int _errorCount;

        private bool _isEnable;

        private int _startingCount;

        private int _stopCount;

        private int _testedIndex;

        private uint _totalProbingCounts;

        private uint _accumulateErrCount;

        double[] channels = null;

    //    private int _consecutiveErrorCountSpec = 100;

        public MultiDieAdjacentCheck()
        {
            this._consecutiveErrorCount = 100;

            this._isEnable = false;

            _startingCount = 500;

            _stopCount = 0;

            _errorCount = 0;

            _accumulateErrCount = 0;

        }

        #region >>> Public Method <<<

        public void Reset(bool isEnable, ProductData pd ,TesterSetting setting)
        {
            this._isEnable = isEnable;

            if (pd.TestCondition.TestItemArray == null)
            {
                this._isEnable = false;

                return;
            }

            this._errorCount = 0;

            this._testedIndex = 0;

            this._accumulateErrCount = 0;

            this._consecutiveErrorCount = (int)pd.AdjacentConsecutiveErrorCount;

            this._channelCount =pd.TestCondition.ChannelConditionTable.Count;

            this._startingCount = pd.AdjacentStartingCount;

            this._stopCount = pd.AdjacentStopCount;

            this._totalProbingCounts = setting.TotalProbingCount;

            TestItemData[] data = pd.TestCondition.TestItemArray;

            Dictionary<string, float> result = new Dictionary<string, float>();

            this._channelAdjancentSpec.Clear();

            foreach (TestItemData item in data)
            {
                if (item.MsrtResult != null)
                {
                    for (int i = 0; i < item.MsrtResult.Length; i++)
                    {
                        if (item.MsrtResult[i].EnableAdjacent)
                        {
                            this._channelAdjancentSpec.Add(item.MsrtResult[i].KeyName, new ChannelAdjacentCheckSpec(item.MsrtResult[i].KeyName, item.MsrtResult[i].AdjacentType, item.MsrtResult[i].AdjacentRange));
                        }
                    }
                }
            }

            this._channelDataBuffer = new double[this._channelAdjancentSpec.Count][];

            for (int i = 0; i < this._channelDataBuffer.Length; i++)
            {
                this._channelDataBuffer[i] = new double[_channelCount];
            }

            channels = new double[_channelCount];
        }

        public void Clear()
        {
            this._channelAdjancentSpec.Clear();

            this._errorCount = 0;

            this._testedIndex = 0;

            this._channelDataBuffer = null;
        }

        public EAdjacentResult Push(ChannelConditionTable channelCondtionTable, ChannelResultDataSet channelResultSet)
        {
            if (!this._isEnable)
            {
                return EAdjacentResult.NONE;
            }

            bool[] ispass = new bool[_channelCount];

            bool[] isHaveDie = new bool[_channelCount];

            int testCount=-1;

            Dictionary<string, float> result = new Dictionary<string, float>();

            int index = 0;

            foreach (var item in this._channelAdjancentSpec)
            {
                this._channelDataBuffer[index] = channelCondtionTable.GetItemEachChannelData(item.Key);

                index++;
            }

            for (uint ch = 0; ch < channelResultSet.Count; ch++)
            {
                //ispass[ch] = channelResultSet[ch].IsPass & channelResultSet[ch].IsTested;
                ispass[ch] = channelResultSet[ch].IsTested; // 判斷是否有測試

                if (channelResultSet[ch].IsTested)
                {
                    _testedIndex++;


                    testCount = (int)channelResultSet[ch]["TEST"].Value;
                }
                else
                {
                    // 當點測Block沒有滿靶時，測試機直接回傳Pass不卡控。

                    return EAdjacentResult.PASS;
                }
            }

            if (this.CalcChannelDifferentResult(ispass))
            {
                _errorCount = 0;
            }
            else
            {
                if (_testedIndex >= this._startingCount)
                {
                    _errorCount++;
                }
                else
                {
                    _errorCount = 0;
                }
                //36000-500
                if (_testedIndex >= this._totalProbingCounts - _stopCount)
                {
                    _errorCount = 0;
                }
            }

            if (this._errorCount >= this._consecutiveErrorCount)
            {
                this._errorCount = 0;

                _accumulateErrCount++;

                return EAdjacentResult.ERROR;
            }
            else
            {
                return EAdjacentResult.PASS;
            }
        }

        #endregion

        #region >>> Private Method <<<

        private bool CalcChannelDifferentResult(bool[] ispass)
        {
            bool rtn = true;

            int index = 0;

            foreach (var item in this._channelAdjancentSpec)
            {
                double[] data = this._channelDataBuffer[index];

                double baseValue = data[0];

                if (item.Value.Type == 0)
                {
                    for (int i = 0; i < data.Length; i++)
                    {
                        double dealt=data[i] - baseValue;

                        if (Math.Abs(dealt) > item.Value.Criterion)
                        {
                            if (ispass[i] == true)
                            {
                                rtn = false;
                            }
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < data.Length; i++)
                    {
                        double ratio = data[i] / baseValue;

                        if (Math.Abs(ratio - 1) > (item.Value.Criterion * 0.01))
                        {
                            if (ispass[i] == true)
                            {
                                rtn = false;
                            }
                        }
                    }
                }
            }

            return rtn;
        }

        #endregion

    }
}

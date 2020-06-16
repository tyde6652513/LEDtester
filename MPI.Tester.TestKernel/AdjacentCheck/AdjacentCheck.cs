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
    public class AdjacentCheck
    {
        private const int COORD_REPEAT_NUM = 2;

        private Dictionary<int, Dictionary<string, float>> _recordData = new Dictionary<int, Dictionary<string, float>>(); // col to outputData

        private Dictionary<string, int> _theFirstChipOfConsecutiveError = new Dictionary<string, int>();

        private List<Point2D> _outSpecChip = new List<Point2D>();

        private EPBMoveDirection _moveDirection = 0;

        private TestItemData[] _data;

        protected int _consecutiveErrorCount = 0;

        private int _reTestCoordX;

        private int _reTestCoordY;

        private bool _isFindMoveDirection;

        private int recordXCoord;

        private int recordYCoord;

        private int _sameXCount;

        private int _sameYCount;

        private int _proberCoord;

        private int _testerCoord;

        private int _consecutivPassCount = 0;

        private bool _isSecondCheck = false;

        private bool _isSaveLog = false;

        private int _index = 0;

        private bool _isAutoCleanNeedle = false;

        public AdjacentCheck()
        {
            _isFindMoveDirection = false;
            _reTestCoordX = 0;
            _reTestCoordY = 0;
            recordXCoord = 0;
            recordYCoord = 0;
            _sameXCount = 0;
            _sameYCount = 0;
            _proberCoord = 3;
            _testerCoord = 4;
            _consecutivPassCount = 0;
            _isSecondCheck = false;
        }

        #region >>> Public Property <<<

        public int ReTestCoordX
        {
            get { return this._reTestCoordX; }
        }

        public int ReTestCoordY
        {
            get { return this._reTestCoordY; }
        }

        #endregion

        #region >>> Public Method <<<

        public void Reset(int pboberCoord, int testerCoord, int numbers, bool isSaveLog)
        {
            this._proberCoord = pboberCoord;
            this._testerCoord = testerCoord;
            this._consecutiveErrorCount = numbers;
            this._isFindMoveDirection = false;
            _sameXCount = 0;
            _sameYCount = 0;
            _isSecondCheck = false;
            _isSaveLog = isSaveLog;
        }

        public void Clear()
        {
            this._recordData.Clear();

            this._theFirstChipOfConsecutiveError.Clear();

            _outSpecChip.Clear();
        }

        public EAdjacentResult Push(int index, int colX, int rowY, TestItemData[] data)
        {
            this._index = index;
            this._data = data;
            this.CheckMoveAxis(colX, rowY);
            return Run(colX, rowY);
        }

        public void ChangeMapRowCol()
        {
            switch (this._proberCoord)
            {
                case (int)ECoordSet.Second:
                    this._reTestCoordX *= (-1);
                    break;
                case (int)ECoordSet.Third:
                    this._reTestCoordX *= (-1);
                    this._reTestCoordY *= (-1);
                    break;
                case (int)ECoordSet.Fourth:
                    this._reTestCoordY *= (-1);
                    break;
                default:
                    break;
            }

            switch (this._testerCoord)
            {
                case (int)ECoordSet.Second:
                    this._reTestCoordX *= (-1);
                    break;
                case (int)ECoordSet.Third:
                    this._reTestCoordX *= (-1);
                    this._reTestCoordY *= (-1);
                    break;
                case (int)ECoordSet.Fourth:
                    this._reTestCoordY *= (-1);
                    break;
                default:
                    break;
            }
        }

        #endregion

        #region >>> Private Method <<<

        private void CheckMoveAxis(int colX, int rowY)
        {
            if (!this._isFindMoveDirection)
            {
                if (recordXCoord == colX)
                {
                    this._sameXCount++;
                }
                else
                {
                    this._sameXCount = 0;
                }

                if (recordYCoord == rowY)
                {
                    this._sameYCount++;
                }
                else
                {
                    this._sameYCount = 0;
                }

                //Console.WriteLine("(" + colX.ToString() + "," + rowY.ToString() + ")  = " + _sameYCount.ToString());

                if (this._sameXCount >= COORD_REPEAT_NUM)
                {
                    _moveDirection = EPBMoveDirection.AxisY;
                    Console.WriteLine("Prober Move Dircetion :  Y aixs ");
                    this._isFindMoveDirection = true;
                    this._recordData.Clear();
                    _outSpecChip.Clear();
                    return;
                }

                if (_sameYCount >= COORD_REPEAT_NUM)
                {
                    _moveDirection = EPBMoveDirection.AxisX;
                    Console.WriteLine("Prober Move Dircetion :  X aixs ");

                    this._isFindMoveDirection = true;
                    this._recordData.Clear();
                    _outSpecChip.Clear();
                    return;
                }
                recordXCoord = colX;
                recordYCoord = rowY;
            }
        }

        private EAdjacentResult Run(int colX, int colY)
        {
            EAdjacentResult result = EAdjacentResult.NONE;

            if (this._isFindMoveDirection == false)
                return EAdjacentResult.NONE;

            int mainMoveAxis = 0;
            int y = 0;

            if (this._moveDirection == EPBMoveDirection.AxisX)
            {
                mainMoveAxis = colX;
                y = colY;
            }
            else if (this._moveDirection == EPBMoveDirection.AxisY)
            {
                y = colX;
                mainMoveAxis = colY;
            }

            if (CheckResult(mainMoveAxis, y))  // PASS
            {
                // 當pass的情況下,且前面有outSpec的晶粒,此時要清除out spec晶粒,再把紀錄的資料清除,
                // 代表相信此連續的開始資料為真.

                if (this._outSpecChip.Count != 0)
                {
                    _consecutivPassCount++;

                    if (_consecutivPassCount > 3)
                    // if encount error  , and the next  chip is pass , can't  clear database,
                    // When consecutiv chips pass count =2 , that means the test is normal, 
                    // clear database in this time 
                    // 當晶粒連續兩顆pass時,清除紀錄fail count的dataBase
                    // 紀錄by col的 dictionary 
                    {
                        this._recordData.Clear();
                        this._outSpecChip.Clear();
                        this._isSecondCheck = false;
                        if (this._isSaveLog)
                        {
                            Console.WriteLine("(" + colX.ToString() + "," + colY.ToString() + ")  Clean out spec data ");
                        }
                    }
                }
            }
            else
            {
                this._outSpecChip.Add(new Point2D(mainMoveAxis, y));
                this._consecutivPassCount = 0;
            }

            // 
            int currentConsecutiveErrorCount = 0;

            if (_isSecondCheck)
            {
                currentConsecutiveErrorCount = this._consecutiveErrorCount + 1;
            }
            else
            {
                currentConsecutiveErrorCount = this._consecutiveErrorCount;
            }

            // -----------------------------------------------------------------
            // 當上下排誤差過大的晶粒大於設定數量時,進行報警或是自動清針的動作
            // -----------------------------------------------------------------
            if (this._outSpecChip.Count >= this._consecutiveErrorCount)
            {
                // Prober already move to the next chip

                if (this._moveDirection == EPBMoveDirection.AxisX)
                {
                    if (_isSecondCheck)
                    {
                        this._reTestCoordX = _outSpecChip[1].X;
                        this._reTestCoordY = _outSpecChip[1].Y;
                    }
                    else
                    {
                        this._reTestCoordX = _outSpecChip[0].X;
                        this._reTestCoordY = _outSpecChip[0].Y;
                    }
                }
                else if (_moveDirection == EPBMoveDirection.AxisY)
                {
                    this._reTestCoordX = _outSpecChip[0].Y;
                    this._reTestCoordY = _outSpecChip[0].X;
                }

                // Find we want to restest row/col 

                string chipKeyName = "X" + _reTestCoordX.ToString() + "Y" + _reTestCoordY.ToString();

                // -----------------------------------------------------------------
                // 當已經有alarm報警過的晶粒,再次報警時證明自動清針後還是異常,要停機
                // 沒有報警過的晶粒,進行重測的動作
                // -----------------------------------------------------------------

                if (this._theFirstChipOfConsecutiveError.ContainsKey(chipKeyName))
                {
                    int times = this._theFirstChipOfConsecutiveError[chipKeyName];
                    this._theFirstChipOfConsecutiveError.Remove(chipKeyName);
                    this._theFirstChipOfConsecutiveError.Add(chipKeyName, times + 1);
                    this._recordData.Clear();
                    result = EAdjacentResult.ERROR;  // FAIL....Error
                }
                else
                {
                    this._theFirstChipOfConsecutiveError.Add(chipKeyName, 0);
                    result = EAdjacentResult.RETEST;  // FAIL...ReTest
                }

                // -----------------------------------------------------------------
                // 第一階段,只要重新測試的狀態,直接停機
                // -----------------------------------------------------------------
                if (!_isAutoCleanNeedle)
                {
                    this._recordData.Clear();
                    result = EAdjacentResult.ERROR;  // FAIL...ReTest                
                }
                // -----------------------------------------------------------------
                // 清除Fail 晶粒的data base
                // -----------------------------------------------------------------

                Console.WriteLine("(" + colX.ToString() + "," + colY.ToString() + ")  alarm !! clean out-spec data ");
                this._outSpecChip.Clear();
                _isSecondCheck = true;
            }
            else
            {
                result = EAdjacentResult.PASS;
            }
            return result;
        }

        private bool CheckResult(int mainMoveAxis, int y)
        {
            bool isPass = true;
            Dictionary<string, float> result = new Dictionary<string, float>();

            foreach (TestItemData item in this._data)
            {
                if (item.MsrtResult != null)
                {
                    for (int i = 0; i < item.MsrtResult.Length; i++)
                    {
                        result.Add(item.MsrtResult[i].KeyName, (float)item.MsrtResult[i].Value);
                    }
                }
            }

            if (this._recordData.ContainsKey(mainMoveAxis))
            {
                Dictionary<string, float> recordResult = _recordData[mainMoveAxis];

                foreach (TestItemData item in this._data)
                {
                    if (!item.IsEnable)
                        continue;

                    if (item.MsrtResult == null)
                    {
                        continue;
                    }

                    foreach (TestResultData data in item.MsrtResult)
                    {
                        if (data.EnableAdjacent && recordResult.ContainsKey(data.KeyName))
                        {
                            double delta = result[data.KeyName] - recordResult[data.KeyName];
                            double scale = 0.0d;

                            if (result[data.KeyName] == 0.0f)
                            {
                                scale = 0.0d;
                            }
                            else if (recordResult[data.KeyName] == 0.0f)
                            {
                                scale = double.MaxValue;
                            }
                            else
                            {
                                scale = result[data.KeyName] / recordResult[data.KeyName];
                            }
                            // Type 1 : Gain 
                            if (data.AdjacentType == 1)
                            {
                                if (Math.Abs(scale - 1) > (data.AdjacentRange * 0.01) && data.Value > 0.01)
                                {
                                    if (this._isSaveLog)
                                    {
                                        //  Console.WriteLine("("+mainMoveAxis.ToString() + "," + y.ToString() + ")  error =  " + scale.ToString());
                                        if (result.ContainsKey("WATT_1"))
                                            Console.WriteLine("(" + mainMoveAxis.ToString() + "," + y.ToString() + ")  previous " + recordResult["WATT_1"].ToString() + " =>  " + result["WATT_1"].ToString() + "  , error =  " + scale.ToString("0.000"));
                                    }
                                    //   Console.WriteLine(" -- "+result[data.KeyName].ToString() + " - " + recordResult[data.KeyName].ToString());
                                    isPass &= false;
                                }
                            }
                            else
                            {
                                if (Math.Abs(delta) > (data.AdjacentRange))
                                {
                                    if (this._isSaveLog)
                                    {
                                        if (result.ContainsKey("WATT_1"))
                                            Console.WriteLine("(" + mainMoveAxis.ToString() + "," + y.ToString() + ")  error =  " + scale.ToString());
                                    }
                                    isPass &= false;
                                }

                            }
                        }
                    }
                }

                if (isPass == true || this._index < 1000)  // Pass Replace data
                {
                    this._recordData.Remove(mainMoveAxis);

                    this._recordData.Add(mainMoveAxis, result);

                    if (this._isSaveLog)
                    {
                        if (result.ContainsKey("WATT_1"))
                            Console.WriteLine("(" + mainMoveAxis.ToString() + "," + y.ToString() + ")  remove " + recordResult["WATT_1"].ToString() + " , =>  " + result["WATT_1"].ToString());
                    }
                }
            }
            else
            {
                this._recordData.Add(mainMoveAxis, result);
                if (this._isSaveLog)
                {
                    if (result.ContainsKey("WATT_1"))
                        Console.WriteLine("(" + mainMoveAxis.ToString() + "," + y.ToString() + ")  = " + result["WATT_1"].ToString());
                }
                //  Console.WriteLine("(" + mainMoveAxis.ToString() + "," + y.ToString() + ")  = " + result["WATT_1"].ToString());
                isPass &= true;
            }
            return isPass;
        }

        #endregion

    }
}


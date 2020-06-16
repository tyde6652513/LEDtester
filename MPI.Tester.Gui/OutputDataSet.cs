using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MPI.Tester.TestKernel;
using MPI.Tester.Data;
using System.IO;

namespace MPI.Tester.Gui
{
    public class OutputBigData
    {
        #region >>> Private Property <<<

        private List<string> _sweepDataIndex;
        private List<OutputSweepData> _sweepData;

        private List<string> _spectrumDataIndex;
        private List<OutputSpectrumData> _spectrumData;

        private List<string> _livDataIndex;
        private List<OutputLIVData> _livData;

        private List<string> _pivDataIndex;
        private List<OutputPIVData> _pivData;

        private bool _isCheckRowCol;
        private bool _isEnableSaveAllSweepData;
        private bool _isEnableSaveAbsoluteSpectrum;
        private bool _isEnableSaveRelativeSpectrum;
        private bool _isEnableSaveDarkSpectrum;
        private uint _saveSpectrumMaxCount;
        private bool _isEnableSaveLIVData;
        private bool _isEnableSavePIVData;

        #endregion

        #region >>> Constructor / Disposor <<<

        public OutputBigData()
        {
            this._sweepDataIndex = new List<string>();

            this._sweepData = new List<OutputSweepData>();

            this._spectrumDataIndex = new List<string>();

            this._spectrumData = new List<OutputSpectrumData>();

            this._livData = new List<OutputLIVData>();

            this._livDataIndex = new List<string>();

            this._pivData = new List<OutputPIVData>();

            this._pivDataIndex = new List<string>();

            this._isEnableSaveAllSweepData = false;

            this._isEnableSaveAbsoluteSpectrum = false;

            this._isEnableSaveRelativeSpectrum = false;

            this._isEnableSaveDarkSpectrum = false;

            this._saveSpectrumMaxCount = 0;

            this._isEnableSaveLIVData = false;

            this._isEnableSavePIVData = false;
        }

        #endregion

        #region >>> Public Property <<<

        public bool IsCheckRowCol
        {
            get { return this._isCheckRowCol; }
            set { this._isCheckRowCol = value; }
        }

        public bool IsEnableSaveAllSweepData
        {
            get { return this._isEnableSaveAllSweepData; }
            set { this._isEnableSaveAllSweepData = value; }
        }

        public bool IsEnableSaveRelativeSpectrum
        {
            get { return this._isEnableSaveRelativeSpectrum; }
            set { this._isEnableSaveRelativeSpectrum = value; }
        }

        public bool IsEnableSaveAbsoluteSpectrum
        {
            get { return this._isEnableSaveAbsoluteSpectrum; }
            set { this._isEnableSaveAbsoluteSpectrum = value; }
        }

        public bool IsEnableSaveDarkSpectrum
        {
            get { return this._isEnableSaveDarkSpectrum; }
            set { this._isEnableSaveDarkSpectrum = value; }
        }

        public uint SaveSpectrumMaxCount
        {
            get { return this._saveSpectrumMaxCount; }
            set { this._saveSpectrumMaxCount = value; }
        }

        public bool IsEnableSaveLIVData
        {
            get { return this._isEnableSaveLIVData; }
            set { this._isEnableSaveLIVData = value; }
        }

        public bool IsEnableSavePIVData
        {
            get { return this._isEnableSavePIVData; }
            set { this._isEnableSavePIVData = value; }
        }

        public List<OutputSweepData> OutputSweepList
        {
            get { return this._sweepData; }
            set { this._sweepData = value; }
        }

        public List<OutputSpectrumData> OutputSpectrumList
        {
            get { return this._spectrumData; }
            set { this._spectrumData = value; }
        }

        public List<OutputLIVData> OutputLIVList
        {
            get { return this._livData; }
            set { this._livData = value; }
        }

        public List<OutputPIVData> OutputPIVList
        {
            get { return this._pivData; }
            set { this._pivData = value; }
        }

        #endregion

        #region >>> Private Method <<<

        private void AddSweepData(int col, int row, uint channel, ElecSweepDataSet sweepDataSet)
        {
            if (!this._isEnableSaveAllSweepData || sweepDataSet.Count == 0)
            {
                return;
            }

            string rowColKey = "X" + col.ToString() + "Y" + row.ToString();

            int count = 0;

            OutputSweepData data = new OutputSweepData(col, row);

            foreach (var item in sweepDataSet)
            {
                if (!item.IsEnable)
                {
                    continue;
                }

                if (item.Channel != channel)
                    continue;

                data.DataSet.Add(new ElecSweepData());

                data.DataSet[count].IsEnable = item.IsEnable;

                data.DataSet[count].KeyName = item.KeyName;

                data.DataSet[count].Name = item.Name;

                data.DataSet[count].TimeChain = item.TimeChain.Clone() as double[];

                data.DataSet[count].ApplyData = item.ApplyData.Clone() as double[];

                data.DataSet[count].SweepData = item.SweepData.Clone() as double[];

                count++;
            }

            if (data != null)
            {
                if (this._isCheckRowCol && this._sweepDataIndex.Contains(rowColKey))
                {
                    int index = this._sweepDataIndex.IndexOf(rowColKey);

                    this._sweepData[index] = data;
                }
                else
                {
                    this._sweepData.Add(data);

                    this._sweepDataIndex.Add(rowColKey);
                }
            }
        }

        private void AddSpectrumData(int col, int row, uint channel, SpectrumDataSet spectrumDataSet)
        {
            bool isAddData = false;

            if (spectrumDataSet.Count == 0)
            {
                return;
            }

            if (this._spectrumData.Count >= this.SaveSpectrumMaxCount)
            {
                return;
            }

            string rowColKey = "X" + col.ToString() + "Y" + row.ToString();

            OutputSpectrumData data = new OutputSpectrumData(col, row);

            foreach (var item in spectrumDataSet)
            {
                if (!item.IsEnable)
                {
                    continue;
                }

                if (item.Channel != channel)
                    continue;

                if (this._isEnableSaveAbsoluteSpectrum || this._isEnableSaveRelativeSpectrum || this._isEnableSaveDarkSpectrum)
                {
                    data.Wavelength.Add(item.Name, item.Wavelength.Clone() as double[]);
                }

                if (this._isEnableSaveAbsoluteSpectrum)
                {
                    data.Absoluate.Add(item.Name, item.Absoluate.Clone() as double[]);
                }

                if (this._isEnableSaveRelativeSpectrum)
                {
                    data.Intensity.Add(item.Name, item.Intensity.Clone() as double[]);
                }

                if (this._isEnableSaveDarkSpectrum)
                {
                    data.Dark.Add(item.Name, item.Dark.Clone() as double[]);
                }

                isAddData = true;
            }

            if (isAddData)
            {
                if (this._isCheckRowCol && this._spectrumDataIndex.Contains(rowColKey))
                {
                    int index = this._spectrumDataIndex.IndexOf(rowColKey);

                    this._spectrumData[index] = data;
                }
                else
                {
                    this._spectrumData.Add(data);

                    this._spectrumDataIndex.Add(rowColKey);
                }
            }
        }

        private void AddLIVData(int col, int row, LIVDataSet livDataSet)
        {
            if (!this._isEnableSaveLIVData || livDataSet.Count == 0)
            {
                return;
            }

            string rowColKey = "X" + col.ToString() + "Y" + row.ToString();

            OutputLIVData data = new OutputLIVData(col, row, livDataSet.Clone() as LIVDataSet);

            if (this._isCheckRowCol && this._livDataIndex.Contains(rowColKey))
            {
                int index = this._livDataIndex.IndexOf(rowColKey);

                this._livData[index] = data;
            }
            else
            {
                this._livData.Add(data);

                this._livDataIndex.Add(rowColKey);
            }
        }

        private void AddPIVData(int col, int row, PIVDataSet pivDataSet)
        {
            if (!this._isEnableSavePIVData || pivDataSet.Count == 0)
            {
                return;
            }

            string rowColKey = "X" + col.ToString() + "Y" + row.ToString();

            int count = 0;

            OutputPIVData data = new OutputPIVData(col, row);

            foreach (var item in pivDataSet)
            {
                if (!item.IsEnable)
                {
                    continue;
                }

                data.DataSet.Add(new PIVData());

                data.DataSet[count].IsEnable = item.IsEnable;

                data.DataSet[count].KeyName = item.KeyName;

                data.DataSet[count].Name = item.Name;

                data.DataSet[count].PowerData = item.PowerData.Clone() as double[];

                data.DataSet[count].CurrentData = item.CurrentData.Clone() as double[];

                data.DataSet[count].VoltageData = item.VoltageData.Clone() as double[];

                count++;
            }

            if (data != null)
            {
                if (this._isCheckRowCol && this._pivDataIndex.Contains(rowColKey))
                {
                    int index = this._pivDataIndex.IndexOf(rowColKey);

                    this._pivData[index] = data;
                }
                else
                {
                    this._pivData.Add(data);

                    this._pivDataIndex.Add(rowColKey);
                }
            }
        }

        #endregion

        #region >>> Public Method <<<

        public void AddBigData(int col, int row, uint channel, AcquireData acquireData)
        {

            if (!Report.ReportProcess.IsImplement)
            {
                this.AddSweepData(col, row, channel, acquireData.ElecSweepDataSet);
                this.AddSpectrumData(col, row, channel, acquireData.SpectrumDataSet);
                this.AddLIVData(col, row, acquireData.LIVDataSet);
                this.AddPIVData(col, row, acquireData.PIVDataSet);
            }
            //if (!Report.ReportProcess.IsImplementSweepDataReport)
            //{
            //    this.AddSweepData(col, row, channel, acquireData.ElecSweepDataSet);
            //}

            //if (!Report.ReportProcess.IsImplementSpectrumReport)
            //{
            //    this.AddSpectrumData(col, row, channel, acquireData.SpectrumDataSet);
            //}

            //if (!Report.ReportProcess.IsImplementLIVReport)
            //{
            //    this.AddLIVData(col, row, acquireData.LIVDataSet);
            //}

            //if (!Report.ReportProcess.IsImplementPIVDataReport)
            //{
            //    this.AddPIVData(col, row, acquireData.PIVDataSet);
            //}
        }

        public bool SaveSpectrumData(string path, string testResultFileName)
        {
            if (Report.ReportProcess.IsImplementSpectrumReport)
            {
                return true;
            }
            
            if (!MPIFile.CreateDirectory(path))
            {
                return false;
            }

            ////////////////////////////////////////////////////////////
            // Save Absolute Spectrum
            ////////////////////////////////////////////////////////////
            if (this._isEnableSaveAbsoluteSpectrum)
            {
                Dictionary<string, List<double[]>> absData = new Dictionary<string, List<double[]>>();

                foreach (var count in this._spectrumData)
                {
                    if (absData.Count == 0)
                    {
                        foreach (var item in count.Wavelength)
                        {
                            if (!absData.ContainsKey(item.Key))
                            {
                                absData.Add(item.Key, new List<double[]>() { item.Value });
                            }
                        }
                    }

                    foreach (var item in count.Absoluate)
                    {
                        absData[item.Key].Add(item.Value);
                    }
                }


                foreach (var item in absData)
                {
                    string fileAndPath = Path.Combine(path, testResultFileName + "_" + item.Key + ".abs");

                    List<string[]> csvdata = new List<string[]>();

                    for (int i = 0; i < item.Value[0].Length; i++)
                    {
                        string[] rowdata = new string[item.Value.Count];

                        for (int j = 0; j < item.Value.Count; j++)
                        {
                            rowdata[j] = item.Value[j][i].ToString();
                        }

                        csvdata.Add(rowdata);
                    }

                    CSVUtil.WriteCSV(fileAndPath, csvdata);
                }
            }

            ////////////////////////////////////////////////////////////
            // Save Relative Spectrum
            ////////////////////////////////////////////////////////////
            if (this._isEnableSaveRelativeSpectrum)
            {
                Dictionary<string, List<double[]>> intData = new Dictionary<string, List<double[]>>();

                foreach (var count in this._spectrumData)
                {
                    if (intData.Count == 0)
                    {
                        foreach (var item in count.Wavelength)
                        {
                            if (!intData.ContainsKey(item.Key))
                            {
                                intData.Add(item.Key, new List<double[]>() { item.Value });
                            }
                        }
                    }

                    foreach (var item in count.Intensity)
                    {
                        intData[item.Key].Add(item.Value);
                    }
                }


                foreach (var item in intData)
                {
                    string fileAndPath = Path.Combine(path, testResultFileName + "_" + item.Key + ".rel");

                    List<string[]> csvdata = new List<string[]>();

                    for (int i = 0; i < item.Value[0].Length; i++)
                    {
                        string[] rowdata = new string[item.Value.Count];

                        for (int j = 0; j < item.Value.Count; j++)
                        {
                            rowdata[j] = item.Value[j][i].ToString();
                        }

                        csvdata.Add(rowdata);
                    }

                    CSVUtil.WriteCSV(fileAndPath, csvdata);
                }
            }

            ////////////////////////////////////////////////////////////
            // Save Dark Spectrum
            ////////////////////////////////////////////////////////////
            if (this._isEnableSaveDarkSpectrum)
            {
                Dictionary<string, List<double[]>> darkData = new Dictionary<string, List<double[]>>();

                foreach (var count in this._spectrumData)
                {
                    if (darkData.Count == 0)
                    {
                        foreach (var item in count.Wavelength)
                        {
                            if (!darkData.ContainsKey(item.Key))
                            {
                                darkData.Add(item.Key, new List<double[]>() { item.Value });
                            }
                        }
                    }

                    foreach (var item in count.Dark)
                    {
                        darkData[item.Key].Add(item.Value);
                    }
                }


                foreach (var item in darkData)
                {
                    string fileAndPath = Path.Combine(path, testResultFileName + "_" + item.Key + ".dk");

                    List<string[]> csvdata = new List<string[]>();

                    for (int i = 0; i < item.Value[0].Length; i++)
                    {
                        string[] rowdata = new string[item.Value.Count];

                        for (int j = 0; j < item.Value.Count; j++)
                        {
                            rowdata[j] = item.Value[j][i].ToString();
                        }

                        csvdata.Add(rowdata);
                    }

                    CSVUtil.WriteCSV(fileAndPath, csvdata);
                }
            }

            return true;
        }

        public bool SaveSweepData(string path, string testResultFileName, int startIndex, int length)
        {
            if (Report.ReportProcess.IsImplementSweepDataReport)
            {
                return true;
            }
            
            if (!this.IsEnableSaveAllSweepData || this._sweepData.Count == 0)
            {
                return true;
            }

            if (!MPIFile.CreateDirectory(path))
            {
                return false;
            }

            if (startIndex > this._sweepData.Count - 1)
            {
                return false;
            }

            int endIndex = startIndex + length;

            if (endIndex > this._sweepData.Count)
            {
                endIndex = this._sweepData.Count;
            }

            for (int i = startIndex; i < endIndex; i++)
            {
                foreach (var item in this._sweepData[i].DataSet)
                {
                    if (!item.IsEnable)
                    {
                        continue;
                    }

                    string fileName = string.Empty;

                    if (this._isCheckRowCol)
                    {
                        //TestResultFileName_C(-1)_R(1)_ItemName.csv
                        fileName = testResultFileName + "_C(" + this._sweepData[i].Col.ToString() + ")_R(" + this._sweepData[i].Row.ToString() + ")_" + item.Name + ".csv";
                    }
                    else
                    {
                        //TestResultFileName_0001_C(-1)_R(1)_ItemName.csv
                        fileName = testResultFileName + "_" + i.ToString("0000") + "_C(" + this._sweepData[i].Col.ToString() + ")_R(" + this._sweepData[i].Row.ToString() + ")_" + item.Name + ".csv";
                    }

                    string fileAndPath = Path.Combine(path, fileName);

                    string[][] title;

                    if (item.KeyName.Contains("VISCAN"))
                    {
                        title = new string[][] { new string[] { "VISCAN "},
                                                 new string[] { "Time(ms)", "Apply(V)", "Msrt(A)" } };
                    }
                    else if (item.KeyName.Contains("VISWEEP"))
                    {
                        title = new string[][] { new string[] { "VISweep "},
                                                 new string[] { "Time(ms)", "Apply(V)", "Msrt(A)" } };
                    }
                    else
                    {
                        title = new string[][] { new string[] { "IVSweep "},
                                                 new string[] { "Time(ms)", "Apply(A)", "Msrt(V)" } };
                    }

                    double[][] data = new double[3][];

                    data[0] = item.TimeChain;

                    data[1] = item.ApplyData;

                    data[2] = item.SweepData;

                    List<string[]> csvdata = new List<string[]>();

                    csvdata.AddRange(title);

                    for (int j = 0; j < data[0].Length; j++)
                    {
                        string[] rowdata = new string[data.Length];

                        for (int k = 0; k < data.Length; k++)
                        {
                            rowdata[k] = data[k][j].ToString();
                        }

                        csvdata.Add(rowdata);
                    }

                    if (!CSVUtil.WriteCSV(fileAndPath, csvdata))
                    {
                        Console.WriteLine("[OutputBigData], SaveSweepData(), Write Data fail.");
                    }
                }
            }

            return true;
        }

        public bool SaveLIVData(string path, string testResultFileName, int startIndex, int length)
        {
            if (Report.ReportProcess.IsImplementLIVReport)
            {
                return true;
            }
            
            if (!this._isEnableSaveLIVData || this._livData.Count == 0)
            {
                return true;
            }

            if (startIndex > this._livData.Count - 1)
            {
                return false;
            }

            int endIndex = startIndex + length;

            if (endIndex > this._livData.Count)
            {
                endIndex = this._livData.Count;
            }

            for (int i = startIndex; i < endIndex; i++)
            {
                foreach (var testItem in this._livData[i].DataSet)
                {
                    List<string[]> data = new List<string[]>();

                    if (!testItem.IsEnable)
                    {
                        continue;
                    }

                    foreach (var resultItem in testItem)
                    {
                        if (!resultItem.IsEnable)
                        {
                            continue;
                        }

                        string[] result = new string[resultItem.DataArray.Length + 1];

                        result[0] = resultItem.Name;

                        for (int j = 0; j < resultItem.DataArray.Length; j++)
                        {
                            result[j + 1] = resultItem.DataArray[j].ToString(resultItem.Formate);
                        }

                        data.Add(result);
                    }

                    if (data.Count == 0)
                    {
                        Console.WriteLine("[OutputBigData], SaveLIVData(), No Msrt Result Enable!");

                        return false;
                    }

                    string fileName = string.Empty;

                    if (this._isCheckRowCol)
                    {
                        //TestResultFileName_C(-1)_R(1)_ItemName.csv
                        fileName = testResultFileName + "_C(" + this._livData[i].Col.ToString() + ")_R(" + this._livData[i].Row.ToString() + ")_" + testItem.Name + ".csv";
                    }
                    else
                    {
                        //TestResultFileName_0001_C(-1)_R(1)_ItemName.csv
                        fileName = testResultFileName + "_" + i.ToString("0000") + "_C(" + this._livData[i].Col.ToString() + ")_R(" + this._livData[i].Row.ToString() + ")_" + testItem.Name + ".csv";
                    }

                    string fileAndPath = Path.Combine(path, fileName);

                    List<string[]> csvdata = new List<string[]>();

                    for (int j = 0; j < data[0].Length; j++)
                    {
                        string[] rowdata = new string[data.Count];

                        for (int k = 0; k < data.Count; k++)
                        {
                            rowdata[k] = data[k][j].ToString();
                        }

                        csvdata.Add(rowdata);
                    }

                    if (!CSVUtil.WriteCSV(fileAndPath, csvdata))
                    {
                        Console.WriteLine("[OutputBigData], SaveLIVData(), Write title fail.");
                    }
                }
            }

            return true;
        }

        public bool SavePIVData(string path, string testResultFileName, int startIndex, int length)
        {
            if (Report.ReportProcess.IsImplementPIVDataReport)
            {
                return true;
            }
            
            if (!this._isEnableSavePIVData || this._pivData.Count == 0)
            {
                return true;
            }

            if (!MPIFile.CreateDirectory(path))
            {
                return false;
            }

            if (startIndex > this._pivData.Count - 1)
            {
                return false;
            }

            int endIndex = startIndex + length;

            if (endIndex > this._pivData.Count)
            {
                endIndex = this._pivData.Count;
            }
           
            string fileName = string.Format("PIV_{0}_{1}.csv", testResultFileName, DateTime.Now.ToString("yyyyMMddHHmm"));
            string fileNameAndPath = Path.Combine(path, fileName);
            string key = string.Empty;

            List<string[]> csvdata = new List<string[]>();
            int count = 0;

            for (int i = startIndex; i < endIndex; i++)
            {
                foreach (var item in this._pivData[i].DataSet)
                {
                    if (!item.IsEnable)
                    {
                        continue;
                    }

                    key = string.Format("C{0}R{1}", this._pivData[i].Col.ToString(), this._pivData[i].Row.ToString());

                    string[] iArray = new string[item.CurrentData.Length + 2];
                    string[] vArray = new string[item.VoltageData.Length + 2];
                    string[] pArray = new string[item.PowerData.Length + 2];

                    iArray[0] = key;
                    vArray[0] = key;
                    pArray[0] = key;

                    iArray[1] = "Apply(A)";
                    vArray[1] = "Msrt(V)";
                    pArray[1] = "Pow(mW)";

                    //-------------------------------------------
                    count = 0;

                    foreach (var value in item.CurrentData)
                    {
                        iArray[count + 2] = value.ToString();
                        count++;
                    }

                    csvdata.Add(iArray);

                    //-------------------------------------------
                    count = 0;

                    foreach (var value in item.VoltageData)
                    {
                        vArray[count + 2] = value.ToString();
                        count++;
                    }

                    csvdata.Add(vArray);

                    //-------------------------------------------
                    count = 0;

                    foreach (var value in item.PowerData)
                    {
                        pArray[count + 2] = value.ToString();
                        count++;
                    }

                    csvdata.Add(pArray);

                }
            }

            if (!CSVUtil.WriteCSV(fileNameAndPath, csvdata))
            {
                Console.WriteLine("[OutputBigData], SavePIVData(), Write Data fail.");
            }

            return true;
        }

        public void Clear()
        {
            this._sweepData.Clear();

            this._sweepDataIndex.Clear();

            this._spectrumData.Clear();

            this._spectrumDataIndex.Clear();

            this._livData.Clear();

            this._livDataIndex.Clear();

            this._pivData.Clear();

            this._pivDataIndex.Clear();
        }

        #endregion
    }
    
    public class OutputSweepData
    {
        #region >>> Private Property <<<

        private int _col;
        private int _row;
        private ElecSweepDataSet _sweepData;

        #endregion

        #region >>> Constructor / Disposor <<<

        public OutputSweepData(int col, int row)
        {
            this._col = col;

            this._row = row;

            this._sweepData = new ElecSweepDataSet();
        }

        #endregion

        #region >>> Public Property <<<

        public int Col
        {
            get { return this._col; }
        }

        public int Row
        {
            get { return this._row; }
        }

        public ElecSweepDataSet DataSet
        {
            get { return this._sweepData; }
        }

        #endregion
    }

    public class OutputSpectrumData
    {
        #region >>> Private Property <<<

        private int _col;
        private int _row;
        private Dictionary<string, double[]> _wavelength;
        private Dictionary<string, double[]> _intensity;
        private Dictionary<string, double[]> _absoluate;
        private Dictionary<string, double[]> _dark;

        #endregion

        #region >>> Constructor / Disposor <<<

        public OutputSpectrumData(int col, int row)
        {
            this._col = col;

            this._row = row;

            this._wavelength = new Dictionary<string, double[]>();

            this._intensity = new Dictionary<string, double[]>();

            this._absoluate = new Dictionary<string, double[]>();

            this._dark = new Dictionary<string, double[]>();
        }

        #endregion

        #region >>> Public Property <<<

        public int Col
        {
            get { return this._col; }
            set { this._col = value; }
        }

        public int Row
        {
            get { return this._row; }
            set { this._row = value; }
        }

        public Dictionary<string, double[]> Wavelength
        {
            get { return this._wavelength; }
        }

        public Dictionary<string, double[]> Intensity
        {
            get { return this._intensity; }
        }

        public Dictionary<string, double[]> Absoluate
        {
            get { return this._absoluate; }
        }

        public Dictionary<string, double[]> Dark
        {
            get { return this._dark; }
        }

        #endregion
    }

    public class OutputLIVData
    {
        #region >>> Private Property <<<

        private int _col;
        private int _row;
        private LIVDataSet _livData;

        #endregion

        #region >>> Constructor / Disposor <<<

        public OutputLIVData(int col, int row, LIVDataSet data)
        {
            this._col = col;

            this._row = row;

            this._livData = data;
        }

        #endregion

        #region >>> Public Property <<<

        public int Col
        {
            get { return this._col; }
            set { this._col = value; }
        }

        public int Row
        {
            get { return this._row; }
            set { this._row = value; }
        }

        public LIVDataSet DataSet
        {
            get { return this._livData; }
        }

        #endregion
    }

    public class OutputPIVData
    {
        #region >>> Private Property <<<

        private int _col;
        private int _row;
        private PIVDataSet _dataSet;

        #endregion

        #region >>> Constructor / Disposor <<<

        public OutputPIVData(int col, int row)
        {
            this._col = col;

            this._row = row;

            this._dataSet = new PIVDataSet();
        }

        #endregion

        #region >>> Public Property <<<

        public int Col
        {
            get { return this._col; }
        }

        public int Row
        {
            get { return this._row; }
        }

        public PIVDataSet DataSet
        {
            get { return this._dataSet; }
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;

using MPI.Tester.Data;

namespace MPI.Tester.Report.User.DOWA
{
    partial class Report : ReportBase
    {
        private string _laserTempFileName = "";
        //private bool _LogFileCreated = false;
        List<string> _nameList = new List<string>();
        private StreamWriter _lsw = null;
        public override void PushRemarkInfo(string pushStr)
        {
            bool needPushLog = false;
            if (this.UISetting.LaserPowerLogPath != null &&
               this.UISetting.LaserPowerLogPath.EnablePath)
            {
                if (_lsw == null)
                {
                    OpenLogFile(pushStr);
                }

                try
                {
                    List<string> sList1 = new List<string>(pushStr.Split(','));
                    string outStr = "";
                    foreach (string str in sList1)
                    {
                        if (str != null && str.Length > 0)
                        {
                            if (str.Contains("Time:"))
                            {
                                DateTime dt = DateTime.Now;
                                if (DateTime.TryParse(str.Replace("Time:", ""), out dt))
                                {
                                    outStr += dt.ToString("yyyy/MM/dd HH:mm:ss");
                                }
                                else
                                {
                                    outStr += str.Replace("Time:", "");
                                }
                                outStr +=  ",";
                                
                            }
                            else
                            {
                                var sArr = str.Split(':');
                                outStr += sArr[1] + ",";
                            }
                        }
                    }

                    needPushLog = !WriteLog(outStr);
                }
                catch (Exception e)
                {
                    Console.WriteLine("[DOWA.Report]PushRemarkInfo(),Exception:" + e.Message);
                    return;
                }                
            }

            if(needPushLog)
            {
                RemarkList.Add(pushStr);
            }
        
            
        }


        #region
        private bool OpenLogFile(string str)
        {
            try
            {
                Console.WriteLine("[DOWA.Report],OpenLogFile()");

                if (!Directory.Exists(Constants.Paths.MPI_TEMP_DIR2))
                {
                    Directory.CreateDirectory(Constants.Paths.MPI_TEMP_DIR2);
                }
                string fileName = this.TestResultFileNameWithoutExt() + "_Laser";
                _laserTempFileName = Path.Combine(Constants.Paths.MPI_TEMP_DIR2, fileName + ReportBase.EXTEN_LASERLOG);

                _lsw = new StreamWriter(this._laserTempFileName, true, this._reportData.Encoding);
                IsLogFileCreated = true;
                IsLogFileCreated = CreateLogFileHeader(str);
            }
            catch (Exception e)
            {
                Console.WriteLine("[DOWA.Report]OpenLogFile(),Exception:" + e.Message);
                return false;
            }
            return true;
            
        }

        private bool WriteLog(string str)
        {
            try {
                _lsw.WriteLine(str);
                this._lsw.Flush();
            }
            catch (Exception e)
            {
                Console.WriteLine("[DOWA.Report]WriteLog(),Str:"+ str +",Exception:" + e.Message );
                return false;
            }
            return true;
        }

        private bool CloseLaserLog()
        {
            try
            {
                Console.WriteLine("[DOWA.Report]CloseLaserLog()");
                if (_lsw != null)
                {
                    _lsw.Close();
                    _lsw.Dispose();
                }

                if ((this.UISetting.LaserPowerLogPath != null &&
                    this.UISetting.LaserPowerLogPath.EnablePath) ||
                    _lsw == null)//沒開的話才丟到報表上
                {
                    string tarFolder = GetPathWithFolder(UISetting.LaserPowerLogPath);
                    string fileName = Path.GetFileNameWithoutExtension(_laserTempFileName);
                    string tarFileName = Path.Combine(tarFolder, fileName + ".csv");
                    MPIFile.CopyFile(_laserTempFileName, tarFileName);
                    MPIFile.DeleteFile(_laserTempFileName);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("[DOWA.Report]OpenLogFile(),Exception:" + e.Message);
                return false;
            }
            return true;
        }

        private bool CreateLogFileHeader(string refData)
        {
            Console.WriteLine("[DOWA.Report],CreateLogFileHeader()");


            _nameList = new List<string>();

            try
            {
                string header = "";
                List<string> sList1 = new List<string>(refData.Split(','));

                foreach (string str in sList1)
                {
                    if (str != null && str.Length > 0)
                    {

                        if (str.Contains("Time:"))
                        {
                            
                            _nameList.Add("Time");
                            header += "Time" + ",";
                        }
                        else
                        {
                            var sArr = str.Split(':');
                            _nameList.Add(sArr[0]);
                            header += sArr[0] + ",";
                        }
                        
                    }
                }
                WriteLog(header);
            }
            catch (Exception e)
            {
                Console.WriteLine("[DOWA.Report]CreateLogFileHeader(),Exception:" + e.Message);
                return false;
            }
            return true;
        }
        #endregion
    }
}

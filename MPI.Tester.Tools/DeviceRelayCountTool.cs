using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

using MPI.Tester;

namespace MPI.Tester.Tools
{
    /// <summary>
    /// 此工具應同時兼備下列功能
    /// 1.單次將log物件寫出，此功能作為log對象重新設定或關閉時，將對象狀態的物件序列化寫出
    /// 2.讀取資料夾中最後一份log，將其反序列化讀取最後的狀態
    /// 3.單筆寫出
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DeviceRelayCountTool<T> where T :class,ICloneable,new()
    {
       
        public DeviceRelayCountTool(string folderPath,string logItemName)
        {
            Console.WriteLine("[DeviceRelayCountTool],DeviceRelayCountTool() " + logItemName);
            LogItemName = logItemName;
            FolderPath = folderPath;
            LogObject = new T();

        }
        #region >>protected property<<
        protected bool _isSingleLogBusy = false;
        #endregion
        
        #region >>public property<<
        public T LogObject { get; set; }
        public string LogItemName { get;set;}
        public string FolderPath { get; set; }

        #endregion
 
        #region
        
        public T Deserialize(string fileName = "")
        {
            Console.WriteLine("[DeviceRelayCountTool],Deserialize()");
            if (fileName == "")
            {
                fileName = Path.Combine(FolderPath, LogItemName);
                fileName += GetExtension();
            }

            bool needBackup = false;
            try
            {
                LogObject = (T)DeSerializeBinary(fileName);
            }
            catch(Exception e)
            {
                Console.WriteLine("[DeviceLogTool],Deserialize(), LogItemName excption:" + e.Message);
               
            }

            return LogObject;
        
        }

        public void Serializ(string fileName = "")
        {
            try
            {
                Console.WriteLine("[DeviceRelayCountTool],Serializ() ");
                if (fileName == "")
                {
                    //fileName = Path.Combine(FolderPath, DateTime.Now.ToString("yyyy-MM-dd"),LogItemName, LogItemName+"_"+DateTime.Now.ToString("HH-mm-ss"));//目標輸出路徑
                    fileName = Path.Combine(FolderPath, LogItemName);
                    fileName += GetExtension();
                }
                string folder = Path.GetDirectoryName(fileName);
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }
                

                SerializeBinary(LogObject, fileName);
                Console.WriteLine("[DeviceRelayCountTool],Serializ() " + fileName);

                string noExtendName = Path.GetFileNameWithoutExtension(fileName);
                string xmlFile = Path.Combine(folder, noExtendName + ".xml");

                SerializToXml(xmlFile);//create xml file for user read
                Console.WriteLine("[DeviceRelayCountTool],Serializ() " + xmlFile);
            }
            catch (Exception e)
            {
                Console.WriteLine("[DeviceLogTool],Serializ(), excption:" + e.Message);

            }

        }

        public void SerializToXml(string fileName = "")//計數器被手動調整時記錄用
        {
            try
            {
                
                if (fileName == "")
                {
                    fileName = Path.Combine(FolderPath, LogItemName, LogItemName + "_" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm"));//目標輸出路徑
                    fileName += ".xml";
                }
                Console.WriteLine("[DeviceRelayCountTool],SerializToXml(),FileName:" + fileName);
                string folder = Path.GetDirectoryName(fileName);
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }

                MPI.Xml.XmlFileSerializer.Serialize(LogObject, fileName);

                FileInfo fInfo = new FileInfo(fileName);
            }
            catch (Exception e)
            {
                Console.WriteLine("[DeviceLogTool],SerializToXml(), excption:" + e.Message);

            }

        }

        public bool BackupXMLLog(string srcFileName = "",string tarFileName="")
        {
            try
            {
                if (srcFileName == "")
                {
                    srcFileName = Path.Combine(FolderPath, LogItemName + ".xml");
                }
                Console.WriteLine("[DeviceLogTool],Deserialize(),BackupXMLLog() srcPath:" + srcFileName);
                if (File.Exists(srcFileName))
                {
                    if (tarFileName == "")
                    {
                        tarFileName = Path.Combine(FolderPath, LogItemName + "_" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm") + "_Backup.xml");
                    }
                    Console.WriteLine("[DeviceLogTool],Deserialize(),BackupXMLLog() srcPath exist,tarPath:" + tarFileName);
                    MPIFile.CopyFile(srcFileName, tarFileName);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("[DeviceLogTool],Deserialize(), BackupXMLLog() excption:" + e.Message);

            }
            return true;
        }

        #endregion

        #region private method
        private string GetExtension()
        {
            string fileName = "";
            fileName = ".log";
            return fileName;
        }

        private void SerializeBinary(T obj, string fileName)
        {
            try
            {
                Console.WriteLine("[DeviceRelayCountTool],SerializeBinary(),FileName:" + fileName);
                //建立資料流物件
                FileStream oFileStream = new FileStream(fileName, FileMode.Create);
                //建立二進位格式化物件
                BinaryFormatter myBinaryFormatter = new BinaryFormatter();
                //將物件進行二進位格式序列化，並且將之儲存成檔案
                myBinaryFormatter.Serialize(oFileStream, obj);
                oFileStream.Flush();
                oFileStream.Close();
                oFileStream.Dispose();
            }
            catch (Exception e)
            {
                Console.WriteLine("[DeviceRelayCountTool],SerializeBinary().Exception:" + e.Message);
            }
        }

        private object DeSerializeBinary( string fileName)
        {
            FileStream oFileStream = new FileStream(fileName, FileMode.Open);
            BinaryFormatter myBinaryFormatter = new BinaryFormatter();
            object o = myBinaryFormatter.Deserialize(oFileStream);
            return o;
        }

        
        #endregion
    }

}

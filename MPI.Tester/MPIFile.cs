using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;
using System.Xml.Serialization;

using System.Security;
using System.Security.AccessControl;

using System.Net;
using System.Net.NetworkInformation;

namespace MPI.Tester
{
    public class  MPIFile
    {
        [DllImport("kernel32.dll")]
        public static extern IntPtr _lopen(string lpPathName, int iReadWrite);
        [DllImport("kernel32.dll")]
        public static extern bool CloseHandle(IntPtr hObject);
        public const int OF_READWRITE = 2;
        public const int OF_SHARE_DENY_NONE = 0x40;
        public static readonly IntPtr HFILE_ERROR = new IntPtr(-1);

        public static bool CheckFileIsOpen(string fileNameAndPath)
        {
            IntPtr vHandle = _lopen(fileNameAndPath, OF_READWRITE | OF_SHARE_DENY_NONE);

            if (vHandle == HFILE_ERROR)
            {
                return true;
            }

            CloseHandle(vHandle);

            return false;
        }

        public static bool CopyFile(string sourceFileName, string targetFileName, string backupPath = @"C:\MPI\Backup",bool isCheckFileSize = false)
        //來源檔案完整路徑名稱,輸出檔案完整路徑名稱,複製失敗儲存路徑,是否確認來源/輸出檔案大小是否一致(部分檔案如圖檔會將名稱存在檔案中，會因此導致複製後大小改變)
        {

            string fileName = Path.GetFileNameWithoutExtension(targetFileName) + "_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + Path.GetExtension(targetFileName);
            string tempTar = Path.Combine(backupPath, fileName);
			try
			{
                Console.WriteLine("[MPIFile],CopyFile(),from " + sourceFileName + " to " + targetFileName);
                if (!IsAccessableIP(sourceFileName))
                {
                    Console.WriteLine("[MPIFile],CopyFile(),source Ip path not accessable.");
                    return false;
                }

				if (!File.Exists(sourceFileName))
				{
					Console.WriteLine("[MPIFile],CopyFile()," + sourceFileName + " File is not Exist!");

					return false;
				}
                if (!IsAccessableIP(targetFileName))
                {
                    Console.WriteLine("[MPIFile],CopyFile(),target Ip path not accessable. Backup to " + tempTar);
                    File.Copy(sourceFileName, tempTar);//強迫複製，免得自我呼叫變成無限迴圈
                    return false;
                }

				string dir = Path.GetDirectoryName(targetFileName);

				if (!Directory.Exists(dir))
				{
					Console.WriteLine("[MPIFile],CopyFile()," + dir + " Dir is not Exist & Create!");

					Directory.CreateDirectory(dir);
				}

				if (File.Exists(targetFileName))
				{
					Console.WriteLine("[MPIFile],CopyFile()," + targetFileName + " File is Exist & Delete!");

                   // 以下程式碼會將檔案可讀取及寫入的。

                     FileInfo fInfo = new FileInfo(targetFileName);

                     fInfo.IsReadOnly = false;

                     Console.WriteLine("[MPIFile],Set File Is not Ready only");

					File.Delete(targetFileName);
				}

               

				Console.WriteLine("[MPIFile],CopyFile()," + sourceFileName + " => " + targetFileName);

				File.Copy(sourceFileName, targetFileName);

               

                if (!File.Exists(targetFileName))
                {
                    File.Copy(sourceFileName, tempTar);//強迫複製，免得自我呼叫變成無限迴圈
                    Console.WriteLine("[MPIFile],CopyFile(),Copy Fail, target file not found.");
                    return false;
                }

                if (isCheckFileSize)
                {
                    long slength = new System.IO.FileInfo(sourceFileName).Length;
                    long tlength = new System.IO.FileInfo(targetFileName).Length;
                    if (slength != tlength)
                    {
                        File.Copy(sourceFileName, tempTar);
                        Console.WriteLine("[MPIFile],CopyFile(),Copy Fail, target file size diff from sorce size.");
                        return false;
                    }
                }


				return true;
			}
			catch (IOException e)
			{
                File.Copy(sourceFileName, tempTar,true);//強迫複製，免得自我呼叫變成無限迴圈
				Console.WriteLine("[MPIFile],CopyFile()," + e.ToString());

				return false;
			}
			catch (Exception E)
			{
                File.Copy(sourceFileName, tempTar, true);//強迫複製，免得自我呼叫變成無限迴圈
				Console.WriteLine("[MPIFile],CopyFile()," + E.ToString());

				return false;
			}
        }

		public static bool DeleteFile(string targetFileName)
		{
			try
			{
				Console.WriteLine("[MPIFile],DeleteFile()," + targetFileName);

				if (File.Exists(targetFileName))
				{
					File.Delete(targetFileName);
				}

				return true;
			}
			catch (IOException e)
			{
				Console.WriteLine("[MPIFile],DeleteFile()," + e.ToString());

				return false;
			}
			catch (Exception E)
			{
				Console.WriteLine("[MPIFile],DeleteFile()," + E.ToString());

				return false;
			}
		}

		public static bool DeleteDirectory(string targetDirectory, bool recursive)
		{
			try
			{
				Console.WriteLine("[MPIFile],targetDirectory()," + targetDirectory);

				if (Directory.Exists(targetDirectory))
				{
					Directory.Delete(targetDirectory, recursive);
				}

				return true;
			}
			catch (IOException e)
			{
				Console.WriteLine("[MPIFile],DeleteDirectory()," + e.ToString());

				return false;
			}
			catch (Exception E)
			{
				Console.WriteLine("[MPIFile],DeleteDirectory()," + E.ToString());

				return false;
			}
		}

		public static bool CreateDirectory(string targetDirectory)
		{
			try
			{
				Console.WriteLine("[MPIFile],CreatDirectory()," + targetDirectory);

				if (!Directory.Exists(targetDirectory))
				{
					Directory.CreateDirectory(targetDirectory);
				}

				return true;
			}
			catch (IOException e)
			{
				Console.WriteLine("[MPIFile],CreatDirectory()," + e.ToString());

				return false;
			}
			catch (Exception E)
			{
				Console.WriteLine("[MPIFile],CreatDirectory()," + E.ToString());

				return false;
			}
		}

        public static bool ObjectEquals(object obj1, object obj2)
        {
            var expectedArray = getObjectByte(obj1);
            var targetArray = getObjectByte(obj2);       
            var equals = expectedArray.SequenceEqual(targetArray);
            return equals;
        }

        public static byte[] getObjectByte(object obj)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                XmlSerializer xs = new XmlSerializer(obj.GetType());
                xs.Serialize(memory, obj);
                var array = memory.ToArray();
                return array;
            }
        }

        public static string StringToBinary(string data)
        {
            StringBuilder sb = new StringBuilder();

            foreach (char c in data.ToCharArray())
            {
                sb.Append(Convert.ToString(c, 2).PadLeft(8, '0'));
            }
            return sb.ToString();
        }

        public static string BinaryToString(string data)
        {
            List<Byte> byteList = new List<Byte>();

            for (int i = 0; i < data.Length; i += 8)
            {
                byteList.Add(Convert.ToByte(data.Substring(i, 8), 2));
            }

            return Encoding.ASCII.GetString(byteList.ToArray());
        }

        public static bool IsAccessableIP(string fileAddress)//確認是否為網路資料夾，且可操作
        {
            string[] strArr = fileAddress.Split(new char[] { '\\' });
            if (strArr != null && strArr.Length >= 1)
            {
                string str = strArr[0];
                IPAddress ip;

                if (IPAddress.TryParse(str, out ip))
                {
                    Ping x = new Ping();
                    PingReply reply = x.Send(ip, 1000);

                    if (reply.Status != IPStatus.Success)
                    {
                        Console.WriteLine("[MPIFile], IsDirOfFileExist(),IP Address " + fileAddress + " connect fail");
                        return false;//is ip, but ping fail
                    }

                    try {
                        for (int i = 0; i < 3; ++i)
                        {
                            string tempDicName = DateTime.Now.Ticks.ToString();
                            string tPath = Path.Combine("\\" + str, tempDicName);
                            if (Directory.Exists(tPath))
                            {
                                continue;
                            }
                            else
                            {
                                Directory.CreateDirectory(tPath);
                                Directory.Delete(tPath);
                                break;
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("[MPIFile], IsDirOfFileExist(),IP Address " + fileAddress + " connect fail, Exception:" +  e.Message);
                        return false;//is ip, but ping fail
                    }
                }
            }
            return true;
        }

        public static void DeleteFile(int day, string dir)
        {
            DirectoryInfo dirinfo = new DirectoryInfo(dir);

            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            DirectoryInfo[] dirList = dirinfo.GetDirectories();

            foreach (DirectoryInfo item in dirList)
            {
                DateTime dateTime;

                if (!DateTime.TryParse(item.Name, out  dateTime))
                    continue;

                //Calculate DateTime
                TimeSpan ts1 = new TimeSpan(dateTime.Ticks);
                TimeSpan ts2 = new TimeSpan(DateTime.Now.Ticks);
                TimeSpan ts = ts1.Subtract(ts2).Duration();

                if (ts.Days > day)
                {
                    DeleteDirectory(item.FullName);
                }
            }
        }

        public static void DeleteDirectory(string targetDir)
        {
            string[] files = Directory.GetFiles(targetDir);
            string[] dirs = Directory.GetDirectories(targetDir);
            foreach (string file in files)
            {
                File.SetAttributes(file, FileAttributes.Normal);
                File.Delete(file);
            }
            foreach (string dir in dirs)
            {
                DeleteDirectory(dir);
            }
            Directory.Delete(targetDir, false);

        }
    }
}

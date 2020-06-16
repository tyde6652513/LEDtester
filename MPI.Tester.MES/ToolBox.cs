using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using System.Threading;
using System.ComponentModel;
using MPI.Tester;

namespace Tool
{
	public class ToolBox
	{
		#region >>> DateTimeList Property <<<
		private static string _logPath = @"C:\";
		private static int _logFileKeepLine = 100;
		public static string[] DateTimeList = {	"yyyy/M/d tt hh:mm:ss", 
												  "yyyy/MM/dd tt hh:mm:ss", 
												  "yyyy/MM/dd HH:mm:ss", 
												  "yyyy/M/d HH:mm:ss", 
												  "yyyy/M/dd HH:mm:ss",
												  "yyyy/MM/d HH:mm:ss",
												  "yyyy/M/d tt hh:mm", 
												  "yyyy/MM/dd tt hh:mm", 
												  "yyyy/MM/dd HH:mm", 
												  "yyyy/M/d HH:mm", 
												  "yyyy/M/d H:mm",
												  "yyyy/M/d H:m",
												  "yyyy/M/d", 
												  "yyyy/MM/dd",
												  "yyyy/M/d H:m:s",
												  "yyyy/M/d H:m:ss",
												  "yyyy/M/d H:mm:s", 
												  "yyyy/M/d H:mm:ss",
												  "yyyy/M/d HH:m:s",
												  "yyyy/M/d HH:m:ss",
												  "yyyy/M/d HH:mm:s",
												  "yyyy/M/d HH:mm:ss",
												  "yyyy/M/dd H:m:s",
												  "yyyy/M/dd H:m:ss",
												  "yyyy/M/dd H:mm:s",
												  "yyyy/M/dd H:mm:ss",
												  "yyyy/M/dd HH:m:s",
												  "yyyy/M/dd HH:m:ss",
												  "yyyy/M/dd HH:mm:s",
												  "yyyy/M/dd HH:mm:ss",
												  "yyyy/MM/d H:m:s",
												  "yyyy/MM/d H:m:ss",
												  "yyyy/MM/d H:mm:s", 
												  "yyyy/MM/d H:mm:ss",
												  "yyyy/MM/d HH:m:s",
												  "yyyy/MM/d HH:m:ss",
												  "yyyy/MM/d HH:mm:s",
												  "yyyy/MM/d HH:mm:ss",
												  "yyyy/MM/dd H:m:s",
												  "yyyy/MM/dd H:m:ss",
												  "yyyy/MM/dd H:mm:s",
												  "yyyy/MM/dd H:mm:ss",
												  "yyyy/MM/dd HH:m:s",
												  "yyyy/MM/dd HH:m:ss",
												  "yyyy/MM/dd HH:mm:s",
												  "yyyy/MM/dd HH:mm:ss",

												  "yyyy-M-d tt hh:mm:ss", 
												  "yyyy-MM-dd tt hh:mm:ss", 
												  "yyyy-MM-dd HH:mm:ss", 
												  "yyyy-M-d HH:mm:ss", 
												  "yyyy-M-dd HH:mm:ss",
												  "yyyy-MM-d HH:mm:ss",
												  "yyyy-M-d tt hh:mm", 
												  "yyyy-MM-dd tt hh:mm", 
												  "yyyy-MM-dd HH:mm", 
												  "yyyy-M-d HH:mm", 
												  "yyyy-M-d H:mm",
												  "yyyy-M-d H:m",
												  "yyyy-M-d", 
												  "yyyy-MM-dd",
												  "yyyy-M-d H:m:s",
												  "yyyy-M-d H:m:ss",
												  "yyyy-M-d H:mm:s", 
												  "yyyy-M-d H:mm:ss",
												  "yyyy-M-d HH:m:s",
												  "yyyy-M-d HH:m:ss",
												  "yyyy-M-d HH:mm:s",
												  "yyyy-M-d HH:mm:ss",
												  "yyyy-M-dd H:m:s",
												  "yyyy-M-dd H:m:ss",
												  "yyyy-M-dd H:mm:s",
												  "yyyy-M-dd H:mm:ss",
												  "yyyy-M-dd HH:m:s",
												  "yyyy-M-dd HH:m:ss",
												  "yyyy-M-dd HH:mm:s",
												  "yyyy-M-dd HH:mm:ss",
												  "yyyy-MM-d H:m:s",
												  "yyyy-MM-d H:m:ss",
												  "yyyy-MM-d H:mm:s", 
												  "yyyy-MM-d H:mm:ss",
												  "yyyy-MM-d HH:m:s",
												  "yyyy-MM-d HH:m:ss",
												  "yyyy-MM-d HH:mm:s",
												  "yyyy-MM-d HH:mm:ss",
												  "yyyy-MM-dd H:m:s",
												  "yyyy-MM-dd H:m:ss",
												  "yyyy-MM-dd H:mm:s",
												  "yyyy-MM-dd H:mm:ss",
												  "yyyy-MM-dd HH:m:s",
												  "yyyy-MM-dd HH:m:ss",
												  "yyyy-MM-dd HH:mm:s",
												  "yyyy-MM-dd HH:mm:ss",

												  "yyyy M d tt hh:mm:ss", 
												  "yyyy MM dd tt hh:mm:ss", 
												  "yyyy MM dd HH:mm:ss", 
												  "yyyy M d HH:mm:ss",
												  "yyyy M dd HH:mm:ss",
												  "yyyy MM d HH:mm:ss", 
												  "yyyy M d tt hh:mm", 
												  "yyyy MM dd tt hh:mm", 
												  "yyyy MM dd HH:mm", 
												  "yyyy M d HH:mm", 
												  "yyyy M d H:mm",
												  "yyyy M d H:m",
												  "yyyy M d", 
												  "yyyy MM dd",
												  "yyyy M d H:m:s",
												  "yyyy M d H:m:ss",
												  "yyyy M d H:mm:s", 
												  "yyyy M d H:mm:ss",
												  "yyyy M d HH:m:s",
												  "yyyy M d HH:m:ss",
												  "yyyy M d HH:mm:s",
												  "yyyy M d HH:mm:ss",
												  "yyyy M dd H:m:s",
												  "yyyy M dd H:m:ss",
												  "yyyy M dd H:mm:s",
												  "yyyy M dd H:mm:ss",
												  "yyyy M dd HH:m:s",
												  "yyyy M dd HH:m:ss",
												  "yyyy M dd HH:mm:s",
												  "yyyy M dd HH:mm:ss",
												  "yyyy MM d H:m:s",
												  "yyyy MM d H:m:ss",
												  "yyyy MM d H:mm:s", 
												  "yyyy MM d H:mm:ss",
												  "yyyy MM d HH:m:s",
												  "yyyy MM d HH:m:ss",
												  "yyyy MM d HH:mm:s",
												  "yyyy MM d HH:mm:ss",
												  "yyyy MM dd H:m:s",
												  "yyyy MM dd H:m:ss",
												  "yyyy MM dd H:mm:s",
												  "yyyy MM dd H:mm:ss",
												  "yyyy MM dd HH:m:s",
												  "yyyy MM dd HH:m:ss",
												  "yyyy MM dd HH:mm:s",
												  "yyyy MM dd HH:mm:ss",

												  "yy/M/d tt hh:mm:ss", 
												  "yy/MM/dd tt hh:mm:ss", 
												  "yy/MM/dd HH:mm:ss", 
												  "yy/M/d HH:mm:ss",
												  "yy/M/dd HH:mm:ss",
												  "yy/MM/d HH:mm:ss", 
												  "yy/M/d tt hh:mm", 
												  "yy/MM/dd tt hh:mm", 
												  "yy/MM/dd HH:mm", 
												  "yy/M/d HH:mm", 
												  "yy/M/d H:mm",
												  "yy/M/d H:m",
												  "yy/M/d", 
												  "yy/MM/dd",
												  "yy/M/d H:m:s",
												  "yy/M/d H:m:ss",
												  "yy/M/d H:mm:s", 
												  "yy/M/d H:mm:ss",
												  "yy/M/d HH:m:s",
												  "yy/M/d HH:m:ss",
												  "yy/M/d HH:mm:s",
												  "yy/M/d HH:mm:ss",
												  "yy/M/dd H:m:s",
												  "yy/M/dd H:m:ss",
												  "yy/M/dd H:mm:s",
												  "yy/M/dd H:mm:ss",
												  "yy/M/dd HH:m:s",
												  "yy/M/dd HH:m:ss",
												  "yy/M/dd HH:mm:s",
												  "yy/M/dd HH:mm:ss",
												  "yy/MM/d H:m:s",
												  "yy/MM/d H:m:ss",
												  "yy/MM/d H:mm:s", 
												  "yy/MM/d H:mm:ss",
												  "yy/MM/d HH:m:s",
												  "yy/MM/d HH:m:ss",
												  "yy/MM/d HH:mm:s",
												  "yy/MM/d HH:mm:ss",
												  "yy/MM/dd H:m:s",
												  "yy/MM/dd H:m:ss",
												  "yy/MM/dd H:mm:s",
												  "yy/MM/dd H:mm:ss",
												  "yy/MM/dd HH:m:s",
												  "yy/MM/dd HH:m:ss",
												  "yy/MM/dd HH:mm:s",
												  "yy/MM/dd HH:mm:ss",

												  "yy-M-d tt hh:mm:ss", 
												  "yy-MM-dd tt hh:mm:ss", 
												  "yy-MM-dd HH:mm:ss", 
												  "yy-M-d HH:mm:ss", 
												  "yy-M-dd HH:mm:ss",
												  "yy-MM-d HH:mm:ss",
												  "yy-M-d tt hh:mm", 
												  "yy-MM-dd tt hh:mm", 
												  "yy-MM-dd HH:mm", 
												  "yy-M-d HH:mm", 
												  "yy-M-d H:mm",
												  "yy-M-d H:m",
												  "yy-M-d", 
												  "yy-MM-dd",
												  "yy-M-d H:m:s",
												  "yy-M-d H:m:ss",
												  "yy-M-d H:mm:s", 
												  "yy-M-d H:mm:ss",
												  "yy-M-d HH:m:s",
												  "yy-M-d HH:m:ss",
												  "yy-M-d HH:mm:s",
												  "yy-M-d HH:mm:ss",
												  "yy-M-dd H:m:s",
												  "yy-M-dd H:m:ss",
												  "yy-M-dd H:mm:s",
												  "yy-M-dd H:mm:ss",
												  "yy-M-dd HH:m:s",
												  "yy-M-dd HH:m:ss",
												  "yy-M-dd HH:mm:s",
												  "yy-M-dd HH:mm:ss",
												  "yy-MM-d H:m:s",
												  "yy-MM-d H:m:ss",
												  "yy-MM-d H:mm:s", 
												  "yy-MM-d H:mm:ss",
												  "yy-MM-d HH:m:s",
												  "yy-MM-d HH:m:ss",
												  "yy-MM-d HH:mm:s",
												  "yy-MM-d HH:mm:ss",
												  "yy-MM-dd H:m:s",
												  "yy-MM-dd H:m:ss",
												  "yy-MM-dd H:mm:s",
												  "yy-MM-dd H:mm:ss",
												  "yy-MM-dd HH:m:s",
												  "yy-MM-dd HH:m:ss",
												  "yy-MM-dd HH:mm:s",
												  "yy-MM-dd HH:mm:ss",

												  "yy M d tt hh:mm:ss", 
												  "yy MM dd tt hh:mm:ss", 
												  "yy MM dd HH:mm:ss", 
												  "yy M d HH:mm:ss", 
												  "yy M dd HH:mm:ss",
												  "yy MM d HH:mm:ss",
												  "yy M d tt hh:mm", 
												  "yy MM dd tt hh:mm", 
												  "yy MM dd HH:mm", 
												  "yy M d HH:mm", 
												  "yy M d H:mm",
												  "yy M d H:m",
												  "yy M d", 
												  "yy MM dd",
												  "yy M d H:m:s",
												  "yy M d H:m:ss",
												  "yy M d H:mm:s", 
												  "yy M d H:mm:ss",
												  "yy M d HH:m:s",
												  "yy M d HH:m:ss",
												  "yy M d HH:mm:s",
												  "yy M d HH:mm:ss",
												  "yy M dd H:m:s",
												  "yy M dd H:m:ss",
												  "yy M dd H:mm:s",
												  "yy M dd H:mm:ss",
												  "yy M dd HH:m:s",
												  "yy M dd HH:m:ss",
												  "yy M dd HH:mm:s",
												  "yy M dd HH:mm:ss",
												  "yy MM d H:m:s",
												  "yy MM d H:m:ss",
												  "yy MM d H:mm:s", 
												  "yy MM d H:mm:ss",
												  "yy MM d HH:m:s",
												  "yy MM d HH:m:ss",
												  "yy MM d HH:mm:s",
												  "yy MM d HH:mm:ss",
												  "yy MM dd H:m:s",
												  "yy MM dd H:m:ss",
												  "yy MM dd H:mm:s",
												  "yy MM dd H:mm:ss",
												  "yy MM dd HH:m:s",
												  "yy MM dd HH:m:ss",
												  "yy MM dd HH:mm:s",
												  "yy MM dd HH:mm:ss",
												};
		#endregion

		public ToolBox()
		{
		}

		public static bool WriteCSV(string pathFileName, List<String[]> ls)
		{
			return WriteCSV(pathFileName, false, ls);
		}

		public static bool WriteCSV(string pathFileName, bool append, List<String[]> ls)
		{

			if (Directory.Exists(Path.GetDirectoryName(pathFileName)) == false)
			{
				Directory.CreateDirectory(Path.GetDirectoryName(pathFileName));
			}

			try
			{
				using (StreamWriter fileWriter = new StreamWriter(pathFileName, append, Encoding.Default))
				{
					foreach (String[] strArr in ls)
					{
						fileWriter.WriteLine(String.Join(",", strArr));
					}
					fileWriter.Flush();
					fileWriter.Close();
				}
			}
			catch (IOException)
			{
				return false;
			}
			catch (Exception )
			{
				return false;
			}

			return true;

		}

		public static List<String[]> ReadCSV(string pathFileName)
		{
			List<String[]> ls = null;

			if (File.Exists(pathFileName))
			{
				try
				{
					ls = new List<String[]>();
					using (StreamReader fileReader = new StreamReader(pathFileName))
					{

						string strLine = "";
						while (strLine != null)
						{
							strLine = fileReader.ReadLine();

							if (strLine != null)
							{
								if (strLine.Length >= 1)
								{
									if (strLine.Substring(strLine.Length - 1, 1) == ",")
									{
										strLine = strLine.Remove(strLine.Length - 1);
									}
									ls.Add(strLine.Split(','));
								}
								else if (strLine.Length == 0)
								{
									string[] empty = new string[1];
									empty[0] = "";
									ls.Add(empty);
								}
							}
						}

						fileReader.Close();
					}
				}
				catch (IOException)
				{
					return null;
				}
				catch (Exception)
				{
					return null;
				}

				return ls;
			}
			else
			{
				return null;
			}

		}

		public static List<string[]> ReadRow(string pathFileName)
		{
			List<string[]> Cs = new List<string[]>();

			if (File.Exists(pathFileName))
			{

				StreamReader fileReader = new StreamReader(pathFileName);
				string strRow = ".";
				strRow = fileReader.ReadLine();
				fileReader.Close();
			}

			return Cs;

		}

		public static bool Serialize(string FileName, object Obj)
		{
			try
			{
				if (System.IO.Path.GetExtension(FileName).ToLower() == ".xml")
				{
					System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(Obj.GetType());
					System.Xml.XmlTextWriter xmlTextWriter = new System.Xml.XmlTextWriter(FileName, Encoding.ASCII);
					x.Serialize(xmlTextWriter, Obj);
					xmlTextWriter.Close();
				}
				else if (System.IO.Path.GetExtension(FileName).ToLower() == ".dat")
				{
					using (System.IO.FileStream fileStream = new System.IO.FileStream(FileName, System.IO.FileMode.Create))
					{
						System.Runtime.Serialization.Formatters.Binary.BinaryFormatter binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
						binaryFormatter.Serialize(fileStream, Obj);

						fileStream.Close();
					}
				}

				return true;
			}
			catch
			{
				return false;
			}
		}

		public static T Deserialize<T>(string FileName)
		{
			System.Xml.XmlDocument xdoc = new System.Xml.XmlDocument();

			try
			{
				object obj = new object();

				if (System.IO.Path.GetExtension(FileName).ToLower() == ".xml")
				{
					xdoc.Load(FileName);
					System.Xml.XmlNodeReader reader = new System.Xml.XmlNodeReader(xdoc.DocumentElement);
					System.Xml.Serialization.XmlSerializer ser = new System.Xml.Serialization.XmlSerializer(typeof(T));
					obj = ser.Deserialize(reader);
				}
				else if (System.IO.Path.GetExtension(FileName).ToLower() == ".dat")
				{
					using (System.IO.FileStream fileStream = new System.IO.FileStream(FileName, System.IO.FileMode.Open))
					{
						System.Runtime.Serialization.Formatters.Binary.BinaryFormatter binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
						obj = binaryFormatter.Deserialize(fileStream);

						fileStream.Close();
					}
				}

				return (T)obj;
			}
			catch
			{
				return default(T);
			}
		}

		public static bool MoveFileToBackUpPath(string FileName)
		{
			string backupPath = Path.Combine(Path.GetDirectoryName(FileName), "BackUp");

			DirectoryInfo backupDir = new DirectoryInfo(backupPath);

			try
			{
				if (!backupDir.Exists)
				{
					Directory.CreateDirectory(backupPath);
				}

				string backupFullName = Path.Combine(backupPath, Path.GetFileName(FileName));

				MPIFile.DeleteFile(backupFullName);

				File.Move(FileName, Path.Combine(backupPath, Path.GetFileName(FileName)));

				return true;
			}
			catch
			{
				return false;
			}
		}

		public static bool MoveFileToBackUpPath(string FileName, string BackupFileName, bool isBackup = true)
		{
			DirectoryInfo backupDir = new DirectoryInfo(BackupFileName);

			try
			{
				if (!backupDir.Exists)
				{
					Directory.CreateDirectory(BackupFileName);
				}

				string backupFullName = Path.Combine(BackupFileName, Path.GetFileName(FileName));

				MPIFile.DeleteFile(backupFullName);

				MPIFile.CopyFile(FileName, backupFullName);

				if (isBackup)
				{
					return MoveFileToBackUpPath(FileName);
				}
				else
				{
					MPIFile.DeleteFile(FileName);
					return true;
				}
			}
			catch
			{
				return false;
			}
		}

		public static bool CopyFileToBackUpPath(string FileName, string BackupFileName, string reName = "")
		{
			DirectoryInfo backupDir = new DirectoryInfo(BackupFileName);

			try
			{
				string backupFullName = "";

				if (reName == "")
				{
					backupFullName = Path.Combine(BackupFileName, Path.GetFileName(FileName));
				}
				else
				{
					backupFullName = Path.Combine(BackupFileName, reName);
				}

				if (!backupDir.Exists)
				{
                    backupDir.Create();
				}


				MPIFile.DeleteFile(backupFullName);

				MPIFile.CopyFile(FileName, backupFullName);

				return true;
			}
			catch
			{
				return false;
			}
		}

		public static void LogStart(string logPath, int keepLine)
		{
			_logPath = logPath;
			_logFileKeepLine = keepLine;
		}

		public static void Log(string logMsg)
		{
			try
			{
				List<string[]> logFile;

				if (File.Exists(_logPath))
				{
					logFile = ReadCSV(_logPath);
				}
				else
				{
					logFile = new List<string[]>();
				}

				while (logFile.Count > _logFileKeepLine - 1)
				{
					logFile.RemoveAt(logFile.Count - 1);
				}

				string[] lineStr = new string[2];

				lineStr[0] = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.ffff");

				lineStr[1] = logMsg;

				logFile.Insert(0, lineStr);

				WriteCSV(_logPath, false, logFile);
			}
			catch { }
		}
	}
}

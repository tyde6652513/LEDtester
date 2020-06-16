using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using MPI.Tester;
using System.Windows.Forms;
using DevComponents.DotNetBar;

namespace MPI.Tester.Gui
{
    class UILog
    {
        private static string _logPath = @"C:\";
        private static int _logFileKeepLine = 100;

		private static void WriteLog(string Text)
		{
			try
			{
				List<string[]> logFile;

				if (File.Exists(_logPath))
				{
					logFile = CSVUtil.ReadCSV(_logPath);
				}
				else
				{
					logFile = new List<string[]>();
				}

				while (logFile.Count > _logFileKeepLine - 1)
				{
					logFile.RemoveAt(0);
				}

				string[] lineStr = new string[2];

				lineStr[0] = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.ffff");

				lineStr[1] = Text;

				logFile.Add(lineStr);

				CSVUtil.WriteCSV(_logPath, false, logFile);
			}
			catch
			{

			}
		}

        public static void LogStart(string logPath, string fileName, int keepLine)
        {
			_logPath = Path.Combine(logPath, DateTime.Now.ToString("yyyy-MM-dd"), fileName + "ui");
            _logFileKeepLine = keepLine;

			Log("");
			Log("=========================================");
			Log("========== Application Start   ==========");
			Log("=========================================");
        }

        public static void Log(string logMsg)
        {
            WriteLog(logMsg);
        }

		public static void Log(Form frmObj, object compObj, string eventArgs, string logMsg = "")
		{
			string Text = string.Empty;

			if (frmObj == null)
			{
				Text = "[null],";
			}
			else
			{
				Text = "[" + frmObj.Name + "],";
			}

			if (compObj == null)
			{
				Text = Text + "[null],";
			}
			//else if (compObj is ToolStrip)
			//{
			//    Text = Text + "[" + (compObj as ToolStrip).Name + "],";
			//}
			else if (compObj is SuperTabControl)
			{
				Text = Text + "[" + (compObj as SuperTabControl).Name + "],";
			}
			else if (compObj is ButtonX)
			{
				Text = Text + "[" + (compObj as ButtonX).Name + "],";
			}
			else
			{
				Text = Text + "[UILog Unefine],";
			}		

			Text = Text + "[" + eventArgs + "]";

			if (logMsg != "")
			{
				Text = Text + "," + logMsg;
			}

			WriteLog(Text);
		}
    }
}

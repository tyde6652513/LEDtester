using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Threading;
using System.IO;

using MPI.Tester.Data;


namespace MPI.Tester.Gui
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>

		static System.Threading.Mutex ReentryMutex;

		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();

			Application.SetCompatibleTextRenderingDefault( false );

			Thread.CurrentThread.Name = "Main Thread";
			Thread.CurrentThread.Priority = ThreadPriority.AboveNormal;
			System.Diagnostics.Process.GetCurrentProcess().PriorityClass = System.Diagnostics.ProcessPriorityClass.High;
			
			// check reentry
			if ( IsReenrty() )
			{
				System.Windows.Forms.MessageBox.Show( "Program has launched already!" );
				return;
			}

			// Creat Log
			string fileName = "Log_" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");

			MPI.MCF.Log.DebugStreamWriter.Transfer(Constants.Paths.LOG_FILE, fileName,30);

            Console.WriteLine("LEDTester_" + Host.GetProgramVersion() + " START ");

			UILog.LogStart(Constants.Paths.LOG_FILE, fileName, 1000);


			// (1) Open "Datacenter", Load all setting and data from files
			DataCenter.Open();      

			// (2) Open "FromAgent", create all UI forms
			FormAgent.Open(EBaseWindowPosition.Default);

			// (3) Open "Host", then initialize tester system varaible and "AppSystem"
			Host.Open();

			// (4) Run program
			Application.Run( FormAgent.MainForm );

			// (5) Close "Host"
			Host.Close();

			// (6) Close "Datacenter", and save all files
			DataCenter.Close();

			// (6) Close "UI"
			FormAgent.Close();

            MPI.MCF.Log.DebugStreamWriter.Restore();

			if ( ReentryMutex != null )
				ReentryMutex.Close();

		}

		private static bool IsReenrty()
		{
			bool created_new = false;

			ReentryMutex = null;
			try
			{
				ReentryMutex = new Mutex( true, Host.Constants.REENTRY_MUTEX_NAME, out created_new );
			}
			catch ( Exception )
			{
				return true;
				//throw new Exception(" Unable to create mutex !");
			}

			// There are three cases : (1) The mutex does not exit.
			// (2) The mutex exists, but the current user doesn't have access.
			// (3) The mutex exists, and the user has access.
			// Ref. MSDN

			if ( created_new ) //crated -> not re-entry
				return false;

			return true; //non-created -> re-entry
		}
	}
}
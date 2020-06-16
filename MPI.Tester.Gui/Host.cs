using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Drawing;
using System.Resources;
using System.Reflection;

using MPI.Tester.TestKernel;
using MPI.Tester.TestServer;
using MPI.Tester.Data;
using MPI.AuthorityControl;
using MPI.Resource;
using MPI.Log;
using MPI.Database;
using MPI.Windows.Forms;

namespace MPI.Tester.Gui
{
	public class Host
	{
	    public class Constants
		{
			public const string REENTRY_MUTEX_NAME = "LEDTester_Reentry_Mutex";
		}

		private static MultilingualAgent MultilingualAgent;
		
		#region >>> Public Static Fields <<<

		public static ResourceManager RM = new ResourceManager("MPI.Tester.Gui.ResourceErr", Assembly.Load("LEDTester"));
		public static System.Threading.Mutex ReentryMutex;		// Reentry mutex
		public static ResourceAgent ResourceAgent;				// resource agent
		public static Storage _MPIStorage = null;

		public static frmLogin _frmLogin = new frmLogin();
 
        public static ResourceManager resourceManagerMessage = new ResourceManager("MPI.Tester.Gui.ResourceMeassage", Assembly.Load("LEDTester"));

        public static StringBuilder _alarmDescribe = new StringBuilder();

        public static event EventHandler OnTestItemChangeEvent;

        public static event EventHandler OnBarcCodeSaveEvent;

		public static event ErrorCodeEventHandler ErrorCodeEvent;

		#endregion		

		#region >>> Private Static Methods <<<

		private static void UserCenter_UserLogin( object sender, EventArgs e )
		{
			//if ( AuthorityCenter.UserCenter.CheckAuthority( MPI.AuthorityControl.EAuthorityLevel.Designer ) )
			//{
			//   Program._frmMain.SystemsecurityLevel( MPI.AuthorityControl.EAuthorityLevel.Designer );
			//}
			//else if ( AuthorityCenter.UserCenter.CheckAuthority( MPI.AuthorityControl.EAuthorityLevel.Maintenance ) )
			//{
			//   Program._frmMain.SystemsecurityLevel( MPI.AuthorityControl.EAuthorityLevel.Maintenance );
			//}

			//else if ( AuthorityCenter.UserCenter.CheckAuthority( MPI.AuthorityControl.EAuthorityLevel.SuperEngineer ) )
			//{
			//   Program._frmMain.SystemsecurityLevel( MPI.AuthorityControl.EAuthorityLevel.SuperEngineer );
			//}

			//else if ( AuthorityCenter.UserCenter.CheckAuthority( MPI.AuthorityControl.EAuthorityLevel.Engineer ) )
			//{
			//   Program._frmMain.SystemsecurityLevel( MPI.AuthorityControl.EAuthorityLevel.Engineer );
			//}
			//else if ( AuthorityCenter.UserCenter.CheckAuthority( MPI.AuthorityControl.EAuthorityLevel.TechnicalAssist ) )
			//{
			//   Program._frmMain.SystemsecurityLevel( MPI.AuthorityControl.EAuthorityLevel.TechnicalAssist );
			//}
			//else if ( AuthorityCenter.UserCenter.CheckAuthority( MPI.AuthorityControl.EAuthorityLevel.Basic ) )
			//{
			//   Program._frmMain.SystemsecurityLevel( MPI.AuthorityControl.EAuthorityLevel.Basic );
			//}
			//else
			//{
			//   Program._frmMain.SystemsecurityLevel( MPI.AuthorityControl.EAuthorityLevel.None );
			//}
		}

		private static string MultilingualStringPrefix( Control c )
		{
			return Data.Constants.I18N_STR_HEADER + c.Name + "::";
		}

		#endregion

		#region >>> Public Static Methods <<<

		public static void Open()
		{
			_frmLogin.ShowDialog();			

			// (1) Create "_MPIStorage" and open it
			_MPIStorage = new Storage();		//new Storage(DataCenter._product);
			_MPIStorage.Open();


			// (2) Create the Form Instance from FormAgent.RetrieveForm() function.
			UpdateDataToAllUIForm();

			// (3) Create "_MPITestKernel", "_MPITestServer" or "_MPITCPIPServer",
			//     Open server then tester can receive command from prober
			AppSystem.Initialize();

            FormAgent.MainForm.Hide();
            FormAgent.BaseForm.Show();
            FormAgent.BaseForm.Hide();

			//_usrLoginBox = new UserLoginBox( true, new Version( 0, 0, 0, 2 ), new Version( 0, 0, 0, 2 ), new Version( 0, 0, 0, 2 ) );
		}

		public static void Close()
		{
			AppSystem.Close();

			_MPIStorage.Close();

			//FormAgent.Close();

			GC.Collect();
		}

        public static void UpdateDataToAllUIForm()
        {
            FormAgent.MainForm.SetAuthorityLevel(DataCenter._userManag.CurrentAuthority);
            FormAgent.RecipeForm.UpdateDataToUIForm();
            FormAgent.ConditionForm.UpdateDataToUIForm();
            FormAgent.ConditionCoefForm.UpdateDataToUIForm();
            FormAgent.BinSettingForm.UpdateDataToUIForm();
            FormAgent.TestResultForm.UpdateDataToUIForm();
            FormAgent.TestResultSpectrum.UpdateDataToUIForm();
            FormAgent.TestResultAnalyzeForm.UpdateDataToUIForm();
            FormAgent.TestResultCurveAnalyzeForm.UpdateDataToUIForm();
            FormAgent.BaseForm.UpdateDataToUIForm();
            FormAgent.SetProductForm.UpdateDataToUIForm();
			FormAgent.SetMachineForm.UpdateDataToUIForm();
            FormAgent.ChannelCondition.UpdateDataToUIForm();

		    FireTestItemChange();
        }

        public static void FireTestItemChange()
        {
            if (!FormAgent.BaseForm.IsHandleCreated)
                return;

            if (OnTestItemChangeEvent != null)
            {
                FormAgent.BaseForm.Invoke((EventHandler)OnTestItemChangeEvent, null, null);
                // FormAgent.BaseForm.Invoke((EventHandler<SwitchUIArgs>)SwitchUIEvent, null, UIdata);
                //  SwitchUIEvent.Invoke(null, map_data);
            }
        }

        public static void FireSaveDataEvent()
        {
            if (!FormAgent.BaseForm.IsHandleCreated)
                return;

            if (OnBarcCodeSaveEvent != null)
            {
                FormAgent.BaseForm.Invoke((EventHandler)OnBarcCodeSaveEvent, null, null);
                // FormAgent.BaseForm.Invoke((EventHandler<SwitchUIArgs>)SwitchUIEvent, null, UIdata);
                //  SwitchUIEvent.Invoke(null, map_data);
            }
        }

		public static void TestItemDataChangeEventHandler(object o, EventArgs e)
		{
			DataCenter.CompareTestItemAndBinItem();

            DataCenter._conditionCtrl.CopyTestItemArrayToEachChannel();

			DataCenter._uiSetting.WeiminUIData.CustomerRemark01 = DataCenter._conditionCtrl.ParseWM617GainOffsetList(DataCenter._product.LOPSaveItem);

           // Host.UpdateDataToAllUIForm();

            DataCenter.Save();

            AppSystem.SetDataToSystem();

            // Roy 2015.08.10
            Host.UpdateDataToAllUIForm();
		}

		/// <summary>
		/// Register a control into multilingual agent.
		/// </summary>
		public static void RegisterMultilingualAgent(System.Windows.Forms.Control c)
		{
			MultilingualAgent.Register(c, MultilingualStringPrefix(c));
		}

		/// <summary>
		/// Unregister a control from multilingual agent.
		/// </summary>
		public static void UnregisterMultilingualAgent(System.Windows.Forms.Control c)
		{
			MultilingualAgent.Unregister(c);
		}

		/// <summary>
		/// Process the multilingual agent.
		/// </summary>
		public static void ProcessMultilingualAgent(Control c)
		{
			if (MultilingualAgent.IsRegistered(c))
				MultilingualAgent.Process(c);
			else
				MultilingualAgent.Process(c, MultilingualStringPrefix(c));
		}

		/// <summary>
		/// Process the multilingual agent.
		/// </summary>
		public static void ChangeLanguage(ELanguage language)
		{
			if (Host.ResourceAgent.Language == language)
				return;

			Host.ResourceAgent.Language = language;
			MultilingualAgent.Process();
		}
		
		public static string GetProgramVersion()
		{
			Assembly asm = Assembly.GetExecutingAssembly();
			AssemblyTitleAttribute titleAtt = (AssemblyTitleAttribute)Attribute.GetCustomAttribute(asm, typeof(AssemblyTitleAttribute));
			Version asmVer = asm.GetName().Version;

			DateTime compileDate = new DateTime((asmVer.Build - 1) * TimeSpan.TicksPerDay + asmVer.Revision * TimeSpan.TicksPerSecond * 2).AddYears(1999);
			// string myver = asmVer.Major.ToString() + "." + asmVer.Minor.ToString();
			// myver = myver + "." + compileDate.ToString("yyyyMMdd-hh:mm");
			// this.Text = titleAtt.Title + "  [" + myver + "]" + "-" + DateStamp.BuildTime.ToShortDateString() ; 

			object[] att = asm.GetCustomAttributes(typeof(AssemblyFileVersionAttribute), false);
			object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyFileVersionAttribute), false);
			string myver = "";
			if (att == null || att.Length == 0)
			{
				myver = "0.0.0.1";
			}
			else
			{
				myver = ((AssemblyFileVersionAttribute)attributes[0]).Version;
			}

			myver = "Ver " + myver;

#if (DebugVer)
			myver += "_";
			myver += compileDate.ToString("yyyy/MM/dd-hh:mm");
#endif
			return myver;
		}

		public static void SetErrorCode(EErrorCode errorCode, string errorMsg = "")
		{
			if (Host.ErrorCodeEvent != null)
			{
                if (errorCode != EErrorCode.NONE)
                {
                    Host.ErrorCodeEvent(new ErrorCodeEventArgs(errorCode, errorMsg));
                }
			}
		}

		#endregion
	}
}

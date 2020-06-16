using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Globalization;
using System.ComponentModel;
using System.Drawing;
using System.Xml;
using System.IO;

using MPI.Tester.Data;

namespace MPI.Tester.Gui
{
	public struct FormItem
	{
		public string Name;
		public string Title;
		public string IconPath;
		public bool IsDefault;
		public string FullAssembly;

		public Type FormType
		{
			get
			{
				return Type.GetType( this.FullAssembly );
			}
		}

		public Image FormImage
		{
			get
			{
				return Image.FromFile( Path.Combine( Application.StartupPath, Path.Combine( "Icon", this.IconPath ) ) );
			}
		}
	}
	
	/// <summary>
	/// FormAgent class.
	/// </summary>
	class FormAgent
	{

		#region >>> Private Field <<<

		private static readonly Size defaultSize = new Size( 1280, 1024 );
		private static Dictionary<Type, Form> formList;
		private static Form _baseForm = null;
		private static Form _floatToolbar = null;
		private static Dictionary<string, List<FormItem>> _toolbarItems;
		#endregion

		#region >>> Public Static Property <<<

		public static frmMain MainForm
		{
			//get { return _floatToolbar; }
			get
			{
				return (frmMain) _floatToolbar;
			}
		}

		public static frmBase BaseForm
		{
			get
			{
				return (frmBase)_baseForm;
				//return (frmBase)FormAgent.RetrieveForm(typeof(frmBase));
			}
		}

		public static frmWaferMap WaferMapForm
		{
			get
			{
				return ( frmWaferMap ) FormAgent.RetrieveForm( typeof( frmWaferMap ) );
			}
		}

		public static frmCondition ConditionForm
		{
			get
			{
				return ( frmCondition ) FormAgent.RetrieveForm( typeof( frmCondition ) );
			}
		}

        public static frmSetProduct SetProductForm
        {
            get
            {
                return (frmSetProduct)FormAgent.RetrieveForm(typeof(frmSetProduct));
            }
        }

		public static frmConditionCoef ConditionCoefForm
		{
			get
			{
				return (frmConditionCoef)FormAgent.RetrieveForm(typeof(frmConditionCoef));
			}
		}

		public static frmBinSetting BinSettingForm
		{
			get
			{
				return ( frmBinSetting ) FormAgent.RetrieveForm( typeof( frmBinSetting ) );
			}
		}

		public static frmOpRecipe RecipeForm
		{
			get
			{
				return (frmOpRecipe)FormAgent.RetrieveForm(typeof(frmOpRecipe));
			}
		}

		public static frmTestResult TestResultForm
		{
			get
			{
				return ( frmTestResult ) FormAgent.RetrieveForm( typeof( frmTestResult ) );
			}
		}

		public static frmTestResultSpectrum TestResultSpectrum
		{
			get
			{
				return (frmTestResultSpectrum)FormAgent.RetrieveForm(typeof(frmTestResultSpectrum));
			}
		}

        public static frmTestResultChart frmTestResultChart
        {
            get
            {
                return (frmTestResultChart)FormAgent.RetrieveForm(typeof(frmTestResultChart));
            }
        }

        public static frmTestResultCurveAnalysis TestResultCurveAnalyzeForm
        {
            get
            {
                return (frmTestResultCurveAnalysis)FormAgent.RetrieveForm(typeof(frmTestResultCurveAnalysis));
            }
        }

        public static frmTestResultAnalyze TestResultAnalyzeForm
        {
            get
            {
                return (frmTestResultAnalyze)FormAgent.RetrieveForm(typeof(frmTestResultAnalyze));
            }
        }

        public static frmTestResultAnalyze2 TestResultAnalyzeForm2
        {
            get
            {
                return (frmTestResultAnalyze2)FormAgent.RetrieveForm(typeof(frmTestResultAnalyze2));
            }
        }

		public static frmUserManager UserManagerForm
		{
			get
			{
				return ( frmUserManager ) FormAgent.RetrieveForm( typeof( frmUserManager ) );
			}
		}

        public static frmSetUISetting SetUISettingForm
        {
            get
            {
                return (frmSetUISetting)FormAgent.RetrieveForm(typeof(frmSetUISetting));
            }
        }

        public static frmSetSysParam SetSysParamForm
        {
            get
            {
                return (frmSetSysParam)FormAgent.RetrieveForm(typeof(frmSetSysParam));
            }
        }

        public static frmSetMachine SetMachineForm
        {
            get
            {
                return (frmSetMachine)FormAgent.RetrieveForm(typeof(frmSetMachine));
            }
        }

        public static frmChannelCondition ChannelCondition
        {
            get
            {
                return (frmChannelCondition)FormAgent.RetrieveForm(typeof(frmChannelCondition));
            }
        }

        public static frmConditionItemSetting ConditionItemSetting
        {
            get
            {
                return (frmConditionItemSetting)FormAgent.RetrieveForm(typeof(frmConditionItemSetting));
            }
        }

		public static Point BasePosition;

		#endregion

		#region >>> Private Static Method <<<

		private static void setBaseWindowPosition(EBaseWindowPosition posi)
		{
			Console.WriteLine("[FormAgent], setBaseWindowPosition()");

			switch (posi)
			{
				case EBaseWindowPosition.Default:
					BasePosition = new Point();
					break;
				//-----------------------------------------------------------------------------------------
				case EBaseWindowPosition.Bottom:
					BasePosition = new Point(0, Screen.PrimaryScreen.Bounds.Height);
					break;
				//-----------------------------------------------------------------------------------------
				case EBaseWindowPosition.Right:
					BasePosition = new Point(Screen.PrimaryScreen.Bounds.Right, 0);
					break;
				//-----------------------------------------------------------------------------------------
				case EBaseWindowPosition.AutoFind:
					BasePosition = Screen.PrimaryScreen.WorkingArea.Location;
					for (int i = 0; i < Screen.AllScreens.Length; i++)
					{
						if (Screen.AllScreens[i].Primary == true)
							continue;
						BasePosition = Screen.AllScreens[i].WorkingArea.Location;
						break;
					}
					break;
				//-----------------------------------------------------------------------------------------
				default:
					break;
			}

			_baseForm.Location = BasePosition;
		}

		private static void loadToolbarSetting()
		{
			Console.WriteLine("[FormAgent], loadToolbarSetting()");

			string path = Path.Combine(Constants.Paths.DATA_FILE, "FormAgent.xml");

			XmlDocument xml = new XmlDocument();
			xml.Load(path);

			_toolbarItems = new Dictionary<string, List<FormItem>>(xml.DocumentElement.ChildNodes.Count);
			foreach (XmlElement item in xml.DocumentElement.ChildNodes)
			{
				string tag_name = item.GetAttribute("tag");

				List<FormItem> list = new List<FormItem>(item.ChildNodes.Count);
				foreach (XmlElement form in item.ChildNodes)
				{
					FormItem fi = new FormItem();
					fi.FullAssembly = form["assembly"].InnerText;
					fi.IconPath = form.GetAttribute("icon");
					fi.IsDefault = "true".Equals(form.GetAttribute("default"), StringComparison.OrdinalIgnoreCase);
					fi.Name = form.GetAttribute("name");
					fi.Title = form.GetAttribute("title");
					list.Add(fi);
				}

				_toolbarItems.Add(tag_name, list);
			}

		}

		public static void BuildFormTabs()
		{
			Console.WriteLine("[FormAgent], BuildFormTabs()");

			(_baseForm as frmBase).BuildTabs( _toolbarItems);
		}

		#endregion

		#region >>> Public Static Method <<<

		/// <summary>
		/// Initial form agent.
		/// </summary>
		public static void Open( EBaseWindowPosition posi )
		{
			Console.WriteLine("[FormAgent], Open()");

			if ( Screen.AllScreens.Length > 1 )
				DataCenter._bStandAlone = false;
			else
				DataCenter._bStandAlone = true;

			if ( formList == null )
				formList = new Dictionary<Type, Form>();

			if ( _floatToolbar == null )
			{
				_floatToolbar = new frmMain();
			}

			_baseForm = new frmBase();
			formList.Add( typeof( frmBase ), _baseForm );

			FormAgent.setBaseWindowPosition( posi );
			FormAgent.loadToolbarSetting();
			FormAgent.MultiLanguage(DataCenter._uiSetting.MultiLanguage);
			FormAgent.AddFormToList();
            FormAgent.BuildFormTabs();           
		}

		public static void Close()
		{
			if ( formList != null )
			{
				if ( formList.Count > 0 )
				{
					foreach ( Form form in formList.Values )
					{
						if ( form != null && form.IsDisposed == false )
							form.Close();
					}

					formList.Clear();
				}

				formList = null;
			}


			if ( DataCenter._bStandAlone )
			{
				if ( _baseForm != null && _baseForm.IsDisposed == false )
				{
					_baseForm.Close();
					_baseForm = null;
				}
			}
		}

		public static void HideAll()
		{
			if ( formList !=null && formList.Values != null )
			{
				foreach (Form form in formList.Values)
				{
					if (form != null)
					{
						form.Visible = false;
						form.Hide();
					}
				}
			}

			if ( _floatToolbar.IsDisposed != true )
			{
				_floatToolbar.Show();
			}
		}

		public static void PopupWaferMapForm( Type type, string map_type )
		{
			frmWaferMap target = FormAgent.RetrieveForm( type ) as frmWaferMap;
			target.Show();
			target = null;
		}

		public static void PopupCIEForm( Type type )
		{
		}

		public static void SwitchForm( string tag )
		{
			if ( _baseForm.Visible == false )
			{
				_baseForm.Size = defaultSize;
				_baseForm.Show();
			}

			_floatToolbar.Hide();

			BaseForm.SwitchMenuTab(tag);
			BaseForm.SetAuthorityLevel(DataCenter._userManag.CurrentAuthority);
		}

		public static void ShowAlert( string title, string msg )
		{
			AlertCustom form = ( AlertCustom ) FormAgent.RetrieveForm( typeof( AlertCustom ) );
			form.Title = title;
			form.Message = msg;
			form.ShowAt( 0, 0 );
		}

		public static Form RetrieveForm(Type type)
		{
			if (formList.ContainsKey(type) == false)
			{
				Form temp = System.Activator.CreateInstance(type) as Form;
				formList.Add(type, temp);
				return temp;
			}
			return formList[type];
		}

		public static void AddFormToList()
		{
			Console.WriteLine("[FormAgent], AddFormToList()");

			formList.Clear();

			foreach ( string key in _toolbarItems.Keys )
			{
				Console.WriteLine("[FormAgent], AddFormToList(), key:" + key);

				foreach (FormItem item in _toolbarItems[key])
				{
					if (item.FormType == null)
					{
						continue;
					}

					Form temp = System.Activator.CreateInstance(item.FormType) as Form;

					formList.Add(item.FormType, temp);
				}
			}
		}
		
		public static string AddFormToList(Type type)
		{
			if ( formList.ContainsKey(type) == false )
			{
				Form temp = System.Activator.CreateInstance(type) as Form;
				Console.WriteLine("[FormAgent], AddFormToList(), Text:" + temp.Text);
				formList.Add(type, temp);
				return temp.Text;
			}
			return string.Empty;
		}

		public static void MultiLanguage(int language)
		{
			Console.WriteLine("[FormAgent], MultiLanguage()");

			switch (language)
			{ 
				case  (int)EMultiLanguage.ENG :
					MultiLanguage( _floatToolbar, "en-US");
					MultiLanguage(_baseForm, "en-US");

					if (formList.Count > 0)
					{
						foreach (Form form in formList.Values)
						{
							MultiLanguage(form, "en-US");
						}
					}
					break;
				//-------------------------------------------------------------------------------------
				case (int)EMultiLanguage.CHT:
					MultiLanguage(_floatToolbar, "zh-TW");
					MultiLanguage(_baseForm, "zh-TW");

					if (formList.Count > 0)
					{
						foreach (Form form in formList.Values)
						{
							MultiLanguage(form, "zh-TW");
						}
					}
					break;
                //-------------------------------------------------------------------------------------
                case (int)EMultiLanguage.JPN:
                    MultiLanguage(_floatToolbar, "ja");
                    MultiLanguage(_baseForm, "ja");

                    if (formList.Count > 0)
                    {
                        foreach (Form form in formList.Values)
                        {
                            MultiLanguage(form, "ja");
                        }
                    }
                    break;
				//-------------------------------------------------------------------------------------
				//case EMultiLanguage.CHS:
				//    MultiLanguage(_floatToolbar, "zh-CN");
				//    MultiLanguage(_baseForm, "zh-CN");

				//    if (formList.Count > 0)
				//    {
				//        foreach (Form form in formList.Values)
				//        {
				//            MultiLanguage(form, "zh-CN");
				//        }
				//    }
				//    break;
				//-------------------------------------------------------------------------------------
				default:
					MultiLanguage( _floatToolbar, "en-US");
					MultiLanguage(_baseForm, "en-US");

					if (formList.Count > 0)
					{
						foreach (Form form in formList.Values)
						{
							MultiLanguage(form, "en-US");
						}
					}
					break;
			}
			
			
		}

		public static void MultiLanguage(Form form, string Language)
        {
            if ( string.IsNullOrEmpty(Language) || form == null ) 
                return;

            System.Globalization.CultureInfo info;
            try
            {
                info = new System.Globalization.CultureInfo(Language);
            }
            catch (Exception ex)
            {
                throw ex;
            }

			Thread.CurrentThread.CurrentUICulture = info;		// cahnge to specified culture
            ComponentResourceManager manager = null;
            try
            {
                manager = new ComponentResourceManager(form.GetType());
                manager.ApplyResources(form, "$this");

                foreach (Control ctrl in form.Controls)
                {
					ChangeControlLanguage(ctrl, manager);
                }
            }
            finally
            {
                manager = null;
            }
        }

		private static void ChangeControlLanguage(Control ctrl, ComponentResourceManager manager)
		{
			if (ctrl == null || manager == null)
				return;

			if (ctrl is MenuStrip)
			{
				MenuStrip menu = ctrl as MenuStrip;
				manager.ApplyResources(ctrl, ctrl.Name);
				foreach (ToolStripItem item in menu.Items)
				{
					ApplyLanguage(item, manager);
				}
			}
			else if (ctrl is ToolStrip)
			{
				ToolStrip menu = ctrl as ToolStrip;
				manager.ApplyResources(ctrl, ctrl.Name);
				foreach (ToolStripItem item in menu.Items)
				{
					ChangeControlLanguage(item, manager);
				}
			}
			else if ( ctrl is DevComponents.DotNetBar.SuperTabControl)
			{
				DevComponents.DotNetBar.SuperTabControl menu = ctrl as DevComponents.DotNetBar.SuperTabControl;
				manager.ApplyResources(ctrl, ctrl.Name);
				foreach (DevComponents.DotNetBar.SuperTabItem item in menu.Tabs)
				{
					manager.ApplyResources(item, item.Name);
				}
			}
			else
			{
				manager.ApplyResources(ctrl, ctrl.Name);
				foreach (Control childCtrl in ctrl.Controls)
				{
					ChangeControlLanguage(childCtrl, manager);
				}
			}

		
		}

		private static void ChangeControlLanguage(ToolStripItem ctrl, ComponentResourceManager manager)
		{

			if (ctrl is ToolStripMenuItem)
			{
				manager.ApplyResources(ctrl, ctrl.Name);

				ToolStripMenuItem menu = ctrl as ToolStripMenuItem;
				foreach (ToolStripItem item in menu.DropDownItems)
				{
					ChangeControlLanguage(item, manager);
				}
			}
			else
			{
				manager.ApplyResources(ctrl, ctrl.Name);
			}
		}

        private static void ApplyLanguage(Control Ctrl, ComponentResourceManager Manager)
        {
            if (Ctrl is MenuStrip)
            {
                MenuStrip menu = Ctrl as MenuStrip;
                Manager.ApplyResources(Ctrl, Ctrl.Name);

                foreach (ToolStripItem item in menu.Items)
                {
                    ApplyLanguage(item, Manager);
                }
            }
            else if (Ctrl is ToolStrip)
            {
                ToolStrip menu = Ctrl as ToolStrip;
                Manager.ApplyResources(Ctrl, Ctrl.Name);

                foreach (ToolStripItem item in menu.Items)
                {
                    ApplyLanguage(item, Manager);
                }
            }
            else
            {
                Manager.ApplyResources(Ctrl, Ctrl.Name);
                foreach (Control item in Ctrl.Controls)
                {
                    ApplyLanguage(item, Manager);
                }
            }
        }

        private static void ApplyLanguage(ToolStripItem Ctrl, ComponentResourceManager Manager)
        {
            Manager.ApplyResources(Ctrl, Ctrl.Name);
            ToolStripMenuItem menu = Ctrl as ToolStripMenuItem;
            foreach (ToolStripItem item in menu.DropDownItems)
            {
                ApplyLanguage(item, Manager);
            }
        }

		#endregion		
	}

}

using System;
using System.Text;
using System.Drawing;
//using System.Windows.Forms;
using System.Collections.Generic;

using MPI.Windows.Forms;

namespace MPI.Tester.Gui
{
	/// <summary>
	/// UIBoxes class
	/// </summary>
	public class UIBoxes
	{
		// event
		public static event EventHandler BeforeBoxShow;
		public static event EventHandler AferBoxHide;

		#region >>> Generic Box <<<

		/// <summary>
		/// Exception box.
		/// </summary>
		public static void ExceptionBox( Exception ex )
		{
			if ( BeforeBoxShow != null )
				BeforeBoxShow( null, null );

			// log
			//Logs.LogServers.PutLog( MPI.MappingSorter.Logs.ELogMessage.ExceptionMsg, ex.Message, MPI.AuthorityControl.EAuthorityLevel.Maintenance );

			// show box
			Boxes.ExceptionBox( ex, Host.Constants.UI_TITLE );

			if ( AferBoxHide != null )
				AferBoxHide( null, null );
		}

		/// <summary>
		/// Warning box.
		/// </summary>
		public static void WarningBox( string message )
		{
			WarningBox( message, null );
		}

		/// <summary>
		/// Warning box.
		/// </summary>
		public static void WarningBox( string message, string[] helpMsgs )
		{
			if ( BeforeBoxShow != null )
				BeforeBoxShow( null, null );

			// log
			//Logs.LogServers.PutLog( MPI.MappingSorter.Logs.ELogMessage.WarningMsg, message, MPI.AuthorityControl.EAuthorityLevel.Maintenance );

			// show box
			StringBuilder sb = new StringBuilder();

			sb.Append( message );

			if ( helpMsgs != null )
			{
				sb.Append( "\n\n" );

				for ( int i = 0; i < helpMsgs.Length; i++ )
					sb.AppendFormat( "[ {0} ] {1}\n", i + 1, helpMsgs[i] );
			}

			Boxes.WarningBox( sb.ToString(), Host.Constants.UI_TITLE );

			if ( AferBoxHide != null )
				AferBoxHide( null, null );
		}

		/// <summary>
		/// Warning box.
		/// </summary>
		public static void WarningBox_I18N( EWarningMsg MsgID )
		{
			string msg = Host.ResourceAgent.StringTable.LookUp( Host.Constants.UI_MSG_HEADER + Convert.ToInt32( MsgID ).ToString() );

			WarningBox( msg );
		}

		/// <summary>
		/// Warning box.
		/// </summary>
		public static void WarningBox_I18N( EWarningMsg MsgID, params EHelpMsg[] helpMsgIDs )
		{
			string msg = Host.ResourceAgent.StringTable.LookUp( Host.Constants.UI_MSG_HEADER + Convert.ToInt32( MsgID ).ToString() );

			List<string> helpMsgs = new List<string>();

			if ( helpMsgIDs != null )
			{
				foreach ( EHelpMsg id in helpMsgIDs )
					helpMsgs.Add( Host.ResourceAgent.StringTable.LookUp( Host.Constants.UI_MSG_HEADER + Convert.ToInt32( id ).ToString() ) );
			}

			WarningBox( msg, helpMsgs.ToArray() );
		}

		/// <summary>
		/// Warning box.
		/// </summary>
		public static void WarningBox_I18N( EWarningMsg MsgID, string info )
		{
			StringBuilder sb = new StringBuilder();

			sb.Append( Host.ResourceAgent.StringTable.LookUp( Host.Constants.UI_MSG_HEADER + Convert.ToInt32( MsgID ).ToString() ) );

			sb.Append( "\n\n" );
			sb.Append( info );

			WarningBox( sb.ToString() );
		}

		/// <summary>
		/// Warning box.
		/// </summary>
		public static void WarningBox_I18N( EWarningMsg MsgID, Exception ex )
		{
			System.Text.StringBuilder sb = new System.Text.StringBuilder();

			sb.Append( Host.ResourceAgent.StringTable.LookUp( Host.Constants.UI_MSG_HEADER + Convert.ToInt32( MsgID ).ToString() ) );
			sb.Append( '\n' );
			sb.Append( "Message = " );
			sb.Append( ex.Message );

			WarningBox( sb.ToString() );
		}

		/// <summary>
		/// Message box.
		/// </summary>
		public static void MsgBox_I18N( EMessage MsgID )
		{
			string msg = Host.ResourceAgent.StringTable.LookUp( Host.Constants.UI_MSG_HEADER + Convert.ToInt32( MsgID ).ToString() );

			MsgBox( msg );
		}

		/// <summary>
		/// Message box.
		/// </summary>
		public static void MsgBox( string message )
		{
			if ( BeforeBoxShow != null )
				BeforeBoxShow( null, null );

			Boxes.MsgBox( message, Host.Constants.UI_TITLE );

			if ( AferBoxHide != null )
				AferBoxHide( null, null );
		}

		/// <summary>
		/// Question box.
		/// </summary>
		public static bool QuestionBox( string message )
		{
			if ( BeforeBoxShow != null )
				BeforeBoxShow( null, null );

			bool ret = Boxes.YesNoBox( message, Host.Constants.UI_TITLE );

			if ( AferBoxHide != null )
				AferBoxHide( null, null );

			return ret;
		}

		/// <summary>
		/// Question box.
		/// </summary>
		public static bool QuestionBox_I18N( EQuestionMsg MsgID )
		{
			string msg = Host.ResourceAgent.StringTable.LookUp( Host.Constants.UI_MSG_HEADER + Convert.ToInt32( MsgID ).ToString() );

			return QuestionBox( msg );
		}

		/// <summary>
		/// Input box.
		/// </summary>
		public static string InputBox_I18N( string frame, EInputMsg MsgID, string defaultValue )
		{
			string msg = Host.ResourceAgent.StringTable.LookUp( Host.Constants.UI_MSG_HEADER + Convert.ToInt32( MsgID ).ToString() );

			return InputBox( frame, msg, defaultValue );
		}

		/// <summary>
		/// Input box.
		/// </summary>
		public static string InputBox( string frame, string prompt, string defaultValue )
		{
			return InputBox( frame, prompt, defaultValue, ( char ) 0 );
		}

		/// <summary>
		/// Input box.
		/// </summary>
		public static string InputBox( string frame, string prompt, string defaultValue, char passwordChar )
		{
			if ( BeforeBoxShow != null )
				BeforeBoxShow( null, null );

			// show input box
			MPI.Windows.Forms.InputBox inputbox = new MPI.Windows.Forms.InputBox((Host.Constants.UI_TITLE, frame, prompt, defaultValue, passwordChar);

			inputbox.ShowDialog();

			if ( inputbox.DialogResult == System.Windows.Forms.DialogResult.Cancel )
			{
				inputbox.Dispose();
				return "";
			}

			string InputValue = inputbox.InputValue;

			inputbox.Dispose();

			if ( AferBoxHide != null )
				AferBoxHide( null, null );

			return InputValue;
		}

		/// <summary>
		/// Input combo box.
		/// </summary>
		public static object InputComboBox_I18N( string frame, EInputMsg MsgID, object[] list, object defaultValue )
		{
			string msg = Host.ResourceAgent.StringTable.LookUp( Host.Constants.UI_MSG_HEADER + Convert.ToInt32( MsgID ).ToString() );

			return InputComboBox( frame, msg, list, defaultValue );
		}

		/// <summary>
		/// Input combo box.
		/// </summary>
		public static object InputComboBox( string frame, string prompt, object[] list, object defaultValue )
		{
			if ( BeforeBoxShow != null )
				BeforeBoxShow( null, null );

			// show input box
			InputComboBox inputbox = new InputComboBox( Host.Constants.UI_TITLE, frame, prompt, list, defaultValue );

			inputbox.ShowDialog();

			if ( inputbox.DialogResult == System.Windows.Forms.DialogResult.Cancel )
			{
				inputbox.Dispose();
				return null;
			}

			object InputValue = inputbox.InputValue;

			inputbox.Dispose();

			if ( AferBoxHide != null )
				AferBoxHide( null, null );

			return InputValue;
		}

		/// <summary>
		/// Input list box.
		/// </summary>
		public static object InputListBox_I18N( string frame, EInputMsg MsgID, object[] list, object defaultValue )
		{
			string msg = Host.ResourceAgent.StringTable.LookUp( Host.Constants.UI_MSG_HEADER + Convert.ToInt32( MsgID ).ToString() );

			return InputListBox( frame, msg, list, defaultValue );
		}

		/// <summary>
		/// Input list box.
		/// </summary>
		public static object InputListBox( string frame, string prompt, object[] list, object defaultValue )
		{
			if ( BeforeBoxShow != null )
				BeforeBoxShow( null, null );

			// show input box
			InputListBox inputbox = new InputListBox( Host.Constants.UI_TITLE, frame, prompt, list, defaultValue );

			inputbox.ShowDialog();

			if ( inputbox.DialogResult == System.Windows.Forms.DialogResult.Cancel )
			{
				inputbox.Dispose();
				return null;
			}

			object InputValue = inputbox.InputValue;

			inputbox.Dispose();

			if ( AferBoxHide != null )
				AferBoxHide( null, null );

			return InputValue;
		}

		/// <summary>
		/// Open file confirm box.
		/// </summary>
		public static bool OpenFileConfirmBox( string filename )
		{
			StringBuilder sb = new StringBuilder();

			sb.Append( Host.ResourceAgent.StringTable.LookUp( Host.Constants.UI_MSG_HEADER + Convert.ToInt32( EQuestionMsg.DoYouWantToOpenFile ).ToString() ) );

			sb.Append( " '" );
			sb.Append( filename );
			sb.Append( "' ?" );

			return UIBoxes.QuestionBox( sb.ToString() );
		}

		/// <summary>
		/// Save file confirm box.
		/// </summary>
		public static bool SaveFileConfirmBox( string filename )
		{
			StringBuilder sb = new StringBuilder();

			sb.Append( Host.ResourceAgent.StringTable.LookUp( Host.Constants.UI_MSG_HEADER + Convert.ToInt32( EQuestionMsg.DoYouWantToSaveFile ).ToString() ) );

			sb.Append( " '" );
			sb.Append( filename );
			sb.Append( "' ?" );

			return UIBoxes.QuestionBox( sb.ToString() );
		}

		/// <summary>
		/// Delete file confirm box.
		/// </summary>
		public static bool DeleteFileConfirmBox( string filename )
		{
			StringBuilder sb = new StringBuilder();

			sb.Append( Host.ResourceAgent.StringTable.LookUp( Host.Constants.UI_MSG_HEADER + Convert.ToInt32( EQuestionMsg.DoYouWantToDeleteFile ).ToString() ) );

			sb.Append( " '" );
			sb.Append( filename );
			sb.Append( "' ?" );

			return UIBoxes.QuestionBox( sb.ToString() );
		}

		/// <summary>
		/// Show hint box.
		/// </summary>
		public static HintBox ShowHintBox_I18N( EMessage MsgID )
		{
			return ShowHintBox( Host.ResourceAgent.StringTable.LookUp( Host.Constants.UI_MSG_HEADER + Convert.ToInt32( MsgID ).ToString() ) );
		}

		/// <summary>
		/// Show hint box.
		/// </summary>
		public static HintBox ShowHintBox( string prompt )
		{
			if ( BeforeBoxShow != null )
				BeforeBoxShow( null, null );

			HintBox box = new HintBox();

			box.Show( prompt );

			box.Update();

			for ( int i = 0; i < 10; i++ )
			{
				System.Windows.Forms.Application.DoEvents();
				System.Threading.Thread.Sleep( 10 );
			}

			return box;
		}

		/// <summary>
		/// Hide hint box.
		/// </summary>
		public static void HideHintBox( HintBox box )
		{
			box.Hide();

			box.Dispose();

			if ( AferBoxHide != null )
				AferBoxHide( null, null );
		}

		/// <summary>
		/// Folder browser box.
		/// </summary>
		public static string FolderBrowserBox( Environment.SpecialFolder rootPath, string defaultPath, bool newFolderButton )
		{
			using ( FolderBrowserDialog dialog = new FolderBrowserDialog() )
			{
				dialog.RootFolder = rootPath;
				dialog.ShowNewFolderButton = newFolderButton;

				if ( System.IO.Directory.Exists( defaultPath ) )
					dialog.SelectedPath = defaultPath;

				if ( dialog.ShowDialog() == DialogResult.OK )
					return dialog.SelectedPath;
				else
					return null;
			}
		}

		/// <summary>
		/// Open file box.
		/// </summary>
		public static string OpenFileBoxEx( string initialPath, string defaultExt, string filter )
		{
			if ( BeforeBoxShow != null )
				BeforeBoxShow( null, null );

			// show input box
			OpenFileBox box = new OpenFileBox( Host.Constants.UI_TITLE,
												Host.ResourceAgent.StringTable.LookUp( Host.Constants.UI_MSG_HEADER + Convert.ToInt32( EMessage.OpenFile ).ToString() ),
												Host.ResourceAgent.StringTable.LookUp( Host.Constants.UI_MSG_HEADER + Convert.ToInt32( EMessage.FileName ).ToString() ),
												initialPath, defaultExt, filter );

			box.ShowDialog();

			if ( box.DialogResult == System.Windows.Forms.DialogResult.Cancel )
			{
				box.Dispose();
				box = null;
				return null;
			}

			string InputValue = box.InputValue;

			box.Dispose();
			box = null;

			if ( AferBoxHide != null )
				AferBoxHide( null, null );

			return InputValue;
		}

		/// <summary>
		/// Open file box.
		/// </summary>
		public static string OpenFileBox( string initialPath, string defaultExt, string filter )
		{
			OpenFileDialog dialog = new OpenFileDialog();

			dialog.RestoreDirectory = true;
			dialog.InitialDirectory = initialPath;
			dialog.DefaultExt = defaultExt;
			dialog.Filter = filter;
			dialog.FilterIndex = 0;

			string filename = null;

			if ( dialog.ShowDialog() == DialogResult.OK )
				filename = dialog.FileName;

			dialog.Dispose();
			dialog = null;

			return filename;
		}

		/// <summary>
		/// Save file box.
		/// </summary>
		public static string SaveFileBox( string initialPath, string defaultExt, string filter )
		{
			SaveFileDialog dialog = new SaveFileDialog();

			dialog.RestoreDirectory = true;
			dialog.InitialDirectory = initialPath;
			dialog.DefaultExt = defaultExt;
			dialog.Filter = filter;
			dialog.FilterIndex = 0;

			string filename = null;

			if ( dialog.ShowDialog() == DialogResult.OK )
				filename = dialog.FileName;

			dialog.Dispose();
			dialog = null;

			return filename;
		}

		#endregion
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using MPI.UCF;
using MPI.UCF.UIBox;

namespace MPI.Tester.Gui
{
	static public class TopMessageBox
	{
		//public delegate void ShowMessageHandler(int msgCode, string msg, string title);
		//  public static ShowMessageHandler onShowMessage;
		public static frmAlarmMessage _frmAlertCustom = new frmAlarmMessage();

		static public DialogResult Show(string message)
		{
			return Show(message, string.Empty, MessageBoxButtons.OK);
		}

		static public DialogResult Show(string message, string title)
		{
			return Show(message, title, MessageBoxButtons.OK);
		}


		static public DialogResult Show(string message, string title, MessageBoxButtons buttons)
		{
			// Create a host form that is a TopMost window which will be the parent of the MessageBox.
			Form topForm = new Form();
			// We do not want anyone to see this window so position it off the visible screen and make it as small as possible
			topForm.Size = new System.Drawing.Size(100, 100);
			topForm.StartPosition = FormStartPosition.Manual;
			System.Drawing.Rectangle rect = SystemInformation.VirtualScreen;
			topForm.Location = new System.Drawing.Point(rect.Bottom + 10, rect.Right + 10);
			topForm.Show();
			// Make this form the active form and make it TopMost
			topForm.Focus();
			topForm.BringToFront();
			topForm.TopMost = true;
			// Finally show the MessageBox with the form just created as its owner
			DialogResult result = MessageBox.Show(topForm, message, title, buttons);
			topForm.Dispose(); // clean it up all the way

			return result;
		}

		public static DialogResult ShowMessage(int msgCode, string msg, EMessageType type)
		{
			string key = "MSG_" + msgCode.ToString("000");

			string msg2 = Host.resourceManagerMessage.GetString(key);

			_frmAlertCustom.MessageType = type;

			_frmAlertCustom.Message = msg2;

			_frmAlertCustom.ErrorHandler = string.Empty;

			_frmAlertCustom.ShowDialog();

			DialogResult result = _frmAlertCustom.Result;

			return result;
		}

		public static void PopWarning(string msg)
		{
			InfoBox.PopupWarning(msg);
		}

		public static void PopMessage(int msgCode)
		{
			string key = "MSG_" + msgCode.ToString("000");

			string msg = Host.resourceManagerMessage.GetString(key);

			InfoBox.Popup(msg);
		}

		public static void ShowHistory()
		{
			AlarmBox.ShowHistory();
		}

		public static DialogResult ShowMessage(int msgCode, string msg, string caption)
		{
			DialogResult result;

			string key = "MSG_" + msgCode.ToString("000");

			string msg2 = Host.resourceManagerMessage.GetString(key);

			if (msg2 == null)
			{
				msg2 = msg;
			}

			bool answer = InfoBox.PopupQuestion(msg2);

			if (answer == true)
			{
				result = DialogResult.OK;
			}
			else
			{
				result = DialogResult.No;
			}

			return result;
		}

		public static DialogResult ShowMessage(int msgCode, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton)
		{
			return ShowMessage(msgCode, text, caption);
		}

	}
}

using System;
using MPI.AuthorityControl;

using MPI.Tester.Data;

namespace MPI.Tester.Gui
{
	/// <summary>
	/// AuthorityCenter class.
	/// </summary>
	public class AuthorityCenter
	{
		// user management center
		public static UserManagementCenter UserCenter;

		/// <summary>
		/// Initial center.
		/// </summary>
		public static void Open()
		{
			// initial user authority center
			UserCenter = new UserManagementCenter();

			try
			{
				LoadUserTable();
			}
			catch
			{
				UserCenter.CreateNewUserTable();

				SaveUserTable();
			}

			UserCenter.Login( UserCenter.UserTable[0] );
		}

		public static void LoadUserTable()
		{
			string filename = Constants.Paths.DATA_FILE + Constants.Files.SYSTEM_USER_TABLE;

			UserCenter.Load( filename, Constants.UI_TITLE, Constants.UI_AUTHOR );
		}

		public static void SaveUserTable()
		{
			string filename = Constants.Paths.DATA_FILE + Constants.Files.SYSTEM_USER_TABLE;

			UserCenter.Save( filename, Constants.UI_TITLE, Constants.UI_AUTHOR );
		}
	}
}

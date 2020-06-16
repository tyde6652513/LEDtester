    using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace MPI.Tester.Data
{
	public class UserManageCtrl
	{
		private object _lockObj;

		private static UserManageData _data;

		private string _pathAndFullFileName;

		private List<UserRecord> _userRecordList;

		private string _currentUserName;

		private EAuthority _currentAuthority;

		public UserManageCtrl(string path, string fileNameWithExt)
		{
			this._lockObj = new object();

			this._pathAndFullFileName = Path.Combine(path, fileNameWithExt);

			this._userRecordList = new List<UserRecord>(10);

			_data = new UserManageData();
		}

		public string CurrentUserName
		{
			get { return this._currentUserName; }
		}

		public EAuthority CurrentAuthority
		{
			get { return this._currentAuthority; } 
		}

		private void CreateNewUserTable()
		{
			this._userRecordList.Clear();

			this._userRecordList.Add(new UserRecord("simulator", "calibration", EAuthority.Engineer));

			this._userRecordList.Add(new UserRecord("Super", "ilovet200", EAuthority.Super));

			this._userRecordList.Add(new UserRecord("Admin", "89647050", EAuthority.Admin));

			this._userRecordList.Add(new UserRecord("gpt", "gpt200", EAuthority.Engineer));

			this._userRecordList.Add(new UserRecord("op", "", EAuthority.Operator));
			
			_data.UserRecordArray = this._userRecordList.ToArray();

			SaveFile();
		}

		public void OpenFile()
		{
			BinaryFormatter bformatter = new BinaryFormatter();

			_data = new UserManageData();

			//Read the data from a file and desiralize it
			try
			{
				using (Stream openStream = File.Open(this._pathAndFullFileName, FileMode.Open))
				{
					_data = (UserManageData)bformatter.Deserialize(openStream);

					this._userRecordList.Clear();

					this._userRecordList.AddRange(_data.UserRecordArray);

					openStream.Close();
				}
			}
			catch
			{
				this.CreateNewUserTable();
			}
		}

		public void SaveFile()
		{

			BinaryFormatter bformatter = new BinaryFormatter();
			// Opens a file and serializes the object into it.
			using (Stream stream = File.Open(this._pathAndFullFileName, FileMode.Create))
			{
				bformatter.Serialize(stream, _data);

				stream.Close();
			}
		}

		public bool Login(string name, string passWord)
		{
			if (_data.CheckUserPassWord(name, passWord, out this._currentAuthority))
			{
				this._currentUserName = name;
				return true;
			}
			else
			{ 
				return false;
			}
		}

		public void AddUser(string name, string passWord, EAuthority level)
		{
			UserRecord record = new UserRecord(name, passWord, level);
			this._userRecordList.Add(record);
			_data.UserRecordArray = this._userRecordList.ToArray();
		}

		public void Remove()
		{
			if (this._userRecordList.Count == 0)
				return;

			this._userRecordList.Clear();

			_data.UserRecordArray = this._userRecordList.ToArray();
		}

		public void Remove(int index)
		{
			if (index < 0 || this._userRecordList.Count == 0)
				return;

			this._userRecordList.RemoveAt(index);

			_data.UserRecordArray = this._userRecordList.ToArray();
		}

		public void Modify(int index, string name, string passWord, EAuthority level)
		{
			if (index < 0 || this._userRecordList.Count == 0)
				return;

			UserRecord item = this._userRecordList[index];
			item.Name = name;
			item.PassWord = passWord;
			item.AuthorityLevel = level;

			_data.UserRecordArray = this._userRecordList.ToArray();
		}

		public void ChangeToOperatorAuthority()
		{
			bool isExistOP = false;

			foreach (var item in this._userRecordList)
			{
				if (item.Name == "op")
				{
					isExistOP = true;
				}
			}

			if (!isExistOP)
			{
				this._userRecordList.Add(new UserRecord("op", "", EAuthority.Operator));
			}

			this.Login("op", "");
		}

		public UserRecord GetUserRecord(string name)
		{
			UserRecord rtnData = new UserRecord();

			foreach (UserRecord item in this._userRecordList)
			{ 
				if(item.Name == name)
				{
					rtnData.Name = item.Name;
					rtnData.PassWord = item.PassWord;
					rtnData.AuthorityLevel = item.AuthorityLevel;

					return rtnData;
				}
			}

			return null;
		}

		public List<UserRecord> GetUserRecordList()
		{
			List<UserRecord> rtnData = new List<UserRecord>();

			foreach(UserRecord item in this._userRecordList)
			{
				UserRecord userRecord = new UserRecord();
				userRecord.Name = item.Name;
				userRecord.PassWord = item.PassWord;
				userRecord.AuthorityLevel = item.AuthorityLevel;

				rtnData.Add(userRecord);
			}

			return rtnData;
		}
	}
}

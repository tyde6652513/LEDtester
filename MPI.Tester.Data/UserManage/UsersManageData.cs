using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;


namespace MPI.Tester.Data
{
	public enum EAuthority : int 
	{
		Operator		= 0,
		QC				= 10,
		Engineer		= 20,   // 2 mk
		Admin			= 30,     
		Super			= 40,
        EOP             = 50,
	}
	
	[Serializable]
	public class UserRecord : ICloneable
	{
		public string Name;
		public string PassWord;
		public EAuthority AuthorityLevel;

		public UserRecord()
		{ }

		public UserRecord(string name, string passWord, EAuthority level)
		{
			Name = name;
			PassWord = passWord;
			AuthorityLevel = level;
		}

		public object Clone()
		{
			return this.MemberwiseClone();
		}
	}

	[Serializable]
	public class UserManageData : ICloneable
	{
		private object _lockObj;

		private UserRecord[] _userRecordArray;

		public UserManageData()
		{
			this._lockObj = new object();

			this._userRecordArray = null;
		}

		#region >>> Public Property <<<

		public UserRecord[] UserRecordArray
		{
			get { return this._userRecordArray; }
			set { lock (this._lockObj) { this._userRecordArray = value; } }
		}

		#endregion

		#region >>> Private Method <<<

		//private DateTime ReadPWToken()
		//{
		//    if (File.Exists(Path.Combine(Environment.SystemDirectory, "token")))
		//    {
		//        using (FileStream myFileSream = File.Open(Path.Combine(Environment.SystemDirectory, "token"), FileMode.Open,FileAccess.Read))
		//        {
		//            BinaryReader myReader = new BinaryReader(myFileSream);
		//            myReader.BaseStream.Seek(0, SeekOrigin.Begin);
		//            long inData = myReader.ReadInt64();		// long == int64
		//            myReader.Close();
		//            myFileSream.Close();
		//            return DateTime.FromBinary(inData);
		//        }
		//    }
		//    else
		//    {	// First time, create the token file
		//        CreateNewPWToken();
		//        return new DateTime(DateTime.Now.Day);
		//    }
		//}

		//private void CreateNewPWToken()
		//{
		//    using (FileStream myFileSream = File.Open(Path.Combine(Environment.SystemDirectory, "token"), FileMode.Create, FileAccess.ReadWrite))
		//    {
		//        BinaryWriter myWriter = new BinaryWriter(myFileSream); 
		//        myWriter.Write( DateTime.Now.ToBinary()); 
		//        myWriter.Close();
		//        myFileSream.Close();
		//    }
		//}

		//private bool CheckSpecialUserByToken(string passWord)
		//{
		//    DateTime recordPWToken = ReadPWToken();
		//    DateTime now = DateTime.Now;

		//    if (DateTime.Now.Year > recordPWToken.Year ||
		//         DateTime.Now.Month > recordPWToken.Month)
		//    {
		//        CreateNewPWToken();
		//        recordPWToken = ReadPWToken();
		//    }

		//    int yy = Convert.ToInt32(recordPWToken.Year);
		//    int mm = Convert.ToInt32(recordPWToken.Month);

		//    Int64  data = (Int64) (Math.Exp((double)mm) * ((double)yy) * Math.PI);

		//    if (passWord == data.ToString("x"))
		//    {
		//        return true;
		//    }
		//    else
		//    {
		//        return false;
		//    }
		//}

		#endregion

		#region >>> Public Method <<<

		public bool CheckUserPassWord(string name, string passWord, out EAuthority level)
		{
			bool isMatch = false;
			level = EAuthority.Operator;

			if (this._userRecordArray == null)		
				return false;

			foreach (UserRecord record in this._userRecordArray)
			{
				if (record.Name == name && record.PassWord == passWord)
				{
					level = record.AuthorityLevel;
					isMatch = true;
					break;
				}
			}


			return isMatch;
		}

		public object Clone()
        {
            return this.MemberwiseClone();
		}

		#endregion
	}	
}

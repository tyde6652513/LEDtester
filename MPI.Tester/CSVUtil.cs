using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace MPI.Tester
{
	public class CSVUtil
	{
		private static string _pathAndFileName = string.Empty;
		private static bool _isAppend = false;
		private static List<String[]> _csvStrList = null;

		public CSVUtil()
		{
		}

		#region >>> Static Private Methods <<<

        private static bool WriteStrArrayToCSV(char splitch = ',')
		{
			try
			{
				if (Directory.Exists(Path.GetDirectoryName(_pathAndFileName)) == false)
				{
					Directory.CreateDirectory(Path.GetDirectoryName(_pathAndFileName));
				}

				using (StreamWriter streamWriter = new StreamWriter( _pathAndFileName, _isAppend, Encoding.Default) )
				{
                    if (_csvStrList != null)
                    {
                        foreach (string[] oneRowStr in _csvStrList)
                        {
                            if (oneRowStr != null)
                            {
                                streamWriter.WriteLine(String.Join(splitch.ToString(), oneRowStr));
                            }
                            else
                            {
                                streamWriter.WriteLine("");
                            }

                        }
                    }
                    streamWriter.Flush();
                    streamWriter.Close();
				}
			}
			catch (IOException)
			{
				// Console.WriteLine("The file could not be opened because it was locked by another process.");
				return false;
			}
			catch (Exception)
			{
				// Console.WriteLine(ex.ToString());
				return false;
			}

			return true;
		}

		private static bool ReadStrArrayFromCSV(char splitchar = ',')
		{
			if ( File.Exists( _pathAndFileName ) == false )
				return false;

			try
			{
				_csvStrList = new List<String[]>();
				using (StreamReader fileReader = new StreamReader(_pathAndFileName, Encoding.Default))
				{
					while ( !fileReader.EndOfStream )
					{
						string strLine = fileReader.ReadLine();
						if ( strLine == null  || strLine.Length < 0)
							continue;

                        _csvStrList.Add(strLine.Split(splitchar));
					}
					fileReader.Close();
				}
			}
			catch (IOException)
			{
				// Console.WriteLine("The file could not be opened because it was locked by another process.");
				return false;
			}
			catch (Exception)
			{
				// Console.WriteLine(ex.ToString());
				return false;
			}

			return true;
		}

		private static int SearchKeyTitleIndex(string keyTitleString , bool isMatchFullKey)
		{
			int index = -1;

			if (keyTitleString == null || keyTitleString.Trim() == "")
			{
				index = -1;
			}
			else 
			{
				string trimKeyString = keyTitleString.Replace(" ","");
				string searchLineString = string.Empty;

				if ( trimKeyString.Length != 0 )
				for (index = 0; index < _csvStrList.Count; index++)
				{
					searchLineString =String.Join(",", _csvStrList[index]).Replace(" ","");
					if ( searchLineString.Length < trimKeyString.Length )
						continue;

					if (isMatchFullKey)
					{
						if (searchLineString == trimKeyString)
							break;
					}
					else
					{
						if (searchLineString.Substring(0, trimKeyString.Length) == trimKeyString)
							break;
					}
				}

				if  ( index == _csvStrList.Count )
				{
					index = -1;
				}
			}

			return index;
		}

		//private static void RemoveData(int startIndex, int endIndex, bool isIncludeTitle)
		private static void RemoveData(int endIndex, bool isRemoveTitle)
		{
			if (endIndex < 0)
			{
				_csvStrList.Clear();
				return;
			}

			if (isRemoveTitle)
			{
				_csvStrList.RemoveRange(0, endIndex + 1);
			}
			else
			{
				if (endIndex != 0)
				{
					_csvStrList.RemoveRange(0, endIndex);
				}
			}				
		}

		#endregion

		#region >>> Static Public Methods <<<

		public static bool WriteCSV(string pathFileName, List<String[]> ls,char splitch = ',')
		{
            return WriteCSV(pathFileName, false, ls, splitch);
		}

		public static bool WriteCSV(string pathAndFileName, bool isAppend, List<String[]> ls,char splitch = ',')
		{
			_pathAndFileName = pathAndFileName;
			_isAppend = isAppend;
			_csvStrList = ls;

            return WriteStrArrayToCSV(splitch);
		}

		public static bool WriteToCSV(string pathFileName, bool isAppend, string[][] csvStrArray)
		{
			_pathAndFileName = pathFileName;
			_isAppend = isAppend;
			_csvStrList = new List<string[]>(csvStrArray);

			return WriteStrArrayToCSV();
		}

		public static bool WriteToCSV(string pathFileName, bool isAppend, double[][] dataArray)
		{
			_pathAndFileName = pathFileName;
			_isAppend = isAppend;
			StringBuilder sb = new StringBuilder();
			_csvStrList = new List<string[]>(dataArray.Length);

			for (int row = 0; row < dataArray.Length; row++)
			{
				sb.Clear();
                if (dataArray[row] == null || dataArray[row].Length == 0)
                {
                    _csvStrList.Add(new string[]{""});
                    continue;
                }

				for (int col = 0; col < dataArray[row].Length; col++)
				{
					sb.Append(dataArray[row][col].ToString());
					if ((col + 1) != dataArray[row].Length)
					{
						sb.Append(',');
					}
				}
				_csvStrList.Add(sb.ToString().Split(','));
			}
			
			return WriteStrArrayToCSV();
		}

		public static bool WriteToCSV(string pathFileName, bool isAppend, double[][] dataArray, bool isTransferMatrix)
		{
			if ( isTransferMatrix == false)
			{
				return WriteToCSV(pathFileName, isAppend, dataArray);
			}
			
			_pathAndFileName = pathFileName;
			_isAppend = isAppend;
			StringBuilder sb = new StringBuilder();
			_csvStrList = new List<string[]>(dataArray.Length);
            int maxReversedRow = 0;                                      // origianl col count of dataArray
            int maxReversedCol = dataArray.Length;              // origianl row count of dataArray

            for (int i = 0; i < dataArray.Length; i++)
            {
                if (dataArray[i] != null && dataArray[i].Length > maxReversedRow)
                {
                    maxReversedRow = dataArray[i].Length;
                }
            }

            for (int row = 0; row < maxReversedRow; row++)
			{
				sb.Clear();

                for (int col = 0; col < maxReversedCol; col++)
				{          
                    if (dataArray[col] == null || dataArray.Length == 0)
                    {
                        sb.Append("");
                    }
                    else
                    {
                        sb.Append(dataArray[col][row].ToString());
                    }
                    if ((col + 1) != maxReversedCol)
                    {
                        sb.Append(',');
                    }
                    
				}
				_csvStrList.Add(sb.ToString().Split(','));
			}
			return WriteStrArrayToCSV();

		}

		public static List<String[]> ReadCSV(string pathFileName,char ch = ',')
		{
			_pathAndFileName = pathFileName;
            if (ReadStrArrayFromCSV(ch))
			{
				return _csvStrList;
			}
			else
			{
				return null;
			}
			
		}

        public static List<String[]> ReadCSV(string pathFileName, string keyTitleString, bool isMatchFullKey, bool isRemoveTitle, char ch = ',')
		{
			_pathAndFileName = pathFileName;

            if (ReadStrArrayFromCSV(ch) == false)
			{
				return null;
			}

			int index = SearchKeyTitleIndex(keyTitleString, isMatchFullKey);
			RemoveData(index, isRemoveTitle);

			if ( _csvStrList.Count == 0 )
			{
				return null;
			}
			else
			{
				return _csvStrList;
			}

		}

		public static bool ReadFromCSV(string pathFileName, out string[][] csvResultStr)
		{
			_pathAndFileName = pathFileName;

			if ( ReadStrArrayFromCSV() == false)
			{
				csvResultStr = null;
				return false;
			}
			else
			{
				if (_csvStrList.Count == 0)
				{
					csvResultStr = null;
				}
				else
				{
					csvResultStr = _csvStrList.ToArray();
				}
				return true;
			}			
		}

		public static bool ReadFromCSV(string pathFileName, out string[][] csvResultStr, string keyTitleString, bool isMatchFullKey, bool isRemoveTitle)
		{
			_pathAndFileName = pathFileName;

			if (ReadStrArrayFromCSV() == false)
			{
				csvResultStr = null;
				return false;
			}

			int index = SearchKeyTitleIndex(keyTitleString, isMatchFullKey);
			RemoveData(index, isRemoveTitle);

			if (_csvStrList.Count == 0)
			{
				csvResultStr = null;
			}
			else
			{
				csvResultStr = _csvStrList.ToArray();
			}
			return true;
		}

		public static bool ReadFromCSV(string pathFileName, out double[][] dataResult)
		{
			return ReadFromCSV(pathFileName, 0, out dataResult);
		}

		public static bool ReadFromCSV(string pathFileName, uint fromIndex,out double[][] dataResult)
		{
			_pathAndFileName = pathFileName;

			if ( ReadStrArrayFromCSV() == false)
			{
				dataResult = null;
				return false;
			}

			if (_csvStrList.Count == 0)
			{
				dataResult = null;
				return true;
			}

			if (fromIndex >= _csvStrList.Count)
			{
				dataResult = null;
				return true;
			}

			dataResult = new double[ _csvStrList.Count - fromIndex][];
			int moveIndex = 0;

			for (int row = 0; row < dataResult.Length; row++)
			{
				moveIndex = row + (int)fromIndex;
				dataResult[row] = new double[_csvStrList[moveIndex].Length];

				for (int col = 0; col < dataResult[row].Length; col++)
				{
					if (_csvStrList[moveIndex][col].Trim() == string.Empty)
					{
						dataResult[row][col] = 0.0d;
					}
					else if (double.TryParse(_csvStrList[moveIndex][col], out dataResult[row][col]) == false)
					{
						dataResult = null;
						return false;
					}
				}
			}

			return true;
		}

		public static bool ReadFromCSV(string pathFileName, out double[][] dataResult, string keyTitleString, bool isMatchFullKey, bool isRemoveTitle)
		{
			_pathAndFileName = pathFileName;

			if ( ReadStrArrayFromCSV() == false)
			{
				dataResult = null;
				return false;
			}

			int index = SearchKeyTitleIndex(keyTitleString, isMatchFullKey);
			RemoveData(index, isRemoveTitle);
			//--------------------------------------------------------------------------------------------------
			if (_csvStrList.Count == 0)
			{
				dataResult = null;
				return true;
			}

			dataResult = new double[ _csvStrList.Count][];
			for (int row = 0; row < _csvStrList.Count; row++)
			{
				dataResult[row] = new double[_csvStrList[row].Length];

				for (int col = 0; col < dataResult[row].Length; col++)
				{
					if (_csvStrList[row][col].Trim() == string.Empty)
					{
						dataResult[row][col] = 0.0d;
					}
					else if (double.TryParse(_csvStrList[row][col], out dataResult[row][col]) == false)
					{
						dataResult = null;
						return false;
					}
				}
			}

			return true;
		}

		//public static bool ConverterUesrReport(string formatFilePath, string outputFilePath, char keyStart, char keyEnd, Dictionary<string, string> data, string emptyString)
		//{
		//    if (!File.Exists(formatFilePath))
		//        return false;

		//    string text = File.ReadAllText(formatFilePath, Encoding.GetEncoding("Big5"));

		//    while (true)
		//    {
		//        int start = text.IndexOf(keyStart);
		//        int End = text.IndexOf(keyEnd);

		//        if (start < 0 || End < 0)
		//            break;

		//        string value;
		//        string identifier = text.Substring(start, End - start + 1);
		//        string key = identifier;
		//        key = key.Remove(key.IndexOf((char)0x01), 1);
		//        key = key.Remove(key.IndexOf((char)0x02), 1);

		//        if (data.TryGetValue(key, out value))
		//        {
		//            text = text.Replace(identifier, value);
		//        }
		//        else
		//        {
		//            text = text.Replace(identifier, emptyString);
		//        }
		//    }


		//    try
		//    {
		//        File.WriteAllText(outputFilePath, text, Encoding.GetEncoding("Big5"));
		//    }
		//    catch (IOException e)
		//    {
		//        // Gilbert Error
		//        Console.WriteLine("The file could not be opened because it was locked by another process. {0}", e.ToString());
		//        return false;
		//    }
		//    catch (Exception ex)
		//    {
		//        // Gilbert Error
		//        Console.WriteLine(ex.ToString());
		//        return false;
		//    }

		//    return true;
		//}

		public static bool ConverterUesrReport(string formatFilePath, string outputFilePath, char keyStart, char keyEnd, Dictionary<string, string> data, string LOPSaveItem, int exceptionCount = 0)
        {
			if (!File.Exists(formatFilePath))
				return false;

			try
			{
				string text = File.ReadAllText(formatFilePath, Encoding.GetEncoding("Big5"));

				///////////////////////////////////////////////////////////
				//Get File Name
				///////////////////////////////////////////////////////////
				if (text.IndexOf(((char)0x04).ToString()) == 0)
				{
					string value;

					int start = text.IndexOf(((char)0x04).ToString());

					int end = text.IndexOf(((char)0x0d).ToString() + ((char)0x0a).ToString());//CR LF

					string identifier = text.Substring(start, end - start + 1);

					string key = identifier;

					key = key.Replace(((char)0x04).ToString(), "");

					key = key.Replace(((char)0x0d).ToString(), "");

					if (data.TryGetValue(key, out value))
					{
						outputFilePath = Path.Combine(Path.GetDirectoryName(outputFilePath), value + ".txt");

						text = text.Remove(start, end + 2);
					}
				}

				///////////////////////////////////////////////////////////
				//Find the LOP and mW Item
				///////////////////////////////////////////////////////////
				while (true)
				{
					int start = text.IndexOf((char)0x06);

					int end = text.IndexOf((char)0x07);

					if (start < 0 || end < 0)
					{
						break;
					}

					string identifier = text.Substring(start, end - start + 1);

					string key = identifier;

					key = key.Replace(((char)0x06).ToString(), "");

					key = key.Replace(((char)0x07).ToString(), "");

					if (LOPSaveItem == "mcd"  && key == "LOP"  ||
						LOPSaveItem == "watt" && key == "WATT" ||
						LOPSaveItem == "lm"   && key == "LM")
					{
						text = text.Remove(start, end - start + 1);
					}
					else
					{
						int endLine = text.IndexOf(((char)0x0d).ToString() + ((char)0x0a).ToString(), start);//CR LF

						text = text.Remove(start, endLine - start + 3);
					}
				}

				///////////////////////////////////////////////////////////
				//Replace key Value
				///////////////////////////////////////////////////////////
				while (true)
				{
					int start = text.IndexOf(keyStart);

					int end = text.IndexOf(keyEnd);

					if (start < 0 || end < 0)
					{
						break;
					}

					string value = string.Empty;

					string identifier = text.Substring(start, end - start + 1);

					string item = identifier;

					item = item.Replace(keyStart.ToString(), "");

					item = item.Replace(keyEnd.ToString(), "");

					string[] keys = item.Split(',');

					string key = keys[0];

					string formate = string.Empty;

					int spaceNum = 0;

					if (keys.Length > 1)
					{
						formate = keys[1];
					}

					if (keys.Length > 2)
					{
						spaceNum = int.Parse(keys[2]);
					}

					if (data.TryGetValue(key, out value))
					{
						double number = 0.0d;

						if(double.TryParse(value, out number))
						{
							value = number.ToString(formate);
						}
					}
					else
					{
						value = (0.0d).ToString(formate);
					}

					if (spaceNum > 0)
					{
						text = text.Replace(identifier, value.PadLeft(spaceNum));
					}
					else
					{
						text = text.Replace(identifier, value);
					}









					//if (data.TryGetValue(key, out value))
					//{
					//    string str = double

					//    text = text.Replace(identifier, str);//.PadLeft(spaceNum));
					//}
					//else
					//{
					//    string spaceEmpty = "0.00";

					//    if (spaceNum > 0)
					//    {
					//        text = text.Replace(identifier, spaceEmpty.PadLeft(spaceNum));

					//        spaceNum = 0;
					//    }
					//    else
					//    {
					//        text = text.Replace(identifier, spaceEmpty);
					//    }
					//}
				}

				///////////////////////////////////////////////////////////
				//Append
				///////////////////////////////////////////////////////////
				if (File.Exists(outputFilePath))
				{
					if (text.IndexOf(((char)0x03).ToString()) < 0)
					{
						//nonAppend Mode
						MPIFile.DeleteFile(outputFilePath);
					}
					else
					{
						while (true)
						{
							//Remove Tiatle
							int start = text.IndexOf(((char)0x03).ToString());

							int end = text.IndexOf(((char)0x0d).ToString() + ((char)0x0a).ToString());//CR LF

							if (start < 0 || end < 0)
							{
								break;
							}
							else
							{
								text = text.Remove(start, end);
							}
						}

						//Append
						text = File.ReadAllText(outputFilePath, Encoding.GetEncoding("Big5")) + text;
					}
				}
					
				text = text.Replace(((char)0x03).ToString(), "");

				File.WriteAllText(outputFilePath, text, Encoding.GetEncoding("Big5"));

				return true;
			}
			catch (IOException e)
			{
				if (exceptionCount > 100)
				{
					Console.WriteLine("The file could not be opened because it was locked by another process. {0}", e.ToString());

					return false;
				}

				int count = exceptionCount + 1;

				return ConverterUesrReport(formatFilePath, outputFilePath, keyStart, keyEnd, data, LOPSaveItem, count);
			}

          }

        public static List<string[]> Transpose(List<string[]> data)
        {
            int maxLength = 0;

            List<string[]> transpose = new List<string[]>();

            foreach (var item in data)
            {
                if (item.Length > maxLength)
                {
                    maxLength = item.Length;
                }
            }

            for (int j = 0; j < maxLength; j++)
            {
                List<string> row = new List<string>();

                for (int k = 0; k < data.Count; k++)
                {
                    if (data[k].Length > j)
                    {
                        row.Add(data[k][j]);
                    }
                    else
                    {
                        row.Add("");
                    }
                }

                transpose.Add(row.ToArray());
            }

            return transpose;
        }

		#endregion
	}
}

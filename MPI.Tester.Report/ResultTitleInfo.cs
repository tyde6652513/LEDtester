using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MPI.Tester.Data;
using MPI.Tester.TestServer;
using MPI.Tester.TestKernel;

namespace MPI.Tester.Report
{
	public class ResultTitleInfo : IEnumerable<KeyValuePair<string, string>>
	{
		#region >>> Private Property <<<

		private Dictionary<string, string> _resultTitle;
		private string _titleStr;
		private int _testIndex;
		private int _rowIndex;
		private int _colIndex;
		private int _chIndex;
        private int _groupBinIndex;
        private int _binIndex;
        private int _sequenceTimeIndex;
		private char _splitchar;
        private int _chipIndexIndex;
        private int _passIndex;

		#endregion

		#region >>> Constructor / Disposor <<<

		public ResultTitleInfo()
		{
			this._resultTitle = new Dictionary<string, string>();

			this._titleStr = string.Empty;

			this._testIndex = -1;

			this._rowIndex = -1;

			this._colIndex = -1;

			this._chIndex = -1;

            this._groupBinIndex = -1;

            this._binIndex = -1;

            this._sequenceTimeIndex = -1;
			this._splitchar = ',';

            this._chipIndexIndex = -1;
		}

		#endregion

		#region >>> Private Method <<<

		private void UpdataDataInfo()
		{
			int index = 0;

			this._titleStr = string.Empty;

			this._testIndex = -1;

			this._rowIndex = -1;

			this._colIndex = -1;

            this._groupBinIndex = -1;

            this._binIndex = -1;

            this._sequenceTimeIndex = -1;

			this._chipIndexIndex = -1;
			foreach (var item in this._resultTitle)
			{
				if (item.Key == ESysResultItem.TEST.ToString())
				{
					this._testIndex = index;
				}
				else if (item.Key == EProberDataIndex.ROW.ToString())
				{
					this._rowIndex = index;
				}
				else if (item.Key == EProberDataIndex.COL.ToString())
				{
					this._colIndex = index;
				}
				else if (item.Key == ESysResultItem.CHANNEL.ToString())
				{
					this._chIndex = index;
				}
                else if (item.Key == ESysResultItem.BIN.ToString() || item.Key == ESysResultItem.BINSN.ToString())
                {
                    this._binIndex = index;
                }
                else if (item.Key == ESysResultItem.SEQUENCETIME.ToString())//SEQUENCETIME
                {
                    this._sequenceTimeIndex = index;
                }
				else if (item.Key == ESysResultItem.CHIP_INDEX.ToString() )
                {
                    this._chipIndexIndex = index;
                }
                else if (item.Key == ESysResultItem.ISALLPASS.ToString())
                {
                    this._passIndex = index;
                }

				this._titleStr += item.Value;

				index++;

				if (index != this._resultTitle.Count)
				{
                    this._titleStr += _splitchar.ToString() ;
				}
			}
		}

		#endregion

		#region >>> Public Method <<<

        public void SetTitleStr(string key)
        {
            this._titleStr = key;
        }

		public void AddResultData(string keyName, string name)
		{
			this._resultTitle.Add(keyName, name);

			this.UpdataDataInfo();
		}

		public void AppendResultData(Dictionary<string, TestResultData> appendData)
		{
			foreach (var item in appendData)
			{
				this._resultTitle.Add(item.Value.KeyName, item.Value.Name);
			}

			this.UpdataDataInfo();
		}

        public void SetResultData(Dictionary<string, string> data, char spl = ',')
		{
            _splitchar = spl;
			this._resultTitle.Clear();

			foreach (var item in data)
			{
				this._resultTitle.Add(item.Key, item.Value);
			}

			this.UpdataDataInfo();
		}

        public int GetIndexOfKey(string key)
        {
            int counter = 0;
            foreach (var item in this._resultTitle)
            {
                if (item.Key == key)
                { return counter; }
                counter++;
            }
            return -1;
 
        }

		public void Clear()
		{
			this._resultTitle.Clear();

			this.UpdataDataInfo();
		}

		#endregion

		#region >>> Public Property <<<

		public string TitleStr
		{
			get { return this._titleStr; }
		}

		public int TestIndex
		{
			get { return this._testIndex; }
		}

		public int RowIndex
		{
			get { return this._rowIndex; }
		}

		public int ColIndex
		{
			get { return this._colIndex; }
		}

		public int CHIndex
		{
			get { return this._chIndex; }
		}

        public int ChipIndexIndex
        {
            get { return this._chipIndexIndex; }
        }

		public int ResultCount
		{
			get { return this._resultTitle.Count; }
		}

        public int GroupBinIndex
        {
            get { return this._groupBinIndex; }
        }

        public int BinIndex
        {
            get { return this._binIndex; }
        }


        public int SeqTimeIndex
        {
            get { return this._sequenceTimeIndex; }
        }

        public int IsAllPassIndex
        {
            get { return _passIndex; }
        }

		#endregion

		#region >>> IEnumerator Interface <<<

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return (System.Collections.IEnumerator)(new ResultTitleInfoEnum(this._resultTitle));
		}

		IEnumerator<KeyValuePair<string, string>> IEnumerable<KeyValuePair<string, string>>.GetEnumerator()
		{
			return (IEnumerator<KeyValuePair<string, string>>)(new ResultTitleInfoEnum(this._resultTitle));
		}

		#endregion

		#region >>> ResultTitleInfoEnum Class <<<

		private class ResultTitleInfoEnum : IEnumerator<KeyValuePair<string, string>>
		{
			#region >>> Private Property <<<

			private int _position;
			private KeyValuePair<string, string> _data;
			private Dictionary<string, string> _dataList;

			#endregion

			#region >>> Constructor / Disposor <<<

			public ResultTitleInfoEnum(Dictionary<string, string> dataList)
			{
				this._position = -1;

				this._data = default(KeyValuePair<string, string>);

				this._dataList = dataList;
			}

			#endregion

			#region >>> Interface Property <<<

			object System.Collections.IEnumerator.Current
			{
				get { return Current; }
			}

			#endregion

			#region >>> Public Method <<<

			public KeyValuePair<string, string> Current
			{
				get { return this._data; }
			}

			public bool MoveNext()
			{
				if (++this._position >= this._dataList.Count)
				{
					return false;
				}
				else
				{
					int index = 0;

					foreach (var item in this._dataList)
					{
						if (this._position == index)
						{
							this._data = item;

							break;
						}

						index++;
					}
				}

				return true;
			}

			public void Reset()
			{
				this._position = -1;
			}

			public void Dispose()
			{
			}

			#endregion
		}

		#endregion
	}
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Schema;
using System.IO;

using Newtonsoft.Json;
namespace MPI.RemoteControl2.Tester.Mpi.Command.Base
{
	[Serializable]
	public class TransferableDataObject
	{
		#region >>> Private field <<<

		private string _dataContext;
		
		#endregion
		
		/// <summary>
		/// Constructor.
		/// </summary>
		public TransferableDataObject()
		{
			_dataContext = "";
		}

		#region >>> Public property <<<

		[XmlAttribute]
		[JsonProperty("ID")]
		public int IdentifiedID;

		[XmlElement]
		[JsonProperty("Data")]
		public string DataContext
		{
			get
			{
				return this._dataContext;
			}
			set
			{
				this._dataContext = (value == null) ? "" : value;
			}
		}

		#endregion

		#region >>> Public method <<<

		public string Serialize()
		{
			return Newtonsoft.Json.JsonConvert.SerializeObject(this);
		}

		public bool Deserialize(string context)
		{
				try
				{
				TransferableDataObject obj = Newtonsoft.Json.JsonConvert.DeserializeObject(context, typeof(TransferableDataObject)) as TransferableDataObject;

				if (obj == null) return false;

					this.IdentifiedID = obj.IdentifiedID;
					this.DataContext = obj.DataContext;

					return true;
				}
			catch
				{
					return false;
				}
			}

		#endregion
	}
}

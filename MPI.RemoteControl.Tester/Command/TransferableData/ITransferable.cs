using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace MPI.RemoteControl.Tester
{
	public interface ITransferable
	{
		string Serialize();
		bool Deserialize(string context);
	}
}

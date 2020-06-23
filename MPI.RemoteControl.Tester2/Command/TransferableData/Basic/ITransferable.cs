using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace MPI.RemoteControl2.Tester.Mpi.Command.Base
{
	public interface ITransferable
	{
		string Serialize();
		bool Deserialize(string context);
	}
}
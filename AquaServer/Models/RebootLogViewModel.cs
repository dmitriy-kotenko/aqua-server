using System.Collections.Generic;
using System.Linq;
using AquaServer.Service.Models;

namespace AquaServer.Models
{
	public class RebootLogViewModel
	{
		public RebootLogRecord[] LogRecords { get; private set; }

		public RebootLogViewModel(IEnumerable<RebootLogRecord> logRecords)
		{
			LogRecords = logRecords.ToArray();
		}
	}
}
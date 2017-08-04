using System.Collections.Generic;
using AquaServer.Service.Models;

namespace AquaServer.Models
{
	public class DeviceStateLogViewModel
	{
		public IEnumerable<DeviceStateLogRecord> LogRecords { get; private set; }

		public DeviceStateLogViewModel(IEnumerable<DeviceStateLogRecord> logRecords)
		{
			LogRecords = logRecords;
		}
	}
}
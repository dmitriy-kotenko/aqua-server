using System;
using AquaServer.Core.Utils;
using AquaServer.Service.Models.Enums;

namespace AquaServer.Service.Models
{
	public class DeviceStateLogRecord
	{
		public DateTime DateTime { get; private set; }

		public Devices Device { get; private set; }

		public DeviceStates State { get; private set; }

		public DeviceStateLogRecord(DateTime utcDateTime, Devices device, DeviceStates state)
		{
			DateTime = utcDateTime.ToLocal();
			Device = device;
			State = state;
		}
	}
}

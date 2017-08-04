using System;
using AquaServer.Core.Utils;

namespace AquaServer.Service.Models
{
	public class RebootLogRecord
	{
		public DateTime RebootDateTime { get; private set; }

		public TimeSpan UptimeToReboot { get; private set; }

		public RebootLogRecord(DateTime utcRebootDateTime, long uptimeMillisToReboot)
		{
			RebootDateTime = utcRebootDateTime.ToLocal();
			UptimeToReboot = TimeSpan.FromMilliseconds(uptimeMillisToReboot);
		}
	}
}

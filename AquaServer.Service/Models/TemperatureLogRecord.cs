using System;
using AquaServer.Core.Utils;

namespace AquaServer.Service.Models
{
	public class TemperatureLogRecord
	{
		public DateTime DateTime { get; private set; }

		public decimal Temperature { get; private set; }

		public TemperatureLogRecord(DateTime utcDateTime, decimal temperature)
		{
			DateTime = utcDateTime.ToLocal();
			Temperature = temperature;
		}
	}
}

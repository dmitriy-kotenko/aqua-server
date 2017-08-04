using System;
using AquaServer.Core.Utils;

namespace AquaServer.Models
{
	public class CurrentStateViewModel
	{
		public const int TempDisplayLowerBound = 20;
		public const int TempDisplayUpperBound = 30;

		private const decimal TempWarningLowerBound = 24;
		private const decimal TempWarningUpperBound = 26;
		private const int InactiveMinutesTreshold = 5;

		public decimal Temperature { get; set; }

		public bool TemperatureWarning => Temperature < TempWarningLowerBound || Temperature > TempWarningUpperBound;

		public TimeSpan Uptime { get; set; }

		public DateTime BootDateTime { get; set; }

		public int MinutesSinceUpdate => (int)(DateTime.UtcNow.ToLocal() - BootDateTime.Add(Uptime)).TotalMinutes;

		public bool UptimeWarning => MinutesSinceUpdate >= InactiveMinutesTreshold;
	}
}
using System;
using System.Configuration;

namespace AquaServer.Core
{
	public static class Configuration
	{
		private static readonly Lazy<TimeZoneInfo> _lazyTimeZoneId = new Lazy<TimeZoneInfo>(readTimeZoneInfo);

		public static TimeZoneInfo TimeZoneId => _lazyTimeZoneId.Value;

		private static readonly Lazy<string> _lazyLogDateTimeFormat = new Lazy<string>(() => ConfigurationManager.AppSettings["LogDateTimeFormat"]);

		public static string LogDateTimeFormat => _lazyLogDateTimeFormat.Value;

		private static TimeZoneInfo readTimeZoneInfo()
		{
			string timeZoneId = ConfigurationManager.AppSettings["TimeZoneId"];
			return TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
		}
	}
}
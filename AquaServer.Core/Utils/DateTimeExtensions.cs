using System;

namespace AquaServer.Core.Utils
{
	public static class DateTimeExtensions
	{
		private static readonly DateTime _unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

		public static DateTime ToLocal(this DateTime dateTime)
		{
			if (dateTime == DateTime.MinValue)
			{
				return DateTime.MinValue;
			}

			dateTime = DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);
			return TimeZoneInfo.ConvertTime(dateTime, Configuration.TimeZoneId);
		}

		public static DateTime ToUtc(this DateTime dateTime)
		{
			if (dateTime == DateTime.MinValue)
			{
				return DateTime.MinValue;
			}

			dateTime = DateTime.SpecifyKind(dateTime, DateTimeKind.Unspecified);
			return TimeZoneInfo.ConvertTime(dateTime, Configuration.TimeZoneId, TimeZoneInfo.Utc);
		}

		public static int ToUnixTime(this DateTime dateTime)
		{
			if (dateTime == DateTime.MinValue)
			{
				return 0;
			}

			return (int)(dateTime - _unixEpoch).TotalSeconds;
		}

		public static long ToUnixTimeMilliseconds(this DateTime dateTime)
		{
			return (long)dateTime.ToUnixTime() * 1000;
		}
	}
}

using System;
using System.Text;

namespace AquaServer.Core.Utils
{
	public static class TimeSpanExtensions
	{
		public static string ToReadableString(this TimeSpan timeSpan)
		{
			var sb = new StringBuilder();
			if (timeSpan.Days >= 1)
			{
				sb.AppendFormat(new PluralFormatProvider(), "{0:day;days} ", timeSpan.Days);
			}

			if (timeSpan.TotalHours >= 1)
			{
				sb.AppendFormat("{0} hr ", timeSpan.Hours);
			}

			if (timeSpan.TotalMinutes >= 1)
			{
				sb.AppendFormat("{0} min", timeSpan.Minutes);
			}
			else
			{
				sb.AppendFormat("{0} sec", timeSpan.Seconds);
			}
			return sb.ToString();
		}
	}
}

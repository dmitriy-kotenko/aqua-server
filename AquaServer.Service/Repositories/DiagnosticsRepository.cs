using System;
using System.Collections.Generic;
using AquaServer.Core.Data;
using AquaServer.Service.Models;

namespace AquaServer.Service.Repositories
{
	public class DiagnosticsRepository : BaseRepository
	{
		internal IEnumerable<RebootLogRecord> GetRebootLog(int numberOfRecords)
		{
			var records = new List<RebootLogRecord>();

			var selectCommand = new SqlCommandInfo(
				$@"SELECT TOP {numberOfRecords} reboot_time, uptime
					FROM
						(
							SELECT
								origin_datetime AS reboot_time,
								uptime,
								LAG(uptime) OVER(ORDER BY origin_datetime DESC) AS next_uptime
							FROM DiagnosticInfoLog
						) subq
					WHERE
						uptime > next_uptime");

			SqlExecutionHelper.ExecuteReader(
				selectCommand,
				row =>
				{
					DateTime rebootDateTime = row.GetDateTime(row.GetOrdinal("reboot_time"));
					long uptime = row.GetInt64(row.GetOrdinal("uptime"));

					records.Add(new RebootLogRecord(rebootDateTime, uptime));
				});

			return records;
		}
	}
}

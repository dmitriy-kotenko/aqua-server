using System;
using System.Collections.Generic;
using System.Text;
using AquaServer.Core.Data;
using AquaServer.Service.Models;
using AquaServer.Service.Models.Enums;

namespace AquaServer.Service.Repositories
{
	internal class AquaRepository : BaseRepository
	{
		private const int ErrorMinutes = 5;

		internal long GetUptime()
		{
			var selectCommand = new SqlCommandInfo("SELECT TOP 1 uptime FROM DiagnosticInfoLog ORDER BY origin_datetime DESC");
			return SqlExecutionHelper.ExecuteScalar<long>(selectCommand);
		}

		internal DateTime GetBootDateTime()
		{
			var selectCommand = new SqlCommandInfo(
				@"SELECT TOP 1 boot_time
					FROM
					(
						SELECT
							LAG(origin_datetime) OVER(ORDER BY origin_datetime DESC) AS boot_time,
							uptime,
							LAG(uptime) OVER(ORDER BY origin_datetime DESC) AS next_uptime
						FROM DiagnosticInfoLog
					) subq
				WHERE uptime > next_uptime");

			return SqlExecutionHelper.ExecuteScalar<DateTime>(selectCommand);
		}

		internal decimal GetCurrentTemperature()
		{
			var selectCommand = new SqlCommandInfo("SELECT TOP 1 temperature FROM TemperatureLog ORDER BY origin_datetime DESC");
			var temperature = SqlExecutionHelper.ExecuteScalar<decimal>(selectCommand);
			return temperature;
		}

		internal IEnumerable<TemperatureLogRecord> GetTemperatureChartData()
		{
			return getTemperatureChartData(SqlCommandText.Empty);
		}

		internal IEnumerable<TemperatureLogRecord> GetTemperatureChartData(TimeSpan period)
		{
			SqlCommandText whereClause = getConditionByPeriod("origin_datetime", period);
			return getTemperatureChartData(whereClause);
		}

		internal IEnumerable<TemperatureLogRecord> GetTemperatureChartData(DateTime lastKnownTime)
		{
			var whereClause = new SqlCommandText("origin_datetime > @lastKnownTime");
			whereClause.Parameters.Add("lastKnownTime", lastKnownTime);
			return getTemperatureChartData(whereClause);
		}

		private IEnumerable<TemperatureLogRecord> getTemperatureChartData(SqlCommandText whereClause)
		{
			var data = new List<TemperatureLogRecord>();

			var selectCommand = new SqlCommandInfo("SELECT origin_datetime, temperature FROM TemperatureLog");

			if (!whereClause.IsEmpty)
			{
				selectCommand.CommandText += " WHERE " + whereClause.CommandText;
				selectCommand.AddParameters(whereClause);
			}

			SqlExecutionHelper.ExecuteReader(
					selectCommand,
					row =>
					{
						DateTime datetime = row.GetDateTime(row.GetOrdinal("origin_datetime"));
						decimal temperature = row.GetDecimal(row.GetOrdinal("temperature"));

						data.Add(new TemperatureLogRecord(datetime, temperature));
					});

			return data.ToArray();
		}

		internal IEnumerable<DeviceStateLogRecord> GetDeviceStateLog(TimeSpan period, Devices[] devices)
		{
			SqlCommandText conditionByDevices = getConditionToDeviceStateLogByDevices(devices);
			SqlCommandText conditionByPeriod = getConditionToDeviceStateLogByPeriod(period);
			SqlCommandText whereClause = conditionByDevices.Concat(conditionByPeriod);

			return getDeviceStateLog(whereClause);
		}

		private static SqlCommandText getConditionToDeviceStateLogByPeriod(TimeSpan period)
		{
			return getConditionByPeriod("switch_datetime", period);
		}

		internal IEnumerable<DeviceStateLogRecord> GetDeviceStateLog(Devices[] devices)
		{
			SqlCommandText whereClause = getConditionToDeviceStateLogByDevices(devices);
			return getDeviceStateLog(whereClause);
		}

		private static SqlCommandText getConditionToDeviceStateLogByDevices(Devices[] devices)
		{
			if (devices.Length == 0)
			{
				return SqlCommandText.Empty;
			}

			var whereClause = new SqlCommandText();
			var parametersListBuilder = new StringBuilder();
			foreach (Devices device in devices)
			{
				parametersListBuilder.Append($"@{device},");
				whereClause.Parameters.Add(device.ToString(), (byte)device);
			}
			parametersListBuilder.Length--;

			whereClause.CommandText = $"device IN ({parametersListBuilder})";

			return whereClause;
		}

		private IEnumerable<DeviceStateLogRecord> getDeviceStateLog(SqlCommandText whereClause)
		{
			var records = new List<DeviceStateLogRecord>();

			var selectCommand = new SqlCommandInfo(
				"SELECT" +
					" switch_datetime," +
					" device," +
					" state" +
				" FROM DeviceStateLog" +
				(whereClause.IsEmpty ? string.Empty : " WHERE " + whereClause.CommandText) +
				" ORDER BY switch_datetime DESC");

			selectCommand.AddParameters(whereClause);

			SqlExecutionHelper.ExecuteReader(
				selectCommand,
				row =>
				{
					DateTime datetime = row.GetDateTime(row.GetOrdinal("switch_datetime"));
					var device = (Devices)row.GetInt16(row.GetOrdinal("device"));
					var state = (DeviceStates)row.GetInt16(row.GetOrdinal("state"));

					records.Add(new DeviceStateLogRecord(datetime, device, state));
				});

			return records;
		}

		private static SqlCommandText getConditionByPeriod(string datetimeFieldName, TimeSpan period)
		{
			var whereClause = new SqlCommandText($"{datetimeFieldName} >= DATEADD(MINUTE, -@errorMinutes, DATEADD(SECOND, -@seconds, GETUTCDATE()))");
			whereClause.Parameters.Add("seconds", period.TotalSeconds);
			whereClause.Parameters.Add("errorMinutes", ErrorMinutes);

			return whereClause;
		}
	}
}

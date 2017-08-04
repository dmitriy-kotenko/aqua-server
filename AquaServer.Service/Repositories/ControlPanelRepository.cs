using System.Collections.Generic;
using AquaServer.Core.Data;
using AquaServer.Service.Models;
using AquaServer.Service.Models.Enums;

namespace AquaServer.Service.Repositories
{
	public class ControlPanelRepository : BaseRepository
	{
		public DeviceStates GetDeviceState(Devices device)
		{
			var selectCommand = new SqlCommandInfo(
				"SELECT current_state" +
				" FROM DeviceStates" +
				" WHERE device = @device");
			selectCommand.Parameters.Add("device", device);

			var rawDeviceState = SqlExecutionHelper.ExecuteScalar<bool>(selectCommand);
			return rawDeviceState ? DeviceStates.On : DeviceStates.Off;
		}

		public IEnumerable<DeviceControlOptionsRecord> GetDeviceControlOptions()
		{
			var records = new List<DeviceControlOptionsRecord>();

			var selectCommand = new SqlCommandInfo(
				"SELECT" +
					" device," +
					" current_state," +
					" use_schedule," +
					" target_state " +
				" FROM DeviceStates");

			SqlExecutionHelper.ExecuteReader(
				selectCommand,
				row =>
				{
					var device = row.GetFieldValue<Devices>("device");
					var useSchedule = row.GetFieldValue<bool>("use_schedule");

					var record = new DeviceControlOptionsRecord(device, useSchedule)
					{
						CurrentState = row.GetFieldValue<DeviceStates?>("current_state"),
						TargetState = row.GetFieldValue<DeviceStates?>("target_state")
					};

					records.Add(record);
				});

			return records;
		}

		public void MergeDeviceState<T>(Devices device, string fieldName, T fieldValue)
		{
			string commandText = " MERGE DeviceStates AS target" +
				   $" USING(SELECT @device, @fieldValue) AS source (device, {fieldName})" +
					" ON(target.device = source.device)" +
				" WHEN MATCHED THEN" +
				   $" UPDATE SET {fieldName} = source.{fieldName}" +
				" WHEN NOT MATCHED THEN" +
				   $" INSERT(device, {fieldName})" +
				   $" VALUES(source.device, source.{fieldName});";

			var mergeCommand = new SqlCommandInfo(commandText);
			mergeCommand.Parameters.Add("device", device);
			mergeCommand.Parameters.Add("fieldValue", fieldValue);

			SqlExecutionHelper.ExecuteNonQuery(mergeCommand);
		}
	}
}

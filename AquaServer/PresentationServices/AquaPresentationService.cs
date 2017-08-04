using System;
using System.Collections.Generic;
using System.Linq;
using AquaServer.Core.Utils;
using AquaServer.Models;
using AquaServer.PresentationServices.ZoneFormatters;
using AquaServer.Service.Models;
using AquaServer.Service.Models.Enums;
using AquaServer.Service.Services;

namespace AquaServer.PresentationServices
{
	public class AquaPresentationService
	{
		private readonly AquaService _service = new AquaService();

		#region TemperatureChart

		public TemperatureChartViewModel GetTemperatureChartModel(bool highlightZones)
		{
			return GetTemperatureChartModel(TimeSpan.MinValue, highlightZones);
		}

		public TemperatureChartViewModel GetTemperatureChartModel(TimeSpan period, bool highlightZones)
		{
			IEnumerable<TemperatureLogRecord> logRecords = period != TimeSpan.MinValue
				? _service.GetTemperatureChartData(period)
				: _service.GetTemperatureChartData();

			object[][] data = convertTemperatureLogToData(logRecords);

			var model = new TemperatureChartViewModel(data);
			if (highlightZones)
			{
				model.Zones = GetTemperatureChartZones(period);
			}

			return model;
		}

		public object[][] GetTemperatureChartData(long lastKnownMillis)
		{
			DateTime lastKnownTime = DateTimeOffset.FromUnixTimeMilliseconds(lastKnownMillis).DateTime.ToUtc();
			IEnumerable<TemperatureLogRecord> logRecords = _service.GetTemperatureChartData(lastKnownTime);

			return convertTemperatureLogToData(logRecords);
		}

		private static object[][] convertTemperatureLogToData(IEnumerable<TemperatureLogRecord> logRecords)
		{
			return logRecords
				.Select(item => new object[] { item.DateTime.ToUnixTimeMilliseconds(), item.Temperature })
				.ToArray();
		}

		public TemperatureChartZoneModel[] GetTemperatureChartZones(TimeSpan period)
		{
			TimeSpan extendedPeriod = period.Add(TimeSpan.FromHours(6));
			IEnumerable<DeviceStateLogRecord> states = _service.GetDeviceStateLog(extendedPeriod, Devices.Cooler, Devices.Heater);

			var zones = new List<TemperatureChartZoneModel>();

			foreach (var statesByDevice in states.Reverse().GroupBy(state => state.Device))
			{
				ZoneFormatBase format = ZoneFormatBase.GetFormat(statesByDevice.Key);
				var zonesByDevice = new List<TemperatureChartZoneModel>();

				IEnumerable<DeviceStateLogRecord> uniqueConsecutiveStatesByDevice = getUniqueConsecutiveRecords(statesByDevice);

				foreach (DeviceStateLogRecord stateRecord in uniqueConsecutiveStatesByDevice)
				{
					if (stateRecord.State == DeviceStates.Off && zonesByDevice.Count == 0)
					{
						continue;
					}

					zonesByDevice.Add(createZone(stateRecord, format));
				}

				// add format to infinite timespan if device is still switched on
				if (statesByDevice.Last().State == DeviceStates.On)
				{
					var recordToPresent = new DeviceStateLogRecord(DateTime.MinValue, statesByDevice.Key, DeviceStates.Off);
					zonesByDevice.Add(createZone(recordToPresent, format));
				}

				zones.AddRange(zonesByDevice);
			}

			return zones.OrderBy(zone => zone.Value.GetValueOrDefault(long.MaxValue)).ToArray();
		}

		private static IEnumerable<DeviceStateLogRecord> getUniqueConsecutiveRecords(IEnumerable<DeviceStateLogRecord> records)
		{
			var results = new List<DeviceStateLogRecord>();
			foreach (DeviceStateLogRecord record in records)
			{
				if (results.Count == 0 || results.Last().State != record.State)
				{
					results.Add(record);
				}
			}

			return results;
		}

		private static TemperatureChartZoneModel createZone(DeviceStateLogRecord logRecord, ZoneFormatBase format)
		{
			return new TemperatureChartZoneModel
			{
				Value = logRecord.DateTime == DateTime.MinValue ? (long?)null : logRecord.DateTime.ToUnixTimeMilliseconds(),
				Color = format.GetColor(logRecord.State),
				DashStyle = format.GetDashStyle(logRecord.State)
			};
		}

		#endregion

		#region DeviceStateLog

		public DeviceStateLogViewModel GetDeviceStateLogViewModel()
		{
			IEnumerable<DeviceStateLogRecord> logRecords = _service.GetDeviceStateLog();
			return new DeviceStateLogViewModel(logRecords);
		}

		public DeviceStateLogViewModel GetDeviceStateLogViewModel(TimeSpan period)
		{
			IEnumerable<DeviceStateLogRecord> logRecords = _service.GetDeviceStateLog(period);
			return new DeviceStateLogViewModel(logRecords);
		}

		#endregion

		#region CurrentState

		public CurrentStateViewModel GetCurrentStateViewModel()
		{
			decimal currentTemperature = _service.GetCurrentTemperature();
			return GetCurrentStateViewModel(currentTemperature);
		}

		public CurrentStateViewModel GetCurrentStateViewModel(decimal temperature)
		{
			return new CurrentStateViewModel
			{
				Temperature = temperature,
				Uptime = getUptime(),
				BootDateTime = _service.GetBootDateTime()
			};
		}

		private TimeSpan getUptime()
		{
			long uptime = _service.GetUptime();
			return TimeSpan.FromMilliseconds(uptime);
		}

		#endregion
	}
}
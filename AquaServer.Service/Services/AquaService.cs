using System;
using System.Collections.Generic;
using AquaServer.Core.Utils;
using AquaServer.Service.Models;
using AquaServer.Service.Repositories;
using AquaServer.Service.Models.Enums;

namespace AquaServer.Service.Services
{
	public class AquaService
	{
		private readonly AquaRepository _repository = new AquaRepository();

		public long GetUptime()
		{
			return _repository.GetUptime();
		}

		public DateTime GetBootDateTime()
		{
			DateTime utcBootTime = _repository.GetBootDateTime();
			return utcBootTime.ToLocal();
		}

		public decimal GetCurrentTemperature()
		{
			return _repository.GetCurrentTemperature();
		}

		public IEnumerable<TemperatureLogRecord> GetTemperatureChartData()
		{
			return _repository.GetTemperatureChartData();
		}

		public IEnumerable<TemperatureLogRecord> GetTemperatureChartData(TimeSpan period)
		{
			return _repository.GetTemperatureChartData(period);
		}

		public IEnumerable<TemperatureLogRecord> GetTemperatureChartData(DateTime lastKnownTime)
		{
			return _repository.GetTemperatureChartData(lastKnownTime);
		}

		public IEnumerable<DeviceStateLogRecord> GetDeviceStateLog(TimeSpan period, params Devices[] devices)
		{
			return _repository.GetDeviceStateLog(period, devices);
		}

		public IEnumerable<DeviceStateLogRecord> GetDeviceStateLog(params Devices[] devices)
		{
			return _repository.GetDeviceStateLog(devices);
		}
	}
}

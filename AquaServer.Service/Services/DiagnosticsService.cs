using System;
using System.Collections.Generic;
using AquaServer.Service.Models;
using AquaServer.Service.Repositories;

namespace AquaServer.Service.Services
{
	public class DiagnosticsService
	{
		private readonly AquaRepository _aquaRepository = new AquaRepository();
		private readonly DiagnosticsRepository _repository = new DiagnosticsRepository();

		public IEnumerable<RebootLogRecord> GetRebootLog(int numberOfRecords)
		{
			return _repository.GetRebootLog(numberOfRecords);
		}

		public TimeSpan GetUptime()
		{
			long uptime = _aquaRepository.GetUptime();
			return TimeSpan.FromMilliseconds(uptime);
		}
	}
}

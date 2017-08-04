using System;

namespace AquaServer.Models
{
	public class DiagnosticsViewModel
	{
		public TimeSpan Uptime { get; set; }

		public RebootLogViewModel RebootLog { get; set; }
	}
}
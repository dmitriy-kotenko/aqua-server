using System.Collections.Generic;
using AquaServer.Service.Models.Enums;

namespace AquaServer.Models
{
	public class ControlPanelViewModel
	{
		public Dictionary<Devices, DeviceControlOptions> DevicesOptions { get; set; }

		public class DeviceControlOptions
		{
			public DeviceStates? CurrentState { get; set; }

			public bool UseSchedule { get; set; }

			public DeviceStates? TargetState { get; set; }

			public static readonly DeviceControlOptions Default = new DeviceControlOptions { UseSchedule = true };
		}
	}
}
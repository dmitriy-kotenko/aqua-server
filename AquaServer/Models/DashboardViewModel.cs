using AquaServer.Service.Models.Enums;

namespace AquaServer.Models
{
	public class DashboardViewModel
	{
		public CurrentStateViewModel CurrentState { get; set; }

		public TemperatureChartViewModel TemperatureChart { get; set; }

		public DeviceStateLogViewModel DeviceStates { get; set; }

		public Devices[] SwitchableDevices { get; set; }

		public ControlPanelViewModel ControlPanel { get; set; }
	}
}
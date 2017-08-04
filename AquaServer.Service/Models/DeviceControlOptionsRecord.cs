using AquaServer.Service.Models.Enums;

namespace AquaServer.Service.Models
{
	public class DeviceControlOptionsRecord
	{
		public Devices Device { get; private set; }

		public DeviceStates? CurrentState { get; set; }

		public bool UseSchedule { get; private set; }

		public DeviceStates? TargetState { get; set; }

		public DeviceControlOptionsRecord(Devices device, bool useSchedule)
		{
			Device = device;
			UseSchedule = useSchedule;
		}
	}
}

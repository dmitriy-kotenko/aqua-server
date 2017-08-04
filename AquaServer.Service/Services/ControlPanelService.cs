using System.Collections.Generic;
using AquaServer.Service.Models;
using AquaServer.Service.Models.Enums;
using AquaServer.Service.Repositories;

namespace AquaServer.Service.Services
{
	public class ControlPanelService
	{
		private readonly ControlPanelRepository _repository = new ControlPanelRepository();

		public DeviceStates GetDeviceState(Devices device)
		{
			return _repository.GetDeviceState(device);
		}

		public IEnumerable<DeviceControlOptionsRecord> GetDeviceControlOptions()
		{
			return _repository.GetDeviceControlOptions();
		}

		public void SetDeviceCurrentState(Devices device, DeviceStates targetState)
		{
			_repository.MergeDeviceState(device, "current_state", targetState);
		}

		public void SetDeviceUseSchedule(Devices device, bool useSchedule)
		{
			_repository.MergeDeviceState(device, "use_schedule", useSchedule);
		}

		public void SetDeviceTargetState(Devices device, DeviceStates targetState)
		{
			_repository.MergeDeviceState(device, "target_state", targetState);
		}
	}
}

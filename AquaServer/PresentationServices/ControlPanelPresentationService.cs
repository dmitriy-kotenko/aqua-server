using System.Linq;
using AquaServer.Models;
using AquaServer.Service.Models.Enums;
using AquaServer.Service.Services;

namespace AquaServer.PresentationServices
{
	public class ControlPanelPresentationService
	{
		private readonly ControlPanelService _service = new ControlPanelService();

		public ControlPanelViewModel GetControlPanelViewModel()
		{
			return new ControlPanelViewModel
			{
				DevicesOptions = _service.GetDeviceControlOptions()
					.ToDictionary(
						item => item.Device,
						item =>
						new ControlPanelViewModel.DeviceControlOptions
						{
							CurrentState = item.CurrentState,
							UseSchedule = item.UseSchedule,
							TargetState = item.TargetState
						})
			};
		}

		public void SetDeviceUseSchedule(Devices device, bool useSchedule)
		{
			_service.SetDeviceUseSchedule(device, useSchedule);
		}

		public void SetDeviceTargetState(Devices device, DeviceStates targetState)
		{
			_service.SetDeviceTargetState(device, targetState);
		}
	}
}
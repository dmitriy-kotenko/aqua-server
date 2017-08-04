using System.Web.Http;
using AquaServer.Models;
using AquaServer.Models.Api;
using AquaServer.PresentationServices;

namespace AquaServer.Controllers.Api
{
	public class ControlPanelController : ApiController
	{
		private readonly ControlPanelPresentationService _presentationService = new ControlPanelPresentationService();

		[ActionName("deviceUseSchedule")]
		public IHttpActionResult PostDeviceUseSchedule(PostDeviceUseScheduleRequest request)
		{
			_presentationService.SetDeviceUseSchedule(request.Device, request.UseSchedule);

			return Ok();
		}

		[ActionName("deviceTargetState")]
		public IHttpActionResult PostDeviceTargetState(PostDeviceTargetStateRequest request)
		{
			_presentationService.SetDeviceTargetState(request.Device, request.State);

			return Ok();
		}

		[ActionName("deviceStates")]
		public IHttpActionResult GetDeviceStates()
		{
			ControlPanelViewModel controlPanelViewModel = _presentationService.GetControlPanelViewModel();
			return Json(controlPanelViewModel.DevicesOptions);
		}
	}
}

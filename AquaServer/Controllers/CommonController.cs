using System;
using System.Web.Mvc;
using AquaServer.Core.MvcExtensions;
using AquaServer.Models;
using AquaServer.PresentationServices;

namespace AquaServer.Controllers
{
	public class CommonController : Controller
	{
		private static readonly TimeSpan _temperatureChartPeriod = TimeSpan.FromDays(45);
		private static readonly TimeSpan _deviceStateLogPeriod = TimeSpan.FromDays(10);

		private readonly AquaPresentationService _presentationService = new AquaPresentationService();

		public ActionResult TemperatureLog()
		{
			TemperatureChartViewModel viewModel = _presentationService.GetTemperatureChartModel(_temperatureChartPeriod, false);
			viewModel.SelectedButtonIndex = 3;
			return View(viewModel);
		}

		public ActionResult DeviceStateLog()
		{
			DeviceStateLogViewModel viewModel = _presentationService.GetDeviceStateLogViewModel(_deviceStateLogPeriod);
			return View(viewModel);
		}

		public ActionResult TemperatureChartJs(TemperatureChartViewModel model)
		{
			return new JsViewResult(model);
		}
	}
}
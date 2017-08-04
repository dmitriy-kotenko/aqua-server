using System;
using System.Linq;
using System.Web.Mvc;
using AquaServer.Core.MvcExtensions;
using AquaServer.Models;
using AquaServer.PresentationServices;
using AquaServer.Service.Models.Enums;

namespace AquaServer.Controllers
{
	public class DashboardController : Controller
	{
		internal static readonly TimeSpan Period = TimeSpan.FromDays(1);

		private static readonly Devices[] _switchableDevices =
		{
			Devices.Light,
			Devices.Filter,
			Devices.Heater,
			Devices.Cooler,
			Devices.Pump,
			Devices.CO2
		};

		private readonly AquaPresentationService _presentationService = new AquaPresentationService();
		private readonly ControlPanelPresentationService _controlPanelPresentationService = new ControlPanelPresentationService();

		public ActionResult Index()
		{
			TemperatureChartViewModel temperatureChartViewModel = _presentationService.GetTemperatureChartModel(Period, true);
			temperatureChartViewModel.RemoveLastButtons(2);
			temperatureChartViewModel.SelectedButtonIndex = 2;

			DeviceStateLogViewModel deviceStateLogViewModel = _presentationService.GetDeviceStateLogViewModel(Period);
			ControlPanelViewModel controlPanelViewModel = _controlPanelPresentationService.GetControlPanelViewModel();

			object[] lastTempData = temperatureChartViewModel.Data.LastOrDefault();

			CurrentStateViewModel currentState = _presentationService.GetCurrentStateViewModel((decimal?)lastTempData?[1] ?? 0);

			var model = new DashboardViewModel
			{
				CurrentState = currentState,
				TemperatureChart = temperatureChartViewModel,
				DeviceStates = deviceStateLogViewModel,
				SwitchableDevices = _switchableDevices,
				ControlPanel = controlPanelViewModel
			};

			return View(model);
		}

		public ActionResult DeviceStateLogSection()
		{
			DeviceStateLogViewModel viewModel = _presentationService.GetDeviceStateLogViewModel(Period);
			return PartialView("_DeviceStateLogSection", viewModel);
		}

		public ActionResult CurrentStateSection()
		{
			CurrentStateViewModel viewModel = _presentationService.GetCurrentStateViewModel();
			return PartialView("_CurrentStateSection", viewModel);
		}

		[OutputCache(Duration = int.MaxValue, VaryByParam = "alphaChannelValue")]
		public ActionResult TemperatureBarCss(decimal alphaChannelValue)
		{
			return new CssViewResult(alphaChannelValue);
		}
	}
}

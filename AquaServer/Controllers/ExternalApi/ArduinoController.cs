using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AquaServer.Core.Data;
using AquaServer.Core.Utils;
using AquaServer.Models.ExternalApi;
using AquaServer.Service.Models;
using AquaServer.Service.Models.Enums;
using AquaServer.Service.Services;

namespace AquaServer.Controllers.ExternalApi
{
	public class ArduinoController : ApiController
	{
		private readonly ControlPanelService _controlPanelService = new ControlPanelService();

		private readonly SqlExecutionHelper _sqlExecutionHelper = new SqlExecutionHelper();

		// GET api/arduino/currentTime
		[ActionName("currentTime")]
		public int GetCurrentTime()
		{
			return DateTime.UtcNow.ToLocal().ToUnixTime();
		}

		// POST api/arduino/data
		[ActionName("data")]
		public void PostData(PostDataRequest request)
		{
			var insertTemperatureCommand = new SqlCommandInfo("INSERT INTO TemperatureLog(temperature) VALUES (@temperature)");
			insertTemperatureCommand.Parameters.Add("temperature", request.Temperature);
			_sqlExecutionHelper.ExecuteNonQuery(insertTemperatureCommand);

			var insertDiagnosticInfoCommand = new SqlCommandInfo("INSERT INTO DiagnosticInfoLog(uptime) VALUES (@uptime)");
			insertDiagnosticInfoCommand.Parameters.Add("uptime", request.Uptime);
			_sqlExecutionHelper.ExecuteNonQuery(insertDiagnosticInfoCommand);

			foreach (var deviceState in request.DeviceStates)
			{
				onDeviceStateChanged(deviceState.Key, deviceState.Value);
			}
		}

		// POST api/arduino/deviceState
		[ActionName("deviceState")]
		public void PostDeviceState(PostDeviceStateRequest request)
		{
			onDeviceStateChanged(request.Device, request.State);
		}

		// GET api/arduino/settings
		[ActionName("settings")]
		public IHttpActionResult GetSettings(char? separator = null)
		{
			IEnumerable<DeviceControlOptionsRecord> deviceControlOptions = _controlPanelService.GetDeviceControlOptions();

			IEnumerable<string> deviceSettings = deviceControlOptions.Select(deviceControlOption =>
				string.Format(
					"{0}:{1}:{2}",
					(int)deviceControlOption.Device,
					deviceControlOption.UseSchedule ? 1 : 0,
					deviceControlOption.TargetState.HasValue
						? (int)deviceControlOption.TargetState.Value
						: (int)DeviceStates.Off));

			string joinSeparator = separator?.ToString() ?? Environment.NewLine;
			string settingsString = string.Join(joinSeparator, deviceSettings);

			var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
			{
				Content = new StringContent(settingsString)
			};
			return ResponseMessage(responseMessage);
		}

		private void onDeviceStateChanged(Devices device, DeviceStates state)
		{
			DeviceStates currentDeviceState = _controlPanelService.GetDeviceState(device);
			if (state == currentDeviceState)
			{
				return;
			}

			_controlPanelService.SetDeviceCurrentState(device, state);
			addDeviceStateLogRecord(device, state);
		}

		private void addDeviceStateLogRecord(Devices device, DeviceStates state)
		{
			var insertCommand = new SqlCommandInfo("INSERT INTO DeviceStateLog(device, state) VALUES (@device, @state)");
			insertCommand.Parameters.Add("device", device);
			insertCommand.Parameters.Add("state", state);

			_sqlExecutionHelper.ExecuteNonQuery(insertCommand);
		}
	}
}
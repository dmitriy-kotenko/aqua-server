using System;
using System.Linq;
using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;
using AquaServer.Models.ExternalApi;
using AquaServer.Service.Models.Enums;

namespace AquaServer.Binders
{
	public class ArduinoDataBinder : IModelBinder
	{
		private const int NumberOfFixedLines = 2;

		public bool BindModel(HttpActionContext actionContext, ModelBindingContext bindingContext)
		{
			string request = actionContext.Request.Content.ReadAsStringAsync().Result;
			string[] lines = request.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

			if (lines.Length < NumberOfFixedLines + 1)
			{
				addModelError(bindingContext, "Malformed data");
				return false;
			}

			float temperature;
			if (!float.TryParse(lines[0], out temperature))
			{
				addModelError(bindingContext, "Cannot parse temperature value");
				return false;
			}

			long uptime;
			if (!long.TryParse(lines[1], out uptime))
			{
				addModelError(bindingContext, "Cannot parse uptime value");
				return false;
			}

			var model = new PostDataRequest
			{
				Temperature = temperature,
				Uptime = uptime
			};

			foreach (string deviceStateLine in lines.Skip(NumberOfFixedLines))
			{
				Devices device;
				DeviceStates state;
				if (!parseDeviceStateLine(deviceStateLine, bindingContext, out device, out state))
				{
					return false;
				}

				model.DeviceStates[device] = state;
			}

			bindingContext.Model = model;
			return true;
		}

		private static bool parseDeviceStateLine(string line, ModelBindingContext bindingContext, out Devices device, out DeviceStates state)
		{
			device = 0;
			state = 0;
			const char separator = ':';
			string[] parts = line.Split(separator);
			if (parts.Length != 2)
			{
				addModelError(bindingContext, $"Device state lines should contain 2 parts divided by '{separator}'");
				return false;
			}

			if (!Enum.TryParse(parts[0], out device))
			{
				addModelError(bindingContext, "Cannot parse device value");
				return false;
			}
			if (!Enum.IsDefined(typeof(Devices), device))
			{
				addModelError(bindingContext, "Unknown device");
				return false;
			}

			if (!Enum.TryParse(parts[1], out state))
			{
				addModelError(bindingContext, "Cannot parse device state value");
				return false;
			}
			if (!Enum.IsDefined(typeof(DeviceStates), state))
			{
				addModelError(bindingContext, "Unknown device state");
				return false;
			}

			return true;
		}

		private static void addModelError(ModelBindingContext bindingContext, string errorMessage)
		{
			bindingContext.ModelState.AddModelError(bindingContext.ModelName, errorMessage);
		}
	}
}
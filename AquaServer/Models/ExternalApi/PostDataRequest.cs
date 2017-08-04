using System.Collections.Generic;
using System.Web.Http.ModelBinding;
using AquaServer.Binders;
using AquaServer.Service.Models.Enums;

namespace AquaServer.Models.ExternalApi
{
	[ModelBinder(typeof(ArduinoDataBinder))]
	public class PostDataRequest
	{
		public float Temperature { get; set; }

		public Dictionary<Devices, DeviceStates> DeviceStates { get; }

		public long Uptime { get; set; }

		public PostDataRequest()
		{
			DeviceStates = new Dictionary<Devices, DeviceStates>();
		}
	}
}
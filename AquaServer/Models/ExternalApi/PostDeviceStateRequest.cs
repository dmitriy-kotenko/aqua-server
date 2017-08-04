using AquaServer.Core.Attributes;
using AquaServer.Service.Models.Enums;

namespace AquaServer.Models.ExternalApi
{
	public class PostDeviceStateRequest
	{
		[ValidEnumValue]
		public Devices Device { get; set; }

		[ValidEnumValue]
		public DeviceStates State { get; set; }
	}
}
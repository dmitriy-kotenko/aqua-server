using AquaServer.Core.Attributes;
using AquaServer.Service.Models.Enums;

namespace AquaServer.Models.Api
{
	public class PostDeviceTargetStateRequest
	{
		[ValidEnumValue]
		public Devices Device { get; set; }

		[ValidEnumValue]
		public DeviceStates State { get; set; }
	}
}
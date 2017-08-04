using AquaServer.Core.Attributes;
using AquaServer.Service.Models.Enums;

namespace AquaServer.Models.Api
{
	public class PostDeviceUseScheduleRequest
	{
		[ValidEnumValue]
		public Devices Device { get; set; }

		public bool UseSchedule { get; set; }
	}
}
using AquaServer.Service.Models.Enums;

namespace AquaServer.PresentationServices.ZoneFormatters
{
	public class CoolerZoneFormat : ZoneFormatBase
	{
		public override string GetDashStyle(DeviceStates deviceState)
		{
			return deviceState == DeviceStates.On ? null : "ShortDot";
		}
	}
}
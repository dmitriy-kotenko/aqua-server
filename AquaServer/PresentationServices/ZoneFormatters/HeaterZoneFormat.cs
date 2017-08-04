using AquaServer.Service.Models.Enums;

namespace AquaServer.PresentationServices.ZoneFormatters
{
	public class HeaterZoneFormat : ZoneFormatBase
	{
		public override string GetColor(DeviceStates deviceState)
		{
			return deviceState == DeviceStates.On ? null : "red";
		}
	}
}
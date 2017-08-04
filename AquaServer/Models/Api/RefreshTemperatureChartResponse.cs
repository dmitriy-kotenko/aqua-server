// ReSharper disable UnusedAutoPropertyAccessor.Global
namespace AquaServer.Models.Api
{
	public class RefreshTemperatureChartResponse
	{
		public object[][] NewData { get; set; }

		public TemperatureChartZoneModel[] Zones { get; set; }
	}
}
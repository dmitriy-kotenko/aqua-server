using System.Web.Http;
using AquaServer.Models.Api;
using AquaServer.PresentationServices;

namespace AquaServer.Controllers.Api
{
	public class DashboardApiController : ApiController
	{
		private readonly AquaPresentationService _service = new AquaPresentationService();

		[ActionName("getRefreshData")]
		public IHttpActionResult GetRefreshData(long lastKnownMillis)
		{
			object[][] newData = _service.GetTemperatureChartData(lastKnownMillis);

			var response = new RefreshTemperatureChartResponse
			{
				NewData = newData,
				Zones = newData.Length == 0
					? new Models.TemperatureChartZoneModel[0]
					: _service.GetTemperatureChartZones(DashboardController.Period)
			};
			return Ok(response);
		}
	}
}
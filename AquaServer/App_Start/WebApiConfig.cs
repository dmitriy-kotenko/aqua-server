using System.Web.Http;
using System.Web.Http.ModelBinding;
using System.Web.Http.ModelBinding.Binders;
using AquaServer.Binders;
using AquaServer.Core.Attributes;
using AquaServer.Models.ExternalApi;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace AquaServer
{
	public static class WebApiConfig
	{
		public static void Register(HttpConfiguration configuration)
		{
			configuration.Routes.MapHttpRoute(
				"Dashboard API",
				"api/dashboard/{action}",
				new { controller = "DashboardApi" });

			configuration.Routes.MapHttpRoute(
				"API Default",
				"api/{controller}/{action}/{parameter}",
				new { parameter = RouteParameter.Optional });

			configuration.Filters.Add(new ValidModelStateAttribute());


			var provider = new SimpleModelBinderProvider(typeof(PostDataRequest), new ArduinoDataBinder());
			configuration.Services.Insert(typeof(ModelBinderProvider), 0, provider);

			var jsonSerializerSettings = new JsonSerializerSettings
			{
				NullValueHandling = NullValueHandling.Ignore,
				ContractResolver = new CamelCasePropertyNamesContractResolver()
			};

			configuration.Formatters.JsonFormatter.SerializerSettings = jsonSerializerSettings;
			JsonConvert.DefaultSettings = () => jsonSerializerSettings;
		}
	}
}
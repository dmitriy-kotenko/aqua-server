using System.Web.Optimization;

namespace AquaServer
{
	public static class BundleConfig
	{
		public static void RegisterScriptBundles(BundleCollection bundles)
		{
			bundles.UseCdn = true;

			bundles.Add(new ScriptBundle("~/bundles/jquery", "https://code.jquery.com/jquery-2.2.3.min.js").Include(
						"~/Scripts/jquery-2.2.3.js"));
			bundles.Add(new ScriptBundle("~/bundles/bootstrap", "https://maxcdn.bootstrapcdn.com/bootstrap/3.3.6/js/bootstrap.min.js").Include(
						"~/Scripts/bootstrap.js"));
			bundles.Add(new ScriptBundle("~/bundles/highstock", "https://code.highcharts.com/stock/4.2.5/highstock.js").Include(
						"~/Scripts/highstock.js"));
			bundles.Add(new ScriptBundle("~/bundles/bootstrap-toggle", "https://gitcdn.github.io/bootstrap-toggle/2.2.2/js/bootstrap-toggle.min.js").Include(
						"~/Scripts/bootstrap-toggle.js"));
			bundles.Add(new ScriptBundle("~/bundles/moment", "https://cdnjs.cloudflare.com/ajax/libs/moment.js/2.17.1/moment.js").Include(
						"~/Scripts/moment.js"));

			bundles.Add(new ScriptBundle("~/bundles/dashboard").Include(
				"~/Scripts/Site/dashboard.js",
				"~/Scripts/Site/common.js",
				"~/Scripts/Site/control-panel.js",
				"~/Scripts/Site/current-state.js"));
		}

		public static void RegisterStyleBundles(BundleCollection bundles)
		{
			bundles.Add(new StyleBundle("~/Content/bootstrap-css", "https://maxcdn.bootstrapcdn.com/bootstrap/3.3.6/css/bootstrap.min.css").Include(
						"~/Content/bootstrap.css"));
			bundles.Add(new StyleBundle("~/Content/bootstrap-toggle", "https://gitcdn.github.io/bootstrap-toggle/2.2.2/css/bootstrap-toggle.min.css").Include(
						"~/Content/bootstrap-toggle.css"));

			bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/Site/common.css"));
			bundles.Add(new StyleBundle("~/Content/dashboard").Include(
				"~/Content/Site/dashboard.css",
				"~/Content/Site/current-state.css"));
		}
	}
}

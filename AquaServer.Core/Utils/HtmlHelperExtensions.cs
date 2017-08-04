using System.Web.Mvc;
using System.Web.Routing;

namespace AquaServer.Core.Utils
{
	public static class HtmlHelperExtensions
	{
		public static string IsSelected(this HtmlHelper html, string controller, string action)
		{
			ViewContext viewContext = html.ViewContext;
			bool isChildAction = viewContext.Controller.ControllerContext.IsChildAction;

			if (isChildAction)
			{
				viewContext = html.ViewContext.ParentActionViewContext;
			}

			RouteValueDictionary routeValues = viewContext.RouteData.Values;
			string currentAction = routeValues["action"].ToString();
			string currentController = routeValues["controller"].ToString();

			return currentAction == action && currentController == controller
				? "active"
				: string.Empty;
		}
	}
}

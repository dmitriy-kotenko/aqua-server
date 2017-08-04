using System.Web.Mvc;

namespace AquaServer.Core.MvcExtensions
{
	public class JsViewResult : PartialViewResult
	{
		public JsViewResult(object model)
		{
			if (model != null)
			{
				ViewData.Model = model;
			}
		}
	}
}

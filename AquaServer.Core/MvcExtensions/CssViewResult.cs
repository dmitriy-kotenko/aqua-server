using System.Web.Mvc;

namespace AquaServer.Core.MvcExtensions
{
	public class CssViewResult : PartialViewResult
	{
		public CssViewResult(object model)
		{
			if (model != null)
			{
				ViewData.Model = model;
			}
		}

		public override void ExecuteResult(ControllerContext context)
		{
			context.HttpContext.Response.ContentType = "text/css";
			base.ExecuteResult(context);
		}
	}
}

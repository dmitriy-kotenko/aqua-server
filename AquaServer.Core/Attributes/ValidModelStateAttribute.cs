﻿using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;
using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;

namespace AquaServer.Core.Attributes
{
	public class ValidModelStateAttribute : ActionFilterAttribute
	{
		public override void OnActionExecuting(HttpActionContext actionContext)
		{
			ModelStateDictionary modelState = actionContext.ModelState;

			if (!modelState.IsValid)
			{
				actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.BadRequest, modelState);
			}
		}
	}
}

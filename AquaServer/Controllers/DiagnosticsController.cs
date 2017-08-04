using System.Collections.Generic;
using System.Web.Mvc;
using AquaServer.Models;
using AquaServer.Service.Models;
using AquaServer.Service.Services;

namespace AquaServer.Controllers
{
	public class DiagnosticsController : Controller
	{
		private const int NumberOfRecords = 15;

		private readonly DiagnosticsService _diagnosticsService = new DiagnosticsService();

		// GET: Diagnostic
		public ActionResult Index()
		{
			IEnumerable<RebootLogRecord> rebootLog = _diagnosticsService.GetRebootLog(NumberOfRecords);

			var diagnosticsViewModel = new DiagnosticsViewModel
			{
				Uptime = _diagnosticsService.GetUptime(),
				RebootLog = new RebootLogViewModel(rebootLog)
			};
			return View(diagnosticsViewModel);
		}
	}
}
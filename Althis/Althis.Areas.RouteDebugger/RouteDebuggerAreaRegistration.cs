using System.Web.Http;
using System.Web.Mvc;

namespace Althis.Areas.RouteDebugger;

public class RouteDebuggerAreaRegistration : AreaRegistration
{
	public override string AreaName => "RouteDebugger";

	public override void RegisterArea(AreaRegistrationContext context)
	{
		context.MapRoute("RouteDebugger_default", "rd/{action}", new
		{
			controller = "RouteDebugger",
			action = "Index"
		});
		RouteDebuggerConfig.Register(GlobalConfiguration.Configuration);
	}
}

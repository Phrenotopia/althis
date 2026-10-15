using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;
using System.Web.Optimization;

namespace Althis.Areas.RouteDebugger;

public static class RouteDebuggerConfig
{
	public static void Register(HttpConfiguration config)
	{
		config.MessageHandlers.Add(new InspectHandler());
		ServicesContainer services = config.Services;
		config.Services.Replace(typeof(IHttpActionInvoker), (object)new InspectActionInvoker(ServicesExtensions.GetActionInvoker(services)));
		config.Services.Replace(typeof(IHttpActionSelector), (object)new InspectActionSelector(ServicesExtensions.GetActionSelector(services)));
		config.Services.Replace(typeof(IHttpControllerSelector), (object)new InspectControllerSelector(ServicesExtensions.GetHttpControllerSelector(services)));
		BundleTable.Bundles.Add(new ScriptBundle("~/bundles/routedebugger").Include("~/Scripts/jquery-{version}.js", "~/Scripts/knockout-{version}.js", "~/Areas/RouteDebugger/Scripts/detect.js"));
	}
}

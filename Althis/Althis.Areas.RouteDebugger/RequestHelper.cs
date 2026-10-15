using System;
using System.Linq;
using System.Net.Http;
using System.Web.Http.Hosting;
using Althis.Areas.RouteDebugger.Components;

namespace Althis.Areas.RouteDebugger;

public static class RequestHelper
{
	public static readonly string InspectHeaderName = "RouteInspecting";

	public static readonly string RouteDataCache = "RD_ROUTEDATA";

	public static readonly string RoutesCache = "RD_ROUTES";

	public static readonly string ControllerCache = "RD_CONTROLLER";

	public static readonly string ActionCache = "RD_ACTION";

	public static readonly string SelectedController = "RD_SELECTED_CONTROLLER";

	public static bool IsInspectRequest(this HttpRequestMessage request)
	{
		if (request.Headers.TryGetValues(InspectHeaderName, out var values) && string.Equals(values.FirstOrDefault(), "true", StringComparison.InvariantCulture))
		{
			return request.IsFromLocal();
		}
		return false;
	}

	public static bool IsFromLocal(this HttpRequestMessage request)
	{
		if (request == null)
		{
			return false;
		}
		if (request.Properties.TryGetValue<Lazy<bool>>(HttpPropertyKeys.IsLocalKey, out var value))
		{
			return value.Value;
		}
		return false;
	}
}

using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Routing;
using Althis.Areas.RouteDebugger.Models;

namespace Althis.Areas.RouteDebugger;

public class InspectHandler : DelegatingHandler
{
	protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
	{
		if (request.IsInspectRequest())
		{
			HttpConfiguration config = GlobalConfiguration.Configuration;
			request.Properties[RequestHelper.RouteDataCache] = new RouteDataInfo
			{
				RouteTemplate = HttpRequestMessageExtensions.GetRouteData(request).Route.RouteTemplate,
				Data = HttpRequestMessageExtensions.GetRouteData(request).Values.Select((KeyValuePair<string, object> pair) => new KeyValuePair<string, string>(pair.Key, pair.Value.ToString())).ToArray()
			};
			request.Properties[RequestHelper.RoutesCache] = ((IEnumerable<IHttpRoute>)config.Routes).Select((IHttpRoute route) => new RouteInfo
			{
				RouteTemplate = route.RouteTemplate,
				Defaults = ((route.Defaults != null) ? route.Defaults.Select((KeyValuePair<string, object> pair) => new KeyValuePair<string, string>(pair.Key, pair.Value.ToString())).ToArray() : null),
				Constraints = ((route.Constraints != null) ? route.Constraints.Select((KeyValuePair<string, object> pair) => new KeyValuePair<string, string>(pair.Key, pair.Value.ToString())).ToArray() : null),
				DataTokens = ((route.DataTokens != null) ? route.DataTokens.Select((KeyValuePair<string, object> pair) => new KeyValuePair<string, string>(pair.Key, pair.Value.ToString())).ToArray() : null),
				Handler = ((route.Handler != null) ? route.Handler.GetType().Name : null),
				Picked = (route.RouteTemplate == HttpRequestMessageExtensions.GetRouteData(request).Route.RouteTemplate)
			}).ToArray();
			HttpResponseMessage response = await base.SendAsync(request, cancellationToken);
			if (response.StatusCode != HttpStatusCode.OK)
			{
				HttpRequestMessage requestMessage = response.RequestMessage;
				InspectData inspectData = new InspectData(requestMessage);
				inspectData.RealHttpStatus = response.StatusCode;
				response = HttpRequestMessageExtensions.CreateResponse<InspectData>(requestMessage, HttpStatusCode.OK, inspectData);
			}
			response.Headers.Add(RequestHelper.InspectHeaderName, "done");
			return response;
		}
		return await base.SendAsync(request, cancellationToken);
	}
}

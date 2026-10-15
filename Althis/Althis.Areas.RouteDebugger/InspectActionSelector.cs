using System.Linq;
using System.Net.Http;
using System.Web.Http.Controllers;
using Althis.Areas.RouteDebugger.Components;

namespace Althis.Areas.RouteDebugger;

public class InspectActionSelector : IHttpActionSelector
{
	private IHttpActionSelector _innerSelector;

	public InspectActionSelector(IHttpActionSelector innerSelector)
	{
		_innerSelector = innerSelector;
	}

	public ILookup<string, HttpActionDescriptor> GetActionMapping(HttpControllerDescriptor controllerDescriptor)
	{
		return _innerSelector.GetActionMapping(controllerDescriptor);
	}

	public HttpActionDescriptor SelectAction(HttpControllerContext controllerContext)
	{
		HttpRequestMessage request = controllerContext.Request;
		if (request.IsInspectRequest())
		{
			ActionSelectSimulator actionSelectSimulator = new ActionSelectSimulator();
			request.Properties[RequestHelper.ActionCache] = actionSelectSimulator.Simulate(controllerContext);
		}
		return _innerSelector.SelectAction(controllerContext);
	}
}

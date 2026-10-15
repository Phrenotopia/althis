using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;
using Althis.Areas.RouteDebugger.Models;

namespace Althis.Areas.RouteDebugger;

public class InspectControllerSelector : IHttpControllerSelector
{
	private IHttpControllerSelector _innerSelector;

	public InspectControllerSelector(IHttpControllerSelector innerSelector)
	{
		_innerSelector = innerSelector;
	}

	public IDictionary<string, HttpControllerDescriptor> GetControllerMapping()
	{
		return _innerSelector.GetControllerMapping();
	}

	public HttpControllerDescriptor SelectController(HttpRequestMessage request)
	{
		if (request.IsInspectRequest())
		{
			ControllerSelectionInfo[] value = _innerSelector.GetControllerMapping().Values.Select((HttpControllerDescriptor desc) => new ControllerSelectionInfo
			{
				ControllerName = desc.ControllerName,
				ControllerType = desc.ControllerType.AssemblyQualifiedName
			}).ToArray();
			request.Properties[RequestHelper.ControllerCache] = value;
		}
		HttpControllerDescriptor val = _innerSelector.SelectController(request);
		request.Properties[RequestHelper.SelectedController] = val.ControllerName;
		return val;
	}
}

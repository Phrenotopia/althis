using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using Althis.Areas.RouteDebugger.Models;

namespace Althis.Areas.RouteDebugger;

public class InspectActionInvoker : IHttpActionInvoker
{
	private IHttpActionInvoker _innerInvoker;

	public InspectActionInvoker(IHttpActionInvoker innerInvoker)
	{
		_innerInvoker = innerInvoker;
	}

	public Task<HttpResponseMessage> InvokeActionAsync(HttpActionContext actionContext, CancellationToken cancellationToken)
	{
		if (actionContext.Request.IsInspectRequest())
		{
			InspectData inspectData = new InspectData(actionContext.Request);
			inspectData.RealHttpStatus = HttpStatusCode.OK;
			return Task.FromResult(HttpRequestMessageExtensions.CreateResponse<InspectData>(actionContext.Request, HttpStatusCode.OK, inspectData));
		}
		return _innerInvoker.InvokeActionAsync(actionContext, cancellationToken);
	}
}

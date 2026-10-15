using System.Linq;
using System.Net.Http;
using System.Web.Http.Controllers;

namespace Althis.Areas.RouteDebugger.Models;

public class ActionSelectionInfo
{
	public string ActionName { get; set; }

	public HttpMethod[] SupportedHttpMethods { get; set; }

	public HttpParameterDescriptorInfo[] Parameters { get; set; }

	public bool? FoundByActionName { get; set; }

	public bool? FoundByActionNameWithRightVerb { get; set; }

	public bool? FoundByVerb { get; set; }

	public bool? FoundWithRightParam { get; set; }

	public bool? FoundWithSelectorsRun { get; set; }

	public ActionSelectionInfo(HttpActionDescriptor descriptor)
	{
		ActionName = descriptor.ActionName;
		SupportedHttpMethods = descriptor.SupportedHttpMethods.ToArray();
		Parameters = (from p in descriptor.GetParameters()
			select new HttpParameterDescriptorInfo(p)).ToArray();
	}
}

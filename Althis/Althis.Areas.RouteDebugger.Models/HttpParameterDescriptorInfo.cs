using System;
using System.Web.Http.Controllers;

namespace Althis.Areas.RouteDebugger.Models;

public class HttpParameterDescriptorInfo
{
	public string ParameterName { get; set; }

	public Type ParameterType { get; set; }

	public string ParameterTypeName { get; set; }

	public HttpParameterDescriptorInfo(HttpParameterDescriptor descriptor)
	{
		ParameterName = descriptor.ParameterName;
		ParameterType = descriptor.ParameterType;
		ParameterTypeName = descriptor.ParameterType.Name;
	}
}

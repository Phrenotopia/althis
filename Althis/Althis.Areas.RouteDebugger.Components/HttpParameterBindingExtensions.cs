using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;
using System.Web.Http.ValueProviders;
using System.Web.Http.ValueProviders.Providers;

namespace Althis.Areas.RouteDebugger.Components;

internal static class HttpParameterBindingExtensions
{
	public static bool WillReadUri(this HttpParameterBinding parameterBinding)
	{
		if (parameterBinding == null)
		{
			throw new ArgumentNullException("parameterBinding");
		}
		IValueProviderParameterBinding val = (IValueProviderParameterBinding)(object)((parameterBinding is IValueProviderParameterBinding) ? parameterBinding : null);
		if (val != null)
		{
			IEnumerable<ValueProviderFactory> valueProviderFactories = val.ValueProviderFactories;
			if (valueProviderFactories.Any() && valueProviderFactories.All((ValueProviderFactory factory) => factory is QueryStringValueProviderFactory || factory is RouteDataValueProviderFactory))
			{
				return true;
			}
		}
		return false;
	}
}

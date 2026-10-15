using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Routing;
using Althis.Areas.RouteDebugger.Models;

namespace Althis.Areas.RouteDebugger.Components;

public class ActionSelectSimulator
{
	private ReflectedHttpActionDescriptor[] _actionDescriptors;

	private IDictionary<ReflectedHttpActionDescriptor, string[]> _actionParameterNames = new Dictionary<ReflectedHttpActionDescriptor, string[]>();

	private ILookup<string, ReflectedHttpActionDescriptor> _actionNameMapping;

	private void Initialize(HttpControllerDescriptor controllerDesc)
	{
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Expected Obj, but got Unknown
		MethodInfo[] methods = controllerDesc.ControllerType.GetMethods(BindingFlags.Instance | BindingFlags.Public);
		MethodInfo[] array = Array.FindAll(methods, IsValidActionMethod);
		_actionDescriptors = new ReflectedHttpActionDescriptor[array.Length];
		for (int i = 0; i < array.Length; i++)
		{
			MethodInfo methodInfo = array[i];
			ReflectedHttpActionDescriptor val = new ReflectedHttpActionDescriptor(controllerDesc, methodInfo);
			_actionDescriptors[i] = val;
			HttpActionBinding actionBinding = ((HttpActionDescriptor)val).ActionBinding;
			_actionParameterNames.Add(val, (from binding in actionBinding.ParameterBindings
				where !binding.Descriptor.IsOptional && TypeHelper.IsSimpleUnderlyingType(binding.Descriptor.ParameterType) && binding.WillReadUri()
				select binding.Descriptor.Prefix ?? binding.Descriptor.ParameterName).ToArray());
		}
		_actionNameMapping = _actionDescriptors.ToLookup((ReflectedHttpActionDescriptor actionDesc) => ((HttpActionDescriptor)actionDesc).ActionName, StringComparer.OrdinalIgnoreCase);
	}

	public ActionSelectionLog Simulate(HttpControllerContext controllerContext)
	{
		Initialize(controllerContext.ControllerDescriptor);
		ActionSelectionLog actionSelectionLog = new ActionSelectionLog((IEnumerable<HttpActionDescriptor>)(object)_actionDescriptors);
		ReflectedHttpActionDescriptor[] array = null;
		IHttpRouteData routeData = HttpRequestMessageExtensions.GetRouteData(controllerContext.Request);
		if (routeData.Values.TryGetValue<string>("action", out var value))
		{
			ReflectedHttpActionDescriptor[] array2 = _actionNameMapping[value].OfType<ReflectedHttpActionDescriptor>().ToArray();
			array = array2.Where((ReflectedHttpActionDescriptor actionDescriptor) => ((HttpActionDescriptor)actionDescriptor).SupportedHttpMethods.Contains(controllerContext.Request.Method)).ToArray();
			actionSelectionLog.ActionName = value;
			actionSelectionLog.MarkSelected((IEnumerable<HttpActionDescriptor>)(object)array2, (ActionSelectionInfo info) =>
			{
				info.FoundByActionName = true;
			});
			actionSelectionLog.MarkOthersSelected((IEnumerable<HttpActionDescriptor>)(object)array2, (ActionSelectionInfo info) =>
			{
				info.FoundByActionName = false;
			});
			actionSelectionLog.MarkSelected((IEnumerable<HttpActionDescriptor>)(object)array2, (ActionSelectionInfo info) =>
			{
				info.FoundByActionNameWithRightVerb = false;
			});
			actionSelectionLog.MarkSelected((IEnumerable<HttpActionDescriptor>)(object)array, (ActionSelectionInfo info) =>
			{
				info.FoundByActionNameWithRightVerb = true;
			});
		}
		else
		{
			actionSelectionLog.ActionName = string.Empty;
			actionSelectionLog.HttpMethod = controllerContext.Request.Method;
			if (string.IsNullOrEmpty(value))
			{
				array = FindActionsForVerb(actionSelectionLog.HttpMethod);
				actionSelectionLog.MarkSelected((IEnumerable<HttpActionDescriptor>)(object)array, (ActionSelectionInfo info) =>
				{
					info.FoundByVerb = true;
				});
			}
		}
		if (array != null && array.Length != 0)
		{
			ReflectedHttpActionDescriptor[] array3 = FindActionUsingRouteAndQueryParameters(controllerContext, array, !string.IsNullOrEmpty(value)).ToArray();
			actionSelectionLog.MarkSelected((IEnumerable<HttpActionDescriptor>)(object)array, (ActionSelectionInfo info) =>
			{
				info.FoundWithRightParam = false;
			});
			actionSelectionLog.MarkSelected((IEnumerable<HttpActionDescriptor>)(object)array3, (ActionSelectionInfo info) =>
			{
				info.FoundWithRightParam = true;
			});
			ReflectedHttpActionDescriptor[] actions = RunSelectionFilters(controllerContext, (IEnumerable<HttpActionDescriptor>)(object)array3).ToArray();
			actionSelectionLog.MarkSelected((IEnumerable<HttpActionDescriptor>)(object)array3, (ActionSelectionInfo info) =>
			{
				info.FoundWithSelectorsRun = false;
			});
			actionSelectionLog.MarkSelected((IEnumerable<HttpActionDescriptor>)(object)actions, (ActionSelectionInfo info) =>
			{
				info.FoundWithSelectorsRun = true;
			});
		}
		return actionSelectionLog;
	}

	private ReflectedHttpActionDescriptor[] FindActionsForVerb(HttpMethod verb)
	{
		List<ReflectedHttpActionDescriptor> list = new List<ReflectedHttpActionDescriptor>();
		ReflectedHttpActionDescriptor[] actionDescriptors = _actionDescriptors;
		foreach (ReflectedHttpActionDescriptor val in actionDescriptors)
		{
			if (((HttpActionDescriptor)val).SupportedHttpMethods.Contains(verb))
			{
				list.Add(val);
			}
		}
		return list.ToArray();
	}

	private IEnumerable<ReflectedHttpActionDescriptor> FindActionUsingRouteAndQueryParameters(HttpControllerContext controllerContext, IEnumerable<ReflectedHttpActionDescriptor> actionsFound, bool hasActionRouteKey)
	{
		IDictionary<string, object> values = controllerContext.RouteData.Values;
		HashSet<string> hashSet = new HashSet<string>(values.Keys, StringComparer.OrdinalIgnoreCase);
		hashSet.Remove("controller");
		if (hasActionRouteKey)
		{
			hashSet.Remove("action");
		}
		HttpRequestMessage request = controllerContext.Request;
		Uri requestUri = request.RequestUri;
		bool flag = requestUri != null && !string.IsNullOrEmpty(requestUri.Query);
		if (hashSet.Count != 0 || flag)
		{
			HashSet<string> combinedParameterNames = new HashSet<string>(hashSet, StringComparer.OrdinalIgnoreCase);
			if (flag)
			{
				foreach (KeyValuePair<string, string> queryNameValuePair in HttpRequestMessageExtensions.GetQueryNameValuePairs(request))
				{
					combinedParameterNames.Add(queryNameValuePair.Key);
				}
			}
			actionsFound = actionsFound.Where((ReflectedHttpActionDescriptor descriptor) => IsSubset(_actionParameterNames[descriptor], combinedParameterNames));
			if (actionsFound.Count() > 1)
			{
				actionsFound = (from descriptor in actionsFound
					group descriptor by _actionParameterNames[descriptor].Length into g
					orderby g.Key descending
					select g).First();
			}
		}
		else
		{
			actionsFound = actionsFound.Where((ReflectedHttpActionDescriptor descriptor) => _actionParameterNames[descriptor].Length == 0);
		}
		return actionsFound;
	}

	private static bool IsValidActionMethod(MethodInfo methodInfo)
	{
		if (methodInfo.IsSpecialName)
		{
			return false;
		}
		if (methodInfo.GetBaseDefinition().DeclaringType.IsAssignableFrom(TypeHelper.ApiControllerType))
		{
			return false;
		}
		return true;
	}

	private static bool IsSubset(string[] actionParameters, HashSet<string> routeAndQueryParameters)
	{
		foreach (string item in actionParameters)
		{
			if (!routeAndQueryParameters.Contains(item))
			{
				return false;
			}
		}
		return true;
	}

	private static List<ReflectedHttpActionDescriptor> RunSelectionFilters(HttpControllerContext controllerContext, IEnumerable<HttpActionDescriptor> descriptorsFound)
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Expected Obj, but got Unknown
		List<ReflectedHttpActionDescriptor> list = null;
		List<ReflectedHttpActionDescriptor> list2 = new List<ReflectedHttpActionDescriptor>();
		foreach (ReflectedHttpActionDescriptor item in descriptorsFound)
		{
			ReflectedHttpActionDescriptor val = item;
			NonActionAttribute[] array = ((HttpActionDescriptor)val).GetCustomAttributes<NonActionAttribute>().ToArray();
			if (array.Length == 0)
			{
				list2.Add(val);
			}
		}
		if (list != null && list.Count > 0)
		{
			return list;
		}
		return list2;
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http.Controllers;

namespace Althis.Areas.RouteDebugger.Models;

public class ActionSelectionLog
{
	private Dictionary<HttpActionDescriptor, ActionSelectionInfo> _actionDescriptors;

	public string ActionName { get; set; }

	public HttpMethod HttpMethod { get; set; }

	public ActionSelectionInfo[] ActionSelections => _actionDescriptors.Values.ToArray();

	public ActionSelectionLog(IEnumerable<HttpActionDescriptor> actions)
	{
		_actionDescriptors = new Dictionary<HttpActionDescriptor, ActionSelectionInfo>();
		foreach (HttpActionDescriptor action in actions)
		{
			_actionDescriptors[action] = new ActionSelectionInfo(action);
		}
	}

	internal void MarkSelected(IEnumerable<HttpActionDescriptor> actions, Action<ActionSelectionInfo> marking)
	{
		foreach (HttpActionDescriptor action in actions)
		{
			if (_actionDescriptors.TryGetValue(action, out var value))
			{
				marking(value);
			}
		}
	}

	internal void MarkOthersSelected(IEnumerable<HttpActionDescriptor> actions, Action<ActionSelectionInfo> marking)
	{
		HashSet<HttpActionDescriptor> hashSet = new HashSet<HttpActionDescriptor>(_actionDescriptors.Keys);
		foreach (HttpActionDescriptor action in actions)
		{
			hashSet.Remove(action);
		}
		foreach (HttpActionDescriptor item in hashSet)
		{
			if (_actionDescriptors.TryGetValue(item, out var value))
			{
				marking(value);
			}
		}
	}
}

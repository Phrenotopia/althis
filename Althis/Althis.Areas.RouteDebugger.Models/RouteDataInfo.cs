using System.Collections.Generic;

namespace Althis.Areas.RouteDebugger.Models;

public class RouteDataInfo
{
	public string RouteTemplate { get; set; }

	public KeyValuePair<string, string>[] Data { get; set; }
}

using System;
using System.Collections.Generic;

namespace Althis.Areas.RouteDebugger.Components;

internal static class DictionaryExtensions
{
	public static bool TryGetValue<T>(this IDictionary<string, object> dictionary, string key, out T value)
	{
		if (dictionary == null)
		{
			throw new ArgumentNullException("dictionary");
		}
		if (dictionary.TryGetValue(key, out var value2) && value2 is T)
		{
			value = (T)value2;
			return true;
		}
		value = default;
		return false;
	}
}

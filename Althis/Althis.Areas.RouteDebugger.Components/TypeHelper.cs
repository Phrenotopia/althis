using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace Althis.Areas.RouteDebugger.Components;

internal static class TypeHelper
{
	private static readonly Type TaskGenericType = typeof(Task<>);

	internal static readonly Type HttpControllerType = typeof(IHttpController);

	internal static readonly Type ApiControllerType = typeof(ApiController);

	internal static Type GetTaskInnerTypeOrNull(Type type)
	{
		if (type.IsGenericType && !type.IsGenericTypeDefinition)
		{
			Type genericTypeDefinition = type.GetGenericTypeDefinition();
			if (TaskGenericType == genericTypeDefinition)
			{
				return type.GetGenericArguments()[0];
			}
		}
		return null;
	}

	internal static Type ExtractGenericInterface(Type queryType, Type interfaceType)
	{
		Func<Type, bool> func = (Type t) => t.IsGenericType && t.GetGenericTypeDefinition() == interfaceType;
		if (!func(queryType))
		{
			return queryType.GetInterfaces().FirstOrDefault(func);
		}
		return queryType;
	}

	internal static Type[] GetTypeArgumentsIfMatch(Type closedType, Type matchingOpenType)
	{
		if (!closedType.IsGenericType)
		{
			return null;
		}
		Type genericTypeDefinition = closedType.GetGenericTypeDefinition();
		if (!(matchingOpenType == genericTypeDefinition))
		{
			return null;
		}
		return closedType.GetGenericArguments();
	}

	internal static bool IsCompatibleObject(Type type, object value)
	{
		if (value != null || !TypeAllowsNullValue(type))
		{
			return type.IsInstanceOfType(value);
		}
		return true;
	}

	internal static bool IsNullableValueType(Type type)
	{
		return Nullable.GetUnderlyingType(type) != null;
	}

	internal static bool TypeAllowsNullValue(Type type)
	{
		if (type.IsValueType)
		{
			return IsNullableValueType(type);
		}
		return true;
	}

	internal static bool IsSimpleType(Type type)
	{
		if (!type.IsPrimitive && !type.Equals(typeof(string)) && !type.Equals(typeof(DateTime)) && !type.Equals(typeof(decimal)) && !type.Equals(typeof(Guid)) && !type.Equals(typeof(DateTimeOffset)))
		{
			return type.Equals(typeof(TimeSpan));
		}
		return true;
	}

	internal static bool IsSimpleUnderlyingType(Type type)
	{
		Type underlyingType = Nullable.GetUnderlyingType(type);
		if (underlyingType != null)
		{
			type = underlyingType;
		}
		return IsSimpleType(type);
	}

	internal static bool HasStringConverter(Type type)
	{
		return TypeDescriptor.GetConverter(type).CanConvertFrom(typeof(string));
	}

	internal static ReadOnlyCollection<T> OfType<T>(object[] objects) where T : class
	{
		int num = objects.Length;
		List<T> list = new List<T>(num);
		int num2 = 0;
		for (int i = 0; i < num; i++)
		{
			if (objects[i] is T item)
			{
				list.Add(item);
				num2++;
			}
		}
		list.Capacity = num2;
		return new ReadOnlyCollection<T>(list);
	}
}

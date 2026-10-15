using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web.Http.Controllers;
using System.Web.Http.Description;
using System.Xml.XPath;

namespace Althis.Areas.HelpPage;

public class XmlDocumentationProvider : IDocumentationProvider
{
	private const string MethodExpression = "/doc/members/member[@name='M:{0}']";

	private const string ParameterExpression = "param[@name='{0}']";

	private XPathNavigator _documentNavigator;

	public XmlDocumentationProvider(string documentPath)
	{
		if (documentPath == null)
		{
			throw new ArgumentNullException("documentPath");
		}
		XPathDocument xPathDocument = new XPathDocument(documentPath);
		_documentNavigator = xPathDocument.CreateNavigator();
	}

	public virtual string GetDocumentation(HttpActionDescriptor actionDescriptor)
	{
		XPathNavigator methodNode = GetMethodNode(actionDescriptor);
		if (methodNode != null)
		{
			XPathNavigator xPathNavigator = methodNode.SelectSingleNode("summary");
			if (xPathNavigator != null)
			{
				return xPathNavigator.Value.Trim();
			}
		}
		return null;
	}

	public virtual string GetDocumentation(HttpParameterDescriptor parameterDescriptor)
	{
		ReflectedHttpParameterDescriptor val = (ReflectedHttpParameterDescriptor)(object)((parameterDescriptor is ReflectedHttpParameterDescriptor) ? parameterDescriptor : null);
		if (val != null)
		{
			XPathNavigator methodNode = GetMethodNode(((HttpParameterDescriptor)val).ActionDescriptor);
			if (methodNode != null)
			{
				string name = val.ParameterInfo.Name;
				XPathNavigator xPathNavigator = methodNode.SelectSingleNode(string.Format(CultureInfo.InvariantCulture, "param[@name='{0}']", new object[1] { name }));
				if (xPathNavigator != null)
				{
					return xPathNavigator.Value.Trim();
				}
			}
		}
		return null;
	}

	private XPathNavigator GetMethodNode(HttpActionDescriptor actionDescriptor)
	{
		ReflectedHttpActionDescriptor val = (ReflectedHttpActionDescriptor)(object)((actionDescriptor is ReflectedHttpActionDescriptor) ? actionDescriptor : null);
		if (val != null)
		{
			string xpath = string.Format(CultureInfo.InvariantCulture, "/doc/members/member[@name='M:{0}']", new object[1] { GetMemberName(val.MethodInfo) });
			return _documentNavigator.SelectSingleNode(xpath);
		}
		return null;
	}

	private static string GetMemberName(MethodInfo method)
	{
		string text = string.Format(CultureInfo.InvariantCulture, "{0}.{1}", new object[2]
		{
			method.DeclaringType.FullName,
			method.Name
		});
		ParameterInfo[] parameters = method.GetParameters();
		if (parameters.Length != 0)
		{
			string[] value = parameters.Select((ParameterInfo param) => GetTypeName(param.ParameterType)).ToArray();
			text += string.Format(CultureInfo.InvariantCulture, "({0})", new object[1] { string.Join(",", value) });
		}
		return text;
	}

	private static string GetTypeName(Type type)
	{
		if (type.IsGenericType)
		{
			Type genericTypeDefinition = type.GetGenericTypeDefinition();
			Type[] genericArguments = type.GetGenericArguments();
			string fullName = genericTypeDefinition.FullName;
			fullName = fullName.Substring(0, fullName.IndexOf('`'));
			string[] value = genericArguments.Select((Type t) => GetTypeName(t)).ToArray();
			return string.Format(CultureInfo.InvariantCulture, "{0}{{{1}}}", new object[2]
			{
				fullName,
				string.Join(",", value)
			});
		}
		return type.FullName;
	}
}

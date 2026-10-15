using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Web.Http.Description;
using System.Xml.Linq;
using Newtonsoft.Json;

namespace Althis.Areas.HelpPage;

public class HelpPageSampleGenerator
{
	public IDictionary<HelpPageSampleKey, Type> ActualHttpMessageTypes { get; internal set; }

	public IDictionary<HelpPageSampleKey, object> ActionSamples { get; internal set; }

	public IDictionary<Type, object> SampleObjects { get; internal set; }

	public HelpPageSampleGenerator()
	{
		ActualHttpMessageTypes = new Dictionary<HelpPageSampleKey, Type>();
		ActionSamples = new Dictionary<HelpPageSampleKey, object>();
		SampleObjects = new Dictionary<Type, object>();
	}

	public IDictionary<MediaTypeHeaderValue, object> GetSampleRequests(ApiDescription api)
	{
		return GetSample(api, SampleDirection.Request);
	}

	public IDictionary<MediaTypeHeaderValue, object> GetSampleResponses(ApiDescription api)
	{
		return GetSample(api, SampleDirection.Response);
	}

	public virtual IDictionary<MediaTypeHeaderValue, object> GetSample(ApiDescription api, SampleDirection sampleDirection)
	{
		if (api == null)
		{
			throw new ArgumentNullException("api");
		}
		string controllerName = api.ActionDescriptor.ControllerDescriptor.ControllerName;
		string actionName = api.ActionDescriptor.ActionName;
		IEnumerable<string> parameterNames = api.ParameterDescriptions.Select((ApiParameterDescription p) => p.Name);
		Type type = ResolveType(api, controllerName, actionName, parameterNames, sampleDirection, out var formatters);
		Dictionary<MediaTypeHeaderValue, object> dictionary = new Dictionary<MediaTypeHeaderValue, object>();
		IEnumerable<KeyValuePair<HelpPageSampleKey, object>> allActionSamples = GetAllActionSamples(controllerName, actionName, parameterNames, sampleDirection);
		foreach (KeyValuePair<HelpPageSampleKey, object> item in allActionSamples)
		{
			dictionary.Add(item.Key.MediaType, WrapSampleIfString(item.Value));
		}
		if (type != null && !typeof(HttpResponseMessage).IsAssignableFrom(type))
		{
			object sampleObject = GetSampleObject(type);
			foreach (MediaTypeFormatter item2 in formatters)
			{
				foreach (MediaTypeHeaderValue supportedMediaType in item2.SupportedMediaTypes)
				{
					if (!dictionary.ContainsKey(supportedMediaType))
					{
						object obj = GetActionSample(controllerName, actionName, parameterNames, type, item2, supportedMediaType, sampleDirection);
						if (obj == null && sampleObject != null)
						{
							obj = WriteSampleObjectUsingFormatter(item2, sampleObject, type, supportedMediaType);
						}
						dictionary.Add(supportedMediaType, WrapSampleIfString(obj));
					}
				}
			}
		}
		return dictionary;
	}

	public virtual object GetActionSample(string controllerName, string actionName, IEnumerable<string> parameterNames, Type type, MediaTypeFormatter formatter, MediaTypeHeaderValue mediaType, SampleDirection sampleDirection)
	{
		if (ActionSamples.TryGetValue(new HelpPageSampleKey(mediaType, sampleDirection, controllerName, actionName, parameterNames), out var value) || ActionSamples.TryGetValue(new HelpPageSampleKey(mediaType, sampleDirection, controllerName, actionName, new string[1] { "*" }), out value) || ActionSamples.TryGetValue(new HelpPageSampleKey(mediaType, type), out value))
		{
			return value;
		}
		return null;
	}

	public virtual object GetSampleObject(Type type)
	{
		if (!SampleObjects.TryGetValue(type, out var value))
		{
			ObjectGenerator objectGenerator = new ObjectGenerator();
			return objectGenerator.GenerateObject(type);
		}
		return value;
	}

	public virtual Type ResolveType(ApiDescription api, string controllerName, string actionName, IEnumerable<string> parameterNames, SampleDirection sampleDirection, out Collection<MediaTypeFormatter> formatters)
	{
		if (!Enum.IsDefined(typeof(SampleDirection), sampleDirection))
		{
			throw new InvalidEnumArgumentException("sampleDirection", (int)sampleDirection, typeof(SampleDirection));
		}
		if (api == null)
		{
			throw new ArgumentNullException("api");
		}
		if (ActualHttpMessageTypes.TryGetValue(new HelpPageSampleKey(sampleDirection, controllerName, actionName, parameterNames), out var value) || ActualHttpMessageTypes.TryGetValue(new HelpPageSampleKey(sampleDirection, controllerName, actionName, new string[1] { "*" }), out value))
		{
			Collection<MediaTypeFormatter> collection = new Collection<MediaTypeFormatter>();
			foreach (MediaTypeFormatter item in (Collection<MediaTypeFormatter>)(object)api.ActionDescriptor.Configuration.Formatters)
			{
				if (IsFormatSupported(sampleDirection, item, value))
				{
					collection.Add(item);
				}
			}
			formatters = collection;
		}
		else
		{
			switch (sampleDirection)
			{
			case SampleDirection.Request:
			{
				ApiParameterDescription val = api.ParameterDescriptions.FirstOrDefault((ApiParameterDescription p) =>
				{
					//IL_0001: Unknown result type (might be due to invalid IL or missing references)
					//IL_0007: Invalid comparison between Unknown and I4
					return (int)p.Source == 1;
				});
				value = ((val == null) ? null : val.ParameterDescriptor.ParameterType);
				formatters = api.SupportedRequestBodyFormatters;
				break;
			}
			default:
				value = api.ActionDescriptor.ReturnType;
				formatters = api.SupportedResponseFormatters;
				break;
			}
		}
		return value;
	}

	public virtual object WriteSampleObjectUsingFormatter(MediaTypeFormatter formatter, object value, Type type, MediaTypeHeaderValue mediaType)
	{
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Expected Obj, but got Unknown
		if (formatter == null)
		{
			throw new ArgumentNullException("formatter");
		}
		if (mediaType == null)
		{
			throw new ArgumentNullException("mediaType");
		}
		object result = string.Empty;
		MemoryStream memoryStream = null;
		HttpContent httpContent = null;
		try
		{
			if (formatter.CanWriteType(type))
			{
				memoryStream = new MemoryStream();
				httpContent = (HttpContent)new ObjectContent(type, value, formatter, mediaType);
				formatter.WriteToStreamAsync(type, value, (Stream)memoryStream, httpContent, (TransportContext)null).Wait();
				memoryStream.Position = 0L;
				StreamReader streamReader = new StreamReader(memoryStream);
				string text = streamReader.ReadToEnd();
				if (mediaType.MediaType.ToUpperInvariant().Contains("XML"))
				{
					text = TryFormatXml(text);
				}
				else if (mediaType.MediaType.ToUpperInvariant().Contains("JSON"))
				{
					text = TryFormatJson(text);
				}
				result = new TextSample(text);
			}
			else
			{
				result = new InvalidSample(string.Format(CultureInfo.CurrentCulture, "Failed to generate the sample for media type '{0}'. Cannot use formatter '{1}' to write type '{2}'.", new object[3]
				{
					mediaType,
					((object)formatter).GetType().Name,
					type.Name
				}));
			}
		}
		catch (Exception ex)
		{
			result = new InvalidSample(string.Format(CultureInfo.CurrentCulture, "An exception has occurred while using the formatter '{0}' to generate sample for media type '{1}'. Exception message: {2}", new object[3]
			{
				((object)formatter).GetType().Name,
				mediaType.MediaType,
				ex.Message
			}));
		}
		finally
		{
			memoryStream?.Dispose();
			httpContent?.Dispose();
		}
		return result;
	}

	private static string TryFormatJson(string str)
	{
		try
		{
			object value = JsonConvert.DeserializeObject(str);
			return JsonConvert.SerializeObject(value, Formatting.Indented);
		}
		catch
		{
			return str;
		}
	}

	private static string TryFormatXml(string str)
	{
		try
		{
			XDocument xDocument = XDocument.Parse(str);
			return xDocument.ToString();
		}
		catch
		{
			return str;
		}
	}

	private static bool IsFormatSupported(SampleDirection sampleDirection, MediaTypeFormatter formatter, Type type)
	{
		return sampleDirection switch
		{
			SampleDirection.Request => formatter.CanReadType(type), 
			SampleDirection.Response => formatter.CanWriteType(type), 
			_ => false, 
		};
	}

	private IEnumerable<KeyValuePair<HelpPageSampleKey, object>> GetAllActionSamples(string controllerName, string actionName, IEnumerable<string> parameterNames, SampleDirection sampleDirection)
	{
		HashSet<string> parameterNamesSet = new HashSet<string>(parameterNames, StringComparer.OrdinalIgnoreCase);
		foreach (KeyValuePair<HelpPageSampleKey, object> sample in ActionSamples)
		{
			KeyValuePair<HelpPageSampleKey, object> keyValuePair = sample;
			HelpPageSampleKey sampleKey = keyValuePair.Key;
			if (string.Equals(controllerName, sampleKey.ControllerName, StringComparison.OrdinalIgnoreCase) && string.Equals(actionName, sampleKey.ActionName, StringComparison.OrdinalIgnoreCase) && (sampleKey.ParameterNames.SetEquals(new string[1] { "*" }) || parameterNamesSet.SetEquals(sampleKey.ParameterNames)) && sampleDirection == sampleKey.SampleDirection)
			{
				yield return sample;
			}
		}
	}

	private static object WrapSampleIfString(object sample)
	{
		if (sample is string text)
		{
			return new TextSample(text);
		}
		return sample;
	}
}

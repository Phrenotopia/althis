using System.Web.Http;

namespace Althis;

public static class WebApiConfig
{
	public static void Register(HttpConfiguration config)
	{
		HttpRouteCollectionExtensions.MapHttpRoute(config.Routes, "DefaultApi", "api/{controller}/{id}");
		HttpRouteCollectionExtensions.MapHttpRoute(config.Routes, "GetAllApi", "api/{controller}/all/{filter}");
		HttpRouteCollectionExtensions.MapHttpRoute(config.Routes, "GetByGuidApi", "api/{controller}/byguid/{guid}");
		HttpRouteCollectionExtensions.MapHttpRoute(config.Routes, "GetNewApi", "api/{controller}/new/{name}");
		HttpRouteCollectionExtensions.MapHttpRoute(config.Routes, "MapSectionApi", "api/{controller}/section/{id}/{pos}/{width}/{height}", (object)new
		{
			id = 1,
			pos = 0,
			width = 16,
			height = 16
		});
		HttpRouteCollectionExtensions.MapHttpRoute(config.Routes, "GroupByMapIdApi", "api/{controller}/bymapid/{mapid}");
		HttpRouteCollectionExtensions.MapHttpRoute(config.Routes, "GroupByGameIdApi", "api/{controller}/bygameid/{gameid}");
	}
}

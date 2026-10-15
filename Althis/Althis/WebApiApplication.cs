using System.IO;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using Althis.Models;

namespace Althis;

public class WebApiApplication : HttpApplication
{
	public Map mastermap;

	protected void Application_Start()
	{
		AreaRegistration.RegisterAllAreas();
		WebApiConfig.Register(GlobalConfiguration.Configuration);
		FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
		RouteConfig.RegisterRoutes(RouteTable.Routes);
		string[] files = Directory.GetFiles(Server.MapPath("\\Resources\\Maps\\"), "*.*", SearchOption.AllDirectories);
		string[] array = files;
		for (int i = 0; i < array.Length; i++)
		{
			_ = array[i];
		}
	}
}

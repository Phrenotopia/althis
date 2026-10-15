using System.Web.Http;
using System.Web.Mvc;
using Althis.Areas.HelpPage.Models;

namespace Althis.Areas.HelpPage.Controllers;

public class HelpController : Controller
{
	public HttpConfiguration Configuration { get; private set; }

	public HelpController()
		: this(GlobalConfiguration.Configuration)
	{
	}

	public HelpController(HttpConfiguration config)
	{
		Configuration = config;
	}

	public ActionResult Index()
	{
		return View(ServicesExtensions.GetApiExplorer(Configuration.Services).ApiDescriptions);
	}

	public ActionResult Api(string apiId)
	{
		if (!string.IsNullOrEmpty(apiId))
		{
			HelpPageApiModel helpPageApiModel = Configuration.GetHelpPageApiModel(apiId);
			if (helpPageApiModel != null)
			{
				return View(helpPageApiModel);
			}
		}
		return View("Error");
	}
}

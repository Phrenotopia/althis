using System.Web.Mvc;

namespace Althis.Controllers;

public class HomeController : Controller
{
	public ActionResult Index()
	{
		return View();
	}
}

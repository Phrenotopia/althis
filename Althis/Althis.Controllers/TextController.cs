using System.Web.Http;

namespace Althis.Controllers;

public class TextController : ApiController
{
	public string[] Get()
	{
		return new string[2] { "Echo", "Is alive! " };
	}
}

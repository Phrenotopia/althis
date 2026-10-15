using System.Collections.Generic;
using System.Web.Http;

namespace Althis.Controllers;

public class ValuesController : ApiController
{
	public IEnumerable<string> Get()
	{
		return new string[2] { "value1", "value2" };
	}

	public string Get(int id)
	{
		return "value";
	}

	public void Post([FromBody] string value)
	{
	}

	public void Put(int id, [FromBody] string value)
	{
	}

	public void Delete(int id)
	{
	}
}

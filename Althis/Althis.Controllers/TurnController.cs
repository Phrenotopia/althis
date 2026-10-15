using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Althis.Logic;
using Althis.Models;

namespace Althis.Controllers;

public class TurnController : ApiController
{
	public List<Turn> Get(string filter)
	{
		return TurnKeeper.Turns;
	}

	public Turn Get(int id)
	{
		return TurnKeeper.Turns.First((Turn g) => g.Id == id);
	}

	public Turn GetByGuid(Guid? guid)
	{
		IEnumerable<Turn> source = TurnKeeper.Turns.Where((Turn t) =>
		{
			Guid? guid2 = t.Guid;
			Guid? guid3 = guid;
			return guid2 == guid3;
		});
		if (source.Count() > 0)
		{
			return source.First();
		}
		return null;
	}

	public string Get(int id, int parm1, int parm2)
	{
		return "get turn: " + id + ":" + parm1 + "," + parm2;
	}

	public Turn GetNewTurn(int name)
	{
		return TurnKeeper.Next(name);
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

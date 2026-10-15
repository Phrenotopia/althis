using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Althis.Logic;
using Althis.Models;

namespace Althis.Controllers;

public class GameController : ApiController
{
	public List<Game> Get(string filter)
	{
		return GameHolder.Games;
	}

	public Game Get(int id)
	{
		return GameHolder.Games.First((Game g) => g.Id == id);
	}

	public Game GetByGuid(Guid? guid)
	{
		IEnumerable<Game> source = GameHolder.Games.Where((Game m) =>
		{
			Guid guid2 = m.Guid;
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
		return "get game: " + id + ":" + parm1 + "," + parm2;
	}

	public Game GetNewGame(string name)
	{
		return GameFactory.Create(name, HttpContext.Current.Server.MapPath("~/Resources/Maps/"));
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

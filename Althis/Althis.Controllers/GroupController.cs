using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Althis.Logic;
using Althis.Models;

namespace Althis.Controllers;

public class GroupController : ApiController
{
	public List<Group> Get(string filter)
	{
		return GroupHolder.Groups;
	}

	public List<Group> GetByMapId(int mapid)
	{
		if (mapid == 0)
		{
			return GroupHolder.Groups;
		}
		return GroupHolder.Groups.Where((Group g) => g.CurrentTile.MapId == mapid).ToList();
	}

	public List<Group> GetByGameId(int gameid)
	{
		if (gameid == 0)
		{
			return new List<Group>();
		}
		return GameHolder.Games.Where((Game g) => g.Id == gameid).FirstOrDefault().Groups;
	}

	public Group[] GetByTileId(long tileid)
	{
		return null;
	}

	public Group GetByGuid(Guid? guid)
	{
		IEnumerable<Group> source = GroupHolder.Groups.Where((Group m) =>
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
		return "got: " + id + ":" + parm1 + "," + parm2;
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

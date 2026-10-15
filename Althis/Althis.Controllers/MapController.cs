using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Althis.Logic;
using Althis.Models;

namespace Althis.Controllers;

public class MapController : ApiController
{
	public List<Map> Get()
	{
		return MapHolder.Maps;
	}

	public List<Map> Get(string filter)
	{
		return MapHolder.Maps;
	}

	public Map Get(int id)
	{
		return MapHolder.Maps.First((Map m) => m.Id == id);
	}

	public Map GetByGuid(Guid? guid)
	{
		IEnumerable<Map> source = MapHolder.Maps.Where((Map m) =>
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

	public List<Tile> Get(int id, int pos, int width, int height)
	{
		List<Tile> list = new List<Tile>();
		Map map = MapHolder.Maps.First((Map m) => m.Id == id);
		if (map == null)
		{
			map = MapHolder.Maps.First();
		}
		int xinc = 0;
		for (int num = 0; num < height; num++)
		{
			xinc = map.Width * num;
			list.AddRange(map.Tiles.Where((Tile t) => t.Id >= pos + xinc && t.Id <= pos + width - 1 + xinc));
		}
		return list;
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

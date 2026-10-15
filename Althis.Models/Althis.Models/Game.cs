using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Althis.Models;

public class Game
{
	public int Id { get; set; }

	public Guid Guid { get; set; }

	public string Name { get; set; }

	public int MapId { get; set; }

	[XmlIgnore]
	public Map Map { get; set; }

	public Turn CurrentTurn { get; set; }

	public List<Group> Groups { get; set; }

	public Game(Map m)
	{
		MapId = m.Id;
		Map = m;
	}

	public Game(Map m, int i)
	{
		MapId = m.Id;
		Map = m;
		Id = i;
		Name = m.Name + " " + i;
	}

	public void DoTurn()
	{
		foreach (Tile tile in Map.Tiles)
		{
			tile.PrimaryProduction();
		}
		foreach (Group group in Groups)
		{
			group.TakeTurn();
		}
	}
}

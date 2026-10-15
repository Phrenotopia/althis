using System.Collections.Generic;
using Althis.Models;

namespace Althis.Logic.Algorithm.Population;

public class Single : PopulationMethod
{
	public List<Group> Populate(Map m)
	{
		List<Group> list = new List<Group>();
		int x = m.Width / 2;
		int y = m.Height / 2;
		Group obj = new Group();
		obj.CurrentTile = m.GetTile(x, y);
		obj.Size = 100;
		AbilityHolder.GrantAbility(obj, "Hunting");
		list.Add(obj);
		return list;
	}

	public string GetName()
	{
		return "Single";
	}
}

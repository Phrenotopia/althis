using System;
using System.Collections.Generic;
using Althis.Logic.Algorithm.Population;
using Althis.Models;

namespace Althis.Logic;

public static class MapPopulator
{
	public static List<Group> PopulateMap(Map m)
	{
		return new Althis.Logic.Algorithm.Population.Single().Populate(m);
	}

	public static void PopulateMap(Map m, PopulationMethod pm)
	{
		throw new NotImplementedException();
	}
}

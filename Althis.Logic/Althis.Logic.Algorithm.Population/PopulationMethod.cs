using System.Collections.Generic;
using Althis.Models;

namespace Althis.Logic.Algorithm.Population;

public interface PopulationMethod
{
	List<Group> Populate(Map m);

	string GetName();
}

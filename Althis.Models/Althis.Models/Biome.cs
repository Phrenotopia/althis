using System;
using System.Collections.Generic;

namespace Althis.Models;

public class Biome : TileTrait
{
	public BiomeType Type { get; set; }

	public List<ResourceType> ResourceTypes { get; set; }

	public Biome()
	{
		Id = 2;
		Name = "Generic Biome";
		Type = BiomeType.Generic;
		ResourceTypes = new List<ResourceType>();
		ResourceTypes.Add(ResourceType.Fruitage);
		ResourceTypes.Add(ResourceType.WildGame);
		ResourceTypes.Add(ResourceType.Fruitage);
	}

	public override int GetPrimaryProduction()
	{
		throw new NotImplementedException();
	}
}

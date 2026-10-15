using System;
using System.Collections.Generic;
using System.Linq;

namespace Althis.Models;

public class Tile
{
	public int Id { get; set; }

	public long MapId { get; set; }

	public int LongitudeTenth { get; set; }

	public int LatitudeTenth { get; set; }

	public int Elevation { get; set; }

	public Biome Biome { get; set; }

	public List<TileTrait> Traits { get; set; }

	public List<Resource> Resources { get; set; }

	public Tile()
	{
	}

	public Tile(int id, int longtenth, int latitenth, long mapId)
	{
		Id = id;
		LongitudeTenth = longtenth;
		LatitudeTenth = latitenth;
		MapId = mapId;
		Traits = new List<TileTrait>();
		Resources = new List<Resource>();
	}

	public Tile Initialize()
	{
		if (Biome == null)
		{
			Biome = new Biome();
			GenerateResources();
		}
		return this;
	}

	private void GenerateResources()
	{
		if (Biome == null)
		{
			return;
		}
		foreach (ResourceType resourceType in Biome.ResourceTypes)
		{
			Resources.Add(new Resource(resourceType, biological: true, 100, 5000, this));
		}
	}

	internal void AddTrait(int id, int section, byte value)
	{
		switch (section)
		{
		case 1:
			Elevation = Convert.ToInt32(value);
			Traits.Add(new TileTrait
			{
				Id = id,
				Name = "Elevation",
				Value = Convert.ToInt32(value)
			});
			break;
		case 2:
			break;
		}
	}

	public void PrimaryProduction()
	{
		int amount = 500;
		IEnumerable<Resource> enumerable = Resources.Where((Resource r) => r.Biological);
		foreach (Resource item in enumerable)
		{
			item.Produce(amount);
		}
	}

	public int GetCarryingCapacity()
	{
		return 500;
	}
}

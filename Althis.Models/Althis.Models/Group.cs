using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace Althis.Models;

public class Group
{
	public int Id { get; set; }

	public Guid Guid { get; set; }

	public int Size { get; set; }

	public Tile CurrentTile { get; set; }

	public int Subsistence { get; set; }

	public Satiation Satiation { get; set; }

	public decimal MortalityRate { get; set; }

	public decimal BirthRate { get; set; }

	[XmlIgnore]
	public List<Ability> Abilities { get; set; }

	[XmlIgnore]
	public List<Biome> BiomeKnowledge { get; set; }

	[XmlIgnore]
	public List<Tile> TileKnowledge { get; set; }

	[XmlIgnore]
	public List<Activity> Actions { get; set; }

	public Group()
	{
		Guid = Guid.NewGuid();
		Abilities = new List<Ability>();
		BiomeKnowledge = new List<Biome>();
		TileKnowledge = new List<Tile>();
		Actions = new List<Activity>();
		MortalityRate = 20m;
		BirthRate = 24m;
	}

	public void TakeTurn()
	{
		Subsist();
		Reproduce();
		Act();
	}

	public void Subsist()
	{
		BiomeKnowledge.Contains(CurrentTile.Biome);
		TileKnowledge.Contains(CurrentTile);
		foreach (Ability a in Abilities)
		{
			List<Resource> resources = CurrentTile.Resources;
			Func<Resource, bool> predicate = (Resource r) => r.Type == a.ResourceType;
			foreach (Resource item in resources.Where(predicate))
			{
				int size = Size;
				item.Consume(size);
				if (item.Quantity > 0)
				{
					Subsistence += size;
				}
				else
				{
					item.Quantity = 0;
				}
			}
		}
		switch (Subsistence / Size)
		{
		case 0:
			Satiation = Satiation.starving;
			break;
		case 1:
			Satiation = Satiation.hungry;
			break;
		case 2:
			Satiation = Satiation.sated;
			break;
		default:
			Satiation = Satiation.gorged;
			break;
		}
	}

	public void Reproduce()
	{
		double growthRate = GetGrowthRate();
		int carryingCapacity = GetCarryingCapacity();
		int size = Size;
		Size += Convert.ToInt32(growthRate * (double)size * (double)(1 / (1 + size / carryingCapacity)));
	}

	private double GetGrowthRate()
	{
		return 0.1;
	}

	private int Death()
	{
		return (int)Math.Round(MortalityRate / 1000m * (decimal)Size);
	}

	private int GetCarryingCapacity()
	{
		return CurrentTile.GetCarryingCapacity();
	}

	public void Act()
	{
	}
}

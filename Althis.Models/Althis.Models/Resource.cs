using System;

namespace Althis.Models;

public class Resource
{
	private ResourceType rt;

	public int Id { get; set; }

	public bool Biological { get; set; }

	public ResourceType Type { get; set; }

	public int Quantity { get; set; }

	public Tile HomeTile { get; set; }

	public int TileCarryingCapacity { get; set; }

	public Resource(ResourceType type, bool biological, int initialquantity)
	{
		Type = type;
		Biological = biological;
		Quantity = initialquantity;
	}

	public Resource(ResourceType type, bool biological, int initialquantity, int carryingCapacity)
	{
		Type = type;
		Biological = biological;
		Quantity = initialquantity;
		TileCarryingCapacity = carryingCapacity;
	}

	public Resource(ResourceType type, bool biological, int initialquantity, int carryingCapacity, Tile tile)
	{
		Type = type;
		Biological = biological;
		Quantity = initialquantity;
		HomeTile = tile;
		TileCarryingCapacity = carryingCapacity;
	}

	public void Produce(int amount)
	{
		Quantity += amount;
		double num = 0.2;
		int tileCarryingCapacity = TileCarryingCapacity;
		int quantity = Quantity;
		Quantity += Convert.ToInt32(num * (double)quantity * (double)(1 / (1 + quantity / tileCarryingCapacity)));
	}

	public void Consume(int amount)
	{
		Quantity -= amount;
	}
}

using System;

namespace Althis.Models;

public class Ability
{
	private Ability ability;

	public int Id { get; set; }

	public Guid Guid { get; set; }

	public string Name { get; set; }

	public AbilityType Type { get; set; }

	public ResourceType ResourceType { get; set; }

	public int Experience { get; set; }

	public Ability()
	{
	}

	public Ability(Ability ability, int experience)
	{
		Id = ability.Id;
		Guid = Guid.NewGuid();
		Name = ability.Name;
		Type = ability.Type;
		ResourceType = ability.ResourceType;
		Experience = experience;
	}
}

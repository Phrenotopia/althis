using System.Collections.Generic;
using System.Linq;
using Althis.Models;

namespace Althis.Logic;

public static class AbilityHolder
{
	public static List<Ability> Abilities { get; set; }

	public static void Initialize()
	{
		Abilities = new List<Ability>();
		Ability ability = new Ability();
		ability.Id = 1;
		ability.Name = "Gathering";
		ability.Type = AbilityType.Strategy;
		ability.ResourceType = ResourceType.Fruitage;
		Ability item = ability;
		Ability ability2 = new Ability();
		ability2.Id = 1;
		ability2.Name = "Hunting";
		ability2.Type = AbilityType.Strategy;
		ability2.ResourceType = ResourceType.WildGame;
		Ability item2 = ability2;
		Ability ability3 = new Ability();
		ability3.Id = 2;
		ability3.Name = "Herding";
		ability3.Type = AbilityType.Strategy;
		ability3.ResourceType = ResourceType.Plants;
		Ability item3 = ability3;
		Abilities.Add(item);
		Abilities.Add(item2);
		Abilities.Add(item3);
	}

	public static void GrantAbility(Group g, string abilityName)
	{
		GrantAbility(g, abilityName, 1);
	}

	public static void GrantAbility(Group g, string abilityName, int experience)
	{
		Ability ability = null;
		IEnumerable<Ability> source = Abilities.Where((Ability a) => a.Name == abilityName);
		if (source.Count() > 0)
		{
			ability = source.FirstOrDefault();
		}
		if (ability != null)
		{
			g.Abilities.Add(new Ability(ability, experience));
		}
	}
}

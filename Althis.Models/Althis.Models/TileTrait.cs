using System;

namespace Althis.Models;

public class TileTrait
{
	public int Id;

	public string Name { get; set; }

	public int Value { get; set; }

	public virtual int GetPrimaryProduction()
	{
		throw new NotImplementedException();
	}
}

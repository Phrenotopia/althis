using System;

namespace Althis.Models;

public class Elevation : TileTrait
{
	public Elevation()
	{
		Id = 1;
		Name = "Elevation";
	}

	public override int GetPrimaryProduction()
	{
		throw new NotImplementedException();
	}
}

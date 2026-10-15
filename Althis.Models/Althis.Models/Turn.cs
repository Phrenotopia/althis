using System;

namespace Althis.Models;

public class Turn
{
	public int Id { get; set; }

	public Guid? Guid { get; set; }

	public int Number { get; set; }

	public int GameId { get; set; }

	public Guid GameGuid { get; set; }
}

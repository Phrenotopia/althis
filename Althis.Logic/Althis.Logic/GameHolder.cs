using System.Collections.Generic;
using System.Linq;
using Althis.Models;

namespace Althis.Logic;

public static class GameHolder
{
	public static List<Game> Games = new List<Game>();

	internal static int GetNewGameId()
	{
		Game game = Games.LastOrDefault();
		if (game == null)
		{
			return 1;
		}
		return game.Id + 1;
	}

	internal static Game GetGame(int gameid)
	{
		return Games.Where((Game g) => g.Id == gameid).FirstOrDefault();
	}
}

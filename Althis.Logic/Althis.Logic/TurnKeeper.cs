using System.Collections.Generic;
using System.Linq;
using Althis.Models;

namespace Althis.Logic;

public static class TurnKeeper
{
	public static List<Turn> Turns = new List<Turn>();

	public static Turn Next(int gameid)
	{
		Turn turn = new Turn();
		turn.GameId = gameid;
		int id = 1;
		Turn turn2 = Turns.LastOrDefault();
		if (turn2 != null)
		{
			id = turn2.Id + 1;
		}
		Turn turn3 = Turns.Where((Turn t) => t.GameId == gameid).LastOrDefault();
		if (turn3 == null)
		{
			turn3 = new Turn();
			turn3.GameId = gameid;
			turn3.Number = 0;
			Turns.Add(turn3);
		}
		int number = turn3.Number + 1;
		turn.Id = id;
		turn.Number = number;
		Turns.Add(turn);
		Game game = GameHolder.GetGame(gameid);
		game.CurrentTurn = turn;
		game.DoTurn();
		return turn;
	}
}

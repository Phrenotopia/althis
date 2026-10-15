using Althis.Models;

namespace Althis.Logic;

public static class GameFactory
{
	public static Game Create(string mapname, string filepath)
	{
		AbilityHolder.Initialize();
		return CreateGame(new MapFactory().Create(mapname, filepath));
	}

	private static Game CreateGame(Map map)
	{
		int newGameId = GameHolder.GetNewGameId();
		Game game = new Game(map, newGameId);
		game.Groups = MapPopulator.PopulateMap(map);
		GameHolder.Games.Add(game);
		return game;
	}
}

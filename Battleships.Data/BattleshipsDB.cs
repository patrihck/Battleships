using System.Collections.Concurrent;
using Battleships.Data.Entity;

namespace Battleships.Data;

public class BattleshipsDB
{
    private static ConcurrentDictionary<string, Game> _games = new ConcurrentDictionary<string, Game>();

    public void AddGame(Game game)
    {
        _games.TryAdd(game.Id, game);
    }

    public Game? GetGame(string id)
    {
        if (!_games.TryGetValue(id.ToString(), out Game game))
        {
            return null;
        }

        return game;
    }

}
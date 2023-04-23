using Battleships.Business.Model;
using Battleships.Business.Model.Request;
using Battleships.Business.Model.Response;
using Battleships.Data;
using Battleships.Data.Entity;

namespace Battleships.Business.Logic;

public class GameService
{
    private readonly BattleshipsDB _db = new BattleshipsDB();
    private readonly ShipGenerator _shipGenerator = new ShipGenerator();
    
    public MoveResponse MakeMove(MoveRequest req)
    {
        var game = _db.GetGame(req.GameId);

        if (game == null)
        {
            return new MoveResponse(true) { ErrorMessage = "Game not found" };
        }

        game.Turns++;
        
        foreach (var ship in game.Ships)
        {
            if (CheckForHit(ship, req.Row, req.Column))
            {
                ship.Sunken = true;
                return new MoveResponse(false) { Result = "Hit", ShipsSunkenCount = game.Ships.Where(s => s.Sunken).Count(), Turns = game.Turns };
            }
        }
        

        return new MoveResponse(false) { Result = "Miss", ShipsSunkenCount = game.Ships.Where(s => s.Sunken).Count(), Turns = game.Turns };
    }

    public Game CreateGame(int shipsCount = 3)
    {
        var newGame = new Game();
        newGame.Id = Guid.NewGuid().ToString();
        Random random = new Random();
        Dictionary<ShipType, int> shipCounts = new Dictionary<ShipType, int> { { ShipType.Destroyer, 2 }, { ShipType.Battleship, 1 } };
        int gridSize = 10;

        List<Ship> ships = _shipGenerator.GenerateNonOverlappingShips(shipCounts, gridSize);
        newGame.Ships = ships;
        newGame.Turns = 0;

        _db.AddGame(newGame);
        return newGame;
    }
    
    private static bool CheckForHit(Ship ship, int row, string column)
    {
        int shipLength = (int)ship.Type;
        int shipColumn = ship.Column[0] - 'A';
        int inputColumn = column[0] - 'A';

        if (ship.Orientation == ShipOrientation.Horizontal)
        {
            return ship.Row == row && inputColumn >= shipColumn && inputColumn <= (shipColumn + shipLength - 1);
        }
        else
        {
            return shipColumn == inputColumn && row >= ship.Row && row <= (ship.Row + shipLength - 1);
        }
    }

}
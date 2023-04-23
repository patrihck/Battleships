using Battleships.Data.Entity;

namespace Battleships.Business.Logic;

public class ShipGenerator
{
    private static readonly Random random = new Random();

    public List<Ship> GenerateNonOverlappingShips(Dictionary<ShipType, int> shipCounts, int gridSize = 10)
    {
        List<Ship> ships = new List<Ship>();

        try
        {
            foreach (KeyValuePair<ShipType, int> shipCount in shipCounts)
            {
                for (int i = 0; i < shipCount.Value; i++)
                {
                    Ship newShip;
                    bool isOverlapping;

                    int retryCount = 0;
                    const int maxRetries = 100;

                    do
                    {
                        newShip = GenerateRandomShip(gridSize, (int)shipCount.Key);
                        newShip.Type = shipCount.Key;
                        isOverlapping = false;

                        foreach (Ship existingShip in ships)
                        {
                            if (IsOverlapping(existingShip, newShip, (int)existingShip.Type, (int)shipCount.Key))
                            {
                                isOverlapping = true;
                                break;
                            }
                        }

                        retryCount++;

                        if (retryCount >= maxRetries)
                        {
                            Console.WriteLine($"Failed to generate a non-overlapping ship of type {shipCount.Key} after {maxRetries} retries.");
                            break;
                        }

                    } while (isOverlapping);

                    if (retryCount < maxRetries)
                    {
                        ships.Add(newShip);
                    }
                    else
                    {
                        Console.WriteLine($"Skipping ship of type {shipCount.Key} due to too many retries.");
                    }
                }
            }

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        
        

        return ships;
    }

    private static Ship GenerateRandomShip(int gridSize = 10, int shipLength = 4)
    {
        Ship ship = new Ship();
        ship.Orientation = (ShipOrientation)random.Next(0, 2);

        if (ship.Orientation == ShipOrientation.Vertical)
        {
            ship.Row = random.Next(1, gridSize - shipLength + 2);
            ship.Column = ConvertToLetter(random.Next(0, gridSize));
        }
        else
        {
            ship.Row = random.Next(1, gridSize + 1);
            ship.Column = ConvertToLetter(random.Next(0, gridSize - shipLength + 1));
        }

        return ship;
    }


    private static string ConvertToLetter(int number)
    {
        return ((char)(number + 'A')).ToString();
    }
    

    private static bool IsOverlapping(Ship ship1, Ship ship2, int shipLength1, int shipLength2)
    {
        int ship1StartRow = ship1.Row;
        int ship1StartColumn = ship1.Column[0] - 'A';
        int ship1EndRow = ship1.Orientation == ShipOrientation.Vertical ? ship1.Row + shipLength1 - 1 : ship1.Row;
        int ship1EndColumn = ship1.Orientation == ShipOrientation.Horizontal ? ship1.Column[0] - 'A' + shipLength1 - 1 : ship1.Column[0] - 'A';

        int ship2StartRow = ship2.Row;
        int ship2StartColumn = ship2.Column[0] - 'A';
        int ship2EndRow = ship2.Orientation == ShipOrientation.Vertical ? ship2.Row + shipLength2 - 1 : ship2.Row;
        int ship2EndColumn = ship2.Orientation == ShipOrientation.Horizontal ? ship2.Column[0] - 'A' + shipLength2 - 1 : ship2.Column[0] - 'A';

        bool noVerticalOverlap = ship1EndRow < ship2StartRow - 1 || ship2EndRow < ship1StartRow - 1;
        bool noHorizontalOverlap = ship1EndColumn < ship2StartColumn - 1 || ship2EndColumn < ship1StartColumn - 1;

        return !(noVerticalOverlap || noHorizontalOverlap);
    }

}

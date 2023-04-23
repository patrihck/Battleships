namespace Battleships.Data.Entity;

public class Game
{
    public string Id { get; set; }
    public IEnumerable<Ship> Ships { get; set; }
    public int Turns { get; set; }
}
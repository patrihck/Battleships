namespace Battleships.Data.Entity;

public class Ship
{
    public int Row { get; set; }
    public string Column { get; set; }
    public ShipOrientation Orientation { get; set; }
    public ShipType Type { get; set; }
    public bool Sunken { get; set; }
}
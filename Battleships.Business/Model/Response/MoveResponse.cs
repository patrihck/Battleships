namespace Battleships.Business.Model.Response;

public record MoveResponse(bool ErrorOccured)
{
    public string Result { get; set; }
    public int ShipsSunkenCount { get; set; }
    public int Turns { get; set; }
    public string ErrorMessage { get; set; }
}
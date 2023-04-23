namespace Battleships.Business.Model.Request;

public record MoveRequest(int Row, string Column, string GameId);
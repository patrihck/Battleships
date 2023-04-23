using BattleshipApi.Controllers.DTO;
using Battleships.Business.Logic;
using Battleships.Business.Model.Request;
using Battleships.Business.Model.Response;
using Battleships.Data.Entity;
using Microsoft.AspNetCore.Mvc;

namespace Battleships.Controllers
{
    [ApiController]
    [Route("api/game")]
    public class GameController : ControllerBase
    {
        private readonly GameService _gameService = new GameService();

        [HttpGet]
        public ActionResult<Game> CreateGame()
        {
            return _gameService.CreateGame();
        }

        [HttpPut("{id}/move")]
        public ActionResult<MoveResponse> MakeMove(string id, [FromBody] MoveRequestDto request)
        {
            var result = _gameService.MakeMove(new MoveRequest(request.Row, request.Column, id));

            if (result.ErrorOccured)
            {
                if (result.ErrorMessage == "Game not found")
                {
                    return NotFound(result.ErrorMessage);
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, result.ErrorMessage);
                }
            }

            return result;
        }
    }
}
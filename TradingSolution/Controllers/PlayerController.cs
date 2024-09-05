using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using TradingSolution.Interfaces;
using TradingSolution.Models;
using TradingSolution.Services;

namespace TradingSolution.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayerController : ControllerBase
    {
        private static readonly Dictionary<int, IPlayer> s_players = PlayerDB.Instance.Players;

        [HttpGet("number/{number}")]
        public ActionResult<IPlayer> GetPlayer(int number)
        {
            try
            {
                IPlayer player = PlayerManager.GetPlayer(number);
                return Ok(player);
            }
            catch (KeyNotFoundException ex)
            {
                
                return NotFound(ex.Message);
            }
        }

        [HttpGet]
        public ActionResult<IEnumerable<(int, IPlayer)>> GetAllPlayers()
        {
            return Ok(s_players);
        }

        [HttpPost]
        public ActionResult<IPlayer> CreatePlayer([FromBody] NFLPlayer player)
        {
            if (player == null)
            {
                return BadRequest("Player is null");
            }
            try
            {
                PlayerManager.CreatePlayer(player);
                return CreatedAtAction(nameof(GetPlayer), new { number = player.Name }, player);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}

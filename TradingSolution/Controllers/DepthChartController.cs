using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TradingSolution.Helpers;
using TradingSolution.Interfaces;
using TradingSolution.Models;
using TradingSolution.Models.DTOs;
using TradingSolution.Services;

namespace TradingSolution.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepthChartController : ControllerBase
    {
        // Simulate db with in-memory list 
        private static readonly Dictionary<NFLPosition, LinkedList<(IPlayer Player, int Depth)>> s_chart = DepthChart.Instance.Chart;


        [HttpGet]
        public ActionResult<Dictionary<string, List<PlayerDepthDto>>> GetAllPlayers()
        {
            return Ok(EasySerialization.PlayerChartConvert(s_chart));
        }

        [HttpPost]
        public ActionResult<IPlayer> AddPlayerToDepthChart([FromBody] AddPlayerToDepthChartDto dto)
        {
            try
            {
                DepthChartManager.AddPlayerToDepthChart(dto.Position, dto.NFLPlayer, dto.Depth);

                return CreatedAtAction(null, dto.NFLPlayer);
            }
            catch (ArgumentException ex)
            {

                return BadRequest(ex.Message);
            }

        }

        [HttpPost("get-backups")]
        public ActionResult<List<IPlayer?>> GetBackups(GetBackupsDto dto)
        {
            return Ok(DepthChartManager.GetBackups(dto.Position, dto.Player));
        }

        [HttpDelete]
        public ActionResult<List<IPlayer?>> DeletePlayerFromChart([FromBody] DeletePlayerFromChartDto dto)
        {

            return Ok(DepthChartManager.RemovePlayerFromDepthChart(dto.Position, dto.Player));
        }
    }
}

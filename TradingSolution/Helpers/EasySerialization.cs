using System;
using TradingSolution.Interfaces;
using TradingSolution.Models;
using TradingSolution.Models.DTOs;

namespace TradingSolution.Helpers;

public class EasySerialization
{
    public static Dictionary<string, List<PlayerDepthDto>> PlayerChartConvert(Dictionary<NFLPosition, LinkedList<(IPlayer, int)>> chart)
    {
        var result = new Dictionary<string, List<PlayerDepthDto>>();

        foreach (var kvp in chart)
        {
            var position = kvp.Key.ToString();
            var players = kvp.Value;

            var playerList = new List<PlayerDepthDto>();
            foreach (var (player, depth) in players)
            {
                playerList.Add(new PlayerDepthDto
                {
                    Name = player.Name,
                    Number = player.Number,
                    Depth = depth
                });
            }   
            result[position] = playerList;
        }

        return result;
    }
}

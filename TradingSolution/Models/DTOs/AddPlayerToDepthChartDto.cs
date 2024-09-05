using System;
namespace TradingSolution.Models.DTOs;

public class AddPlayerToDepthChartDto
{
    public required NFLPosition Position { get; set; }
    public required NFLPlayer NFLPlayer { get; set; }
    public int? Depth { get; set; }
}

using System;
using TradingSolution.Interfaces;

namespace TradingSolution.Models.DTOs;

public class DeletePlayerFromChartDto
{
    public required NFLPosition Position { get; set; }
    public required NFLPlayer Player { get; set; }


}

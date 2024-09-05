using System;

namespace TradingSolution.Models.DTOs;

public class GetBackupsDto
{
    public required NFLPosition Position { get; set; }
    public required NFLPlayer Player { get; set; }
}

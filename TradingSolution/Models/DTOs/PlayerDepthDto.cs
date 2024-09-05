using System;

namespace TradingSolution.Models.DTOs;

public class PlayerDepthDto
{
    public required string Name { get; set; }
    public int Number { get; set; }
    public int Depth { get; set; }
}

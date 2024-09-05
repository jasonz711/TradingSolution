using System;
using TradingSolution.Models;

namespace TradingSolution.Interfaces;

public interface IPlayer
{

    string Name { get; set; }
    int Number { get; set; }
    Team? Team { get; set; }

}

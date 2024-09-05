using System;
using TradingSolution.Interfaces;

namespace TradingSolution.Models;

public class NFLPlayer(string name, int number, Team? team) : IPlayer
{
    public string Name { get; set;} = name;
    public int Number { get; set; } = number;
    public Team? Team { get; set; } = team;

    public override bool Equals(object? obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        NFLPlayer other = (NFLPlayer)obj;
        return Name == other.Name && Number == other.Number;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Name, Number);
    }
}

using System;
using TradingSolution.Interfaces;

namespace TradingSolution.Services;
// Simulate a database with an in-memory list
public class PlayerDB
{
    private static readonly PlayerDB s_instance = new([]);
    public Dictionary<int, IPlayer> Players { get; set; }
    static PlayerDB()
    {

    }
    private PlayerDB(Dictionary<int, IPlayer> players)
    {
        Players = players;
    }

    public static PlayerDB Instance
    {
        get
        {
            return s_instance;
        }
    }
}

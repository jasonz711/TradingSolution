using System;
using TradingSolution.Interfaces;
using TradingSolution.Models;

namespace TradingSolution.Services;
// Simulate a database with an in-memory list
public class DepthChart
{
    private static readonly DepthChart s_instance = new([]);
    // Dictionary to store players by position with a list sorted by depth position
    public Dictionary<NFLPosition, LinkedList<(IPlayer Player, int Depth)>> Chart { get; set; }

    static DepthChart()
    {

    }

    private DepthChart(Dictionary<NFLPosition, LinkedList<(IPlayer Player, int Depth)>> chart)
    {
        Chart = chart;
    }

    public static DepthChart Instance
    {
        get
        {
            return s_instance;
        }
    }
}

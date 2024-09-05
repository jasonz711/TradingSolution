using System;
using TradingSolution.Interfaces;
using TradingSolution.Models;

namespace TradingSolution.Services;

public class DepthChartManager
{
    private static readonly Dictionary<NFLPosition, LinkedList<(IPlayer Player, int Depth)>> s_chart = DepthChart.Instance.Chart;
    public static void AddPlayerToDepthChart(NFLPosition position, IPlayer player, int? positionDepth)
    {
        // Try to create this player first
        if (PlayerDB.Instance.Players.TryGetValue(player.Number, out IPlayer? foundPlayer))
        {
            if (!foundPlayer.Equals(player))
            {
                throw new ArgumentException("Adding invalid player due to duplicated number");
            }
        }
        else
        {
            PlayerManager.CreatePlayer(player);
        }

        var chartData = s_chart;

        if (!chartData.ContainsKey(position))
        {
            s_chart[position] = new LinkedList<(IPlayer, int)>();
            chartData[position] = new LinkedList<(IPlayer, int)>();
        }

        foreach (var tempPlayer in chartData[position])
        {
            if (tempPlayer.Player.Equals(player))
            {
                throw new ArgumentException("Player exists");
            }
        }

        positionDepth ??= (s_chart[position].Last == null ? 0: s_chart[position].Last().Depth + 1);

        var players = s_chart[position];
        var insertPoint = FindInsertPoint(players, (int)positionDepth);

        InsertPlayer(players, insertPoint, player, (int)positionDepth);
        AdjustPlayerDepth(players, insertPoint);
    }

    public static List<IPlayer?> RemovePlayerFromDepthChart(NFLPosition position, IPlayer player)
    {
        var result = new List<IPlayer?>();
        if (s_chart.ContainsKey(position))
        {
            var players = s_chart[position];
            var node = players.First;
            while (node != null)
            {
                if (node.Value.Player.Equals(player))
                {
                    players.Remove(node);
                    result.Add(player);
                    break;
                }
                node = node.Next;
            }
        }
        return result;
    }
    public static List<IPlayer?> GetBackups(NFLPosition position, IPlayer player)
    {
        var result = new List<IPlayer?>();
        if (s_chart.ContainsKey(position))
        {
            var players = s_chart[position];
            var node = players.First;
            var startAdding = false;
            while (node != null)
            {
                if (startAdding)
                {
                    result.Add(node.Value.Player);
                }

                if (!startAdding && node.Value.Player.Equals(player))
                {
                    startAdding = true;
                }
                node = node.Next;
            }
        }
        return result;
    }
    public static Dictionary<NFLPosition, LinkedList<(IPlayer Player, int Depth)>> GetFullDepthChart()
    {

        return s_chart;

    }

    private static LinkedListNode<(IPlayer Player, int Depth)>? FindInsertPoint(LinkedList<(IPlayer Player, int Depth)> players, int positionDepth)
    {
        var currentNode = players.First;

        while (currentNode != null && currentNode.Value.Depth < positionDepth)
        {
            currentNode = currentNode.Next;
        }

        return currentNode;
    }

    private static void InsertPlayer(LinkedList<(IPlayer Player, int Depth)> players, LinkedListNode<(IPlayer Player, int Depth)>? insertPoint, IPlayer player, int depth)
    {
        if (insertPoint == null)
        {
            players.AddLast((player, depth));
        }
        else
        {
            players.AddBefore(insertPoint, (player, depth));
        }
    }

    private static void AdjustPlayerDepth(LinkedList<(IPlayer Player, int Depth)> players, LinkedListNode<(IPlayer Player, int Depth)>? insertPoint)
    {
        while (insertPoint != null)
        {
            // Move the depth position to 1 behind
            insertPoint.Value = (insertPoint.Value.Player, insertPoint.Value.Depth + 1);
            insertPoint = insertPoint.Next;
        }
    }
}

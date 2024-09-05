using System;
using TradingSolution.Interfaces;

namespace TradingSolution.Services;

public class PlayerManager
{
    private static readonly Dictionary<int, IPlayer> s_players = PlayerDB.Instance.Players;
    public static void CreatePlayer(IPlayer player)
    {
        if (s_players.ContainsKey(player.Number))
        {
            throw new ArgumentException("Player number exists");
        }
        s_players[player.Number] = player;

    }

    public static IPlayer GetPlayer(int number)
    {
        if (s_players.TryGetValue(number, out IPlayer? player))
        {
            return player;
        }
        else
        {
            throw new KeyNotFoundException("Player number not exists");
        }
    }
}

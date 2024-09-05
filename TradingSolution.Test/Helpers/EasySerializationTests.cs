using System;
using TradingSolution.Helpers;
using TradingSolution.Interfaces;
using TradingSolution.Models;
using TradingSolution.Models.DTOs;
using TradingSolution.Services;

namespace TradingSolution.Test.Helpers;

public class EasySerializationTests
{
    [Fact]
    public void PlayerChartConvert_ValidChart_ReturnsCorrectDictionary()
    {
        // Arrange
        var player1 = new NFLPlayer("Ash", 1, null);
        var player2 = new NFLPlayer("Bob", 2, null);

        var chart = new Dictionary<NFLPosition, LinkedList<(IPlayer, int)>>()
        {
            {
                NFLPosition.LG,
                new LinkedList<(IPlayer, int)>(new[] { ((IPlayer)player1, 1), (player2, 2) })
            }
        };

        // Expected result
        var expected = new Dictionary<string, List<PlayerDepthDto>>()
        {
            {
                "LG",
                new List<PlayerDepthDto>()
                {
                    new PlayerDepthDto { Name = "Ash", Number = 1, Depth = 1 },
                    new PlayerDepthDto { Name = "Bob", Number = 2, Depth = 2 }
                }
            }
        };

        // Act
        var result = EasySerialization.PlayerChartConvert(chart);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expected.Count, result.Count);
        Assert.True(result.ContainsKey("LG"));
        Assert.Equal(expected["LG"].Count, result["LG"].Count);

        for (int i = 0; i < expected["LG"].Count; i++)
        {
            Assert.Equal(expected["LG"][i].Name, result["LG"][i].Name);
            Assert.Equal(expected["LG"][i].Number, result["LG"][i].Number);
            Assert.Equal(expected["LG"][i].Depth, result["LG"][i].Depth);
        }
    }

    [Fact]
    public void PlayerChartConvert_EmptyChart_ReturnsEmptyDictionary()
    {
        // Arrange
        var chart = new Dictionary<NFLPosition, LinkedList<(IPlayer, int)>>();

        // Act
        var result = EasySerialization.PlayerChartConvert(chart);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }
}

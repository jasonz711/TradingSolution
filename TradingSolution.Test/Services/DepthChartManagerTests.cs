using System;
using Moq;
using TradingSolution.Interfaces;
using TradingSolution.Models;
using TradingSolution.Services;

namespace TradingSolution.Test.Services;

[Collection("DepthChartTests")]
public class DepthChartTests
{
    private Dictionary<NFLPosition, LinkedList<(IPlayer Player, int Depth)>> _chart = DepthChart.Instance.Chart;
    private NFLPosition _position = NFLPosition.C;
    private NFLPlayer _player1 = new NFLPlayer("Ash", 1, null);
    private NFLPlayer _player2 = new NFLPlayer("Bob", 2, null);
    private NFLPlayer _player3 = new NFLPlayer("Chris", 3, null);
    [Fact]
    public void AddPlayerToDepthChart_AddsPlayerAtCorrectDepth()
    {
        // Arrange
        _chart.Clear();
        _chart[_position] = new();
        _chart[_position].AddFirst((_player1, 1));

        // Act
        DepthChartManager.AddPlayerToDepthChart(_position, _player2, 2);
        DepthChartManager.AddPlayerToDepthChart(_position, _player3, null); // Adds to the end

        // Assert
        var players = DepthChart.Instance.Chart[_position];
        Assert.Equal(3, players.Count);
        Assert.NotNull(players.First);
        Assert.Equal(_player1, players.First.Value.Player); // Ash at position 1
        Assert.Equal(1, players.First.Value.Depth);
        Assert.Equal(_player2, players.Skip(1).FirstOrDefault().Player); // Bob at position 2
        Assert.Equal(2, players.Skip(1).FirstOrDefault().Depth);
        Assert.Equal(_player3, players.Skip(2).FirstOrDefault().Player); // Chris added to the end
        Assert.Equal(3, players.Skip(2).FirstOrDefault().Depth);
    }

    [Fact]
    public void AddPlayerToDepthChart_InsertsPlayerAndShiftsOthers()
    {
        // Arrange
        _chart.Clear();
        _chart[_position] = new();
        _chart[_position].AddFirst((_player1, 1));
        _chart[_position].AddLast((_player2, 2));

        // Act
        DepthChartManager.AddPlayerToDepthChart(_position, _player3, 1); // Should push the others down

        // Assert
        var players = DepthChart.Instance.Chart[_position];
        Assert.Equal(3, players.Count);
        Assert.NotNull(players.First);
        Assert.Equal(_player3, players.First.Value.Player); // Chris at position 1
        Assert.Equal(1, players.First.Value.Depth);
        Assert.Equal(_player1, players.Skip(1).FirstOrDefault().Player); // Ash pushed to position 2
        Assert.Equal(2, players.Skip(1).FirstOrDefault().Depth);
        Assert.Equal(_player2, players.Skip(2).FirstOrDefault().Player); // Bob pushed to position 3
        Assert.Equal(3, players.Skip(2).FirstOrDefault().Depth);
    }

    [Fact]
    public void AddPlayerToDepthChart_ShouldThrowArgumentException_WhenAddingDifferentPlayerWithSameNumber()
    {
        // Arrange
        _chart.Clear();
        _chart[_position] = new();
        _chart[_position].AddFirst((_player1, 1));

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => DepthChartManager.AddPlayerToDepthChart(_position, new NFLPlayer("Bob", 1, null), 2));
        Assert.Equal("Adding invalid player due to duplicated number", exception.Message);
    }

    [Fact]
    public void AddPlayerToDepthChart_ShouldThrowArgumentException_WhenPlayerExists()
    {
        // Arrange
        _chart.Clear();
        _chart[_position] = new();
        _chart[_position].AddFirst((_player1, 1));

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => DepthChartManager.AddPlayerToDepthChart(_position, _player1, 2));
        Assert.Equal("Player exists", exception.Message);
    }

    [Fact]
    public void RemovePlayerFromDepthChart_PlayerExists_RemovesPlayerAndReturnsPlayer()
    {
        // Arrange
        _chart.Clear();
        _chart[_position] = new();
        _chart[_position].AddFirst((_player1, 1));

        // Act
        var result = DepthChartManager.RemovePlayerFromDepthChart(_position, _player1);

        // Assert
        Assert.Single(result);
        Assert.Equal(_player1, result[0]);

        // Check that the player is no longer in the depth chart
        Assert.Empty(DepthChart.Instance.Chart[_position]);
    }

    [Fact]
    public void RemovePlayerFromDepthChart_PlayerDoesNotExist_ReturnsEmptyList()
    {
        // Arrange
        _chart.Clear();
        _chart[_position] = new();
        _chart[_position].AddFirst((_player1, 1));

        // Act
        var result = DepthChartManager.RemovePlayerFromDepthChart(_position, _player2);

        // Assert
        Assert.Empty(result); 
        Assert.Single(DepthChart.Instance.Chart[_position]); // The player should still be in the chart
        Assert.Equal(_player1, DepthChart.Instance.Chart[_position].First!.Value.Player);
    }

    [Fact]
    public void GetBackups_HaveBackups()
    {
        // Arrange
        _chart.Clear();
        _chart[_position] = new();
        _chart[_position].AddFirst((_player1, 1));
        _chart[_position].AddLast((_player2, 2));
        _chart[_position].AddLast((_player3, 3));

        // Act
        var result = DepthChartManager.GetBackups(_position, _player1);

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Equal(_player2, result[0]);
        Assert.Equal(_player3, result[1]);
    }

    [Fact]
    public void GetBackups_NoBackups()
    {
        // Arrange
        _chart.Clear();
        _chart[_position] = new();
        _chart[_position].AddFirst((_player1, 1));
        _chart[_position].AddLast((_player2, 2));
        _chart[_position].AddLast((_player3, 3));

        // Act
        var result = DepthChartManager.GetBackups(_position, _player3);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void GetFullDepthChart_ReturnsCorrectChart()
    {

        // Add players to the depth chart
        var players = DepthChartManager.GetFullDepthChart();
        players[_position] = new LinkedList<(IPlayer Player, int Depth)>();
        players[_position].AddLast((_player1, 1));
        players[_position].AddLast((_player2, 2));

        // Act
        var result = DepthChartManager.GetFullDepthChart();

        // Assert
        Assert.True(result.ContainsKey(_position), "Chart should contain the position.");
        var playersAtPosition = result[_position];
        Assert.Equal(2, playersAtPosition.Count);

        var firstNode = playersAtPosition.First;
        Assert.NotNull(firstNode);
        Assert.Equal(_player1, firstNode.Value.Player);
        Assert.Equal(1, firstNode.Value.Depth);

        var secondNode = firstNode.Next;
        Assert.NotNull(secondNode);
        Assert.Equal(_player2, secondNode.Value.Player);
        Assert.Equal(2, secondNode.Value.Depth);
    }
}

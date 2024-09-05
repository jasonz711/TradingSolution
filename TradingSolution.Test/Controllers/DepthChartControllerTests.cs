using System;
using Microsoft.AspNetCore.Mvc;
using TradingSolution.Controllers;
using TradingSolution.Interfaces;
using TradingSolution.Models;
using TradingSolution.Models.DTOs;
using TradingSolution.Services;

namespace TradingSolution.Test.Controllers;

[Collection("DepthChartTests")]
public class DepthChartControllerTests
{
    private DepthChartController _controller;
    private readonly Dictionary<NFLPosition, LinkedList<(IPlayer Player, int Depth)>> _chart = DepthChart.Instance.Chart;
    private NFLPosition _position = NFLPosition.C;
    private NFLPlayer _player1 = new NFLPlayer("Ash", 1, null);
    private NFLPlayer _player2 = new NFLPlayer("Bob", 2, null);
    private NFLPlayer _player3 = new NFLPlayer("Chris", 3, null);

    public DepthChartControllerTests()
    {
        _controller = new DepthChartController();
    }

    [Fact]
    public void GetAllPlayers_ReturnsOkResult_WithCorrectData()
    {
        // Arrange
        _chart.Clear();
        _chart[_position] = new();
        _chart[_position].AddFirst((_player1, 1));
        _chart[_position].AddLast((_player2, 2));
        // Act
        var result = _controller.GetAllPlayers();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var chartData = Assert.IsType<Dictionary<string, List<PlayerDepthDto>>>(okResult.Value);

        Assert.NotNull(chartData);
        Assert.Equal(2, chartData["C"].Count);
        Assert.Equal(_player1.Name, chartData["C"][0].Name);
        Assert.Equal(_player2.Name, chartData["C"][1].Name);
    }

    [Fact]
    public void AddPlayerToDepthChart_ValidData_ReturnsCreatedResult()
    {
        // Arrange
        var dto = new AddPlayerToDepthChartDto
        {
            Position = _position,
            NFLPlayer = _player1,
            Depth = 1
        };
        _chart.Clear();

        // Act
        var result = _controller.AddPlayerToDepthChart(dto);

        // Assert
        var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        var player = Assert.IsType<NFLPlayer>(createdResult.Value);

        Assert.Equal("Ash", player.Name);
        Assert.Equal("Ash", _chart[_position].First!.Value.Player.Name);
        Assert.Equal(1, player.Number);
        Assert.Equal(1, _chart[_position].First!.Value.Player.Number);
    }

    [Fact]
    public void AddPlayerToDepthChart_ExistingPlayer_ReturnsBadRequest()
    {
        // Arrange
        var dto = new AddPlayerToDepthChartDto
        {
            Position = _position,
            NFLPlayer = _player1,
            Depth = 1
        };

        // Add the player first
        _controller.AddPlayerToDepthChart(dto);

        // Try adding the same player again
        var result = _controller.AddPlayerToDepthChart(dto);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.Equal("Player exists", badRequestResult.Value);
    }

      [Fact]
    public void AddPlayerToDepthChart_SameNumberDifferentPlayerName_ReturnsBadRequest()
    {
        // Arrange
        var dto = new AddPlayerToDepthChartDto
        {
            Position = _position,
            NFLPlayer = new NFLPlayer("Ash", 1, null),
            Depth = 1
        };

        var dto2 = new AddPlayerToDepthChartDto
        {
            Position = _position,
            NFLPlayer = new NFLPlayer("Bob", 1, null),
            Depth = 1
        };

        // Add the player first
        _controller.AddPlayerToDepthChart(dto);

        // Try adding the same player again
        var result = _controller.AddPlayerToDepthChart(dto2);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.Equal("Adding invalid player due to duplicated number", badRequestResult.Value);
    }

    [Fact]
    public void GetBackups_ValidPosition_ReturnsOkResult()
    {
        // Arrange
        _chart.Clear();
        _chart[_position] = new();
        _chart[_position].AddFirst((_player1, 1));
        _chart[_position].AddLast((_player2, 2));
        _chart[_position].AddLast((_player3, 3));
        var dto = new GetBackupsDto
        {
            Position = _position,
            Player = _player1
        };

        // Act
        var result = _controller.GetBackups(dto);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var backups = Assert.IsType<List<IPlayer?>>(okResult.Value);
        Assert.NotNull(backups);
        Assert.Equal(2, backups.Count);
        Assert.Equal(_player2, backups[0]);
        Assert.Equal(_player3, backups[1]);
    }

    [Fact]
    public void GetBackups_LastDepth_ReturnsEmpty()
    {
        // Arrange
        _chart.Clear();
        _chart[_position] = new();
        _chart[_position].AddFirst((_player1, 1));
        _chart[_position].AddLast((_player2, 2));
        _chart[_position].AddLast((_player3, 3));
        var dto = new GetBackupsDto
        {
            Position = _position,
            Player = _player3
        };

        // Act
        var result = _controller.GetBackups(dto);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var backups = Assert.IsType<List<IPlayer?>>(okResult.Value);
        Assert.Empty(backups);
    }

    [Fact]
    public void GetBackups_InvalidPlayer_ReturnsEmpty()
    {
        // Arrange
        _chart.Clear();
        _chart[_position] = new();
        _chart[_position].AddFirst((_player1, 1));
        _chart[_position].AddLast((_player2, 2));
        _chart[_position].AddLast((_player3, 3));
        var dto = new GetBackupsDto
        {
            Position = _position,
            Player = new NFLPlayer("Nobody", 99, null)
        };

        // Act
        var result = _controller.GetBackups(dto);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var backups = Assert.IsType<List<IPlayer?>>(okResult.Value);
        Assert.Empty(backups);
    }

    [Fact]
    public void DeletePlayerFromChart_ValidData_ReturnsOkResult()
    {
        // Arrange
        _chart.Clear();
        _chart[_position] = new();
        _chart[_position].AddFirst((_player1, 1));
        _chart[_position].AddLast((_player2, 2));
        _chart[_position].AddLast((_player3, 3));
        var dto = new DeletePlayerFromChartDto
        {
            Position = _position,
            Player = _player1
        };

        // Act
        var result = _controller.DeletePlayerFromChart(dto);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var removedPlayer = Assert.IsAssignableFrom<List<IPlayer?>>(okResult.Value);
        Assert.NotNull(removedPlayer);
        Assert.Equal("Ash", removedPlayer[0]!.Name);
        Assert.DoesNotContain((_player1, 1), _chart[_position]);
    }

    [Fact]
    public void DeletePlayerFromChart_InvalidPlayer_ReturnsEmpty()
    {
        // Arrange
        _chart.Clear();
        _chart[_position] = new();
        _chart[_position].AddFirst((_player1, 1));
        _chart[_position].AddLast((_player2, 2));
        _chart[_position].AddLast((_player3, 3));
        var dto = new DeletePlayerFromChartDto
        {
            Position = _position,
            Player = new NFLPlayer("Nobody", 99, null)
        };

        // Act
        var result = _controller.DeletePlayerFromChart(dto);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var removedPlayer = Assert.IsAssignableFrom<List<IPlayer?>>(okResult.Value);
        Assert.Empty(removedPlayer);
        Assert.Equal(3, _chart[_position].Count);
    }
}

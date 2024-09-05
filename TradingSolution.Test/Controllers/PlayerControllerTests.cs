using System;
using Microsoft.AspNetCore.Mvc;
using TradingSolution.Controllers;
using TradingSolution.Interfaces;
using TradingSolution.Models;
using TradingSolution.Services;

namespace TradingSolution.Test.Controllers;

[Collection("DepthChartTests")]
public class PlayerControllerTests
{
    private Dictionary<int, IPlayer> _players = PlayerDB.Instance.Players;
    [Fact]
    public void GetPlayer_ReturnsPlayerByNumber()
    {
        // Arrange
        _players.Clear();
        var controller = new PlayerController();
        var player = new NFLPlayer("Ash", 1, null);
        _players[1] = player;

        // Act
        var result = controller.GetPlayer(player.Number);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedPlayer = Assert.IsType<NFLPlayer>(okResult.Value);
        Assert.Equal(player.Name, returnedPlayer.Name);
        Assert.Equal(player.Number, returnedPlayer.Number);
    }

    [Fact]
    public void GetPlayer_ReturnsNotFoundForInvalidNumber()
    {
        // Arrange
        var controller = new PlayerController();
        _players.Clear();
        var player = new NFLPlayer("Ash", 1, null);
        _players[1] = player;

        // Act
        var result = controller.GetPlayer(999);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result.Result);
    }
    [Fact]
    public void GetPlayers_ReturnsListOfPlayers()
    {
        // Arrange
        var controller = new PlayerController();
        _players.Clear();
        _players[1] = new NFLPlayer("Ash", 1, null);
        _players[2] = new NFLPlayer("Bob", 2, null);

        // Act
        var result = controller.GetAllPlayers();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var players = Assert.IsType<Dictionary<int, IPlayer>>(okResult.Value);
        Assert.Equal(2, players.Count);
        Assert.Equal("Ash", players[1].Name);
        Assert.Equal("Bob", players[2].Name);
    }

    [Fact]
    public void AddPlayer_ReturnsCreatedAtAction()
    {
        // Arrange
        _players.Clear();
        var controller = new PlayerController();
        var newPlayer = new NFLPlayer("Ash", 1, null);

        // Act
        var result = controller.CreatePlayer(newPlayer);

        // Assert
        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        var returnedPlayer = Assert.IsAssignableFrom<IPlayer>(createdAtActionResult.Value);

        // Validate that the returned player's properties match the input
        Assert.Equal("Ash", returnedPlayer.Name);
        Assert.Equal(1, returnedPlayer.Number);

        // Check if player was added by calling GetPlayers
        var getPlayersResult = controller.GetPlayer(1);
        var okResult = Assert.IsType<OkObjectResult>(getPlayersResult.Result);
        var player = Assert.IsType<NFLPlayer>(okResult.Value);

        Assert.Equal("Ash", player.Name);
    }
}

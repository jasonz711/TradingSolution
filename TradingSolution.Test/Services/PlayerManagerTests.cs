using System;
using Moq;
using TradingSolution.Interfaces;
using TradingSolution.Services;

namespace TradingSolution.Test.Services;

[Collection("DepthChartTests")]
public class PlayerManagerTests
{
    [Fact]
    public void CreatePlayer_ShouldAddPlayer_WhenPlayerDoesNotExist()
    {
        // Arrange
        var playerMock = new Mock<IPlayer>();
        playerMock.Setup(p => p.Number).Returns(10);


        // Act
        PlayerManager.CreatePlayer(playerMock.Object);

        // Assert
        Assert.True(PlayerDB.Instance.Players.ContainsKey(10));
        Assert.Equal(playerMock.Object, PlayerDB.Instance.Players[10]);
    }

    [Fact]
    public void CreatePlayer_ShouldThrowArgumentException_WhenPlayerAlreadyExists()
    {
        // Arrange
        var playerMock = new Mock<IPlayer>();
        playerMock.Setup(p => p.Number).Returns(10);

        PlayerDB.Instance.Players[10] = playerMock.Object; // Simulate existing player

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => PlayerManager.CreatePlayer(playerMock.Object));
        Assert.Equal("Player number exists", exception.Message);
    }

    [Fact]
    public void GetPlayer_ShouldFindPlayer_WhenPlayerExists()
    {
        // Arrange
        var playerMock = new Mock<IPlayer>();
        playerMock.Setup(p => p.Number).Returns(10);

        PlayerDB.Instance.Players[10] = playerMock.Object; 

        // Act & Assert
        Assert.Equal(playerMock.Object, PlayerManager.GetPlayer(10));
    }

    [Fact]
    public void GetPlayer_ShouldThrowKeyNotFoundException_WhenPlayerDoesNotExists()
    {

        // Act & Assert
        var exception = Assert.Throws<KeyNotFoundException>(() => PlayerManager.GetPlayer(999));
        Assert.Equal("Player number not exists", exception.Message);
    }
}

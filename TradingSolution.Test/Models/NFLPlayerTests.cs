using System;
using TradingSolution.Models;

namespace TradingSolution.Test.Models;

public class NFLPlayerTests
{
    [Fact]
    public void Equals_SameProperties_ReturnsTrue()
    {
        // Arrange
        var player1 = new NFLPlayer("Ash", 12, null);
        var player2 = new NFLPlayer("Ash", 12, null);

        // Act & Assert
        Assert.True(player1.Equals(player2));
    }

    [Fact]
    public void Equals_DifferentName_ReturnsFalse()
    {
        // Arrange
        var player1 = new NFLPlayer("Ash", 12, null);
        var player2 = new NFLPlayer("ash", 12, null);

        // Act & Assert
        Assert.False(player1.Equals(player2));
    }

    [Fact]
    public void Equals_DifferentNumber_ReturnsFalse()
    {
        // Arrange
        var player1 = new NFLPlayer("Ash", 12, null);
        var player2 = new NFLPlayer("Ash", 15, null);

        // Act & Assert
        Assert.False(player1.Equals(player2));
    }

    [Fact]
    public void Equals_Null_ReturnsFalse()
    {
        // Arrange
        var player1 = new NFLPlayer("Ash", 12, null);

        // Act & Assert
        Assert.False(player1.Equals(null));
    }

    [Fact]
    public void GetHashCode_SameProperties_ReturnsSameHashCode()
    {
        // Arrange
        var player1 = new NFLPlayer("Ash", 12, null);
        var player2 = new NFLPlayer("Ash", 12, null);

        // Act & Assert
        Assert.Equal(player1.GetHashCode(), player2.GetHashCode());
    }

    [Fact]
    public void GetHashCode_DifferentProperties_ReturnsDifferentHashCode()
    {
        // Arrange
        var player1 = new NFLPlayer("Ash", 12, null);
        var player2 = new NFLPlayer("Bob", 15, null);

        // Act & Assert
        Assert.NotEqual(player1.GetHashCode(), player2.GetHashCode());
    }
}

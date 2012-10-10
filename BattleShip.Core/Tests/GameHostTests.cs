using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    using System.Device.Location;

    using BattleShip.Core;

    using NUnit.Framework;

    [TestFixture]
    class GameHostTests
    {
        [Test]
        public void GameHost_SingleFullStackGame_PlaysAsExpected()
        {
            const string player1Email = "A@pebblegame.com";
            const string player2Email = "B@pebblegame.com";
            var player1Location = new GeoCoordinate(0.0, 0.0);
            var player2Location = new GeoCoordinate(5.0, 5.0);
            var gameHost = new GameHost();
            Guid gameId = gameHost.CreateGame("My Game", "A", player1Email, "B", player2Email);
            gameHost.SetPlayerLocation(gameId, player1Email, player1Location);
            gameHost.SetPlayerLocation(gameId, player2Email, player2Location);

            var player1TargetZone = gameHost.GetOpponentTargetZone(gameId, player2Email);

            var shotLocation = new GeoCoordinate(player1TargetZone.Center.Latitude + GameHost.ShotBlastRadius + 0.1, player1TargetZone.Center.Longitude + GameHost.ShotBlastRadius + 0.1);
            var result = gameHost.TakeShot(gameId, player2Email, shotLocation);
            Assert.AreEqual(ShotResult.Miss, result);

            result = gameHost.TakeShot(gameId, player1Email, player2Location);
            Assert.AreEqual(ShotResult.Hit, result);

            result = gameHost.TakeShot(gameId, player2Email, player2Location);
            Assert.AreEqual(ShotResult.GameAlreadyOver, result);
        }

        [Test]
        public void GameHost_TwoActiveGames_PlaysAsExpected()
        {
            const string player1Email = "A@pebblegame.com";
            const string player2Email = "B@pebblegame.com";
            var player1Game1Location = new GeoCoordinate(0.0, 0.0);
            var player2Game1Location = new GeoCoordinate(5.0, 5.0);
            var player1Game2Location = new GeoCoordinate(1.0, 1.0);
            var player2Game2Location = new GeoCoordinate(6.0, 6.0);
            var gameHost = new GameHost();

            Guid game1Id = gameHost.CreateGame("My Game 1", "A", player1Email, "B", player2Email);
            gameHost.SetPlayerLocation(game1Id, player1Email, player1Game1Location);
            gameHost.SetPlayerLocation(game1Id, player2Email, player2Game1Location);

            Guid game2Id = gameHost.CreateGame("My Game 2", "A", player1Email, "B", player2Email);
            gameHost.SetPlayerLocation(game2Id, player1Email, player1Game2Location);
            gameHost.SetPlayerLocation(game2Id, player2Email, player2Game2Location);

            var player1Game1TargetZone = gameHost.GetOpponentTargetZone(game1Id, player2Email);
            var player1Game2TargetZone = gameHost.GetOpponentTargetZone(game2Id, player2Email);

            //Game1, player1 - miss 
            var shotLocation = new GeoCoordinate(player1Game1TargetZone.Center.Latitude + GameHost.ShotBlastRadius + 0.1, player1Game1TargetZone.Center.Longitude + GameHost.ShotBlastRadius + 0.1);
            var result = gameHost.TakeShot(game1Id, player2Email, shotLocation);
            Assert.AreEqual(ShotResult.Miss, result);

            //Game2, player1 - miss 
            shotLocation = new GeoCoordinate(player1Game2TargetZone.Center.Latitude + GameHost.ShotBlastRadius + 0.1, player1Game2TargetZone.Center.Longitude + GameHost.ShotBlastRadius + 0.1);
            result = gameHost.TakeShot(game2Id, player2Email, shotLocation);
            Assert.AreEqual(ShotResult.Miss, result);

            //Game1, player2 - hit
            result = gameHost.TakeShot(game1Id, player1Email, player2Game1Location);
            Assert.AreEqual(ShotResult.Hit, result);

            //Game2, player2 - hit
            result = gameHost.TakeShot(game2Id, player1Email, player2Game2Location);
            Assert.AreEqual(ShotResult.Hit, result);

            //Game1, player1 - game already over
            result = gameHost.TakeShot(game1Id, player2Email, player2Game1Location);
            Assert.AreEqual(ShotResult.GameAlreadyOver, result);

            //Game2, player1 - game already over
            result = gameHost.TakeShot(game2Id, player2Email, player2Game2Location);
            Assert.AreEqual(ShotResult.GameAlreadyOver, result);
        }
    }
}

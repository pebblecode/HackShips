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
            var player2TargetZone = gameHost.GetOpponentTargetZone(gameId, player1Email);

            var shotLocation = new GeoCoordinate(player1TargetZone.Center.Latitude + GameHost.ShotBlastRadius + 0.1, player1TargetZone.Center.Longitude + GameHost.ShotBlastRadius + 0.1);
            var result = gameHost.TakeShot(gameId, player2Email, shotLocation);
            Assert.AreNotEqual(ShotResult.Hit, result);

            result = gameHost.TakeShot(gameId, player1Email, player2Location);
            Assert.AreEqual(ShotResult.Hit, result);

            result = gameHost.TakeShot(gameId, player2Email, player2Location);
            Assert.AreEqual(ShotResult.GameAlreadyOver, result);
        }
    }
}

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
    public class GameTests
    {
        const double playerTargetZoneRadius = 2.0;
        const double shotBlastRadius = 1.0;

        [Test]
        public void TakeShot_ShotOnExactTarget_ReturnsHit()
        {
            Player player1, player2;
            var game = CreateGame(out player1, out player2);
            player1.UpdateLocation(new GeoCoordinate(0.0, 0.0), playerTargetZoneRadius);
            player2.UpdateLocation(new GeoCoordinate(5.0, 5.0), playerTargetZoneRadius);

            var result = game.TakeShot(player2, player1.Location);

            Assert.AreEqual(ShotResult.Hit, result);
        }

        [Test]
        public void TakeShot_TargetWithinBlastRadius_ReturnsHit()
        {
            Player player1, player2;
            var game = CreateGame(out player1, out player2);
            player1.UpdateLocation(new GeoCoordinate(0.0, 0.0), playerTargetZoneRadius);
            player2.UpdateLocation(new GeoCoordinate(5.0, 5.0), playerTargetZoneRadius);

            var shotLocation = new GeoCoordinate(player1.Location.Latitude + shotBlastRadius / 2.0, player1.Location.Longitude + shotBlastRadius / 2.0);
            var result = game.TakeShot(player2, shotLocation);

            Assert.AreEqual(ShotResult.Hit, result);
        }

        [Test]
        public void TakeShot_ShotOffTarget_ReturnsMiss()
        {
            Player player1, player2;
            var game = CreateGame(out player1, out player2);
            player1.UpdateLocation(new GeoCoordinate(0.0, 0.0), playerTargetZoneRadius);
            player2.UpdateLocation(new GeoCoordinate(5.0, 5.0), playerTargetZoneRadius);

            var shotLocation = GetOffTargetShot(player1);
            var result = game.TakeShot(player2, shotLocation);

            Assert.AreEqual(ShotResult.Miss, result);
        }

        private static GeoCoordinate GetOffTargetShot(Player targetPlayer, double offset = 0.1)
        {
            return new GeoCoordinate(targetPlayer.Location.Latitude + shotBlastRadius + offset, targetPlayer.Location.Longitude + shotBlastRadius + offset);
        }

        [Test]
        public void TakeShot_ShotOutsideTargetZone_ReturnsOutsideTargetZone()
        {
            Player player1, player2;
            var game = CreateGame(out player1, out player2);
            player1.UpdateLocation(new GeoCoordinate(0.0, 0.0), playerTargetZoneRadius);
            player2.UpdateLocation(new GeoCoordinate(5.0, 5.0), playerTargetZoneRadius);

            var shotLocation = new GeoCoordinate(player1.TargetZone.Center.Latitude + player1.TargetZone.Radius + 0.1, player1.TargetZone.Center.Longitude + player1.TargetZone.Radius + 0.1);
            var result = game.TakeShot(player2, shotLocation);

            Assert.AreEqual(ShotResult.OutsideTargetZone, result);
        }

        [Test]
        public void TakeShot_WrongPlayerTakingShot_ReturnsIllegalPlayer()
        {
            Player player1, player2;
            var game = CreateGame(out player1, out player2);
            player1.UpdateLocation(new GeoCoordinate(0.0, 0.0), playerTargetZoneRadius);
            player2.UpdateLocation(new GeoCoordinate(5.0, 5.0), playerTargetZoneRadius);

            var shotLocation = player2.Location;
            var result = game.TakeShot(player1, shotLocation);

            Assert.AreEqual(ShotResult.IllegalPlayer, result);
        }

        [Test]
        public void TakeShot_GameAlreadyOver_ReturnsGameAlreadyOver()
        {
            Player player1, player2;
            var game = CreateGame(out player1, out player2);
            player1.UpdateLocation(new GeoCoordinate(0.0, 0.0), playerTargetZoneRadius);
            player2.UpdateLocation(new GeoCoordinate(5.0, 5.0), playerTargetZoneRadius);

            var result = game.TakeShot(player2, player1.Location);
            Assert.AreEqual(ShotResult.Hit, result);

            var shotLocation = GetOffTargetShot(player2, 0.12);
            result = game.TakeShot(player1, shotLocation);

            Assert.AreEqual(ShotResult.GameAlreadyOver, result);
        }

        [Test]
        public void TakeShot_SequenceOfAlternatingMissedShots_ReturnsMissEachTime()
        {
            Player player1, player2;
            var game = CreateGame(out player1, out player2);
            player1.UpdateLocation(new GeoCoordinate(0.0, 0.0), playerTargetZoneRadius);
            player2.UpdateLocation(new GeoCoordinate(5.0, 5.0), playerTargetZoneRadius);

            var shotLocation = GetOffTargetShot(player1, 0.1);
            var result = game.TakeShot(player2, shotLocation);
            Assert.AreEqual(ShotResult.Miss, result);

            shotLocation = GetOffTargetShot(player2, 0.12);
            result = game.TakeShot(player1, shotLocation);
            Assert.AreEqual(ShotResult.Miss, result);

            shotLocation = GetOffTargetShot(player1, 0.11);
            result = game.TakeShot(player2, shotLocation);
            Assert.AreEqual(ShotResult.Miss, result);
        }

        private Game CreateGame(out Player player1, out Player player2)
        {
            player1 = new Player("A@b.com", "A");
            player2 = new Player("B@b.com", "B");

            var game = new Game("Test", player1, player2, playerTargetZoneRadius, shotBlastRadius);

            return game;
        }
    }
}

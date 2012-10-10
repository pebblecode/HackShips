namespace BattleShip.Core
{
    using System;
    using System.Collections.Generic;
    using System.Device.Location;
    using System.Linq;

    public class Game
    {
        public Player NextPlayerToTakeShot { get; private set; }

        private readonly double playerTargetZoneRadius;

        private readonly double shotBlastRadius;

        public Guid Id { get; private set; }

        public string Name { get; private set; }

        public Player Player1 { get; private set; }

        public Player Player2 { get; private set; }

        public Game(string name, Player initiatingPlayer, Player acceptingPlayer, double playerTargetZoneRadius, double shotBlastRadius)
        {
            Id = Guid.NewGuid();
            Name = name;
            Player1 = initiatingPlayer;
            Player2 = acceptingPlayer;

            NextPlayerToTakeShot = acceptingPlayer;
            this.playerTargetZoneRadius = playerTargetZoneRadius;
            this.shotBlastRadius = shotBlastRadius;
        }

        private readonly Stack<Shot> shots = new Stack<Shot>();

        public ShotResult TakeShot(Player playerTakingShot, GeoCoordinate shotLocation)
        {
            if (playerTakingShot != NextPlayerToTakeShot)
            {
                return ShotResult.IllegalPlayer; 
            }

            if (LastShotWasHit())
            {
                return ShotResult.GameAlreadyOver;
            }

            var targetPlayer = NextPlayerToTakeShot == Player1 ? Player2 : Player1;
            if (targetPlayer.Location == null)
            {
                return ShotResult.TargetHasNoLocation;
            }

            if (ShotOutsideTargetZone(targetPlayer, shotLocation))
            {
                return ShotResult.OutsideTargetZone;
            }

            //Take shot
            var shot = ShotOnTarget(targetPlayer, shotLocation) ? new Shot(playerTakingShot, shotLocation, ShotResult.Hit) : new Shot(playerTakingShot, shotLocation, ShotResult.Miss);

            //Push to stack 
            shots.Push(shot);

            NextPlayerToTakeShot = targetPlayer;

            return shot.ShotResult;
        }

        private bool LastShotWasHit()
        {
            if (!shots.Any())
            {
                return false;
            }

            return shots.Peek().ShotResult == ShotResult.Hit;
        }

        private bool ShotOutsideTargetZone(Player targetPlayer, GeoCoordinate shotLocation)
        {
            return GetGeoDistanceBetween(shotLocation, targetPlayer.Location) > playerTargetZoneRadius;
        }

        private bool ShotOnTarget(Player targetPlayer, GeoCoordinate shotLocation)
        {
            return GetGeoDistanceBetween(shotLocation, targetPlayer.Location) < shotBlastRadius;
        }

        private double GetGeoDistanceBetween(GeoCoordinate first, GeoCoordinate second)
        {
            return Math.Sqrt(Math.Pow(first.Latitude - second.Latitude, 2) + Math.Pow(first.Longitude - second.Longitude, 2));
        }
    }
}
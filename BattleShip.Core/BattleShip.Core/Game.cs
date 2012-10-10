namespace BattleShip.Core
{
    using System;
    using System.Collections.Generic;
    using System.Device.Location;

    public class Game
    {
        private Player nextPlayerToTakeShot;

        private readonly double playerTargetZoneRadius;

        private readonly double shotBlastRadius;

        public Guid Id { get; private set; }

        public string Name { get; private set; }

        public Player Player1 { get; private set; }

        public Player Player2 { get; private set; }

        public Game(Guid id, string name, Player initiatingPlayer, Player acceptingPlayer, double playerTargetZoneRadius, double shotBlastRadius)
        {
            Id = id;
            Name = name;
            Player1 = initiatingPlayer;
            Player2 = acceptingPlayer;

            nextPlayerToTakeShot = acceptingPlayer;
            this.playerTargetZoneRadius = playerTargetZoneRadius;
            this.shotBlastRadius = shotBlastRadius;
        }

        private readonly Stack<Shot> shots = new Stack<Shot>();

        public ShotResult TakeShot(Player playerTakingShot, GeoCoordinate shotLocation)
        {
            if (playerTakingShot != nextPlayerToTakeShot)
            {
                return ShotResult.IllegalPlayer; 
            }

            var targetPlayer = nextPlayerToTakeShot == Player1 ? Player2 : Player1;
            if (targetPlayer.Location == null)
            {
                return ShotResult.TargetHasNoLocation;
            }

            if (ShotOutsideTargetZone(targetPlayer, shotLocation))
            {
                return ShotResult.OutsideTargetZone;
            }

            //Take shot
            if (ShotOnTarget(targetPlayer, shotLocation))
            {
                //create hit host
            }
            else
            {
                //create miss shot
            }
            //Push to stack 
            
            //swap next player
            if (nextPlayerToTakeShot == Player1)
            {
                nextPlayerToTakeShot = Player2;
            }
            else
            {
                nextPlayerToTakeShot = Player1;
            }

            return ShotResult.Hit;
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
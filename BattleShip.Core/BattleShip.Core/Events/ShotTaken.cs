namespace BattleShip.Core
{
    using System;
    using System.Device.Location;

    public class ShotTaken
    {
        public ShotTaken(Guid gameId, Player playerTakingShot, GeoCoordinate shotLocation)
        {
            GameId = gameId;
            PlayerTakingShot = playerTakingShot;
            ShotLocation = shotLocation;
        }

        public Guid GameId { get; private set; }

        public Player PlayerTakingShot { get; private set; }

        public GeoCoordinate ShotLocation { get; private set; }
    }
}
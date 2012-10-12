namespace BattleShip.Core
{
    using System;
    using System.Device.Location;

    public class UpdatePlayerLocation
    {
        public UpdatePlayerLocation(Guid gameId, string playerEmail, GeoCoordinate location)
        {
            this.GameId = gameId;
            this.PlayerEmail = playerEmail;
            this.Location = location;
        }

        public Guid GameId { get; private set; }

        public string PlayerEmail { get; private set; }

        public GeoCoordinate Location { get; private set; }
    }
}
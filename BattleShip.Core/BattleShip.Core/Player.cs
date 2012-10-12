using System.Collections.Generic;

namespace BattleShip.Core
{
    using System;
    using System.Device.Location;
    using System.Linq;

    public class Player
    {
        private readonly double targetZoneRadius;

        public string Email { get; private set; }

        public string Name { get; private set; }

        public GeoCoordinate Location { get; private set; }

        public TargetZone TargetZone { get; private set; }

        public Player(string email, string name, GeoCoordinate location, double targetZoneRadius)
        {
            this.targetZoneRadius = targetZoneRadius;
            Email = email;
            Name = name;
            Location = location;
            TargetZone = null;
        }

        public Player Apply(PlayerLocationUpdated playerLocationUpdated) 
        {
            return new Player(Email, Name, playerLocationUpdated.Location, targetZoneRadius);
        }

        private TargetZone CreateRandomZoneAroundPlayer(double radius)
        {
            var rnd = new Random();

            //choose random radius (0->playerTargetZoneRadius)
            var rndRadius = rnd.NextDouble() * radius;

            //choose random angle (0->360 degrees)
            var rndAngleInRadians = rnd.NextDouble() * 2.0 * Math.PI;

            //create target zone
            var coord = Helper.FromPolarToCartesian(rndRadius, rndAngleInRadians);
            return new TargetZone(new GeoCoordinate(Location.Latitude + coord.Y, Location.Longitude + coord.X), radius);
        }
    }
}

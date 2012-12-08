using System.Collections.Generic;

namespace BattleShip.Core
{
    using System;
    using System.Device.Location;
    using System.Linq;

    public class Player
    {
        private readonly Stack<GeoCoordinate> locationStack = new Stack<GeoCoordinate>();

        public string Email { get; private set; }

        public string Name { get; private set; }

        public IEnumerable<GeoCoordinate> LocationHistory 
        {
            get
            {
                return locationStack;
            }
        }

        public GeoCoordinate Location 
        { 
            get
            {
                return locationStack.Any() ? locationStack.Peek() : null;
            }
        }

        public TargetZone TargetZone { get; private set; }

        public Player(string email, string name)
        {
            Email = email;
            Name = name;
        }

        public void UpdateLocation(GeoCoordinate coordinate, double radius)
        {
            locationStack.Push(coordinate);

            TargetZone = CreateRandomZoneAroundPlayer(coordinate, radius);
        }

        private TargetZone CreateRandomZoneAroundPlayer(GeoCoordinate coordinate, double radius)
        {
            var rnd = new Random();

            //choose random radius (0->playerTargetZoneRadius)
            var rndRadius = rnd.NextDouble() * radius;

            //choose random angle (0->360 degrees)
            var rndAngleInRadians = rnd.NextDouble() * 2.0 * Math.PI;

            //create target zone
            var coord = Helper.FromPolarToCartesian(rndRadius, rndAngleInRadians);
            return new TargetZone(new GeoCoordinate(coordinate.Latitude + coord.Y, coordinate.Longitude + coord.X), radius);
        }
    }
}

namespace BattleShip.Core
{
    using System.Device.Location;

    public class TargetZone
    {
        public GeoCoordinate Center { get; private set; }

        public double Radius { get; private set; }

        public TargetZone(GeoCoordinate center, double radius)
        {
            Center = center;
            Radius = radius;
        }
    }
}
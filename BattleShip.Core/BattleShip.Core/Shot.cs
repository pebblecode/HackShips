namespace BattleShip.Core
{
    using System.Device.Location;

    public class Shot
    {
        public Player Player { get; private set; }

        public GeoCoordinate Location { get; private set; }

        public ShotResult ShotResult { get; private set; }

        public Shot(Player player, GeoCoordinate location, ShotResult shotResult)
        {
            Player = player;
            Location = location;
            ShotResult = shotResult;
        }
    }
}
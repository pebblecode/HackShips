namespace BattleShip.Core
{
    using System.Device.Location;

    public class Shot
    {
        public Player PlayerTakingShot { get; private set; }

        public GeoCoordinate Location { get; private set; }

        public ShotResult ShotResult { get; private set; }

        public Shot(Player playerTakingShot, GeoCoordinate location, ShotResult shotResult)
        {
            PlayerTakingShot = playerTakingShot;
            Location = location;
            ShotResult = shotResult;
        }
    }
}
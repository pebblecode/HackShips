namespace BattleShip.Core
{
    using System;
    using System.Device.Location;

    public class Game
    {
        private readonly double playerTargetZoneRadius;

        private readonly double shotBlastRadius;

        public Guid Id { get; private set; }

        public string Name { get; private set; }

        public Player NextPlayerToTakeShot { get; private set; }

        public Player Opponent { get; private set; }

        public Shot LastShot { get; private set; }

        public Game(string name, Player nextPlayerToTakeShot, Player opponent, Shot lastShot, double playerTargetZoneRadius, double shotBlastRadius)
        {
            Id = Guid.NewGuid();
            Name = name;
            NextPlayerToTakeShot = nextPlayerToTakeShot;
            Opponent = opponent;
            LastShot = lastShot;

            this.playerTargetZoneRadius = playerTargetZoneRadius;
            this.shotBlastRadius = shotBlastRadius;
        }

        public Game Apply(PlayerLocationUpdated playerLocationUpdated)
        {
            var player = FindPlayerByEmail(playerLocationUpdated.PlayerEmail);

            var newPlayer = player.Apply(playerLocationUpdated);
            
            return new Game(Name, player == NextPlayerToTakeShot ? newPlayer : NextPlayerToTakeShot, player == Opponent ? newPlayer : Opponent, LastShot, playerTargetZoneRadius, shotBlastRadius);
        }

        public Game Apply(ShotTaken shotTaken)
        {
            if (shotTaken.PlayerTakingShot.Email != NextPlayerToTakeShot.Email)
            {
                return new Game(Name, NextPlayerToTakeShot, Opponent, new Shot(shotTaken.PlayerTakingShot, shotTaken.ShotLocation, ShotResult.IllegalPlayer), playerTargetZoneRadius, shotBlastRadius); 
            }

            if (LastShotWasHit())
            {
                return new Game(Name, NextPlayerToTakeShot, Opponent, new Shot(shotTaken.PlayerTakingShot, shotTaken.ShotLocation, ShotResult.GameAlreadyOver), playerTargetZoneRadius, shotBlastRadius);
            }

            if (Opponent.Location == null)
            {
                return new Game(Name, NextPlayerToTakeShot, Opponent, new Shot(shotTaken.PlayerTakingShot, shotTaken.ShotLocation, ShotResult.TargetHasNoLocation), playerTargetZoneRadius, shotBlastRadius);
            }

            if (ShotOutsideTargetZone(Opponent, shotTaken.ShotLocation))
            {
                return new Game(Name, NextPlayerToTakeShot, Opponent, new Shot(shotTaken.PlayerTakingShot, shotTaken.ShotLocation, ShotResult.OutsideTargetZone), playerTargetZoneRadius, shotBlastRadius);
            }

            var shot = ShotOnTarget(Opponent, shotTaken.ShotLocation) 
                ? new Shot(shotTaken.PlayerTakingShot, shotTaken.ShotLocation, ShotResult.Hit) 
                : new Shot(shotTaken.PlayerTakingShot, shotTaken.ShotLocation, ShotResult.Miss);

            return new Game(Name, NextPlayerToTakeShot, Opponent, shot, playerTargetZoneRadius, shotBlastRadius);
        }

        private Player FindPlayerByEmail(string playerEmail)
        {
            if (NextPlayerToTakeShot.Email == playerEmail)
            {
                return NextPlayerToTakeShot;
            }

            if (Opponent.Email == playerEmail)
            {
                return Opponent;
            }

            throw new InvalidOperationException("No such player.");
        }

        private bool LastShotWasHit()
        {
            if (LastShot == null)
            {
                return false;
            }

            return LastShot.ShotResult == ShotResult.Hit;
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
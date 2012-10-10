using System;
using System.Collections.Generic;

namespace BattleShip.Core
{
    using System.Device.Location;

    public class GameHost
    {
        private readonly Dictionary<Guid, Game> games = new Dictionary<Guid, Game>();

        private const double PlayerTargetZoneRadius = 5;

        private const double ShotBlastRadius = 1;

        public Guid CreateGame(string gameName, string initiatingPlayerName, string initiatingPlayerEmail, string acceptingPlayerName, string acceptingPlayerEmail)
        {
            var initiatingPlayer = new Player(initiatingPlayerEmail, initiatingPlayerName);
            var acceptingPlayer = new Player(acceptingPlayerEmail, acceptingPlayerName);

            var game = new Game(gameName, initiatingPlayer, acceptingPlayer, PlayerTargetZoneRadius, ShotBlastRadius);
            games.Add(game.Id, game);

            return game.Id;
        }

        public void SetPlayerLocation(Guid gameId, string playerEmail, GeoCoordinate location)
        {
            Player player;

            var game = FindGameAndPlayer(gameId, playerEmail, out player);
            player.UpdateLocation(location, PlayerTargetZoneRadius);
        }

        private Game FindGameAndPlayer(Guid gameId, string playerEmail, out Player player)
        {
            Game game;
            if (games.TryGetValue(gameId, out game))
            {
                player = FindPlayerByEmail(game, playerEmail);
            } 
            else
            {
                throw new InvalidOperationException("No such game");
            }

            return game;
        }

        private Player FindPlayerByEmail(Game game, string playerEmail)
        {
            if (game.Player1.Email == playerEmail)
            {
                return game.Player1;
            }

            if (game.Player2.Email == playerEmail)
            {
                return game.Player2;
            }

            throw new InvalidOperationException("No such player.");
        }

        public TargetZone GetOpponentTargetZone(Guid gameId, string requestPlayerEmail)
        {
            Player player;
            var game = FindGameAndPlayer(gameId, requestPlayerEmail, out player);

            return player.TargetZone;
        }

        public ShotResult TakeShot(Guid gameId, string playerTakingShotEmail, GeoCoordinate shotLocation)
        {
            Player player;
            var game = FindGameAndPlayer(gameId, playerTakingShotEmail, out player);

            return game.TakeShot(player, shotLocation);
        }

        public string GetNextPlayerEmail(Guid gameId)
        {
            Game game;
            
            if (games.TryGetValue(gameId, out game))
            {
                return game.NextPlayerToTakeShot.Email;
            }

            return string.Empty;
        }
    }
}

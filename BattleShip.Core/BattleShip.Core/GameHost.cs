using System;
using System.Collections.Generic;

namespace BattleShip.Core
{
    using System.Device.Location;

    public class GameHost
    {
        private readonly Dictionary<Guid, Game> games = new Dictionary<Guid, Game>();

        public const double PlayerTargetZoneRadius = 0.1;

        public const double ShotBlastRadius = 0.01;

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

        public TargetZone GetOpponentTargetZone(Guid gameId, string requestPlayerEmail)
        {
            var game = FindGame(gameId);

            return FindOpponentByPlayerEmail(game, requestPlayerEmail).TargetZone;
        }

        public ShotResult TakeShot(Guid gameId, string playerTakingShotEmail, GeoCoordinate shotLocation)
        {
            Player player;
            var game = FindGameAndPlayer(gameId, playerTakingShotEmail, out player);

            return game.TakeShot(player, shotLocation);
        }

        public string GetNextPlayerEmail(Guid gameId)
        {
            Game game = FindGame(gameId);

            return game.NextPlayerToTakeShot.Email;
        }

        private Game FindGameAndPlayer(Guid gameId, string playerEmail, out Player player)
        {
            Game game = FindGame(gameId);
            player = FindPlayerByEmail(game, playerEmail);

            return game;
        }

        private Game FindGame(Guid gameId)
        {
            Game game;
            if (games.TryGetValue(gameId, out game))
            {
                return game;
            }

            throw new InvalidOperationException("No such game");
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

        private Player FindOpponentByPlayerEmail(Game game, string playerEmail)
        {
            if (game.Player1.Email == playerEmail)
            {
                return game.Player2;
            }

            if (game.Player2.Email == playerEmail)
            {
                return game.Player1;
            }

            throw new InvalidOperationException("No such player.");
        }
    }
}

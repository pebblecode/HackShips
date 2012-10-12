using System;
using System.Collections.Generic;
using System.Linq;

namespace BattleShip.Core
{
    public class GameService
    {
        GameHost gameHost = new GameHost(Enumerable.Empty<Game>());

        public Guid ExecuteCommand(CreateGame createGame)
        {
            var guid = Guid.NewGuid();

            var gameCreated = new GameCreated(
                guid,
                createGame.GameName,
                createGame.InitiatingPlayerName,
                createGame.InitiatingPlayerEmail,
                createGame.AcceptingPlayerName,
                createGame.AcceptingPlayerEmail);

            gameHost = gameHost.Apply(gameCreated);

            return guid;
        }        
    }

    public class GameHost
    {
        private readonly Dictionary<Guid, Game> games = new Dictionary<Guid, Game>();

        public const double PlayerTargetZoneRadius = 0.05;

        public const double ShotBlastRadius = 0.015;

        public GameHost(IEnumerable<Game> games)
        {
            this.games = games.ToDictionary(g => g.Id, g => g);
        }

        public GameHost Apply(GameCreated gameCreated)
        {
            var initiatingPlayer = new Player(gameCreated.InitiatingPlayerEmail, gameCreated.InitiatingPlayerName, null, PlayerTargetZoneRadius);
            var acceptingPlayer = new Player(gameCreated.AcceptingPlayerEmail, gameCreated.AcceptingPlayerName, null, PlayerTargetZoneRadius);

            var game = new Game(gameCreated.GameName, acceptingPlayer, initiatingPlayer, null, PlayerTargetZoneRadius, ShotBlastRadius);

            return new GameHost(games.Values.Concat(new[] { game }));
        }

        public GameHost Apply(PlayerLocationUpdated playerLocationUpdated)
        {
            var game = FindGame(playerLocationUpdated.GameId);

            var newGame = game.Apply(playerLocationUpdated);

            return UpdateGame(game);
        }

        public GameHost Apply(ShotTaken shotTaken)
        {
            var game = FindGame(shotTaken.GameId);

            var newGame = game.Apply(shotTaken);

            return UpdateGame(game);
        }

        public TargetZone GetOpponentTargetZone(Guid gameId, string requestPlayerEmail)
        {
            var game = FindGame(gameId);

            return FindOpponentByPlayerEmail(game, requestPlayerEmail).TargetZone;
        }

        private GameHost UpdateGame(Game newGame)
        {
            var newGames = games.Values.Select(g => g.Id == newGame.Id ? newGame : g);

            return new GameHost(newGames);
        }

        public string GetNextPlayerEmail(Guid gameId)
        {
            Game game = FindGame(gameId);

            return game.NextPlayerToTakeShot.Email;
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

        private Player FindOpponentByPlayerEmail(Game game, string playerEmail)
        {
            if (game.NextPlayerToTakeShot.Email == playerEmail)
            {
                return game.NextPlayerToTakeShot;
            }

            if (game.Opponent.Email == playerEmail)
            {
                return game.Opponent;
            }

            throw new InvalidOperationException("No such player.");
        }
    }
}

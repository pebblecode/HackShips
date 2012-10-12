namespace BattleShip.Core
{
    using System;

    public class GameCreated
    {
        public Guid Id { get; private set; }

        public string GameName { get; private set; }

        public string InitiatingPlayerName { get; private set; }

        public string InitiatingPlayerEmail { get; private set; }

        public string AcceptingPlayerName { get; private set; }

        public string AcceptingPlayerEmail { get; private set; }

        public GameCreated(Guid id, string gameName, string initiatingPlayerName, string initiatingPlayerEmail, string acceptingPlayerName, string acceptingPlayerEmail)
        {
            Id = id;
            GameName = gameName;
            InitiatingPlayerName = initiatingPlayerName;
            InitiatingPlayerEmail = initiatingPlayerEmail;
            AcceptingPlayerName = acceptingPlayerName;
            AcceptingPlayerEmail = acceptingPlayerEmail;
        }
    }
}
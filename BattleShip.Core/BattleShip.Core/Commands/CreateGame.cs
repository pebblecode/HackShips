namespace BattleShip.Core
{
    public class CreateGame
    {
        public string GameName { get; private set; }

        public string InitiatingPlayerName { get; private set; }

        public string InitiatingPlayerEmail { get; private set; }

        public string AcceptingPlayerName { get; private set; }

        public string AcceptingPlayerEmail { get; private set; }

        public CreateGame(string gameName, string initiatingPlayerName, string initiatingPlayerEmail, string acceptingPlayerName, string acceptingPlayerEmail)
        {
            this.GameName = gameName;
            this.InitiatingPlayerName = initiatingPlayerName;
            this.InitiatingPlayerEmail = initiatingPlayerEmail;
            this.AcceptingPlayerName = acceptingPlayerName;
            this.AcceptingPlayerEmail = acceptingPlayerEmail;
        }
    }
}
using System.Collections.Generic;

public static partial class GameLobbyUpdatesParser
{
    public struct Update
    {
        public List<GameLobbyPlayerData> Players { get; set; }
        public string OwnerId { get; set; }
    }
}
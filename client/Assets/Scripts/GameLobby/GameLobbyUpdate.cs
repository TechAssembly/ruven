using System.Collections.Generic;

public struct GameLobbyUpdate
{
    public IList<GameLobbyPlayerData> Players { get; set; }
    public IDictionary<string, IList<string>> Teams { get; set; }
    public string OwnerId { get; set; }
}
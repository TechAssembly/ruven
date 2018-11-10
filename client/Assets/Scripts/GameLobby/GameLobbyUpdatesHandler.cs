using System.Linq;
using GameDevWare.Serialization;
using System.Collections.Generic;

public static class GameLobbyUpdatesHandler
{
    static IndexedDictionary<string, object> ReadObject(object obj) => obj as IndexedDictionary<string, object>;

    public static List<GameLobbyPlayerData> ReadPlayers(IndexedDictionary<string, object> changes)
    {
        return ReadObject(changes["players"]).Values.Select(ReadPlayer).ToList();
    }

    public static GameLobbyPlayerData ReadPlayer(object playerEntry)
    {
        var player = ReadObject(playerEntry);
        var playerData = new GameLobbyPlayerData
        {
            Id = player["id"] as string,
            Name = player["name"] as string,
            Ready = player["ready"] as bool? == true,
        };
        return playerData;
    }
}
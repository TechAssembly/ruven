using System.Linq;
using GameDevWare.Serialization;
using System.Collections.Generic;

public static class GameLobbyUpdatesParser
{
    public struct Update
    {
        public List<GameLobbyPlayerData> Players { get; set; }
        public string OwnerId { get; set; }
    }

    static IndexedDictionary<string, object> ReadObject(object obj) => obj as IndexedDictionary<string, object>;

    public static Update Parse(IndexedDictionary<string, object> changes)
    {
        var ownerId = changes["roomOwner"] as string;
        var players = ReadObject(changes["players"]).Values
                .Select(player => ReadPlayer(player, ownerId)).ToList();

        return new Update { Players = players, OwnerId = ownerId };
    }

    public static GameLobbyPlayerData ReadPlayer(object playerEntry, string ownerId)
    {
        var player = ReadObject(playerEntry);
        var id = player["id"] as string;
        var playerData = new GameLobbyPlayerData
        {
            Id = id,
            Name = player["name"] as string,
            Ready = player["ready"] as bool? == true,
            Owner = id == ownerId,
        };
        return playerData;
    }
}
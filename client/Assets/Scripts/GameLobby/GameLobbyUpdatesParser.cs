using System.Collections.Generic;
using System.Linq;
using GameDevWare.Serialization;

public static class GameLobbyUpdatesParser
{
    static IndexedDictionary<string, object> ReadObject(object obj) => obj as IndexedDictionary<string, object>;

    public static GameLobbyUpdate Parse(IndexedDictionary<string, object> changes)
    {
        var ownerId = changes["roomOwner"] as string;
        var players = ReadObject(changes["players"]).Values
                .Select(player => ReadPlayer(player, ownerId)).ToList();
        var teams = changes.ContainsKey("teams") ? ReadTeams(ReadObject(changes["teams"])) : null;

        return new GameLobbyUpdate
        {
            Players = players,
            Teams = teams,
            OwnerId = ownerId
        };
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

    public static IDictionary<string, IList<string>> ReadTeams(IndexedDictionary<string, object> teamsObj)
    {
        if (teamsObj == null)
        {
            return null;
        }

        Dictionary<string, IList<string>> teamToPlayerMapping = new Dictionary<string, IList<string>>();
        foreach (var teamObj in teamsObj)
        {
            var team = ReadObject(teamObj.Value);
            var players = team["players"] as IList<object>;
            var playerIds = players.Select(ReadObject).Select(p => p["id"] as string).ToList();
            teamToPlayerMapping[teamObj.Key] = playerIds;
        }
        return teamToPlayerMapping;
    }
}
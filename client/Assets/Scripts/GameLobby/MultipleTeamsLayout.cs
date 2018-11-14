using UnityEngine;
using System.Collections.Generic;
using GameDevWare.Serialization;
using System.Linq;

public class MultipleTeamsLayout : BaseTeamLayout
{
    GameObject playerListPrefab;
    Dictionary<string, PlayersListLayoutGroup> Teams { get; } = new Dictionary<string, PlayersListLayoutGroup>();

    public override void CreateLayout(GameObject playerListPrefab)
    {
        this.playerListPrefab = playerListPrefab;
    }

    public override GameLobbyUpdate UpdateTeams(IndexedDictionary<string, object> state)
    {
        var update = GameLobbyUpdatesParser.Parse(state);
        if (Teams.Count == 0)
        {
            CreateLayoutWithTeams(update.Teams);
        }
        foreach (var team in Teams)
        {
            var players = update.Players.Where(p => update.Teams[team.Key].Contains(p.Id)).ToList();
            team.Value.HandlePlayersList(players);
        }
        return update;
    }

    void CreateLayoutWithTeams(IDictionary<string, IList<string>> teams)
    {
        foreach (var teamId in teams.Keys)
        {
            var listingGameObject = Instantiate(playerListPrefab, transform, false);
            Teams[teamId] = listingGameObject.GetComponentInChildren<PlayersListLayoutGroup>();
        }
    }
}

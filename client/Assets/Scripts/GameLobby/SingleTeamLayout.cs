﻿using GameDevWare.Serialization;
using UnityEngine;

public class SingleTeamLayout : BaseTeamLayout
{
    PlayersListLayoutGroup layoutGroup;

    public override void CreateLayout(GameObject playerListPrefab)
    {
        var listingGameObject = Instantiate(playerListPrefab, transform, false);
        layoutGroup = listingGameObject.GetComponentInChildren<PlayersListLayoutGroup>();
    }

    public override GameLobbyUpdate UpdateTeams(IndexedDictionary<string, object> state)
    {
        var update = GameLobbyUpdatesParser.Parse(state);
        layoutGroup.HandlePlayersList(update.Players);
        return update;
    }
}

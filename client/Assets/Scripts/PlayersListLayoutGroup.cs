using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayersListLayoutGroup : MonoBehaviour
{
    public GameObject playerListingPrefab;
    public List<GameLobbyPlayerListing> playerListings;

    public void HandlePlayersList(List<GameLobbyPlayerData> players)
    {
        var playerIds = players.Select(room => room.Id);
        var playersToDelete = playerListings.Where(room => !playerIds.Contains(room.Id)).ToList();
        players.ForEach(UpdatePlayer);
        playersToDelete.ForEach(DeletePlayer);
    }

    void DeletePlayer(GameLobbyPlayerListing listing)
    {
        Debug.LogFormat("Removing player {0}({1})", listing.Name, listing.Id);
        playerListings.Remove(listing);
        Destroy(listing.gameObject);
    }

    void UpdatePlayer(GameLobbyPlayerData playerData)
    {
        var listing = playerListings.FirstOrDefault(l => l.Id == playerData.Id);
        if (listing == null)
        {
            listing = CreateNewListing();
        }
        listing.UpdatePlayer(playerData);
    }

    GameLobbyPlayerListing CreateNewListing()
    {
        var listingGameObject = Instantiate(playerListingPrefab, transform, false);
        var newListing = listingGameObject.GetComponent<GameLobbyPlayerListing>();
        playerListings.Add(newListing);
        return newListing;
    }
}

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoomLayoutGroup : MonoBehaviour
{
    public GameObject roomListingPrefab;
    public List<RoomListing> roomListings;

    public void HandleRoomsList(List<RoomData> rooms)
    {
        var roomIds = rooms.Select(room => room.Id);
        var roomsToDelete = roomListings.Where(room => !roomIds.Contains(room.Id)).ToList();
        rooms.ForEach(UpdateRoom);
        roomsToDelete.ForEach(DeleteRoom);
    }

    void DeleteRoom(RoomListing listing)
    {
        Debug.LogFormat("Removing room {0}({1})", listing.Name, listing.Id);
        roomListings.Remove(listing);
        Destroy(listing.gameObject);
    }

    void UpdateRoom(RoomData roomData)
    {
        var listing = roomListings.FirstOrDefault(l => l.Id == roomData.Id);
        if (listing == null)
        {
            listing = CreateNewListing();
        }
        listing.UpdateRoom(roomData);
    }

    RoomListing CreateNewListing()
    {
        var listingGameObject = Instantiate(roomListingPrefab, transform, false);
        var newListing = listingGameObject.GetComponent<RoomListing>();
        Debug.LogFormat("Creating room {0}({1})", newListing.Name, newListing.Id);
        roomListings.Add(newListing);
        return newListing;
    }
}

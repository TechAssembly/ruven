using UnityEngine;
using UnityEngine.UI;


public class RoomListing : MonoBehaviour
{
    public Text roomNameText;
    public Text roomModeText;
    public Text roomClientsNumberText;

    RoomData roomData;

    public string Id => roomData?.Id;
    public string Name => roomData?.Name;

    public void UpdateRoom(RoomData room)
    {
        roomData = room;
        roomNameText.text = roomData.Name;
        roomModeText.text = RoomData.FormatGameMode(roomData.Mode);
        roomClientsNumberText.text = $"{roomData.CurrentClients} / {roomData.MaxClients}";
    }

    public void JoinRoom()
    {
        FindObjectOfType<ColyseusLobbyManager>().JoinRoom(roomData);
    }


}

using UnityEngine;
using UnityEngine.UI;

public class GameLobbyInfo : MonoBehaviour
{
    public Text gameTypeText;
    public Text playerCountTitleText;
    public Text playerCountText;

    void Update()
    {
        var room = ColyseusRoom.Instance;
        if (room == null)
            return;

        RoomData data = room.RoomData;
        if (data == null)
            return;

        UpdateRoomInfo(data);
    }

    private void UpdateRoomInfo(RoomData data)
    {
        gameTypeText.text = RoomData.FormatGameMode(data.Mode);
        if (data.Mode == RoomData.GameMode.FreeForAll)
        {
            playerCountTitleText.text = "Player Count";
            playerCountText.text = data.MaxClients.ToString();
        }
        else
        {
            playerCountTitleText.text = "Team Size";
            playerCountText.text = string.Format("{0} vs {0}", data.MaxClients / 2);
        }
    }
}

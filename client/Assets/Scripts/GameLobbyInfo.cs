using UnityEngine;
using UnityEngine.UI;

public class GameLobbyInfo : MonoBehaviour
{
    public Text gameTypeText;
    public Text playerCountTitleText;
    public Text playerCountText;

    void Update()
    {
        RoomData data = ColyseusRoom.Instance.RoomData;
        if (data == null)
            return;

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

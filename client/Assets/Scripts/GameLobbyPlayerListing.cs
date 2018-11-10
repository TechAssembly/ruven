using UnityEngine;
using UnityEngine.UI;

public class GameLobbyPlayerListing : MonoBehaviour
{
    public Text playerNameText;
    public Toggle playerReadyToggle;

    GameLobbyPlayerData playerData;

    public string Id => playerData?.Id;
    public string Name => playerData?.Name;

    public void UpdatePlayer(GameLobbyPlayerData player)
    {
        playerData = player;
        playerNameText.text = playerData.Name;
        playerReadyToggle.isOn = playerData.Ready;
    }
}

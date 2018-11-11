using UnityEngine;
using UnityEngine.UI;

public class GameLobbyPlayerListing : MonoBehaviour
{
    static readonly Color NormalColor = new Color(255, 255, 255, 228);
    static readonly Color OwnClientColor = new Color(0, 246, 120, 228);

    public Image backgroundImage;
    public Text playerNameText;
    public Toggle playerReadyToggle;
    public Image ownerImage;

    GameLobbyPlayerData playerData;

    public string Id => playerData?.Id;
    public string Name => playerData?.Name;

    public void UpdatePlayer(GameLobbyPlayerData player)
    {
        playerData = player;
        playerNameText.text = playerData.Name;
        playerReadyToggle.isOn = playerData.Ready;
        backgroundImage.color = Id == ColyseusRoom.Instance.PlayerId ? OwnClientColor : NormalColor;
        ownerImage.enabled = player.Owner;
    }
}

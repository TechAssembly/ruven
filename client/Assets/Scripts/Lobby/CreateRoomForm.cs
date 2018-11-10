using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateRoomForm : MonoBehaviour
{
    static readonly Dictionary<int, int> TeamSizeToPlayerCount = new Dictionary<int, int>
    {
        { 0, 6 },
        { 1, 8 },
        { 2, 10 },
    };

    public Text playerCountPreviewText;
    public Slider playerCountSlider;
    public Dropdown teamSizeDropdown;
    public Dropdown gameTypeDropdown;

    public GameObject playerCountField;
    public GameObject teamSizeField;

    public ColyseusLobbyManager colyseusLobbyManager;

    void Start()
    {
        OnGameTypeChange();
        OnPlayerCountChange();
    }

    RoomData.GameMode CurrentGameType => (RoomData.GameMode)gameTypeDropdown.value;

    public void OnGameTypeChange()
    {
        var gameType = CurrentGameType;
        Debug.Log("Set game type to " + gameType.ToString());
        playerCountField.SetActive(gameType == RoomData.GameMode.FreeForAll);
        teamSizeField.SetActive(gameType == RoomData.GameMode.TeamDeathmatch);
    }

    public void OnPlayerCountChange()
    {
        var actualCount = (int)playerCountSlider.value;
        playerCountPreviewText.text = actualCount.ToString();
    }

    public void CreateRoom()
    {
        int maxClients;
        if (CurrentGameType == RoomData.GameMode.FreeForAll)
            maxClients = (int)playerCountSlider.value;
        else
            maxClients = TeamSizeToPlayerCount[teamSizeDropdown.value];
        colyseusLobbyManager.CreateRoom(CurrentGameType, maxClients);
    }
}

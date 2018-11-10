using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateRoomForm : MonoBehaviour
{
    enum GameType
    {
        TDM = 0,
        FFA = 1,
    }

    static readonly Dictionary<GameType, string> LobbyNames = new Dictionary<GameType, string>
    {
        { GameType.TDM, "team_deathmatch_lobby" },
        { GameType.FFA, "free_for_all_lobby" },
    };

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

    GameType CurrentGameType => (GameType)gameTypeDropdown.value;

    public void OnGameTypeChange()
    {
        var gameType = CurrentGameType;
        Debug.Log("Set game type to " + gameType.ToString());
        playerCountField.SetActive(gameType == GameType.FFA);
        teamSizeField.SetActive(gameType == GameType.TDM);
    }

    public void OnPlayerCountChange()
    {
        var actualCount = (int)playerCountSlider.value;
        playerCountPreviewText.text = actualCount.ToString();
    }

    public void CreateRoom()
    {
        string lobbyName = LobbyNames[(GameType)gameTypeDropdown.value];
        int maxClients;
        if (CurrentGameType == GameType.FFA)
            maxClients = (int)playerCountSlider.value;
        else
            maxClients = TeamSizeToPlayerCount[teamSizeDropdown.value];
        colyseusLobbyManager.CreateRoom(lobbyName, maxClients);
    }
}

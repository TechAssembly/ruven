using UnityEngine;
using UnityEngine.UI;

public class GameLobbyActions : MonoBehaviour
{
    public ColyseusGameLobby lobby;

    public Button readyButton;
    public Button startButton;

    void Start()
    {
        startButton.gameObject.SetActive(false);
    }

    void Update()
    {
        if (lobby.IsReady)
        {
            readyButton.interactable = false;
        }
        if (lobby.IsReady && lobby.IsOwner)
        {
            readyButton.gameObject.SetActive(false);
            startButton.gameObject.gameObject.SetActive(true);
            startButton.interactable = lobby.AllPlayersReady;
        }
    }
}

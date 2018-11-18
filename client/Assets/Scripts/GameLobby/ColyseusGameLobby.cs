using System;
using System.Linq;
using Colyseus;
using GameDevWare.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ColyseusGameLobby : MonoBehaviour
{
    public GameObject teamListPrefab;
    public Transform teamListHolder;

    BaseTeamLayout teamLayout;

    public bool IsOwner { get; private set; }
    public bool AllPlayersReady { get; private set; }
    public bool IsReady { get; private set; }

    void Start()
    {
        ColyseusRoom.Instance.OnJoin += Room_OnJoin;
        ColyseusRoom.Instance.OnError += Room_OnError;
        ColyseusRoom.Instance.OnLeave += Room_OnLeave;
        ColyseusRoom.Instance.OnMessage += Room_OnMessage;
        ColyseusRoom.Instance.OnStateChange += Room_OnStateChange;
    }

    void Unsubscribe()
    {
        ColyseusRoom.Instance.OnJoin -= Room_OnJoin;
        ColyseusRoom.Instance.OnError -= Room_OnError;
        ColyseusRoom.Instance.OnLeave -= Room_OnLeave;
        ColyseusRoom.Instance.OnMessage -= Room_OnMessage;
        ColyseusRoom.Instance.OnStateChange -= Room_OnStateChange;
    }

    public void SendPlayerReady()
    {
        ColyseusRoom.Instance.Room.Send(new IndexedDictionary<string, object>
        {
            {"action", "ready"},
        });
    }

    public void SendStartGame()
    {
        if (!IsOwner || !AllPlayersReady)
            return;

        ColyseusRoom.Instance.Room.Send(new IndexedDictionary<string, object>
        {
            {"action", "start"},
        });
    }

    void Room_OnJoin(object sender, EventArgs e)
    {
        Debug.Log("Joined room!!!");
        teamLayout = CreateTeamLayout(ColyseusRoom.Instance.RoomData.Mode);
        teamLayout.CreateLayout(teamListPrefab);
    }

    BaseTeamLayout CreateTeamLayout(RoomData.GameMode gameMode)
    {
        if (gameMode == RoomData.GameMode.FreeForAll)
        {
            return teamListHolder.gameObject.AddComponent<SingleTeamLayout>();
        }
        return teamListHolder.gameObject.AddComponent<MultipleTeamsLayout>();
    }

    void Room_OnError(object sender, ErrorEventArgs e)
    {
        Debug.LogError("Room error = " + e);
    }

    void Room_OnLeave(object sender, EventArgs e)
    {
        Debug.Log("Leaving room - unsubscribing all events");
        Unsubscribe();
    }

    void Room_OnStateChange(object sender, RoomUpdateEventArgs e)
    {
        Debug.Log("OnStateChange");
        Debug.Log("Is First State = " + e.isFirstState);
        Debug.Log("State = " + e.state);

        var update = teamLayout.UpdateTeams(e.state);
        string myId = ColyseusConnector.Instance.Client.id;
        IsOwner = myId == update.OwnerId;
        IsReady = update.Players.FirstOrDefault(p => p.Id == myId)?.Ready ?? false;
        AllPlayersReady = update.Players.All(p => p.Ready);
    }

    void Room_OnMessage(object sender, MessageEventArgs e)
    {
        var message = e.message as IndexedDictionary<string, object>;
        var action = message["action"] as string;
        if (action != "join_game")
        {
            Debug.LogError("Received GameLobby message other than to start a game");
            return;
        }
        var roomId = message["roomId"] as string;
        Debug.Log("Received START_GAME with ROOM_ID = " + roomId);
        ColyseusRoom.Instance.JoinRoom(roomId);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}

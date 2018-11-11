using System;
using System.Linq;
using Colyseus;
using GameDevWare.Serialization;
using UnityEngine;

public class ColyseusGameLobby : MonoBehaviour
{
    public PlayersListLayoutGroup playersListLayoutGroup;

    [HideInInspector]
    public bool IsOwner { get; private set; }
    [HideInInspector]
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

        var update = GameLobbyUpdatesParser.Parse(e.state);
        playersListLayoutGroup.HandlePlayersList(update.Players);
        string myId = ColyseusRoom.Instance.Room.sessionId;
        IsOwner = myId == update.OwnerId;
        IsReady = update.Players.FirstOrDefault(p => p.Id == myId)?.Ready ?? false;
        AllPlayersReady = update.Players.All(p => p.Ready);
    }

    void Room_OnMessage(object sender, MessageEventArgs e)
    {
        Debug.Log("Received message = " + e);
    }
}

using System;
using Colyseus;
using GameDevWare.Serialization;
using UnityEngine;

public class ColyseusGameLobby : MonoBehaviour
{
    public PlayersListLayoutGroup playersListLayoutGroup;

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
        var players = GameLobbyUpdatesHandler.ReadPlayers(e.state);
        playersListLayoutGroup.HandlePlayersList(players);
    }

    void Room_OnMessage(object sender, MessageEventArgs e)
    {
        Debug.Log("Received message = " + e);
    }
}

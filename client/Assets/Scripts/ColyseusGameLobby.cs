using System;
using Colyseus;
using GameDevWare.Serialization;
using UnityEngine;

public class ColyseusGameLobby : MonoBehaviour
{
    public PlayersListLayoutGroup playersListLayoutGroup;

    void Start()
    {
        ColyseusRoom.Instance.OnJoin += Instance_OnJoin;
        ColyseusRoom.Instance.OnError += Instance_OnError;
        ColyseusRoom.Instance.OnLeave += Instance_OnLeave;
        ColyseusRoom.Instance.OnMessage += Instance_OnMessage;
        ColyseusRoom.Instance.OnStateChange += Instance_OnStateChange;
        ColyseusRoom.Instance.Room.Listen("players/:id", HandleAction);
    }

    void Unsubscribe()
    {
        ColyseusRoom.Instance.OnJoin -= Instance_OnJoin;
        ColyseusRoom.Instance.OnError -= Instance_OnError;
        ColyseusRoom.Instance.OnLeave -= Instance_OnLeave;
        ColyseusRoom.Instance.OnMessage -= Instance_OnMessage;
        ColyseusRoom.Instance.OnStateChange -= Instance_OnStateChange;
    }

    public void SendPlayerReady()
    {
        ColyseusRoom.Instance.Room.Send(new IndexedDictionary<string, object>
        {
            {"action", "ready"},
        });
    }

    void Instance_OnJoin(object sender, EventArgs e)
    {
        Debug.Log("Joined room!!!");
    }

    void Instance_OnError(object sender, ErrorEventArgs e)
    {
        Debug.LogError("Room error = " + e);
    }

    void Instance_OnLeave(object sender, EventArgs e)
    {
        Debug.Log("Leaving room - unsubscribing all events");
        Unsubscribe();
    }

    void Instance_OnStateChange(object sender, RoomUpdateEventArgs e)
    {
        Debug.Log("OnStateChange");
        Debug.Log("Is First State = " + e.isFirstState);
        Debug.Log("State = " + e.state);
        var players = GameLobbyUpdatesHandler.ReadPlayers(e.state);
        playersListLayoutGroup.HandlePlayersList(players);
    }

    void Instance_OnMessage(object sender, MessageEventArgs e)
    {
        Debug.Log("Received message = " + e);
    }
}

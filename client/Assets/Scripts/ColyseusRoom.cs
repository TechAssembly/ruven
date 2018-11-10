using UnityEngine;
using Colyseus;
using System;
using System.Collections.Generic;

public class ColyseusRoom : MonoBehaviour
{
    Room room;

    public event EventHandler OnJoin;
    public event EventHandler<ErrorEventArgs> OnError;
    public event EventHandler OnLeave;
    public event EventHandler<MessageEventArgs> OnMessage;
    public event EventHandler<RoomUpdateEventArgs> OnStateChange;

    public void JoinRoom(RoomData roomData)
    {
        if (room != null && room.id == roomData.Id)
        {
            Debug.Log("Already joined room " + roomData.Id);
            return;
        }

        room?.Leave();
        room = ColyseusConnector.Instance.Client.Join(roomData.Id);
        ConnectToRoom();
    }

    public void CreateRoom(string lobbyName, int maxClients)
    {
        room?.Leave();
        room = ColyseusConnector.Instance.Client.Join(lobbyName, new Dictionary<string, object>
        {
            { "forceCreate", true },
            { "maxClients", maxClients },
        });
        ConnectToRoom();
    }

    void ConnectToRoom()
    {
        room.OnReadyToConnect += (sender, e) =>
        {
            Debug.Log("Ready to connect to room!");
            StartCoroutine(room.Connect());
        };

        room.OnJoin += Room_OnJoin;
        room.OnMessage += Room_OnMessage;
        room.OnLeave += Room_OnLeave;
        room.OnError += Room_OnError;
        room.OnStateChange += Room_OnStateChange;
    }

    void Room_OnMessage(object sender, MessageEventArgs e)
    {
        Debug.Log("Recieved message:" + e.message);
        OnMessage?.Invoke(sender, e);
    }

    void Room_OnJoin(object sender, EventArgs e)
    {
        Debug.Log("Joined room!");
        OnJoin?.Invoke(sender, e);
    }

    void Room_OnLeave(object sender, EventArgs e)
    {
        Debug.Log("Left room!");
        OnLeave?.Invoke(sender, e);
    }

    void Room_OnError(object sender, ErrorEventArgs e)
    {
        Debug.LogError("Room error");
        Debug.LogError(e);
        OnError?.Invoke(sender, e);
    }

    void Room_OnStateChange(object sender, RoomUpdateEventArgs e)
    {
        OnStateChange?.Invoke(sender, e);
    }

    #region Singleton Component Implementation

    public static ColyseusRoom Instance
    {
        get; private set;
    }

    void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnDestroy()
    {
        if (this != Instance || room == null)
            return;

        room.Leave();
    }

    #endregion Singleton Component Implementation
}

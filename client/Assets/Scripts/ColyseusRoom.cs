using UnityEngine;
using Colyseus;
using System;
using System.Collections.Generic;

public class ColyseusRoom : MonoBehaviour
{
    public Room Room { get; private set; }
    public RoomData RoomData { get; private set; }

    public event EventHandler OnJoin;
    public event EventHandler<ErrorEventArgs> OnError;
    public event EventHandler OnLeave;
    public event EventHandler<MessageEventArgs> OnMessage;
    public event EventHandler<RoomUpdateEventArgs> OnStateChange;

    public string PlayerId => ColyseusConnector.Instance.Client.id;

    string PlayerName => PlayerPrefs.GetString("ColyseusPlayerName", PlayerId ?? "Unnamed");

    public void JoinRoom(RoomData roomData)
    {
        if (Room != null && Room.id == roomData.Id)
        {
            Debug.Log("Already joined room " + roomData.Id);
            return;
        }

        Room?.Leave();
        RoomData = roomData;
        Room = ColyseusConnector.Instance.Client.Join(roomData.Id, new Dictionary<string, object>
        {
            { "name", PlayerName }
        });
        ConnectToRoom();
    }

    public void JoinRoom(string roomIdOrLobbyName)
    {
        if (Room != null && Room.id == roomIdOrLobbyName)
        {
            Debug.Log("Already joined room " + roomIdOrLobbyName);
            return;
        }

        Room?.Leave();
        RoomData = null;
        Room = ColyseusConnector.Instance.Client.Join(roomIdOrLobbyName);
        ConnectToRoom();
    }

    public void CreateRoom(RoomData.GameMode gameMode, int maxClients)
    {
        Room?.Leave();

        Room = ColyseusConnector.Instance.Client.Join(RoomData.GameModeToLobbyName[gameMode], new Dictionary<string, object>
        {
            { "forceCreate", true },
            { "maxClients", maxClients },
        });
        RoomData = new RoomData
        {
            Mode = gameMode,
            MaxClients = maxClients,
        };
        ConnectToRoom();
    }

    void ConnectToRoom()
    {
        Room.OnReadyToConnect += (sender, e) =>
        {
            Debug.Log("Ready to connect to room!");
            StartCoroutine(Room.Connect());
        };

        Room.OnJoin += Room_OnJoin;
        Room.OnMessage += Room_OnMessage;
        Room.OnLeave += Room_OnLeave;
        Room.OnError += Room_OnError;
        Room.OnStateChange += Room_OnStateChange;
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
        if (this != Instance || Room == null)
            return;

        Room.Leave();
    }

    #endregion Singleton Component Implementation
}

using System;
using System.Linq;
using System.Collections;
using Colyseus;
using UnityEngine;
using System.Collections.Generic;

public class ColyseusLobbyManager : MonoBehaviour
{
    public RoomLayoutGroup roomsLayout;
    public int roomsRefreshInterval = 1;

    readonly AvailableRooms availableRooms = new AvailableRooms();

    Room room;

    IEnumerator Start()
    {
        yield return StartCoroutine(ColyseusConnector.Instance.EnsureClientOpen());
        yield return StartCoroutine(GetAvailableRooms());
    }

    IEnumerator GetAvailableRooms()
    {
        QueryForAvilableRooms("team_deathmatch_lobby", "Team Deathmatch");
        QueryForAvilableRooms("free_for_all_lobby", "Free For All");
        yield return new WaitForSeconds(roomsRefreshInterval);
        roomsLayout.HandleRoomsList(availableRooms.GetAllRooms());
        StartCoroutine(GetAvailableRooms());
    }

    void QueryForAvilableRooms(string lobbyName, string visualRoomName)
    {
        void callback(RoomAvailable[] rooms)
        {
            var wrappedRooms = rooms.ToList()
                .Select(room => RoomData.FromColyseusRoom(visualRoomName, room)).ToList();
            availableRooms.UpdateRoomList(lobbyName, wrappedRooms);
        }
        ColyseusConnector.Instance.Client.GetAvailableRooms(lobbyName, callback);
    }

    internal void JoinRoom(RoomData roomData)
    {
        if (room != null && room.id == roomData.Id)
        {
            Debug.Log("Already joined room " + roomData.Id);
            return;
        }

        if (room != null)
        {
            room.Leave();
        }

        room = ColyseusConnector.Instance.Client.Join(roomData.Id);
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
    }

    internal void CreateRoom(string lobbyName, int maxClients)
    {
        room = ColyseusConnector.Instance.Client.Join(lobbyName, new Dictionary<string, object>
        {
            { "forceCreate", true },
            { "maxClients", maxClients },
        });
        ConnectToRoom();
    }

    void Room_OnMessage(object sender, MessageEventArgs e)
    {
        Debug.Log("Recieved message:" + e.message);
    }

    void Room_OnJoin(object sender, EventArgs e)
    {
        Debug.Log("Joined room!");
    }

}

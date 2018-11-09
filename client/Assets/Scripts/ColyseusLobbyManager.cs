using System;
using System.Linq;
using System.Collections;
using Colyseus;
using UnityEngine;

public class ColyseusLobbyManager : MonoBehaviour
{
    public RoomLayoutGroup roomsLayout;

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
        yield return new WaitForSeconds(2);
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
        if (room != null)
        {
            room.Leave();
        }

        room = ColyseusConnector.Instance.Client.Join(roomData.Id);
        room.OnReadyToConnect += (sender, e) =>
        {
            Debug.Log("Ready to connect to room!");
            StartCoroutine(room.Connect());
        };
        room.OnJoin += Room_OnJoin;
        room.OnMessage += Room_OnMessage;
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

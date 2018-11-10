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
    static readonly Dictionary<string, string> LobbyNameToGameType = new Dictionary<string, string>
    {
        {"team_deathmatch_lobby", "Team Deathmatch"},
        {"free_for_all_lobby", "Free For All"},
    };

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
        ColyseusRoom.Instance.JoinRoom(roomData);
    }

    internal void CreateRoom(string lobbyName, int maxClients)
    {
        ColyseusRoom.Instance.CreateRoom(lobbyName, maxClients);
    }

}

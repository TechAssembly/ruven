using System.Linq;
using System.Collections;
using Colyseus;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ColyseusLobbyManager : MonoBehaviour
{
    public RoomLayoutGroup roomsLayout;
    public int roomsRefreshInterval = 1;

    readonly AvailableRooms availableRooms = new AvailableRooms();

    IEnumerator Start()
    {
        yield return StartCoroutine(ColyseusConnector.Instance.EnsureClientOpen());
        yield return StartCoroutine(GetAvailableRooms());
    }

    IEnumerator GetAvailableRooms()
    {
        QueryForAvilableRooms(RoomData.GameMode.TeamDeathmatch);
        QueryForAvilableRooms(RoomData.GameMode.FreeForAll);
        yield return new WaitForSeconds(roomsRefreshInterval);
        roomsLayout.HandleRoomsList(availableRooms.GetAllRooms());
        StartCoroutine(GetAvailableRooms());
    }

    void QueryForAvilableRooms(RoomData.GameMode gameMode)
    {
        var lobbyName = RoomData.GameModeToLobbyName[gameMode];
        void callback(RoomAvailable[] rooms)
        {
            var wrappedRooms = rooms.ToList()
                .Select(room => RoomData.FromColyseusRoom(gameMode, room)).ToList();
            availableRooms.UpdateRoomList(lobbyName, wrappedRooms);
        }
        ColyseusConnector.Instance.Client.GetAvailableRooms(lobbyName, callback);
    }

    internal void JoinRoom(RoomData roomData)
    {
        ColyseusRoom.Instance.JoinRoom(roomData);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    internal void CreateRoom(RoomData.GameMode gameMode, int maxClients)
    {
        ColyseusRoom.Instance.CreateRoom(gameMode, maxClients);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

}

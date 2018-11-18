using System;
using System.Collections;
using System.Collections.Generic;
using Colyseus;
using GameDevWare.Serialization;
using UnityEngine;

public class ColyseusGame : MonoBehaviour
{
    public GameObject dummyPlayerPrefab;

    IEnumerator Start()
    {
        yield return StartCoroutine(ColyseusConnector.Instance.EnsureClientOpen());
        ColyseusRoom.Instance.Room.Listen("players/:id", Room_OnPlayerLeave);
        ColyseusRoom.Instance.Room.Listen("players/:id/playerGameState", Room_OnPlayerGameState);
        ColyseusRoom.Instance.Room.Listen("players/:id/playerGameState/:stat", Room_OnPlayerGameStateStat);
    }

    public void OnPlayerStateChange(PlayerGameState player)
    {
        ColyseusRoom.Instance?.Room?.Send(new IndexedDictionary<string, object>
        {
            {"action", "gameStateChange"},
            {"data", player.ToColyseus()},
        });
    }

    readonly Dictionary<string, GameObject> players = new Dictionary<string, GameObject>();
    readonly Dictionary<string, IDictionary<string, object>> playerData =
        new Dictionary<string, IDictionary<string, object>>();

    void Room_OnPlayerGameState(DataChange change)
    {
        var playerId = change.path["id"];
        if (playerId == ColyseusRoom.Instance.PlayerId)
            return;

        if (change.operation == "add")
        {
            Debug.Log("Adding player " + playerId);
            GameObject cube = Instantiate(dummyPlayerPrefab);
            players[playerId] = cube;
            AddPlayer(playerId, change, cube);
        }

        if (change.operation == "replace")
        {
            Debug.Log("Updating player " + playerId);
            if (players.TryGetValue(playerId, out GameObject cube))
            {
                UpdatePlayerStat(change, cube);
            }
            else
            {
                GameObject anotherCube = Instantiate(dummyPlayerPrefab);
                players[playerId] = anotherCube;
                AddPlayer(playerId, change, anotherCube);
            }
        }
    }

    void Room_OnPlayerLeave(DataChange change)
    {
        var playerId = change.path["id"];
        if (playerId == ColyseusRoom.Instance.PlayerId)
            return;

        if (change.operation == "remove")
        {
            Debug.Log("Removing player " + playerId);
            players.TryGetValue(playerId, out GameObject cube);
            Destroy(cube);
            players.Remove(playerId);
        }
    }

    void AddPlayer(string playerId, DataChange change, GameObject cube)
    {
        var playerGameState = (IDictionary<string, object>)change.value;
        playerData[playerId] = playerGameState;
        UpdatePlayer(cube, playerGameState);
    }

    void UpdatePlayerStat(DataChange change, GameObject cube)
    {
        var player = playerData[change.path["id"]];
        player[change.path["stat"]] = change.value;
        UpdatePlayer(cube, player);
    }

    static void UpdatePlayer(GameObject cube, IDictionary<string, object> player)
    {
        PlayerGameState update = PlayerGameState.FromColyseus(player);
        var eular = cube.transform.rotation.eulerAngles;
        eular.y = (float)update.rotation;
        cube.transform.SetPositionAndRotation(update.Position, Quaternion.Euler(eular));
    }

    void Room_OnPlayerGameStateStat(DataChange obj)
    {
        var playerId = obj.path["id"];
        if (playerId == ColyseusRoom.Instance.PlayerId)
            return;

        players.TryGetValue(playerId, out GameObject cube);
        UpdatePlayerStat(obj, cube);
    }
}

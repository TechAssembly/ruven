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
        ColyseusRoom.Instance.JoinRoom("game");
        ColyseusRoom.Instance.OnJoin += Room_OnJoin;
        ColyseusRoom.Instance.OnError += Room_OnError;
        ColyseusRoom.Instance.OnLeave += Room_OnLeave;
        ColyseusRoom.Instance.OnMessage += Room_OnMessage;
        ColyseusRoom.Instance.OnStateChange += Room_OnStateChange;
        ColyseusRoom.Instance.Room.Listen("players/:id", Room_OnPlayerChange);
        ColyseusRoom.Instance.Room.Listen("players/:id/:axis", Room_OnPlayerChange);
        ColyseusRoom.Instance.Room.Listen("players/:id/position/:axis", Room_OnPlayerMove);
        ColyseusRoom.Instance.Room.Listen("players/:id/rotation", Room_OnPlayerMove);
    }

    void Unsubscribe()
    {
        ColyseusRoom.Instance.OnJoin -= Room_OnJoin;
        ColyseusRoom.Instance.OnError -= Room_OnError;
        ColyseusRoom.Instance.OnLeave -= Room_OnLeave;
        ColyseusRoom.Instance.OnMessage -= Room_OnMessage;
        ColyseusRoom.Instance.OnStateChange -= Room_OnStateChange;
    }

    public void OnPlayerMove(Transform player)
    {
        ColyseusRoom.Instance?.Room?.Send(CreateMoveAction(player));
    }

    static object CreateMoveAction(Transform player)
    {
        return new IndexedDictionary<string, object>
        {
            {"action", "move"},
            {"position", new IndexedDictionary<string, object> {
                    {"x", player.position.x},
                    {"y", player.position.y},
                    {"z", player.position.z},
            }},
            {"rotation", player.rotation.eulerAngles.y},
        };
    }

    readonly Dictionary<string, GameObject> players = new Dictionary<string, GameObject>();

    void Room_OnPlayerChange(DataChange change)
    {
        var playerId = change.path["id"];
        if (playerId == ColyseusRoom.Instance.PlayerId)
            return;

        if (change.operation == "add")
        {
            Debug.Log("Adding player " + playerId);
            GameObject cube = Instantiate(dummyPlayerPrefab);
            UpdatePlayer(change, cube);
            players.Add(playerId, cube);
        }
        else if (change.operation == "replace")
        {
            Debug.Log("Updating player " + playerId);
            players.TryGetValue(playerId, out GameObject cube);
            UpdatePlayer(change, cube);
        }
        else if (change.operation == "remove")
        {
            Debug.Log("Removing player " + playerId);
            players.TryGetValue(playerId, out GameObject cube);
            Destroy(cube);
            players.Remove(playerId);
        }
    }

    void Room_OnPlayerMove(DataChange change)
    {
        var playerId = change.path["id"];
        if (playerId == ColyseusRoom.Instance.PlayerId)
            return;

        players.TryGetValue(playerId, out GameObject cube);
        var positionChanged = change.path.ContainsKey("axis");
        if (change.path.TryGetValue("axis", out string axis))
        {
            var value = Convert.ToSingle(change.value);
            var currentPosition = cube.transform.position;
            if (axis == "x")
                currentPosition.x = value;
            else if (axis == "y")
                currentPosition.y = value;
            else if (axis == "z")
                currentPosition.z = value;
            cube.transform.position = currentPosition;
        }
        else
        {
            var eular = cube.transform.rotation.eulerAngles;
            eular.y = Convert.ToSingle(change.value);
            cube.transform.rotation = Quaternion.Euler(eular);
        }
    }

    static void UpdatePlayer(DataChange change, GameObject cube)
    {
        PlayerUpdate update = PlayerUpdate.FromColyseus(change.value);
        var eular = cube.transform.rotation.eulerAngles;
        eular.y = update.Rotation;
        cube.transform.SetPositionAndRotation(update.Position, Quaternion.Euler(eular));
    }

    void Room_OnLeave(object sender, EventArgs e)
    {
        Unsubscribe();
    }

    void Room_OnError(object sender, ErrorEventArgs e)
    {
        Unsubscribe();
    }

    void Room_OnStateChange(object sender, RoomUpdateEventArgs e)
    {
    }

    void Room_OnMessage(object sender, MessageEventArgs e)
    {
    }

    void Room_OnJoin(object sender, EventArgs e)
    {
    }
}

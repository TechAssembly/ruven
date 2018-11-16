using System;
using GameDevWare.Serialization;
using UnityEngine;

public class PlayerUpdate
{
    static IndexedDictionary<string, object> ReadObject(object obj) => obj as IndexedDictionary<string, object>;

    public Vector3 Position { get; private set; }
    public float Rotation { get; private set; }
    public string Name {get; private set;}

    public static PlayerUpdate FromColyseus(object update)
    {
        var updateObj = ReadObject(update);
        var playerName = updateObj["name"].ToString();
        var positionObj = ReadObject(updateObj["position"]);
        var position = new Vector3(
            Convert.ToSingle(positionObj["x"]),
            Convert.ToSingle(positionObj["y"]),
            Convert.ToSingle(positionObj["z"]));

        var rotation = updateObj["rotation"] as float? ?? 0;
        return new PlayerUpdate
        {
            Rotation = rotation,
            Position = position,
            Name = playerName,
        };
    }
}

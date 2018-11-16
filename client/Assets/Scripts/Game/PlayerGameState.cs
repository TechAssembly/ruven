using System.Collections.Generic;
using System.Runtime.Serialization;
using Colyseus;
using GameDevWare.Serialization;
using UnityEngine;

public class PlayerGameState
{
    static IndexedDictionary<string, object> ReadObject(object obj) => obj as IndexedDictionary<string, object>;

#pragma warning disable IDE1006 // Naming Styles
    [DataMember]
    public float xPosition { get; set; }
    [DataMember]
    public float yPosition { get; set; }
    [DataMember]
    public float zPosition { get; set; }
    [DataMember]
    public float rotation { get; set; }
    [DataMember]
    public float xInput { get; set; }
    [DataMember]
    public float zInput { get; set; }
    [DataMember]
    public KeyState leftShiftPressed { get; set; }
#pragma warning restore IDE1006 // Naming Styles

    public Vector3 Position => new Vector3(xPosition, yPosition, zPosition);

    public static PlayerGameState FromColyseus(object update) => ObjectExtensions.ToObject<PlayerGameState>(update);

    public IDictionary<string, object> ToColyseus() => ObjectExtensions.ToDictionary(this);

    static float ReadFloat(IndexedDictionary<string, object> obj, string key)
    {
        return obj[key] as float? ?? 0f;
    }

    static bool ReadBool(IndexedDictionary<string, object> obj, string key)
    {
        return obj[key] as bool? ?? false;
    }
}

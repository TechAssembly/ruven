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
    public double xPosition { get; set; }
    [DataMember]
    public double yPosition { get; set; }
    [DataMember]
    public double zPosition { get; set; }
    [DataMember]
    public double rotation { get; set; }
    [DataMember]
    public double xInput { get; set; }
    [DataMember]
    public double zInput { get; set; }
    [DataMember]
    public KeyState leftShiftPressed { get; set; }
#pragma warning restore IDE1006 // Naming Styles
    
    public Vector3 Position => new Vector3((float)xPosition, (float)yPosition, (float)zPosition);

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

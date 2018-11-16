using System.Collections.Generic;
using UnityEngine;

public enum PathDisplayMode { None, Connections, Paths }

public class AIWaypointNetwork : MonoBehaviour
{
    [HideInInspector]
    public PathDisplayMode DisplayMode = PathDisplayMode.Connections;

    [HideInInspector]
    public int From;

    [HideInInspector]
    public int To;

    public List<Transform> Waypoints = new List<Transform>();
}

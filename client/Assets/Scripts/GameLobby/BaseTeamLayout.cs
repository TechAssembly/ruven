using GameDevWare.Serialization;
using UnityEngine;

public abstract class BaseTeamLayout : MonoBehaviour
{
    public abstract void CreateLayout(GameObject playerListPrefab);
    public abstract GameLobbyUpdate UpdateTeams(IndexedDictionary<string, object> state);
}
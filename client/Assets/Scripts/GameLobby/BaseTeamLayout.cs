﻿using UnityEngine;

public abstract class BaseTeamLayout : MonoBehaviour
{
    public abstract void CreateLayout(GameObject playerListPrefab);
    public abstract void UpdateLayout(object update);
}

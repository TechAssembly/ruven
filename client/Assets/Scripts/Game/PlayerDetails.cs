using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDetails : MonoBehaviour
{
    public Text nameLabel;
    public GameObject placeholder;

    void Update()
    {
        Vector3 namePos = Camera.main.WorldToScreenPoint(placeholder.transform.position);
        nameLabel.transform.position = namePos;
    }

    public void UpdatePlayer(PlayerUpdate player)
    {
        nameLabel.text = player.Name;
    }
    
}

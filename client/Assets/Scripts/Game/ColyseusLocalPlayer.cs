using UnityEngine;

public class ColyseusLocalPlayer : MonoBehaviour
{
    public ColyseusGame game;
    Vector3 lastPosition = Vector3.zero;
    Quaternion lastRotation = Quaternion.identity;

    void Start()
    {
        game.OnPlayerMove(transform);
    }

    void Update()
    {
        if (transform.position == lastPosition && transform.rotation == lastRotation)
            return;

        game.OnPlayerMove(transform);
        lastRotation = transform.rotation;
        lastPosition = transform.position;
    }
}

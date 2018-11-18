using UnityEngine;

public partial class ColyseusLocalPlayer : MonoBehaviour
{
    public ColyseusGame game;
    public PlayerGameState PlayerState { get; private set; }


    void Update()
    {
        PlayerState = new PlayerGameState
        {
            xPosition = transform.position.x,
            yPosition = transform.position.y,
            zPosition = transform.position.z,
            xInput = Input.GetAxis("Horizontal"),
            zInput = Input.GetAxis("Vertical"),
            rotation = transform.rotation.eulerAngles.y,
            leftShiftPressed = KeyCode.LeftShift.GetStateOfKey(),
        };
        game.OnPlayerStateChange(PlayerState);
    }
}

using UnityEngine;

public static class Utils
{
    public static KeyState GetStateOfKey(this KeyCode keyCode)
    {
        if (Input.GetKeyUp(keyCode))
            return KeyState.Up;
        if (Input.GetKeyDown(keyCode))
            return KeyState.Down;
        if (Input.GetKey(keyCode))
            return KeyState.Pressed;
        return KeyState.Released;
    }
}

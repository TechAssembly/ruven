using UnityEngine;
using UnityEngine.UI;

public class ColyseusServerStatus : MonoBehaviour
{
    public Text statusText;
    string currentStatusText = "Connecting";

    void Start()
    {
        if (ColyseusConnector.Instance.ClientOpen)
            currentStatusText = "Connected";

        ColyseusConnector.Instance.OnOpen += (sender, e) => currentStatusText = "Connected";
        ColyseusConnector.Instance.OnClose += (sender, e) => currentStatusText = "Disconnected";
        ColyseusConnector.Instance.OnError += (sender, e) => currentStatusText = "Error";
    }

    void Update()
    {
        statusText.text = currentStatusText;
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ColyseusLoginForm : MonoBehaviour
{
    public InputField addressInput;
    public InputField nameInput;
    public Button connectButton;
    public Text errorMessageText;

    string errorMessage = "";
    bool awaitingConnection;

    void Start()
    {
        var savedAddress = PlayerPrefs.GetString("ColyseusServerAddress", "");
        var savedName = PlayerPrefs.GetString("ColyseusPlayerName", "");

        if (!string.IsNullOrWhiteSpace(savedAddress))
            addressInput.text = savedAddress;
        if (!string.IsNullOrWhiteSpace(savedName))
            nameInput.text = savedName;

        ColyseusConnector.Instance.OnOpen += Client_OnOpen;
        ColyseusConnector.Instance.OnError += Client_OnError;
        ColyseusConnector.Instance.OnClose += Client_OnClose;
    }

    void Update()
    {
        errorMessageText.text = errorMessage;
        connectButton.interactable =
                         !awaitingConnection &&
                         !string.IsNullOrWhiteSpace(addressInput.text) &&
                         !string.IsNullOrWhiteSpace(nameInput.text);
    }

    public void OnConnectClicked()
    {
        errorMessage = "";
        string address = addressInput.text;
        PlayerPrefs.SetString("ColyseusServerAddress", address);
        PlayerPrefs.SetString("ColyseusPlayerName", nameInput.text);
        awaitingConnection = true;
        ColyseusConnector.Instance.ConnectToServer(address);
    }

    void Client_OnOpen(object sender, System.EventArgs e)
    {
        ColyseusConnector.Instance.OnOpen -= Client_OnOpen;
        ColyseusConnector.Instance.OnError -= Client_OnError;
        ColyseusConnector.Instance.OnClose -= Client_OnClose;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    void Client_OnError(object sender, Colyseus.ErrorEventArgs e)
    {
        awaitingConnection = false;
        errorMessage = $"Error occured: {e.message}";
    }

    void Client_OnClose(object sender, System.EventArgs e)
    {
        awaitingConnection = false;
        errorMessage = "Connecting failed - try again!";
    }

}

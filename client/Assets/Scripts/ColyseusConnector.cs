using System;
using System.Collections;
using Colyseus;
using UnityEngine;

public class ColyseusConnector : MonoBehaviour
{
    public string defaultHost = "localhost";
    public int defaultPort = 3000;
    public bool connectOnStart = true;

    public event EventHandler OnOpen;
    public event EventHandler OnClose;
    public event EventHandler<ErrorEventArgs> OnError;

    public bool ClientOpen { get; private set; }
    public Client Client { get; private set; }

    void Start()
    {
        if (connectOnStart)
            ConnectToServer($"{defaultHost}:{defaultPort}");
    }

    public void ConnectToServer(string address)
    {
        StartCoroutine(ServerConnectionCoroutine(address));
    }

    IEnumerator ServerConnectionCoroutine(string address)
    {
        string uri = $"ws://{address}";
        Debug.Log("Conencting to Colyseus on: " + uri);
        Client = new Client(uri);

        Client.OnOpen += Client_OnOpen;
        Client.OnClose += Client_OnClose;
        Client.OnError += Client_OnError;

        Debug.Log("Let's connect the client!");
        yield return StartCoroutine(Client.Connect());

        while (true)
        {
            Client.Recv();
            yield return null;
        }
    }

    public IEnumerator EnsureClientOpen()
    {
        yield return new WaitUntil(() => ClientOpen);
    }

    void Client_OnOpen(object sender, EventArgs e)
    {
        ClientOpen = true;
        Debug.Log("CONNECTION OPEN");
        OnOpen?.Invoke(this, e);
    }

    void Client_OnClose(object sender, EventArgs e)
    {
        ClientOpen = false;
        Debug.LogError("CONNECTION CLOSED");
        OnClose?.Invoke(this, e);
    }

    void Client_OnError(object sender, ErrorEventArgs e)
    {
        ClientOpen = false;
        Debug.LogError("CONNECTION ERROR");
        Debug.LogError(e.message);
        OnError?.Invoke(this, e);
    }


    #region Singleton Component Implementation

    public static ColyseusConnector Instance
    {
        get; private set;
    }

    void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnDestroy()
    {
        if (this != Instance || Client == null)
            return;

        Client.Close();
    }

    #endregion Singleton Component Implementation
}

using System;
using System.Collections;
using Colyseus;
using UnityEngine;

public class ColyseusConnector : MonoBehaviour
{
    public string host = "localhost";
    public int port = 3000;

    bool clientOpen;
    public Client Client { get; private set; }

    IEnumerator Start()
    {
        string uri = $"ws://{host}:{port}";
        Debug.Log("Conencting to Colyseus on: " + uri);
        Client = new Client(uri);

        Client.OnOpen += Client_OnOpen;
        Client.OnClose += Client_OnClose;

        Debug.Log("Let's connect the client!");
        yield return StartCoroutine(Client.Connect());

        while (true)
        {
            Client.Recv();
            yield return 0;
        }
    }

    public IEnumerator EnsureClientOpen()
    {
        yield return new WaitUntil(() => clientOpen);
    }

    void Client_OnOpen(object sender, EventArgs e)
    {
        clientOpen = true;
        Debug.Log("CONNECTION OPEN");
    }

    void Client_OnClose(object sender, EventArgs e)
    {
        clientOpen = false;
        Debug.Log("CONNECTION CLOSED");
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

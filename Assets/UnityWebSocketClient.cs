using UnityEngine;
using System.Collections;
using LeanCloud.Realtime.Internal;
using System;
using LeanCloud.Realtime;
using System.Threading.Tasks;
using System.Collections.Generic;
using LeanCloud.Storage.Internal;

public class UnityWebSocketClient : MonoBehaviour, WebSocketUnityDelegate, IWebSocketClient
{
    void Awake()
    {
        Debug.Log("Awake.gameObject.name:" + this.gameObject.name);
    }
    void Start()
    {
        Debug.Log("Start.gameObject.name:" + this.gameObject.name);
#if UNITY_ANDROID && !UNITY_EDITOR
        webSocket = new WebSocketUnity(this);
#endif
    }
    string mGameObjectName = "";
    void Update()
    {
        mGameObjectName = this.gameObject.name;
    }
    // Web Socket for Unity
    //    Desktop
    //    WebPlayer
    //    Android
    //    ios (+ ios simulator)
    //      WebGL
    private WebSocketUnity webSocket;

    #region WebSocketUnityDelegate implementation

    // These callbacks come from WebSocketUnityDelegate
    // You will need them to manage websocket events

    // This event happens when the websocket is opened
    public void OnWebSocketUnityOpen(string sender)
    {
        Debug.Log("WebSocket connected, " + sender);

        this.OnOpened();
    }

    // This event happens when the websocket is closed
    public void OnWebSocketUnityClose(string reason)
    {
        Debug.Log("WebSocket Close : " + reason);
        this.OnClosed(-1, reason, "");
    }

    // This event happens when the websocket received a message
    public void OnWebSocketUnityReceiveMessage(string message)
    {
        Debug.Log("Received from server : " + message);

        this.OnMessage(message);
    }

    // This event happens when the websocket received data (on mobile : ios and android)
    // you need to decode it and call after the same callback than PC
    public void OnWebSocketUnityReceiveDataOnMobile(string base64EncodedData)
    {
        // it's a limitation when we communicate between plugin and C# scripts, we need to use string
        byte[] decodedData = webSocket.decodeBase64String(base64EncodedData);
        OnWebSocketUnityReceiveData(decodedData);
    }

    // This event happens when the websocket did receive data
    public void OnWebSocketUnityReceiveData(byte[] data)
    {
        var decodeStr = System.Convert.ToBase64String(data);
        OnWebSocketUnityReceiveMessage(decodeStr);
    }

    // This event happens when you get an error@
    public void OnWebSocketUnityError(string error)
    {
        Debug.LogError("WebSocket Error : " + error);
    }

    #endregion

    #region LeanCloud

    public bool IsOpen
    {
        get
        {
            return webSocket.IsOpened();
        }
    }

    public void Close()
    {
        webSocket.Close();
    }

    public void Open(string url, string protocol = null)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        webSocket.mUrl = url;
        webSocket.mGameObjectName = this.mGameObjectName;
#else
        webSocket = new WebSocketUnity(url, this);
#endif
        webSocket.Open();

    }

    public void Send(string message)
    {
        if (this.IsOpen)
            webSocket.Send(message);
    }

    public event Action<int, string, string> OnClosed;

    public event Action<string> OnMessage;

    public event Action<string> OnLog;

    public event Action<string> OnError;

    public event Action OnOpened;

    #endregion

}
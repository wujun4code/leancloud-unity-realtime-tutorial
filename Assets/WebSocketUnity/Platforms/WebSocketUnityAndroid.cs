//----------------------------------------------
// WebSocketUnity
// Copyright (c) 2015, Jonathan Pavlou
// All rights reserved
//----------------------------------------------

using UnityEngine;
using System.Collections;

#if UNITY_ANDROID
public class WebSocketUnityAndroid : IWebSocketUnityPlatform
{

    private AndroidJavaObject mWebSocket = null;
    public string Url { get; set; }
    public string GameObjectName { get; set; }

    // Constructor
    // param : url of your server (for example : ws://echo.websocket.org)
    // param : gameObjectName name of the game object who will receive events
    public WebSocketUnityAndroid(string url, string gameObjectName)
    {
        Url = url;
        object[] parameters = new object[1];
        parameters[0] = gameObjectName;
        mWebSocket = new AndroidJavaObject("com.jonathanpavlou.WebSocketUnityImpl", parameters);
    }

    #region Basic features

    // Open a connection with the specified url
    public void Open()
    {
        AndroidJNI.AttachCurrentThread();
        mWebSocket.Call("initialize", new object[1] { Url });
        mWebSocket.Call("setHandlerGameObjectName", new object[1] { GameObjectName });
        Debug.LogError("Open");
        mWebSocket.Call("connect");
        AndroidJNI.DetachCurrentThread();
    }

    // Close the opened connection
    public void Close()
    {
        Debug.LogError("Close");
        mWebSocket.Call("close");
    }

    // Check if the connection is opened
    public bool IsOpened()
    {
        AndroidJNI.AttachCurrentThread();
        Debug.LogError("IsOpened");
        var rtn = mWebSocket.Call<bool>("isOpen");
        return rtn;

    }

    // Send a message through the connection
    // param : message is the sent message
    public void Send(string message)
    {
        AndroidJNI.AttachCurrentThread();
        Debug.LogError("Send message");
        mWebSocket.Call("send", message);
        AndroidJNI.DetachCurrentThread();
    }

    // Send a message through the connection
    // param : data is the sent byte array message
    public void Send(byte[] data)
    {
        Debug.LogError("Send byte");
        mWebSocket.Call("send", data);
    }
    #endregion

}
#else
public class WebSocketUnityAndroid {}
#endif // UNITY_ANDROID

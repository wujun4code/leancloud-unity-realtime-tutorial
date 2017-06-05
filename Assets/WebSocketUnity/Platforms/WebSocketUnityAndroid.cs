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

    // Constructor
    // param : url of your server (for example : ws://echo.websocket.org)
    // param : gameObjectName name of the game object who will receive events
    public WebSocketUnityAndroid(string url, string gameObjectName)
    {
        Url = url;
        if (0 == AndroidJNI.AttachCurrentThread())
        {
            Debug.LogError("AttachCurrentThread success");
            object[] parameters = new object[1];
            parameters[0] = gameObjectName;

            mWebSocket = new AndroidJavaObject("com.jonathanpavlou.WebSocketUnityImpl", parameters);
        }
        else
        {
            Debug.LogError("AttachCurrentThread faliue");
        }
    }

    #region Basic features

    // Open a connection with the specified url
    public void Open()
    {
        mWebSocket.Call("initialize", new object[1] { Url });
        Debug.LogError("Open");
        mWebSocket.Call("connect");
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
        Debug.LogError("IsOpened");
        if (0 == AndroidJNI.AttachCurrentThread())
        {
            return mWebSocket.Call<bool>("isOpen");
        }
        return false;
    }

    // Send a message through the connection
    // param : message is the sent message
    public void Send(string message)
    {
        Debug.LogError("Send message");
        mWebSocket.Call("send", message);
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

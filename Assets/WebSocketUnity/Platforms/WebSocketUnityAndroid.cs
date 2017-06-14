//----------------------------------------------
// WebSocketUnity
// Copyright (c) 2015, Jonathan Pavlou
// All rights reserved
//----------------------------------------------

using UnityEngine;
using System.Collections;
using System.Threading;
using System;



#if UNITY_ANDROID
public class WebSocketUnityAndroid : IWebSocketUnityPlatform
{
	private class Attacher : IDisposable
	{
		private int tid;

		public Attacher (object o)
		{
			tid = System.Threading.Thread.CurrentThread.ManagedThreadId;
			if (tid != 1) {
				AndroidJNI.AttachCurrentThread ();
			}
		}

		public void Dispose ()
		{
			if (tid != 1) {
				AndroidJNI.DetachCurrentThread ();
			}
		}
	}

	private AndroidJavaObject mWebSocket = null;

	public string Url { get; set; }

	public string GameObjectName { get; set; }

	// Constructor
	// param : url of your server (for example : ws://echo.websocket.org)
	// param : gameObjectName name of the game object who will receive events
	public WebSocketUnityAndroid (string url, string gameObjectName)
	{
		Url = url;
		object[] parameters = new object[1];
		parameters [0] = gameObjectName;
		mWebSocket = new AndroidJavaObject ("com.jonathanpavlou.WebSocketUnityImpl", parameters);
	}

	#region Basic features

	// Open a connection with the specified url
	public void Open ()
	{
		using (new Attacher (this)) {
			mWebSocket.Call ("initialize", new object[1] { Url });
			mWebSocket.Call ("setHandlerGameObjectName", new object[1] { GameObjectName });
			mWebSocket.Call ("connect");
		}
	}

	// Close the opened connection
	public void Close ()
	{
		using (new Attacher (this)) {
			mWebSocket.Call ("close");
		}
	}

	// Check if the connection is opened
	public bool IsOpened ()
	{
		using (new Attacher (this)) {
			return mWebSocket.Call<bool> ("isOpen");
		}
	}

	// Send a message through the connection
	// param : message is the sent message
	public void Send (string message)
	{
		using (new Attacher (this)) {
			mWebSocket.Call ("send", message);
		}
	}

	// Send a message through the connection
	// param : data is the sent byte array message
	public void Send (byte[] data)
	{
		using (new Attacher (this)) {
			mWebSocket.Call ("send", data);
		}
	}

	#endregion

}
#else
public class WebSocketUnityAndroid {}
#endif // UNITY_ANDROID

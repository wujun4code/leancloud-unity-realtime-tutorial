using UnityEngine;
using System.Collections;
using LeanCloud.Realtime.Internal;
using System;
using LeanCloud.Realtime;
using System.Threading.Tasks;
using System.Collections.Generic;
using MiniMessagePack;
using LeanCloud.Storage.Internal;

/// <summary>
/// 消息
/// </summary>
[AVIMMessageClassName("IMNormalTalk ")]
public class IMNormalTalk : IAVIMMessage
{
    static MiniMessagePacker m_MsgPacker;

    public IMNormalTalk()
    {
        if (m_MsgPacker == null)
        {
            m_MsgPacker = new MiniMessagePacker();
        }
    }

    #region Content Define
    //Sendding name
    public string m_SendName { get; set; }

    public string m_Content { get; set; }

    public IMNormalTalk(string senderName, string content)
    {
        if (m_MsgPacker == null)
        {
            m_MsgPacker = new MiniMessagePacker();
        }

        m_SendName = senderName;
        m_Content = content;
    }
    #endregion

    public string ConversationId
    {
        get; set;
    }

    public string FromClientId
    {
        get; set;
    }

    public string Id
    {
        get; set;
    }

    public long RcpTimestamp
    {
        get; set;
    }

    public long ServerTimestamp
    {
        get; set;
    }

    public IAVIMMessage Deserialize(string msgStr)
    {
        var spiltStrs = msgStr.Split(':');
        var serContent = spiltStrs[1];
        var tMessage = m_MsgPacker.Unpack(System.Convert.FromBase64String(serContent)) as Dictionary<string, object>;

        this.m_SendName = Convert.ToString(tMessage["S"]);
        this.m_Content = Convert.ToString(tMessage["C"]);

        return this;
    }

    public string Serialize()
    {
        Dictionary<string, object> tMessage = new Dictionary<string, object>();
        tMessage.Add("S", m_SendName);
        tMessage.Add("C", m_Content);
        var c = m_MsgPacker.Pack(tMessage);
        return "N:" + System.Convert.ToBase64String(c);
    }

    public bool Validate(string msgStr)
    {
        var spiltStrs = msgStr.Split(':');
        return spiltStrs[0] == "N";
    }


}

/// <summary>
/// 自定义表情消息
/// </summary>
[AVIMMessageClassName("Emoji")]
[AVIMTypedMessageTypeIntAttribute(2)]
public class Emoji : AVIMTypedMessage
{
    [AVIMMessageFieldName("Ecode")]
    public string Ecode { get; set; }
}

[AVIMMessageClassName("NikkiMessage")]
public class NikkiMessage : AVIMTypedMessage
{
    public NikkiMessage() { }

    public NikkiMessage(AVIMMessage message)
    {
        NikkiType = 1;
        NikkiContent = "hahaha";
    }

    [AVIMMessageFieldName("NikkiType")]
    public int NikkiType { get; set; }
    [AVIMMessageFieldName("NikkiContent")]
    public string NikkiContent { get; set; }
}

/// <summary>
/// 二进制消息
/// </summary>
[AVIMMessageClassName("BinaryMessage")]
public class BinaryMessage : IAVIMMessage
{
    public BinaryMessage()
    {

    }
    /// <summary>
    /// 从 bytes[] 构建一条消息
    /// </summary>
    /// <param name="data"></param>
    public BinaryMessage(byte[] data)
    {
        BinaryData = data;
    }

    public byte[] BinaryData { get; set; }

    public string ConversationId
    {
        get; set;
    }

    public string FromClientId
    {
        get; set;
    }

    public string Id
    {
        get; set;
    }

    public long RcpTimestamp
    {
        get; set;
    }

    public long ServerTimestamp
    {
        get; set;
    }

    public IAVIMMessage Deserialize(string msgStr)
    {
        var spiltStrs = msgStr.Split(':');
        this.BinaryData = System.Convert.FromBase64String(spiltStrs[1]);
        return this;
    }

    public string Serialize()
    {
        return "bin:" + System.Convert.ToBase64String(this.BinaryData);
    }

    public bool Validate(string msgStr)
    {
        var spiltStrs = msgStr.Split(':');
        return spiltStrs[0] == "bin";
    }
}
public class MyWebSocketClient : MonoBehaviour, WebSocketUnityDelegate, IWebSocketClient
{
    public static AVIMClient ClientInstance { get; set; }

    string mGameObjectName = "";
    void Awake()
    {

    }
    void Start()
    {

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
        Dispatcher.Instance.Post(() =>
        {
            webSocket = new WebSocketUnity(url, this);
            webSocket.Open();
        });
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
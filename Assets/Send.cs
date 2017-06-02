using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using LeanCloud.Realtime;
using System.Threading;
using System.Threading.Tasks;
using LeanCloud.Core;
using LeanCloud;
using LeanCloud.Storage.Internal;

public class Send : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public Task<AVIMClient> GetClient(string clientId)
    {
        if (MyWebSocketClient.ClientInstance != null)
        {
            Debug.Log("client already created.");
            return Task.FromResult<AVIMClient>(MyWebSocketClient.ClientInstance);
        }
        else
        {
            return AVRealtime.Instance.CreateClientAsync(clientId: clientId, secure: true).ContinueWith(s =>
                   {
                       if (s.IsFaulted)
                       {
                           Debug.Log("IsFaulted");
                       }
                       if (s.Exception != null)
                       {
                           var inner = s.Exception.InnerException;
                           var inners = s.Exception.InnerExceptions;
                           if (inner != null)
                           {
                               Debug.Log("inner");
                               Debug.Log(inner.Message);
                           }

                           if (inners != null)
                           {
                               Debug.Log("inners");
                               foreach (var e in inners)
                               {
                                   Debug.Log(e.Message);
                               }
                           }
                       }

                       var client = s.Result;
                       MyWebSocketClient.ClientInstance = client;
                       MyWebSocketClient.ClientInstance.OnMessageReceived += OnMessageReceived;

                       return client;
                   });
        }

    }

    public Task<AVIMConversation> GetTestConversation(string clientId, string convId)
    {
        return GetClient(clientId).ContinueWith(t =>
        {
            MyWebSocketClient.ClientInstance = t.Result;
            var conversation = AVIMConversation.CreateWithoutData(convId, MyWebSocketClient.ClientInstance);
            return conversation;
        });
    }
    AVIMConversation chatRoom;
    public Task<AVIMConversation> GetChatRoom(string clientId, string chatRoomId)
    {
        return this.GetClient(clientId).OnSuccess(t =>
        {
            if (chatRoom != null)
            {
                Debug.Log("chatRoom already created.");
                return Task.FromResult(chatRoom);
            }
            return t.Result.GetConversationAsync(chatRoomId, true);
        }).Unwrap().OnSuccess(x =>
        {
            chatRoom = x.Result;
            return chatRoom;
        });
    }

    public void SendToChatRoom()
    {
        var textMessage = new AVIMTextMessage("兄弟们，睡什么睡，起来嗨！");

        this.GetChatRoom("junwu", "5930ea19fab00f41ddc7f42b").OnSuccess(t =>
         {
             return MyWebSocketClient.ClientInstance.SendMessageAsync(t.Result, textMessage);
         }).Unwrap().OnSuccess(x =>
         {
             Debug.Log("chat room sent");
         });
    }

    private void OnMessageReceived(object sender, AVIMMessageEventArgs e)
    {
        Debug.Log("OnMessageReceived" + e.Message.ToString() + e.Message.GetType());
        if (e.Message is BinaryMessage)
        {
            var binaryMessage = e.Message as BinaryMessage;
            var binaryData = binaryMessage.BinaryData;
            // 下面的字符串内容就是 I love Unity
            var text = System.Text.Encoding.UTF8.GetString(binaryData);
            Debug.Log("binary message:" + text);
        }
        else if (e.Message is IMNormalTalk)
        {
            var imNormalTalkMessage = e.Message as IMNormalTalk;
            Debug.Log("IMNormalTalk message:" + imNormalTalkMessage.m_Content);
        }
        else if (e.Message is AVIMTextMessage)
        {
            var textMessage = e.Message as AVIMTextMessage;
            Debug.Log("text message:" + textMessage.Content);
        }
        else if (e.Message is Emoji)
        {
            var emoji = e.Message as Emoji;
            Debug.Log("Emoji message:" + emoji.Ecode);
        }
    }

    public void TestSendTextMessage()
    {
        try
        {
            this.GetTestConversation("junwu", "5930ea19fab00f41ddc7f42b").ContinueWith(t =>
            {
                var conversation = t.Result;
                Debug.Log(conversation.Name);
                var textMessage = new AVIMTextMessage("兄弟们，睡什么睡，起来嗨！");
                return conversation.SendMessageAsync(textMessage);
            }).Unwrap().ContinueWith(s =>
            {
                if (s.IsFaulted)
                {
                    Debug.Log("IsFaulted");
                }
                if (s.Exception != null)
                {
                    var inner = s.Exception.InnerException;
                    var inners = s.Exception.InnerExceptions;
                    if (inner != null)
                    {
                        Debug.Log("inner");
                        Debug.Log(inner.Message);
                    }

                    if (inners != null)
                    {
                        Debug.Log("inners");
                        foreach (var e in inners)
                        {
                            Debug.Log(e.Message);
                        }
                    }
                }
            });
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
        }
    }

    public void TestSendEmojiMessage()
    {
        this.GetTestConversation("junwu", "5930ea19fab00f41ddc7f42b").ContinueWith(t =>
        {
            var conversation = t.Result;
            var emojiMessage = new Emoji()
            {
                Ecode = "#e001",// 应用内置的表情编码
            };
            return conversation.SendMessageAsync(emojiMessage);
        });
    }

    public void TestSendNikkiMessage()
    {
        this.GetTestConversation("junwu", "5930ea19fab00f41ddc7f42b").ContinueWith(t =>
        {
            var conversation = t.Result;
            var nikki = new NikkiMessage()
            {
                NikkiContent = "I am angry with Mono Reflections",
                NikkiType = 99
            };
            return conversation.SendMessageAsync(nikki);
        });
    }

    public void TestSendBinaryMessage()
    {
        this.GetTestConversation("junwu", "5930ea19fab00f41ddc7f42b").ContinueWith(t =>
        {
            var conversation = t.Result;
            var text = "I love Unity";
            var textBytes = System.Text.Encoding.UTF8.GetBytes(text);
            var binaryMessage = new BinaryMessage(textBytes);
            return conversation.SendMessageAsync(binaryMessage);
        });
    }

    public void TestSendIMNormalTalkMessage()
    {
        this.GetTestConversation("junwu", "58341551fab00f41dd85da12").ContinueWith(t =>
        {
            var conversation = t.Result;
            var testMessage = new IMNormalTalk("junwu", "I love Unity");
            return conversation.SendMessageAsync(testMessage);
        });
    }

    public void LogOut()
    {
        this.GetClient("junwu").ContinueWith(t =>
        {
            var client = t.Result;
            client.CloseAsync();
        });
    }

    public void PrintState()
    {
        Debug.Log(AVRealtime.Instance.State.ToString());
    }
}

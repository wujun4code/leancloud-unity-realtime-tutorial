using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using LeanCloud.Realtime;
using System.Threading;
using System.Threading.Tasks;
using LeanCloud.Core;
using LeanCloud;
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

    Task<AVIMConversation> GetTestConversation(string clientId, string convId)
    {
        Func<Task<AVIMClient>> getClient = () =>
        {
            if (MyWebSocketClient.ClientInstance != null)
            {
                return Task.FromResult<AVIMClient>(MyWebSocketClient.ClientInstance);
            }

            AVRealtime.Instance.CreateClient(clientId: clientId,deviceId:"Iphpne 6s-xaajdkj",tag:"unity-ios");
            return AVRealtime.Instance.CreateClient(clientId: clientId, secure: true).ContinueWith(s =>
            {
                var client = s.Result;
                client.OnMessageReceived += OnMessageReceived;

                return client;
            });
        };

        return getClient().ContinueWith(t =>
        {
            MyWebSocketClient.ClientInstance = t.Result;
            var conversation = AVIMConversation.CreateWithoutData(convId, MyWebSocketClient.ClientInstance);
            return conversation;
        });
    }
    private void OnMessageReceived(object sender, AVIMMessageEventArgs e)
    {
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
    }

    public void TestSendTextMessage()
    {
        try
        {
            this.GetTestConversation("junwu", "58be1f5392509726c3dc1c8b").ContinueWith(t =>
            {
                var conversation = t.Result;
                Debug.Log(conversation.Name);
                var textMessage = new AVIMTextMessage("兄弟们，睡什么睡，起来嗨！");
                return conversation.SendMessageAsync(textMessage);
            }).Unwrap().ContinueWith(s =>
            {
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
                Debug.Log(s.Result.Id);
            });
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
        }
    }

    public void TestSendEmojiMessage()
    {
        this.GetTestConversation("junwu", "58be1f5392509726c3dc1c8b").ContinueWith(t =>
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
        this.GetTestConversation("junwu", "58be1f5392509726c3dc1c8b").ContinueWith(t =>
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
        this.GetTestConversation("junwu", "58be1f5392509726c3dc1c8b").ContinueWith(t =>
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
        this.GetTestConversation("junwu", "58be1f5392509726c3dc1c8b").ContinueWith(t =>
        {
            var conversation = t.Result;
            var testMessage = new IMNormalTalk("junwu","I love Unity");
            return conversation.SendMessageAsync(testMessage);
        });
    }

    public void PrintState()
    {
        Debug.Log(AVRealtime.Instance.State.ToString());
    }
}

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

            return AVRealtime.Instance.CreateClient(clientId: clientId, secure: true);
        };

        return getClient().ContinueWith(t =>
        {
            MyWebSocketClient.ClientInstance = t.Result;
            var conversation = AVIMConversation.CreateWithoutData(convId, MyWebSocketClient.ClientInstance);
            return conversation;
        });
    }

    public void TestSendTextMessage()
    {
        this.GetTestConversation("junwu", "58be1f5392509726c3dc1c8b").ContinueWith(t =>
        {
            var conversation = t.Result;
            var textMessage = new AVIMTextMessage("兄弟们，睡什么睡，起来嗨！");
            return conversation.SendMessageAsync(textMessage);
        });
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
}

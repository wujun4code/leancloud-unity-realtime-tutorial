using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LeanCloud.Realtime;
using UnityEngine;

public class Member : MonoBehaviour
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
            return Task.FromResult<AVIMClient>(MyWebSocketClient.ClientInstance);
        }

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

            return client;
        });
    }

    public Task<AVIMConversation> GetTestConversation(string clientId, string convId)
    {
        return GetClient(clientId).ContinueWith(t =>
        {
            if (t.IsFaulted)
            {
                Debug.Log("IsFaulted");
            }
            if (t.Exception != null)
            {
                var inner = t.Exception.InnerException;
                var inners = t.Exception.InnerExceptions;
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
            MyWebSocketClient.ClientInstance = t.Result;
            var conversation = AVIMConversation.CreateWithoutData(convId, MyWebSocketClient.ClientInstance);
            return conversation;
        });
    }

    public void InviteMembers()
    {
        GetTestConversation("junwu", "58341551fab00f41dd85da12").ContinueWith(t =>
        {
            var conv = t.Result;
            var randomMember = RandomString(6);
            return MyWebSocketClient.ClientInstance.InviteAsync(conv, randomMember);
        }).ContinueWith(s =>
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
    public string RandomString(int length)
    {
        System.Random random = new System.Random();
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, length)
          .Select(s => s[random.Next(s.Length)]).ToArray());
    }
}

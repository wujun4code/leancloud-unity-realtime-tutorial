using System.Collections;
using System.Collections.Generic;
using LeanCloud.Storage.Internal;
using UnityEngine;

public class Create : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void CreateNormalConversation()
    {
        var name = "桃园";
        ClientSingleton.Current.CreateConversationAsync("Bill", name: name).OnSuccess(t =>
        {
            ConversationSingleton.Current = t.Result;
        });
    }
}

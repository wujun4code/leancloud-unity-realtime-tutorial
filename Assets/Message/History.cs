using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LeanCloud.Realtime;
using LeanCloud.Storage.Internal;

public class History : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void QueryHistory()
    {
        ConversationSingleton.Current.QueryMessageAsync().OnSuccess(t =>
        {
            var messages = t.Result;
            foreach (var message in messages)
            {
                if (message is AVIMTextMessage)
                {
					var textMessage = message as AVIMTextMessage;
                }
            }
        });
    }

}

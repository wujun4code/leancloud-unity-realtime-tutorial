using System.Collections;
using System.Collections.Generic;
using LeanCloud.Storage.Internal;
using UnityEngine;
using LeanCloud.Realtime;

public class Invite : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void InviteMember()
    {
        ClientSingleton.Current.InviteAsync(ConversationSingleton.Current, "heiheihei").OnSuccess(s =>
        {
            AVRealtime.PrintLog("invited success.");
        });
    }
}

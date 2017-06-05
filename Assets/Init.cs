using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LeanCloud;
using System.Threading.Tasks;
using LeanCloud.Realtime;
using LeanCloud.Push.Internal;
using LeanCloud.Storage.Internal;

public class Init : MonoBehaviour
{
    public static AVRealtime RealtimeInstance { get; set; }
    bool tokenSent;
    public AVInstallation currentInstallation = null;
    // Use this for initialization
    void Start()
    {
        var websocketPlugin = GameObject.FindObjectOfType<UnityWebSocketClient>();
        var config = new AVRealtime.Configuration()
        {
            ApplicationId = "3knLr8wGGKUBiXpVAwDnryNT-gzGzoHsz",
            ApplicationKey = "3RpBhjoPXJjVWvPnVmPyFExt",
            WebSocketClient = websocketPlugin // 使用已经初始化的 WebSocketClient 实例作为 AVRealtime 初始化的配置参数
        };

        AVRealtime.WebSocketLog(UnityEngine.Debug.Log);

        RealtimeSingleton.Init(new AVRealtime(config));
        RealtimeSingleton.Current.UseLeanEngineSignatureFactory();

        RealtimeSingleton.Current.CreateClientAsync("junwu").OnSuccess(t =>
        {
            ClientSingleton.Init(t.Result);

            return ClientSingleton.Current.GetQuery().FirstAsync();
        }).Unwrap().OnSuccess(s =>
        {
            ConversationSingleton.Current = s.Result;
            if (ConversationSingleton.Current != null)
                AVRealtime.PrintLog("current conversation id is: " + ConversationSingleton.Current.ConversationId);
        });

    }

    // Update is called once per frame
    void Update()
    {
    }
}

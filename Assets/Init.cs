using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LeanCloud;
using System.Threading.Tasks;
using LeanCloud.Realtime;

public class Init : MonoBehaviour
{
    public static AVRealtime RealtimeInstance { get; set; }
    // Use this for initialization
    void Start()
    {
        var sc = GameObject.FindObjectOfType<MyWebSocketClient>();
        var config = new AVRealtime.Configuration()
        {
            ApplicationId = "uay57kigwe0b6f5n0e1d4z4xhydsml3dor24bzwvzr57wdap",
            ApplicationKey = "kfgz7jjfsk55r5a8a3y4ttd3je1ko11bkibcikonk32oozww",
            WebSocketClient = sc // 使用已经初始化的 WebSocketClient 实例作为 AVRealtime 初始化的配置参数
        };

        AVRealtime.WebSocketLog(UnityEngine.Debug.Log);



        RealtimeInstance = new AVRealtime(config);

        AVRealtime.Instance.UseLeanEngineSignatureFactory();
        RealtimeInstance.RegisterMessageType<Emoji>();
        RealtimeInstance.RegisterMessageType<NikkiMessage>();
        RealtimeInstance.RegisterMessageType<BinaryMessage>();
        RealtimeInstance.RegisterMessageType<IMNormalTalk>();

        //AVRealtime.Instance.ToggleHeartBeating(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TestLocalEngineRequest()
    {
        AVCloud.CallFunctionAsync<string>("hello", null).ContinueWith(engineResponse =>
        {
            Debug.Log("local engine response" + engineResponse.Result);
        });
    }
}

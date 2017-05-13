using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LeanCloud;
using System.Threading.Tasks;
using LeanCloud.Realtime;

public class LocalEngine : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OpenWss()
    {
        AVRealtime.Instance.OpenAsync("wss://rtm55.leancloud.cn/");
    }

    public void Open()
    {
        AVRealtime.Instance.OpenAsync();
    }

    public void CallLocalEngine()
    {
        AVClient.HttpLog(Debug.Log);

        AVCloud.CallFunctionAsync<string>("hello", null).ContinueWith(t =>
         {
             Debug.Log(t.Result);
         });
    }

    public void SaveObject()
    {
        AVClient.HttpLog(Debug.Log);
        var todo = new AVObject("Todo");
        todo["title"]="XD";
        todo.SaveAsync();
    }
}

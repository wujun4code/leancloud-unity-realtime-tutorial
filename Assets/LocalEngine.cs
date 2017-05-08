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
}

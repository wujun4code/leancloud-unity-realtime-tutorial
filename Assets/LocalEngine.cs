using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LeanCloud;
using System.Threading.Tasks;

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

    public void TestLocalEngineRequest()
    {
        AVCloud.CallFunctionAsync<string>("hello", null).ContinueWith(engineResponse =>
        {
            Debug.Log("local engine response" + engineResponse.Result);
        });
    }
}

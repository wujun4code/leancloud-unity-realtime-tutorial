using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LeanCloud.Realtime;
using LeanCloud.Storage.Internal;

public class Send : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SendText()
	{
		var textMessage = new AVIMTextMessage("我是一条文本消息");
		ConversationSingleton.Current.SendMessageAsync(textMessage).OnSuccess(tag =>{
			
		});
	}
}

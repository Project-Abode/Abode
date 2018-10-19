using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;


public class SocketClient : MonoBehaviour {

	// Use this for initialization
	private WebSocket ws;
	bool connected = false;

	private MessageParser msgParser;

	private string serverIP = "52.12.67.196";
	//private string serverIP = "127.0.0.1";
	public string port = "1337";

	void Start () {
		msgParser.GetComponent<MessageParser>();
		ws = new WebSocket("ws://"+serverIP+":"+port);
		ws.OnOpen += OnOpenHandler;
		ws.OnMessage += OnMessageHandler;
		ws.OnClose += OnCloseHandler;
		ws.Connect();
	}
	
	int count = 0;
	void Update () {
		if(Input.GetKeyDown(KeyCode.Space)) {
			if(connected) {
				ws.Send("Current Count is: " + count);
				count ++;
			}
		}
	}

	public void SendMyMessage(string msg) {
		if(connected) {
			ws.Send(msg);
		}
	}

	private void OnOpenHandler(object sender, System.EventArgs e) {
		Debug.Log("Connection Opened");
		connected = true;
	}

	private void OnMessageHandler(object sender, WebSocketSharp.MessageEventArgs e) {
		//Debug.Log("Received Message: " + e.data);
		if (e.IsText) {
			Debug.Log("Received Message: " + e.Data);
			msgParser.ParseMessage(e.Data);
			return;
		}
	}

	private void OnCloseHandler(object sender,WebSocketSharp.CloseEventArgs e) {
		Debug.Log("Socket Closed with reason: " + e.Reason);
		connected = false;
	}

}


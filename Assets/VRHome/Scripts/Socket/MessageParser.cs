using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageParser : MonoBehaviour {
	public void ParseMessage(string msg) {

		if(msg.Equals("")) return;

		//Receive Invitation
		switch(msg) {
			case "send invitation":
				//pop up guest invatation ui
				var menuController = GameObject.Find("MenuController").GetComponent<ShowMene>();
				if(menuController) {
					menuController.ShowTV();
				}

			break;

			case "accept invitation":
				//ETA shows on host
				var tvController = GameObject.Find("TV").GetComponent<TVController>();
				if(tvController) {
					tvController.OnGuestAccept();
				}
			break;
		}


	}


}

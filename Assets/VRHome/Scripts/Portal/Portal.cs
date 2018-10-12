using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExitGames.SportShooting;
public class Portal : MonoBehaviour {

	public string roomID = "";

	void SetToGoID(string id) {
		roomID = id;
	}

	void OnTriggerEnter(Collider col) {
		if(col.tag == "MainCamera") {
			if(!roomID.Equals(""))
				GameController.Instance.JoinRoom(roomID);
		}
	}

}

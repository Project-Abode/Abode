using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour {

	public static Settings instance = null;

	public bool isHost;
	public int room;
	public int id;
	public int method;
	public int exvitation;
	public int avatar;

	//avatar
	//exit
	//entry
	//invitation
	//exvitation

	PhotonView settingSync;

	void Awake() {
		if (instance == null)
			 instance = this;
		else if (instance != this)
             Destroy(gameObject); 

		ClearUpSettings();
		settingSync = GetComponent<PhotonView>();
	}


	public void SetRoom(int value) {
		room = value;
		id = value;
	}

	//0: portal
	//1: magic wand
	//2: elevator
	//3: levelstream
	//4: magic door
	//5: hot airballoon

	public void SetEntryExitMethod(int value) {
		method = value;
	}

	public void SetExvitation(int value) {
		exvitation = value;
	}

	public void SetAvatar(int value) {
		avatar = value;
	}

	public void ClearUpSettings() {
		isHost = false;
		room = -1;
		id = -1;
		method = -1;
		exvitation = -1;
		avatar = -1;
	}

	public void OnHostRequstedSync() {
		settingSync.RPC("UpdateSettingsFromHostData", PhotonTargets.Others, room, method, exvitation, avatar);
	}

	
	public List<int> buffer;

	//delegate for buffer data ready

	[PunRPC]
	public void UpdateSettingsFromHostData(int room, int method, int exvitation, int avatar) {
		buffer = new List<int>(4);
		buffer[0] = room;
		buffer[1] = method;
		buffer[2] = exvitation;
		buffer[3] = avatar;

		//call buffer data ready delegate

	}

	public void CopyBufferIntoSettings() {
		if(buffer!=null) {
			room = buffer[0];
			id = room;
			method = buffer[1];
			exvitation = buffer[2];
			avatar = buffer[3];
			//buffer = null;
		}
	}

	void Update() {
		// if(Input.GetKeyDown(KeyCode.Alpha0)) {
		// 	room = 0; //hearth
		// 	id = 0;
		// }

		// if(Input.GetKeyDown(KeyCode.Alpha1)) {
		// 	room = 1; //meditation
		// 	id = 1;
		// }

		// if(Input.GetKeyDown(KeyCode.Alpha2)) {
		// 	room = 2; //garden
		// 	id = 2;
		// }

		



	}

}

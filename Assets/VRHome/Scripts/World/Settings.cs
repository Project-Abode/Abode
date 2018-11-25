using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExitGames.SportShooting;

public class Settings : MonoBehaviour {

	public static Settings instance = null;

	public bool isHost = true;
	public int room;
	public int id;
	public int method;
	public int exvitation;
	public int avatar;

    public bool QA_test;

	void Awake() {
		if (instance == null)
			 instance = this;
		else if (instance != this)
             Destroy(gameObject); 

		//ClearUpSettings();
		
	}

	public void SetIsHost(bool value) {
		isHost = value;
	}

	public void SetRoom(int value) {
		room = value;
		id = value;
	}
    
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

        if (QA_test) return;
           
		isFinished = false;
		syncDone = false;

		isHost = true;
		room = -1;
		id = -1;
		method = -1;
		exvitation = -1;
		avatar = -1;
	}

	public void Seed() {

		//debug

		isHost = true;
		room = 1;
		id = 1;
		//0-P, 2-E, 3-L
		method = 2;
		exvitation = 0;
		avatar = 0;
	}

	public void SeedForGuest() {
		isHost = false;
		room = 1;
		id = 1;
		//0-P, 2-E, 3-L
		method = -1;
		exvitation = -1;
		avatar = 2;
	}

	public void CopyBufferIntoSettings() { //List<int> buffer
		var buffer = SettingBuffer.instance.GetBuffer();
		if(buffer!=null) {
			method = buffer[0];
			exvitation = buffer[1];
		}
	}

	public void FinishedSetting(){
		isFinished = true;
	}

	bool isFinished = false;

	bool syncDone = false;
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

		//Guest waiting for host buffer data
		if(isHost) return;

		if(!syncDone) {
			if(SettingBuffer.instance.IsBufferReady() && isFinished) {
				CopyBufferIntoSettings();
				GameController.Instance.EnterGameWithSettings();
				syncDone = true;
			}
		}
		
	}

}

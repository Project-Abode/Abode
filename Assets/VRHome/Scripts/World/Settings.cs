using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour {

	public static Settings instance = null;

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

	void Awake() {
		if (instance == null)
			 instance = this;
		 else if (instance != this)
             Destroy(gameObject); 
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


	void Update() {
		if(Input.GetKeyDown(KeyCode.Alpha0)) {
			room = 0; //hearth
			id = 0;
		}

		if(Input.GetKeyDown(KeyCode.Alpha1)) {
			room = 1; //meditation
			id = 1;
		}

		if(Input.GetKeyDown(KeyCode.Alpha2)) {
			room = 2; //garden
			id = 2;
		}

		
	}

}

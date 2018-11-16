using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour {

	public static Settings instance = null;

	public int room;
	public int id;
	public int method;
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

		//Entry and exit method

		// if(Input.GetKeyDown(KeyCode.P)) {
		// 	//Portal
		// 	method = 0;
		// }

		// if(Input.GetKeyDown(KeyCode.E)) {
		// 	//Elevator
		// 	method = 1;
		// }

		// if(Input.GetKeyDown(KeyCode.L)) {
		// 	//level streaming
		// 	method = 0;
		// }
	}

}

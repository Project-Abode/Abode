using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour {

	public static Settings instance = null;

	public int room;

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

}

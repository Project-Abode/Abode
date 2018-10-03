using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExitGames.SportShooting;


public class InteractableGenerator : MonoBehaviour {

	// Use this for initialization

	public List<GameObject> interactables = new List<GameObject>();

	void Start () {
		
	}
	
	void Update () {
		if(Input.GetKeyDown(KeyCode.G)) {
			Generate();
		}
	}

	void Generate() {
		foreach(var interactable in interactables) {
			 GameObject go = PhotonNetwork.Instantiate(interactable.name, new Vector3(0,1,0), Quaternion.identity, 0) as GameObject;
		}

	}
}

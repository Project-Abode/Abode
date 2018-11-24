using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WandEEMethod : EEMethod {
	Transform player;

	//public GameObject wandPrefab;
	WandController wandController;

	Transform guestBase;
    Transform hostBase;

	int guestID;
	int hostID;


	GameObject wand;

	string wandPrefabName = "MagicWand";

	override public void InitMethod(Transform VRPlayer = null) {
	
		
		player = VRPlayer;

		guestID = from;
        hostID = to;

        guestBase = RoomSwitcher.instance.GetDescriptionAt(guestID).origin;
        hostBase = RoomSwitcher.instance.GetDescriptionAt(hostID).origin;

		//Photon network instantiate
		wand = PhotonNetwork.Instantiate(wandPrefabName, guestBase.position, Quaternion.identity,0);
		//wand = PhotonNetwork.Instantiate(wandPrefabName, spawnPoint, Quaternion.identity, 0) as GameObject;
		
		if(forPlayer != Settings.instance.id) {
			//limit grab by guest
			wand.gameObject.tag = "Untagged";
			return;
		}
		
		//Only guest needs to init
		wandController = wand.GetComponent<WandController>();
		wandController.Init(player, hostBase.position, guestBase.position);
		wandController.requestTransport += OnRequestTransport;

	}


	void OnRequestTransport() {
		EntryExitManager.instance.TeleportPlayerTo(hostID, hostBase.position);

		//network clean up wand
		PhotonNetwork.Destroy(wand);

	}

	override public void CleanUpMethod() {
		// Destroy(wandController.gameObject);
		// player = null;
		// gameObject.SetActive(false);
	}

}

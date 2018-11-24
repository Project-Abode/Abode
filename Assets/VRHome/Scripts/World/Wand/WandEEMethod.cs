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
	
		
		if(forPlayer != Settings.instance.id) {
			return;
		}

		player = VRPlayer;

		guestID = from;
        hostID = to;

        guestBase = RoomSwitcher.instance.GetDescriptionAt(guestID).origin;
        hostBase = RoomSwitcher.instance.GetDescriptionAt(hostID).origin;

		if(forPlayer != Settings.instance.id) {
			//limit grab by guest
			//wand.gameObject.tag = "Untagged";
			return;
		}

		wand = PhotonNetwork.Instantiate(wandPrefabName, guestBase.position, Quaternion.identity,0);
		wand.gameObject.tag = "grabable";
	
		//Only guest needs to init
		wandController = wand.GetComponent<WandController>();
		wandController.Init(player, hostBase.position, guestBase.position);
		wandController.requestTransport += OnRequestTransport;

	}


	void OnRequestTransport() {
		EntryExitManager.instance.TeleportPlayerTo(hostID, hostBase.position);

		//network clean up wand
		//PhotonNetwork.Destroy(wand);

	}

	override public void CleanUpMethod() {
		// Destroy(wandController.gameObject);
		// player = null;
		// gameObject.SetActive(false);
	}

}

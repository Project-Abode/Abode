using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WandEEMethod : EEMethod {

	void Awake() {
	}

	Transform player;

	public GameObject wandPrefab;
	WandController wandController;

	Transform guestBase;
    Transform hostBase;

	int guestID;
	int hostID;


	override public void InitMethod(Transform VRPlayer = null) {

		if(forPlayer != Settings.instance.id) {
			return;
		}

		player = VRPlayer;

		guestID = from;
        hostID = to;

        guestBase = RoomSwitcher.instance.GetDescriptionAt(guestID).origin;
        hostBase = RoomSwitcher.instance.GetDescriptionAt(hostID).origin;

		var wand = Instantiate(wandPrefab, Vector3.zero, Quaternion.identity);
		wandController = wand.GetComponent<WandController>();
		wandController.Init(player, hostBase.position, guestBase.position);

		wandController.requestTransport += OnRequestTransport;

	}


	void OnRequestTransport() {
		EntryExitManager.instance.TeleportPlayerTo(hostID, hostBase.position);
	}

	override public void CleanUpMethod() {
		// Destroy(wandController.gameObject);
		// player = null;
		// gameObject.SetActive(false);
	}

}

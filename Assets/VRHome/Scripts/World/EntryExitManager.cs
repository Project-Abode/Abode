using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntryExitManager : MonoBehaviour {

	public RoomSwitcher roomSwitcher;
	public Transform VRPlayer;


	private GameObject current_method;
	public List<GameObject> EEMethodInstances;
	

	public static EntryExitManager instance = null;

	PhotonView photonView;

	void Awake() {
		if (instance == null)
			 instance = this;
		 else if (instance != this)
             Destroy(gameObject); 

		photonView = GetComponent<PhotonView>();

	} 

	public void Init(Transform player) {
		VRPlayer = player;
	}


	[PunRPC]
	public void SetUpMethod(int methodIndex, int from, int to, int for_player) {

		if(current_method) {
			//need clean up?
			//current_method.CleanUpMethod();
			var ee = current_method.GetComponent<EEMethod>();
			if(ee!=null) {
				ee.CleanUpMethod();
			}
			current_method.SetActive(false);
		}

		current_method = EEMethodInstances[methodIndex];
		current_method.SetActive(true);

		var eemethod = current_method.GetComponent<EEMethod>();
		eemethod.SetUpBasicInfo(from,to,for_player);
		eemethod.InitMethod(VRPlayer);

	}

	// public delegate void NotifyEnterArea();
	// public NotifyEnterArea notifyEnterArea;

	public void TeleportPlayerTo(int roomID, Vector3 position) {
		
		if(Settings.instance.method != 3)
			roomSwitcher.ChangeToRoomWithDescription(roomID);

		if(VRPlayer!=null)
			VRPlayer.position = position;

		//send on arrive
		if(roomID == 1) { 
			photonView.RPC("GuestLeftRoom", PhotonTargets.Others);
		}else{
			photonView.RPC("GuestArriveAtRoom", PhotonTargets.Others);
		}

	}
	
	public delegate void NotifyGuestArrive();
	public NotifyGuestArrive notifyGuestArrive;

	public delegate void NotifyGuestLeft();
	public NotifyGuestLeft notifyGuestLeft;

	[PunRPC] 
	void GuestArriveAtRoom(int roomID) {
		if(notifyGuestArrive!=null) {
			notifyGuestArrive();
		}
	
	}

	[PunRPC] 
	void GuestLeftRoom(int roomID) {
		if(notifyGuestLeft!=null) {
			notifyGuestLeft();
		}
	}



	// public void CleanUpMethod() {
	// 	if(current_method!=null)
	// 		current_method.CleanUpMethod();
		
	// 	current_method = null;
	// }


	//Debug for invitation/exvitation 
	void Update() {

		if(Input.GetKeyDown(KeyCode.S)) {
			// int cur_room = roomSwitcher.GetCurrentRoomIndex();
			// int to_room = cur_room == 1? 0:1;

			// int cur_room = 0;
			// int to_room = 1;
			
			OnSetUpMethod(1, 0, 1);
		}

		if(Input.GetKeyDown(KeyCode.O)) {
			// int cur_room = roomSwitcher.GetCurrentRoomIndex();
			// int to_room = cur_room == 1? 0:1;

			// int cur_room = 0;
			// int to_room = 1;
			
			OnSetUpMethod(0, 1, 1);
		}

	}

	//Depart from ____, to ______, for player ____;
	public void OnSetUpMethod(int from, int to, int for_player) {
		
		int methodIndex = Settings.instance.method;

		if(methodIndex == 1 || methodIndex == 5) //Magic wand
			photonView.RPC("SetUpMethod", PhotonTargets.All, 1, from, to, for_player);
		else {
			photonView.RPC("SetUpMethod", PhotonTargets.All, 0, from, to, for_player);
		}
				

	}
	
}

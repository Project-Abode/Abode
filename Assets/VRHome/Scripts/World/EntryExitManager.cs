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
			current_method.SetActive(false);
		}

		current_method = EEMethodInstances[methodIndex];
		current_method.SetActive(true);

		var eemethod = current_method.GetComponent<EEMethod>();
		eemethod.CleanUpMethod();
		eemethod.SetUpBasicInfo(from,to,for_player);
		eemethod.InitMethod(VRPlayer);

	}

	public void TeleportPlayerTo(int roomID, Vector3 position) {
		
		roomSwitcher.ChangeToRoomWithDescription(roomID);

		if(VRPlayer!=null)
			VRPlayer.position = position;

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

		//if(methodIndex == 2 || methodIndex == )

		photonView.RPC("SetUpMethod", PhotonTargets.All, 0, from, to, for_player);

	}
	
}

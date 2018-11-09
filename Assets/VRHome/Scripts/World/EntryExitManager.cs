using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntryExitManager : MonoBehaviour {

	public RoomSwitcher roomSwitcher;
	public Transform VRPlayer;

	//private EEMethod current_method;
	private GameObject current_method;

	//public List<GameObject> EEMethodPrefabs;
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

		// if(current_method) {
		// 	CleanUpMethod();
		// }

		if(current_method) {
			//need clean up?
			current_method.SetActive(false);
		}

		//Should be network instantiate or scene photonview activate
		// current_method = Instantiate(EEMethodPrefabs[methodIndex]).GetComponent<EEMethod>();
		// if(current_method!=null) {
		// 	current_method.SetUpBasicInfo(from,to,for_player);
		// 	current_method.InitMethod();
		// }

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
			int cur_room = roomSwitcher.GetCurrentRoomIndex();
			int to_room = cur_room == 1? 0:1;

			OnSetUpMethod(cur_room, to_room, 1);
		}

	}

	//Depart from ____, to ______, for player ____;
	public void OnSetUpMethod(int from, int to, int for_player) {
		
		int methodIndex = Settings.instance.method;
		photonView.RPC("SetUpMethod", PhotonTargets.All, methodIndex, from, to, for_player);

	}
	
}

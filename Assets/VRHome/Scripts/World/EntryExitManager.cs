using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntryExitManager : MonoBehaviour {

	public RoomSwitcher roomSwitcher;
	public Transform VRPlayer;

	private EEMethod current_method;

	public List<GameObject> EEMethodPrefabs;

	public static EntryExitManager instance = null;

	void Awake() {
		if (instance == null)
			 instance = this;
		 else if (instance != this)
             Destroy(gameObject); 
	} 

	public void Init(Transform player) {
		VRPlayer = player;
	}


	public void SetUpMethod(int index, int from, int to, int for_player) {

		if(current_method) {
			CleanUpMethod();
		}

		current_method = Instantiate(EEMethodPrefabs[index]).GetComponent<EEMethod>();
		if(current_method!=null) {
			current_method.SetUpBasicInfo(from,to,for_player);
			current_method.InitMethod();
		}
	}

	public void TeleportPlayerTo(int roomID, Vector3 position) {
		
		roomSwitcher.ChangeToRoomWithDescription(roomID);
		if(VRPlayer!=null)
			VRPlayer.position = position;

	}

	public void CleanUpMethod() {
		if(current_method!=null)
			current_method.CleanUpMethod();
		
		current_method = null;
	}


	//Debug for invitation/exvitation 
	void Update() {

		if(Input.GetKeyDown(KeyCode.S)) {
			int cur_room = roomSwitcher.GetCurrentRoomIndex();
			int to_room = cur_room == 1? 0:1;
			SetUpMethod(0, cur_room, to_room , 0);
		}

	}
	
}

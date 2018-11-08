using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RoomSwitcher : MonoBehaviour {


	public List<VRRoomDescription> roomDescriptions;

	public static RoomSwitcher instance = null;
	
	void Awake () {
		if (instance == null)
			 instance = this;
		 else if (instance != this)
             Destroy(gameObject); 

		if(Settings.instance!=null)
			ChangeToRoomWithDescription(Settings.instance.room);

	}
	
	void Update () {
		
		if(Input.GetKeyDown(KeyCode.Alpha0)) {
			ChangeToRoomWithDescription(0);
		}

		if(Input.GetKeyDown(KeyCode.Alpha1)) {
			ChangeToRoomWithDescription(1);
		}

		if(Input.GetKeyDown(KeyCode.Alpha2)) {
			ChangeToRoomWithDescription(2);
		}
	}

	int last_idx = -1;

	public int GetCurrentRoomIndex() {
		return last_idx;
	}

	public void ClearLastRoomSetting() {
		if(last_idx >= 0) {
			//clear light
			roomDescriptions[last_idx].Light.SetActive(false);
		}
	}
	public void ChangeToRoomWithDescription(int index) {
		ClearLastRoomSetting();

		roomDescriptions[index].Light.SetActive(true);
		RenderSettings.skybox = roomDescriptions[index].skybox;
		
		last_idx = index;
	}


	public VRRoomDescription GetDescriptionAt(int index) {

		if(index >= 0) {
			return roomDescriptions[index];
		}
		return null;
	}

	public VRRoomDescription GetCurrentDescription() {

		if(last_idx >= 0) {
			return roomDescriptions[last_idx];
		}
		return null;
	}

	

}

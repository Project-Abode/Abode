using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorEEMethod : EEMethod {

	public List<ElevatorController> elevators;

	public Transform VRPlayer;

	int guestID;
    int hostID ;

	public CollideDetection guestInside;
	public CollideDetection hostInside;
	public CollideDetection hostOutside;


	Transform guestBase;
    Transform hostBase;


	override public void InitMethod(Transform VRPlayer = null) {
		if(forPlayer != Settings.instance.id) return;

		this.VRPlayer = VRPlayer;

		guestID = from;
        hostID = to;

	
		guestInside.notifyTouched += OnGuestInsideCollide;
		hostInside.notifyTouched += OnHostInsideCollide;
		hostOutside.notifyTouched += OnHostOutsideCollide;

		guestInside.SetActive(true);
		hostOutside.SetActive(true);

		guestBase = RoomSwitcher.instance.GetDescriptionAt(guestID).origin;
        hostBase = RoomSwitcher.instance.GetDescriptionAt(hostID).origin;

		elevators[guestID].OpenDoor();

	}


	void OnGuestInsideCollide() {
		guestInside.SetActive(false);
		elevators[guestID].CloseDoor();

		StartCoroutine(ElevatorCountDown(3,0));
	}


	IEnumerator ElevatorCountDown(float seconds, int choice) {

		yield return new WaitForSeconds(seconds);


		if(choice == 0) {
			EntryExitManager.instance.TeleportPlayerTo(hostID, hostBase.position);
			//VRPlayer.position = hostBase.position;
			elevators[hostID].OpenDoor();
		}

		// }else {
			
		// 	VRPlayer.position = guestBase.position;
		// 	elevators[guestID].OpenDoor();
		// }	
		
	}

	IEnumerator ElevatorBackCountDown(float seconds) {
		//VRPlayer.position = guestBase.position;

		EntryExitManager.instance.TeleportPlayerTo(guestID, guestBase.position);

		yield return new WaitForSeconds(seconds);
		
		elevators[guestID].OpenDoor();
	}


	void OnHostInsideCollide() {
		hostInside.SetActive(false);
		elevators[hostID].CloseDoor();
		StartCoroutine(ElevatorBackCountDown(3));
		
	}

	void OnHostOutsideCollide() {
		hostOutside.SetActive(false);

		elevators[hostID].CloseDoor();

		hostInside.SetActive(true);
	}

	
}

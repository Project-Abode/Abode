using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExitGames.SportShooting;
public class RemoteController : MonoBehaviour {

	// Use this for initialization
	
	public LaserPointerSteamVR1 pointer;
	public GameObject remoteModel;
	 
	bool isGrabbed = false;

	// void Awake () {
	// 	ReleaseRemoteController();
	// }
	
	// public void GrabRemoteController() {
	// 	pointer.OpenLaser();
	// 	remoteModel.SetActive(true);
	// 	isGrabbed = true;
	// }

	// public void ReleaseRemoteController() {
	// 	pointer.CloseLaser();
	// 	remoteModel.SetActive(false);
	// 	isGrabbed = false;
	// }


}

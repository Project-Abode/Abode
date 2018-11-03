using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalEEMethod : EEMethod {

	public GameObject portal_in_prefab;
	public GameObject portal_out_prefab;
	private GameObject portal_in, portal_out;

	private PortalEETrigger teleTrigger;
	public GameObject teleTriggerPrefab;

	public Vector3 toGoPos;
	public Vector3 nowPos;
	

	private PhotonView pv;

	void Awake() {
		pv = GetComponent<PhotonView>();
	}


	public Vector3 offset;
	override public void InitMethod() {
		//set up now pos and to go pos
		switch(to) {
			case 0:
				toGoPos = new Vector3(0,0,0) + offset;
				break;
			case 1:
				toGoPos = new Vector3(1000,1000,1000) + offset;
				break;
			default:
				toGoPos = new Vector3(0,0,0) + offset;
				break;
		}
		switch(from) {
			case 0:
				nowPos = new Vector3(0,0,0) + offset;
				break;
			case 1:
				nowPos = new Vector3(1000,1000,1000) + offset;
				break;
			default:
				nowPos = new Vector3(0,0,0) + offset;
				break;
		}


		//instantiate portal models
		portal_in = Instantiate(portal_in_prefab, nowPos, Quaternion.identity);
		portal_out = Instantiate(portal_out_prefab, toGoPos, Quaternion.identity);

		//Set up trigger
		if(forPlayer == Settings.instance.id) {
			var portal_tele_trigger = Instantiate(teleTriggerPrefab, nowPos, Quaternion.identity);
			if(portal_tele_trigger) {
				teleTrigger = portal_tele_trigger.GetComponent<PortalEETrigger>();
				if(teleTrigger) {
					teleTrigger.Init(this);
				}
			}
		}


	}

	
	override public void CleanUpMethod() { //RESET
		//TODO:		
		if(portal_in) Destroy(portal_in);
		if(portal_out) Destroy(portal_out);

		if(teleTrigger) teleTrigger.DestroyThisTrigger();

	}

	override public void TeleportTriggered() {
		if(pv) {
			pv.RPC("PreTeleportEffect",PhotonTargets.All);
		}

		StartCoroutine(TeleportCountDown(3f));
		
		if(pv) {
			pv.RPC("AfterTeleportEffect",PhotonTargets.All);
		}

	}


	IEnumerator TeleportCountDown(float seconds) {

		yield return new WaitForSeconds(seconds);
		EntryExitManager.instance.TeleportPlayerTo(to, toGoPos);
		teleTrigger.DestroyThisTrigger();

	}


	[PunRPC]
	public void PreTeleportEffect() {
		if(portal_in) {
			var effectController = portal_in.GetComponent<PortalEffectController>();
			effectController.PlayGoEffect(3f);
			//portal_in.SetActive(false);
		}
	}


	[PunRPC]
	public void AfterTeleportEffect() {

		Debug.Log("Player arrives destination");

		if(portal_out) {
			portal_out.SetActive(false);
		}
		
	}

}

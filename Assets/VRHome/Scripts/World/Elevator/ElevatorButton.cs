using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExitGames.SportShooting;
public class ElevatorButton : MonoBehaviour {

	public delegate void NotifyTouched();
	public NotifyTouched notifyTouched;

	void OnTriggerEnter(Collider col) {

		if(col.gameObject.tag != "Hand") return;

		Player collidePlayer = col.gameObject.GetComponentInParent<Player>();
		
		if(collidePlayer == null) return;

		//Only Current Player can control
		if(!collidePlayer.playerId.Equals(Settings.instance.id)) 
			return;

		
		
		if(notifyTouched!=null)
			notifyTouched();

	}


}

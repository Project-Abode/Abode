using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinInteraction : MonoBehaviour {

	private GameObject coinInHand;
	public Transform fingerEnd;

	private bool collidingWithCoinPack = false;

	public void CreateCoin() {
		
		if(collidingWithCoinPack) {
			coinInHand = PhotonNetwork.Instantiate("Coin", fingerEnd.position, fingerEnd.rotation, 0);
			coinInHand.transform.parent = fingerEnd;
		}
		
	}

	public void ReleaseCoin() {
		if(!coinInHand) return;
		
		coinInHand.transform.parent = null;

		var rb = coinInHand.GetComponent<Rigidbody>();
		rb.isKinematic = false;
		rb.useGravity = true;

		coinInHand = null;
	}

	public void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.name == "coin pack") {
			collidingWithCoinPack = true;
		}
	}

	public void OnTriggerExit(Collider other)
	{
		if(other.gameObject.name == "coin pack") {
			collidingWithCoinPack = false;
		}
	}

	public void OnTriggerStay(Collider other)
	{
		if(other.gameObject.name == "coin pack") {
			collidingWithCoinPack = true;
		}
	}

}

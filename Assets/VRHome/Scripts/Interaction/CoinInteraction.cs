using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinInteraction : MonoBehaviour {

	private GameObject coinInHand;
	public Transform fingerEnd;

	private bool collidingWithCoinPack = false;

	private Rigidbody hand_rb;
	void Awake(){
		hand_rb = GetComponent<Rigidbody>();
	}

	public void CreateCoin() {
		
		if(collidingWithCoinPack) {
			coinInHand = PhotonNetwork.Instantiate("Coin", fingerEnd.position, fingerEnd.rotation, 0);
			//coinInHand.transform.parent = fingerEnd;
		}
		
	}

	void Update() {
		if(coinInHand) {
			coinInHand.transform.position = fingerEnd.transform.position;
			coinInHand.transform.rotation = fingerEnd.transform.rotation;
		}
	}

	public void ReleaseCoin() {
		if(!coinInHand) return;
		
		//coinInHand.transform.parent = null;

		var rb = coinInHand.GetComponent<Rigidbody>();
		
		rb.isKinematic = false;
		rb.useGravity = true;

		//velocity
		rb.velocity = hand_rb.velocity;
		rb.angularVelocity = hand_rb.angularVelocity;

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

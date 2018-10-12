using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExitGames.SportShooting;

public class GrabInteraction : MonoBehaviour {

	public GameObject collidingObject;
	public GameObject remoteController;
    public GameObject objectInHand;

	public PhotonView _photonView;

	public void ControllerTriggerDown(SteamVR_TrackedController controller) {
		_photonView.RPC("TryGrab", PhotonTargets.MasterClient);
	}


	public void ControllerTriggerUp(SteamVR_TrackedController controller) {
		_photonView.RPC("TryRelease", PhotonTargets.MasterClient);
	}



	[PunRPC]
	public void TryGrab() {
		if (collidingObject) {
				var interactable = collidingObject.GetComponent<InteractableObject>();
				if(!interactable.locked) {
					interactable.HoldObject();
					GrabObject ();
				}
			}
	}

	[PunRPC]
	public void TryRelease() {
		if (objectInHand) {
				var interactable = objectInHand.GetComponent<InteractableObject>();
				if(interactable.locked) {
					interactable.ReleaseObject();
				}
				ReleaseObject ();
			}
	}


	private void SetCollidingObject(Collider col)
	{	
		if (collidingObject || !col.GetComponent<Rigidbody>() || col.tag != "interactable")
		{
			return;
		}

		collidingObject = col.gameObject;
	}

	private void SetRemoteController(Collider other) {
		remoteController = other.gameObject;
	}
	

	public void OnTriggerEnter(Collider other)
	{
		SetCollidingObject (other);
	}

	public void OnTriggerStay(Collider other)
	{
        SetCollidingObject (other);
	}

	

	public void OnTriggerExit(Collider other)
	{
		if (!collidingObject) 
		{
			return;
		}

		collidingObject = null;
	}


	private void GrabObject()
	{
		objectInHand = collidingObject;

        Debug.Log("Grabbing " + objectInHand.name);
     
        if (objectInHand.GetComponent<FixedJoint>())
        {
            objectInHand.GetComponent<FixedJoint>().connectedBody = null;
            Destroy(objectInHand.GetComponent<FixedJoint>());
        }

        collidingObject = null;

        
		var joint = AddFixedJoint ();
		joint.connectedBody = objectInHand.GetComponent<Rigidbody> ();
	}

	private FixedJoint AddFixedJoint()
	{
		FixedJoint fx = gameObject.AddComponent<FixedJoint> ();
		fx.breakForce = 20000;
		fx.breakTorque = 20000;
		return fx;
	}

	private void ReleaseObject()
	{
		if (GetComponent<FixedJoint> ())
		{
			GetComponent<FixedJoint> ().connectedBody = null;
			Destroy (GetComponent<FixedJoint> ());

			//TODO:
			//objectInHand.GetComponent<Rigidbody> ().velocity = Controller.velocity;
			//objectInHand.GetComponent<Rigidbody> ().angularVelocity = Controller.angularVelocity;
		}

		
		objectInHand = null;
	}


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleGrab : MonoBehaviour {

	public ControllerEvent controllerEvent;

 	public GameObject collidingObject;
	
    public GameObject objectInHand;


	void OnEnable() {	
		controllerEvent.onTriggerDown += TryGrab;
		controllerEvent.onTriggerUp += TryRelease;
	}

	void OnDisable() {	
		controllerEvent.onTriggerDown -= TryGrab;
		controllerEvent.onTriggerUp -= TryRelease;
	}

	public void TryGrab() {
		if (collidingObject) {
				GrabObject ();
			}
	}

	public void TryRelease() {
		if (objectInHand) {
				ReleaseObject ();
			}
	}


	private void SetCollidingObject(Collider col)
	{	
		if (collidingObject || !col.GetComponent<Rigidbody>() || col.tag != "grabable")
		{
			return;
		}

		collidingObject = col.gameObject;
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

		var grabable = objectInHand.GetComponent<SimpleGrabable>();
		if(grabable!= null) {
			grabable.Grab();
		}
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

			var grabable = objectInHand.GetComponent<SimpleGrabable>();
			if(grabable!= null) {
				grabable.Release();
			}
			//TODO:
			//objectInHand.GetComponent<Rigidbody> ().velocity = Controller.velocity;
			//objectInHand.GetComponent<Rigidbody> ().angularVelocity = Controller.angularVelocity;
		}
	
		objectInHand = null;
	}
}

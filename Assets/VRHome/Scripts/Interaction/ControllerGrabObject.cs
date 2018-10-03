using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerGrabObject : MonoBehaviour {


	//public static bool clockRotate = false;
	private SteamVR_TrackedObject trackedObj;

	//private GameObject player;


    private GameObject collidingObject;
    private GameObject objectInHand;

    //private static AudioSource audioSource;
    //private bool hasPlay = false;

	private SteamVR_Controller.Device Controller
	{
		get{ return SteamVR_Controller.Input ((int)trackedObj.index);}
	}

	void Awake()
	{
		trackedObj = GetComponent<SteamVR_TrackedObject> ();
		//player = GameObject.Find("VR");
	}

	private void SetCollidingObject(Collider col)
	{	
		if (collidingObject || !col.GetComponent<Rigidbody>() || col.tag != "interactable")
		{
			return;
		}

		collidingObject = col.gameObject;
	}

	void Update () {
		
		if (Controller.GetHairTriggerDown ()) {
            
			if (collidingObject) {

				var interactable = collidingObject.GetComponent<InteractableObject>();
				if(!interactable.locked) {
					interactable.HoldObject();
					GrabObject ();
				}
				
			}
		}

		if (Controller.GetHairTriggerUp ()) {
         
			if (objectInHand) {
				var interactable = objectInHand.GetComponent<InteractableObject>();
				if(interactable.locked) {
					interactable.ReleaseObject();
				}
				ReleaseObject ();
			}
		
		}
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
        // Debug.Log("grabing");
		//set objectinhand parent to player
		//objectInHand.transform.parent = player.transform;

        Debug.Log("Grabbing " + objectInHand.name);
        //audioSource = GameObject.Find(objectInHand.name).GetComponent<AudioSource>();
        // if (audioSource && !hasPlay)
        // {
        //     hasPlay = true;
        //     audioSource.Play();
        // }
        
        if (objectInHand.GetComponent<FixedJoint>())
        {
            //Debug.Log("has joint");
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

			objectInHand.GetComponent<Rigidbody> ().velocity = Controller.velocity;
			objectInHand.GetComponent<Rigidbody> ().angularVelocity = Controller.angularVelocity;
		}

		//detach from parent
		//objectInHand.transform.parent = null;
		objectInHand = null;
        //hasPlay = false;
	}
		
}

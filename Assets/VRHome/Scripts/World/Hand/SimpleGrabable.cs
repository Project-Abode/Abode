using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleGrabable : MonoBehaviour {
	
	public delegate void OnGrab();
	public OnGrab onGrab;

	public delegate void OnRelease();
	public OnRelease onRelease;
	
	public void Grab() {
		if(onGrab!=null)
			onGrab();
	}

	public void Release() {
		if(onRelease!=null)
			onRelease();
	}


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : MonoBehaviour {

	public Transform target;

	float offset = 0.4f;

	public bool enabled = false;

	private Vector3 velocity = Vector3.zero;
	public float smoothTime = 0.3F;

	public void Init(Transform _target) {
		target = _target;
		enabled = true;
	}

	public void Disable() {
		enabled = false;
	}

	
	// Update is called once per frame
	void Update () {
		if(enabled) {
			//transform.position = Vector3.Lerp(transform.position, target.position + target.forward * offset, Time.deltaTime * 1000);
			transform.position = Vector3.SmoothDamp(transform.position, target.position + target.forward * offset, ref velocity, smoothTime);
     		transform.rotation = Quaternion.Lerp(transform.rotation, target.rotation, Time.deltaTime * 1000);
		}
		
		
	}
}

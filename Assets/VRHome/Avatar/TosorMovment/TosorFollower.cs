using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TosorFollower : MonoBehaviour {

	public Transform target;
	public float offset = 0.34f;
	public float speed = 1f;
 
    void Update () {
       
			transform.position = target.position + new Vector3(0,-1,0) * offset; //+ target.up * offset;
			var targetRotation = Quaternion.LookRotation(target.forward, Vector3.up);
//			showForward = target.forward;
        
			Vector3 targetDir = target.forward;
			targetDir.y = 0;

			if(targetDir == Vector3.zero) {
				return;
			}

			float step = speed * Time.deltaTime;

			Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0f);
			transform.rotation = Quaternion.LookRotation(newDir);
		
	}
}

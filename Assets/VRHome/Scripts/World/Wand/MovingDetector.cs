using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingDetector : MonoBehaviour {

	float movingLastingSeconds = 0f;
	Vector3 lastPosition;
	//public double threshold = 0.1f;
	void Start () {
		lastPosition = transform.position;
        StartCoroutine(MovementDetection());
	}

	//void Update () {
	//	var distance = Vector3.Distance(transform.position,lastPosition);
	//	if(distance > 0) {
	//		movingLastingSeconds += Time.deltaTime;
	//	}else {
	//		movingLastingSeconds = 0f;
	//	}
	//	lastPosition = transform.position;
	//}

    IEnumerator MovementDetection()
    {
        for(; ; )
        {
            var distance = Vector3.Distance(transform.position, lastPosition);
            if (distance > 0)
            {
                movingLastingSeconds += 0.1f;
            }
            else
            {
                movingLastingSeconds = 0f;
            }
            lastPosition = transform.position;
            yield return new WaitForSeconds(0.1f);
        }

    }


	public bool DoesMovingLastSeconds(float seconds) {
		if(movingLastingSeconds > seconds) {
			return true;
		}else {
			return false;
		}	
	}

	public bool IsMoving() {
		return (movingLastingSeconds > 0);
	}


}

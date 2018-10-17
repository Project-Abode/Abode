using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionSwitcher : MonoBehaviour {

	// Use this for initialization

	//public Transform obj;

	public List<Transform> targets;
	int index;
	int N;

	void Start () {
		N = targets.Count;
		index = 0;
	}
	
	void Update () {
		if(Input.GetKeyDown(KeyCode.RightArrow)) {
			if(index >= N)
				index = 0;

			transform.position = targets[index].position;
			index ++;

		}
	}
}

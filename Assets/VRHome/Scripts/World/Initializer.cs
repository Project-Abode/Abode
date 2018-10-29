using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Initializer : MonoBehaviour {

	public List<GameObject> rooms;
	public List<GameObject> lights;


	GameObject cur_room;
	GameObject cur_light;

	public Vector3 origin;
	void Start () {
		


	}
	
	// Update is called once per frame
	void Update () {

		if(Input.GetKeyDown(KeyCode.Alpha1)) {
			if(cur_room) {
				Destroy(cur_room);
			}
			if(cur_light) {
				Destroy(cur_light);
			}
			
			cur_room = Instantiate(rooms[0]);
			cur_light = Instantiate(lights[0]);
			
		}	

		if(Input.GetKeyDown(KeyCode.Alpha2)) {

			if(cur_room) {
				Destroy(cur_room);
			}
			if(cur_light) {
				Destroy(cur_light);
			}

			cur_room = Instantiate(rooms[1]);
			cur_light = Instantiate(lights[1]);
		}
	}
}

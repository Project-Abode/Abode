using UnityEngine;
using System.Collections;

public class livingBirdsDemoScript : MonoBehaviour {
	public lb_BirdController birdControl;
	public Camera camera1;
	public Camera camera2;

	Camera currentCamera;
	bool cameraDirections = true;
	Ray ray;
	RaycastHit[] hits;

	void Start(){
		currentCamera = Camera.main;
		birdControl = GameObject.Find ("_livingBirdsController").GetComponent<lb_BirdController>();
		SpawnSomeBirds();
	}

	// Update is called once per frame
	void Update () {
		if(Input.GetKey (KeyCode.D) || Input.GetKey (KeyCode.RightArrow)){
			camera1.transform.localEulerAngles += new Vector3(0.0f,90.0f,0.0f)*Time.deltaTime;
		}
		if(Input.GetKey (KeyCode.A) || Input.GetKey (KeyCode.LeftArrow)){
			camera1.transform.localEulerAngles -= new Vector3(0.0f,90.0f,0.0f)*Time.deltaTime;
		}
		if(Input.GetMouseButtonDown(0)){
			ray = currentCamera.ScreenPointToRay(Input.mousePosition);
			hits = Physics.RaycastAll (ray);
			foreach(RaycastHit hit in hits){
				if (hit.collider.tag == "lb_bird"){
					hit.transform.SendMessage ("KillBirdWithForce",ray.direction*500);
					break;
				}
			}
		}
	}



	IEnumerator SpawnSomeBirds(){
		yield return 2;
		birdControl.SendMessage ("SpawnAmount",10);
	}

	void ChangeCamera(){
		if(camera2.gameObject.activeSelf){
			camera1.gameObject.SetActive(true);
			camera2.gameObject.SetActive(false);
			birdControl.SendMessage("ChangeCamera",camera1);
			cameraDirections = true;
			currentCamera = camera1;
		}else{
			camera1.gameObject.SetActive(false);
			camera2.gameObject.SetActive(true);
			birdControl.SendMessage("ChangeCamera",camera2);
			cameraDirections = false;
			currentCamera = camera2;
		}
	}
}

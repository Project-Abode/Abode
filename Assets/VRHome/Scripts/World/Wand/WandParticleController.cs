using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WandParticleController : MonoBehaviour {

	public List<GameObject> particles;


	public void PlayParticleEffect(int index) {

		if(index == -1) {
			StopAll();
			return;
		}

		if(index > particles.Count) {
			return;
		}

		if(particles[index].activeSelf == true) {
			return;
		}

		StopAll();
		particles[index].SetActive(true);

	}

	public void StopAll() {
		for(int i = 0; i < particles.Count; i++) {
			particles[i].SetActive(false);
		}
	}

	void Update(){
		//fix the third particle to the ground
		particles[2].transform.position = new Vector3(particles[2].transform.position.x, 0, particles[2].transform.position.z);
	}


}

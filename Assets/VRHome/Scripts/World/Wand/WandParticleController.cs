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


}

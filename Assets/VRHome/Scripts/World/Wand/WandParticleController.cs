using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WandParticleController : MonoBehaviour {

	public List<GameObject> particles;

    Transform target = null;
	//AudioSource audioSource;

    public void SetParticle3Target(Transform _target)
    {
        target = _target;
    }
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
        if(target != null)
        {
            particles[2].transform.position = new Vector3(target.position.x, target.parent.parent.position.y, target.position.z);
        }
            //particles[2].transform.position = new Vector3(target.position.x , 0, target.position.z);
        particles[2].transform.eulerAngles = new Vector3(-90,0,0);

    }


}

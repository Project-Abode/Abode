using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalEffectController : MonoBehaviour {

	private AudioSource audioSource;
	public AudioClip goSound;
	void Awake() {
		audioSource = GetComponent<AudioSource>();
	}
	public void PlayGoEffect(float duration) {
		if(audioSource) {
			audioSource.Stop();
			audioSource.PlayOneShot(goSound);
		}
			
		StartCoroutine(Scale(duration));
	}

	IEnumerator Scale(float duration) 
	{
		transform.localScale = new Vector3(1, 0f, 1);

		for (float f = duration; f >= 0; f -= 0.01f) 
		{
			transform.localScale = new Vector3(1, (duration-f)/duration, 1);
			yield return new WaitForSeconds(0.01f);
		}

		this.gameObject.SetActive(false);
	}

	// void Update() {
	// 	if(Input.GetKeyDown(KeyCode.X)) {
	// 		PlayGoEffect(3.0f);
	// 	}
	// }

}

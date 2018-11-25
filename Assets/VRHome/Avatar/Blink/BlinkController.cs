using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkController : MonoBehaviour {

	public List<GameObject> objs;

	List<Renderer> renderers;

	public GameObject hair;
	Renderer[] hair_renderers;

	public List<Material> standards;
	public List<Material> hair_standards;
	public List<Material> dissolves;
	public List<Material> hair_dissolves;
	
	public Material stand;
	public Material dissolve;

	public int avatar_choice = 0;

	void Awake() {
		renderers = new List<Renderer>();
		hair_renderers = hair.GetComponentsInChildren<Renderer>();
		for(int i = 0 ; i < objs.Count; i++){
			renderers.Add(objs[i].GetComponent<Renderer>());
		}
	}
	
	
	void Update () {
		if(Input.GetKeyDown(KeyCode.D)) {
			FadeOut();
		}

		if(Input.GetKeyDown(KeyCode.A)) {
			FadeIn();
		}
	}

	public void FadeIn() {
		Fade(0);
	}

	public void FadeOut() {
		Fade(1);
	}
	

	public void Fade(int avatar_choice, float from) { // 2 or 0
		//Set to dissolves:

		for(int i = 0 ; i < renderers.Count; i++) {
			renderers[i].material = dissolves[avatar_choice];
		}
		for(int i = 0 ; i < hair_renderers.Length; i++) {
			hair_renderers[i].material = hair_dissolves[avatar_choice];
		}
		

		StartCoroutine(StartFade(avatar_choice,from,2f));

	}


	public void Fade(float from) { // 2 or 0
		//Set to dissolves:

		for(int i = 0 ; i < renderers.Count; i++) {
			renderers[i].material = dissolves[avatar_choice];
		}
		for(int i = 0 ; i < hair_renderers.Length; i++) {
			hair_renderers[i].material = hair_dissolves[avatar_choice];
		}
		
		StartCoroutine(StartFade(avatar_choice,from,2f));

	}


	IEnumerator StartFade(int avatar_choice,float from, float duration) {
		
		dissolves[avatar_choice].SetFloat("_Distance", from);
		hair_dissolves[avatar_choice].SetFloat("_Distance", from);

		float start = from;
		float end = (from == 0) ? 1 : 0;

		float elapsed = 0;
		
		while(elapsed < duration) {
			
			float val = Mathf.Lerp(start, end, elapsed/duration);

			Vector3 center = GetMeshesCenter();
			var v4 = new Vector4(center.x, center.y, center.z, 0);

			// dissolve.SetFloat("_Distance", val);
			// dissolve.SetVector("_Center",v4);

			dissolves[avatar_choice].SetFloat("_Distance", val);
			hair_dissolves[avatar_choice].SetFloat("_Distance", val);
			dissolves[avatar_choice].SetVector("_Center",v4);
			hair_dissolves[avatar_choice].SetVector("_Center", v4);

			elapsed += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}

		ResetToStandard(avatar_choice);
		//yield return null;
	}


	Vector3 GetMeshesCenter() {

		Vector3 res = Vector3.zero;

		for(int i = 0 ; i < objs.Count; i++) {
			res += objs[i].transform.position;
		}

		return res/objs.Count;
	}


	void ResetToStandard(int avatar_choice) {
		for(int i = 0 ; i < renderers.Count; i++) {
			renderers[i].material = standards[avatar_choice];
		}
		

		for(int i = 0 ; i < hair_renderers.Length; i++) {
			hair_renderers[i].material = hair_standards[avatar_choice];
		}
	}


}

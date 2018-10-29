using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyBoxInit : MonoBehaviour {

	public Material skybox;

	void Awake () {
		if(skybox)
			RenderSettings.skybox = skybox;
	}
	
}

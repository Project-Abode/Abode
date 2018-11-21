using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingBuffer : MonoBehaviour {


	public static SettingBuffer instance = null;

	PhotonView settingSync;

	List<int> buffer;
	bool bufferReady = false;

	public void OnHostRequstedSync() {
		int method = Settings.instance.method;
		int exvitation = Settings.instance.exvitation;
		//if(isHost)
			settingSync.RPC("UpdateSettingsFromHostData", PhotonTargets.Others, method, exvitation);
	}

	[PunRPC]
	public void UpdateSettingsFromHostData(int method, int exvitation) {
		buffer = new List<int>{0,0,0,0};

		buffer[0] = method;
		buffer[1] = exvitation;

		bufferReady = true;
	}

	public List<int> GetBuffer() {
		return buffer;
	}

	public bool IsBufferReady() {
		return bufferReady;
	}

	void Awake() {
		if (instance == null)
			 instance = this;
		else if (instance != this)
             Destroy(gameObject); 

		settingSync = GetComponent<PhotonView>();
	}

	
}

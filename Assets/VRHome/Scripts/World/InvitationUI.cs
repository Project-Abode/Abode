using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvitationUI : MonoBehaviour {

	public GameObject invitationPanel;

	public void OnGoButtonClicked(){
		EntryExitManager.instance.SetUpMethodTriggered(1,0,1);
		
	}

	void Update() {
		if(Input.GetKeyDown(KeyCode.Space)) {
			invitationPanel.SetActive(!invitationPanel.activeSelf);
		}
	}
}

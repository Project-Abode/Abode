using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExitGames.SportShooting;
public class VRButton : MonoBehaviour {

    private LobbyMenuPresenter presenter; 
	void Awake () {
        presenter = GameObject.Find("SceneSwitcher").GetComponent<LobbyMenuPresenter>();
    }
	
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Hand")
        {
            if(presenter)
                presenter.JoinRoom("001");
            Debug.Log("Enter Go Button");
        }
    }
}

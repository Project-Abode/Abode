using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour {

	public static Settings instance = null;

    //UI Panels
    public GameObject[] UI_Panels;
    public GameObject UI_Parent;
    [HideInInspector] public enum Panel { welcome, role, room, garden_entryExit, hearth_entryExit, hearth_exvitation, garden_exvitation, exvitecontroller, time, avatar, loading };
    public Panel panel;


    [HideInInspector] public int room;
    [HideInInspector] public int id;
    [HideInInspector] public int method;

    [HideInInspector] public bool isHost=false;
    /*
    //avatar
    enum Avatar {a1,a2,a3};
    Avatar hostAvatar;
    Avatar guestAvatar;*/

    int time;

    //enum ExvitationControl { time, host, facilitator}
    //ExvitationControl exviteControl;
	
        
	void Awake() {
		if (instance == null)
			 instance = this;
		 else if (instance != this)
             Destroy(gameObject); 
	}

    private void Start()
    {
        UI_Panels = new GameObject[UI_Parent.transform.childCount];

        for (int i = 0; i < UI_Parent.transform.childCount; i++)
        {
            UI_Panels[i] = UI_Parent.transform.GetChild(i).gameObject;
        }

        WelcomeScreen();
        ShowUI();
    }

    void Update() {


		if(Input.GetKeyDown(KeyCode.Alpha0)) {
			room = 0;
			id = 0;
		}

		if(Input.GetKeyDown(KeyCode.Alpha1)) {
			room = 1;
			id = 1;
		}

		if(Input.GetKeyDown(KeyCode.Alpha2)) {
			room = 2;
			id = 2;
		}

        ShowUI();
	}

    //switch between UI panels based on Panel enum value
    private void ShowUI()
    {
        for (int i = 0; i < UI_Panels.Length; i++)
        {
            UI_Panels[i].SetActive(false);
        }
        UI_Panels[(int)panel].SetActive(true);               
    }

    //flow of the UI experience:
    //0. Welcome
    //1. Role
    //2. Room
    //3. Inv/Entry/Exit
    //4. Exvitaiton Prompt
    //5. Exvitation Controller
    //6. Experience Timer
    //7. Avatar (go button here)
    //8. Loading Screen

    #region welcome 
    //0. welcome
    void WelcomeScreen()
    {
        panel = Panel.welcome;
    }

    public void PressedBegin()
    {
        Debug.Log("pressed");
        panel = Panel.role;
    }
    #endregion

    #region role
    //1. Role
    public void ChooseHost()
    {
        isHost = true;
        panel = Panel.room;
    }

    public void ChooseGuest()
    {
        isHost = false;
        //set room id to meditation here

        //rest of the things, except the avatar are selected by the host
        panel = Panel.avatar;
    }
    #endregion

    #region room
    //room
    public void SelectHearth()
    {
        room = 0;
        id = 0;
        panel = Panel.hearth_entryExit;
    }

    public void SelectGarden()
    {
        room = 2;
        id = 2;
        panel = Panel.garden_entryExit;
    }
    #endregion

    #region invitation/entry/exit
    //inv/entry/exit 
    public void Hearth_inv_entry_exit()
    {
        //store entry/exit system in some variable here
        panel = Panel.hearth_exvitation;
    }

    public void Garden_inv_entry_exit()
    {  
        //store entry/exit system in some variable here
        panel = Panel.garden_exvitation;
    }
    #endregion

    #region exvitation
    //exvitation controller
    public void Hearth_exvitation()
    {
        //store exvitation in some variable here
        panel = Panel.exvitecontroller;
    }

    public void Garden_exvitation()
    {
        //store exvitation in some variable here
        panel = Panel.exvitecontroller;
    }
    #endregion

    #region exvitation controller
    //exvitation controller
    public void ExvitationController()
    {
        //store exvitationController in some variable here
        panel = Panel.time;
    }
    #endregion

    #region experience timer
    //set time variable
    public void SetTime5()
    {
        time = 5;
        panel = Panel.avatar;
    }
    public void SetTime10()
    {
        time = 10;
        panel = Panel.avatar;
    }
    public void SetTime15()
    {
        time = 15;
        panel = Panel.avatar;
    }
    #endregion

    #region avatar
    //set time variable
    public void Avatar1()
    {
        //store avatar type in some variable here
        panel = Panel.avatar;
    }
    public void Avatar2()
    {
        //store avatar type in some variable here
        panel = Panel.avatar;
    }
    public void Avatar3()
    {
        //store avatar type in some variable here
        panel = Panel.loading;
    }
    #endregion

}

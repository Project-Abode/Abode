using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using ExitGames.SportShooting;

public class LobbyMenuController: MonoBehaviour {

    //UI Panels
    GameObject[] UI_Panels;
    public GameObject UI_Parent;
    [HideInInspector] public enum Panel { welcome, role, room, 
    garden_entryExit, hearth_entryExit, hearth_exvitation, 
    garden_exvitation, exvitecontroller, time, avatar, loading,

    waitingHost

    };
    public Panel panel;

    public GameObject[] UIStatusText;

    // [HideInInspector] public bool isHost=false;
    
    /*
    //avatar
    enum Avatar {a1,a2,a3};
    Avatar hostAvatar;
    Avatar guestAvatar;*/

    //int time;

    //enum ExvitationControl { time, host, facilitator}
    //ExvitationControl exviteControl;
	
    public List<Text> choices;

    private void Awake()
    {
        UI_Panels = new GameObject[UI_Parent.transform.childCount];

        for (int i = 0; i < UI_Parent.transform.childCount; i++)
        {
            UI_Panels[i] = UI_Parent.transform.GetChild(i).gameObject;
        }

        for (int i = 0; i < UIStatusText.Length; i++)
        {
            UIStatusText[i].SetActive(false);
        }

        WelcomeScreen();
        ShowUI();
    }

    void Update() {
        ShowUI();

        if(Input.GetKeyDown(KeyCode.S)) {
            Settings.instance.Seed();
            Ending();
        }

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

    //button callbacks

    #region welcome 
    //0. welcome
    void WelcomeScreen()
    {
        panel = Panel.welcome;
    }

    public void PressedBegin()
    {
        //Debug.Log("pressed");
        panel = Panel.role;
    }
    #endregion

    #region role
    //1. Role
    public void ChooseHost()
    {
//        isHost = true;
        panel = Panel.room;
        GameObject.Find("/Lobby UI Parent/Lobby Selection UI/Canvas--Choices made/Role/Host").SetActive(true);
        Settings.instance.SetIsHost(true);
    }

    public void ChooseGuest()
    {
        //isHost = false;
        //set room id to meditation here
        Settings.instance.SetRoom(1);
        choices[1].text = "Room: " + "Guest Room";
        Settings.instance.SetIsHost(false);
        //rest of the things, except the avatar are selected by the host
        panel = Panel.avatar;

        GameObject.Find("/Lobby UI Parent/Lobby Selection UI/Canvas--Choices made/Role/Guest").SetActive(true);
        //GameObject.Find("/Lobby UI Parent/Lobby Selection UI/Canvas--Choices made/Room/Guest Home").SetActive(true);
        GameObject.Find("/Lobby UI Parent/Lobby Selection UI/Canvas--Choices made/Inv-Entry-Exit/Selected by Host").SetActive(true);
        GameObject.Find("/Lobby UI Parent/Lobby Selection UI/Canvas--Choices made/Exvitation Prompt/Selected by Host").SetActive(true);
        GameObject.Find("/Lobby UI Parent/Lobby Selection UI/Canvas--Choices made/Experience Time/Selected by Host").SetActive(true);
        GameObject.Find("/Lobby UI Parent/Lobby Selection UI/Canvas--Choices made/Exvitation Controller/Selected by Host").SetActive(true);
    }
    #endregion

    #region room
    //room
    public void SelectHearth()
    {    
        Settings.instance.SetRoom(0);
        panel = Panel.hearth_entryExit;
        
        choices[1].text = "Room: " + "Hearth";
    }

    public void SelectGarden()
    {
        Settings.instance.SetRoom(2);
        panel = Panel.garden_entryExit;
        choices[1].text = "Room: " + "Garden";
       
    }
    #endregion


    #region invitation/entry/exit
    //inv/entry/exit 

    List<string> EEDict = new List<string>{
        //0: portal
        //1: magic wand
        //2: elevator
        //3: levelstream
        //4: magic door
        //5: hot airballoon
        "portal", "magic wand", "elevator", "levelstream", "magic door", "hot airballoon"

    };
    public void Hearth_inv_entry_exit(int setting_value)
    {
        //store entry/exit system in some variable here
        
        Settings.instance.SetEntryExitMethod(setting_value);

        panel = Panel.hearth_exvitation;
        choices[2].text = "Inv-Entry-Exit: " + EEDict[setting_value];

    }

    public void Garden_inv_entry_exit(int setting_value)
    {  
        //store entry/exit system in some variable here
        Settings.instance.SetEntryExitMethod(setting_value);
        panel = Panel.garden_exvitation;
        choices[2].text = "Inv-Entry-Exit: " + EEDict[setting_value];

    }
    #endregion

    List<string> ExvitationDict =  new List<string>{
        "Clock",
        "Rain", 
        "Candle", 
        "Gift Hearth",
        "Gift Garden", 
        "Air Balloon"
    };

    #region exvitation
    //exvitation controller
    public void ExvitationPrompt(int setting_value)
    {
        //store exvitation in some variable here
        Settings.instance.SetExvitation(setting_value);
        panel = Panel.exvitecontroller;
        choices[3].text = "Exvitation Prompt: " + ExvitationDict[setting_value];

    }

    #endregion

    List<string> ExvitationControlDict =  new List<string>{

        "Host","Timer", "Facilitator"

    };

    #region exvitation controller
    //exvitation controller
    public void ExvitationController(int setting_value)
    {

        panel = Panel.time;
        choices[4].text = "Exvitation Controller: " + ExvitationControlDict[setting_value];

    }
    #endregion

    #region experience timer
    //set time variable

    public void SetTimer(float time) {
        panel = Panel.avatar;
        choices[5].text = "Time: " + time + " Min";
    }

    #endregion

    #region avatar
    //set time variable

    public void SetAvatar(int avatar) {
        Settings.instance.SetAvatar(avatar);

        Ending();
        
    }

    void Ending() {
        if(Settings.instance.isHost) {
            panel = Panel.loading;
            Settings.instance.OnHostRequstedSync();
            GameController.Instance.EnterGameWithSettings();
        }else {
            panel = Panel.waitingHost;
        }
    }

    #endregion



}

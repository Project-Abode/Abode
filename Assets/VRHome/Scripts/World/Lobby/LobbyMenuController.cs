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

    //public GameObject[] UIStatusText;
    public GameObject choicePanel;
	
    public List<Text> choices; //record panel of settings

    AudioSource audio;

    private void Awake()
    {
        UI_Panels = new GameObject[UI_Parent.transform.childCount];

        for (int i = 0; i < UI_Parent.transform.childCount; i++)
        {
            UI_Panels[i] = UI_Parent.transform.GetChild(i).gameObject;
        }

        // for (int i = 0; i < UIStatusText.Length; i++)
        // {
        //     UIStatusText[i].SetActive(false);
        // }

        WelcomeScreen();
        ShowUI();

        //clear up previous settings
        Settings.instance.ClearUpSettings();


        audio = GetComponent<AudioSource>();
    }

    void Update() {
        ShowUI();

        //debug for host seed settings
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
        panel = Panel.role;
        choicePanel.SetActive(true);
        audio.Play();
    }
    #endregion

    #region role
    //1. Role
    public void ChooseHost()
    {
        panel = Panel.room;
        //GameObject.Find("/Lobby UI Parent/Lobby Selection UI/Canvas--Choices made/Role/Host").SetActive(true);
        choices[0].text = "ROLE: " + "Host";
        Settings.instance.SetIsHost(true);
        audio.Play();
    }

    public void ChooseGuest()
    {
        Settings.instance.SetRoom(1);
        choices[0].text = "ROLE: " + "Guest";
        choices[1].text = "ROOM: " + "Guest Room";
        Settings.instance.SetIsHost(false);
        //rest of the things, except the avatar are selected by the host
        panel = Panel.avatar;

        choices[2].text = "ENTRY | EXIT: " +  "Selected By Host";
        choices[3].text = "EXVITATION PROMPT: " +  "Selected By Host";
        choices[4].text = "EXVITATION CONTROL: " +  "Selected By Host";
        choices[5].text = "TIMER: " + "Selected By Host";


        audio.Play();

        //GameObject.Find("/Lobby UI Parent/Lobby Selection UI/Canvas--Choices made/Role/Guest").SetActive(true);
        // GameObject.Find("/Lobby UI Parent/Lobby Selection UI/Canvas--Choices made/Inv-Entry-Exit/Selected by Host").SetActive(true);
        // GameObject.Find("/Lobby UI Parent/Lobby Selection UI/Canvas--Choices made/Exvitation Prompt/Selected by Host").SetActive(true);
        // GameObject.Find("/Lobby UI Parent/Lobby Selection UI/Canvas--Choices made/Experience Time/Selected by Host").SetActive(true);
        // GameObject.Find("/Lobby UI Parent/Lobby Selection UI/Canvas--Choices made/Exvitation Controller/Selected by Host").SetActive(true);
    }
    #endregion

    #region room
    public void SelectHearth()
    {    
        Settings.instance.SetRoom(0);
        panel = Panel.hearth_entryExit;
        
        choices[1].text = "Room: " + "Hearth";
        audio.Play();
    }

    public void SelectGarden()
    {
        Settings.instance.SetRoom(2);
        panel = Panel.garden_entryExit;
        choices[1].text = "Room: " + "Garden";
        audio.Play();
       
    }
    #endregion


    #region invitation/entry/exit

    List<string> EEDict = new List<string>{
        //0: portal
        //1: magic wand
        //2: elevator
        //3: levelstream
        //4: magic door
        //5: hot airballoon
        "Hearth Portal",
        "Hearth Magic Wand", 

        "Elevator", 
        "Level Stream",
        
        "Garden Portal", 
        "Garden Magic Wand"

    };
    public void Hearth_inv_entry_exit(int setting_value)
    {       
        Settings.instance.SetEntryExitMethod(setting_value);

        panel = Panel.hearth_exvitation;
        choices[2].text = "Inv-Entry-Exit: " + EEDict[setting_value];
        audio.Play();
    }

    public void Garden_inv_entry_exit(int setting_value)
    {  
        Settings.instance.SetEntryExitMethod(setting_value);
        panel = Panel.garden_exvitation;
        choices[2].text = "Inv-Entry-Exit: " + EEDict[setting_value];
        audio.Play();
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
    public void ExvitationPrompt(int setting_value)
    {
        Settings.instance.SetExvitation(setting_value);
        panel = Panel.exvitecontroller;
        choices[3].text = "Exvitation Prompt: " + ExvitationDict[setting_value];
        audio.Play();
    }

    #endregion

    List<string> ExvitationControlDict =  new List<string>{

       "Facilitator", "Host","Timer"

    };

    #region exvitation controller
    public void ExvitationController(int setting_value)
    {
        choices[4].text = "Exvitation Controller: " + ExvitationControlDict[setting_value];

        if(setting_value == 2) {
            panel = Panel.time;
        }else {
            panel = Panel.avatar;
        }

        audio.Play();
    }
    #endregion

    #region experience timer
    //set time variable
    public void SetTimer(float time) {
        panel = Panel.avatar;
        choices[5].text = "Time: " + time + " Min";
        audio.Play();
    }

    #endregion

    #region avatar
    //set avatar
    public void SetAvatar(int avatar) {
        Settings.instance.SetAvatar(avatar);
        audio.Play();
        Ending();
    }


    void Ending() {
        Settings.instance.FinishedSetting();

        if(Settings.instance.isHost) {
            panel = Panel.loading;
            SettingBuffer.instance.OnHostRequstedSync();
            GameController.Instance.EnterGameWithSettings();
        }else {
            panel = Panel.waitingHost;
        }
    }

    #endregion


    Dictionary<Panel,Panel> prevPanel = new Dictionary<Panel, Panel>() {
        {Panel.role, Panel.welcome},
        {Panel.room, Panel.role},
        {Panel.garden_entryExit, Panel.room},
        {Panel.hearth_entryExit, Panel.room},
        {Panel.hearth_exvitation, Panel.hearth_entryExit},
        {Panel.garden_exvitation, Panel.garden_entryExit},

        {Panel.time, Panel.exvitecontroller},
        {Panel.avatar, Panel.exvitecontroller}
        
    };


    // [HideInInspector] public enum Panel { welcome, role, room, 
    // garden_entryExit, hearth_entryExit, hearth_exvitation, 
    // garden_exvitation, exvitecontroller, time, avatar, loading,

    // waitingHost

    // };

    public void Back() {

        Panel backToPanel = Panel.welcome;
        switch(panel) {
            case Panel.hearth_entryExit:
                backToPanel = Panel.room;
                break;
            case Panel.hearth_exvitation:
                backToPanel = Panel.hearth_entryExit;
                break;
            case Panel.garden_exvitation:
                backToPanel = Panel.garden_entryExit;
                break;
            case Panel.exvitecontroller:
                if(Settings.instance.room == 0) {
                    backToPanel = Panel.hearth_exvitation;
                }else if (Settings.instance.room == 2) {
                    backToPanel = Panel.garden_exvitation;
                }
                break;
            case Panel.avatar:
                if(Settings.instance.id == 1) {
                    backToPanel = Panel.role;
                }else {
                    backToPanel = Panel.exvitecontroller;
                }
                break;
            default:
                backToPanel = (Panel)((int)panel-1);
                break;
        }

        if(backToPanel!=null && (int)backToPanel >= 0)
            panel = backToPanel;

    }

}

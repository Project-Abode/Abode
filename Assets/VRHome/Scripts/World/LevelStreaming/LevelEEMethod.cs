using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEEMethod : EEMethod {

    //DoorListener

    public List<SpaceDoor> doors;

    public List<HallwayDoor> hallwayDoors;

    public Transform VRPlayer;

    int guestID;
    int hostID ;

    //Positions:
    public Transform hallBase;
    Transform guestBase;
    Transform hostBase;

    public InRangeDetector guestOutDetector;
    public InRangeDetector hostOutDetector;
    public InRangeDetector hallHostOutDetector;

    override public void InitMethod(Transform VRPlayer = null) {

        this.VRPlayer = VRPlayer;

        if(forPlayer != Settings.instance.id) return;

        guestID = from;
        hostID = to;

        doors[guestID].notifyOpen += OnGuestDoorOpen;
        doors[guestID].notifyClose += OnGuestDoorClose;
        doors[hostID].notifyOpen += OnHostDoorOpen;
        doors[hostID].notifyClose += OnHostDoorClose;

        hallwayDoors[hostID].notifyTouched += OnHostHallWayDoorTouched;
        hallwayDoors[guestID].notifyTouched += OnGuestHallWayDoorTouched;


        guestOutDetector.notifyEnterArea += OnGuestEnterOutArea;
        hostOutDetector.notifyEnterArea += OnGuestEnterBackArea;

        guestBase = RoomSwitcher.instance.GetDescriptionAt(guestID).origin;
        hostBase = RoomSwitcher.instance.GetDescriptionAt(hostID).origin;

        doors[guestID].OnlyOpenDoor();
        
    }
    
    void OnDisable() {
        doors[guestID].notifyOpen -= OnGuestDoorOpen;
        doors[guestID].notifyClose -= OnGuestDoorClose;
        doors[hostID].notifyOpen -= OnHostDoorOpen;
        doors[hostID].notifyClose -= OnHostDoorClose;

        hallwayDoors[hostID].notifyTouched -= OnHostHallWayDoorTouched;
        hallwayDoors[guestID].notifyTouched -= OnGuestHallWayDoorTouched;

        this.VRPlayer = null;
        guestID = -1;
        hostID = -1;
    }

    void OnGuestHallWayDoorTouched() {
        Debug.Log("OnGuestHallWayDoorTouched");
        //doors[guestID].OperateDoor();
        doors[guestID].OnlyOpenDoor();
        //VRPlayer.position = guestBase.position;
        EntryExitManager.instance.TeleportPlayerTo(guestID, guestBase.position);
    }

    void OnHostHallWayDoorTouched() {
        Debug.Log("OnHostHallWayDoorTouched");
        doors[hostID].KnockDoor();
    }

    void OnGuestDoorOpen() {
        Debug.Log("OnGuestDoorOpen");
    
    }

    void OnGuestDoorClose() {
        Debug.Log("OnGuestDoorClose");

        // if(guestOutDetector.InRange()) {
        //     //VRPlayer.position = hallBase.position;
        //     EntryExitManager.instance.TeleportPlayerTo(2, hallBase.position);
        // }

    }

    void OnGuestEnterOutArea(){
        //close guest door
        guestOutDetector.notifyEnterArea -= OnGuestEnterOutArea;
        guestOutDetector.enabled = false;
        doors[guestID].OnlyCloseDoor();
        EntryExitManager.instance.TeleportPlayerTo(2, hallBase.position);
    }

    void OnGuestEnterBackArea() {
        hostOutDetector.notifyEnterArea -= OnGuestEnterBackArea;
        hostOutDetector.enabled = false;
        EntryExitManager.instance.TeleportPlayerTo(2, hallBase.position);
    }


    void OnHostDoorOpen() {
        Debug.Log("OnHostDoorOpen");

        if(hallHostOutDetector.InRange()) {
            //VRPlayer.position = hostBase.position;
            EntryExitManager.instance.TeleportPlayerTo(hostID, hostBase.position);
        }

    }

    void OnHostDoorClose() {
        Debug.Log("OnHostDoorClose");

        //  if(hostOutDetector.InRange()) {
        //     //VRPlayer.position = hallBase.position;
        //     EntryExitManager.instance.TeleportPlayerTo(2, hallBase.position);
        // }

    }

    void Update() {
        if(Input.GetKeyDown(KeyCode.Space)) {
            InitMethod();

            //HACK only for single player
            VRPlayer = GameObject.Find("Player(Clone)").transform;

        }
    }

    void Awake() {

    }



}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExitGames.SportShooting;

public class Door : MonoBehaviour {


    bool opened = false;
    public bool isMainDoor = false;

    [SerializeField]
    PhotonView _photonView;

    private AudioSource _knockSound;

    private Coroutine coroutine;

    IEnumerator OpenDoor()
    {
        //corutine
        var time = 3f;
        var elapsedTime = 0f;
        Quaternion startingRotation = transform.rotation; // have a startingRotation as well
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, 0f));
        while (elapsedTime < time)
        {
            elapsedTime += Time.deltaTime; // <- move elapsedTime increment here
            transform.rotation = Quaternion.Slerp(startingRotation, targetRotation, (elapsedTime / time));
            yield return new WaitForEndOfFrame();
        }

        yield return null;

    }

    IEnumerator CloseDoor()
    {
        //corutine
        var time = 3f;
        var elapsedTime = 0f;
        Quaternion startingRotation = transform.rotation; // have a startingRotation as well
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0.0f, 90.0f, 0f));
        while (elapsedTime < time)
        {
            elapsedTime += Time.deltaTime; // <- move elapsedTime increment here
            transform.rotation = Quaternion.Slerp(startingRotation, targetRotation, (elapsedTime / time));
            yield return new WaitForEndOfFrame();
        }

        yield return null;

    }


    private void OnTriggerEnter(Collider col)
    {

        //when guest hands on:
        //play knock sound

        //when host hands on:
        //open the door
        //or close the door

        //hack judge if this hand is remote hand
        var player_t = col.transform.parent.parent.parent;
        Debug.Log(player_t.name);
        //var player = col.transform.parent.parent.parent.GetComponent<Player>();
        var player = player_t.GetComponent<Player>();
        var isThisPlayer = player.CameraRig.gameObject.activeSelf;
        //Debug
        if (!isThisPlayer)
        {
            return;
        }

        //Debug.Log("Collide");

        if (col.gameObject.tag == "Hand")
        {

            if (PhotonNetwork.isMasterClient || isMainDoor) //Host
            {
                Debug.Log("Host hand collide");
                if (col.gameObject.tag == "Hand")
                {

                    //return;
                    //open the door
                    if (opened)
                    {
                        opened = false;
                        if (coroutine != null)
                        {
                            StopCoroutine(coroutine);
                        }

                        coroutine = StartCoroutine(CloseDoor());
                    }
                    else
                    {
                        opened = true;
                        if (coroutine != null)
                        {
                            StopCoroutine(coroutine);
                        }

                        coroutine = StartCoroutine(OpenDoor());
                    }


                }

            }
            else //Guest
            {

                Debug.Log("Guest hand collide");

                if (col.gameObject.tag == "Hand")
                {
                    //play sound RPC
                    Debug.Log("Play knock sound");


                    _photonView.RPC("PlayKnockSound", PhotonTargets.All);
                }
            }
        }
        
 

    }


    [PunRPC]
    public void PlayKnockSound()
    {
        Debug.Log("in knock sound");
        _knockSound.PlayOneShot(_knockSound.clip);
    }


    // Use this for initialization
    void Awake () {
        _knockSound = GetComponent<AudioSource>();
    }

}

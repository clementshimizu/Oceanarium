using UnityEngine;
using System.Collections;

//This script makes any Sprite object look at the player's camera with the correct orientation from Gravity 
public class CameraFacingBillboard : MonoBehaviour
{
    GameObject player;
    PlayerController pc;
    private GravityBody playerBody;
    Camera playerCam;
    CameraController camControl;
    //fades sprite when in front of pcam
    FadeForCamera cameraFader;
    SpriteRenderer sr;

    [Tooltip("Has gravity body set up")]
    public bool hasGravityBody;
    GravityBody gravBody;

	void Awake(){
        //player refs
        player = GameObject.FindGameObjectWithTag("Player");
        if (player!= null)
        {
            playerBody = player.GetComponent<GravityBody>();
            pc = player.GetComponent<PlayerController>();
        }
      
        playerCam = Camera.main;
        camControl = playerCam.GetComponent<CameraController>();

        sr = GetComponent<SpriteRenderer>();

        //add camera fader if not on object already 
        cameraFader = GetComponent<FadeForCamera>();
        if(cameraFader == null)
        {
            cameraFader = gameObject.AddComponent<FadeForCamera>();
        }
        //gets and uses own grav body 
        if (hasGravityBody)
        {
            gravBody = GetComponent<GravityBody>();

            if(gravBody == null)
            {
                gravBody = gameObject.AddComponent<GravityBody>();
            }
        }
	}

	void Update(){
      
        //uses own gravity for up axis
        if (hasGravityBody)
        {
            if(sr.isVisible)
                transform.LookAt(playerCam.transform.position, gravBody.GetUp());
        }
        //normal, uses player gravity body for Look at function 
        else
        {
            //NORMAL MOVEMENT 
            if(pc.moveState != PlayerController.MoveStates.MEDITATING)
            {
                if (player)
                    transform.LookAt(playerCam.transform.position, playerBody.GetUp());
                else
                    transform.LookAt(playerCam.transform.position, Vector3.up);
            }
            //MEDITATING
            else if (pc.moveState == PlayerController.MoveStates.MEDITATING)
            {
                //fp -- look at cam
                if (pc.firstOrThirdPersonMeditation)
                    transform.LookAt(playerCam.transform.position, playerCam.transform.up);
                //tp -- use astral body up 
                else
                    transform.LookAt(playerCam.transform.position, camControl.gravityBody.GetUp());
            }
        }
	}

    private void OnBecameVisible()
    {
        if (hasGravityBody)
        {
            if (gravBody.enabled)
                gravBody.enabled = false;
        }
    }

    private void OnBecameInvisible()
    {
        if (hasGravityBody)
        {
            if (!gravBody.enabled)
                gravBody.enabled = true;
        }
    }

}

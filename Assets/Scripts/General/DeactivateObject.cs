﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactivateObject : MonoBehaviour
{
    GameObject _player;
    PlayerController pc;
    WorldManager wm;

    //so worldmanager can access these 
    [HideInInspector]
    public SpriteRenderer sRenderer;
    [HideInInspector]
    public FadeSprite fader;
    
    [Tooltip("Added to WorldMan dist")]
    public float individualDistOffset = 10f;
    public float distFromPlayer;
    void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        pc = _player.GetComponent<PlayerController>();
        wm = FindObjectOfType<WorldManager>();
        sRenderer = GetComponent<SpriteRenderer>();

        //assure sprite renderer and fade sprite exist 
        if (sRenderer == null)
        {
            sRenderer = GetComponentInChildren<SpriteRenderer>();

            fader = sRenderer.GetComponent<FadeSprite>();
            if (fader == null)
            {
                fader = sRenderer.gameObject.AddComponent<FadeSprite>();
            }
        }
        //sprite renderer found on this transform
        else
        {
            fader = GetComponent<FadeSprite>();
            if (fader == null)
            {
                fader = gameObject.AddComponent<FadeSprite>();
            }
        }

        //set fader
        if (fader)
        {
            fader.wm = this;
            fader.worldManage = true;
        }
    }

    void Update()
    {
        //wm null check
        if(wm != null)
        {
            //if player is moving 
            if (pc.playerRigidbody.velocity.magnitude > 0)
            {
                DistCheck();
            }
        }
    }

    //deactivate object when it's far enough away from player 
    void DistCheck()
    {
        distFromPlayer = Vector3.Distance(_player.transform.position, transform.position);

        //check to see if its greater than wm dist
        if (distFromPlayer > (wm.activationDistance + individualDistOffset))
        {
            //null check on sprite renderer
            if(fader != null)
            {
                //fade out! -- if not already. Fader will Deactivate once it is transparent
                if (fader.fadeState != FadeSprite.FadeStates.FADINGOUT && 
                    fader.fadeState != FadeSprite.FadeStates.TRANSPARENT)
                {
                    fader.FadeOut();
                }
            }
            //just deactivate
            else
            {
                Deactivate();
            }
        }
    }

    public void Deactivate()
    {
        //first add to list
        wm.allInactiveObjects.Add(gameObject);
        //then deactivate 
        gameObject.SetActive(false);
    }
}
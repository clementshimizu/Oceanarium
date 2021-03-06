﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using InControl;

public class QuitGame : MonoBehaviour {
    PlayerController pc;
    CameraController camControl;
    MenuSelections mainMenuSelections;

    public GameObject escMenu;
    [Header("Controls UI")]
    public GameObject controlsUI;

    void Awake()
    {
        mainMenuSelections = GetComponent<MenuSelections>();
        pc = FindObjectOfType<PlayerController>();
        camControl = FindObjectOfType<CameraController>();
    }

    void Start()
    {
        escMenu.SetActive(false);
    }

    void Update ()
    {
        bool pressed = false;

        //get input device 
        var inputDevice = InputManager.ActiveDevice;
        
        //activate quit group   
        if ((Input.GetKeyDown(KeyCode.Escape) ||  inputDevice.Command.WasPressed) && escMenu.activeSelf == false && !pressed)
        {
            ActivateQuitMenu();
           
            pressed = true;
        }

        //either turns off all sub menus, or leaves esc menu 
        if ((Input.GetKeyDown(KeyCode.Escape) || inputDevice.Command.WasPressed) && escMenu.activeSelf == true && !pressed)
        {
            bool subMenus = mainMenuSelections.CheckSubMenusActive();

            if (subMenus)
                mainMenuSelections.DeactivateAllSubMenus();
            else
            {
                DeactiveMenu();
            }
                
            pressed = true;
        }
    }
    
    //called to open esc menu 
    public void ActivateQuitMenu()
    {
        //disable movement
        if(pc.moveState == PlayerController.MoveStates.MEDITATING)
            pc.DisableMeditation();
        pc.canMove = false;
        pc.playerRigidbody.velocity = Vector3.zero;
        camControl.canMoveCam = false;
      
        //enable cursor
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        //activate menu & set Selectors
        escMenu.SetActive(true);
        mainMenuSelections.ActivateMenu();
    }
    
    public void DeactiveMenu()
    {
        //leave menu
        mainMenuSelections.DeactivateMenu();
        DeactivateObj(escMenu);

        //enable movement 
        pc.canMove = true;
        camControl.canMoveCam = true;
       
        //relock cursor 
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void ShowControls()
    {
        //enable controls header obj
        controlsUI.SetActive(true);
    }
    
    //on the 'no' under q prompts
    public void DeactivateObj(GameObject obj)
    {
        obj.SetActive(false);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(0);
    }
}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GH;

public class CheckForRestart : GH.Event
{

}

public class MouseClickedData : GH.Event
{
    public bool clicked = false;
}
public class KeyboardPressed : GH.Event
{
    public float horizontal = Input.GetAxis("Horizontal");
    public float vertical = Input.GetAxis("Vertical");
}
public class Rushing : GH.Event
{
    public bool rush = false;
}

public class Eat : GH.Event
{
    public bool eat = false;
}



public class InputManager : MonoBehaviour
{

    protected static InputManager _instance;
    bool checkRestartButton = false;
    public bool inverted = false;

    public bool pasue = false;

    public static InputManager Instance
    {
        get { return _instance; }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }

        EventSystem.instance.AddListener<CheckForRestart>(CheckRestartButton);
    }
    private void OnDisable()
    {
        EventSystem.instance.RemoveListener<CheckForRestart>(CheckRestartButton);
    }

    private void Update()
    {

        if (Input.GetKey(KeyCode.I))
        {
            inverted = !inverted;
        }

        if (Input.GetKey(KeyCode.P) || Input.GetKey(KeyCode.Joystick1Button3))
        {
            if (!pasue)
            {
                EventSystem.instance.RaiseEvent(new PauseGame { pause = true });
                pasue = true;
            }
            else
            {
                EventSystem.instance.RaiseEvent(new PauseGame { pause = false });
                pasue = false;
            }
        }

        if (!inverted)
        {
            if (GameManager.Instance.Platform == Platform.PC)
            {
                EventSystem.instance.RaiseEvent(new KeyboardPressed
                {
                    horizontal = Input.GetAxis("Horizontal"),
                    vertical = Input.GetAxis("Vertical")
                });
                //            Debug.Log("This is PC");
            }
            else if (GameManager.Instance.Platform == Platform.Logitech)
            {
                EventSystem.instance.RaiseEvent(new KeyboardPressed
                {

                    horizontal = Input.GetAxis("Logitech_RightJoystickX"),
                    vertical = Input.GetAxis("Logitech_RightJoystickY")
                });
                Debug.Log("This is controller");
            }
            else if (GameManager.Instance.Platform == Platform.Xbox)
            {
                EventSystem.instance.RaiseEvent(new KeyboardPressed
                {
                    horizontal = Input.GetAxis("XboxOne_RightJoyStickX"),
                    vertical = Input.GetAxis("XboxOne_RightJoyStickY")
                });
            }
            else if (GameManager.Instance.Platform == Platform.PS4)
            {
                EventSystem.instance.RaiseEvent(new KeyboardPressed
                {
                    horizontal = Input.GetAxis("PS4_RightJoyStickX"),
                    vertical = Input.GetAxis("PS4_RightJoyStickY")
                });
            }
        }
        else
        {
            if (GameManager.Instance.Platform == Platform.PC)
            {
                EventSystem.instance.RaiseEvent(new KeyboardPressed
                {
                    horizontal = Input.GetAxis("Horizontal"),
                    vertical = Input.GetAxis("Vertical") * -1
                });
                //            Debug.Log("This is PC");
            }
            else if (GameManager.Instance.Platform == Platform.Xbox)
            {
                EventSystem.instance.RaiseEvent(new KeyboardPressed
                {
                    horizontal = Input.GetAxis("XboxOne_RightJoyStickX"),
                    vertical = Input.GetAxis("XboxOne_RightJoyStickY") * -1
                });
            }
            else if (GameManager.Instance.Platform == Platform.PS4)
            {
                EventSystem.instance.RaiseEvent(new KeyboardPressed
                {
                    horizontal = Input.GetAxis("PS4_RightJoyStickX"),
                    vertical = Input.GetAxis("PS4_RightJoyStickY") * -1
                });
            }
            else if (GameManager.Instance.Platform == Platform.Logitech)
            {
                EventSystem.instance.RaiseEvent(new KeyboardPressed
                {

                    horizontal = Input.GetAxis("Logitech_RightJoystickX"),
                    vertical = Input.GetAxis("Logitech_RightJoystickY") * -1
                });
                Debug.Log("This is controller");
            }
        }
        //Can proabbaly use Events but nah
        /* switch (GameManager.Instance.Platform)
         {
             case (Platform.PC):
                 EventSystem.instance.RaiseEvent(new KeyboardPressed
                 {
                     horizontal = Input.GetAxis("Horizontal"),
                     vertical = Input.GetAxis("Vertical")
                 });
                 Debug.Log("This is PC");

                 break;
             case (Platform.Logitech):
                 EventSystem.instance.RaiseEvent(new KeyboardPressed
                 {

                     horizontal = Input.GetAxis("RightJoystickX"),
                     vertical = Input.GetAxis("RightJoystickY")
                 });
                 Debug.Log("This is controller");

                 break;
         }
         */


        //I edited the Input for Fires since there was an error for me if you don't have one change it back to what works
        if ((GameManager.Instance.Platform == Platform.PC && (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.R))) ||
            (GameManager.Instance.Platform == Platform.Xbox && Input.GetKeyDown(KeyCode.JoystickButton1))||
            ((GameManager.Instance.Platform == Platform.PS4 || GameManager.Instance.Platform == Platform.Logitech) && Input.GetKeyDown(KeyCode.JoystickButton2))
            )//Rush Button
        {
            //_rush = true;
            EventSystem.instance.RaiseEvent(new Rushing
            {
                rush = true
            });
        }
        else
        {
            //_rush = false;
            EventSystem.instance.RaiseEvent(new Rushing { rush = false });
        }

        //The following isn't good enough because time needs to pass before returning back to false
        if ((GameManager.Instance.Platform == Platform.PC && Input.GetKeyUp(KeyCode.E)) || 
            ((GameManager.Instance.Platform == Platform.Logitech || GameManager.Instance.Platform == Platform.PS4) && Input.GetKeyUp(KeyCode.JoystickButton0))||
            (GameManager.Instance.Platform == Platform.Xbox && Input.GetKeyUp(KeyCode.JoystickButton2))
            )//Eat Button
        {
            EventSystem.instance.RaiseEvent(new Eat { eat = true });
        }
        else
        {
            EventSystem.instance.RaiseEvent(new Eat { eat = false });
        }

        if (Input.GetButton("Fire3"))
        {
            //_pause = !_pause;
        }

        if ((GameManager.Instance.Platform == Platform.PC && Input.GetMouseButton(0)) || 
            ((GameManager.Instance.Platform == Platform.PS4 || GameManager.Instance.Platform == Platform.Logitech) && (Input.GetKey(KeyCode.JoystickButton7)) || Input.GetKey(KeyCode.JoystickButton6))||
            (GameManager.Instance.Platform == Platform.Xbox && (Input.GetAxis("Xbox_RightTrigger")!=0 || Input.GetAxis("Xbox_LeftTrigger") != 0))
            )
        {
            EventSystem.instance.RaiseEvent(new MouseClickedData { clicked = true });
        }
        else
        {
            EventSystem.instance.RaiseEvent(new MouseClickedData { clicked = false });
        }

        if (checkRestartButton)
        {

            if (
                (Input.GetKey(KeyCode.Space) && GameManager.Instance.Platform == Platform.PC) || 
                (Input.GetKey(KeyCode.JoystickButton3) && (GameManager.Instance.Platform == Platform.PS4 || GameManager.Instance.Platform == Platform.Logitech))||
                (Input.GetKey(KeyCode.JoystickButton3) && GameManager.Instance.Platform == Platform.Xbox)
                )
            {
                EventSystem.instance.RaiseEvent(new Restart { });
            }
        }
        if (Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.JoystickButton0) || Input.GetKey(KeyCode.JoystickButton2) || Input.GetKey(KeyCode.JoystickButton1) || Input.GetKey(KeyCode.JoystickButton3))
        {
            EventSystem.instance.RaiseEvent(new OnDismissCollectable { });
        }

    }

    void CheckRestartButton(CheckForRestart restart)
    {
        checkRestartButton = true;
    }
}

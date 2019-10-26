using System.Collections;
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

                    horizontal = Input.GetAxis("RightJoystickX"),
                    vertical = Input.GetAxis("RightJoystickY")
                });
                Debug.Log("This is controller");
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
            else if (GameManager.Instance.Platform == Platform.Logitech)
            {
                EventSystem.instance.RaiseEvent(new KeyboardPressed
                {

                    horizontal = Input.GetAxis("RightJoystickX"),
                    vertical = Input.GetAxis("RightJoystickY") * -1
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
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Joystick1Button1))//Rush Button
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
        if (Input.GetKeyUp(KeyCode.E) || Input.GetKeyUp(KeyCode.Joystick1Button2))//Eat Button
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

        if (Input.GetMouseButton(0) || Input.GetKey(KeyCode.Joystick1Button7) || Input.GetKey(KeyCode.Joystick1Button6))
        {
            EventSystem.instance.RaiseEvent(new MouseClickedData { clicked = true });
        }
        else
        {
            EventSystem.instance.RaiseEvent(new MouseClickedData { clicked = false });
        }

        if (checkRestartButton)
        {
            if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.Joystick1Button0))
            {
                EventSystem.instance.RaiseEvent(new Restart { });
            }
        }
        if (Input.GetKey(KeyCode.M) || Input.GetKey(KeyCode.Joystick1Button2))
        {
            EventSystem.instance.RaiseEvent(new OnDismissCollectable { });
        }

    }

    void CheckRestartButton(CheckForRestart restart)
    {
        checkRestartButton = true;
    }
}

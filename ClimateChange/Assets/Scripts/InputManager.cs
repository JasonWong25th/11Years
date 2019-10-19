using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GH;

public class CheckForRestart : GH.Event
{

}

<<<<<<< Updated upstream:ClimateChange/Assets/Scripts/InputManager.cs
#region Input Events
=======
>>>>>>> Stashed changes:ClimateChange/Assets/_Scripts/InputManager.cs
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
<<<<<<< Updated upstream:ClimateChange/Assets/Scripts/InputManager.cs
public class AnalogStick : GH.Event
{
    public float horizontal = Input.GetAxis("Horizontal_Controller");
    public float vertical = Input.GetAxis("Vertical_Controller");
}
#endregion

public class InputManager : MonoBehaviour {

    protected static InputManager _instance;

    public static InputManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new InputManager();
            }
            return _instance;
        }
    }

    #region Variables we aren't currently using
    protected Vector3 _movement = new Vector3(0.0f,0.0f,0.0f);
    protected Vector2 _camera;
    protected bool _inControl;
    protected bool _rush;
    protected bool _eat;
    protected bool _pause;
    protected bool _mouseClicked;
=======

public class Eat : GH.Event
{
    public bool eat = false;
}

>>>>>>> Stashed changes:ClimateChange/Assets/_Scripts/InputManager.cs


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

<<<<<<< Updated upstream:ClimateChange/Assets/Scripts/InputManager.cs
    public bool Pause
    {
        get { return _pause && _inControl; } //should player be able to pause at any time?
    }
    public bool MouseClicked
    {
        get { return _mouseClicked; } //should player be able to pause at any time?
    }
    #endregion

    private void Update()
    {
        if(Input.GetAxis("Vertical_Controller") == 0 && Input.GetAxis("Horizontal_Controller") == 0)
=======
    private void Awake()
    {
        if (_instance == null)
>>>>>>> Stashed changes:ClimateChange/Assets/_Scripts/InputManager.cs
        {
            EventSystem.instance.RaiseEvent(new KeyboardPressed
            {
                horizontal = Input.GetAxis("Horizontal"),
                vertical = Input.GetAxis("Vertical")
            });
        }
<<<<<<< Updated upstream:ClimateChange/Assets/Scripts/InputManager.cs
        else
=======
        else if (_instance != this)
>>>>>>> Stashed changes:ClimateChange/Assets/_Scripts/InputManager.cs
        {
            EventSystem.instance.RaiseEvent(new AnalogStick
            {
                horizontal = Input.GetAxis("Horizontal_Controller"),
                vertical = Input.GetAxis("Vertical_Controller")
            });
        }

<<<<<<< Updated upstream:ClimateChange/Assets/Scripts/InputManager.cs
        //I edited the Input for Fires since there was an error for me if you don't have one change it back to what works
        if (Input.GetButton("Fire1") )//Rush Button
        {
            //_rush = true;
            Debug.Log("detected rush input");  // Rush input is always getting input...?
            EventSystem.instance.RaiseEvent(new Rushing
            {
                rush = true
            });
        }
        else 
        {
            //_rush = false;
            EventSystem.instance.RaiseEvent(new Rushing{rush = false});
        }

        //The following isn't good enough because time needs to pass before returning back to false
        if(Input.GetButtonDown("Fire2"))//Eat Button
        {
            _eat = true;
        }
        else
=======
        EventSystem.instance.AddListener<CheckForRestart>(CheckRestartButton);
    }
    private void OnDisable()
    {
        EventSystem.instance.RemoveListener<CheckForRestart>(CheckRestartButton);
    }

    private void Update()
    {
        
            if (Input.GetKey(KeyCode.I))
>>>>>>> Stashed changes:ClimateChange/Assets/_Scripts/InputManager.cs
        {
            inverted = !inverted;
        }

        if (Input.GetKey(KeyCode.P)||Input.GetKey(KeyCode.Joystick1Button3))
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
<<<<<<< Updated upstream:ClimateChange/Assets/Scripts/InputManager.cs

        if (Input.GetMouseButton(0) || Input.GetKey(KeyCode.Joystick1Button0))//Mouse Click
=======
        if (!inverted)
>>>>>>> Stashed changes:ClimateChange/Assets/_Scripts/InputManager.cs
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
                    vertical = Input.GetAxis("RightJoystickY") *-1
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

<<<<<<< Updated upstream:ClimateChange/Assets/Scripts/InputManager.cs
=======
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
        
>>>>>>> Stashed changes:ClimateChange/Assets/_Scripts/InputManager.cs
    }

    void CheckRestartButton(CheckForRestart restart)
    {
        checkRestartButton = true;
    }
}

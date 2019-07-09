using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GH;


#region Input Events
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

    public Vector3 MoveInput
    {
        get{ return _movement; } //can insert checks in order to manage sensitivity of controller input
    }

    public Vector2 CameraInput
    {
        get { return _camera;  }//same reasoning as above
    }

    public bool Attack
    {
        get { return _rush && _inControl; }
    }

    public bool Rush
    {
        get { return _rush && _inControl; }
    }

    public bool Eat
    {
        get { return _eat && _inControl; }
    }

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
        {
            EventSystem.instance.RaiseEvent(new KeyboardPressed
            {
                horizontal = Input.GetAxis("Horizontal"),
                vertical = Input.GetAxis("Vertical")
            });
        }
        else
        {
            EventSystem.instance.RaiseEvent(new AnalogStick
            {
                horizontal = Input.GetAxis("Horizontal_Controller"),
                vertical = Input.GetAxis("Vertical_Controller")
            });
        }

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
        {
            _eat = false;
        }

        if(Input.GetButton("Fire3"))
        {
            _pause = !_pause;
        }

        if (Input.GetMouseButton(0) || Input.GetKey(KeyCode.Joystick1Button0))//Mouse Click
        {
            EventSystem.instance.RaiseEvent(new MouseClickedData{ clicked = true});
        }
        else
        {
            EventSystem.instance.RaiseEvent(new MouseClickedData { clicked = false });
        }

    }

}

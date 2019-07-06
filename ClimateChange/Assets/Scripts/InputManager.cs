using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GH;


//Based off of Raymond's class but rn it don't do anything
public class PassingInputData : GH.Event
{

}
public class InputManager : MonoBehaviour {

    protected static InputManager _instance;

    public static InputManager Instance
    {
        get { return _instance; }
    }


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
    private void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
        }
        else if(_instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        _movement.Set(Input.GetAxis("Horizontal"), 0.0f,Input.GetAxis("Vertical")); //Need to think more about this logic -Jason- I edited it not sure if it works
        CameraInput.Set(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));  //" "

        //I edited the Input for Fires since there was an error for me if you don't have one change it back to what works
        if (Input.GetButton("Fire1") )//Rush Button
        {
            _rush = true;
        }
        else 
        {
            _rush = false;
        }

        //The following isn't good enough because time needs to pass before returning back to false
        if(Input.GetButton("Fire2"))//Eat Button
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
        if (Input.GetMouseButton(0))
        {
            _mouseClicked = true;
        }
        else
        {
            _mouseClicked = false;
        }

        //COMMUNICATION WITH ___?

    }

}

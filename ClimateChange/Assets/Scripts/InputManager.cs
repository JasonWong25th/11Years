using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {

    protected static InputManager _instance;

    public static InputManager Instance
    {
        get { return _instance; }
    }


    protected Vector2 _movement;
    protected Vector2 _camera;
    protected bool _inControl;
    protected bool _rush;
    protected bool _eat;
    protected bool _pause;

    public Vector2 MoveInput
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
        MoveInput.Set(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")); //Need to think more about this logic
        CameraInput.Set(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));  //" "

        if (Input.GetButtonDown("Fire1") || Input.GetKeyDown("Fire1"))//Rush Button
        {
            _rush = true;
        }
        else 
        {
            _rush = false;
        }

        //The following isn't good enough because time needs to pass before returning back to false
        if(Input.GetButtonDown("Fire2") || Input.GetKeyDown("Fire2"))//Eat Button
        {
            _eat = true;
        }
        else
        {
            _eat = false;
        }

        if(Input.GetButtonDown("Fire3") || Input.GetKeyDown("Fire3"))
        {
            _pause = !_pause;
        }

        //COMMUNICATION WITH ___?

    }

}

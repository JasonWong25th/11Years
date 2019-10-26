using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GH;

public class PullCamera : GH.Event
{
    public bool closeToPlayer;
}

public class CameraController : MonoBehaviour {

    /*
    public Transform target;

    public Vector3 offset;

    public float distance;

    public float minFov = 35f;
    public float maxFov = 60f;

    public float sensitivity = .5f;


    private void LateUpdate()
    {
        //Copped this online after hella fucking googling and trying to do it
        //Felt hella dumb after find out it's only 4 lines of code
        Vector3 v = target.position;
        v += v.normalized * distance;

        transform.position = v;
        transform.LookAt(target.transform, transform.up);
    }

    private void Update()
    {
        float fov = Camera.main.fieldOfView;
        fov += Input.GetAxis("Mouse ScrollWheel"); //* -sensitivity;
        fov = Mathf.Clamp(fov, minFov, maxFov);
        Camera.main.fieldOfView = fov;
    }
    */
    [SerializeField]
    private Transform target;
    public Transform mainTarget;
    public Transform anemonePosition;

    [SerializeField]
    private Vector3 offsetPosition;

    [SerializeField]
    private Space offsetPositionSpace = Space.Self;

    [SerializeField]
    private bool lookAt = true;
    public bool follow = true;

    float startTime;
    float journeyLength;
    public float rotateFollowSpeed = 2f;
    public float positionFollowSpeed = .05f;
    public Transform target_rot;
    //[SerializeField]
    //private bool startTimer = false;

    /*private void Awake()
    {
        EventSystem.instance.AddListener<MouseClickedData>(InitiateLateFollow);
    }
    */
    private void Awake()
    {
        EventSystem.instance.AddListener<PullCamera>(PullCamera);
    }

    void Start()
    {

        // Keep a note of the time the movement started.
        //startTime = Time.time;

        // Calculate the journey length.
        //journeyLength = Vector3.Distance(transform.position, target.position);
    }


    private void FixedUpdate() //use a smooth damp for camera spring like position
    {                          //and use fix delta time for deltatime * constant
                               // Distance moved = time * speed.
                               //float distCovered = (Time.time - startTime) * speed;

        // Fraction of journey completed = current distance divided by total distance.
        //float fracJourney = distCovered / journeyLength;
        //Debug.Log(fracJourney);
        // Set our position as a fraction of the distance between the markers.
        //transform.position = Vector3.Lerp(transform.position, target.position, fracJourney);

     
       // if (follow)
        //{
            target = mainTarget;
            //SpositionFollowSpeed = 2f;
        //}
        //else
        //{
            //target = anemonePosition;
        //}
        Vector3 currVel = Vector3.zero;
        transform.position = Vector3.SmoothDamp(transform.position, target.position, ref currVel, positionFollowSpeed);

        Rotate();
    }

    private void LateUpdate()
    {
        
        
    }

    void PullCamera(PullCamera cam)
    {
        follow = cam.closeToPlayer;
    }

    public void Rotate() // use a smooth damp for spring like rotation pull and float damp 
    {                     // lfor smooth time like player
        /*if (target == null)
        {
            Debug.LogWarning("Missing target ref !", this);

            return;
        }

        // compute position
        if (offsetPositionSpace == Space.Self)
        {
            transform.position = target.TransformPoint(offsetPosition);
        }
        else
        {
            transform.position = target.position + offsetPosition;
        }*/

        // compute rotation
        if (lookAt)
        {
            Quaternion targetRot = Quaternion.LookRotation(target_rot.transform.position - transform.position);
            float angleDelta = Quaternion.Angle(transform.rotation, targetRot);
            float angleChange = 0f;
            if (angleDelta > 0f)
            {
                float damp = Mathf.SmoothDampAngle(angleDelta, 0f, ref angleChange, .1f); //This float scales rotation speed

                damp = 1f - (damp / angleDelta);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.fixedDeltaTime * rotateFollowSpeed); //This operation scales the lerp
            }

           
        
            //The smaller the number, more apparent the lerp
            //transform.LookAt(target_rot);
        }
        else
        {
            transform.rotation = target_rot.rotation;
        }
    }
}

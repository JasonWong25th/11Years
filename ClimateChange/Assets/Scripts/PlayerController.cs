using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GravityBody))] 
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    // public vars
    //public float mouseSensitivityX = 1;
    //public float mouseSensitivityY = 1;
    public float walkSpeed = 80;
    public float jumpForce = 220;
    public LayerMask groundedMask;

    // System vars
    bool grounded;
    Vector3 moveAmount;
    Vector3 smoothMoveVelocity;
    //float verticalLookRotation;
    //Transform cameraTransform;
    Rigidbody rigidbody;

    Animator anim;

    float rotX = 0.0f;
    float rotY = 0.0f;
    readonly float rotSpeed = 80.0f;


    void Awake()
    {
        /*
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        cameraTransform = Camera.main.transform;
        */
        rigidbody = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {

        //Look rotation: IF First Person
        //transform.Rotate(Vector3.up * Input.GetAxis("Mouse X") * mouseSensitivityX);
        //verticalLookRotation += Input.GetAxis("Mouse Y") * mouseSensitivityY;
        //verticalLookRotation = Mathf.Clamp(verticalLookRotation, -60, 60);
        //cameraTransform.localEulerAngles = Vector3.left * verticalLookRotation;

        // Calculate movement:
        float inputY= Input.GetAxisRaw("Vertical");
        float inputX = Input.GetAxisRaw("Horizontal");

        //Vector3 moveDir = new Vector3(0, 0, inputY).normalized;
        /*
        Vector3 moveDir = transform.forward;
        Vector3 targetMoveAmount = moveDir * walkSpeed * inputY;
        moveAmount = Vector3.SmoothDamp(moveAmount, targetMoveAmount, ref smoothMoveVelocity, .15f);
        */
        rotX = inputX * rotSpeed * Time.deltaTime;
        rotY = inputY * walkSpeed * Time.deltaTime;

        GameManager.instance.Planet.transform.Rotate(Vector3.Cross(this.transform.forward,this.transform.up), rotY, Space.World);
        
        

        anim.SetBool("isWalking", ((inputY !=0 ) ? true : false));
        // || (inputY != 0)
       
        // Jump
        if (Input.GetButtonDown("Jump"))
        {
            if (grounded)
            {
                rigidbody.AddForce(transform.up * jumpForce);
            }
        }

        // Grounded check
        Ray ray = new Ray(transform.position, -transform.up);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 1 + .1f, groundedMask))
        {
            grounded = true;
        }
        else
        {
            grounded = false;
        }
    }


    void FixedUpdate()
    {
        // Apply movement to rigidbody
        //Vector3 localMove = transform.TransformDirection(moveAmount) * Time.fixedDeltaTime;
        //rigidbody.MovePosition(rigidbody.position + localMove);
        transform.Rotate(0.0f, rotX, 0.0f, Space.Self);
    }
}

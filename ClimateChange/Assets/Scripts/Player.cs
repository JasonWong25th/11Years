using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using LowPolyAnimalPack;
using GH;

public class Player : MonoBehaviour
{

    //Copped from the Wander Script

    #region All the differnet States
    //Ideally we have a animation manager so we don't have to do it here?
    [Header("Animation States"), Space(5)]
    [SerializeField]
    private IdleState[] idleStates;
    [SerializeField]
    private MovementState[] movementStates;
    [SerializeField]
    private AnimalState[] attackingStates;
    [SerializeField]
    private AnimalState[] deathStates;
    //To see Gizmos
    [SerializeField, Tooltip("If true, gizmos will be drawn in the editor.")]
    private readonly bool showGizmos = true;

    #endregion
    #region Variables needed for calcuations
    private bool dead = false;
    //Acesss the animator
    private Animator animator;
    //Used to Calcuate the player's movement
    Vector3 moveAmount;
    Vector3 smoothMoveVelocity;
    //Access the NavMeshAgent on the Player
    private Rigidbody rb;
    //Bool to see if they're moving
    bool isMoving = false;
    //Timer variables
    public float m_hungerDelpetionTime = 0.6f;
    private float m_currentTime = 0.0f;
    #endregion


    [Header("Player Settings"), Space(5)]
    [SerializeField]
    private float rotSpeed = 80f;
    private float rotX = 0f;
    private float rotZ = 0f;

    [Header("Gameplay Stats"), Space(5)]
    [SerializeField]
    private float health = 100;
    [SerializeField]
    private float depletionCoEff = 0.1f;

    public void Awake()
    {
        //Checks if there is an animation added
        if (idleStates.Length == 0 && movementStates.Length == 0)
        {
            Debug.LogError(string.Format("{0} has no idle or movement states state.", gameObject.name));
            enabled = false;
            return;
        }
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        
        animator.applyRootMotion = false;

        EventSystem.instance.AddListener<MouseClickedData>(UponMouseClick);
        EventSystem.instance.AddListener<KeyboardPressed>(KeyboardInput);
        EventSystem.instance.AddListener<AnalogStick>(ControllerInput);
    }
    private void OnDisable()
    {
        EventSystem.instance.RemoveListener<MouseClickedData>(UponMouseClick);
        EventSystem.instance.RemoveListener<KeyboardPressed>(KeyboardInput);

    }

    void MoveAnimation(bool state)
    {
        if (!string.IsNullOrEmpty(movementStates[0].animationBool))
        {
            animator.SetBool(movementStates[0].animationBool, state);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!dead)
        {
            m_currentTime += Time.deltaTime;

            if (m_currentTime >= m_hungerDelpetionTime)
            {
                m_currentTime = 0.0f;
                depletionCoEff += 0.1f;
            }

            //
            //Decrease health
            if (health > 0)
            {
                health -= depletionCoEff * Time.deltaTime;
            }
            else
            {
                dead = true;
            }

            #region Movement based on Input
            //Also ideally we don't do the direct refernce 
            switch (GameManager.Instance.Platform)
            {
                case Platform.PC:
                    //Rotation Code

                    MoveAnimation(isMoving);
                    break;
                case Platform.Xbox:
                    //Do your code here Dante
                    break;
            }
            #endregion

        }
        else
        {
            Die();
        }

    }

    void KeyboardInput(KeyboardPressed keyboardPressedData)
    {
        //Rotation Code
        rotX = keyboardPressedData.horizontal * rotSpeed * Time.deltaTime;
        rotZ = keyboardPressedData.vertical * rotSpeed * Time.deltaTime;
    }

    void ControllerInput(AnalogStick analog)
    {
        rotX = analog.horizontal * rotSpeed * Time.deltaTime;
        rotZ = analog.vertical * rotSpeed * Time.deltaTime;
    }

    //Decouple attempt
    void UponMouseClick(MouseClickedData mouseClickedData)
    {
        if (mouseClickedData.clicked)
        {
            //In Game the player moves differnetly then expected
            Vector3 targetMoveAmount = this.transform.forward * movementStates[0].moveSpeed;

            /*if (InputManager.Instance.Rush)
            {
                targetMoveAmount *= 10;
            }
            */
            moveAmount = Vector3.SmoothDamp(moveAmount, targetMoveAmount, ref smoothMoveVelocity, 0.15f);
            //Animation
            isMoving = true;
        }
        else
        {
            moveAmount *= 0;
            isMoving = false;
        }
    }

    void PlayerRush(Rushing rushing)
    {
        if (rushing.rush)
        {
            moveAmount *= 10;
        }
    }

    void FixedUpdate()
    {
        // Apply movement to rigidbody
        Vector3 localMove = transform.TransformDirection(moveAmount) * Time.fixedDeltaTime;
        rb.MovePosition(transform.position + localMove);
        //Apply Rotation
        transform.Rotate(rotZ, rotX, 0.0f, Space.Self);
    }

    
    //Set animator to the Death State
    public void Die()
    {
        dead = true;
        foreach (AnimalState state in idleStates)
        {
            if (!string.IsNullOrEmpty(state.animationBool))
            {
                animator.SetBool(state.animationBool, false);
            }
        }

        foreach (AnimalState state in movementStates)
        {
            if (!string.IsNullOrEmpty(state.animationBool))
            {
                animator.SetBool(state.animationBool, false);
            }
        }

        foreach (AnimalState state in attackingStates)
        {
            if (!string.IsNullOrEmpty(state.animationBool))
            {
                animator.SetBool(state.animationBool, false);
            }
        }


        if (!string.IsNullOrEmpty(deathStates[0].animationBool))
        {
            animator.SetBool(deathStates[0].animationBool, true);
        }
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using LowPolyAnimalPack;
using GH;
using AmbientSounds;

public enum CausesOfDeath
{
    Deadzones,
    Hunger,
    Starvation,
    Fishnets,
    Eaten,
    Acidification
}

public class UpdatePlayerUI : GH.Event
{
    public float healhpoints;
    public float hungerpoints;
}
public class OnHealth : GH.Event
{
    public float deltaHealthAmt;
}
public class PlayerDie : GH.Event
{
    public CausesOfDeath causeOfDeath;
}
public class BeginRitualToEnterHabitat : GH.Event
{
    public bool onEnter;
}
public class PlayerPosition : GH.Event
{
    public Vector3 position;
}

public class Player : MonoBehaviour
{
    #region Copped from the Low Poly Animal Pack: Wander Script - All the differnet States
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

    //Player Components
    private Animator animator;
    private Rigidbody rb;
    public Vector3 centerOfMass;


    //Used to Calcuate the player's movement
    Vector3 input;
    public Vector3 steeringThrust;
    Vector3 forwardThrust;
    float forwardThrustAmount;

    //States
    bool isThrusting = false;
    bool isBoosting = false;
    [SerializeField]
    private bool isHiding = false;
    [SerializeField]
    private bool dead = false; // --> Also used for player & game state

    private bool isInControl = true;
    private bool isPauseOn = false;

    [Header("Player Settings"), Space(5)]
    //Speed Values
    public float speed = 4f;
    public float boostSpeed = 6f;
    public float maxSpeed = 2f;

    public float boostTime;
    float currBoostTime;
    public float boostLimit;
    float boostCount;
    public float boostCoolDown;
    public float currCoolTime;

    public float turnForceHorzintal = 3; // This is used in calculating player movement too
    public float turnForceVertical;
    public float rotMax;
    public float rotMin;

    [Header("Gameplay Stats"), Space(5)]
    //Player Life Variables
    [SerializeField]
    public float health = 100;
    [SerializeField]
    private float hunger = 100;

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
        centerOfMass = rb.centerOfMass;
        animator = GetComponent<Animator>();

        animator.applyRootMotion = false;

        EventSystem.instance.AddListener<MouseClickedData>(UponMouseClick);
        EventSystem.instance.AddListener<KeyboardPressed>(KeyboardInput);
        EventSystem.instance.AddListener<Rushing>(PlayerRush);
        EventSystem.instance.AddListener<Eat>(Attack);
        EventSystem.instance.AddListener<PlayerDie>(Die);
        EventSystem.instance.AddListener<OnHealth>(UpdateHealth);
        EventSystem.instance.AddListener<CollectableEvent>(GotCollectable);
        EventSystem.instance.AddListener<OnDismissCollectable>(DismissCollectable);

        EventSystem.instance.AddListener<PauseGame>(PauseControl);

    }
    private void OnDisable()
    {
        EventSystem.instance.RemoveListener<MouseClickedData>(UponMouseClick);
        EventSystem.instance.RemoveListener<KeyboardPressed>(KeyboardInput);
        EventSystem.instance.RemoveListener<Rushing>(PlayerRush);
        EventSystem.instance.RemoveListener<Eat>(Attack);
        EventSystem.instance.RemoveListener<PlayerDie>(Die);
        EventSystem.instance.RemoveListener<OnHealth>(UpdateHealth);
        EventSystem.instance.RemoveListener<CollectableEvent>(GotCollectable);
        EventSystem.instance.RemoveListener<OnDismissCollectable>(DismissCollectable);

        EventSystem.instance.RemoveListener<PauseGame>(PauseControl);
    }

    void MoveAnimation(bool state)
    {
        if (!string.IsNullOrEmpty(movementStates[0].animationBool))
        {
            animator.SetBool(movementStates[0].animationBool, state);

            if(animator.GetBool(movementStates[0].animationBool))
            {
                //Debug.Log("Should be moving");
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        EventSystem.instance.RaiseEvent(new PlayerPosition { position = this.transform.position });
        EventSystem.instance.RaiseEvent(new UpdatePlayerUI { healhpoints = health, hungerpoints = hunger });
        DeathCheck();
        
    }

    //Is called multiple time per frame, use for physics
    void FixedUpdate()
    {
//        Debug.Log(transform.rotation.eulerAngles.x);
        if (isInControl && isPauseOn == false)
        {
            if (!dead)
            {
                // Restricts player movement so only on input
                if (isThrusting)
                {
                    //Makes sure to update forwardThrustAmount before applying
                    Boost();
                    forwardThrust = Vector3.forward * forwardThrustAmount;

                    rb.AddRelativeForce(forwardThrust, ForceMode.Force);
                }

                Turn();
            }
        }

    }

    void OnDrawGizmos()
    {
        // Draw a yellow sphere at the transform's position
        //Gizmos.color = Color.yellow;
        //Gizmos.DrawSphere( transform.rotation * transform.position + centerOfMass, 7f);
    }

    #region Input Functions
    void KeyboardInput(KeyboardPressed keyboardPressedData)
    {
        if (isInControl && isPauseOn == false)
        {
            if (!dead && !GameManager.Instance.pause)
            {
                input.Set(
                 keyboardPressedData.horizontal,
                 keyboardPressedData.vertical,
                 0.0f
                );

                steeringThrust = input.normalized;
                steeringThrust.x *= turnForceHorzintal;
                steeringThrust.y *= turnForceVertical;
            }
        }

    }

    void PlayerRush(Rushing rushing)
    {
        if (rushing.rush && (boostCount < boostLimit))
        {
            isBoosting = true;
            boostCount++;
        }
    }

    void UponMouseClick(MouseClickedData mouseClickedData)
    {
        if (isInControl && isPauseOn == false)
        {
            if (mouseClickedData.clicked && !dead && !GameManager.Instance.pause)
            {

                if (!isBoosting)
                {
                    forwardThrustAmount = speed;
                }
                else
                {
                    forwardThrustAmount = boostSpeed;
                }
                //Animation
                isThrusting = true;
            }
            else
            {
                forwardThrustAmount = 0;
                isThrusting = false;
            }
        }

    }
    #endregion

    #region Player Action Functions
    void Boost()
    {
        if (isBoosting)
        {
            if (currBoostTime <= boostTime)
            {
                currBoostTime += Time.fixedDeltaTime;
            }
            else
            {
                currBoostTime = 0f;
                isBoosting = false;
            }
        }

        if (boostCount >= boostLimit && currBoostTime == 0f)
        {
            if (currCoolTime <= boostCoolDown)
            {
                currCoolTime += Time.fixedDeltaTime;
            }
            else
            {
                currCoolTime = 0f;
                boostCount = 0f;
            }
        }
    }

    void Turn()
    {
        // Need to convert steeringThrust into a force to be applied to in terms of world space
        Vector3 localRot = (steeringThrust / rb.mass) * Time.deltaTime;
        Vector3 worldRot = transform.localToWorldMatrix.MultiplyVector(localRot);

        //float angleChange = 0f;
        Vector3 lookVector = Vector3.Normalize(transform.forward + worldRot);
        if (lookVector.y >= 0.8f)
        {
            lookVector.y = 0.8f;
        }
        if(lookVector.y <= -.8f)
        {
            lookVector.y = -0.8f;
        }

        Quaternion targetRot = Quaternion.LookRotation(lookVector);
        //if (transform.rotation.x >= 40  || transform.rotation.x <= -80)
        //{
        //    Debug.Log("EXCEEDING");
        //    //Vector3 TargetEulers = targetRot.eulerAngles;
        //    //targetRot = Quaternion.Euler(Mathf.Clamp(TargetEulers.x, rotMin, rotMax), TargetEulers.y, TargetEulers.z);
        //    //

        //    //get axis and angle of target rotation
        //    //
        //}

        Quaternion currentRot = rb.rotation;
        float angleDelta = Quaternion.Angle(currentRot, targetRot);

        if (angleDelta > 0f)
        {
            //float damp = Mathf.SmoothDampAngle(angleDelta, 0f, ref angleChange, .1f); //This float scales rotation speed

            //damp = 1f - (damp / angleDelta);
            //rb.MoveRotation(Quaternion.Slerp(currentRot, targetRot, damp));
            rb.MoveRotation(targetRot);
        }
    }

    void Attack(Eat eat)
    {
        if (eat.eat)
        {
            foreach (AnimalState state in attackingStates)
            {
                if (!string.IsNullOrEmpty(state.animationBool))
                {
                    animator.SetBool(state.animationBool, true);
                }
            }
        }
        else
        {
            foreach (AnimalState state in attackingStates)
            {
                if (!string.IsNullOrEmpty(state.animationBool))
                {
                    animator.SetBool(state.animationBool, false);
                }
            }
        }

    }

    public void Die(PlayerDie playerDie)
    {
        isInControl = false;
        dead = true;
        //have to make sure player movement is no existent
        rb.velocity = Vector3.zero;

        //have to disable all player animations other than death
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

        //Have to collect death data to trigger specific death sequence
        switch (playerDie.causeOfDeath)
        {
            case CausesOfDeath.Acidification:
                EventSystem.instance.RaiseEvent(new DeathUI() {
                    cause = CausesOfDeath.Acidification
                });
                break;
            case CausesOfDeath.Deadzones:
                EventSystem.instance.RaiseEvent(new DeathUI() {  cause = CausesOfDeath.Deadzones });
                break;
            case CausesOfDeath.Eaten:
                EventSystem.instance.RaiseEvent(new DeathUI() { cause = CausesOfDeath.Eaten });
                break;
            case CausesOfDeath.Fishnets:
                EventSystem.instance.RaiseEvent(new DeathUI() {  cause = CausesOfDeath.Fishnets });
                break;
            case CausesOfDeath.Starvation:
                EventSystem.instance.RaiseEvent(new DeathUI() { cause = CausesOfDeath.Starvation });
                break;
            default:
                break;
        }
        AmbienceManager.ActivateEvent("LowerBackground");

    }
    #endregion

    public void UpdateHealth(OnHealth onHealth)
    {
        health += onHealth.deltaHealthAmt;
    }

    void DeathCheck() {
        if (health <= 0 || Input.GetKey(KeyCode.X))
        {
            dead = true;
        }

        if (!dead)
        {
            MoveAnimation(isThrusting);
        }
        else
        {
            EventSystem.instance.RaiseEvent(new PlayerDie() { causeOfDeath = CausesOfDeath.Acidification});
        }
    }
    void GotCollectable(CollectableEvent collectable)
    {
        isInControl = false;
    }
    void DismissCollectable(OnDismissCollectable onDismiss)
    {
        isInControl = true;
    }
    void PauseControl(PauseGame pauseGame)
    {
        isPauseOn = pauseGame.pause;
    }
}

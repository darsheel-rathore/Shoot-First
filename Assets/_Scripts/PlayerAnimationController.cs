using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    #region Vars

    // for caching animator and movement script component
    private Animator animator;
    private PlayerMovement movementScript;
    private PlayerFiring playerFiringScript;

    [Header("IK Targets and Values")]
    [SerializeField] private Transform leftHandTarget;
    [SerializeField] private Transform leftHandIKHintTarget;
    [SerializeField] [Range(0, 1)] private int ikWeight;
    [SerializeField] [Range(0, 1)] private int ikHintWeight;

    // for caching control inputs
    private DynamicJoystick controlStick, rotationStick;
    
    // for storing motion and rotation vectors
    private Vector2 motionVector;
    private Vector2 rotationVector;

    // for strafing animation
    private bool isPlayerStrafing;
    private Vector2 strafingVector;
    public float strafeSpeed;

    public AudioSource footStepAudioSource;

    #endregion

    // ======================================================

    #region Unity Methods


    private void OnEnable()
    {
        // subscribing to rotation control touch
        FireButtonManager.OnTouched += InitiateStrafing;
        FireButtonManager.OnTouchRemoved += StopStrafing;
        FireButtonManager.OnTouched += StartFireAnimation;
        FireButtonManager.OnTouchRemoved += StopFireAnimation;
    }

    private void OnDisable()
    {
        // unsubscribing to rotation control touch
        FireButtonManager.OnTouchRemoved -= StopStrafing;
        FireButtonManager.OnTouched -= InitiateStrafing;
        FireButtonManager.OnTouched -= StartFireAnimation;
        FireButtonManager.OnTouchRemoved -= StopFireAnimation;
    }

    void Start()
    {
        // caching
        animator = GetComponentInChildren<Animator>();
        movementScript = GetComponentInParent<PlayerMovement>();
        playerFiringScript = GetComponentInParent<PlayerFiring>();

        // caching control and rotation sticks
        controlStick = movementScript.GetSticks()[0];
        rotationStick = movementScript.GetSticks()[1];
    }

    void Update()
    {
        // recieving inputs and creating movement animations
        ReceiveInputs();
        PlayerMoveAnimation();
        CalculateAngles();

        // play strafe only when player isStrafing is true
        if (isPlayerStrafing)
        {
            PlayStrafeAnimation();
        }
    }


    #endregion

    // ======================================================

    #region Private Methods

    // to position the left hand correctly
    private void OnAnimatorIK(int layerIndex)
    {
        animator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandTarget.position);
        animator.SetIKRotation(AvatarIKGoal.LeftHand, leftHandTarget.rotation);

        animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, ikWeight);
        animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, ikWeight);

        animator.SetIKHintPosition(AvatarIKHint.LeftElbow, leftHandIKHintTarget.position);
        animator.SetIKHintPositionWeight(AvatarIKHint.LeftElbow, ikHintWeight);
    }

    // play player move animation
    private void PlayerMoveAnimation()
    {
        animator.SetFloat("Speed", motionVector.magnitude);
    }

    // recieving inputs from control and rotation vectors
    private void ReceiveInputs()
    {
        motionVector = new Vector2(controlStick.Horizontal, controlStick.Vertical);
        rotationVector = new Vector2(rotationStick.Horizontal, rotationStick.Vertical);
    }

    // calculate angles between the control and rotation vectors
    private void CalculateAngles()
    {
        // calculating angle between direction of movement and direction the player avatar is facing
        float angle = Vector2.SignedAngle(rotationVector, motionVector);
        strafingVector = Quaternion.AngleAxis(angle, Vector3.forward) * new Vector2(0, 1);
        
        if (motionVector.magnitude <= 0.01f)
            strafingVector = Vector2.zero;
    }

    // play player strafing animation by putting values to respective variables
    private void PlayStrafeAnimation()
    {
        animator.SetFloat("VelocityX", strafingVector.x);
        animator.SetFloat("VelocityZ", strafingVector.y);
    }

    // start strafing || by event
    private void InitiateStrafing()
    {
        // set strafing variables in animator, set strafing speed
        isPlayerStrafing = true;
        animator.SetTrigger("strafing");
        animator.SetBool("isStrafing", true);
        movementScript.SetStrafingSpeed(strafeSpeed);
    }   
    
    // stop strafing || by event
    private void StopStrafing()
    {
        // change animation state by setting animator strafing var to false, reset movement speed
        isPlayerStrafing = false;
        animator.SetBool("isStrafing", false);
        movementScript.SetStrafingSpeed(5);
    }


    private void StartFireAnimation()
    {
        InvokeRepeating("FiringAnimation", 0f, movementScript.GetFireRate());
    }

    private void StopFireAnimation()
    {
        CancelInvoke("FiringAnimation");
    }

    private void FiringAnimation()
    {
        animator.SetTrigger("Fire");
    }

    private void FireProjectile()
    {
        playerFiringScript.Fire();
    }

    public void PlayFootSteps()
    {
        footStepAudioSource.Play();
    }

    #endregion


}

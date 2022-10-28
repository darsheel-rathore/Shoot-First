using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    [SerializeField] Animator animator;

    PlayerInputManager inputManager;
    Movement movement;

    private Vector3 movementVector;
    private Vector3 rotationVector;

    // animator hashes
    private int zVelocityHash = 0;
    private int xVelocityHash = 0;
    private int isStrafingHash = 0;
    private int isFiringHash = 0;
    private int fireTriggerHash = 0;


    private bool isStrafing = false;
    private Vector2 strafingVector;
    private Vector2 dirLeft, dirRight;
    private float signedAngle = 0f;
    private Vector2 referenceVector = new Vector2(0, 1);

    private bool isFireButtonPressed = false;
    private bool isFireTriggered = false;


    // ===========================================


    #region Unity Methods

    private void OnEnable()
    {
        FireButton.OnPressed += FireButtonPressed;
        FireButton.OnLift += FireButtonReleased;
    }

    private void OnDisable()
    {
        FireButton.OnPressed -= FireButtonPressed;
        FireButton.OnLift -= FireButtonReleased;
    }

    void Start()
    {
        inputManager = GetComponent<PlayerInputManager>();
        movement = GetComponent<Movement>();

        CreateAnimatorHashes();
    }

    private void Update()
    {
        CollectInputs();

        SetStrafingAndStrafingSpeed();

        HandlePlayerAnimation();
        
        HandleFireAnimation();
    }

    // prevents running the fire animation 
    // by only tapping the fire button and not moving
    // the fire button
    private void HandleFireAnimation()
    {
        if (isFireButtonPressed)
        {
            if (rotationVector.magnitude <= 0.1f) return;
            if (isFireTriggered) return;

            animator.SetBool(isFiringHash, true);
            animator.SetTrigger(fireTriggerHash);
            isFireTriggered = true;
        }
        else
        {
            animator.SetBool(isFiringHash, false);
            isFireTriggered = false;
        }
    }

    #endregion


    // ===========================================


    #region Private Methods


    private void HandlePlayerAnimation()
    {
        if (isStrafing)
        {
            CalculateStrafingVector();
            PerformStrafingAnimation();
        }
        else
        {
            PerformNonStrafingAnimation();
        }
    }

    private void SetStrafingAndStrafingSpeed()
    {
        isStrafing = movementVector != rotationVector;
        movement.SetStrafingSpeed(isStrafing);
    }

    private void CollectInputs()
    {
        movementVector = inputManager.GetMovementVector();
        rotationVector = inputManager.GetRotationVector();
    }

    private void PerformNonStrafingAnimation()
    {
        animator.SetBool(isStrafingHash, isStrafing);
        animator.SetFloat(zVelocityHash, inputManager.GetMovementVector().magnitude);
    }

    private void PerformStrafingAnimation()
    {
        animator.SetBool(isStrafingHash, isStrafing);
        animator.SetFloat(xVelocityHash, strafingVector.x);
        animator.SetFloat(zVelocityHash, strafingVector.y);
    }

    #endregion


    // ===========================================


    #region Event Methods

    private void FireButtonPressed()
    {
        isFireButtonPressed = true;
    }

    private void FireButtonReleased()
    {
        isFireButtonPressed = false;
    }

    #endregion


    // ===========================================


    #region Helpers

    private void CalculateStrafingVector()
    {
        dirLeft = new Vector2(movementVector.x, movementVector.z);
        dirRight = new Vector2(rotationVector.x, rotationVector.z);

        signedAngle = Vector2.SignedAngle(dirRight, dirLeft);
        strafingVector = Quaternion.AngleAxis(signedAngle, Vector3.forward) * referenceVector;

        if (movementVector.magnitude <= 0.01f)
            strafingVector = Vector2.zero;
    }


    private void CreateAnimatorHashes()
    {
        zVelocityHash = Animator.StringToHash("VelocityZ");
        xVelocityHash = Animator.StringToHash("VelocityX");
        isStrafingHash = Animator.StringToHash("isStrafing");
        isFiringHash = Animator.StringToHash("isFiring");
        fireTriggerHash = Animator.StringToHash("fireTrigger");
    }

    #endregion


    // ===========================================



    #region Public Methods

    public bool GetIsFiringTriggered() => isFireTriggered;

    #endregion

}

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


    // ===========================================


    #region Unity Methods

    private void OnEnable()
    {
        FireButton.OnPressed += FireAnimationPlay;
        FireButton.OnLift += FireAnimationStop;
    }

    private void OnDisable()
    {
        FireButton.OnPressed -= FireAnimationPlay;
        FireButton.OnLift -= FireAnimationStop;
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

    private void FireAnimationPlay()
    {
        animator.SetBool(isFiringHash, true);
        animator.SetTrigger(fireTriggerHash);
    }

    private void FireAnimationStop()
    {
        animator.SetBool(isFiringHash, false);
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




}

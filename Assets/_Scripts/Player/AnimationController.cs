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


    private bool isStrafing = false;
    private Vector2 strafingVector;
    private Vector2 dirLeft, dirRight;
    private float signedAngle = 0f;
    private Vector2 referenceVector = new Vector2(0, 1);


    void Start()
    {
        inputManager = GetComponent<PlayerInputManager>();
        movement = GetComponent<Movement>();

        zVelocityHash = Animator.StringToHash("VelocityZ");
        xVelocityHash = Animator.StringToHash("VelocityX");
        isStrafingHash = Animator.StringToHash("isStrafing");
    }

    private void Update()
    {
        movementVector = inputManager.GetMovementVector();
        rotationVector = inputManager.GetRotationVector();

        isStrafing = movementVector != rotationVector;
        movement.SetStrafingSpeed(isStrafing);

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

    private void CalculateStrafingVector()
    {
        dirLeft = new Vector2(movementVector.x, movementVector.z);
        dirRight = new Vector2(rotationVector.x, rotationVector.z);

        signedAngle = Vector2.SignedAngle(dirRight, dirLeft);
        strafingVector = Quaternion.AngleAxis(signedAngle, Vector3.forward) * referenceVector;

        if (movementVector.magnitude <= 0.01f)
            strafingVector = Vector2.zero;
    }

}

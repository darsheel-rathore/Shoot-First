using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [Range(0, 1f)]
    [SerializeField] private float strafingMultiplier = 0.5f;

    [SerializeField] private float movementSpeed;
    [SerializeField] private float rotationSpeed;

    private Rigidbody rb;
    private PlayerInputManager inputManager;
    private Transform cameraTransform;

    Vector3 movementVector;
    Vector3 rotationVector;
    Quaternion lookRotation;


    // ======================================

    #region Unity Methods

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        inputManager = GetComponent<PlayerInputManager>();
        cameraTransform = Camera.main.transform;
    }

    private void FixedUpdate()
    {
        HandlePlayerMovement();
        HandlePlayerRotation();
    }

    #endregion

    // ======================================


    #region Private Methods


    private void HandlePlayerMovement()
    {
        movementVector = inputManager.GetMovementVector();
        if (movementVector.magnitude >= 1)
            movementVector.Normalize();

        movementVector = movementVector * movementSpeed * Time.fixedDeltaTime;
        movementVector = Quaternion.Euler(0f, cameraTransform.eulerAngles.y, 0) * movementVector;

        rb.MovePosition(transform.position + movementVector);
    }


    private void HandlePlayerRotation()
    {
        rotationVector = inputManager.GetRotationVector();
        if (rotationVector.magnitude <= 0.1f) return;

        rotationVector = Quaternion.Euler(0f, cameraTransform.eulerAngles.y, 0f) * rotationVector;
        lookRotation = Quaternion.LookRotation(rotationVector);
        
        transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, rotationSpeed);
    }


    #endregion


    // ======================================



    #region Public Methods

    public void SetStrafingSpeed(bool isStrafing) =>
        movementSpeed = (isStrafing) ? 2.5f : 5f;

    #endregion
}

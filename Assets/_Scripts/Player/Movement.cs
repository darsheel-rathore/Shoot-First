using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private float strafingSpeed = 2.5f;
    [SerializeField] private float normalMovementSpeed = 5f;
    [SerializeField] private float rotationSpeed;

    private Rigidbody rb;
    private PlayerInputManager inputManager;
    private Transform cameraTransform;

    float movementSpeed;
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
        rotationVector = (rotationVector == Vector3.zero) ? inputManager.GetMovementVector() : rotationVector;

        if (rotationVector.magnitude <= 0.01f) return;

        rotationVector = Quaternion.Euler(0f, cameraTransform.eulerAngles.y, 0f) * rotationVector;
        lookRotation = Quaternion.LookRotation(rotationVector);
        
        transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, rotationSpeed);
    }


    #endregion


    // ======================================



    #region Public Methods

    public void SetStrafingSpeed(bool isStrafing) =>
        movementSpeed = (isStrafing) ? strafingSpeed : normalMovementSpeed;

    #endregion
}

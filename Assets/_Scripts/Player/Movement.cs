using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private float movementSpeed;
    [SerializeField] private float rotationSpeed;

    private Rigidbody rb;
    private Transform cameraTransform;


    float inputHorizontal = 0f;
    float inputVertical = 0f;

    Vector3 movementVector;
    Vector3 rotationVector;
    Quaternion lookRotation;


    // ======================================

    #region Unity Methods

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        cameraTransform = Camera.main.transform;
    }

    private void Update()
    {
        HandleInputs();
    }

    private void FixedUpdate()
    {
        HandlePlayerMovement();
        HandlePlayerRotation();
    }

    #endregion

    // ======================================


    #region Private Methods

    private void HandleInputs()
    {
        inputHorizontal = Input.GetAxis("Horizontal");
        inputVertical = Input.GetAxis("Vertical");
    }

    private void HandlePlayerMovement()
    {
        movementVector = new Vector3(inputHorizontal, 0, inputVertical);
        if (movementVector.magnitude >= 1)
            movementVector.Normalize();

        movementVector = movementVector * movementSpeed * Time.fixedDeltaTime;
        movementVector = Quaternion.Euler(0f, cameraTransform.eulerAngles.y, 0) * movementVector;


        rb.MovePosition(transform.position + movementVector);
    }

    private void HandlePlayerRotation()
    {
        rotationVector = new Vector3(inputHorizontal, 0, inputVertical);
        if (rotationVector.magnitude <= 0.1f) return;

        rotationVector = Quaternion.Euler(0f, cameraTransform.eulerAngles.y, 0f) * rotationVector;
        lookRotation = Quaternion.LookRotation(rotationVector);
        
        transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, rotationSpeed);
    }

    #endregion


    // ======================================

}

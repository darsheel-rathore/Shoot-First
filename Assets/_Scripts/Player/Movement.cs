using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private float movementSpeed;

    private Rigidbody rigidbody;


    float inputHorizontal = 0f;
    float inputVertical = 0f;

    Vector3 movementVector;

    // ======================================

    #region Unity Methods

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        HandleInputs();
    }

    private void FixedUpdate()
    {
        HandlePlayerMovement();
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

        rigidbody.MovePosition(transform.position + movementVector);
    }

    #endregion

    // ======================================

}

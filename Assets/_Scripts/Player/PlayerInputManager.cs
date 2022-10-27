using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputManager : MonoBehaviour
{
    [SerializeField] private DynamicJoystick movementStick;
    [SerializeField] private FixedJoystick rotationStick;

    private Vector3 movementVector;
    private Vector3 rotationVector;

    private void Update()
    {
        TakePlayerInput();
    }

    private void TakePlayerInput()
    {
        movementVector = new Vector3(
            Input.GetAxis("Horizontal"),
            0f,
            Input.GetAxis("Vertical")
        );

        rotationVector = new Vector3(
            rotationStick.Horizontal,
            0f,
            rotationStick.Vertical
        );


        // just to use keybord for moving input
        if (movementStick.Direction != Vector2.zero)
        {
            movementVector = new Vector3(
                movementStick.Horizontal,
                0f,
                movementStick.Vertical
            );
        }
        
        if (rotationVector == Vector3.zero) rotationVector = movementVector;
    }

    public Vector3 GetMovementVector() => movementVector;
    public Vector3 GetRotationVector() => rotationVector;
}
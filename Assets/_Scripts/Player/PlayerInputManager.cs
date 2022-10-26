using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputManager : MonoBehaviour
{
    [SerializeField] private DynamicJoystick movementStick;
    [SerializeField] private DynamicJoystick fireStick;

    private Vector3 movementVector;
    
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
    }

    public Vector3 GetMovementVector() => movementVector;
}

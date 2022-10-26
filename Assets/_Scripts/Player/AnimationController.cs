using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] DynamicJoystick controlStick;

    PlayerInputManager inputManager;


    private Vector3 movementVector;
    private bool isStrafing = false;
    private int zVelocity = 0;

    void Start()
    {
        inputManager = GetComponent<PlayerInputManager>();
        zVelocity = Animator.StringToHash("VelocityZ");
    }

    private void Update()
    {
        animator.SetFloat(zVelocity, inputManager.GetMovementVector().magnitude);
    }

}

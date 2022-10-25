using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    #region Variables

    // input variables
    private float inMovementH;
    private float inMovementF;
    private float inRotationH;
    private float inRotationF;

    // movement, physics and rotation
    private Rigidbody rigidBody;
    private Vector3 movementVector;
    private Vector3 rotationVector;
    private Transform mainCameraTransform;
    private Quaternion lookRotation;

    [Header("Player Control\n")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private bool isMobileInput;
    [SerializeField] private float fireRate;
    [SerializeField] private DynamicJoystick playerControlJoystic;
    [SerializeField] private DynamicJoystick playerRotationJoystic;

    #endregion


    // =============================================


    #region Unity Methods

    // Start is called before the first frame update
    void Start()
    {
        // caching
        rigidBody = GetComponent<Rigidbody>();
        mainCameraTransform = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        // collect input from the player
        TakeMovementInput();
        TakeRotationInput();
    }

    private void FixedUpdate()
    {
        HandlePlayerMovement();
        HandlePlayerRotation();
    }

    #endregion


    // =============================================


    #region Private Methods

    private void TakeMovementInput()
    {
        if (!isMobileInput)
        {
            inMovementH = Input.GetAxis("Horizontal");
            inMovementF = Input.GetAxis("Vertical");
        }
        else
        {
            inMovementH = playerControlJoystic.Horizontal;
            inMovementF = playerControlJoystic.Vertical;
        }
    }

    private void TakeRotationInput()
    {
        inRotationH = playerRotationJoystic.Horizontal;
        inRotationF = playerRotationJoystic.Vertical;
    }

    private void HandlePlayerMovement()
    {
        movementVector = new Vector3(inMovementH, 0, inMovementF);

        // normalize only when the input magnitude is more than 1
        if (movementVector.magnitude >= 1)
            movementVector.Normalize();

        movementVector = movementVector * moveSpeed * Time.fixedDeltaTime;

        // manipulating movement vector with camera angle to move relative to the camera
        movementVector = Quaternion.Euler(0f, mainCameraTransform.eulerAngles.y, 0) * movementVector;

        // update position with rigidbody
        rigidBody.MovePosition(transform.position + movementVector);
    }

    private void HandlePlayerRotation()
    {
        // taking input from movement for rotataion if the rotation vector is null
        inRotationH = (inRotationH == 0 ? inMovementH : inRotationH);
        inRotationF = (inRotationF == 0 ? inMovementF : inRotationF);

        // receive input and create a rotation vector with camera manipulation
        rotationVector = new Vector3(inRotationH, 0, inRotationF);

        // return if input is close to 0
        if (rotationVector.magnitude <= 0.01f) return;

        rotationVector = Quaternion.Euler(0f, mainCameraTransform.eulerAngles.y, 0f) * rotationVector;

        // creating a look rotation as the rotate towards method needs it
        lookRotation = Quaternion.LookRotation(rotationVector);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, rotationSpeed);
    }

    #endregion


    // =============================================


    #region Public Methods

    // getters
    public DynamicJoystick[] GetSticks()
    {
        DynamicJoystick[] sticks = new DynamicJoystick[2];
        sticks[0] = playerControlJoystic;
        sticks[1] = playerRotationJoystic;
        return sticks;
    }

    public float GetFireRate()
    {
        return fireRate;
    }

    // setter for speed
    public void SetStrafingSpeed(float strafingSpeed)
    {
        moveSpeed = strafingSpeed;
    }


    #endregion

}

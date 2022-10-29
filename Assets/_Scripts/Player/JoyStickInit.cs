using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoyStickInit : MonoBehaviour
{
    [SerializeField] DynamicJoystick movementStick;
    [SerializeField] DynamicJoystick rotationStick;

    public static JoyStickInit Instance;

    private void Awake() 
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public (DynamicJoystick movementStick, DynamicJoystick rotationStick) GetJoySticks()
        => (movementStick, rotationStick);
}

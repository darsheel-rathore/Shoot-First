using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI angleText;
    [SerializeField] FloatingJoystick stickLeft, stickRight;
    
    Camera cam;

    void Start()
    {
        cam = Camera.main;

        float x = cam.transform.eulerAngles.x;
        float y = cam.transform.eulerAngles.y;
        float z = cam.transform.eulerAngles.z;
        transform.LookAt(Quaternion.Euler(x, y, z) * transform.forward);
    }

    void Update()
    {
        Vector2 dirLeft = new Vector2(stickLeft.Horizontal, stickLeft.Vertical);
        Vector2 dirRight = new Vector2(stickRight.Horizontal, stickRight.Vertical);

        float signedAngle = Vector2.SignedAngle(dirRight, dirLeft);
        float signedAngle2 = Vector2.SignedAngle(dirLeft, dirRight);
        float unSignedAngle = Vector2.Angle(dirLeft, dirRight);
        Vector2 result = Quaternion.AngleAxis(signedAngle, Vector3.forward) * new Vector2(0, 1);

        angleText.text = $"Signed: {signedAngle} | {signedAngle2} || Un-Signed: {unSignedAngle} || Result: {result}";
    }
}

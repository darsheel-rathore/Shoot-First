using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{

    Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;

        float x = cam.transform.eulerAngles.x;
        float y = cam.transform.eulerAngles.y;
        float z = cam.transform.eulerAngles.z;
        transform.LookAt(Quaternion.Euler(x, y, z) * transform.forward);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIDirectionControl : MonoBehaviour
{
    Quaternion initialRotation;
    bool useRelativeRotation = true;

    private void Start()
    {
        initialRotation = transform.rotation;
    }

    private void Update()
    {
        if (useRelativeRotation)
            transform.rotation = initialRotation;
    }

}

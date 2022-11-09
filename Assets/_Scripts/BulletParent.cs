using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletParent : MonoBehaviour
{
    public int bulletCount;

    private void Update()
    {
        bulletCount = transform.childCount;
    }
}

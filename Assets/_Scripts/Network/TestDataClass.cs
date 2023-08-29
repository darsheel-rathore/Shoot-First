using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TestDataClass
{
    [SerializeField] private int a;
    [SerializeField] private List<GameObject> testObjectList;
    [SerializeField] private SomeData[] someDataTestArray;
}

[Serializable]
public class SomeData
{
    [SerializeField] private List<string> randomList;
}

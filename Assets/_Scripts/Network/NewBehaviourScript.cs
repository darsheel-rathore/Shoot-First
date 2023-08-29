using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewBehaviourScript : MonoBehaviour
{
    public Button testBtn;

    [SerializeField]
    private TestDataClass[] testObject;

    // Start is called before the first frame update
    void Start()
    {
        testBtn.onClick.AddListener(() => Debug.Log("Hello World"));
        testBtn.onClick.AddListener(TestMethod);
    }

    private void TestMethod()
    {
        Debug.Log("Testing code for listeners");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

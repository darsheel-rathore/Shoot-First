using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CanvasControl : MonoBehaviourPunCallbacks
{

    [SerializeField] TextMeshProUGUI h1, h2, h3;

    public PlayerMove p1, p2, p3;

    public static CanvasControl Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);
        }
        Instance = this;
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log(p1);
            Debug.Log(p2);
            Debug.Log(p3);
        }

        if (p1 != null)
            h1.text = $"P: {p1.GetHealth()}";
    
        if (p2 != null)
            h2.text = $"P: {p2.GetHealth()}";

        if (p3 != null)
            h3.text = $"P: {p3.GetHealth()}";
    }

}

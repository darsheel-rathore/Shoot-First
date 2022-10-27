using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FireButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{

    public delegate void OnDirectionPressed();
    public static event OnDirectionPressed OnPressed;

    public delegate void OnDirectionLift();
    public static event OnDirectionLift OnLift;

    public void OnPointerDown(PointerEventData eventData)
    {
        OnPressed();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        OnLift();
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FireButtonManager : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public delegate void OnDirectionTouched();
    public static event OnDirectionTouched OnTouched;

    public delegate void OnDirectionUntouched();
    public static event OnDirectionUntouched OnTouchRemoved;

    public void OnPointerDown(PointerEventData eventData)
    {
        OnTouched();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        OnTouchRemoved();
    }
}

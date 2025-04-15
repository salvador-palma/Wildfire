using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;
using UnityEngine.EventSystems;

public class Clickable : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerClick(PointerEventData eventData) {
        Debug.Log("CLICK!");
    }

    public void OnPointerEnter(PointerEventData eventData) {
        Debug.Log("ENTER!");
    }

    public void OnPointerExit(PointerEventData eventData) {
        Debug.Log("EXIT!");
    }
}

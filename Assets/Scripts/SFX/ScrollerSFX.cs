using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using FMODUnity;
using UnityEngine;
using UnityEngine.UI;

public class ScrollerSFX : MonoBehaviour
{
    public EventReference SFX;
    public RectTransform Content;
    public float DistanceForSound = 0.2f;
    public bool isHorizontal = true;
    public float prev;
    public void ScrollSFX(){

        if (Mathf.Abs(prev - (isHorizontal ?Content.anchoredPosition.x : Content.anchoredPosition.y)) > DistanceForSound){
            prev = isHorizontal ? Content.anchoredPosition.x : Content.anchoredPosition.y;
             AudioManager.PlayOneShot(SFX, UnityEngine.Vector2.zero);
        }
       
    }
}

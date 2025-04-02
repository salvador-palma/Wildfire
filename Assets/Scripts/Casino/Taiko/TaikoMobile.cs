using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaikoMobile : MonoBehaviour
{
    public Taiko taiko;
    public int type;
    protected virtual void OnMouseDown() {
        
        taiko.Hit(type);
    }
}

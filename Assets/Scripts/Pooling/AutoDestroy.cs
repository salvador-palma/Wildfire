using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    [SerializeField]private float Timer;
    [SerializeField]private bool isOn;
    private void Update() {
        if(isOn){
            Timer-=Time.deltaTime;
            if(Timer <= 0){
                Destroy(gameObject);
            }
        }
    }
    public void StartCD(float n ){
        Timer = n;
        isOn = true;
    }

    public void SelfDestroy(){
        Destroy(gameObject);
    }
    public void SelfDestroyTextDMG(){
        DamageUI.DestroyTxtDmg(gameObject);
    }
}

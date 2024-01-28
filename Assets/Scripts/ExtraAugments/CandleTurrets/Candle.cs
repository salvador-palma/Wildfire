using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Candle : MonoBehaviour
{
    
    float atkSpeedTimer;
    [SerializeField] private GameObject candleFlare;
    void UpdateStats(){

    }

    private void Update() {
        if(Flamey.Instance.current_homing == null){return;}
        if(atkSpeedTimer <= 0){
            atkSpeedTimer = 1/CandleTurrets.Instance.atkSpeed;
            Instantiate(candleFlare).GetComponent<CandleFlare>().setPosition(transform.position);

        }else{
            atkSpeedTimer-=Time.deltaTime;
        }
    }
}

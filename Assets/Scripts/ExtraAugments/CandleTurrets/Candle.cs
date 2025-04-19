using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Candle : MonoBehaviour
{
    
    float atkSpeedTimer;
    [SerializeField] private IPoolable candleFlare;
    

    private void Update() {
        if(Flamey.Instance.current_homing == null){return;}
        if(atkSpeedTimer <= 0){
            atkSpeedTimer = 1/CandleTurrets.Instance.atkSpeed;

            if(Flamey.Instance.stunTimeLeft > 0 &&  SkillTreeManager.Instance.getLevel("Ritual") >= 1){
                atkSpeedTimer /= 6;
            }
            
            ObjectPooling.Spawn(candleFlare, new float[]{transform.position.x, transform.position.y});
            //Instantiate(candleFlare).GetComponent<CandleFlare>().setPosition(transform.position);

        }else{
            atkSpeedTimer-=Time.deltaTime;
        }
    }
}

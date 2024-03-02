using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bunny : Enemy
{

    public bool jumping;
    float timer;
    public float jumpTimer;
    // Start is called before the first frame update
    void Start()
    {
        timer = jumpTimer;
        if(!EnemySpawner.Instance.PresentEnemies.Contains(this)){
            EnemySpawner.Instance.PresentEnemies.Add(this);
        }
        base.flame = Flamey.Instance;
        
        Speed =  Distribuitons.RandomTruncatedGaussian(0.02f,Speed,0.075f);
        MaxHealth = Health;
    }

    // Update is called once per frame
    override public void UpdateEnemy()
    {
        if(jumping){
            base.UpdateEnemy();
        }else{
            if(timer > 0){
                timer-=Time.deltaTime;
            }else{
                timer = jumpTimer;
                jumping = true;
                GetComponent<Animator>().Play("Jump");
            }
        }
    }

    public void Landed(){
        jumping = false;
    }
}

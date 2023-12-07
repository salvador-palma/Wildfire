using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snail : Enemy
{
    
    private bool check = false;
    [SerializeField]  private bool withShell;
    private float maxSpeed;
    private void Start() {
        base.flame = Flamey.Instance;
        maxSpeed =  Distribuitons.RandomTruncatedGaussian(0.02f,Speed,0.075f);
        Speed = maxSpeed;
        
        MaxHealth = Health;
        StartAnimations(withShell? 0 : 2);
    }
    private void Update() {
        
        base.Move();
        if(Vector2.Distance(flame.transform.position, HitCenter.position) < AttackRange && !check){
            check = true;
            Speed = 0.00001f;
            GetComponent<Animator>().SetTrigger("InRange");
            InvokeRepeating("Attack",0f, AttackDelay);
        }
    }
    public void SlideOn(){
        Speed = maxSpeed;
    }
    public void SlideOff(){
        Speed = 0.00001f;
    }

    
    
}


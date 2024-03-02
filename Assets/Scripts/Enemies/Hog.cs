using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Hog : Enemy
{
    public int chargeAmount;

   

    private void Start() {

        
        base.flame = Flamey.Instance;
        
        Speed =  Distribuitons.RandomTruncatedGaussian(0.02f,Speed,0.075f);
        MaxHealth = Health;

        //StartAnimations(6);
    }


    override public void UpdateEnemy() {
           
           
        if(chargeAmount < 0 ){
            base.UpdateEnemy();
        }
    }
    

    public void decreaseCharge(){
        chargeAmount--;
        if(chargeAmount <0){
            GetComponent<Animator>().Play("Run");
        }
    }

    bool AttackedAlready = false;
    public override void Attack(){
        if(AttackedAlready){
            flame.Hitted(Damage/2, ArmorPen, this);
        }else{
            AttackedAlready = true;
            flame.Hitted(Damage, ArmorPen, this);
        }
       
    }


    override protected void PlayAttackAnimation(){
        GetComponent<Animator>().Play("Attack");
    }
    public override void CheckFlip()
    {
        base.CheckFlip();
        if(transform.position.x < 0){
            
            transform.Find("SmokeFront").GetComponent<SpriteRenderer>().flipX = true;
            transform.Find("SmokeBack").GetComponent<SpriteRenderer>().flipX = true;
        }
        

    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public Flamey flame;
    public int Damage;
    public float AttackDelay;
    public float AttackRange;
    public float Speed;
    public int Health;
   
    

    
    public void Move(){
        transform.position = Vector2.MoveTowards(transform.position, flame.transform.position, Speed * Time.deltaTime);
    }

    private void Hitted(Tuple<int,bool> res){
        
        Health -= res.Item1;
        Flamey.Instance.ApplyOnHit(res.Item1, Health);
        if(Health <= 0){Die();}
        PlayHitAnimation(res);
    }
    private void PlayHitAnimation(Tuple<int,bool> res){
        GetComponent<Animator>().Play("EnemyHit");
        DamageUI.Instance.spawnTextDmg(transform.position, "-"+res.Item1, res.Item2 ? Color.yellow : Color.white);
    }

    private void Die(){
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        
        if(other.tag == "FlareHit"){
            
            Hitted(Flamey.Instance.getDmg());
        }
    }

    public void Attack(){
        flame.Hitted(Damage);
        PlayAttackAnimation();
    }
    private void PlayAttackAnimation(){
        GetComponent<Animator>().Play("EnemyAttack");
    }
    public IEnumerator AttackRoutine(){
        yield return new WaitForSecondsRealtime(AttackDelay);
        Attack();
    }
    public void target(){
        transform.GetChild(0).gameObject.SetActive(true);
    }
    public void untarget(){
        transform.GetChild(0).gameObject.SetActive(false);
    }


    private void OnMouseDown() {
        
        Flamey.Instance.target(this);
    }
}

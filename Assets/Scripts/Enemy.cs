using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour,IComparable<Enemy>
{
    public Flamey flame;
    public int Damage;
    public float AttackDelay;
    public float AttackRange;
    public float Speed;
    public int Health;
   
    public Transform HitCenter;

    
    public void Move(){
        transform.position = Vector2.MoveTowards(transform.position, flame.transform.position, Speed * Time.deltaTime);
    }

    public void StartAnimations(int ID){
        GetComponent<Animator>().SetInteger("EnemyID", ID);
    }

    private void Hitted(Tuple<int,bool> res, Vector2 explosionPos){
        
        Health -= res.Item1;
        Flamey.Instance.ApplyOnHit(res.Item1, Health);
        if(Health <= 0){Die();}
        PlayHitAnimation(res);
        SpawnExplosion(explosionPos);
    }
    private void PlayHitAnimation(Tuple<int,bool> res){
        GetComponent<Animator>().Play("EnemyHit");
        DamageUI.Instance.spawnTextDmg(transform.position, res.Item1.ToString(), res.Item2 ? 1 : 0);
    }

    private void Die(){
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        
        if(other.tag == "FlareHit"){
            
            Hitted(Flamey.Instance.getDmg(), other.transform.position);
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
    private void SpawnExplosion(Vector2 explosionPos){
        GameObject g = Instantiate(EnemySpawner.Instance.ExplosionPrefab);
        g.transform.position = explosionPos;
    }


    public int CompareTo(Enemy other)
    {
        return Vector2.Distance(transform.position, flame.transform.position) < Vector2.Distance(other.transform.position, flame.transform.position)? -1 : 1; 
    }

    public void EndEnemy(){
        this.enabled = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BeeChemical : Bee
{
    public Dictionary<Enemy, int> victims;

    public override void Start()
    {
        base.Start();
        victims = new Dictionary<Enemy, int>();
        StartCoroutine(TimedAttack());
    }
    protected override void Attack()
    {
        atkTimer = 1/atkSpeed;
        
        if(victims.ContainsKey(target)){
            victims[target] += 5;
        }else{
            victims.Add(target,5);
        }
        target=null;
        
    }
    public override void Update()
    {
        if(EnemySpawner.Instance.isOnAugments){return;}

        if(target == null){
            target = getTarget();
            if(target==null){return;}
        }

        transform.position = Vector2.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
        checkFlip();
        if(atkTimer > 0){
            atkTimer -= Time.deltaTime;
        }else{
            if(Vector2.Distance(transform.position, target.transform.position) < 1.5f){
                Attack();
            }
        }
    }
    public IEnumerator TimedAttack(){
        while(Flamey.Instance.Health > 0){
            foreach (Enemy enemy in victims.Keys.ToArray()){
                if(enemy==null){victims.Remove(enemy); continue;}
                enemy.Hitted(enemy.MaxHealth/25, 14, ignoreArmor:true, onHit:false, source:"Poison");
                victims[enemy]--;
                if(victims[enemy] <= 0){
                    victims.Remove(enemy);
                }
            }

            yield return new WaitForSeconds(1);
            
        }
    }
}
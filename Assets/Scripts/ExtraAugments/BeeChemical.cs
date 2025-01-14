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
        if(target==null){return;}
        if(victims.ContainsKey(target)){
            victims[target] += 10;
        }else{
            victims.Add(target,10);
            target.GetComponent<SpriteRenderer>().material.SetInt("_Poison", 1);
        }
        target=null;
        
    }
    
    public IEnumerator TimedAttack(){
        while(Flamey.Instance.Health > 0){

            if(victims.Count() > 0){AudioManager.PlayOneShot(FMODEvents.Instance.PoisonPop, Vector2.zero);}
            
            
            foreach (Enemy enemy in victims.Keys.ToArray()){
                if(enemy==null){victims.Remove(enemy); continue;}
                enemy.Hitted(enemy.MaxHealth/25, 14, ignoreArmor:true, onHit:false, source:"Poison");
                victims[enemy]--;

                if(victims[enemy] <= 0){
                    enemy.GetComponent<SpriteRenderer>().material.SetInt("_Poison", 0);
                    victims.Remove(enemy);
                }
            }

            yield return new WaitForSeconds(1);
            
        }
    }
}

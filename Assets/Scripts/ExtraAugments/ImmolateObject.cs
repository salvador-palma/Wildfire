using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImmolateObject : MonoBehaviour
{
    int dmg;
    float speed = 0.2f;
    
    Vector2 objective;
    void Start()
    {
        dmg = Immolate.Instance.dmg;
        float x = Immolate.Instance.radius/2f;
        float y = Immolate.Instance.radius/2f;
        speed = x*2/3f;
        objective = new Vector2(x,y);
        
    }

    // Update is called once per frame
    void Update()
    {
        

        transform.localScale = Vector2.MoveTowards(transform.localScale, objective, speed * Time.deltaTime);
        if(transform.localScale.x >= objective.x){
            GetComponent<Animator>().Play("EndImmolate");
            
            GetComponent<CapsuleCollider2D>().enabled = false;
        }
        
    }

    public void Die(){
        Destroy(gameObject);
    }
    public void ImmolateEnemy(int type, Enemy e){
        if(!e.canTarget()){return;}
        switch(type){
            case -1: 
                e.Hitted(dmg, 0, ignoreArmor:false, onHit:false);
                break;
            case 0: //FIRE
                e.Hitted(dmg, 9, ignoreArmor:true, onHit:false);
                break;
            case 1: //WATER
                Flamey.Instance.addHealth(Flamey.Instance.MaxHealth * 0.01f);
                e.Hitted(dmg, 13, ignoreArmor:false, onHit:false);
                break;
            case 2: //EARTH
                Flamey.Instance.addShield(1);
                e.Hitted(dmg, 22, ignoreArmor:false, onHit:false);
                break;
            case 3: //AIR
                //e.Stun(0.5f);
                
                e.KnockBack(Flamey.Instance.transform.position, false, .4f);
                e.Hitted(dmg, 23, ignoreArmor:false, onHit:false);
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Enemy"){
            ImmolateEnemy(Immolate.Instance.ImmolateType, other.GetComponent<Enemy>());
        }
    }
}

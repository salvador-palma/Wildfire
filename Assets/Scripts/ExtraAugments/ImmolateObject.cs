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
        float x = Immolate.Instance.radius;
        float y = transform.localScale.y * x / transform.localScale.x;
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


    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Enemy"){
            Enemy e = other.GetComponent<Enemy>();
            e.Hitted(dmg, 9, ignoreArmor:false, onHit:false);
        }
    }
}

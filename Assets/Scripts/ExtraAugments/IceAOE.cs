using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceAOE : MonoBehaviour
{
    public float Slow;

    public float LastingTime;
    float lt;

    private void Start() {
        
        lt = IceOnLand.Instance.lasting;
        Slow = IceOnLand.Instance.slow;
        Vector2 scale = transform.localScale * IceOnLand.Instance.size;
        transform.localScale = scale;

    }
    void Update()
    {
        lt-=Time.deltaTime;
        if(lt<=0){
            lt = LastingTime;
            GetComponent<Animator>().Play("EndAOEBurn");
        }
    }

    private void OnTriggerEnter2D(Collider2D collider){
        if(collider.tag == "Enemy"){
            collider.GetComponent<Enemy>().setTemporarySpeed(lt, 1-Slow, augmentClass: "IcePool");
        }
    }
    private void OnTriggerExit2D(Collider2D collider){
        if(collider.tag == "Enemy"){
            //INTERRUPT COUROUTINE HERE
        }
    }

    
}

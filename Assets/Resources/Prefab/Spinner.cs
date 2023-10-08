using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spinner : MonoBehaviour
{
    public float speed;
    public float multiplier = 1.2f;

    public GameObject[] Spinners;
   
    private void Start() {
        
        speed = Flamey.Instance.BulletSpeed;
        
    }
    private void Update() {
        
        transform.Rotate(0,0,speed*multiplier*Time.deltaTime);
    }

    public GameObject SpawnCircle(int amount){
        return Instantiate(Spinners[amount]);
    }
    public void kill(){
        Destroy(gameObject);
    }
}

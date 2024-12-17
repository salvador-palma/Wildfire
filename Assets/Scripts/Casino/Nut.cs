using System;
using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using Unity.VisualScripting;
using UnityEngine;

public class Nut : MonoBehaviour
{
    [SerializeField] float speed=0.01f;
    [SerializeField] Rigidbody2D rb;
    private void Start() {
        rb = GetComponent<Rigidbody2D>();
    }
   
    void FixedUpdate()
    {
        if(rb.velocity == Vector2.zero){
            transform.Translate(0, 0, speed * Time.deltaTime);
        }
        
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        var instance = RuntimeManager.CreateInstance(FMODEvents.Instance.AcornDrop);
        float f = collision.relativeVelocity.magnitude;
        //Debug.Log(rb.velocity.magnitude);
        
        instance.setParameterByName("AcornVelocity", Math.Clamp(f/3f,0f,1f));
        instance.start();
        instance.release();
    }
}

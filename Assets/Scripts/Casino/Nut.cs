using System.Collections;
using System.Collections.Generic;
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
}

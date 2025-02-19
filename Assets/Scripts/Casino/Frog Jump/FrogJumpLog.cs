using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FrogJumpLog : MonoBehaviour
{
    [Header("Styles")]
    public Color[] leafColors;
    public Sprite[] leafSprites;

    public float JumpForce = 10;
    public float DespawnDistance = 1;
    public bool SuperJump = false;
    public bool Moving = false;
    public float platformSpeed = 2f;

    public bool isRotten = false;
    void Start()
    {
        Vector2 scale = transform.localScale;
        
        scale.x = Math.Abs(scale.x) * (transform.position.x < 0  ? -1 : 1);
        transform.localScale = scale;
    }
    private void OnCollisionEnter2D(Collision2D other) {
        
        if (other.relativeVelocity.y < 0 && other.gameObject.tag == "Player"){
            if(isRotten){
                other.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(other.gameObject.GetComponent<Rigidbody2D>().velocity.x, 0);
                GetComponent<Animator>().Play("FallLeaf");
                
            }else{
                other.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(other.gameObject.GetComponent<Rigidbody2D>().velocity.x, JumpForce * (SuperJump ? 3 : 1));
                other.gameObject.GetComponent<FrogJump>().invicible = SuperJump;
                if(!Moving){GetComponent<Animator>().Play("LeafLand");}
            }
            
        }
    }
    public void VirtualStart(FrogJump frog){
        
        if(UnityEngine.Random.Range(0f,1f) < 0.1f){
            isRotten = true;
            GetComponent<SpriteRenderer>().sprite = leafSprites[1];
            transform.GetChild(0).GetComponent<SpriteRenderer>().color = leafColors[1];

        }else{
            isRotten = false;
            SuperJump = UnityEngine.Random.Range(0, 100) < 5;
            if(SuperJump){
                GetComponent<SpriteRenderer>().sprite = leafSprites[2];
                transform.GetChild(0).GetComponent<SpriteRenderer>().color = leafColors[2];
            }else{
                GetComponent<SpriteRenderer>().sprite = leafSprites[0];
                transform.GetChild(0).GetComponent<SpriteRenderer>().color = leafColors[0];
            }
        }

        
        Vector2 scale = transform.localScale;
        
        scale.x = Math.Abs(scale.x) * (transform.position.x < 0  ? -1 : 1);
        transform.localScale = scale;

        Moving = UnityEngine.Random.Range(0, 100) < 10;
        platformSpeed = (0.0000002f*frog.transform.position.y + 1f + UnityEngine.Random.Range(-0.5f,0.5f)) * UnityEngine.Random.Range(0f,1f)<0.5f?-1:1;

        transform.GetChild(0).gameObject.SetActive(!Moving);
        GetComponent<Animator>().SetBool("Moving", Moving);

        GetComponent<SpriteRenderer>().color = Color.white;
    }
    private void Update() {
        if(Moving){
            transform.position += new Vector3(-platformSpeed * Time.deltaTime, 0, 0);
            if(Math.Abs(transform.position.x) > 8){
                platformSpeed *= -1f;
            }
        }
    }
    public bool repos;
    public void Reposition(){
        repos = true;
    }
   
}

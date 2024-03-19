using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grasshoper : Enemy
{

    public Transform centerPoint;
    public float initialRadiusX = 9.39f;
    public float initialRadiusY = 5.07f;

    public Vector2 shrinkLimits;
    private float shrinkRate;

    private float radiusX;
    private float radiusY;
    private float angle = 0f;
    public int direction = 1;

    public bool jumping;
    float timer;
    public float jumpTimer;

    // Start is called before the first frame update
    void Start()
    {
        radiusX = initialRadiusX;
        radiusY = initialRadiusY;
        centerPoint = Flamey.Instance.transform;
        timer = jumpTimer;
        if(!EnemySpawner.Instance.PresentEnemies.Contains(this)){
            EnemySpawner.Instance.PresentEnemies.Add(this);
        }
        base.flame = Flamey.Instance;
        
        Speed =  Distribuitons.RandomTruncatedGaussian(0.02f,Speed,0.075f);
        MaxHealth = Health;
        direction = Random.Range(0f, 1f) < 0.5f ? 1 : -1;
        Vector3 relativePosition = transform.position - centerPoint.position;
        angle = Mathf.Atan2(relativePosition.y, relativePosition.x);

        shrinkRate = Random.Range(shrinkLimits[0], shrinkLimits[1]);
    }

    bool check;
    override public void UpdateEnemy()
    {
        if(jumping){
            base.UpdateEnemy();
        }else{
            if(timer > 0){
                timer-=Time.deltaTime;
            }else if(!check){
                check = true;
                GetComponent<Animator>().Play("Jump");
            }
        }
    }

    public void Landed(){
        jumping = false;
        timer = jumpTimer;
        check=false;
    }
    public void Jumping(){
        jumping = true;
    }

    float prevX;
    public override void Move(){



        float x = centerPoint.position.x + Mathf.Cos(angle) * radiusX;
        float y = centerPoint.position.y + Mathf.Sin(angle) * radiusY;

        prevX = transform.position.x;

        transform.position = new Vector3(x, y, transform.position.z);


        angle += Speed * direction * Time.deltaTime;
        radiusX -= shrinkRate * Time.deltaTime;
        radiusY -= shrinkRate * Time.deltaTime;
        
        CheckFlip();
    }

    public override void CheckFlip(){
        
            GetComponent<SpriteRenderer>().flipX = prevX < transform.position.x;

            transform.Find("Effect").GetComponent<SpriteRenderer>().flipX = prevX < transform.position.x;
        
    }
}
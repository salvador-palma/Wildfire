using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Spider : Enemy
{
    public Transform centerPoint;
    public float initialRadiusX = 9.39f;
    public float initialRadiusY = 5.07f;

    public float shrinkRate = 0.1f;

    private float radiusX;
    private float radiusY;
    private float angle = 0f;

    public int direction = 1;
    public bool rest;
    // Start is called before the first frame update
    void Start()
    {
        radiusX = initialRadiusX;
        radiusY = initialRadiusY;
        centerPoint = Flamey.Instance.transform;
        if(!EnemySpawner.Instance.PresentEnemies.Contains(this)){
            EnemySpawner.Instance.PresentEnemies.Add(this);
        }
        base.flame = Flamey.Instance;
        
        Speed =  Distribuitons.RandomTruncatedGaussian(0.02f,Speed,0.075f);
        MaxHealth = Health;

        direction = Random.Range(0f, 1f) < 0.5f ? 1 : -1;
        Vector3 relativePosition = transform.position - centerPoint.position;
        angle = Mathf.Atan2(relativePosition.y, relativePosition.x);

    }

    // Update is called once per frame
    
    
    public override void Move(){

        if(rest){return;}

        float x = centerPoint.position.x + Mathf.Cos(angle) * radiusX;
        float y = centerPoint.position.y + Mathf.Sin(angle) * radiusY;


        transform.position = new Vector3(x, y, transform.position.z);


        angle += Speed * direction * Time.deltaTime;
        radiusX -= shrinkRate * Time.deltaTime;
        radiusY -= shrinkRate * Time.deltaTime;
        

    }

    
}

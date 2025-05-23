using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using FMODUnity;
using FMOD.Studio;
public class SquirrelFlying : Squirrel
{
    [Range(0f,1f)] public float arenaLand;
    public float startingHeight;
    public bool flying = true;
    public float flyingSpeedRatio;

    public Vector2 LandDest;


    public float cos;

    [field: SerializeField] public EventReference FlySound { get; private set; }
    [field: SerializeField] public EventReference LandSound { get; private set; }
    EventInstance FlySoundInstance;

    override protected void Start() {
        VirtualPreStart(); 
        base.Start();

        Vector3 vectorAB = transform.position - Flamey.Instance.transform.position;
        cos = Math.Abs(Vector3.Dot(Vector3.right, vectorAB.normalized));


        FlySoundInstance = AudioManager.CreateInstance(FlySound);
        FlySoundInstance.start();

        LandDest =  Flamey.Instance.transform.position;
        LandDest.x += (transform.position.x - Flamey.Instance.transform.position.x) * arenaLand;
        LandDest.y += (transform.position.y - Flamey.Instance.transform.position.y) * arenaLand;
        transform.position=  new Vector2(transform.position.x, transform.position.y + (float)(startingHeight * Math.Pow(cos,3)));

    }
    public override void UpdateEnemy()  {
        LandDest =  Flamey.Instance.transform.position;
        LandDest.x += (transform.position.x - Flamey.Instance.transform.position.x) * arenaLand;
        LandDest.y += (transform.position.y - Flamey.Instance.transform.position.y) * arenaLand;
        base.UpdateEnemy();
        if( flying && Vector2.Distance(LandDest, HitCenter.position) < 0.3f ){
            Land();
        }
    }

    public override void Move(){
        if(Stunned){return;}
        if (!flying)
        {
            if (placedBomb)
            {
                transform.position = Vector2.MoveTowards(transform.position, Flamey.Instance.getPosition(), Speed* (1-SlowFactor)  * Time.deltaTime * -1f);
            }
            else
            {
                transform.position = Vector2.MoveTowards(transform.position, AttackTarget.getPosition(), Speed* (1-SlowFactor)  * Time.deltaTime);
            }
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, LandDest, Speed * (1 - SlowFactor) * flyingSpeedRatio * Time.deltaTime);
        }
        
    }
    public override int Hitted(int Dmg, int TextID, bool ignoreArmor, bool onHit, string except = null, string source = null, float[] extraInfo = null)
    {
        if(!flying){
            return base.Hitted(Dmg, TextID, ignoreArmor, onHit, except, source, extraInfo);   
        }
        return 0;
    }
    public override bool canTarget(){return !flying;}
    private void Land(){
        FlySoundInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        FlySoundInstance.release();
        AudioManager.PlayOneShot(LandSound,transform.position);

        flying = false;
        GetComponent<Animator>().SetTrigger("InGround");
    }

    
}

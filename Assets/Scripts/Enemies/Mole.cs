using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using FMODUnity;
using FMOD.Studio;
public class Mole : Enemy
{
    
    [Range(0f,1f)] public float DigUpDistance;
    public bool diggingUp;
    public bool isUnderground = true;
    public float diggingDelay = 2f;
    public float undergroundSpeedMult;
    public LineRenderer lineRenderer;
    public GameObject TrailPrefab;

    public float initialDistance;
    public  Vector3 initialPos;

    [field: SerializeField] public EventReference PopOutSound { get; private set; }
    [field: SerializeField] public EventReference DigSound { get; private set; }
    EventInstance DigSoundInstance;
    void Start()
    {

        VirtualPreStart(); 
        if(!EnemySpawner.Instance.PresentEnemies.Contains(this)){
            EnemySpawner.Instance.PresentEnemies.Add(this);
        }
        base.flame = Flamey.Instance;
        
        DigSoundInstance = AudioManager.CreateInstance(DigSound);
        DigSoundInstance.start();

        Speed =  Distribuitons.RandomTruncatedGaussian(0.02f,Speed,0.075f);
        if(EnemySpawner.Instance.current_round >= 60){
            int x = EnemySpawner.Instance.current_round;
            Health = (int)(Health * (float) (Math.Pow(x-10, 2)/350) + 1f);
            Armor = (int)(Armor * (x-45f)/15f); 
            Speed *= (float) (Math.Pow(x-10, 2)/4000f) + 1f;
            Damage = (int)(Damage * (float) (Math.Pow(x-10, 2)/2500f) + 1f);
        }
        MaxHealth = Health;
        initialDistance = Vector2.Distance(AttackTarget.getPosition(), HitCenter.position);


        initialPos = HitCenter.position;

        GameObject g = Instantiate(TrailPrefab);
        lineRenderer = g.GetComponent<LineRenderer>();
        lineRenderer.positionCount =2;
        lineRenderer.SetPositions(new Vector3[2]{initialPos, initialPos});
        if(transform.position.x < 0){
            lineRenderer.textureScale = new Vector2(lineRenderer.textureScale.x, -1);
        }
    }

    // Update is called once per frame
    override public void UpdateEnemy() {
           
        if(!diggingUp){
            base.UpdateEnemy();
        }
        if(Vector2.Distance(AttackTarget.getPosition(), HitCenter.position) < initialDistance * DigUpDistance && isUnderground){
            StartCoroutine(DigUp());
        }
        if(isUnderground){
            UpdateLineRenderer();
        }
    }

    public void UpdateLineRenderer(){
        lineRenderer.SetPosition(1, HitCenter.position);
        lineRenderer.SetPosition(0, initialPos);
        
    }
    public override void Move()
    {
        if(Stunned){return;}
        transform.position = Vector2.MoveTowards(transform.position, AttackTarget.getPosition(), Speed * (1-SlowFactor) * Time.deltaTime * (isUnderground ? undergroundSpeedMult : 1f));
    }

    IEnumerator DigUp(){
        
        DigSoundInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        DigSoundInstance.release();
        AudioManager.PlayOneShot(PopOutSound,transform.position);

        isUnderground = false;
        diggingUp = true;
        GetComponent<Animator>().Play("DigOut");
        SpawnHole();
        yield return new WaitForSeconds(diggingDelay);
        diggingUp= false;
        lineRenderer.GetComponent<Animator>().Play("TrailOff");
        

        
    }

    private void SpawnHole(){
        Transform g = lineRenderer.transform.GetChild(0);
        g.position = transform.position;
        g.gameObject.SetActive(true);

    }
    protected override void OnMouseDown()
    {
        if(isUnderground){return;}
        base.OnMouseDown();
    }

    public override bool canTarget()
    {
        return !isUnderground && !diggingUp;
    }
    public override void Die(bool onKill = true)
    {
        // if(isUnderground){
        //     DigSoundInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        //     DigSoundInstance.release();
        // }
        base.Die(onKill);
    }
     public void OnDestroy()
    {
        if(isUnderground){
            DigSoundInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            DigSoundInstance.release();
        }
        
    }

    public override int Hitted(int Dmg, int TextID, bool ignoreArmor, bool onHit, string except = null, string source = null, float[] extraInfo = null)
    {
        
        if(!isUnderground || (SkillTreeManager.Instance.getLevel("Lava Pool") >= 1 && source != null && source.Equals("Lava Pool"))){ 
            return base.Hitted(Dmg, TextID, ignoreArmor, onHit, except, source, extraInfo);
        }
        return 0;
    }

    
}

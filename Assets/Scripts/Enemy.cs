using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FMOD;
using FMODUnity;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using Debug = UnityEngine.Debug;

public abstract class Enemy : MonoBehaviour,IComparable<Enemy>
{
    public Flamey flame;
    [SerializeField] public Hittable attack_target;
    public Hittable AttackTarget
    {
        get
        {
            if (attack_target == null)
            {
                attack_target = Flamey.Instance;
            }
            return attack_target;
        }
        set
        {
            
            attack_target = value;
            
            
        }
    }
    public string Name;
    public int Damage;
    public float AttackDelay;
    public float extraAtkSpeedDelay;
    public float AttackRange;
    public float Speed;
    public int Health;
    public int MaxHealth;
    public int Armor;
    public float ArmorPen;
    public int WeigthClass;
    static float[] WeightMultipliers = new float[]{2f,1.25f, .75f, .2f};
    public Vector2Int EmberDropRange;
    public bool Attacking;
   
    public Transform HitCenter;

    public bool inEffect;

    public bool Stunned;
    public bool Shiny;

    bool being_carried;


    //FORCED CODE
    [HideInInspector] public bool hitByShred;

    [field: SerializeField] public EventReference DeathSound { get; private set; }
    [field: SerializeField] public EventReference AttackSound { get; private set; }
    [field: SerializeField] public EventReference MovingSound { get; private set; }


    [SerializeField] private float slowfactor;
    [SerializeField] public int poisonLeft;
    protected float SlowFactor{
        get{
            return slowfactor;
        }
        set{
            slowfactor = Math.Clamp(value,0f,.99f);
        }
    }
    protected void VirtualPreStart()
    {
        if (!EnemySpawner.Instance.PresentEnemies.Contains(this))
        {
            EnemySpawner.Instance.PresentEnemies.Add(this);
        }
        Health = (int)(Health * Gambling.getGambleMultiplier(4));
        Speed = Speed * Gambling.getGambleMultiplier(5);
        AttackTarget = Flamey.Instance;
       
    }
    public virtual void UpdateEnemy()  {

        Move();
        
        if(Vector2.Distance(AttackTarget.getPosition(), HitCenter.position) < AttackRange ){
           Attacking = true;
           
           GetComponent<Animator>().SetTrigger("InRange");
           StartCoroutine(PlayAttackAnimation(AttackDelay));
        }


    }
    public string AttackAnimationName = "EnemyAttack";
    public string WalkAnimationName = "Walk";
    protected virtual IEnumerator PlayAttackAnimation(float delay){
        bool breaking = false;
        while(Health>0 && !breaking){
            if (Vector2.Distance(AttackTarget.getPosition(), HitCenter.position) > AttackRange)
            {
                ReturnWalk();
                breaking = true;
            }
            else
            {
                GetComponent<Animator>().Play(AttackAnimationName);

                float startTime = Time.time;
                yield return new WaitUntil(() => Time.time - startTime >= delay || Vector2.Distance(AttackTarget.getPosition(), HitCenter.position) > AttackRange);

                yield return new WaitForSeconds(extraAtkSpeedDelay);
                
            }
        }
    }
    protected virtual void ReturnWalk(){
        Attacking = false;
        GetComponent<Animator>().ResetTrigger("InRange");
        GetComponent<Animator>().Play(WalkAnimationName);
    }
    [ContextMenu("KnockBackCenter")]
    public void KnockBackCenter(){
        StartCoroutine(KnockBackCouroutine(Vector2.zero, false, 5f));
    }
    public virtual void KnockBack(Vector2 origin, bool retracting, float power, float time = 0.5f, bool stopOnOrigin = false, float angleMissStep = 0f, float stopOnOriginMargin=0.05f){
        StartCoroutine(KnockBackCouroutine(origin, retracting, power * WeightMultipliers[WeigthClass], time, stopOnOrigin, angleMissStep, stopOnOriginMargin));
    }
    public static Vector2 RotateNormalizedVector(Vector2 normalizedVec, float angleDegrees)
    {
        float radians = angleDegrees * Mathf.Deg2Rad;

        float cos = Mathf.Cos(radians);
        float sin = Mathf.Sin(radians);

        // Rotate the vector
        float x = normalizedVec.x * cos - normalizedVec.y * sin;
        float y = normalizedVec.x * sin + normalizedVec.y * cos;

        Vector2 rotated = new Vector2(x, y).normalized; // .normalized is optional since length is preserved
        return rotated;
    }
    protected virtual IEnumerator KnockBackCouroutine(Vector2 origin, bool retracting, float power, float timer = 0.5f, bool stopOnOrigin = false, float angleMissStep = 0f, float stopOnOriginMargin=0.05f)
    {


        Vector2 diff = (Vector2)HitCenter.position - origin;
        diff.Normalize();
        diff *= retracting ? -1 : 1;
        diff = RotateNormalizedVector(diff, angleMissStep);

        while (timer > 0)
        {
            timer -= Time.deltaTime;
            transform.position = (Vector2)transform.position + diff * Time.deltaTime * power;
            if (Math.Abs(HitCenter.position.x) > 9.25f || Math.Abs(HitCenter.position.y) > 5.4f || (stopOnOrigin && Vector2.Distance(HitCenter.position, origin) < stopOnOriginMargin))
            {
                timer = 0;
            }
            yield return null;

        }

    }

    public void Poison(int tick)
    {
        if(poisonLeft <= 0){
            GetComponent<SpriteRenderer>().material.SetInt("_Poison", 1);
        }
        poisonLeft += tick;
        
    }
    public void ApplyPoison(){
        if(poisonLeft > 0){
            float perc = Smog.Instance != null && SkillTreeManager.Instance.getLevel("Smog") >= 1 ? 2 : 1;
            Hitted((int)(MaxHealth*Flamey.PoisonDrainPerc*perc), 14, ignoreArmor:true, onHit:false, source:"Poison");
            poisonLeft--;
            if(poisonLeft <= 0){
                GetComponent<SpriteRenderer>().material.SetInt("_Poison", 0);
            }
        }
    }

    public virtual void Move(){
        if(Stunned){return;}
        transform.position = Vector2.MoveTowards(transform.position, AttackTarget.getPosition(), Speed * (1-SlowFactor) * Time.deltaTime);
    }
    float spiralRatio = 2f;
    public virtual void MoveSpiral(float angle = 0, bool reverse = false)
    {
        if (Stunned) { return; }

        Vector2 diff = (Vector2)HitCenter.position - AttackTarget.getPosition();
        diff.Normalize();
        diff.y *= spiralRatio;
        diff.Normalize();

        diff = RotateNormalizedVector(diff, reverse ? 90 - angle : 270 + angle);
        //Math.Abs(HitCenter.position.x) > 9.25f || Math.Abs(HitCenter.position.y) > 5.4f 

        Vector2 next = (Vector2)transform.position + diff * Time.deltaTime * Speed * (1 - SlowFactor);
        next.y = Math.Clamp(next.y, -4.9f, 4.9f);
        next.x = Math.Clamp(next.x, -9.25f, 9.25f);
        transform.position = next;
        //transform.position = Vector2.MoveTowards(transform.position, AttackTarget.getPosition(), Speed * (1-SlowFactor) * Time.deltaTime);
    }
    public virtual void Taunt(Hittable target)
    {
        if (!AttackTarget.Equals(Flamey.Instance) || !canTarget())
        {
            return;
        }
        AttackTarget = target;
    }

    public void StartAnimations(int ID)
    {

        GetComponent<Animator>().SetInteger("EnemyID", ID);
    }
   
    public virtual void Stun(float f, string source = null){
        StartCoroutine(StunCoroutine(f, source));
    }
    private IEnumerator StunCoroutine(float f,string source = null)
    {
        if(!Stunned){
            if(source=="Thunder"){GetComponent<SpriteRenderer>().material.SetFloat("_Shock", 1);}
            Stunned = true;
            GetComponent<Animator>().enabled = false;
            yield return new WaitForSeconds(f);
            if(source=="Thunder"){GetComponent<SpriteRenderer>().material.SetFloat("_Shock", 0);}
            Stunned = false;
            GetComponent<Animator>().enabled = true;
            
        }
    }

    public virtual int Hitted(int Dmg, int TextID, bool ignoreArmor, bool onHit, string except = null, string source = null, float[] extraInfo = null){
        if(Health<=0){return 0;}
        if(attack_target==null){ return 0; }
        
        Dmg = (int)(Dmg * Gambling.getGambleMultiplier(0));

        //EXTRA DAMAGE DUE TO SKILLS
        if(IceOnHit.Instance != null && SkillTreeManager.Instance.getLevel("Freeze") >= 2 && getSlowInfo("IceHit")[0] > 0){
            Dmg *= 2;
        }
        if(!ignoreArmor){
            float B = Dmg/(1+(Armor/100f));
            float armorPen = onHit || Character.Instance.isCharacter("Assassin")? Flamey.Instance.ArmorPen : 0;
            Dmg = (int)(B + (Dmg-B)*armorPen);
            Dmg = Math.Max(Dmg, 1);
        }
        //============================

        //ACHIEVMENTS AND QUESTS
        if(Dmg >= 50000){
            GameUI.Instance.CompleteQuestIfHasAndQueueDialogue(17, "Naal", 13); //AZUREOTH UNLOCK
        }
        if(CritUnlock.Instance != null){
            if(Dmg >= Flamey.Instance.Dmg * 5f){
                GameUI.Instance.CompleteQuestIfHasAndQueueDialogue(18, "Rowl", 17); //POWERED UP UNLOCK
            }
        }

        if(source=="Thorns"){
            float DamageGiven = extraInfo[0];
            if(DamageGiven < Dmg){
                GameUI.Instance.CompleteQuestIfHasAndQueueDialogue(26, "Cloris", 13);
            }
        }

        //============================

        int amountOfOnHit = 0;
        if(onHit){amountOfOnHit = Flamey.Instance.ApplyOnHit(Dmg, Health, this, except);}


        Health -= Dmg;
        Flamey.Instance.TotalDamage+=(ulong)Dmg;
        PlayHitAnimation(Dmg, TextID); 
        return amountOfOnHit;
    }

    public void PlayHitAnimation(int dmg, int textID){
        GetComponent<Animator>().Play("EnemyHit");
        DamageUI.InstantiateTxtDmg(transform.position, dmg.ToString(), textID);
    }

    public virtual void Die(bool onKill = true){
        if(this==null){return;}
        try
        {
            Flamey.Instance.addEmbers(calculateEmbers());
            flame.TotalKills++;
            CameraShake.Shake(0.4f, 0.05f);

            EnemySpawner.AddDeath(Name, Shiny);
            if (onKill) { Flamey.Instance.ApplyOnKill(HitCenter.position); }

            AudioManager.PlayOneShot(DeathSound, transform.position);

            if (Character.Instance.isCharacter("Gravity") && Gravity.Instance != null)
            {

                Gravity.Instance.AddMass(MaxHealth, Vector2.Distance(Flamey.Instance.getPosition(), HitCenter.transform.position));
            }

            Vulture.CallVultures(this, 1);
        }
        catch
        {
            // Debug.Log("Error at: Enemy.Die()");
        }
        
        Destroy(gameObject);
    }


    protected virtual int calculateEmbers(){
        if(Shiny){
            return (int)(UnityEngine.Random.Range(EmberDropRange[0], EmberDropRange[1]) * EnemySpawner.Instance.ShinyMultiplier); 
           // return UnityEngine.Random.Range(EmberDropRange[0], EmberDropRange[1]); 

        }
        return UnityEngine.Random.Range(EmberDropRange[0], EmberDropRange[1]); 
        
    }

    
    

    public virtual void Attack(){
        AudioManager.PlayOneShot(AttackSound,transform.position);
        AttackTarget.Hitted(Damage, ArmorPen, this);
    }
    
    public void PlayMovingSound()
    {
        AudioManager.PlayOneShot(MovingSound,transform.position);
    }
    public void PlayAttackingSound()
    {
        AudioManager.PlayOneShot(AttackSound,transform.position);
    }
    
    public virtual void target(){
        transform.GetChild(0).gameObject.SetActive(true);
    }
    public virtual void untarget(){
        transform.GetChild(0).gameObject.SetActive(false);
    }

    public virtual bool canTarget(){return true;}
    
    

    

    protected virtual void OnMouseDown() {
        
        Flamey.Instance.target(this);
    }
    public static void SpawnExplosion(Vector2 explosionPos){
        ObjectPooling.Spawn(EnemySpawner.Instance.ExplosionPrefab, new float[]{explosionPos.x, explosionPos.y});
    }


    public int CompareTo(Enemy other)
    {
        if(other==null){return 1;}
        return Vector2.Distance(HitCenter.position, flame.transform.position) < Vector2.Distance(other.HitCenter.position, flame.transform.position)? -1 : 1; 
    }

    public void EndEnemy(){
        this.enabled = false;
    }

    public virtual void CheckFlip(){
        if(transform.position.x < 0){
            bool flipped = !GetComponent<SpriteRenderer>().flipX;
            GetComponent<SpriteRenderer>().flipX = flipped;
            transform.Find("Effect").GetComponent<SpriteRenderer>().flipX = flipped;
        }
    }



    public Dictionary<string, float[]> SlowEffectsDuration = new Dictionary<string, float[]>{{"IceHit", new float[2]},{"IceLand", new float[2]}};
    float SlowDecayRate = 0.2f;
    public float SlowSet;
    public virtual void SlowDown(float seconds, float percentage, string SlowEffect){

        float[] prevInfo = getSlowInfo(SlowEffect);

        if(prevInfo == null || prevInfo[0] <= 0){
            SlowSet += percentage; 
            GetComponent<SpriteRenderer>().material.SetFloat("_Frozen", SlowSet);
        }
        if(IceOnLand.Instance!= null && SkillTreeManager.Instance.getLevel("Snow Pool") >= 2 && prevInfo[0] > 0){
            Stun(2f, "IceLand");
        }

        SlowEffectsDuration[SlowEffect] = new float[2]{seconds, percentage};
        
    }  
    public float[] getSlowInfo(string SlowEffect){
        return SlowEffectsDuration == null ? null : SlowEffectsDuration.GetValueOrDefault(SlowEffect, null);
    }
    public void ApplySlowUpdate(){
        foreach (string slow in SlowEffectsDuration.Keys)
        {   
            if(SlowEffectsDuration[slow][0] > 0){
                SlowEffectsDuration[slow][0] -= Time.deltaTime;
                if(SlowEffectsDuration[slow][0] <= 0){
                    SlowSet = Math.Clamp(SlowSet - SlowEffectsDuration[slow][1],0,1);
                }
            }
        }
        if(SlowEffectsDuration.Keys.Count == 0 || (!SlowEffectsDuration.Keys.Any(a => a[0] > 0))){
            SlowSet = 0;
        }
        if(SlowSet > SlowFactor){
            SlowFactor = SlowSet;
            GetComponent<SpriteRenderer>().material.SetFloat("_Frozen", SlowFactor);
        }else if(SlowSet < SlowFactor){
            SlowFactor -= Time.deltaTime * SlowDecayRate;
            GetComponent<SpriteRenderer>().material.SetFloat("_Frozen", SlowFactor);
        }

    } 
    public void removeSlow(string effect){
        if(SlowEffectsDuration.ContainsKey(effect)){
            SlowEffectsDuration[effect][0] = 0;
            //GetComponent<SpriteRenderer>().material.SetFloat("_Frozen", SlowSet);
        }
    }



    public static Vector2 getPredicatedEnemyPosition(Comparison<Enemy> sortingFactor){
        List<Enemy> selected = GameObject.FindGameObjectsWithTag("Enemy").Select(I => I.GetComponent<Enemy>()).ToList();
        if(selected.Count == 0){return Vector2.zero;}
        selected.Sort(sortingFactor);
        return selected.First().HitCenter.position;
        
    }
    public static Enemy getPredicatedEnemy(Comparison<Enemy> sortingFactor, List<Enemy> except = null, Predicate<Enemy> filter = null){
        List<Enemy> selected = GameObject.FindGameObjectsWithTag("Enemy").Select(I => I.GetComponent<Enemy>()).ToList();
        selected = selected.Where(e => filter(e)).ToList();

        if (selected.Count == 0) { return null; }
        selected.Sort(sortingFactor);

        Enemy selectedEnemy = selected.First();
        while (except != null && except.Contains(selectedEnemy))
        {
            selected.Remove(selectedEnemy);
            if(selected.Count == 0){return null;}
            selectedEnemy = selected.First();
        }
        return selected.First();
        
    }
    public static Enemy getClosestEnemy(Vector2 pos, int index = 0){
        Enemy[] go = GameObject.FindGameObjectsWithTag("Enemy").Select(e=>e.GetComponent<Enemy>()).Where(e=>e.canTarget()).ToArray();
        Array.Sort(go, (a,b)=> Vector2.Distance(a.HitCenter.position, pos) < Vector2.Distance(b.HitCenter.position, pos)? -1 : 1);
        if(go.Length == 0){return null;}
        if(go.Length <= index){return go[go.Length - 1];}
        return go[index];
    }

}

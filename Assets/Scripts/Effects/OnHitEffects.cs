using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DG.Tweening;
using FMOD.Studio;
using FMODUnity;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public interface Effect{
    public string getText();
    public string getType();
    public string getDescription();
    public string getIcon();
    public string[] getCaps();
    public GameObject getAbilityOptionMenu();
    
}
public interface OnHitEffects: Effect
{
    public bool addList();
    public int ApplyEffect(float dmg = 0, float health = 0, Enemy en = null);
}
public class VampOnHit : OnHitEffects
{
    public static VampOnHit Instance;
    public float perc;
    public float prob;

    float DamageToOverheal = 1000;
    float DamageOverhealed = 0;
    public Image cooldownImage;
    public VampOnHit(float perc, float prob){
        
        this.perc = perc;
        this.prob = prob;
        if(Instance == null){
            Instance = this;
        }else{
            Instance.Stack(this);
        }
    }
    public int ApplyEffect(float dmg, float health = 0, Enemy en = null)
    {
        if(UnityEngine.Random.Range(0f,1f) < prob){
            Flamey.Instance.addHealth(Math.Abs(dmg * perc * (SkillTreeManager.Instance.getLevel("Vampire") >= 1 && en.Health < Flamey.Instance.Dmg ? 2f : 1f)));
            return 1;
        }
        return 0;
    }
    public void OverHeal(float f){
        DamageOverhealed += f;
        if(DamageOverhealed > DamageToOverheal){
            DamageOverhealed = 0;
            Flamey.Instance.MaxHealth++;
        }
        cooldownImage.fillAmount = DamageOverhealed/DamageToOverheal; 
    }
    public void Stack(VampOnHit vampOnHit){
        perc += vampOnHit.perc;
        prob += vampOnHit.prob;
        RemoveUselessAugments();
    }
    private void RemoveUselessAugments(){
        if(prob >= 1f){
            prob = 1;
            Deck deck = Deck.Instance;
            deck.removeClassFromDeck("VampProb");
        }
        if(perc >= 1f && SkillTreeManager.Instance.getLevel("Vampire") < 2)  {
            perc = 1;
            Deck deck = Deck.Instance;
            deck.removeClassFromDeck("VampPerc");
        }    
        
    }
   
    public bool addList(){
        return Instance == this;
    }

    public string getText()
    {
        return "Vampire";
    }

    public string getType()
    {
        return "On-Hit Effect";
    }

    public string getDescription()
    {
        return "You have a chance of <color=#0CD405>healing</color> a percentage of the <color=#FF5858>damage</color> dealt.";
    }
    public string[] getCaps()
    {
        if(SkillTreeManager.Instance.getLevel("Vampire") < 2){
            return new string[]{"Chance: {0}% (Max. 100%) <br>Healing Percentage: {1}% (Max. 100%)", Mathf.Round(prob*100).ToString(), Mathf.Round(perc*100).ToString()};
        }
        return new string[]{"Chance: {0}% (Max. 100%) <br>Healing Percentage: {1}% (Max. Infinite%)", Mathf.Round(prob*100).ToString(), Mathf.Round(perc*100).ToString()};
        
    }
    public string getIcon()
    {
        return "VampUnlock";
    }

    public GameObject getAbilityOptionMenu(){
        return null;
    }
}

public class IceOnHit : OnHitEffects

{
   
    public static IceOnHit Instance;
    public float duration;
    public float prob;
    private Button activeCooldownImage;
    private int activeRoundsLeft;
    private int activeRoundsCooldown = 2;
    public IceOnHit(float duration, float prob){
       
        this.duration = duration;
        this.prob = prob;
        if(Instance == null){
            Instance = this;
        }else{
            Instance.Stack(this);
        }
    }
    public int ApplyEffect(float dmg, float health = 0, Enemy en = null)
    {
        if(en==null || en.getSlowInfo("IceHit")[0] > 0){return 0;}
        if(Flamey.Instance.MaxHealth <= 1000){return 0;}
        if(UnityEngine.Random.Range(0f,1f) < prob){   
            
            float perc = getSlowPerc();
            en.SlowDown(duration/1000f, perc, "IceHit");

            if(en?.Health > dmg && perc>0 && en?.SlowSet > 0){
                
                AudioManager.PlayOneShot(FMODEvents.Instance.IceProc, Vector2.zero);
            }

            //NEPTUNE ACHIEVMENT
            if(perc >= .5f && FlameCircle.Instance != null ){
                if(Timer == -1){
                    Timer = Time.time;
                }else if(Time.time - Timer <= 1f){
                    frozenEnemies++;
                   
                    if(frozenEnemies>=20){
                        GameUI.Instance.CompleteQuestIfHasAndQueueDialogue(43,"Betsy",21);
                    }
                }else{
                    Timer = Time.time;
                    frozenEnemies = 0;
                }
            }

            return 1;
            
        }
        return 0;
    }
    public float getSlowPerc(){
        int fator = SkillTreeManager.Instance.getLevel("Freeze") >= 1 ? 2 : 1;
            float perc = Mathf.Clamp((Flamey.Instance.MaxHealth-250) * 0.0002666f * fator, 0, 0.75f);
            return perc;
    }
    //QUEST NEPTUNE
    private int frozenEnemies;
    private float Timer = -1f;
    public void Stack(IceOnHit iceOnHit){
        duration += iceOnHit.duration;
        prob += iceOnHit.prob;
        RemoveUselessAugments();
    }
    private void RemoveUselessAugments(){
        Deck deck = Deck.Instance;
        if(prob >= 1f){
            prob = 1;
            deck.removeClassFromDeck("IceProb");
        }
        if(duration >= 10000){
            duration= 10000;
            deck.removeClassFromDeck("IceDuration");
        }    
        
    }
    
    public void SpawnExtraAssets(){
        activeCooldownImage = GameUI.Instance.SpawnUIActiveMetric(Resources.Load<Sprite>("Icons/IceUnlock"));
        activeCooldownImage.transform.GetChild(0).GetComponent<Image>().fillAmount = 1;
        Deck.RoundOver += UpdateActive;
        activeCooldownImage.onClick.AddListener(() => {
            
            Flamey.Instance.GetComponent<Animator>().SetBool("IceSquimo", true);
            foreach (Enemy enemy in EnemySpawner.Instance.PresentEnemies)
            {
                enemy.SlowEffectsDuration["IceHit"] = new float[2];
                int fator = SkillTreeManager.Instance.getLevel("Freeze") >= 1 ? 2 : 1;
                enemy.SlowDown(30, .99f, "IceHit");
            }
            
            Flamey.Instance.callFunctionAfter(() =>{Flamey.Instance.GetComponent<Animator>().SetBool("IceSquimo", false);}, 30f);
            activeCooldownImage.interactable = false;
            activeRoundsLeft = 0;
            activeCooldownImage.transform.GetChild(0).GetComponent<Image>().fillAmount = 0;

        });
    }
    
    private void UpdateActive(object sender, EventArgs e){
        if(activeRoundsLeft<activeRoundsCooldown){
            activeRoundsLeft++;
            activeCooldownImage.transform.GetChild(0).GetComponent<Image>().fillAmount = ((float)activeRoundsLeft)/activeRoundsCooldown;
        }
        if(activeRoundsLeft>=activeRoundsCooldown){
             activeCooldownImage.interactable = true;
        }
    }
    public bool addList(){
        return Instance == this;
    }

    public string getText()
    {
        return "Freeze";
    }

    public string getType()
    {
        return "On-Hit Effect";
    }

    public string getDescription()
    {
        return "You have a chance of slowing the enemy for a percentage of its <color=#AFEDFF>speed</color> for a certain duration. This effect scales with <color=#0CD405>Max Health</color> <color=#FFCC7C>(+1% slow per 33 Extra Max Health)" ;
    }
    public string[] getCaps()
    {
        int fator = SkillTreeManager.Instance.getLevel("Freeze") >= 1 ? 2 : 1;
        float percentage = Mathf.Clamp((Flamey.Instance.MaxHealth-250) * 0.0002666f * fator,0f,0.75f);
        
        
        return new string[]{"Chance: {0}% (Max. 100%) <br>Slow Percentage: {1}% (Max 75%)<br>Duration: {2}s (Max. 10s)", Mathf.Round(prob*100).ToString(), Mathf.Round(percentage * 100).ToString(), (Mathf.Round(duration/10f)/100f).ToString()};
    }

    public string getIcon()
    {
        return "IceUnlock";
    }
    public GameObject getAbilityOptionMenu(){
        return null;
    }
}

public class ShredOnHit : OnHitEffects
{
    
    public static ShredOnHit Instance;
    public float percReduced;
    public float prob;

    float DamageToArmor = 500;
    float DamageArmor = 0;
    private Image cooldownImage;

    public static IPoolable MusicNotesParticle;

    public ShredOnHit(float prob, float percReduced){
       
        this.percReduced = percReduced;
        this.prob = prob;
        if(Instance == null){
            Instance = this;
            MusicNotesParticle = Resources.Load<GameObject>("Prefab/MusicNotes").GetComponent<IPoolable>();
        }else{
            Instance.Stack(this);
        }
    }
    public int ApplyEffect(float dmg, float health = 0, Enemy en = null)
    {
        if(en==null){return 0;}
        
        if(UnityEngine.Random.Range(0f,1f) < prob){
            
           
            if(en.Armor > 0){
                PlayAudio();
                
                ObjectPooling.Spawn(MusicNotesParticle, new float[]{en.HitCenter.position.x, en.HitCenter.position.y});
            }

            float actualPercReduced = percReduced;
            if(SkillTreeManager.Instance.getLevel("Resonance") >= 1){
                actualPercReduced += (Flamey.Instance.accuracy/100f + ((Flamey.Instance.BulletSpeed*Gambling.getGambleMultiplier(1))-5)/15f)/10;
            }
            float prevArmor = en.Armor;
            en.Armor = Math.Max(0, (int)(en.Armor *  actualPercReduced) - 1);
         

            if(Character.Instance.isCharacter("Shred")){
                DamageArmor += prevArmor - en.Armor;
                if(DamageArmor > DamageToArmor){
                    DamageArmor = 0;
                    Flamey.Instance.Armor++;
                }
                cooldownImage.fillAmount = DamageArmor/DamageToArmor; 
            }
            

            if(SkillTreeManager.Instance.getLevel("Resonance") >= 2){
                if(prevArmor - en.Armor > 0){
                    Flamey.Instance.addHealth(prevArmor - en.Armor);
                }
                
            }
            
            return 1;
        }
        return 0;
    }
    
    private void PlayAudio(){
        //AudioManager.PlayOneShot(FMODEvents.Instance.ResonanceEffect, Vector2.zero);
        AudioManager.Instance.PlayResonanceSound();
        //AudioManager.Instance.SetAmbienceParameter("SoundBoost",1);
        //AudioManager.Instance.SetAmbienceParameter("SoundBoost",0);
    }
    public void Stack(ShredOnHit shredOnHit){
        percReduced += shredOnHit.percReduced;
        prob += shredOnHit.prob;
        RemoveUselessAugments();
    }
    private void RemoveUselessAugments(){
        if(prob >= 1f){
            prob = 1;
            Deck deck = Deck.Instance;
            deck.removeClassFromDeck("ShredProb");
        }  
        if(percReduced >= 0.5f){
            percReduced = 0.5f;
            Deck deck = Deck.Instance;
            deck.removeClassFromDeck("ShredPerc");
        }      
        if(percReduced >= 0.5f && prob >= 1f){
            GameUI.Instance.CompleteQuestIfHasAndQueueDialogue(27, "Cloris", 14);

        }
    }
    

    public void SpawnExtraAssets(){
        cooldownImage = GameUI.Instance.SpawnUIMetric(Resources.Load<Sprite>("Icons/ShredUnlock"));
    }
    public bool addList(){
        return Instance == this;
    }

    public string getText()
    {
        return "Resonance";
    }

    public string getType()
    {
        return "On-Hit Effect";
    }

    public string getDescription()
    {
        return "You have a chance of reducing the target's <color=#919191>armor</color> by a certain percentage" ;
    }
    public string[] getCaps()
    {
        return new string[]{"Chance: {0}% (Max. 100%) <br>Percentage Reduced: {1}% (Max. 50%)", Mathf.Round(prob*100).ToString(), Mathf.Round(percReduced * 100).ToString()};
    }

    public string getIcon()
    {
        return "ShredUnlock";
    }
    public GameObject getAbilityOptionMenu(){
        return null;
    }
}

public class ExecuteOnHit : OnHitEffects
{
    
    public static ExecuteOnHit Instance;
    public float percToKill;
    private IPoolable Ghost;
    public ExecuteOnHit(float percToKill){
       
        this.percToKill = percToKill;
        
        if(Instance == null){
            Instance = this;
            Ghost = Resources.Load<GameObject>("Prefab/ExecuteGhost").GetComponent<IPoolable>();
        }else{
            Instance.Stack(this);
        }
    }
    public int ApplyEffect(float dmg, float health, Enemy en)
    {
        try{

            
            if(en==null || en.Health<=0){return 0;}
            if(en.Health < en.MaxHealth * percToKill){
                float f = en.Health;
                Vector2 v = en.transform.position;
                Vector2 pos = en.HitCenter.position;
                int prevHealth = en.Health;
                en.Health = 0;
                
                if(SkillTreeManager.Instance.getLevel("Assassin")>=2){
                    Flamey.Instance.ApplyOnKill(pos);
                    Flamey.Instance.ApplyOnKill(pos);
                }
                
                ObjectPooling.Spawn(Ghost, new float[]{pos.x,pos.y});

                //Debug.Log("Execute: " +  prevHealth + " Health ");
                if(prevHealth > Flamey.Instance.MaxHealth){
                    GameUI.Instance.CompleteQuestIfHasAndQueueDialogue(22, "Naal", 14);
                }
                
                return 1;
            }
        }catch(Exception e){
            Debug.LogError(e.Message + "; " + en);
        }
        return 0;
        
    }
    public void Stack(ExecuteOnHit executeOnHit){
        percToKill = Mathf.Min(0.5f, percToKill + executeOnHit.percToKill);
        RemoveUselessAugments();
    }
    private void RemoveUselessAugments(){
        if(percToKill >= 0.5f){
            percToKill = 0.5f;
            Deck deck = Deck.Instance;
            deck.removeClassFromDeck("Execute");
        } 
    }
    
        
    public bool addList(){
        return Instance == this;
    }

    public string getText()
    {
        return "Assassin";
    }

    public string getType()
    {
        return "On-Hit Effect";
    }

    public string getDescription()
    {
        return "You <color=#FF5858>penetrate</color> through a percentage of enemy <color=#919191>armor</color>. Additionally, hitting enemies below a portion of their <color=#0CD405>Max Health</color> <color=#FFCC7C>executes</color> them." ;
    }
    public string[] getCaps()
    {
        if(SkillTreeManager.Instance.getLevel("Assassin")>=1){
            return new string[]{"Armor Penetration: {0}% (Max. 80%) <br>Execution: {1}% (Max. 50%)", Mathf.Round(Flamey.Instance.ArmorPen * 100).ToString(), Mathf.Round(percToKill*100).ToString()};
        }
        return new string[]{"Armor Penetration: {0}% (Max. 80%)", Mathf.Round(Flamey.Instance.ArmorPen * 100).ToString()};
    }

    public string getIcon()
    {
        return "Assassins";
    }
    public GameObject getAbilityOptionMenu(){
        return null;
    }
}

public class StatikOnHit : OnHitEffects
{
    
    public static StatikOnHit Instance;
    public GameObject prefab;
    public GameObject prefabPowered;
    private GameObject statikMeter;
    private Slider statikMeterSlider;
    public float prob;
    public int dmg;
    public int ttl;

    public int procAmount; //for character
    public StatikOnHit(float prob, int dmg, int ttl){
        
        this.prob = prob;
        
        this.dmg = dmg;
        this.ttl = ttl;
        prefab = Resources.Load<GameObject>("Prefab/StatikShiv");
        prefabPowered = Resources.Load<GameObject>("Prefab/StatikShivEmpowered");
        statikMeter = Resources.Load<GameObject>("Prefab/AbilityCharacter/Statik Meter UI");
        if(Instance == null){
            Instance = this;
        }else{
            Instance.Stack(this);
        }
    }
    public int ApplyEffect(float dmg, float health = 0, Enemy en = null)
    {
        if(en==null){return 0;}
        
        if(UnityEngine.Random.Range(0f,1f) < prob){

            if(Character.Instance.isCharacter("Statik")){
                procAmount++;
                if(procAmount > 100){
                    procAmount=0;
                    Flamey.Instance.CallCoroutine(StatikCouroutine(true,30,8f,en));
                    return 1;
                }
                statikMeterSlider.value = procAmount;
            }
            Flamey.Instance.CallCoroutine(StatikCouroutine(false,ttl,1.75f,en));
            return 1;
        }
        return 0;
        
    }
    public void Stack(StatikOnHit statikOnHit){
        prob += statikOnHit.prob;
        dmg += statikOnHit.dmg;
        ttl += statikOnHit.ttl;
        RemoveUselessAugments();
    }
    private void RemoveUselessAugments(){
        if(prob >= 1f){
            prob = 1f;
            Deck deck = Deck.Instance;
            deck.removeClassFromDeck("StatikProb");
        }  
        if(ttl >= 10){
            ttl = 10;
            Deck deck = Deck.Instance;
            deck.removeClassFromDeck("StatikTTL");
        }      
    
    }
    
    public void SpawnExtraAssets(){
        GameObject g = GameUI.Instance.SpawnUI(statikMeter);
        statikMeterSlider = g.GetComponent<Slider>();
    }
    public bool addList(){
        return Instance == this;
    }

    public string getText()
    {
        return "Static Energy";
    }

    public string getType()
    {
        return "On-Hit Effect";
    }

    public string getDescription()
    {
        return "When you hit an enemy, you have a chance of unleashing a <color=#FFCC7C>static energy chain</color> that travels through enemies nearby, dealing damage to each while applying <color=#FF99F3>On-Hit effects</color>. The more the chain travels the less damage it deals";
    }
    public string[] getCaps()
    {
        return new string[]{"Chance: {0}% (Max. 100%) <br>Travel Distance: {1} Enemies (Max. 10) <br>Damage: +{2}", Mathf.Round(prob * 100).ToString(), ttl.ToString(), dmg.ToString()};

    }

    public string getIcon()
    {
        return "StatikUnlock";
    }
    public GameObject getAbilityOptionMenu(){
        return null;
    }


    private Enemy PickRandomEnemy(Vector2 pos, float radius, List<Enemy> passed){
        try{
            Enemy[] colcol = Physics2D.OverlapCircleAll(pos, radius).OrderBy(x => UnityEngine.Random.value).Select(g=>g.GetComponent<Enemy>()).ToArray();
            foreach(Enemy e in colcol){
                if(!passed.Contains(e) && e.canTarget()){
                    return e;
                }
            }
            return null;
        }catch{ 
            return null;
        }
    }
    int id = 0;
    IEnumerator StatikCouroutine(bool Powered, int TTL, float radius, Enemy en){
        List<Enemy> passed = new List<Enemy>(){en};
        LineRenderer lineRenderer = null;

        Vector3[] points = new Vector3[1]{en.HitCenter.position};
        int Damage = dmg;
        bool decays = SkillTreeManager.Instance.getLevel("Static Energy") < 2;

        int amountOfOnHit = 0;
        for(int i = 0; i<TTL-1; i++){
            
            Enemy next = PickRandomEnemy(points.Last(), radius, passed);
            passed = passed.Append(next).ToList();

            if(next != null){
                try{
                    //CRIAR LINE RENDERER SE N EXISTIR
                    if(lineRenderer==null){

                        GameObject g = ObjectPooling.Spawn(Powered ? prefabPowered.GetComponent<IPoolable>() : prefab.GetComponent<IPoolable>());
                        lineRenderer = g.GetComponent<LineRenderer>();
                        AudioManager.PlayOneShot(FMODEvents.Instance.StatikHit, en.HitCenter.position);
                    }

                    //SETUP POINTS
                    points = points.Append(next.HitCenter.position).ToArray();
                    if(points.Length > 5){
                        points = points.Skip(1).ToArray();
                    }
                    lineRenderer.positionCount = points.Length;
                    lineRenderer.SetPositions(points);
                    
                    if(TTL+1 < lineRenderer.positionCount){
                        Debug.LogError("TTL: " + TTL + "; POINTS: " +lineRenderer.positionCount);
                    }
                    
                    

                    //DAR DAMAGE
                    if(Powered){
                        amountOfOnHit += next.Hitted(Damage, 14, ignoreArmor: true, onHit:true);
                    }else{
                        amountOfOnHit += next.Hitted(Damage, 6, ignoreArmor: false, onHit: SkillTreeManager.Instance.getLevel("Static Energy") >= 1 , except:"Static Energy");
                    }

                    //NEXT ITERATIONS
                    Damage = decays ? (int)(Damage*0.9f):Damage;
                }catch{
                    lineRenderer.GetComponent<IPoolable>().UnPool();
                }
                //DELAY
                yield return new WaitForSeconds(.1f);
            }else{
                break;
            }
        }     
        if(lineRenderer!=null){lineRenderer.GetComponent<IPoolable>().UnPool();}
        
        
        if(amountOfOnHit >= 20){
            if(GameVariables.hasQuest(12)){
                GameUI.Instance.CompleteQuestIfHasAndQueueDialogue(12, "Rowl", 13);
            }
        }
     
    }


}
using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.UI;
using Quaternion = UnityEngine.Quaternion;
using Random = UnityEngine.Random;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public interface OnKillEffects : Effect
{
    public bool addList();
    public void ApplyEffect(Vector2 en);
}


public class Explosion : OnKillEffects
{
    public float prob;
    public int dmg;
    public static Explosion Instance;
    
    public static IPoolable Prefab;
    float radiusExplosion;
    
    private int ExplosionsUntilTrueDamage=20;
    private int ExplosionsDone;
    private Image cooldownImage;
    
    public Explosion(float prob, int dmg){
        this.prob = prob;
        this.dmg = dmg;
        if(Instance == null){
            Instance = this;
            if(SkillTreeManager.Instance.getLevel("Explosion")>=1){
                Prefab = Resources.Load<GameObject>("Prefab/ExplosionOnDeathGiant").GetComponent<IPoolable>();
                radiusExplosion = 1.8f;
            }else{
                Prefab = Resources.Load<GameObject>("Prefab/ExplosionOnDeath").GetComponent<IPoolable>();
                radiusExplosion = 1.2f;
            }
            if(SkillTreeManager.Instance.getLevel("Explosion")>=2){
                cooldownImage = GameUI.Instance.SpawnUIMetric(Resources.Load<Sprite>("Icons/ExplodeUnlock"));
            }
        }else{
            Instance.Stack(this);
        }
    }
    public bool addList()
    {
        return this == Instance;
    }

    public void ApplyEffect(Vector2 pos)
    {
        
        if(Random.Range(0f,1f) < prob){
            

            Collider2D[] targets = Physics2D.OverlapCircleAll(pos, radiusExplosion, Flamey.EnemyMask);
            ObjectPooling.Spawn(Prefab, new float[]{pos.x, pos.y});
            AudioManager.PlayOneShot(FMODEvents.Instance.Explosion, Vector2.zero);

            foreach(Collider2D col in targets){
                col.GetComponent<Enemy>().Hitted(dmg, 1, ignoreArmor:ExplosionsDone>=ExplosionsUntilTrueDamage, onHit:false);
            }

            if(Character.Instance.isCharacter("Explosion")){
                ExplodeCampfire(Flamey.Instance.transform.position);
            }

            if(SkillTreeManager.Instance.getLevel("Explosion")>=2){
                ExplosionsDone++;
                if(ExplosionsDone >= ExplosionsUntilTrueDamage){
                    ExplosionsDone=0;
                }
                
                cooldownImage.fillAmount = ((float)ExplosionsDone)/ExplosionsUntilTrueDamage;
            }
            
            
        }
    }
    public void ExplodeCampfire(Vector2 pos){
        Collider2D[] targets = Physics2D.OverlapCircleAll(pos, radiusExplosion, Flamey.EnemyMask);
        ObjectPooling.Spawn(Prefab, new float[]{pos.x, pos.y});

        foreach(Collider2D col in targets){
            col.GetComponent<Enemy>().Hitted(dmg, 1, ignoreArmor:false, onHit:false);
        }
    }
    public void Stack(Explosion vampOnDeath){
        prob += vampOnDeath.prob;
        dmg += vampOnDeath.dmg;
        RemoveUselessAugments();
    }
    private void RemoveUselessAugments(){
        if(prob >= .5f){
            prob = .5f;
            Deck deck = Deck.Instance;
            deck.removeClassFromDeck("ExplodeProb");
        } 
        
    }
   
    public string getDescription()
    {
        return "Everytime you kill an enemy, there's a chance of generating a <color=#FFCC7C>massive explosion</color> that <color=#FF5858>damages</color> nearby enemies";
    }
    public string[] getCaps()
    {
         return new string[]{"Chance: {0}% (Max. 50%) <br>Damage: +{1}", Mathf.Round(prob * 100).ToString(), dmg.ToString()};
    }

    public string getIcon()
    {
        return "ExplodeUnlock";
    }

    public string getText()
    {
        return "Explosion";
    }

    public string getType()
    {
        return "On-Kill Effect";
    }
    public GameObject getAbilityOptionMenu(){
        return null;
    }
}


public class Necromancer : OnKillEffects
{
    

    public static int AtkTimes = 3;
    public float prob;
    public float dmgPerc;
    public static Necromancer Instance;
    static IPoolable Prefab;
    static IPoolable PrefabMega;
    public float MegaGhoulProbability;

    public Necromancer(float prob, float dmgPerc){
        this.prob = prob;
        this.dmgPerc = dmgPerc;
        if(Instance == null){
            Instance = this;
            Prefab = Resources.Load<IPoolable>("Prefab/Ghoul");
            PrefabMega = Resources.Load<IPoolable>("Prefab/MegaGhoul");
        }else{
            Instance.Stack(this);
        }
    }
    public bool addList()
    {
        return this == Instance;
    }

    public void ApplyEffect(Vector2 pos)
    {
        
        if(Random.Range(0f,1f) < prob){
            if(MegaGhoulProbability > Random.Range(0f,1f)){
                ObjectPooling.Spawn(PrefabMega, new float[]{pos.x, pos.y});
                // Flamey.Instance.SpawnObject(PrefabMega).transform.position = pos;
            }else{
                ObjectPooling.Spawn(Prefab, new float[]{pos.x, pos.y});
                // Flamey.Instance.SpawnObject(Prefab).transform.position = pos;
            }
            
        }
    }
    public void Stack(Necromancer necromancer){
        prob += necromancer.prob;
        dmgPerc += necromancer.dmgPerc;
        RemoveUselessAugments();
    }
    private void RemoveUselessAugments(){
        if(prob >= .5f){
            prob = .5f;
            Deck deck = Deck.Instance;
            deck.removeClassFromDeck("NecroProb");
        } 
        
    }
    
    public string getDescription()
    {
        return "Everytime you kill an enemy, there's a chance of summoning a <color=#FFCC7C>friendly ghoul</color>. Ghouls can attack enemies for up to <color=#FFCC7C>3 times</color> with a percentage of your <color=#FF5858>base damage.";
    }
    public string[] getCaps()
    {
        return new string[]{"Chance: {0}% (Max. 50%) <br>Base Damage Ratio: {1}%", Mathf.Round(prob * 100).ToString(), Mathf.Round(dmgPerc * 100).ToString()};
    }

    public static int getAttackTimes(){
        
        switch(SkillTreeManager.Instance.getLevel("Necromancer")){
            case 1: return 5;
            case 2: return 7;
            case 0: default: return 3 ;
        }
    }
    public string getIcon()
    {
        return "NecroUnlock";
    }

    public string getText()
    {
        return "Necromancer";
    }

    public string getType()
    {
        return "On-Kill Effect";
    }
    public GameObject getAbilityOptionMenu(){
        return null;
    }
}


public class Bullets : OnKillEffects
{



    public float prob;
    public int dmg;
    public int amount;
    public static Bullets Instance;
    static IPoolable Prefab;

    public int EmbersInRound = 0;
    public Bullets(float prob, int dmg, int amount)
    {
        this.prob = prob;
        this.dmg = dmg;
        this.amount = amount;
        if (Instance == null)
        {
            Instance = this;
            Prefab = Resources.Load<GameObject>("Prefab/Bullet").GetComponent<IPoolable>();
            if (SkillTreeManager.Instance.getLevel("Pirate") >= 2) { this.amount *= 2; }
            Deck.RoundOver += ResetEmbersRound;
        }
        else
        {
            Instance.Stack(this);
        }
    }

    private void ResetEmbersRound(object sender, EventArgs e)
    {
        if (EmbersInRound >= 5000)
        {
            GameUI.Instance.CompleteQuestIfHasAndQueueDialogue(15, "Gyomyo", 15);
        }
        //Debug.Log("Embers: " + EmbersInRound);
        EmbersInRound = 0;

    }

    public bool addList()
    {
        return this == Instance;
    }

    public void ApplyEffect(Vector2 pos)
    {

        if (Random.Range(0f, 1f) < prob)
        {

            SpawnBullets(pos);
            Flamey.Instance.addEmbers(20);

        }
    }
    private void SpawnBullets(Vector2 pos)
    {
        AudioManager.PlayOneShot(FMODEvents.Instance.RoundShot, Vector2.zero);
        float randomRotation = Random.Range(0, 360);

        if (SkillTreeManager.Instance.getLevel("Pirate") >= 1)
        {

            for (int i = 0; i != amount; i++)
            {
                Enemy e = Enemy.getClosestEnemy(pos, i);
                if (e != null)
                {
                    randomRotation = Vector2.SignedAngle(Vector2.up, (Vector2)e.HitCenter.position - pos);
                }
                ObjectPooling.Spawn(Prefab, new float[] { pos.x, pos.y, randomRotation });
            }

        }
        else
        {
            for (int i = 0; i != amount; i++)
            {
                ObjectPooling.Spawn(Prefab, new float[] { pos.x, pos.y, i * (360 / amount) + randomRotation });
            }
        }



    }
    public void Stack(Bullets necromancer)
    {
        prob += necromancer.prob;
        dmg += necromancer.dmg;
        amount += necromancer.amount * (SkillTreeManager.Instance.getLevel("Pirate") >= 2 ? 2 : 1);
        RemoveUselessAugments();
    }
    private void RemoveUselessAugments()
    {
        if (prob >= .5f)
        {
            prob = .5f;
            Deck deck = Deck.Instance;
            deck.removeClassFromDeck("BulletsProb");
        }
        if (amount >= 6)
        {
            amount = 6;
            Deck deck = Deck.Instance;
            deck.removeClassFromDeck("BulletsAmount");
        }

    }

    public string getDescription()
    {
        return "Everytime you kill an enemy, there's a chance of shooting <color=#FFCC7C>Cannon Balls</color> out of the enemy's corpse, that deal damage and apply <color=#FF99F3>On-Hit effects</color> whenever they hit another creature. If this effect procs, you will also gain <color=#FFCC7C>+10 embers</color>. <color=#AFEDFF>Cannon Balls' speed</color> scales with <color=#AFEDFF>Bullet Speed";
    }
    public string[] getCaps()
    {
        return new string[] { "Chance: {0}% (Max. 50%) <br>Amount of Cannon Balls: {1} (Max. 6)<br>Damage: +{2}", Mathf.Round(prob * 100f).ToString(), amount.ToString(), dmg.ToString() };
    }

    public string getIcon()
    {
        return "BulletsUnlock";
    }

    public string getText()
    {
        return "Pirate";
    }

    public string getType()
    {
        return "On-Kill Effect";
    }
    public GameObject getAbilityOptionMenu()
    {
        return null;
    }
}

public class Smog : OnKillEffects
{
    public float prob;
    public float area;
    public int ticks;
    public static Smog Instance;

    public static IPoolable Prefab;
    public static IPoolable DrMiasmaSmog;

    //private Image cooldownImage;
    private float CooldownTimer = 10f;
    private Image cooldownImage;
    public Smog(float prob, float area, int ticks)
    {
        this.prob = prob;
        this.area = area;
        this.ticks = ticks;
        if (Instance == null)
        {
            Instance = this;
            Prefab = Resources.Load<GameObject>("Prefab/SmogOnDeath").GetComponent<IPoolable>();
            DrMiasmaSmog = Resources.Load<GameObject>("Prefab/SmogDrMiasma").GetComponent<IPoolable>();
            cooldownImage = GameUI.Instance.SpawnUIMetric(Resources.Load<Sprite>("Icons/PoisonSpread"));
            cooldownImage.fillAmount = 1f;
        }
        else
        {
            Instance.Stack(this);


        }
    }
    public bool addList()
    {
        return this == Instance;
    }

    public void ApplyEffect(Vector2 pos)
    {

        if (Random.Range(0f, 1f) < prob)
        {

            float scale = area * 2f / 100f;
            Collider2D[] targets = Physics2D.OverlapCircleAll(pos, 2f * scale, Flamey.EnemyMask);
            ObjectPooling.Spawn(Prefab, new float[] { pos.x, pos.y, scale });


            foreach (Collider2D col in targets)
            {
                col.GetComponent<Enemy>().Poison(ticks);
            }

        }
    }

    public void Stack(Smog vampOnDeath)
    {
        prob += vampOnDeath.prob;
        area += vampOnDeath.area;
        ticks += vampOnDeath.ticks;
        RemoveUselessAugments();
    }
    bool OnDrMiasma = false;
    public void ActivateDrMiasma()
    {
        if (cooldownImage.fillAmount >= 1f && !OnDrMiasma)
        {
            OnDrMiasma = true;
            cooldownImage.fillAmount = 0f;

            ObjectPooling.Spawn(Prefab, new float[] { 0, 0, 1 });
            Flamey.Instance.StartCoroutine(DrMiasma());

            Vector2 pos = Vector2.zero;
            float scale = area * 2f / 100f;
            Collider2D[] targets = Physics2D.OverlapCircleAll(pos, 2f * scale, Flamey.EnemyMask);

            ObjectPooling.Spawn(DrMiasmaSmog, new float[] { pos.x, pos.y, scale });
            foreach (Collider2D col in targets)
            {
                col.GetComponent<Enemy>().Poison(ticks);
            }
        }

    }
    private IEnumerator DrMiasma()
    {
        Flamey.Instance.Unhittable = true;
        yield return new WaitForSeconds(5f);
        Flamey.Instance.Unhittable = false;

        while (cooldownImage.fillAmount < 1f)
        {
            cooldownImage.fillAmount += 0.25f * (1f / CooldownTimer);
            yield return new WaitForSeconds(0.25f);
        }
        OnDrMiasma = false;
    }
    private void RemoveUselessAugments()
    {
        if (prob >= 1f)
        {
            prob = 1f;
            Deck deck = Deck.Instance;
            deck.removeClassFromDeck("SmogProb");
        }
        if ((area >= 100f && SkillTreeManager.Instance.getLevel("Smog") >= 1) || (area >= 50f && SkillTreeManager.Instance.getLevel("Smog") < 1))
        {
            area = SkillTreeManager.Instance.getLevel("Smog") >= 1 ? 100f : 50f;
            Deck deck = Deck.Instance;
            deck.removeClassFromDeck("SmogArea");
        }
        if (ticks >= 25)
        {
            ticks = 25;
            Deck deck = Deck.Instance;
            deck.removeClassFromDeck("SmogTicks");
        }

    }

    public string getDescription()
    {
        return "Everytime you kill an enemy, there's a chance of generating a <color=#FFCC7C>massive explosion</color> that <color=#FF5858>damages</color> nearby enemies";
    }
    public string[] getCaps()
    {
        if (SkillTreeManager.Instance.getLevel("Smog") >= 1)
        {

            return new string[] { "Chance: {0}% (Max. 100%) <br>Area: +{1} (Max. 100) <br>Poison Ticks: +{2} (Max. 25)", Mathf.Round(prob * 100).ToString(), Mathf.Round(area).ToString(), ticks.ToString() };
        }
        else
        {
            return new string[] { "Chance: {0}% (Max. 100%) <br>Area: +{1} (Max. 50) <br>Poison Ticks: +{2} (Max. 25)", Mathf.Round(prob * 100).ToString(), Mathf.Round(area).ToString(), ticks.ToString() };
        }
    }

    public string getIcon()
    {
        return "PoisonUnlock";
    }

    public string getText()
    {
        return "Smog";
    }

    public string getType()
    {
        return "On-Kill Effect";
    }
    public GameObject getAbilityOptionMenu()
    {
        return null;
    }
}

public class Gravity : OnKillEffects
{
    public float prob;
    public float force;
    public static Gravity Instance;
    
    public static IPoolable Prefab;
    float radius = 1f;
    
    public int currentTargetingOption = 0;
    public GameObject optionMenu;
    
    public Gravity(float prob, float force)
    {
        this.prob = prob;
        this.force = force;
        if (Instance == null)
        {
            Prefab = Resources.Load<GameObject>("Prefab/BlackHole").GetComponent<IPoolable>();
            Instance = this;
            if (SkillTreeManager.Instance.getLevel("Gravity") >= 1)
            {

                radius = 2f;
            }
            else
            {
                // Prefab = Resources.Load<GameObject>("Prefab/ExplosionOnDeath").GetComponent<IPoolable>();
                radius = 1f;
            }
            currentTargetingOption = Math.Max(0, PlayerPrefs.GetInt("BlackHoleTargetingOption", -1));
            optionMenu = GameUI.Instance.AbilityOptionContainer.transform.Find("Blackhole").gameObject;

        }
        else
        {
            Instance.Stack(this);
        }
    }
    public bool addList()
    {
        return this == Instance;
    }

    public void ApplyEffect(Vector2 pos)
    {
        void applyKB(Enemy en)
        {
            switch (currentTargetingOption)
            {
                case 0:
                    en.KnockBack(pos, retracting:false, force);
                    break;
                case 1:
                    en.KnockBack(pos, retracting:true, force, stopOnOrigin:true);
                    break;
                case 2:
                    en.KnockBack(Flamey.Instance.getPosition(), retracting:false, force);
                    break;
                case 3:
                    en.KnockBack(Flamey.Instance.getPosition(), retracting:true, force, stopOnOrigin:true);
                    break;
                default:
                    break;
            }
        }
        if(Random.Range(0f,1f) < prob){
            

            Collider2D[] targets = Physics2D.OverlapCircleAll(pos, radius, Flamey.EnemyMask);
            ObjectPooling.Spawn(Prefab, new float[]{pos.x, pos.y});
            // AudioManager.PlayOneShot(FMODEvents.Instance.Explosion, Vector2.zero);

            foreach(Collider2D col in targets){
                applyKB(col.GetComponent<Enemy>());
            }
            
        }
    }

    public void Stack(Gravity gravity){
        prob += gravity.prob;
        force += gravity.force;
        RemoveUselessAugments();
    }
    private void RemoveUselessAugments()
    {
        if (prob >= .5f)
        {
            prob = .5f;
            Deck deck = Deck.Instance;
            deck.removeClassFromDeck("GravityProb");
        } 
        if (force >= 3f)
        {
            force = 3f;
            Deck deck = Deck.Instance;
            deck.removeClassFromDeck("GravityForce");
        }
        
    }
   
    public string getDescription()
    {
        return "Everytime you kill an enemy, there's a chance of generating a <color=#FFCC7C>massive explosion</color> that <color=#FF5858>damages</color> nearby enemies";
    }
    public string[] getCaps()
    {
         return new string[]{"Chance: {0}% (Max. 50%) <br>Force: {1}N (max. 300N)", Mathf.Round(prob * 100).ToString(), Mathf.Round(force * 100f).ToString()};
    }

    public string getIcon()
    {
        return "ExplodeUnlock";
    }

    public string getText()
    {
        return "Explosion";
    }

    public string getType()
    {
        return "On-Kill Effect";
    }
    public GameObject getAbilityOptionMenu(){
        return SkillTreeManager.Instance.getLevel("Gravity") >= 2 ? optionMenu : null;
    }
}

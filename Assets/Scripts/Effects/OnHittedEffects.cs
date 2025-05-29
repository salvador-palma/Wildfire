using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Loading;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public interface OnHittedEffects : Effect
{
    public bool addList();
    public void ApplyEffect(Enemy en = null, Hittable hitted = null, int dmg = -1);
}

public class ThornsOnHitted : OnHittedEffects
{
    public static ThornsOnHitted Instance;

    public float perc;
    public float prob;

    private Button activeCooldownImage;
    private int activeRoundsLeft;
    private int activeRoundsCooldown = 1;

    private IPoolable ThornsPrefab;

    public ThornsOnHitted(float prob, float perc){
        
        this.prob = prob;
        this.perc = perc;
        if(Instance == null){
            Instance = this;
            ThornsPrefab = Resources.Load<GameObject>("Prefab/Thorn").GetComponent<IPoolable>();
        }else{
            Instance.Stack(this);
            
        }
    }

    public bool addList()
    {
        return Instance == this;
    }

    public void ApplyEffect(Enemy en = null, Hittable hitted = null, int dmg = -1)
    {
        if(en==null){return;}

        if(Random.Range(0f,1f) < prob){
            if(Flamey.Instance.Armor == 0){return;}
            if(SkillTreeManager.Instance.getLevel("Thorns")>=2){
                Enemy[] targets = Physics2D.OverlapCircleAll(en.HitCenter.position, 0.5f, Flamey.EnemyMask).Select(e => e.GetComponent<Enemy>()).ToArray();
                SpawnThorn(en.HitCenter.position, 2);
                foreach(Enemy enemy in targets){
                    enemy.Hitted((int)(Flamey.Instance.Armor * perc), 10, ignoreArmor: false, onHit: SkillTreeManager.Instance.getLevel("Thorns")>=1, source:"Thorns", extraInfo: new float[]{dmg});
                }
            }else{
                en.Hitted((int)(Flamey.Instance.Armor * perc), 10, ignoreArmor: false, onHit: SkillTreeManager.Instance.getLevel("Thorns")>=1, source:"Thorns", extraInfo: new float[]{dmg});
                SpawnThorn(en.HitCenter.position,1);
            }
            
        }

    }
    private void SpawnThorn(Vector2 pos, int type){
        if((int)(Flamey.Instance.Armor * perc)>0){
            AudioManager.PlayOneShot(FMODEvents.Instance.ThornsSlash, Vector2.zero);
            ObjectPooling.Spawn(ThornsPrefab, new float[]{pos.x, pos.y - .4f, type});
        }
        
    }
    public void Stack(ThornsOnHitted thornsOnHitted){
        
        prob += thornsOnHitted.prob;
        perc += thornsOnHitted.perc;
        RemoveUselessAugments();
    }
    public bool maxed;
    private void RemoveUselessAugments(){
        if(prob >= 1){
            prob = 1f;
            Deck deck = Deck.Instance;
            deck.removeClassFromDeck("ThornsProb");
        }      
        
    }


    public void SpawnExtraAssets(){
        activeCooldownImage = GameUI.Instance.SpawnUIActiveMetric(Resources.Load<Sprite>("Icons/ThornsUnlock"));
        activeCooldownImage.transform.GetChild(0).GetComponent<Image>().fillAmount = 1;
        Deck.RoundOver += UpdateActive;
        activeCooldownImage.onClick.AddListener(() => {
            
            Flamey.Instance.Unhittable = true;
            Flamey.Instance.GetComponent<Animator>().SetBool("ThornsArmor", true);
            
            Flamey.Instance.callFunctionAfter(() =>{Flamey.Instance.GetComponent<Animator>().SetBool("ThornsArmor", false);Flamey.Instance.Unhittable = false;}, 30f);
            activeCooldownImage.interactable = false;
            activeRoundsLeft = 0;
            activeCooldownImage.transform.GetChild(0).GetComponent<Image>().fillAmount = 0;

        });
    }
    public void ResetInstance(){
        Instance = null;
        Deck.RoundOver -= UpdateActive;
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
    public string getDescription()
    {
        return "Everytime you get hit by an enemy you have a chance of returning a percentage of your <color=#919191>Armor</color> as <color=#FF5858>damage</color> back and applying <color=#FF99F3>On-Hit Effects";
    }
    public string[] getCaps()
    {
        return new string[]{"Chance: {0}% (Max. 100%) <br>Armor to Damage Percentage: {1}%", Mathf.Round(prob*100f).ToString(), Mathf.Round(perc*100f).ToString()};
    }

    public string getIcon()
    {
        return "ThornsUnlock";
    }

    public string getText()
    {
        return "Thorns";
    }

    public string getType()
    {
        return "Counter Effect";
    }
    public GameObject getAbilityOptionMenu(){
        return null;
    }
}


public class Earthquake : OnHittedEffects
{
    public static Earthquake Instance;

    public float prob;
    public float force;

    private IPoolable EarthquakePrefab;
    private IPoolable MegaEarthquakePrefab;
    
    private float CooldownTimer = 10f;
    private Image cooldownImage;

    public Earthquake(float prob, float force)
    {

        this.prob = prob;
        this.force = force;
        if (Instance == null)
        {
            Instance = this;
            EarthquakePrefab = Resources.Load<GameObject>("Prefab/Earthquake").GetComponent<IPoolable>();
            MegaEarthquakePrefab = Resources.Load<GameObject>("Prefab/EarthquakeMalph").GetComponent<IPoolable>();
            cooldownImage = GameUI.Instance.SpawnUIMetric(Resources.Load<Sprite>("Icons/Gravity"));
            cooldownImage.fillAmount = 1f;
        }
        else
        {
            Instance.Stack(this);

        }
    }

    public bool addList()
    {
        return Instance == this;
    }
    
    public void ApplyEffect(Enemy en = null, Hittable hitted = null, int dmg = -1)
    {
        
        if (en == null) { return; }

        if (Random.Range(0f, 1f) < prob)
        {


            if (Character.Instance.isCharacter("Earthquake"))
            {
                if (hitted.Equals(Flamey.Instance)) { Flamey.Instance.GetComponent<Animator>().Play("GolemHit"); }
                SpawnEarthquake(hitted.getPosition(), en, hitted);

            }
            else
            {
                SpawnEarthquake(hitted.getPosition(), en, hitted);
            }
           
            
        }

    }
    
    private void SpawnEarthquake(Vector2 pos, Enemy en, Hittable hittable, float radius = 2.5f)
    {
        void applyKB(Enemy en)
        {
            en.KnockBack(hittable.getPosition(), retracting: false, force);
            if (SkillTreeManager.Instance.getLevel("Earthquake") >= 1)
            {
                en.Stun(2.5f);
            }
        }

        ObjectPooling.Spawn(EarthquakePrefab, new float[] { pos.x, pos.y });

        Enemy[] targets = Physics2D.OverlapCircleAll(hittable.getPosition(), radius, Flamey.EnemyMask).Select(e => e.GetComponent<Enemy>()).ToArray();
        bool level2 = SkillTreeManager.Instance.getLevel("Earthquake") >= 2;
        foreach (Enemy enemy in targets)
        {
            if (enemy == null || !enemy.canTarget()) { continue; }
            if (!level2)
            {
                if (enemy == en)
                {
                    applyKB(enemy);
                }
            }
            else
            {
                applyKB(enemy);
            }

        }


    }
    public void Stack(Earthquake thornsOnHitted){
        
        prob += thornsOnHitted.prob;
        force += thornsOnHitted.force;
        RemoveUselessAugments();
    }
    public bool maxed;
    private void RemoveUselessAugments()
    {
        if (prob >= 1f)
        {
            prob = 1f;
            Deck deck = Deck.Instance;
            deck.removeClassFromDeck("EarthquakeProb");
        }      
        if(force >= 5f){
            force = 5f;
            Deck deck = Deck.Instance;
            deck.removeClassFromDeck("EarthquakeForce");
        }    
        
    }

    public bool OnAbilityCD = false;
    public void ActivateMegaEarthquake(Enemy en)
    {
        if (cooldownImage.fillAmount >= 1f && !OnAbilityCD)
        {
            OnAbilityCD = true;
            cooldownImage.fillAmount = 0f;
            Flamey.Instance.GetComponent<Animator>().Play("GolemHit");

            Vector2 pos = Flamey.Instance.getPosition();
            void applyKB(Enemy en)
            {
                en.KnockBack(pos, retracting: false, force);
                if (SkillTreeManager.Instance.getLevel("Earthquake") >= 1)
                {
                    en.Stun(2.5f);
                }
            }


            ObjectPooling.Spawn(MegaEarthquakePrefab, new float[] { pos.x, pos.y });

            Enemy[] targets = Physics2D.OverlapCircleAll(pos, 4f, Flamey.EnemyMask).Select(e => e.GetComponent<Enemy>()).ToArray();

            bool level2 = SkillTreeManager.Instance.getLevel("Earthquake") >= 2;
            foreach (Enemy enemy in targets)
            {
                if (enemy == null || !enemy.canTarget()) { continue; }
                if (!level2)
                {
                    if (enemy == en)
                    {
                        applyKB(enemy);
                    }
                }
                else
                {
                    applyKB(enemy);
                }

            }
            
            Flamey.Instance.StartCoroutine(WaitAbilityCD());
        }

    }
    private IEnumerator WaitAbilityCD()
    {
        while (cooldownImage.fillAmount < 1f)
        {
            cooldownImage.fillAmount += 0.25f * (1f / CooldownTimer);
            yield return new WaitForSeconds(0.25f);
        }
        OnAbilityCD = false;
    }

    public string getDescription()
    {
        return "Everytime you get hit by an enemy you have a chance of returning a percentage of your <color=#919191>Armor</color> as <color=#FF5858>damage</color> back and applying <color=#FF99F3>On-Hit Effects";
    }
    public string[] getCaps()
    {
        return new string[]{"Chance: {0}% (Max. 100%) <br>Force: {1}N (max. 500N)", Mathf.Round(prob*100f).ToString(), Mathf.Round(force * 100f).ToString()};
    }

    public string getIcon()
    {
        return "Gravity";
    }

    public string getText()
    {
        return "Earthquake";
    }

    public string getType()
    {
        return "Counter Effect";
    }
    public GameObject getAbilityOptionMenu(){
        return null;
    }
}
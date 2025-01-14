using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public interface OnHittedEffects : Effect
{
    public bool addList();
    public void ApplyEffect(Enemy en = null);
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

    public void ApplyEffect(Enemy en = null)
    {
        if(en==null){return;}

        if(Random.Range(0f,1f) < prob){
            if(Flamey.Instance.Armor == 0){return;}
            if(SkillTreeManager.Instance.getLevel("Thorns")>=2){
                Enemy[] targets = Physics2D.OverlapCircleAll(en.HitCenter.position, 0.5f, Flamey.EnemyMask).Select(e => e.GetComponent<Enemy>()).ToArray();
                SpawnThorn(en.HitCenter.position, 2);
                foreach(Enemy enemy in targets){
                    enemy.Hitted((int)(Flamey.Instance.Armor * perc), 10, ignoreArmor: false, onHit: SkillTreeManager.Instance.getLevel("Thorns")>=1);
                }
            }else{
                en.Hitted((int)(Flamey.Instance.Armor * perc), 10, ignoreArmor: false, onHit: SkillTreeManager.Instance.getLevel("Thorns")>=1);
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
        if(!maxed){CheckMaxed();}
    }

    private void CheckMaxed(){
        if(prob >= 1f && !Character.Instance.isACharacter()){
            Character.Instance.SetupCharacter("Thorns");
            maxed = true;
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
    public string getCaps()
    {
        return string.Format("Chance: {0}% (Max. 100%) <br>Armor to Damage Percentage: {1}%", Mathf.Round(prob*100f), Mathf.Round(perc*100f));
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
        return "On-Hitted Effect";
    }
    public GameObject getAbilityOptionMenu(){
        return null;
    }
}

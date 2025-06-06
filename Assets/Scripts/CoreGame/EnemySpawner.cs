using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EnemySpawner : MonoBehaviour
{
    
    public static EnemySpawner Instance {get; private set;}
    float height;
    float width;
    
    public bool isOn = true;
    [HideInInspector] public bool isOnAugments = false;

    

    public int current_round = 0;

    float EnemyAmount;
    float RoundDuration;
    float TimerEnemySpawn;
    float TimerEnemySpawnCounter;

    [SerializeField] public IPoolable ExplosionPrefab;
    [SerializeField] public GameObject ExplosionGhoulPrefab;
    
    public bool GameEnd = true;

    public static Dictionary<string, int> DeathPerEnemy;
    
    List<List<float>> ProbabiltyList = new List<List<float>>(){
        new List<float>(){1,0,0},
        new List<float>(){0.85f,.15f,0},
        new List<float>(){0.6f,0.4f,0},
        new List<float>(){0.5f,0.5f},
        new List<float>(){0.5f,0.5f},

        new List<float>(){0,.15f,.85f},
        new List<float>(){0.15f,.15f, 0.7f},
        new List<float>(){0.1f,0.3f,0.6f},
        new List<float>(){0.25f,0.25f, 0.5f},
        new List<float>(){0.3f, 0.3f, 0.4f},
    };

    public Enemy[] PickedEnemies;
    public Boss[] PickedBosses;
    public List<Enemy> PresentEnemies;

    public bool Paused;

    //ENEMY PREVIEWER
    [Header("Binoculars")]
    public int HindSightDeepness;
    public string latestSpecies;
    public GameObject BinocularPanel;
    public GameObject[] BinocularSlots;


    private void Awake() {
        Instance = this;
        PresentEnemies = new List<Enemy>();
        DeathPerEnemy = new Dictionary<string, int>();
        resetInstances();
    }
    public void Start(){
        
        GameEnd =true;
        Flamey.Instance.GameEnd = true;
        
        HindSightDeepness = Math.Max(0,GameVariables.GetVariable("BinocularLevel"));
        if(HindSightDeepness == 0){
            BinocularPanel.SetActive(false);
        }
        SetSpawnLimits();
        StartRound();

        
        
        
    }
    
    public void StartGame(){ 
        Flamey.Instance.GameEnd = false;    
        if(PlayerPrefs.GetInt("PlayerLoad", 0) == 0){ 
            Deck.Instance.LoadGame(false);
            PickedEnemies = pickEnemies();
            PickedBosses = pickBosses();
            GameEnd = false;
            InitDefaultEffects();
            
        }else{
            Debug.Log("There will be errors here");
            InitDefaultEffects();
            Deck.Instance.LoadGame(true);
            newRound();
        }
        
        InitBinoculars();
        PlayerPrefs.DeleteKey("PlayerLoad");    
    }
    private void InitDefaultEffects(){
        
        if(SkillTreeManager.Instance.getLevel("Ember Generation") >= 0 && MoneyMultipliers.Instance==null){
            Flamey.Instance.addNotEspecificEffect(new MoneyMultipliers(0, 1));
        }
        if(SkillTreeManager.Instance.getLevel("Gambling") >= 0 && Gambling.Instance==null){
            Flamey.Instance.addNotEspecificEffect(new Gambling());
        }

        foreach (Skills skill in SkillTreeManager.Instance.PlayerData.skills)
        {
            if(skill.type=="Ember Generation" || skill.type=="Gambling"){continue;}
                
            //DeckBuilder.Instance.GetAugmentsFromClasses(new List<string>{skill.type}, inPool:true).ForEach(a=>a.action());
            if(skill.ban){
                DeckBuilder.Instance.GetAugmentsFromClasses(new List<string>{skill.type}, inPool:true).ForEach(a=>Deck.Instance.removeClassFromDeck(a?.AugmentClass));
            }else if(skill.pick){
                DeckBuilder.Instance.GetAugmentsFromClasses(new List<string>{skill.type}, inPool:true).ForEach(a=>a.action());
            }
        }

        GameUI.Instance.defineEffectList();
    }
    private Vector2 getPoint(){
        double angle = Math.PI * (float)Distribuitons.RandomUniform(0,360)/180f;
        double x = 0.52f * width * Math.Cos(angle);
        double y = 0.52f * height * Math.Sin(angle);
        return new Vector2((float)x,(float)y);
    }
    public Vector2 getPointAngle(double angle, float radius = 0.52f){
        angle = Math.PI * (float)angle/180f;
        double x = radius * width * Math.Cos(angle);
        double y = radius * height * Math.Sin(angle);
        return new Vector2((float)x,(float)y);
    }

    private void Update()
    {

        if (GameEnd) { return; }
        UpdateEnemies();
        if (!isOn)
        {

            if (GameObject.FindGameObjectWithTag("Enemy") == null && !isOnAugments)
            {

                if (current_round == 59)
                {//6 AM
                    GameUI.Instance.ShowLimitRoundPanel();
                    SIXAM();
                }
                else
                {
                    isOnAugments = true;
                    Deck.Instance.StartAugments((current_round + 1) % 5 == 0);
                    LogNewRound();
                }

                Flamey.Instance.poisonsLeft = 0;
            }
            return;
        }
        if (new int[] { 20, 40, 60 }.Contains(current_round))
        {
            int id = current_round / 20 - 1;
            Boss spawnable = PickedBosses[id];
            Instantiate(spawnable);

            isOn = false;
            return;
        }

        if (TimerEnemySpawnCounter > 0)
        {
            TimerEnemySpawnCounter -= Time.deltaTime;
        }
        else
        {

            TimerEnemySpawnCounter = TimerEnemySpawn * (1 / Gambling.getGambleMultiplier(3));
            SpawnEnemy(PickRandomEnemy(current_round));
            EnemyAmount--;
            if (EnemyAmount <= 0)
            {
                isOn = false;
            }
        }
    }

    private void SIXAM()
    {
        if(FlameCircle.Instance != null){
            GameUI.Instance.CompleteQuestIfHasAndQueueDialogue(38,"Betsy",17); //EARTH UNLOCK
        }


        if(RoundsBelow50PercMaxHP >= 59 && HealthRegen.Instance != null){
            Debug.Log("UNLOCK PHEONIX");
            GameUI.Instance.CompleteQuestIfHasAndQueueDialogue(30,"Betsy",22); //PHEONIX UNLOCK
        }
    }

    public void UpdateEnemies(){
        PresentEnemies.ForEach(e => {if(e!=null && !e.Attacking){e.UpdateEnemy(); e.ApplySlowUpdate();}});
        List<Enemy> deadEnemies = PresentEnemies.Where(e => e==null || e.Health <= 0).ToList();
        foreach(Enemy enemy in deadEnemies){
            PresentEnemies.Remove(enemy);
            enemy.Die();
        }
    }
    public void ApplyPoisonEnemies(){
        PresentEnemies.ForEach(e => e.ApplyPoison());
    }
    public void addEnemy(Enemy enemy){PresentEnemies.Add(enemy);}
    public float ShinyChance = 0f;
    public float ShinyMultiplier = 10f;
    public void SpawnEnemy(GameObject enemy){

        GameObject g = Instantiate(enemy);
        Enemy e = g.GetComponent<Enemy>();
        addEnemy(e);
        CheckForBinoculars(e);
        g.transform.position = getPoint();
        e.CheckFlip();

        
        
        if(UnityEngine.Random.Range(0f,1f) < ShinyChance){
            e.Shiny = true;
            g.GetComponent<Renderer>().material = LocalBestiary.INSTANCE.getShinyMaterial(enemy);
        }
        
        
    }
    private void SetSpawnLimits(){
        height = 2f * Camera.main.orthographicSize;
        width = height * Camera.main.aspect;
    }
    

    public void newRound(){
        
        current_round++;

        isOn = true;
        isOnAugments = false;
        GameEnd = false; 

        Flamey.Instance.notEspecificEffects.ForEach(effect => effect.ApplyEffect());
        Flamey.Instance.ApplyTimedRound();

        if(Character.Instance.isCharacter("Multicaster")){
            Flamey.Instance.addAttackSpeed(0.2f);
        }

        StartRound();
        
    }
    private void StartRound(){

        EnemyAmount = getSpawnAmount(current_round);
        RoundDuration = getRoundTime(current_round);
        TimerEnemySpawn = RoundDuration/EnemyAmount;
        GameUI.Instance.UpdateProgressBar(current_round); 
        GameUI.Instance.UpdateMenuInfo(current_round); 
    }
    
    /* ===== BINOCULARS ===== */
    private void CheckForBinoculars(Enemy e){
        if(latestSpecies==null){latestSpecies=e.Name; return;}

        int start_from = Array.FindIndex(PickedEnemies, en => e.Name == en.Name);
        try{
            int latest = Array.FindIndex(PickedEnemies, en => latestSpecies == en.Name);
            if(e==null || PickedEnemies == null){return;}
            if( e.Name != latestSpecies){
                if(start_from > latest){
                    
                    latestSpecies = e.Name;
                    IncrementBinocularHindSight();
                }
                
        }
        } catch(Exception ex){
            Debug.Log("Found: " + ex.Message);
        }

        void IncrementBinocularHindSight(){
            
            Debug.Log("Increment Binocular");
            int j = 0;
            for (int i = start_from; i < start_from+HindSightDeepness; i++)
            {
                try{
                    GameObject child = BinocularSlots[j];
                    child.GetComponent<Image>().sprite = PickedEnemies[i+1].GetComponent<SpriteRenderer>().sprite;
                    float[] dimensions = LocalBestiary.INSTANCE.getMeasurements(PickedEnemies[i+1]);
                    ResizeImage(child.GetComponent<RectTransform>(), new Vector2(dimensions[0], dimensions[1]), new Vector2(dimensions[2], dimensions[3]));
                    j++;
                }catch(IndexOutOfRangeException e){
                    Debug.Log("Increment Ignore");
                }
            }
            LocalBestiary.INSTANCE.UpdateSlots();
        }
        void ResizeImage(RectTransform RT, Vector2 IconPos, Vector2 IconSize){
            RT.anchoredPosition = IconPos;
            RT.sizeDelta = IconSize;
        }
    }
    private void InitBinoculars(){

        for (int i = 0; i < HindSightDeepness; i++)
        {
            GameObject child = BinocularSlots[i];
            child.GetComponent<Image>().sprite = PickedEnemies[i+1].gameObject.GetComponent<SpriteRenderer>().sprite;
            if(!LocalBestiary.INSTANCE.hasBeenUnlocked(PickedEnemies[i+1])){
                child.GetComponent<Image>().color = Color.black;
            }
            float[] dimensions = LocalBestiary.INSTANCE.getMeasurements(PickedEnemies[i+1]);
            child.GetComponent<RectTransform>().anchoredPosition = new Vector2(dimensions[0], dimensions[1]);
            child.GetComponent<RectTransform>().sizeDelta = new Vector2(dimensions[2], dimensions[3]);

        }
    }
    /* ===== ENEMY PICK ===== */
    private GameObject PickRandomEnemy(int round){
        if(PickedEnemies==null){
            Debug.Log("error found");
        }
        if (round >= 60)
            return PickedEnemies[UnityEngine.Random.Range(0, PickedEnemies.Length)].gameObject;
        int picked = pickEnemyIndex(ProbabiltyList[round % 10]) + (3*(round/10));
       
        return PickedEnemies[picked].gameObject;
        
    }
    private int pickEnemyIndex(List<float> prob){
        float val = UnityEngine.Random.Range(0f,1f);
        for(int i = 0 ; i< prob.Count; i++){
            if(prob[i] > val){
                return i;
            }else{
                val -= prob[i];
            }
        }
        return prob.Count - 1;
    }

    /* ===== ROUND SETTINGS ===== */
    private float getRoundTime(int round){return Math.Min(5 + 1.2f * round, 40);}
    private float getSpawnAmount(int round){return 5*(round%10)+25*(round/10)+5;}

    private void resetInstances()
    {

        // if(FlameCircle.Instance != null){
        //     FlameCircle.Instance.ResetInstance();
        // }

        FlameCircle.Instance = null;

        MoneyMultipliers.Instance = null;
        CandleTurrets.Instance = null;
        Summoner.Instance = null;

        if (Gambling.Instance != null)
        {
            Gambling.Instance.ResetInstance();
        }
        Gambling.Instance = null;

        if (ThornsOnHitted.Instance != null)
        {
            ThornsOnHitted.Instance.ResetInstance();
        }
        //ThornsOnHitted.Instance = null;

        Explosion.Instance = null;
        Necromancer.Instance = null;
        Bullets.Instance = null;
        Smog.Instance = null;

        VampOnHit.Instance = null;
        if (IceOnHit.Instance != null)
        {
            IceOnHit.Instance.ResetInstance();
        }
        //IceOnHit.Instance = null;
        ShredOnHit.Instance = null;
        ExecuteOnHit.Instance = null;
        StatikOnHit.Instance = null;

        BurnOnLand.Instance = null;
        IceOnLand.Instance = null;
        DrainOnLand.Instance = null;
        Whirpool.Instance = null;

        SecondShot.Instance = null;
        BurstShot.Instance = null;
        if (KrakenSlayer.Instance != null)
        {
            KrakenSlayer.Instance.ResetInstance();
        }
        //KrakenSlayer.Instance = null;
        CritUnlock.Instance = null;


        if (HealthRegen.Instance != null)
        {
            HealthRegen.Instance.ResetInstance();
        }
        // HealthRegen.Instance = null;
        LightningEffect.Instance = null;
        Immolate.Instance = null;
        Laser.Instance = null;

    }

    private Enemy[] pickEnemies(){
        List<Enemy> result = new List<Enemy>();
        for (int i = 0; i < 6; i++)
        {
            result.AddRange(LocalBestiary.INSTANCE.getRandomEnemyCombination(i+1, 3, exclude_banned:true, exclude_elliptical:false));//Character.Instance.isCharacter("Totem")
        }

        Deck.Instance.gameState.EnemyIDs = LocalBestiary.INSTANCE.getEnemiesID(result.ToArray());
        return result.ToArray();
    }
    
    private Boss[] pickBosses(){
        List<Boss> result = new List<Boss>();
        
        List<BossRunTimeData> possible = LocalBestiary.INSTANCE.Bosses.Where(e => e.wave == BossPhase.BEGGINER).ToList();
        result.Add(possible[UnityEngine.Random.Range(0, possible.Count())].boss);
        possible = LocalBestiary.INSTANCE.Bosses.Where(e => e.wave == BossPhase.ADVANCED).ToList();
        result.Add(possible[UnityEngine.Random.Range(0, possible.Count())].boss);
        possible = LocalBestiary.INSTANCE.Bosses.Where(e => e.wave == BossPhase.MASTER).ToList();
        result.Add(possible[UnityEngine.Random.Range(0, possible.Count())].boss);
        
        return result.ToArray();
    }


    //==== DEATH COUNTER ==== //
    public static void AddDeath(string enemy_name, bool shiny = false)
    {
        if (DeathPerEnemy.ContainsKey(enemy_name))
        {
            DeathPerEnemy[enemy_name]++;
        }
        else
        {
            DeathPerEnemy[enemy_name] = 1;
        }

        string og = enemy_name.Contains("Shiny") ? enemy_name.Replace("Shiny", "") : enemy_name;

        if (LocalBestiary.INSTANCE.getMilestoneAmount(og) < 0)
        {
            Debug.Log("Bestiary Doesn't have: " + og);

            LocalBestiary.INSTANCE.UnlockView(og);

            Debug.Log("Unlocked: " + og);
            LocalBestiary.INSTANCE.UpdateSlots();
            Debug.Log("Slots Updated");
        }
        if (shiny && LocalBestiary.INSTANCE.getMilestoneAmountShiny(og) < 0)
        {

            Debug.Log("Unlocking Shiny: " + enemy_name + " : " + shiny);
            LocalBestiary.INSTANCE.UnlockShiny(og);
            LocalBestiary.INSTANCE.UpdateSlots();
        }

        if (shiny)
        {
            print("Shinies counting");


            int shinies = 0;
            foreach (AnimalSaveData a in LocalBestiary.INSTANCE.saved_milestones.animals)
            {
                if (a.ShinyCaptured >= 0) { shinies++; }
            }
            print("Shinies: " + shinies);

            if (shinies == 1 && GameVariables.GetVariable("ShinyTalk") == -1)
            {
                NPC.QueueDialogue("Betsy", 8);
                GameVariables.SetVariable("ShinyTalk", 0);
            }
            if (GameVariables.hasQuest(31))
            {

                if (shinies >= 10)
                {
                    GameUI.Instance.CompleteQuestIfHasAndQueueDialogue(31, "Gyomyo", 9);
                }
            }
        }



    }



    // ===== ROUNDS BASED ACHIEVEMENTS/QUESTS ===== //
    public int RoundsWithoutDamage = 0;
    public int RoundsBelow25PercMaxHP = 0;
    public int RoundsBelow50PercMaxHP = 0;
    private void LogNewRound(){
        //MARS
        RoundsWithoutDamage++;
        RoundsBelow50PercMaxHP++;

        if(RoundsWithoutDamage >= 30 && FlameCircle.Instance != null){
            GameUI.Instance.CompleteQuestIfHasAndQueueDialogue(42,"Betsy",20); //URANUS UNLOCK
        }

        if(Flamey.Instance.Health <= Flamey.Instance.MaxHealth/4f){
            RoundsBelow25PercMaxHP++;
            if(RoundsBelow25PercMaxHP >= 10 && FlameCircle.Instance != null){
                GameUI.Instance.CompleteQuestIfHasAndQueueDialogue(39,"Betsy",18); //MARS UNLOCK
            }
        }else{RoundsBelow25PercMaxHP=0;}

        
        
        
    }

    
}

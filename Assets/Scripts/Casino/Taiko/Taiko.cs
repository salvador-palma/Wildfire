using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FMOD.Studio;
using FMODUnity;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class TimingPoint{
    public int Timing;
    public bool Kiai;
    public int RealtTime;
    public TimingPoint(int timing, bool kiai, int realTime){
        Timing = timing;
        Kiai = kiai;
        RealtTime = realTime;
    }
}
public class Taiko : MonoBehaviour
{
   
    private List<Drum> drums;
    private List<TimingPoint> timingPoints;
    List<int> timestamps;
    public GameObject[] DrumPrefabs;
    public Transform drumParent;
    public RectTransform parentRTr;
    public float XOffset = -305;
    public float mat_speed;
    public EventReference track;

    public KeyCode red1 = KeyCode.F;  // Small Red
    public KeyCode red2 = KeyCode.H;  // Small Red
    public KeyCode blue1 = KeyCode.D; // Small Blue
    public KeyCode blue2 = KeyCode.J; // Small Blue
    


    public Animator turtleAnim;
    public Animator hitFeedbackAnim;
    public Animator Anim;
    public EventReference drimMiss;
    public EventReference[] drumHit;

    public bool AutoBot;
    public float delay;
    EventInstance trackInstance;
    int NoteAmount;
    void SetUpGame(string mapPath)
    {
        score= 0;
        streak = 0;
        quality = new int[4];
        SoulGauge.value = 0;
        SoulGaugeMaxObj.SetActive(SoulGauge.value >= .92f);
        ScoreTxt.text = "";
        StreakTxt.text = "";

        TextAsset osuData = Resources.Load<TextAsset>(mapPath); 
        if (osuData != null)
        {
            SortedDictionary<int, int> hitMap = ParseOsuData(osuData.text);
            timestamps = hitMap.Keys.ToList();
            NoteAmount = hitMap.Count;
            drums = LayoutDrums(hitMap);
        }
        else
        {
            Debug.LogError($"Osu data file {mapPath} not found in Resources!");
        }
        parentRTr = drumParent.GetComponent<RectTransform>();
        parentRTr.anchoredPosition = new Vector2(0,0);
        matUpdateSpeed = mat_speed;
        trackInstance = AudioManager.CreateInstance(track);
    }
    public bool hasStarted;
    public void StartTrack(){
        trackInstance.start();
    }
    int i = 0;
    int[] fr = new int[3]{10,30,60};


    
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.V)){
            Application.targetFrameRate = fr[i];
            i++;
            if(i>=fr.Length) i=0;
        }
       
        
        if(hasStarted){
            
            if(drums.Count <= 0){hasStarted=false; return;}

            parentRTr.anchoredPosition= new Vector2(parentRTr.anchoredPosition.x - matUpdateSpeed*1000*Time.deltaTime, parentRTr.anchoredPosition.y);
            //Debug.Log(matUpdateSpeed);
            CheckForInput();
            CheckForMisses();
            CheckForTimingPoints();

            if(drums.Count <= 0){
                hasStarted=false;
                
                Invoke("End",  5f);
            }
        }
    }

    public float matUpdateSpeed;
    private void CheckForTimingPoints()
    {
        if(timingPoints.Count <= 0) return;
        
        int timing = getTiming();
        
        while(timingPoints[0].Timing < timing){
            TimingPoint tp = timingPoints[0];
            timingPoints.RemoveAt(0);
            Anim.SetBool("Kiai", tp.Kiai);
            matUpdateSpeed = tp.Kiai ? kiaiSpeed : mat_speed;
            //Debug.Log(timing + " : " + tp.Timing + " Updated Speed: " + matUpdateSpeed);
            if(timingPoints.Count <= 0) return;
        }
        
        
    }
    public GameObject SoulGaugeMaxObj;
    private void CheckForMisses()
    {
        int timing = getTiming();
        for(int i = 0; i < drums.Count; i++){
            if(drums[i].Missed(timing)){
                Drum d = drums[i];
                drums.Remove(d);
                Destroy(d.gameObject);
                streak = 0;
                StreakTxt.text = "";
                quality[3]++;
                
                float FillingPerGood = 1f / NoteAmount;
                float FillingPerOk = FillingPerGood * 0.5f;
                float CurrentFilling = quality[0] * FillingPerGood + (quality[1]- quality[3]) * FillingPerOk;
                SoulGauge.value = CurrentFilling * 1.15f;

                SoulGaugeMaxObj.SetActive(SoulGauge.value >= .92f);
                
                
                //AccText.text = accuracy.ToString() + "%";

                ShowFeedback(3);

                AudioManager.PlayOneShot(drimMiss, Vector2.zero);
            }else{break;}
        }
    }


    float doubleClickBuffer = 0.05f;
    bool waitingRed = false;
    bool waitingBlue = false;
    float waitTimer;
    
    private void CheckForInput()
    {
        if(AutoBot){
            Drum d = ClosestType();
            int millis = getTiming();
            int delta = Math.Abs(d.Timing - millis);
            if(delta < 25){
                Hit(d.Type, millis, d);
            }
            return;
        }
        bool red = Input.GetKeyDown(red1) || Input.GetKeyDown(red2);
        bool blue = Input.GetKeyDown(blue1) || Input.GetKeyDown(blue2);

        int timing = getTiming();
        Drum target = ClosestType();

        if(red && waitingRed && Time.deltaTime - waitTimer <= doubleClickBuffer){
            waitingRed = false;
            Hit(2, timing, target);

        }else if(red){
            waitTimer = Time.time;
            if(target.Type==2){
                waitingRed = true;
            }else{
                waitingRed = false;
                Hit(0, timing, target);
            }
        }

        if(blue && waitingBlue && Time.deltaTime - waitTimer <= doubleClickBuffer){
            waitingBlue = false;
            Hit(3, timing, target);

        }else if(blue){
            waitTimer = Time.time;
            if(target.Type==3){
                waitingBlue = true;
            }else{
                waitingBlue = false;
                Hit(1, timing, target);
            }
        }

    }
    int getTiming(){
      
        return Math.Abs((int)(parentRTr.anchoredPosition.x + XOffset));
    }
    void Hit(int type, int timing, Drum closestDrum)
    {
        
        int[] score = closestDrum.Score(type, timing);
        bool res = Score(score);

        ShowFeedback(score[0]==0 ? 3 : score[1]);

        if(res){

            AudioManager.PlayOneShot(drumHit[closestDrum.Type], Vector2.zero);

            drums.Remove(closestDrum); 
            Destroy(closestDrum.gameObject);
            turtleAnim.Play("TaikoBite");
            
            quality[score[1]]++;
            
            float FillingPerGood = 1f / NoteAmount;
            float FillingPerOk = FillingPerGood * 0.5f;
            float CurrentFilling = quality[0] * FillingPerGood + (quality[1]- quality[3]) * FillingPerOk;
            SoulGauge.value = CurrentFilling * 1.15f;

            SoulGaugeMaxObj.SetActive(SoulGauge.value >= .92f);
        }
    } 
    public int score;
    public int streak;
    int[] quality = new int[4];
    public TextMeshProUGUI ScoreTxt;
    public TextMeshProUGUI StreakTxt;
    public Slider SoulGauge;

    bool Score(int[] value){
        if(value[1]==-1){return false;} //Out of Range
        if(value[0]==0){
            streak = 0;
        }else{
            streak = value[0]==2 ? 0 : streak + 1;
            score += value[0] * (1 + streak/25);
        } //Wrong Button and In-Range

        

        

        ScoreTxt.text = score.ToString();
        StreakTxt.text = streak >= 10 ? "x" + streak.ToString() : "";
        return streak != 0;
    }
    Drum ClosestType(){
        
        float millis = drumParent.GetComponent<RectTransform>().anchoredPosition.x + XOffset;
        millis = Math.Abs(millis);

        int low = 0; int high = drums.Count - 1;
        if(millis <= drums[low].Timing) return drums[low];
        if(millis >= drums[high].Timing) return drums[high];

        while(low <= high){
            int mid = low + (high - low)/2;
            if(drums[mid].Timing==millis){
                return drums[mid];

            }else if(drums[mid].Timing < millis){
                low = mid+1;
            }else{
                high = mid-1;
            }
        }

        if (low>=timestamps.Count) return drums[high];
        if (high<0) return drums[low];

        return Math.Abs(drums[high].Timing - millis) < Math.Abs(drums[low].Timing- millis) ? drums[high] : drums[low];
    }
    
    private List<Drum> LayoutDrums(SortedDictionary<int, int> map){
        List<Drum> drums = new List<Drum>();
        Drum LayoutDrum(int timing, int type){
            GameObject go = Instantiate(DrumPrefabs[type], drumParent);

            float x = GetRealTiming(timing);

            go.GetComponent<RectTransform>().anchoredPosition = new Vector2(x,0);
            go.name = timing + ";" + type;
            return go.GetComponent<Drum>();
        }
        foreach(KeyValuePair<int,int> drum in map){
            Drum d = LayoutDrum(drum.Key, drum.Value);
            d.Type = drum.Value;
            d.Timing = GetRealTiming(drum.Key);
            drums.Add(d);
        }
        return drums;
    }
    private SortedDictionary<int, int> ParseOsuData(string data)
    {
        SortedDictionary<int, int> hitObjects = new SortedDictionary<int, int>();

        timingPoints = new List<TimingPoint>();

        string[] lines = data.Split('\n');

        bool OnHitObjects = false;

        
        TimingPoint oldTD = new TimingPoint(0, false, 0);
        foreach (string line in lines)
        {
            try{
                if(line.Contains("[HitObjects]")){
                    OnHitObjects = true;
                    continue;
                }
                if (string.IsNullOrWhiteSpace(line)) continue;

                if(OnHitObjects){
                    string[] parts = line.Split(',');

                    int time = int.Parse(parts[2]); // Hit time in ms
                    int hitType = int.Parse(parts[3]); // Type (normal/big)
                    int hitSound = int.Parse(parts[4]); // Sound (red/blue)

                    int drumType = GetTaikoNoteType(hitType, hitSound);
                    hitObjects[time] = drumType;
                }else{

                    string[] parts = line.Split(',');

                    // Hit time in ms
                    
                    int realTimming = int.Parse(parts[0]);
                    int deltaTimming = Math.Abs(realTimming - oldTD.RealtTime);
                    deltaTimming = (int)(deltaTimming * (oldTD.Kiai ? kiaiSpeed : mat_speed));

                    int time = oldTD.Timing + deltaTimming;
                    bool kiai = int.Parse(parts[7]) == 1; // Type (normal/big)
                    Debug.Log(time + " " + kiai + " " + realTimming);
                    
                    oldTD = new TimingPoint(time, kiai, realTimming);
                    timingPoints.Add(new TimingPoint(time, kiai, realTimming));
                    
                }
                
            }catch{
                continue;
            }
            
        }
        
        return hitObjects;
    }
    public float kiaiSpeed;
    public int GetRealTiming(int factualTiming){
        int timingAcc = 0;  
        TimingPoint oldTD = new TimingPoint(0, false, 0);

        foreach(TimingPoint tp in timingPoints){
            if(factualTiming >= tp.RealtTime){
                int delta = tp.RealtTime - oldTD.RealtTime;
                timingAcc += (int)(delta*(oldTD.Kiai ? kiaiSpeed : mat_speed));
                oldTD = tp;
                // Debug.Log("Acc: " + timingAcc);
            }else{
                int delta = factualTiming - oldTD.RealtTime;
                timingAcc += (int)(delta*(oldTD.Kiai ? kiaiSpeed : mat_speed));
                break;
                // Debug.Log("Acc Final: " + timingAcc);
            }
        }
        if(factualTiming > timingPoints.Last().RealtTime){
            int delta = factualTiming - timingPoints.Last().RealtTime;
            timingAcc += (int)(delta*(timingPoints.Last().Kiai ? kiaiSpeed : mat_speed));
            // Debug.Log("Acc Comp: " + timingAcc);
        }
        //Debug.Log(factualTiming + " > " + timingAcc);
        return timingAcc;
        
    }

    private int GetTaikoNoteType(int hitType, int hitSound)
    {
        switch(hitSound){
            case 0: return 0;
            case 4: return 2;
            case 8: return 1;
            case 12: return 3;
            default: return 0;
        }
    }

    public string[] feedback = new string[4]{"PERFECT", "GOOD", "OK", "MISS"};
    public Color[] feedbackColors;
    public TextMeshProUGUI feedbackText;
    private void ShowFeedback(int type){
        if(type==-1){return;}

        feedbackText.color = feedbackColors[type];
        feedbackText.text = feedback[type];

        hitFeedbackAnim.SetTrigger("Hit");
    }

    public int Difficulty;
    string[] maps = new string[]{"Kantan", "Futsuu", "Muzukashii", "Oni"};
    

    public void setDifficulty(int n){
        Difficulty = n;
    }
    public void Play()
    {
        Anim.GetComponent<Animator>().Play("TaikoIntro");

        SetUpGame(maps[Difficulty]);
        hasStarted = true;
        Invoke("StartTrack", delay/mat_speed);
    }
    public void BackToMenu()
    {
        Anim.GetComponent<Animator>().Play("TaikoReIntro");


    }

    public void End(){
        Debug.Log("ENDING");
        trackInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        trackInstance.release();
        Anim.GetComponent<Animator>().Play("TaikoEnd");
        SetUpEndGameStats();
    }

    [Header("End Game Stats")]
    public DynamicText perfectsTxt;
    public DynamicText goodsTxt;
    public DynamicText oksTxt;
    public DynamicText missesTxt;
    public DynamicText scoreTxt;
    public DynamicText ClearTxt;
    public GameObject highScoreTxt;
    private void SetUpEndGameStats()
    {
        bool passed = true;
        if(quality[0] == NoteAmount){
            ClearTxt.SetText("FULL PERFECTION");
        }else if(streak == NoteAmount){
            ClearTxt.SetText("FULL COMBO");
        }else if(SoulGauge.value >= .5f){
            ClearTxt.SetText("CLEARED");
        }else{
            passed=false;
            ClearTxt.SetText("FAILED");
        }
        if(passed){
            Casino.Instance.CompleteQuestIfHasAndQueueDialogue(47, "Cloris", 18);
        }

        perfectsTxt.SetText("PERFECT<br><size=80%>{0}", new string[]{quality[0].ToString()});
        goodsTxt.SetText("GOOD<br><size=80%>{0}", new string[]{quality[1].ToString()});
        oksTxt.SetText("BAD<br><size=80%>{0}", new string[]{quality[2].ToString()});
        missesTxt.SetText("MISS<br><size=80%>{0}", new string[]{quality[3].ToString()});
        scoreTxt.SetText("SCORE<br><size=80%>{0}", new string[]{score.ToString()});

        
        if(GameVariables.GetVariable(maps[Difficulty]) < score){
            highScoreTxt.SetActive(true);
            GameVariables.SetVariable(maps[Difficulty], score);
        }else{
            highScoreTxt.SetActive(false);
        }
    }
}

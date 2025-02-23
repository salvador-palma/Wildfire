using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class Taiko : MonoBehaviour
{
   
    private List<Drum> drums;
    List<int> timestamps;
    public GameObject[] DrumPrefabs;
    public Transform drumParent;
    public RectTransform parentRTr;
    public float XOffset = -305;
    public float mat_speed = 5f;
    public AudioSource source;

    public KeyCode red1 = KeyCode.F;  // Small Red
    public KeyCode red2 = KeyCode.H;  // Small Red
    public KeyCode blue1 = KeyCode.D; // Small Blue
    public KeyCode blue2 = KeyCode.J; // Small Blue
    
    public RectTransform strip1;
    public RectTransform strip2;

    public Animator turtleAnim;
    void Start()
    {
        TextAsset osuData = Resources.Load<TextAsset>("TaikoHard"); 
        if (osuData != null)
        {
            SortedDictionary<int, int> hitMap = ParseOsuData(osuData.text);
            timestamps = hitMap.Keys.ToList();
            drums = LayoutDrums(hitMap);
        }
        else
        {
            Debug.LogError("Osu data file not found in Resources!");
        }
        parentRTr = drumParent.GetComponent<RectTransform>();

        strip1.sizeDelta = new Vector2(drums[drums.Count-1].transform.position.x - drums[0].transform.position.x + 400, strip1.sizeDelta.y);
        strip2.sizeDelta = new Vector2(drums[drums.Count-1].transform.position.x - drums[0].transform.position.x + 400, strip1.sizeDelta.y);
        //strip1.anchoredPosition = new Vector2((drums[drums.Count-1].transform.position.x + drums[0].transform.position.x)/2f, strip1.anchoredPosition.y);
    }
    public bool hasStarted;
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) && !hasStarted){
            hasStarted = true;
            source.Play();
        }
        
        if(hasStarted){
            
            if(drums.Count <= 0){hasStarted=false; return;}
            parentRTr.anchoredPosition= new Vector2(parentRTr.anchoredPosition.x - mat_speed*Time.deltaTime, parentRTr.anchoredPosition.y);
            CheckForInput();
            CheckForMisses();
        }
    }

    private void CheckForMisses()
    {
        int timing = getTiming();
        for(int i = 0; i < drums.Count; i++){
            if(drums[i].Missed(timing)){
                Drum d = drums[i];
                drums.Remove(d);
                Destroy(d.gameObject);
                streak = 0;
                StreakTxt.text = streak.ToString();
                quality[3]++;
                accuracy = (float)Math.Round((quality[0]*300f+quality[1]*100f+quality[2]*50f) / (quality.Sum()*300f) * 10000f)*0.01f;
                AccText.text = accuracy.ToString() + "%";
            }else{break;}
        }
    }


    float doubleClickBuffer = 0.05f;
    bool waitingRed = false;
    bool waitingBlue = false;
    float waitTimer;
    
    private void CheckForInput()
    {
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
        bool res = Score(score[0]);

        

        if(res){
            drums.Remove(closestDrum); 
            Destroy(closestDrum.gameObject);
            turtleAnim.Play("TaikoBite");
            
            quality[score[1]]++;
            accuracy = (float)Math.Round((quality[0]*300f+quality[1]*100f+quality[2]*50f) / (quality.Sum()*300f) * 10000f)*0.01f;

            AccText.text = accuracy.ToString() + "%";
        }
    } 
    int score;
    int streak = 0;
    float accuracy;
    int[] quality = new int[4];
    public TextMeshProUGUI ScoreTxt;
    public TextMeshProUGUI StreakTxt;
    public TextMeshProUGUI AccText;

    bool Score(int value){
        if(value==-1){return false;} //Out of Range
        if(value==0){streak = 0;}else{streak++;} //Wrong Button and In-Range

        score += value * (1 + streak/25);

        ScoreTxt.text = score.ToString();
        StreakTxt.text = streak.ToString();
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
            float x = timing/1000f * mat_speed;
            go.GetComponent<RectTransform>().anchoredPosition = new Vector2(x,0);
            go.name = timing + ";" + type;
            return go.GetComponent<Drum>();
        }
        foreach(KeyValuePair<int,int> drum in map){
            Drum d = LayoutDrum(drum.Key, drum.Value);
            d.Type = drum.Value;
            d.Timing = drum.Key;
            drums.Add(d);
        }
        return drums;
    }
    private SortedDictionary<int, int> ParseOsuData(string data)
    {
        SortedDictionary<int, int> hitObjects = new SortedDictionary<int, int>();
        string[] lines = data.Split('\n');

        foreach (string line in lines)
        {
            if (string.IsNullOrWhiteSpace(line)) continue;

            string[] parts = line.Split(',');

            int time = int.Parse(parts[2]); // Hit time in ms
            int hitType = int.Parse(parts[3]); // Type (normal/big)
            int hitSound = int.Parse(parts[4]); // Sound (red/blue)

            int drumType = GetTaikoNoteType(hitType, hitSound);
            hitObjects[time] = drumType;
        }
        Debug.Log(hitObjects.Count);
        return hitObjects;
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
}

using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Plinko : MonoBehaviour
{
    public GameObject ArmadilloBall;
    float[] PascalValues = new float[]{0,100f,25f,10f,5f,2f,.5f,0.2f,0.2f,.5f,2f,5f,10f,25f,100f,0};
    public int PascalMultiplier = 1;

   


    
    
    [Header("References")]
    [SerializeField] TMP_InputField AcornValueTxt;
    [SerializeField] TextMeshProUGUI AcornAmountTxt;
    [SerializeField] TextMeshProUGUI TotalEmberAmount;
    [SerializeField] TextMeshProUGUI GainEmberAmount;
    [SerializeField] TextMeshProUGUI TotalRunEmberAmount;

    [SerializeField] Button RepeatBtn;
    [SerializeField] Button BackBtn;

    [SerializeField] int AcornAmount;
    [SerializeField] long TotalRun;



    
    private void Start() {
        AcornValueTxt.text = Math.Min(1000, (int) SkillTreeManager.Instance.PlayerData.embers).ToString();
        TotalRunEmberAmount.text = "";
        TotalEmberAmount.text =  SkillTreeManager.Instance.PlayerData.embers.ToString();
        AcornValueTxt.onValueChanged.AddListener(OnValueChanged);
    }

    private void OnValueChanged(string arg0)
    {
        try{
            int n = int.Parse(arg0);
            if(n < 0){AcornValueTxt.text = "0";}
            if(n*AcornAmount >  SkillTreeManager.Instance.PlayerData.embers){AcornValueTxt.text = ( SkillTreeManager.Instance.PlayerData.embers/AcornAmount).ToString();}
        }catch{
            AcornValueTxt.text="0";
        }
    }

    public long cur;
    public long obj;
    public float interval = .0001f;
    public float intervalTimer = .0001f;
    public int speed = 10;
    public int dir;
    public bool TextOn = false;
    private void Update() {
        if(TextOn){
            if(intervalTimer<0){
                intervalTimer = interval;
                dir = cur < obj ? 1 : -1;
                cur+=dir*speed;
                TotalEmberAmount.text = cur.ToString();
            }else{
                intervalTimer-=Time.deltaTime;
            }
            if((dir==1 && cur > obj )||(dir==-1 && cur < obj )){
                TextOn = false;cur = obj; TotalEmberAmount.text = cur.ToString();
                if(left <= 0){
            
                    TextOn = false;obj = SkillTreeManager.Instance.PlayerData.embers + roundEmberAmount; cur = obj; TotalEmberAmount.text = cur.ToString();
                    Debug.Log("TextOff");
                    SkillTreeManager.Instance.AddEmbers(roundEmberAmount);
                    
                    
                    if(SkillTreeManager.Instance.PlayerData.embers > AcornAmount*int.Parse(AcornValueTxt.text)){
                        RepeatBtn.interactable = true;
                    }
                    BackBtn.interactable=true;
                    
                }
                
            }
            
        }
    }
    public void UpdateEmberAmount(){
        cur = int.Parse(TotalEmberAmount.text);
        obj = SkillTreeManager.Instance.PlayerData.embers + roundEmberAmount;

        speed = Math.Max(10, (int)(Math.Abs(cur-obj)/100f));
        Debug.Log("TextOn");
        TextOn= true;

        if(TotalRun<0){
            TotalRunEmberAmount.text = "You've lost " + TotalRun.ToString() + " Embers in total";
        }else if(TotalRun>0){
            TotalRunEmberAmount.text = "You've gained +" + TotalRun.ToString() + " Embers in total";
        }else{
            TotalRunEmberAmount.text = "";
        }
    }
    public void AddEmbersToSkillTree(long n){
        GainEmberAmount.text = (n < 0 ? "" : "+") + n.ToString();
        if(n>0){
            GainEmberAmount.color = new Color(1, .67f, 0);
        }else{
            GainEmberAmount.color = new Color(.89f, .36f, .3f);
        }

        roundEmberAmount+=n;
        
       
        GetComponent<Animator>().Play("GainEmbers");
        TotalRun += n;

    }  
    public void changeMineAmount(int dir){


        if(int.Parse(AcornValueTxt.text) == 0){AcornValueTxt.text = "1"; return;}

        AcornAmount += dir;

        if(AcornAmount<1){

            AcornAmount=Math.Min(100, (int)( SkillTreeManager.Instance.PlayerData.embers/int.Parse(AcornValueTxt.text)));

        }

        if(AcornAmount>Math.Min(100, (int)( SkillTreeManager.Instance.PlayerData.embers/int.Parse(AcornValueTxt.text)))){
            if(AcornAmount>100){AcornAmount=1;}
            else{
                int prev = int.Parse(AcornValueTxt.text);

                AcornValueTxt.text = (prev * (AcornAmount - dir) / AcornAmount).ToString();
            }

        }


        AcornAmountTxt.text = AcornAmount.ToString();
    } 

    public void StartGame(){
        GetComponent<Animator>().Play("IntroGame");
    }

    public void ReturnGameSetup(){
        if( SkillTreeManager.Instance.PlayerData.embers <= AcornAmount*int.Parse(AcornValueTxt.text)){
            AcornAmount = 1;
            AcornAmountTxt.text = AcornAmount.ToString();
            AcornValueTxt.text = Math.Min(1000,(int) SkillTreeManager.Instance.PlayerData.embers).ToString();
        }
        GetComponent<Animator>().Play("OutroGame");
    }

    public void RepeatPlay(){
        SpawnWave();
    }



    
    

    public long roundEmberAmount;
    
    

    private int left;
    void SpawnWave(){
        roundEmberAmount = 0;
        Array.ForEach(GameObject.FindGameObjectsWithTag("Prop"), e => Destroy(e.gameObject));

        AddEmbersToSkillTree(-1*AcornAmount*int.Parse(AcornValueTxt.text));

        RepeatBtn.interactable = false;
        BackBtn.interactable=false;
        
        left=AcornAmount;
        for (int i = 0; i < AcornAmount; i++)
        {
            SpawnArmadillo();
        }
    }
    void SpawnArmadillo(){
        Instantiate(ArmadilloBall, transform).GetComponent<RectTransform>().anchoredPosition = new Vector2( UnityEngine.Random.Range(-75f, 75f),180);
    }

    public void CheckEntry(int id){
        left--;
        
        if(!(id==0 || id ==15)){
            AddEmbersToSkillTree((long)(int.Parse(AcornValueTxt.text)*PascalValues[id]));
        }

        
    }
    public float getMultiplier(int id){
        return PascalValues[id];
    }

    
}

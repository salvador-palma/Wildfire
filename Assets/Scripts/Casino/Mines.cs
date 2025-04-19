using System;
using System.Collections;
using System.Linq;
using FMODUnity;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class Mines : MonoBehaviour
{
    [Header("References")]
    [SerializeField] TextMeshProUGUI MineAmountText;
    [SerializeField] TMP_InputField WageAmountText;
    [SerializeField] GameObject OutGamePanel;
    [SerializeField] GameObject InGamePanel;
    [SerializeField] Transform grid;
    [SerializeField] TextMeshProUGUI MultiplierTxt;
    [SerializeField] TextMeshProUGUI CashOutTxt;
    [SerializeField] TextMeshProUGUI TotalEmberAmount;
    [SerializeField] TextMeshProUGUI GainEmberAmount;
    [SerializeField] DynamicText TotalRunEmberAmount;

    [Header("Values")]
    [SerializeField] private long bet;
    [SerializeField] private int mineAmount;
    [SerializeField] private int[] mines;
    [SerializeField] private int clearedSlots;
    [SerializeField] private float multiplier;
    [SerializeField] private bool inGame;
    [SerializeField] private long TotalRun;
    private void Start() {
        WageAmountText.text = "1000";
        TotalRunEmberAmount.SetText("");
        TotalEmberAmount.text = SkillTreeManager.Instance.PlayerData.embers.ToString();
        WageAmountText.onValueChanged.AddListener(OnValueChanged);
    }

    private void OnValueChanged(string arg0)
    {
        try{
            int n = int.Parse(arg0);
            if(n < 0){WageAmountText.text = "0";}
            if(n > SkillTreeManager.Instance.PlayerData.embers){WageAmountText.text = SkillTreeManager.Instance.PlayerData.embers.ToString();}
        }catch{
            WageAmountText.text="0";
        }
        
        
    }

    public float calculateWinningProbability(int spaces_cleared, int bombs){
        float winningProbability = 1f;
        for (int i = 0; i < spaces_cleared; i++)
        {
            winningProbability *= (25f-bombs-i)/(25f-i);
        }
        return winningProbability;
    }
    public float calculateMultiplier(float winningProbability){
        return Mathf.Round(.97f * 1f/winningProbability * 100f) * 0.01f;
    }

    public void StartGame(){
        if(inGame){return;}
        inGame=true;
        UpdateSetupUI(true);
        SetupGame();
        
    }

    private void SetupGame()
    {
        ClearPreviousGame();
        bet = int.Parse(WageAmountText.text);
        AddEmbersToSkillTree(-1 * bet);
        clearedSlots = 0;
       
        multiplier=0;
        MultiplierTxt.text = "";
        CashOutTxt.text = bet.ToString();
        

        mines = Enumerable.Range(0, 25).ToArray();
        
        // Shuffle the numbers randomly
        System.Random random = new System.Random();
        for (int i = mines.Length - 1; i > 0; i--)
        {
            int j = random.Next(i + 1);
            // Swap elements at index i and j
            int temp = mines[i];
            mines[i] = mines[j];
            mines[j] = temp;
        }

        // Return the first 'amount' numbers
        mines = mines.Take(mineAmount).ToArray();

    }

    private void ClearPreviousGame()
    {
        foreach (Transform pot in grid)
        {
            pot.GetComponent<Animator>().SetTrigger("Down");
            pot.GetComponent<Button>().interactable = true;
            
            
        }
    }

    //====== SETUP GAME SECTION =======

    public void changeMineAmount(int dir){
        if(inGame){return;}
        mineAmount += dir;
        if(mineAmount<1){mineAmount=24;}
        if(mineAmount>24){mineAmount=1;}
        MineAmountText.text = mineAmount.ToString();
    }
    public void UpdateSetupUI(bool On){
        WageAmountText.interactable = !On;
        InGamePanel.SetActive(On);
        OutGamePanel.SetActive(!On);
    }

    //====== IN GAME SECTION =======

    public void Clicked(int ID){
        grid.GetChild(ID).GetComponent<Animator>().ResetTrigger("Down");
        grid.GetChild(ID).GetComponent<Button>().interactable = false;
        if(!inGame){return;}
        if(mines.Contains(ID)){
            grid.GetChild(ID).GetComponent<Animator>().Play("EaterPopUp");
            WageAmountText.text = Math.Min( int.Parse(WageAmountText.text), SkillTreeManager.Instance.PlayerData.embers).ToString();
            EndGame();
            AudioManager.PlayOneShot(FMODEvents.Instance.FlowerWrong, transform.position);
        }else{
            grid.GetChild(ID).GetComponent<Animator>().Play("FlowerPopUp");
            InGamePanel.GetComponent<Animator>().SetTrigger("Right");

            PlaySoundProgress();
            IncrementClearSlots();
        }
    }

    private void PlaySoundProgress(){
        
        var instance = RuntimeManager.CreateInstance(FMODEvents.Instance.FlowerProgress);
        
        instance.setParameterByName("CorrectGuesses", Math.Clamp(clearedSlots/25f,0f,1f));
        instance.start();
        instance.release();
        
    }
    private void IncrementClearSlots()
    {
        clearedSlots++;
        multiplier = calculateMultiplier(calculateWinningProbability(clearedSlots,mineAmount));
        MultiplierTxt.text = "x" + multiplier.ToString("F2");
        CashOutTxt.text = Math.Round(bet*multiplier).ToString();

    }
    public void Cashout(){
        Debug.Log("Won: +" + Math.Round(bet*multiplier) + " Embers");
        int value = (int)Math.Round(bet*multiplier);
        AddEmbersToSkillTree(value);
        if(GameVariables.hasQuest(8) && value>=100000){
            Casino.Instance.CompleteQuestIfHasAndQueueDialogue(8, "Gyomyo", 11);
            //PUT HERE CASINO QUEST UNLOCKABLE //TO GYOMYO 11
        }

        EndGame();
    }

    private void EndGame(){
        UpdateSetupUI(false);
        foreach (Transform pot in grid)
        {

            pot.GetComponent<Button>().interactable = false;
        }
        inGame = false;
    }

    public void AddEmbersToSkillTree(long n){
        GainEmberAmount.text = (n < 0 ? "" : "+") + n.ToString();
        if(n>0){
            GainEmberAmount.color = new Color(1, .67f, 0);
        }else{
            GainEmberAmount.color = new Color(.89f, .36f, .3f);
        }
        SkillTreeManager.Instance.AddEmbers(n);
       
        GetComponent<Animator>().Play("GainEmbers");
        TotalRun += n;

    }   
    public void UpdateEmberAmount(){

        cur = int.Parse(TotalEmberAmount.text);
        obj = SkillTreeManager.Instance.PlayerData.embers;

        speed = Math.Max(10, (int)(Math.Abs(cur-obj)/100f));
        dir = cur < obj ? 1 : -1;
        TextOn= true;

        if(TotalRun<0){
            TotalRunEmberAmount.SetText("You've lost {0} Embers in total", new string[]{TotalRun.ToString()});
        }else if(TotalRun>0){
            TotalRunEmberAmount.SetText("You've gained +{0} Embers in total", new string[]{TotalRun.ToString()});
        }else{
            TotalRunEmberAmount.SetText("");
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
                cur+=dir*speed;
                TotalEmberAmount.text = cur.ToString();
                AudioManager.PlayOneShot(FMODEvents.Instance.MoneyTick,transform.position);
            }else{
                intervalTimer-=Time.deltaTime;
            }
            if((dir==1 && cur > obj )||(dir==-1 && cur < obj )){TextOn = false;cur = obj; TotalEmberAmount.text = cur.ToString();}
            
        }
    }

}

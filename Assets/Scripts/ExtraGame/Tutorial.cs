using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    public RunTimeDialogues[] TutorialDialogue;
    public RunTimeDialogues[] AugmentTutorialDialogue;
    public RunTimeDialogues[] EffectsDialogue;
    public RunTimeDialogues[] EndDialogue;
    public RunTimeDialogues[] TerminateDialogue;

    public static bool inTutorial = true;
    public static int TutorialGiven = 0;
    public GameObject FlameyPanel;
    public GameObject EnemyPanel;
    public GameObject Catterpillar;
    

    public static List<int> dialoguesRan;

    void Awake()
    {
        
        if(TutorialGiven==0){
            inTutorial = true;
        }

        if(dialoguesRan == null){
            dialoguesRan = new List<int>();
        }
        
    }
    public void Start()
    {
        if(TutorialGiven==0){
            TutorialGiven+=1;
            AudioManager.Instance.SetAmbienceParameter("OST_Volume", 0.2f);
            Invoke("StartTutorial", 3f);
        }
        
    }
    
    void StartTutorial(){
        AudioManager.Instance.SetAmbienceParameter("OST_Volume", 0.2f);
        StartCoroutine(Chat.Instance.StartDialogue(TutorialDialogue[0].dialogues, endAfter:!TutorialDialogue[0].dontEndAfter, after:TutorialDialogue[0].afterEvent));
    }

    public void  StartTutorialAugment(){
        
        Invoke("StartTutorialAugment2", 1f * Time.timeScale);
    }
    private void StartTutorialAugment2(){
        AudioManager.Instance.SetAmbienceParameter("OST_Volume", 0.2f);
        StartCoroutine(Chat.Instance.StartDialogue(AugmentTutorialDialogue[0].dialogues, endAfter:!AugmentTutorialDialogue[0].dontEndAfter, after:AugmentTutorialDialogue[0].afterEvent));
    }

    public void StartTutorialEffect(){
        AudioManager.Instance.SetAmbienceParameter("OST_Volume", 0.2f);
        StartCoroutine(Chat.Instance.StartDialogue(EffectsDialogue[0].dialogues, endAfter:!EffectsDialogue[0].dontEndAfter, after:EffectsDialogue[0].afterEvent));
    }
    public void ShowEffectMenu(bool on){
        if(on){
            GameUI.Instance.TogglePausePanel();
            GameUI.Instance.changeTab(1);
            GameUI.Instance.DisplayEffectInfo(Flamey.Instance.allEffects[0]);
            
            
            Destroy(Catterpillar);
        }else{
            GameUI.Instance.changeTab(0);
            GameUI.Instance.TogglePausePanel();
            
        }
        AudioManager.Instance.SetAmbienceParameter("OST_Volume", on ? 0f : .2f);
        
    }

    public void ShowFlamey(){
        
        FlameyPanel.SetActive(true);
    }
    public void ShowEnemy(){
        Catterpillar.SetActive(true);
        FlameyPanel.SetActive(false);
        EnemyPanel.SetActive(true);
    }
    public void UnShowEnemy(){
        EnemyPanel.SetActive(false);
    }
    public void StartGame(){
        AudioManager.Instance.SetAmbienceParameter("OST_Volume", 1f);
        inTutorial = false; 
    }
    public void NextDialogue(int id){
        
        
        StartCoroutine(Chat.Instance.StartDialogue(TutorialDialogue[id].dialogues,endAfter:!TutorialDialogue[id].dontEndAfter, after:TutorialDialogue[id].afterEvent));
    }

    public void NextDialogueEffects(int id){
        
        
        StartCoroutine(Chat.Instance.StartDialogue(EffectsDialogue[id].dialogues,endAfter:!EffectsDialogue[id].dontEndAfter, after:EffectsDialogue[id].afterEvent));
    }

    public void OpenSteamPage(){
        Application.OpenURL("https://store.steampowered.com/app/3651490/Fire_at_Campsite/");
    }
    public Button RestartButton;
    public Button QuitButton;
    int maxLoop = 1000;
    public void StartEndDialogue(){
        
            int i = 0;
            int n = Random.Range(0, EndDialogue.Length);
            while(dialoguesRan.Contains(n) && i < maxLoop){
                n = Random.Range(0, EndDialogue.Length);
                i++;
            }
            dialoguesRan.Add(n);
            StartCoroutine(Chat.Instance.StartDialogue(EndDialogue[n].dialogues,endAfter:true));
        
            
    }
    public void ThankYou(){
        GameUI.Instance.GetComponent<Animator>().Play("ThankYou");
    }

    public void Quit()
    {
        Application.Quit();
    }

}

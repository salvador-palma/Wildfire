using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    public RunTimeDialogues[] TutorialDialogue;
    public RunTimeDialogues[] EndDialogue;
    public RunTimeDialogues[] TerminateDialogue;

    public static bool inTutorial = true;
    public static bool TutorialGiven = false;
    public GameObject FlameyPanel;
    public GameObject EnemyPanel;
    public GameObject Catterpillar;
    public static int timesPlayed = 0;
    void Awake()
    {
        timesPlayed++;
        if(!TutorialGiven){
            inTutorial = true;
        }
        
    }
    public void Start()
    {
        if(!TutorialGiven){
            TutorialGiven = true;
            AudioManager.Instance.SetAmbienceParameter("OST_Volume", 0.2f);
            Invoke("StartTutorial", 3f);
        }
        
    }
    void StartTutorial(){
        AudioManager.Instance.SetAmbienceParameter("OST_Volume", 0.2f);
        StartCoroutine(Chat.Instance.StartDialogue(TutorialDialogue[0].dialogues, endAfter:!TutorialDialogue[0].dontEndAfter, after:TutorialDialogue[0].afterEvent));
    }
    public void ShowEffectMenu(bool on){
        
        GameUI.Instance.TogglePausePanel();
        GameUI.Instance.changeTab(1);
        GameUI.Instance.DisplayEffectInfo(Flamey.Instance.allEffects[0]);
        AudioManager.Instance.SetAmbienceParameter("OST_Volume", on ? 0f : .2f);
        EnemyPanel.SetActive(false);
        Destroy(Catterpillar);
    }

    public void ShowFlamey(){
        
        FlameyPanel.SetActive(true);
    }
    public void ShowEnemy(){
        Catterpillar.SetActive(true);
        FlameyPanel.SetActive(false);
        EnemyPanel.SetActive(true);
    }
    public void StartGame(){
        AudioManager.Instance.SetAmbienceParameter("OST_Volume", 1f);
        inTutorial = false; 
    }
    public void NextDialogue(int id){
        
        
        StartCoroutine(Chat.Instance.StartDialogue(TutorialDialogue[id].dialogues,endAfter:!TutorialDialogue[id].dontEndAfter, after:TutorialDialogue[id].afterEvent));
    }

    public void OpenSteamPage(){
        Application.OpenURL("https://store.steampowered.com/app/3651490/Fire_at_Campsite/");
    }
    public Button RestartButton;
    public Button QuitButton;

    public void StartEndDialogue(){
        if(timesPlayed >= 3){
            Destroy(RestartButton.gameObject);
           

            int n = Random.Range(0, TerminateDialogue.Length);
            UnityEvent e = new UnityEvent();
            e.AddListener(ThankYou);
            StartCoroutine(Chat.Instance.StartDialogue(TerminateDialogue[n].dialogues,endAfter:true, after:e));
        }else{
            
            int n = Random.Range(0, EndDialogue.Length);
            StartCoroutine(Chat.Instance.StartDialogue(EndDialogue[n].dialogues,endAfter:true));
        }
            
    }
    public void ThankYou(){
        GameUI.Instance.GetComponent<Animator>().Play("ThankYou");
    }

    public void Quit()
    {
        Application.Quit();
    }

}

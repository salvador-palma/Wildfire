using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[Serializable]
public class MiniGameProps{
    public string name;
    public GameObject[] prefab;
    public Button selectButton;
    public bool withSoundTrack;
}
public class Casino : MonoBehaviour
{
    static int offset = 0;
    public List<MiniGameProps> minigames;
    public GameObject settingsPanel;
    public static Casino Instance;
    public Animator SelectPanel;
    public string CurrentMinigame;

    // public static string getMinigame(){
    //     DateTime d = DateTime.Today;
    //     switch((int)d.DayOfWeek % 3 + offset){
    //         case 0: return "Drop the Acorn";
    //         case 1: return "Water the Flower";
    //         case 2: 
    //         default:
    //         return "Frog Jump";
    //     }
    // }
    // public static int getMinigameID(){
    //     DateTime d = DateTime.Today;
    //     return ((int)d.DayOfWeek % 3 ) + offset;
        
    // }
    private void Awake() {
        Instance = this;
        foreach(MiniGameProps props in minigames){
            if(props.name == "Techno Turtle" && !GameVariables.hasQuestCompleted(46)){
                props.selectButton.transform.parent.parent.gameObject.SetActive(false);
            }
            props.selectButton.onClick.RemoveAllListeners();
            props.selectButton.onClick.AddListener(()=> LoadMiniGame(props.name));
        }

        InteractiveCursor.ChangeCursor(0);
        
    }
    private void LoadMiniGame(string minigame){
        CurrentMinigame = minigame;
        SelectPanel.Play("PickedMiniGameCasino");
        GetComponent<Animator>().Play("CurtainsOn");
        foreach(GameObject go in minigames.Find(x => x.name == minigame).prefab){
            go.SetActive(true);
        }
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)){
            ToggleSettings();
        }
    }
    public void ToggleSettings(){
        Vector2 newPos = new Vector2(settingsPanel.GetComponent<RectTransform>().anchoredPosition.x > 2000 ? 0 : 4000, 0);
        AudioManager.Instance.SetAmbienceParameter("OST_Volume", newPos.x <= 0? 0 : 1);
        settingsPanel.GetComponent<RectTransform>().anchoredPosition = newPos;
    }
    public void GoBack(){
        GetComponent<Animator>().Play("CurtainsOff");
    }
    public void LoadCampsite(){
        PlayerPrefs.SetInt("Origin", 1);
        SceneManager.LoadScene("MetaGame");
    }

    [SerializeField] GameObject CornerPopUpAnim;
    public void CornerPopUp(string title, string description, Sprite icon){
        CornerPopUpAnim.transform.GetChild(1).GetComponent<DynamicText>().SetText(title);
        CornerPopUpAnim.transform.GetChild(2).GetComponent<DynamicText>().SetText(description);
        CornerPopUpAnim.transform.GetChild(3).GetComponent<Image>().sprite = icon;
        CornerPopUpAnim.GetComponent<Animator>().Play("CornerPopUp");

    }
    public void CompleteQuestIfHasAndQueueDialogue(int questID, string npcname, int dialogueID){
        if(GameVariables.hasQuest(questID)){
            GameVariables.CompleteQuest(questID);
            NPC.QueueDialogue(npcname, dialogueID);
            Quest q = QuestBoard.Instance.Quests[questID]; 
            CornerPopUp("Quest Complete", q.Title, q.Avatar);
        }

        
        
    }

    public void StartCasinoMusic(){
        if(minigames.Find(x => x.name == CurrentMinigame).withSoundTrack){
            GetComponent<OST_Event_Caller>().StartMusicTrack(3);
        }
       
    }

}

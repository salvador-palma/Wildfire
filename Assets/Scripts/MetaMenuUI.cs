using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MetaMenuUI : MonoBehaviour
{
    //Deug
    [SerializeField] private GameObject BestiaryPanel;
    [SerializeField] private GameObject SkillTreePanel;
    [SerializeField] private GameObject CharacterSelectPanel;
    [SerializeField] private GameObject SkillTree;
    static public MetaMenuUI Instance;
    

    [Header("Chat")]
    [SerializeField] Animator ChatPanel;
    [SerializeField] Image Profile;
    [SerializeField] TextMeshProUGUI Message;
    [SerializeField] Button[] Options;
    [SerializeField] TextMeshProUGUI Name;

    [Header("Chat DataBase")]
    [SerializeField] Sprite[] AvatarBank;

    [Header("Unlockable")]
    [SerializeField] TextMeshProUGUI[] UnlockableTexts;
    [SerializeField] Image UnlockableIcon;
    [SerializeField] Sprite[] Unlockables;
    private void Awake() {
        Instance = this;
    }
    public void StartGame(){
        SceneManager.LoadScene("Game");
    }
    public void PlayOutro(){
        GetComponent<Animator>().Play("Outro");
    }
    
    public void SkillTreeMenuToggle(){
        
        SkillTreeManager.Instance.toggleSkillTree(SkillTreePanel);
    }
    public void CharacterSelectMenuToggle(){
        Character.Instance.toggleCharacterPanel(CharacterSelectPanel);
        
    }

    public void BestiaryMenuToggle(){
        BestiaryPanel.GetComponent<RectTransform>().anchoredPosition = new Vector2(BestiaryPanel.GetComponent<RectTransform>().anchoredPosition.x > 2000 ? 0 : 4000, 0);
    }
 
    public void UpgradeButton(){
        SkillTreeButton selected = SkillTreeButton.SelectedButton;
        if(selected != null){selected.ClickedUpgrade();}
        
    }
    private IEnumerator currentCouroutine;
    public void moveSkillTree(Vector2 pos){
        if(currentCouroutine!=null){StopCoroutine (currentCouroutine);}
        currentCouroutine = SmoothLerp (0.5f, pos);
        StartCoroutine (currentCouroutine);
    }
    
    private IEnumerator SmoothLerp (float time, Vector2 pos)
    {
        Vector3 startingPos  = SkillTree.transform.localPosition;
        Vector3 finalPos = pos;

        float elapsedTime = 0;
        
        while (elapsedTime < time)
        {
            SkillTree.transform.localPosition = Vector3.Lerp(startingPos, finalPos, elapsedTime / time);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }


    public void ClickedPlay(){
        Debug.Log(Application.persistentDataPath);
        if(File.Exists(Application.persistentDataPath +"/gameState.json")){
            StartChat();
            ChatSingular("Do you wish to continue your previous unfinished run?",
                            AvatarBank[0], "Rowl",
                            new string[2]{"Yes", "No"},
                            new UnityAction[2]{
                                new UnityAction(()=>{PlayerPrefs.SetInt("PlayerLoad", 1); EndChat(); PlayOutro();}),
                                new UnityAction(()=>{PlayerPrefs.SetInt("PlayerLoad", 0); GameState.Delete(); EndChat(); PlayOutro();})
                            });
        }else{
            PlayOutro();
        }
    }

    
    public void StartChat(){
        
        ChatPanel.gameObject.SetActive(true);
        ChatPanel.GetComponent<Animator>().Play("Intro");
    }
    public void EndChat(){
        ChatPanel.GetComponent<Animator>().Play("Outro");
        Message.text = "";
        Array.ForEach(Options, e => e.gameObject.SetActive(false));
    }
    private void ChatSingular(string msg,Sprite avatar, string name = null, string[] optionTxt = null, UnityAction[] optionAction = null){
        
        Name.text = name;
        Message.text = "";
        Profile.sprite = avatar;

        if(optionTxt!=null){
            for(int i =0; i < optionTxt.Length; i++){
                Options[i].GetComponentInChildren<TextMeshProUGUI>().text = optionTxt[i];
                Options[i].onClick.RemoveAllListeners();
                Options[i].onClick.AddListener(optionAction[i]);
            }
        }
        
        StartCoroutine(ShowTextTimed(msg, optionTxt!=null));
    }
    public IEnumerator ShowTextTimed(string msg, bool withOptions){
        string formatting_buffer = "";
        foreach(char c in msg){
            if(triggerNext>0){
                Message.text = msg;
                break;
            }
            if(c=='<' || formatting_buffer != ""){
                formatting_buffer += c;
                if(c=='>'){
                    Message.text += formatting_buffer;
                    formatting_buffer = "";
                }
                
            }else{  
                Message.text += c;
                switch(c){
                    case '.':
                    case '!':
                    case '?':
                        yield return new WaitForSeconds(0.4f);
                        break;
                    case ',':
                        yield return new WaitForSeconds(0.05f);
                        break;
                    case ' ':
                        yield return new WaitForSeconds(0.02f);
                        break;
                    default:
                        yield return new WaitForSeconds(0.01f);
                        break;
                }
            }
        }
        
        Array.ForEach(Options, e => e.gameObject.SetActive(withOptions));
        triggerNext=1;
        yield break;
    }


    int triggerNext;
    public void ClickedChatPanel(){
        triggerNext++;
    }
    public IEnumerator StartDialogue(Dialogue[] dialogue, string defaultName= null, UnityEvent after = null){
        StartChat();
        Name.text = defaultName;
        Message.text = "";
        
        foreach (Dialogue d in dialogue)
        {   
            
            ChatSingular(d.message, d.avatar, d.Name == null || d.Name == "" ? defaultName : d.Name);

            yield return new WaitUntil(() => triggerNext >= 2);
            triggerNext = 0;
        }
        EndChat();
        if(after != null){
            after.Invoke();
        }
    }



    public void UnlockableScreen(string title, string name, string description, int iconID){
        UnlockableTexts[0].text = title;
        UnlockableTexts[1].text = name;
        UnlockableTexts[2].SetText(description);
        UnlockableIcon.sprite = Unlockables[iconID];
        UnlockableIcon.transform.parent.GetComponent<Animator>().Play("UnlockableOn");

    }
    public void UnlockOff(){
        UnlockableIcon.transform.parent.GetComponent<Animator>().Play("UnlockableOff");
    }
}



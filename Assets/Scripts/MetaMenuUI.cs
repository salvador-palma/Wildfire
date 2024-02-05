using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MetaMenuUI : MonoBehaviour
{
    //Deug
    [SerializeField] private GameObject SkillTreePanel;
    [SerializeField] private GameObject SkillTree;
    static public MetaMenuUI Instance;
    

    [Header("Chat")]
    [SerializeField] Animator ChatPanel;
    [SerializeField] Image Profile;
    [SerializeField] TextMeshProUGUI Message;
    [SerializeField] Button[] Options;

    [Header("Chat DataBase")]
    [SerializeField] Sprite[] AvatarBank;


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
 
    public void UpgradeButton(){
        SkillTreeButton selected = SkillTreeButton.SelectedButton;
        if(selected != null){selected.ClickedUpgrade();}
        
    }
    private IEnumerator currentCouroutine;
    public void moveSkillTree(Vector2 pos){
        //SkillTree.transform.localPosition = pos;
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
        if(File.Exists(Application.dataPath +"/gameState.json")){
            StartChat();
            ChatSingular("Do you wish to continue your previous unfinished run?",
                            AvatarBank[0],
                            new string[2]{"Yes", "No"},
                            new UnityAction[2]{
                                new UnityAction(()=>{PlayerPrefs.SetInt("PlayerLoad", 1); EndChat(); PlayOutro();}),
                                new UnityAction(()=>{PlayerPrefs.SetInt("PlayerLoad", 0); GameState.Delete(); EndChat(); PlayOutro();})
                            });
        }else{
            PlayOutro();
        }
    }

    private void StartChat(){
        ChatPanel.gameObject.SetActive(true);
        ChatPanel.GetComponent<Animator>().Play("Intro");
    }
    public void EndChat(){
        ChatPanel.GetComponent<Animator>().Play("Outro");
        Array.ForEach(Options, e => e.gameObject.SetActive(false));
    }
    private void ChatSingular(string msg,Sprite avatar, string[] optionTxt = null, UnityAction[] optionAction = null){
        Message.text = "";
        Profile.sprite = avatar;
        for(int i =0; i < optionTxt.Length; i++){
            Options[i].GetComponentInChildren<TextMeshProUGUI>().text = optionTxt[i];
            Options[i].onClick.RemoveAllListeners();
            Options[i].onClick.AddListener(optionAction[i]);
        }
        StartCoroutine(ShowTextTimed(msg));
    }
    public IEnumerator ShowTextTimed(string msg){
        foreach(char c in msg){
            Message.text += c;
            if(char.IsWhiteSpace(c)){
                yield return new WaitForSeconds(0.05f);
            }else{
                yield return new WaitForSeconds(0.025f);
            }
            

        }
        Array.ForEach(Options, e => e.gameObject.SetActive(true));
        yield break;
    }
}



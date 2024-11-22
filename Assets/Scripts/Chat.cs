using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Chat : MonoBehaviour
{
    public static Chat Instance;
    [Header("Chat")]
    [SerializeField] Animator ChatPanel;
    [SerializeField] Image Profile;
    [SerializeField] TextMeshProUGUI Message;
    [SerializeField] Button[] Options;
    [SerializeField] TextMeshProUGUI Name;

    [Header("Chat DataBase")]
    [SerializeField] public Sprite[] AvatarBank;
    void Awake()
    {
        Instance = this;
    }

    
    public void StartChat(){
        
        ChatPanel.gameObject.SetActive(true);
        ChatPanel.Play("Intro");
    }
    public void EndChat(){
        ChatPanel.GetComponent<Animator>().Play("Outro");
        Message.text = "";
        triggerNext=0;
        Array.ForEach(Options, e => e.gameObject.SetActive(false));
    }
    public void DeactivateChat(){
        ChatPanel.gameObject.SetActive(false);
    }
    public void ChatSingular(string msg,Sprite avatar, string name = null, string[] optionTxt = null, UnityAction[] optionAction = null, AudioClip voice=null, float max=1, float min=0){
        
        Name.text = name;
        Message.text = "";
        Profile.sprite = avatar;

        Array.ForEach(Options, e => e.gameObject.SetActive(false));

        if(optionTxt!=null){
            for(int i =0; i < optionTxt.Length; i++){
                Options[i].GetComponentInChildren<TextMeshProUGUI>().text = optionTxt[i];
                Options[i].onClick.RemoveAllListeners();
                Options[i].onClick.AddListener(optionAction[i]);
                //Options[i].gameObject.SetActive(true);
            }
        }
        
        StartCoroutine(ShowTextTimed(msg, optionTxt, voice, max, min));
    }
    public IEnumerator ShowTextTimed(string msg, string[] optionTxt  = null, AudioClip voice=null, float max=1, float min=0){
        string formatting_buffer = "";
        
        int intervalSound = 4;
        int curinterval = 0;
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
                        yield return new WaitForSeconds(0.1f);
                        break;
                    case ',':
                        yield return new WaitForSeconds(0.05f);
                        break;
                    case ' ':
                        yield return new WaitForSeconds(0.02f);
                        break;
                    default:
                        if(curinterval <= 0){curinterval = intervalSound; AudioManager.Speak(voice, max, min);}else{curinterval--;} 
                        yield return new WaitForSeconds(0.01f);
                        break;
                }
            }
        }
        if(optionTxt != null){
            for(int i =0; i < optionTxt.Length; i++){
                Options[i].gameObject.SetActive(true);
            }
        }
        
        //Array.ForEach(Options, e => e.gameObject.SetActive(withOptions));
        triggerNext=1;
        yield break;
    }


    int triggerNext;
    public void ClickedChatPanel(){
        triggerNext++;
    }

    
    public IEnumerator StartDialogue(Dialogue[] dialogue, string defaultName= null, UnityEvent after = null, bool endAfter = true, AudioClip v=null, float max=1, float min=0){
        StartChat();
        Name.text = defaultName;
        Message.text = "";
        foreach (Dialogue d in dialogue)
        {   
            ChatSingular(d.message, d.avatar, d.Name == null || d.Name == "" ? defaultName : d.Name, voice:v, max:max, min:min);
            yield return new WaitUntil(() => triggerNext >= 2);
            ChatPanel.GetComponent<Animator>().SetTrigger("Switch");

            triggerNext = 0;
        }
        if(endAfter){
            EndChat();
        }
        
        if(after != null){
            after.Invoke();
        }
    }
}

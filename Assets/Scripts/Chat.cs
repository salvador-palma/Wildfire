using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FMODUnity;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Chat : MonoBehaviour
{
    public static Chat Instance;
    [Header("Chat")]
    [SerializeField] Animator ChatPanel;
    [SerializeField] Image Profile;
    [SerializeField] DynamicText Message;
    [SerializeField] Button[] Options;
    [SerializeField] TextMeshProUGUI Name;
    
    [SerializeField] public Slider MoodSlider;

    [Header("Chat DataBase")]
    [SerializeField] public Sprite[] AvatarBank;
    void Awake()
    {
        Instance = this;
    }

    
    public void StartChat(){
        AudioManager.PlayOneShot(FMODEvents.Instance.PaperSlide, transform.position);
        
        ChatPanel.gameObject.SetActive(true);
        ChatPanel.Play("Intro");
        
    }
    public void EndChat(){
       
        ChatPanel.GetComponent<Animator>().Play("Outro");
        Message.SetText("");
        triggerNext.value=0;
        Array.ForEach(Options, e => e.gameObject.SetActive(false));
    }
    public void DeactivateChat(){
        ChatPanel.gameObject.SetActive(false);
    }
    public void ChatSingular(string msg,Sprite avatar, string[] arguments = null, string name = null, string[] optionTxt = null, UnityAction[] optionAction = null){
        
        Name.text = name;
        Message.SetText("");
        Profile.color = avatar!= null? Color.white : Color.clear;
        Profile.sprite = avatar;

        Array.ForEach(Options, e => e.gameObject.SetActive(false));

        if(optionTxt!=null){
            for(int i =0; i < optionTxt.Length; i++){
                Options[i].GetComponentInChildren<DynamicText>().SetText(optionTxt[i]);
                Options[i].onClick.RemoveAllListeners();
                Options[i].onClick.AddListener(optionAction[i]);
                 
            }
        }

        MoodSlider.gameObject.SetActive(name=="Jhat" && Item.getLevel("Essence Gauge") > 0);

        StartCoroutine(Message.ShowTextTimed(msg, triggerNext, arguments, optionTxt, Options, FMODEvents.GetVoice(name)));
    }
    


    Int triggerNext = new Int(0);
    public void ClickedChatPanel(){
       
        triggerNext.value++;
    }

    
    public IEnumerator StartDialogue(Dialogue[] dialogue, string defaultName= null, UnityEvent after = null, bool endAfter = true){

        
        
        if(ChatPanel.GetCurrentAnimatorClipInfo(0).Length <= 0  || ChatPanel.GetCurrentAnimatorClipInfo(0)[0].clip.name != "Static"){
            StartChat();
        }
        Name.text = defaultName;
        Message.SetText("");
        int i = 0;
        foreach (Dialogue d in dialogue)
        {   
           
            ChatSingular(d.message, d.avatar, d.arguments, name:d.Name == null || d.Name == "" ? defaultName : d.Name);
            yield return new WaitUntil(() => triggerNext.value >= 2);
            ChatPanel.GetComponent<Animator>().SetTrigger("Switch");

            triggerNext.value = 0;
            i++;
        }
        if(endAfter){
            EndChat();
        }
        
        if(after != null){
            after.Invoke();
        }
        Debug.Log("End Dialogue");
    }
}

public class Int{
    public int value;
    public Int(int v){
        value = v;
    }
}

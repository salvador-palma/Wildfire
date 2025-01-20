using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class QuestBoard : NPC
{
    
    [System.Serializable]
    public class Quest{
        public string Title;
        public Sprite Avatar;
        [TextArea] public string Description;
    }

    [Header("Quests")]
    [SerializeField] public Quest[] Quests;

    [Header("References")]
    [SerializeField] GameObject QuestPanel;
    [SerializeField] Transform QuestContainer;
    [SerializeField] Transform QuestSlot;
    private static QuestBoard Instance;
    
    private void Awake() {
        Instance = this;
    }
    protected override void CharacterLoad()
    { 
        int n = GameVariables.GetVariable("QuestBookReady");

        gameObject.SetActive(n != -1);
        if(n==0){QueueDialogue(0);GameVariables.SetVariable("QuestBookReady", 1);}
        LoadQuests();
    }

    public void ToggleQuestsPanel(){
        QuestPanel.GetComponent<RectTransform>().anchoredPosition = new Vector2(QuestPanel.GetComponent<RectTransform>().anchoredPosition.x > 2000 ? 0 : 4000, 0);
    }
    public void LoadQuests(){
       int[][] Quests = GameVariables.GetQuests();
        foreach (Transform item in QuestContainer)
        {
            if(item.gameObject.activeSelf){
                Destroy(item.gameObject);
            }
        }
        Array.ForEach(Quests[0], ID => SpawnSingularQuest(ID, true));
        Array.ForEach(Quests[1], ID => SpawnSingularQuest(ID, false));
    }
    public static void ReloadQuests(){
        Instance.LoadQuests();
    }



    private void SpawnSingularQuest(int ID, bool Active){
        Quest quest = Quests[ID];
        Transform go = Instantiate(QuestSlot, QuestContainer.transform);

        Image icon = go.GetChild(0).GetComponent<Image>();
        TextMeshProUGUI title = go.GetChild(1).GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI description = go.GetChild(2).GetComponent<TextMeshProUGUI>();

        icon.sprite = quest.Avatar;
        title.text = quest.Title;
        description.text = quest.Description;

        if(!Active){
            
            Color c2 = go.GetComponent<Image>().color;
            c2.a = .3f;
            go.GetComponent<Image>().color = c2;

            icon.color = new Color(1,1,1,.3f);
            Color c = title.color;
            c.a = .3f;
            title.color = c;
            description.color = new Color(1,1,1,.3f);
        }
        go.gameObject.SetActive(true);
    }

    public void MakeTieBreaker(){
        Chat.Instance.StartChat();
        Chat.Instance.ChatSingular("Whose idea should we follow?",
                            Chat.Instance.AvatarBank[0], name:"Rowl",
                            optionTxt:new string[3]{"Betsy", "Cloris", "Rowl"},
                            optionAction:new UnityAction[3]{
                                new UnityAction(()=>{NameQuestBoard(0);}),
                                new UnityAction(()=>{NameQuestBoard(1);}),
                                new UnityAction(()=>{NameQuestBoard(2);})
                            });
    }
    public void NameQuestBoard(int n){
        switch (n)
        {
            case 0:  Debug.Log("Name Quest Board: To Do List"); break;
            case 1:  Debug.Log("Name Quest Board: Classy Campsite Chores"); break;
            case 2:  Debug.Log("Name Quest Board: The Grand Compendium of Noble Tasks Bestowed Upon the Worthy"); break;
        }
       

        GameVariables.SetVariable("QuestBoard", n);
        Chat.Instance.EndChat();
        DequeueDialogue(0);
    }

    




}

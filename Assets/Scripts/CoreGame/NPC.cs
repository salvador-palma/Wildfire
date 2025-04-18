using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;



//string msg,Sprite avatar, string[] optionTxt = null, UnityAction[] optionAction = null
[System.Serializable]
public class Dialogue{
    [TextArea]public string message;
    public string[] arguments;
    public Sprite avatar;
    public string Name;
    public Dialogue(string message, Sprite avatar, string name, string[] arguments = null){
        this.message = message;
        this.avatar = avatar;
        this.arguments = arguments;
        Name = name;
    }
    
}


[System.Serializable]
public class RunTimeDialogues{ 
    public Dialogue[] dialogues;
    public UnityEvent afterEvent;
 
    public bool dequeueAuto;
    public bool dontEndAfter;
    public RunTimeDialogues(Dialogue[] dialogues,UnityEvent afterEvent){
        this.dialogues = dialogues;
        this.afterEvent = afterEvent;
    }
}
[System.Serializable]
public class CharacterSavedDialogues{
    public string name;
    public int[] saved_dialogues = new int[0];
    public void Queue(int id){
        
        if(!saved_dialogues.Contains(id)){
            saved_dialogues = saved_dialogues.Append(id).ToArray();
            Array.ForEach(saved_dialogues, e => Debug.Log(e));
        }
    }
    public void Dequeue(int id){
        if(saved_dialogues.Contains(id)){saved_dialogues = saved_dialogues.Where(e => e != id).ToArray();}
    }
}
public class NPCSaveData{
    public List<CharacterSavedDialogues> data;
}
public class NPC : MonoBehaviour
{
    
    public static bool hasRead;
    [SerializeField] public static NPCSaveData savedData;
    public List<RunTimeDialogues> runTimeDialogues;
    public string Name;
    
    
    public UnityEvent DefaultClickBehaviour;

    public string[] IdleAnimations;

    private void Start() {
        if(IdleAnimations.Length > 0){
            int i = UnityEngine.Random.Range(0, IdleAnimations.Length);
            Debug.Log($"Playing Idle Animation {Name}: {IdleAnimations[i]}");
            GetComponent<Animator>().Play(IdleAnimations[i]);
        }
        SpriteScale = GetComponent<RectTransform>().sizeDelta.x;
        if(!hasRead){
            ReadBestiaryData();
        }
        CharacterLoad();
        UpdateNotification();
        LocalBestiary.INSTANCE.ClaimRewardEvent += UpdateNotification;


        foreach(RunTimeDialogues r in runTimeDialogues){
            foreach (Dialogue item in r.dialogues)
            {
                Translator.AddIfNotExists(item.message);
            }
        }
        
    }

    protected virtual void CharacterLoad(){}

    public bool hasAvailableDialogue(){
        return savedData.data.Any(e => e.name == Name && e.saved_dialogues.Length > 0);
    }

    //MERGE BOTH FUNCTIONS INTO ONE/HALF
    public static void QueueDialogue(string NPCName, int ID){
        CharacterSavedDialogues own_dialogues = savedData.data.FirstOrDefault(e => e.name == NPCName);
        if(own_dialogues == null){
            
            own_dialogues = new CharacterSavedDialogues{name = NPCName,saved_dialogues = new int[1]{ID}};
            savedData.data.Add(own_dialogues);
        }else{

            if(!own_dialogues.saved_dialogues.Contains(ID)){
                savedData.data.Find(i=> i.name == NPCName).Queue(ID);
                
            }
           
                
        }
        Debug.Log("Queueing");
        WritingData();
        
    }
    public void QueueDialogue(int ID){
        CharacterSavedDialogues own_dialogues = savedData.data.FirstOrDefault(e => e.name == Name);
        if(own_dialogues == null){
            
            own_dialogues = new CharacterSavedDialogues{name = Name,saved_dialogues = new int[1]{ID}};
            savedData.data.Add(own_dialogues);
        }else{

            if(!own_dialogues.saved_dialogues.Contains(ID)){
                savedData.data.Find(i=> i.name == Name).Queue(ID);
                
            }
           
                
        }
        Debug.Log("Queueing");
        WritingData();
        UpdateNotification();
    }
    
    public void DequeueDialogue(int ID){
        CharacterSavedDialogues own_dialogues = savedData.data.FirstOrDefault(e => e.name == Name);
        if(own_dialogues != null){
           savedData.data.FirstOrDefault(e => e.name == Name).Dequeue(ID);
           Debug.Log("Dequeing " + ID);
        }
        WritingData();
        UpdateNotification();
    }
    public void StartDialogue(int[] IDs){
        StartDialogue(IDs[UnityEngine.Random.Range(0, IDs.Length)]);
    }
    public void StartDialogue(int ID){
        Debug.Log("Starting Dialogue: " + Name + " " + ID);
        RunTimeDialogues own_dialogues = runTimeDialogues[ID];
        UnityEvent afterEvent = own_dialogues.afterEvent;

        if(own_dialogues.dequeueAuto){
           afterEvent.AddListener(()=>DequeueDialogue(ID));
        }
        
        StartCoroutine(Chat.Instance.StartDialogue(own_dialogues.dialogues, Name, afterEvent, !own_dialogues.dontEndAfter));
    }
    private void StartQueuedDialogue(){
        CharacterSavedDialogues own_dialogues = savedData.data.FirstOrDefault(e => e.name == Name);
        if(own_dialogues==null || own_dialogues.saved_dialogues.Length == 0){return;}
        StartDialogue(own_dialogues.saved_dialogues[0]);
    }
    public virtual void ClickedCharacter(){
        if(hasAvailableDialogue()){
            StartQueuedDialogue();
        }else{
            DefaultClickBehaviour.Invoke();
        }
    }
    public virtual void DefaultBehaviour(){
        DefaultClickBehaviour.Invoke();
    }
    protected virtual bool hasPingNotification(){
        return false;
    }
    protected void UpdateNotification(object sender = null, EventArgs e= null){
        transform.GetChild(0).gameObject.SetActive(hasPingNotification() || hasAvailableDialogue());
    }

    protected void SetVariable(string name, int value){
        GameVariables.SetVariable(name, value); 
    }

    

    /* ===== I/O FUNCTIONS ===== */
    private static void ReadBestiaryData(){
        if(File.Exists(Application.persistentDataPath +"/npcs.json")){
            string json = File.ReadAllText(Application.persistentDataPath +"/npcs.json");
            savedData = JsonUtility.FromJson<NPCSaveData>(json);
           
        }else{
            savedData = new NPCSaveData();
            savedData.data = new List<CharacterSavedDialogues>();
        }
        hasRead = true;
    }

    public static void WritingData(){
        Debug.Log("Saving NPC Data");
        string json = JsonUtility.ToJson(savedData);
        Debug.Log(json);
        File.WriteAllText(Application.persistentDataPath + "/npcs.json", json);
    }

    float SpriteScale;
    IEnumerator ScaleCoroutine;
    protected int HoverCursorType = 1;
    public void SetHovered(bool check) {
        if(check){
            
            AudioManager.PlayOneShot(FMODEvents.Instance.BubblePop , transform.position);
        }
        
        InteractiveCursor.ChangeCursor(check? HoverCursorType : 0);
        if(ScaleCoroutine != null){
            StopCoroutine(ScaleCoroutine);
        }
        ScaleCoroutine = SetHoveredCoroutine(check);
        StartCoroutine(ScaleCoroutine);
        //GetComponent<Animator>().SetBool("NPCHovering", check);
    }
    public IEnumerator SetHoveredCoroutine(bool check){
        RectTransform rt = transform.Find("Notification").GetComponent<RectTransform>();
        if(check){
            
            while(GetComponent<RectTransform>().sizeDelta.x < SpriteScale*1.1f){
                GetComponent<RectTransform>().sizeDelta = new Vector2(GetComponent<RectTransform>().sizeDelta.x * 1.01f,GetComponent<RectTransform>().sizeDelta.y* 1.01f);
                rt.localScale = new Vector2(rt.localScale.x*1.01f,rt.localScale.y*1.01f);
                yield return null;
            }
            GetComponent<RectTransform>().sizeDelta = new Vector2(SpriteScale*1.1f,SpriteScale*1.1f);
            rt.localScale = new Vector2(1.1f,1.1f);
        }else{
            
            while(GetComponent<RectTransform>().sizeDelta.x > SpriteScale){
                GetComponent<RectTransform>().sizeDelta = new Vector2(GetComponent<RectTransform>().sizeDelta.x * .99f,GetComponent<RectTransform>().sizeDelta.y * .99f);
                rt.localScale = new Vector2(rt.localScale.x*.99f,rt.localScale.y*.99f);
                yield return null;
            }
            GetComponent<RectTransform>().sizeDelta = new Vector2(SpriteScale,SpriteScale);
            rt.localScale = new Vector2(1,1);
        }
        
        
        
    }
    

    public void UnlockQuest(int n){

        GameVariables.UnlockQuest(n);

        PopUpQuestNotification(n);

        QuestBoard.ReloadQuests();
    }
    public void CompleteQuest(int n){
        GameVariables.CompleteQuest(n);
        QuestBoard.ReloadQuests();
    }
    
    
    public void PopUpQuestNotification(int n){
        int[] planetException = new int[]{36,37,38,39,40,42,43};
        int[] immolateException = new int[]{32,33,34,35};  
        if(!(planetException.Contains(n)||immolateException.Contains(n))){
            QuestBoard.PopUpQuest(n);    
        } 
        else if(n==32){
            QuestBoard.PopUpImmolateQuest();
        }
    }
    
    
   
}


public class GameVariables{
    [System.Serializable]
    class Variable{
        public string name;
        public int value;
    }
    
    [Serializable]
    class VariableList{
        public int[] OnGoingQuests = new int[0];
        public int[] CompletedQuests = new int[0];

        public List<Variable> variables;
    }
    private static GameVariables Instance;
    private VariableList variableList;
    private GameVariables(){ReadData(); Instance = this;}
    public static GameVariables getInstance(){
        if(Instance==null){return new GameVariables();}
        else{return Instance;}
    }
    
    private void ReadData(){
        if(File.Exists(Application.persistentDataPath +"/variables.json")){
            string json = File.ReadAllText(Application.persistentDataPath +"/variables.json");
            variableList = JsonUtility.FromJson<VariableList>(json);
           
        }else{
            variableList = new VariableList();
            variableList.variables = new List<Variable>();
        }
    }

    public void WritingData(){
        string json = JsonUtility.ToJson(variableList);
        File.WriteAllText(Application.persistentDataPath + "/variables.json", json);
    }
    
    public static void SetVariable(string name, int value){
        Debug.Log("Setting Variable: " + name + " to:" + value);
        GameVariables gv = getInstance();

        if(gv.variableList.variables.Any(i => i.name == name)){
            gv.variableList.variables.Find(i => i.name == name).value = value;
        }else{
            gv.variableList.variables.Add(new Variable(){name= name, value = value});
        }

        getInstance().WritingData();
    }
    public static int GetVariable(string name){
        GameVariables gv = getInstance();
        if(gv.variableList.variables.Any(i => i.name == name)){
            return gv.variableList.variables.Find(i => i.name == name).value;
        }else{
            return -1;
        }
    }



    public static void UnlockQuest(int id){
        Debug.Log("Quest Started: " + id);
        int[] OnGoing = getInstance().variableList.OnGoingQuests;
        if(OnGoing.Contains(id)){
            Debug.Log("Quest was already Active: " + id);
        }else if(!getInstance().variableList.CompletedQuests.Contains(id)){
            getInstance().variableList.OnGoingQuests = OnGoing.Append(id).ToArray();
            getInstance().WritingData();
        }
        if(GetVariable("QuestBookReady") == -1){
            if(getInstance().variableList.OnGoingQuests.Length + getInstance().variableList.CompletedQuests.Length >= 2){
                SetVariable("QuestBookReady", 0);
            }
        }
        

        QuestBoard.ReloadQuests();
        
    }
    
    public static bool CompleteQuest(int id){
        Debug.Log("Quest Completed: " + id);
        int[][] Quests = GetQuests();
        if(Quests[1].Contains(id)){
            Debug.Log("Quest was already Completed: " + id);
            return false;
        }else{
            getInstance().variableList.OnGoingQuests =  Quests[0].Where(x => x != id).ToArray();
            getInstance().variableList.CompletedQuests = getInstance().variableList.CompletedQuests.Append(id).ToArray();

            getInstance().WritingData();
            
        }

        QuestBoard.ReloadQuests();
        return true;


    }
    public static bool hasQuestCompleted(int id){
        return getInstance().variableList.CompletedQuests.Contains(id);
    }
    public static bool hasQuest(int id){
        return getInstance().variableList.OnGoingQuests.Contains(id);
    }
    public static bool hasQuestAssigned(int id){
        return hasQuest(id) || hasQuestCompleted(id);
    }
    public static int[][] GetQuests(){
        GameVariables gv = getInstance();
        return new int[][] {gv.variableList.OnGoingQuests,gv.variableList.CompletedQuests};
    }

    



}

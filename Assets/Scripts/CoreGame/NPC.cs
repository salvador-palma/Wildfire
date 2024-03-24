using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

//string msg,Sprite avatar, string[] optionTxt = null, UnityAction[] optionAction = null
[System.Serializable]
public class Dialogue{
    public string message;
    public Sprite avatar;
    
}
[System.Serializable]
public class RunTimeDialogues{ 
    public Dialogue[] dialogues;
    public UnityEvent afterEvent;
    public bool dequeueAuto;
}
[System.Serializable]
public class CharacterSavedDialogues{
    public string name;
    public int[] saved_dialogues = new int[0];
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

    private void Start() {
        if(!hasRead){
            ReadBestiaryData();
        }
        // if(Name=="Rowl"){
        //     QueueDialogue(0);
        // }
        UpdateNotification();
        LocalBestiary.INSTANCE.ClaimRewardEvent += UpdateNotification;
    }

    public bool hasAvailableDialogue(){
        return savedData.data.Any(e => e.name == Name && e.saved_dialogues.Length > 0);
    }

    public void QueueDialogue(int ID){
        CharacterSavedDialogues own_dialogues = savedData.data.FirstOrDefault(e => e.name == Name);
        if(own_dialogues == null){
            own_dialogues = new CharacterSavedDialogues{name = Name,saved_dialogues = new int[1]{ID}};
            savedData.data.Add(own_dialogues);
        }else{
            if(!own_dialogues.saved_dialogues.Contains(ID)){
                own_dialogues.saved_dialogues.Append(ID);
            }
                
        }
        Debug.Log("Writing " + savedData.data.Count());
        WritingData();
        UpdateNotification();
    }
    public void DequeueDialogue(int ID){
        CharacterSavedDialogues own_dialogues = savedData.data.FirstOrDefault(e => e.name == Name);
        if(own_dialogues != null){
           own_dialogues.saved_dialogues = own_dialogues.saved_dialogues.Where((value, index) => index != ID).ToArray();
        }
        WritingData();
        UpdateNotification();
    }

    public void StartDialogue(int ID){
        CharacterSavedDialogues own_dialogues = savedData.data.FirstOrDefault(e => e.name == Name);
        if(own_dialogues==null || !own_dialogues.saved_dialogues.Contains(ID)){return;}
        UnityEvent afterEvent = runTimeDialogues[ID].afterEvent;

        if(runTimeDialogues[ID].dequeueAuto){
           afterEvent.AddListener(()=>DequeueDialogue(ID));
        }
        StartCoroutine(MetaMenuUI.Instance.StartDialogue(runTimeDialogues[ID].dialogues, afterEvent));
    }
    private void StartQueuedDialogue(){
        CharacterSavedDialogues own_dialogues = savedData.data.FirstOrDefault(e => e.name == Name);
        if(own_dialogues==null || own_dialogues.saved_dialogues.Length == 0){return;}
        StartDialogue(own_dialogues.saved_dialogues[0]);
    }
    public void ClickedCharacter(){
        if(hasAvailableDialogue()){
            StartQueuedDialogue();
        }else{
            DefaultClickBehaviour.Invoke();
        }
    }
    private bool hasPingNotification(){
        switch (Name)
        {
            case "Betsy":
                int claims = LocalBestiary.AvailableClaims;
                return claims > 0;

            default:
                return false;
                   
        } 
    }
    private void UpdateNotification(object sender = null, EventArgs e= null){
        transform.GetChild(0).gameObject.SetActive(hasPingNotification() || hasAvailableDialogue());
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
        Debug.Log("Writing " + savedData.data.Count());
        foreach (CharacterSavedDialogues item in savedData.data)
        {
            Debug.Log(item.name);
        }
        string json = JsonUtility.ToJson(savedData);
        Debug.Log(json);
        File.WriteAllText(Application.persistentDataPath + "/npcs.json", json);
    }

   
}

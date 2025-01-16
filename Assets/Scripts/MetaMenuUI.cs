using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Serialization;
using FMODUnity;
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
    [SerializeField] private GameObject MarketPanel;
    [SerializeField] private Naal naal;
    [SerializeField] private GameObject BestiaryPanel;
    [SerializeField] private GameObject SkillTreePanel;
    [SerializeField] private GameObject CharacterSelectPanel;
    [SerializeField] private GameObject SettingsSelectPanel;
    [SerializeField] public GameObject SkillTree;
    static public MetaMenuUI Instance;
    

    [Header("Unlockable")]
    [SerializeField] TextMeshProUGUI[] UnlockableTexts;
    [SerializeField] Image UnlockableIcon;
    [SerializeField] Sprite[] Unlockables;


    public event System.EventHandler NightFall;


    private void Awake() {
        Instance = this;
        
        QualitySettings.vSyncCount = 0;
		Application.targetFrameRate = -1;

        int origin = PlayerPrefs.GetInt("Origin", -1);
        if(origin == -1){
            GetComponent<Animator>().Play("Intro");
        }else{
            PlayerPrefs.DeleteKey("Origin");
            GetComponent<Animator>().Play("CurtainsOff");
        }
        

    }
    public void StartGame(){
        SceneManager.LoadScene("Game");
    }
    public void PlayOutro(){
        GetComponent<Animator>().Play("Outro");
    }
    public void NatureToNightTransition(){
        NightFall?.Invoke(this, new EventArgs());
    }
    
    public void SkillTreeMenuToggle(){
        
        SkillTreeManager.Instance.toggleSkillTree(SkillTreePanel);
    }
    public void CharacterSelectMenuToggle(){
        Character.Instance.toggleCharacterPanel(CharacterSelectPanel);
        
    }
    public void SettingsMenuToggle(){
        ToggleMenu(SettingsSelectPanel);   
    }
    public bool ToggleMenu(GameObject panel){
        Vector2 newPos = new Vector2(panel.GetComponent<RectTransform>().anchoredPosition.x > 2000 ? 0 : 4000, 0);
        AudioManager.Instance.SetAmbienceParameter("OST_Volume", newPos.x <= 0? 0 : 1);
        panel.GetComponent<RectTransform>().anchoredPosition = newPos;
        return newPos.x <= 0;
    }

    public void BestiaryMenuToggle(){
        bool state = ToggleMenu(BestiaryPanel);
        if(state){LocalBestiary.INSTANCE.UpdateBlackMarketItems();}
    }
    public void MarketMenuToggle(){
        ToggleMenu(MarketPanel);
        if(GameVariables.GetVariable("JhatIntroduction") != 1){
            naal.StartDialogue(2);
        }

    }
 
    public void UpgradeButton(){
        SkillTreeButton selected = SkillTreeButton.SelectedButton;
        if(selected != null){selected.ClickedUpgrade();}
        
    }
    private IEnumerator currentCouroutine;
    public Vector2 IdealPos;
    public void moveSkillTree(Transform buttonTr){
        
        
        
        if(currentCouroutine!=null){StopCoroutine (currentCouroutine);}
        currentCouroutine = SmoothLerp (buttonTr);
        StartCoroutine (currentCouroutine);
    }
    
    private IEnumerator SmoothLerp (Transform buttonTr)
    {        
        float elapsed = 0;
        RectTransform rt = SkillTree.GetComponent<RectTransform>(); 

        while(elapsed<2f && Vector3.Distance( buttonTr.position, IdealPos) > .1f){

            Vector2 dir = IdealPos - (Vector2)buttonTr.position;
            rt.position = (Vector2)rt.position + dir * Time.deltaTime * 2f;
            elapsed+=Time.deltaTime;
            yield return null;
        }
            
        
    }
    public void Update(){
        if(Input.GetAxis("Mouse ScrollWheel") != 0 && SkillTreePanel.GetComponent<RectTransform>().anchoredPosition.x <= 0){
            if(currentCouroutine!=null){StopCoroutine(currentCouroutine);}
            float f = Input.GetAxis("Mouse ScrollWheel");
            float cur = SkillTree.transform.localScale.x;
            cur *= 1+f;
            float v = Math.Clamp(cur, 0.267755f,  2.88112f);

            SkillTree.transform.localScale = new Vector2(v,v);
        }

        if(Input.GetKeyDown(KeyCode.Escape)){
            SettingsMenuToggle();
        }
    }

    public bool SaveStateEnabled = true;
    public void ClickedPlay(){
        Debug.Log(Application.persistentDataPath);
        if(File.Exists(Application.persistentDataPath +"/gameState.json") && SaveStateEnabled){
            
            Chat.Instance.StartChat();
            Chat.Instance.ChatSingular("Do you wish to continue your previous unfinished run?",
                            Chat.Instance.AvatarBank[0], "Rowl",
                            new string[2]{"Yes", "No"},
                            new UnityAction[2]{
                                new UnityAction(()=>{PlayerPrefs.SetInt("PlayerLoad", 1); Chat.Instance.EndChat(); PlayOutro();}),
                                new UnityAction(()=>{PlayerPrefs.SetInt("PlayerLoad", 0); GameState.Delete(); Chat.Instance.EndChat(); PlayOutro();})
                            });
        }else{
            
            PlayOutro();
        }
    }

    
    

    public void GoCasino(){
        SceneManager.LoadScene("Casino");
    }

    public void UnlockableScreen(string title, string name, string description, int iconID){
        UnlockableScreen(title, name, description, Unlockables[iconID]);
    }
    public void UnlockableScreen(string title, string name, string description, Sprite icon){

        AudioManager.Instance.SetAmbienceParameter("OST_Intensity", 0);
        AudioManager.PlayOneShot(FMODEvents.Instance.UnlockedEffect, transform.position);
        UnlockableTexts[0].text = title;
        UnlockableTexts[1].text = name;
        UnlockableTexts[2].SetText(description);
        UnlockableIcon.sprite = icon;
        UnlockableIcon.transform.parent.GetComponent<Animator>().Play("UnlockableOn");

    }
    
    public void UnlockOff(){
        AudioManager.Instance.SetAmbienceParameter("OST_Intensity", 1);
        UnlockableIcon.transform.parent.GetComponent<Animator>().Play("UnlockableOff");
    }


    

}



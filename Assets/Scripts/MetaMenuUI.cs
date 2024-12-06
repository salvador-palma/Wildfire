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
    [SerializeField] private GameObject MarketPanel;
    [SerializeField] private GameObject BestiaryPanel;
    [SerializeField] private GameObject SkillTreePanel;
    [SerializeField] private GameObject CharacterSelectPanel;
    [SerializeField] public GameObject SkillTree;
    static public MetaMenuUI Instance;
    

    [Header("Unlockable")]
    [SerializeField] TextMeshProUGUI[] UnlockableTexts;
    [SerializeField] Image UnlockableIcon;
    [SerializeField] Sprite[] Unlockables;
    private void Awake() {
        Instance = this;
        
        QualitySettings.vSyncCount = 0;
		Application.targetFrameRate = -1;
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
        Vector2 newPos = new Vector2(BestiaryPanel.GetComponent<RectTransform>().anchoredPosition.x > 2000 ? 0 : 4000, 0);
        
        AudioManager.Instance.SetAmbienceParameter("OST_Volume", newPos.x <= 0? 0 : 1);

        
        BestiaryPanel.GetComponent<RectTransform>().anchoredPosition = newPos;

        
    }
    public void MarketMenuToggle(){
        MarketPanel.GetComponent<RectTransform>().anchoredPosition = new Vector2(BestiaryPanel.GetComponent<RectTransform>().anchoredPosition.x > 2000 ? 0 : 7000, 0);
    }
 
    public void UpgradeButton(){
        SkillTreeButton selected = SkillTreeButton.SelectedButton;
        if(selected != null){selected.ClickedUpgrade();}
        
    }
    private IEnumerator currentCouroutine;
    public void moveSkillTree(Transform buttonTr){
        if(currentCouroutine!=null){StopCoroutine (currentCouroutine);}
        currentCouroutine = SmoothLerp (buttonTr);
        StartCoroutine (currentCouroutine);
    }
    
    private IEnumerator SmoothLerp (Transform tr)
    {
        Vector3 Target = new Vector3(-3f,0,0);
        
        Transform button = tr.Find("Icon");
        Vector2 direction = Target - button.position;
        
        
        
        float elapsed = 0;
        while(elapsed<2f && Vector3.Distance( button.position, Target) > .1f){
            SkillTree.transform.position = (Vector2)SkillTree.transform.position + direction * Time.deltaTime;
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



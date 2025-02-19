using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[Serializable]
public class MiniGameProps{
    public string name;
    public GameObject[] prefab;
}
public class Casino : MonoBehaviour
{
    static int offset = 0;
    public List<MiniGameProps> minigames;
    public GameObject settingsPanel;
    public static string getMinigame(){
        DateTime d = DateTime.Today;
        switch((int)d.DayOfWeek % 3 + offset){
            case 0: return "Drop the Acorn";
            case 1: return "Water the Flower";
            case 2: 
            default:
            return "Frog Jump";
        }
    }
    public static int getMinigameID(){
        DateTime d = DateTime.Today;
        return ((int)d.DayOfWeek % 3 ) + offset;
        
    }
    private void Awake() {
        foreach(GameObject go in minigames.Find(x => x.name == getMinigame()).prefab){
            go.SetActive(true);
        }
        
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)){
            ToggleSettings();
        }
    }
    void ToggleSettings(){
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
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Item : MonoBehaviour
{

    public static Dictionary<Item, int> Items;

    [SerializeField] public string Name;
    [SerializeField] public Item[] Unlocks;
    [SerializeField] bool initial;
    
    [HideInInspector] public int level;

    [SerializeField] UnityEvent AfterUnlock;




    public int MinimumPrice;
    public Sprite Icon;
    [TextArea] public string Description;
    [SerializeField] public Dialogue[] presentation;


    [Header("References")]
    public Transform DisplayPanel;
    public Naal naal;
    static int itemCount = 0;
    public void ItemStart() {
        if(Items == null){Items = new Dictionary<Item, int>();}
        Transform parent = transform.parent;
        
        level = GameVariables.GetVariable(Name + " Item");
        if(level == -1){
            level = initial ? 0 : level;
            GameVariables.SetVariable(Name + " Item" , level);
        }
        Items[this] = level;
        
        if(level!= 0 || itemCount >= 6){
            gameObject.SetActive(false);
            if(level==0){
                Debug.Log("Limit: " + Name);
            }
        }else{
            itemCount++;
            GetComponent<Button>().onClick.AddListener(()=>Display(true));
        }
    }
    
    
    public void Purchase(){
        MetaMenuUI.Instance.UnlockableScreen("NEW ITEM ACQUIRED!", Name, Description, 1);
        gameObject.SetActive(false);
        AfterUnlock?.Invoke();
        Unlock();
    }
    public void Unlock(){
        
        level = 1;
        GameVariables.SetVariable(Name + " Item" , level);
        foreach (Item item in Unlocks)
        {
            GameVariables.SetVariable(item.Name + " Item" , 0);
        } 
    }

    static public int getLevel(string name){
        return Items.Where(k => k.Key.Name == name).FirstOrDefault().Value;
    }
    static public bool has(string name){
        return Items.Where(k => k.Key.Name == name).FirstOrDefault().Value > 0;
    }

    public void Display(bool on){
        AudioManager.PlayOneShot(FMODEvents.Instance.PaperSlide, transform.position);
        if(on){
            DisplayPanel.Find("Icon").GetChild(0).GetComponent<Image>().sprite = Icon;
            DisplayPanel.Find("Description").GetComponent<DynamicText>().SetText(Description);
            DisplayPanel.Find("Title").GetComponent<DynamicText>().SetText("<style=\"Yellow\">"+ Name);

            Button NoButton = DisplayPanel.Find("Not Interested").GetComponent<Button>();
            NoButton.onClick.RemoveAllListeners(); NoButton.onClick.AddListener(()=>Display(false));

            Button YesButton = DisplayPanel.Find("Bargain").GetComponent<Button>();
            YesButton.onClick.RemoveAllListeners(); YesButton.onClick.AddListener(()=>{naal.BargainItem(this);Display(false);});
            
        }
        DisplayPanel.gameObject.SetActive(!DisplayPanel.gameObject.activeInHierarchy);
    }

}

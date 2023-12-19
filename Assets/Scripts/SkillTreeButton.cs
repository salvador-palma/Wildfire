using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SkillTreeButton : MonoBehaviour
{
    const string UNLOCKED = "SkillTreeButtonUnlocked";
    const string LOCKED = "SkillTreeButtonLocked";
    const string UNLOCKING = "SkillTreeButtonUnlock";


    [SerializeField] bool UnlockableField;
    [SerializeField] string[] UnlockableClasses;

    [SerializeField] string AugmentClass;
   
    [SerializeField] List<SkillTreeButton> previousNode;
    [SerializeField] List<SkillTreeLine> nextPaths;

    [SerializeField] int Max_Upgrades;
    public int upgradeAmount;
    private bool isBaseStat;

    //MEANINGS
    //-1 => NOT DISCOVERED
    //0 => BUYABLE
    //1 => BOUGHT

    void Start()
    {
        isBaseStat = Max_Upgrades==5;
        int storedValue = PlayerPrefs.GetInt(AugmentClass, -5);
        if(isBaseStat){setUpgradeAmount(1);GetComponent<Animator>().Play(UNLOCKED);}
        if(storedValue != -5){LoadUpgrade(storedValue);}
        GetComponent<Button>().onClick.AddListener(Clicked);
        
    }
    public void ping(){
        bool result =true;
        foreach (SkillTreeButton item in previousNode)
        {
            result = result && item.wasBought();
        }
        if(result){Unlock(Max_Upgrades == 3 ? 1 : 0);}
    }
    private void Unlock(int atPhase){
        setUpgradeAmount(atPhase);
        GetComponent<Animator>().Play(UNLOCKING);
    }
    private void Clicked(){
        //CHECK FOR PRICE
        if(upgradeAmount==Max_Upgrades || upgradeAmount < 0){return;}

        Upgrade();
    }
    private void Upgrade(){
        if(UnlockableField){
            DeckBuilder.Instance.UnlockClass(AugmentClass, UnlockableClasses);
        }else{
            DeckBuilder.Instance.UpgradeClass(AugmentClass);
        }
        setUpgradeAmount(upgradeAmount+1);
        if(upgradeAmount==2 && isBaseStat || upgradeAmount == 1 && !isBaseStat){NextPaths(true);}
    }
    
    public void NextPaths(bool withPing){
        foreach (SkillTreeLine item in nextPaths)
        {
            item.PlayUnlock(withPing);
        }
    }

    private void setUpgradeAmount(int n){
        upgradeAmount = n;
        transform.Find("FillImage").GetComponent<Image>().color = SkillTreeManager.Instance.getColor(n, Max_Upgrades);
        PlayerPrefs.SetInt(AugmentClass, upgradeAmount);
    }

    public bool wasBought(){
        return upgradeAmount>=2 && isBaseStat || upgradeAmount >= 1 && !isBaseStat; 
    }

    public void LoadUpgrade(int at){
        for (int i = isBaseStat? 1:0; i < at; i++)
        {
            if(UnlockableField){
                DeckBuilder.Instance.UnlockClass(AugmentClass, UnlockableClasses);
            }else{
                DeckBuilder.Instance.UpgradeClass(AugmentClass);
            }
        }
        setUpgradeAmount(at);
        GetComponent<Animator>().Play(UNLOCKED);
        if(upgradeAmount>=2 && isBaseStat || upgradeAmount >= 1 && !isBaseStat){NextPaths(false);}
    }

     
}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Betsy : NPC
{
    protected override void CharacterLoad()
    {
        /* GAME VARIABLES
        |   * "BeeQuest"
        |    0 - Not Ready
        |    1 - Given
        |    2 - Completed
        |   * "BestiaryReady"
        |    0 - No
        |    1 - Yes
        |  * "BinocularLevel":
        |    -1 - Nothing Unlocked
        |    1 - 10 Viewed
        |    2 - 10 Bronze
        |    3 - 15 Silver
        |    4 - 25 Gold
        |
        */
        LocalBestiary B = LocalBestiary.INSTANCE;

        if(GameVariables.GetVariable("BestiaryReady") <= 0 && B.get_Amount_Of_Enemies_With_Milestones_Above(0) >= 3){
            GameVariables.SetVariable("BestiaryReady", 1); 
            QueueDialogue(1);
        }
        if(GameVariables.GetVariable("BeeQuest") <= 0 && B.get_Amount_Of_Enemies_With_Milestones_Above(0) >= B.get_Amount_Of_Enemies_With_Milestones_Above(-1)/2){
            GameVariables.SetVariable("BeeQuest", 1); 
            QueueDialogue(6);
        }
        if(GameVariables.GetVariable("BeeQuest") == 1 && B.get_Amount_Of_Enemies_With_Milestones_Above(0) == B.get_Amount_Of_Enemies_With_Milestones_Above(-1)){
            GameVariables.SetVariable("BeeQuest", 2); 
            QueueDialogue(7);
        }


        
        int binocularLvl = GameVariables.GetVariable("BinocularLevel");
        if(binocularLvl == -1){
            GameVariables.SetVariable("BinocularLevel",0);
            binocularLvl = 0;
        }
        
        switch(binocularLvl){
            case 0:
            Debug.Log(B.get_Amount_Of_Enemies_With_Milestones_Above(0));
                if(B.get_Amount_Of_Enemies_With_Milestones_Above(0) >= 10){
                    QueueDialogue(2);}
                break;
            case 1:
                if(B.get_Amount_Of_Enemies_With_Milestones_Above(B.milestones[0]) >= 10){ 
                    QueueDialogue(3);}
                break;
            case 2:
                if(B.get_Amount_Of_Enemies_With_Milestones_Above(B.milestones[1]) >= 15){ 
                    QueueDialogue(4);}
                break;
            case 3:
                if(B.get_Amount_Of_Enemies_With_Milestones_Above(B.milestones[2]) >= 25){ 
                    QueueDialogue(5);}
                break;
        }

        
    }

    public void SetBinocularLevel(int n){
         GameVariables.SetVariable("BinocularLevel",n);
         CharacterLoad();
         UpdateNotification();
         switch(n){
            case 1:
                MetaMenuUI.Instance.UnlockableScreen("UNLOCKED", "FLIMSY BINOCULARS", "Allows you to see what <color=#FFCC7C>enemy</color> will come out next so you can be prepared. To use them just open the <b><color=#FFCC7C>Bestiary</color></b> tab in-game", 1);
                break;
            case 2:
                MetaMenuUI.Instance.UnlockableScreen("UPGRADED", "LIGHTWEIGHT BINOCULARS", "Allows you to see the next <color=#FFCC7C>2 enemies", 1);
                break;
            case 3:
                MetaMenuUI.Instance.UnlockableScreen("UPGRADED", "STURDY BINOCULARS", "Allows you to see the next <color=#FFCC7C>3 enemies", 1);
                break;
            case 4:
                MetaMenuUI.Instance.UnlockableScreen("UPGRADED", "HIGH-TECH BINOCULARS", "Allows you to see the next <color=#FFCC7C>4 enemies", 1);
                break;
         }
          
    }
    public void unlockBestiary(){
        MetaMenuUI.Instance.UnlockableScreen("UNLOCKED", "BETSY'S BESTIARY", "You can now check the stats and abilities of <color=#FFCC7C>animals</color> you've defeated and gather <color=#FFCC7C>milestones</color>", 2);
    }
    public void Default(){
        DefaultClickBehaviour.Invoke();
    }
    [SerializeField] NPC Rowl;
    public void UnlockBees(){
        MetaMenuUI.Instance.UnlockableScreen("NEW SKILL DISCOVERED", "BEE SUMMONER", "You can now unlock the <color=#FFCC7C>Bee Summoner</color> ability, go talk to <color=#FFCC7C><sprite name=\"Rowl\">Rowl", 0);
        Rowl.QueueDialogue(2);
        SkillTreeManager.Instance.Upgrade("SummonUnlock");
        SkillTreeManager.Instance.InvokeUIReset();
    }
    public override void ClickedCharacter(){

        if(GameVariables.GetVariable("BestiaryReady") <= 0){
            StartDialogue(0);
            return;
        }
        base.ClickedCharacter();
    }

    protected override bool hasPingNotification()
    {
        int claims = LocalBestiary.AvailableClaims;
        return claims > 0 && GameVariables.GetVariable("BestiaryReady") > 0;
    }

}

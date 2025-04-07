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

        if(GameVariables.GetVariable("BestiaryReady") <= 0 && B.get_Amount_Of_Enemies_With_Milestones_Above(0) >= 5){
            GameVariables.SetVariable("BestiaryReady", 1); 
            CompleteQuest(0);
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
            
                if(B.get_Amount_Of_Enemies_With_Milestones_Above(B.milestones[0]) >= 5 &&  GameVariables.GetVariable("BlackMarketReady") < 0){
                    
                    CompleteQuest(1);
                    QueueDialogue(9); //call nall
                }
                break;
            case 1:
                if(B.get_Amount_Of_Enemies_With_Milestones_Above(B.milestones[1]) >= 10){ 
                    
                    CompleteQuest(2);
                    QueueDialogue(3);
                    }
                break;
            case 2:
                if(B.get_Amount_Of_Enemies_With_Milestones_Above(B.milestones[2]) >= 15){ 
                    CompleteQuest(3);
                    QueueDialogue(4);}
                break;
            case 3:
                if(B.get_Amount_Of_Enemies_With_Milestones_Above(B.milestones[3]) >= 20){ 
                    CompleteQuest(4);
                    QueueDialogue(5);}
                break;
        }

        if(GameVariables.GetVariable("ShinyTalk")==0){
            QueueDialogue(8);
            GameVariables.SetVariable("ShinyTalk",1);
        }

        
    }
    public void CallNaal(){
        GameVariables.SetVariable("BlackMarketReady",0);
    }

    public void SetBinocularLevel(int n){
        if(n < GameVariables.GetVariable("BinocularLevel")){return;}
         GameVariables.SetVariable("BinocularLevel",n);
         CharacterLoad();
         UpdateNotification();
         switch(n){
            case 1:
                MetaMenuUI.Instance.UnlockableScreen("UNLOCKED", "FLIMSY BINOCULARS", "Allows you to see what <style=\"LYellow\">enemy</style> will come out next so you can be prepared. To use them just open the <b><style=\"LYellow\">Bestiary</style></b> tab in-game", 1);
                break;
            case 2:
                MetaMenuUI.Instance.UnlockableScreen("UPGRADED", "LIGHTWEIGHT BINOCULARS", "Allows you to see the next <style=\"LYellow\">2 enemies", 1);
                break;
            case 3:
                MetaMenuUI.Instance.UnlockableScreen("UPGRADED", "STURDY BINOCULARS", "Allows you to see the next <style=\"LYellow\">3 enemies", 1);
                break;
            case 4:
                MetaMenuUI.Instance.UnlockableScreen("UPGRADED", "HIGH-TECH BINOCULARS", "Allows you to see the next <style=\"LYellow\">4 enemies", 1);
                break;
         }
          
    }
    public void unlockBestiary(){
        MetaMenuUI.Instance.UnlockableScreen("UNLOCKED", "BETSY'S BESTIARY", "You can now check the stats and abilities of <style=\"LYellow\">animals</style> you've defeated and gather <style=\"LYellow\">milestones</style>", 2);
    }
    public void unlockShiny(){
        MetaMenuUI.Instance.UnlockableScreen("UNLOCKED", "SHINY TRACKER", "You can now keep track of <sprite name=\"Shiny\"> Shiny animals that you've encountered", 5);
        QueueDialogue("Gyomyo", 8);
    }
    public void Default(){
        DefaultClickBehaviour.Invoke();
    }
    [SerializeField] NPC Rowl;
    [SerializeField] NPC Naal;
    public void UnlockBees(){
        MetaMenuUI.Instance.UnlockableScreen("NEW SKILL DISCOVERED", "BEE SUMMONER", "You can now unlock the <style=\"LYellow\">Bee Summoner</style> ability, go talk to <color=#FFCC7C><sprite name=\"Rowl\"> Rowl", 0);
        Rowl.QueueDialogue(2);
        SkillTreeManager.Instance.Upgrade("Bee Summoner", Unlock:true);
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

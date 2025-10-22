using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class Gyomyo : NPC
{

    static public long GetNetWorth()
    {
        return Math.Min(2147483647, SkillTreeManager.Instance.PlayerData.embers + SkillTreeManager.Instance.PlayerData.skillTreeEmbers + Math.Max(0, GameVariables.GetVariable("EmbersSpentOnItems")));
    }
    public void ReloadAfterTreeReset()
    {
        
        long NetWorth = GetNetWorth();
        if(NetWorth >= 250000 && GameVariables.GetVariable("CasinoReady") == 0){
            QueueDialogue(4);
        }
    }
    protected override void CharacterLoad()
    {
        long NetWorth = GetNetWorth();

        if(NetWorth < 100000 && GameVariables.GetVariable("CasinoReady") == -1){

            gameObject.SetActive(false);

        }else if(NetWorth >= 100000 && GameVariables.GetVariable("CasinoReady") == -1){
            
            QueueDialogue(0);
            GameVariables.SetVariable("CasinoReady", 0);

        }
        if(NetWorth >= 250000 && GameVariables.GetVariable("CasinoReady") == 0){
            QueueDialogue(4);
        }

    }
    public override void ClickedCharacter(){

        long NetWorth = GetNetWorth();
        long holdingValue = Math.Min(2147483647, SkillTreeManager.Instance.PlayerData.embers);

        if (hasAvailableDialogue()) { base.ClickedCharacter(); return; }

    
        if(GameVariables.GetVariable("CasinoReady") == 0 && NetWorth < 250000){
            float f = UnityEngine.Random.Range(0f,1f);
            if(f<.33f){
                StartDialogue(1);
            }else if(f<.66f){
                StartDialogue(2);
            }else{
                StartDialogue(3);
            }
             //EXTRA DIALOGUES
            return;
        }

        if(GameVariables.GetVariable("CasinoReady") == 1 && holdingValue < 250000){
            
            if(Math.Min(2147483647, SkillTreeManager.Instance.PlayerData.embers + SkillTreeManager.Instance.PlayerData.skillTreeEmbers) < 250000)
            {
                StartDialogue(5); 
            }else{
                StartDialogue(6); 
            }

            return;
        }

        if(GameVariables.GetVariable("CasinoReady") >= 1 && holdingValue >= 250000){
            InviteCasino();

            return;
        }

        
        
        
    }
    public void InviteCasino(){
        string invitation = "How's it going? Wanna come down to the casino?";
        Chat.Instance.StartChat();
        Chat.Instance.ChatSingular(invitation,
                        Chat.Instance.AvatarBank[1], name:"Gyomyo",
                        optionTxt:new string[2]{"No", "Yes"},
                        optionAction:new UnityAction[2]{
                            new UnityAction(()=>{Chat.Instance.EndChat();}),
                            new UnityAction(()=>{Chat.Instance.EndChat(); GoCasino();})
        });
    }
    public void GoCasino(){
        MetaMenuUI.Instance.GetComponent<Animator>().Play("CurtainsOn");
        SteamLeaderboardManager.UnlockAchievment("GYOMYO_CASINO");
        
    }

    public void SetVariable(int n){
        base.SetVariable("CasinoReady",n);
    }
    
    public void UnlockCasino(){
        UnityAction post = () => UnlockQuest(45);
        MetaMenuUI.Instance.UnlockableScreen("UNLOCKED", "GYOMYO'S CASINO", "You can now <style=\"LYellow\">gamble</style> your embers", 6, afterUnlock: post);
        SteamLeaderboardManager.UnlockAchievment("GYOMYO_CASINO");
        if(GameVariables.hasQuest(46)){
            QuestBoard.Instance.Cloris.QueueDialogue(17); 
        }
        
       
    }

    public void QueueMoneyUnlockDialogue(){
        QuestBoard.Instance.Rowl.QueueDialogue(12);
    }
}

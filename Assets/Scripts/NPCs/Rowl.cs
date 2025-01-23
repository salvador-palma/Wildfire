using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rowl : NPC
{
    /* GAME VARIABLES
        |   * "SkillTreeReady"
        |    0 - Not Ready
        |    1 - Ready to Talk
        |    2 - Ready
    */
    
    protected override void CharacterLoad()
    {
        switch(GameVariables.GetVariable("SkillTreeReady")){
            case 1:QueueDialogue(1);
            break;
            case 3:QueueDialogue(3);
            break;
        }

    }
    public override void ClickedCharacter(){

        if(GameVariables.GetVariable("SkillTreeReady") <= 0 && !hasAvailableDialogue()){
            StartDialogue(0);
            return;
        }
        
        base.ClickedCharacter();
    }

    public void UnlockSkillTree(){
        MetaMenuUI.Instance.UnlockableScreen("UNLOCKED", "ROWL'S SKILL TREE", "You can now <style=\"LYellow\">unlock</style> and <style=\"LYellow\">upgrade</style> new abilities for your <style=\"LYellow\">campfire</style>", 3);
       
    }
    [SerializeField] SkillTreeButton[] Unlockables;
    public void ShowUnlockedUpgrade(int unlockableID){
        MetaMenuUI.Instance.SkillTreeMenuToggle();
        Unlockables[unlockableID].Clicked();
    }

    public void SetVariable(int value){
        GameVariables.SetVariable("SkillTreeReady", value);
    }

    public void giveEmbers(int n){
        SkillTreeManager.Instance.changeEmberAmountUI(n);
        SkillTreeManager.AddEmbersToJSON(n);
    }
}

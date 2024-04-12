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
            case 2:QueueDialogue(3);
            break;
        }

    }
    public override void ClickedCharacter(){

        if(GameVariables.GetVariable("SkillTreeReady") <= 0){
            StartDialogue(0);
            return;
        }
        
        base.ClickedCharacter();
    }

    public void UnlockSkillTree(){
        MetaMenuUI.Instance.UnlockableScreen("UNLOCKED", "ROWL'S SKILL TREE", "You can now unlock and upgrade new abilities for your campfire", 3);
        GameVariables.SetVariable("SkillTreeReady",2);
    }
    [SerializeField] SkillTreeButton[] Unlockables;
    public void ShowUnlockedUpgrade(int unlockableID){
        MetaMenuUI.Instance.SkillTreeMenuToggle();
        Unlockables[unlockableID].Clicked();
    }

    public void giveEmbers(int n){
        SkillTreeManager.Instance.changeEmberAmountUI(n);
    }
}

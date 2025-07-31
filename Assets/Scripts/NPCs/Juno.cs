using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Juno : NPC
{
    protected override void CharacterLoad()
    {
        if (GameVariables.GetVariable("JunoReady") == -1)
        {
            gameObject.SetActive(false);

        }

    }

    public void UnlockLeaderBoard()
    {
        MetaMenuUI.Instance.UnlockableScreen("UNLOCKED", "JUNO'S REGISTERS", "You can now compare yourself to <style=\"LYellow\">other players</style> around the world", 3, ()=>
        {
            QueueDialogue(1);
        });
    }
    [SerializeField] SkillTreeButton WhilrpoolButton;
    public void UnlockWhirl(){
        MetaMenuUI.Instance.UnlockableScreen("NEW SKILL DISCOVERED", "WHIRLPOOL", "You can now unlock the <style=\"LYellow\">Whirlpool</style> ability, go talk to <color=#FFCC7C><sprite name=\"Rowl\"> Rowl", 0, ()=>
        {
            MetaMenuUI.Instance.SkillTreeMenuToggle();
            WhilrpoolButton.Clicked();
        });
        SkillTreeManager.Instance.Upgrade("Whirlpool", Unlock:true);
        SkillTreeManager.Instance.InvokeUIReset();
    }
}

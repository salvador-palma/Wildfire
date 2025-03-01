using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloris : NPC
{
    
    
    public override void ClickedCharacter(){

        if(GameVariables.GetVariable("ClorisPresentation")==-1){
            StartDialogue(0);
            return;
        }
        if(!Character.Instance.HasAtLeastOneCharacter()){
            
            StartDialogue(Random.Range(0f,1f)<.5f?1:2);
            return;
        }
        
        base.ClickedCharacter();
    }
    public void PresentationDone(){
        GameVariables.SetVariable("ClorisPresentation",1);

    }
    public void UnlockCharacterSelect(){
        MetaMenuUI.Instance.UnlockableScreen("UNLOCKED", "CLORIS' WARDROBE", "You can now <style=\"LYellow\">style</style> your <style=\"LYellow\">campfire</style> affecting its <style=\"LYellow\">behaviour</style> and <style=\"LYellow\">environment</style>", 4);
    }
}

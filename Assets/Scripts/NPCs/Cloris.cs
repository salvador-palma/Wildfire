using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloris : NPC
{
    
    protected override void CharacterLoad()
    {
        if(GameVariables.GetVariable("ClorisWardrobe") == 0){
            GameVariables.SetVariable("ClorisWardrobe",1);
            CompleteQuest(6);
            QueueDialogue(3);
        }
      

    }
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
        MetaMenuUI.Instance.UnlockableScreen("UNLOCKED", "CLORIS' WARDROBE", "You can now <color=#FFCC7C>style</color> your <color=#FFCC7C>campfire</color> affecting its <color=#FFCC7C>behaviour</color> and <color=#FFCC7C>environment</color>", 4);
    }
}

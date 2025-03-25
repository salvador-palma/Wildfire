using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Betsy : NPC
{
    
        public override void ClickedCharacter(){
        StartDialogue(new int[]{27,28,29});
        
    }

}

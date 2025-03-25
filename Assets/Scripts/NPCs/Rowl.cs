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
    
    
    public override void ClickedCharacter(){
        StartDialogue(new int[]{19,20,21});
        
    }

    
}

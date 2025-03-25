using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class Gyomyo : NPC
{
    

   public override void ClickedCharacter(){
        StartDialogue(new int[]{16,17,18});
        
    }
}

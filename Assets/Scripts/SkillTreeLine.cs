using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTreeLine : MonoBehaviour
{
    [SerializeField] bool LineNext;
    [SerializeField] GameObject[] nextObjects;
    bool TempPingNext;
    public void EndAnimation(){
        if(LineNext){
            foreach(GameObject line in nextObjects){
                line.GetComponent<SkillTreeLine>().PlayUnlock(TempPingNext);
            }
        }else{
            if(TempPingNext){nextObjects[0].GetComponent<SkillTreeButton>().ping();}
            
        }
    }
    public void PlayUnlock(bool pingNext){
        TempPingNext = pingNext;
        GetComponent<Animator>().Play("SkillTreeLineUnlock");
    }
    public void PlayInit(){
        if(LineNext){
            foreach(GameObject line in nextObjects){
                line.GetComponent<SkillTreeLine>().PlayInit();
            }
        }
        GetComponent<Animator>().Play("Init");
    }
}

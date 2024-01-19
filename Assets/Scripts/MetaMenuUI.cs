using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MetaMenuUI : MonoBehaviour
{
    //Deug
    [SerializeField] private GameObject SkillTreePanel;
    [SerializeField] private GameObject SkillTree;
    static public MetaMenuUI Instance;
    
    private void Awake() {
        Instance = this;
    }
    public void StartGame(){
        SceneManager.LoadScene("Game");
    }
    // private void Update() {
    //     float scrollDelta = Input.mouseScrollDelta.y;
    //     if(scrollDelta!=0){
    //         float scaleFactor = (scrollDelta > 0) ? 1 + 0.2f : 1 - 0.2f;
    //         SkillTree.transform.localScale *= scaleFactor;
    //     }

        
        
    // }
    public void SkillTreeMenuToggle(){
        
        SkillTreeManager.Instance.toggleSkillTree(SkillTreePanel);
    }
 
    public void UpgradeButton(){
        SkillTreeButton selected = SkillTreeButton.SelectedButton;
        if(selected != null){selected.ClickedUpgrade();}
        
    }
    private IEnumerator currentCouroutine;
    public void moveSkillTree(Vector2 pos){
        //SkillTree.transform.localPosition = pos;
        if(currentCouroutine!=null){StopCoroutine (currentCouroutine);}
        currentCouroutine = SmoothLerp (0.5f, pos);
        StartCoroutine (currentCouroutine);
    }
    
    private IEnumerator SmoothLerp (float time, Vector2 pos)
{
        Vector3 startingPos  = SkillTree.transform.localPosition;
        Vector3 finalPos = pos;

        float elapsedTime = 0;
        
        while (elapsedTime < time)
        {
            SkillTree.transform.localPosition = Vector3.Lerp(startingPos, finalPos, elapsedTime / time);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
}
}

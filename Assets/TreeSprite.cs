using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

public class TreeSprite : MonoBehaviour
{
    [SerializeField] Sprite[] treeSprites;
    
    void Start()
    {
        int n = -1;
        if(SceneManager.GetActiveScene().name == "MetaGame"){
            n = Random.Range(0, treeSprites.Length);
            PlayerPrefs.SetInt(transform.parent.name + gameObject.name, n);
        }else{
            n = PlayerPrefs.GetInt(transform.parent.name + gameObject.name, -1);
            if(n==-1){
                n = Random.Range(0, treeSprites.Length);
            }

        }
        GetComponent<SpriteRenderer>().sprite = treeSprites[n];
        
    }

   
}

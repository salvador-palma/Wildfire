using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour
{
    [SerializeField] Sprite[] treeSprites;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<SpriteRenderer>().sprite = treeSprites[Random.Range(0, treeSprites.Length)];
    }

   
}

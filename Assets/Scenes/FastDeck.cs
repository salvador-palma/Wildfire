using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastDeck : MonoBehaviour
{
    public Tier tier;
    public void Start() {
        
    }
    void Update(){
        if(Input.GetKeyDown(KeyCode.L)){
            Deck deck = Deck.Instance;
            if(deck==null){Debug.Log("Error");}
            deck.ChangeSingular(deck.randomPicking(tier), gameObject,0);
        }
    }
}

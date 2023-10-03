using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageUI : MonoBehaviour
{
    public static DamageUI Instance {get; private set;}
    [SerializeField] private GameObject[] textDmg;
    [SerializeField] private Canvas targetCanvas;
    

    private void Awake() {
        Instance = this;
    }
    public void spawnTextDmg(Vector2 vec, string str, int index){

        GameObject go = Instantiate(textDmg[index], targetCanvas.transform);
        Vector2 viewportPos = Camera.main.WorldToViewportPoint(vec);
        RectTransform rt = go.GetComponent<RectTransform>();
        go.GetComponentInChildren<TextMeshProUGUI>().text = str;
        
        rt.anchorMin = viewportPos;
        rt.anchorMax = viewportPos;
    }

}

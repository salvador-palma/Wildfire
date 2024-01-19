using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
[System.Serializable]
public class TextDamageType{
    public TMP_FontAsset fontAsset;
    public Color color;
}
public class DamageUI : MonoBehaviour
{
    const int INITIAL_POOL_SIZE = 100;
    const int POOL_INCREMENT = 50;
    List<GameObject> pooledObjects = new List<GameObject>(); 

    public static DamageUI Instance {get; private set;}
    [SerializeField] TextDamageType[] textDamageTypes;
    [SerializeField] GameObject TextDmgTemplate;
    
    [SerializeField] private Canvas targetCanvas;
    

    private void Awake() {
        Instance = this;
        startPool();
    }
    private void startPool(){
        for (int i = 0; i < INITIAL_POOL_SIZE; i++)
        {
            pooledObjects.Add(Instantiate(TextDmgTemplate, targetCanvas.transform));
        }
    }
    private void increasePool(int n){
        Debug.Log("Pool Increased!");
        for (int i = 0; i < n; i++)
        {
            pooledObjects.Add(Instantiate(TextDmgTemplate, targetCanvas.transform));
        }
    }
    private static GameObject getPooledObject(){
        foreach(GameObject g in Instance.pooledObjects){
            if(!g.activeInHierarchy){
                return g;
            }
        }
        Instance.increasePool(POOL_INCREMENT);
        return getPooledObject();
    }
    public static void DestroyTxtDmg(GameObject gameObject){
        gameObject.SetActive(false);
    }
    public static GameObject InstantiateTxtDmg(Vector2 pos, string str,int id){
        GameObject textDmg = getPooledObject();
        transformTxtDmg(textDmg, id, str);
        setTextPosition(textDmg, pos);
        textDmg.SetActive(true);

        return textDmg;
    }


    public static void transformTxtDmg(GameObject go, int id, string str){
        
        TextDamageType textDamageType = Instance.textDamageTypes[id];
        TextMeshProUGUI tmp = go.GetComponentInChildren<TextMeshProUGUI>();
        tmp.text = str;
        tmp.font = textDamageType.fontAsset;
        tmp.color = textDamageType.color;
        
    }
    public static void setTextPosition(GameObject go, Vector2 pos){
        Vector2 viewportPos = Camera.main.WorldToViewportPoint(pos);
        RectTransform rt = go.GetComponent<RectTransform>();
        rt.anchorMin = viewportPos;
        rt.anchorMax = viewportPos;
    }
 

}

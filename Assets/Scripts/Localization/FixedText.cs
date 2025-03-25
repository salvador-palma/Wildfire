using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FixedText : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private string Scene;
    private string OGText;
    void Awake()
    {
        
        Translator.dropdownValueChange += TranslateComponent;
        Scene = SceneManager.GetActiveScene().name;
       
    }

    


    void Start()
    {
        OGText = GetComponent<TextMeshProUGUI>().text;

        if (string.IsNullOrEmpty(OGText)) { return; }

        string translatedText = Translator.getTranslation(OGText);
        GetComponent<TextMeshProUGUI>().text = translatedText;
    }

    private void OnTextChanged(UnityEngine.Object obj)
    {
        TranslateComponent(null, null);
    }

    private void TranslateComponent(object sender, EventArgs e) 
    {
        try{
            //string text = GetComponent<TextMeshProUGUI>().text;

            //if (string.IsNullOrEmpty(text)) { return; }

            string translatedText = Translator.getTranslation(OGText);
            GetComponent<TextMeshProUGUI>().text = translatedText;

        }catch{
            if(Scene != SceneManager.GetActiveScene().name){
                Translator.dropdownValueChange -= TranslateComponent;
            }
        }
        
    }

    void OnDestroy()
    {
        
        Translator.dropdownValueChange -= TranslateComponent;
    }

    [ContextMenu("Call Extra Function")]
    public void test()
    {
        Translator.changeLanguage("Portuguese");
    }

    
}

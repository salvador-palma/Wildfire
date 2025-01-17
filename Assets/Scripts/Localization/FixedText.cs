using System;
using TMPro;
using UnityEngine;

public class FixedText : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        Translator.dropdownValueChange += TranslateComponent;
       
       
    }

    void Start()
    {
        string text = GetComponent<TextMeshProUGUI>().text;

        if (string.IsNullOrEmpty(text)) { return; }

        string translatedText = Translator.getTranslation(text);
        GetComponent<TextMeshProUGUI>().text = translatedText;
    }

    private void OnTextChanged(UnityEngine.Object obj)
    {
        TranslateComponent(null, null);
    }

    private void TranslateComponent(object sender, EventArgs e) 
    {
        string text = GetComponent<TextMeshProUGUI>().text;

        if (string.IsNullOrEmpty(text)) { return; }

        string translatedText = Translator.getTranslation(text);
        GetComponent<TextMeshProUGUI>().text = translatedText;
    }

    [ContextMenu("Call Extra Function")]
    public void test()
    {
        Translator.changeLanguage("Portuguese");
    }

    
}

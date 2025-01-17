 using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DynamicText : MonoBehaviour
{

    [HideInInspector] public string oldPhrase;
    [HideInInspector] public string[] oldArgs;
    void Awake(){
        Translator.dropdownValueChange += TranslateComponent;
    }

    // String deverá seguir o formato translateText("Estou a dever {0} pau à {1}", String[] = {"500", "Betclic"})

    private void TranslateComponent(object sender, EventArgs e) 
    {
        
        SetText(oldPhrase,oldArgs);
    }

    public void SetText(string text, string[] args = null)
    {
        //if (string.IsNullOrEmpty(text)) { return; }

        string translatedText = text=="" ? "" :Translator.getTranslation(text);
        oldPhrase = translatedText;
        

        if(args != null){
            oldArgs = new string[args.Length];
            // Inserir argumentos
            for(int i = 0; i < args.Length; i++)
            {
                if (int.TryParse(args[i], out _)) oldArgs[i] = args[i]; // Se for número não busca tradução
                else oldArgs[i] = args[i]=="" ? "" : Translator.getTranslation(args[i]); 
                translatedText = translatedText.Replace("{"+i+"}", oldArgs[i]);
            }
        }

        GetComponent<TextMeshProUGUI>().text = translatedText;
    }

    public void setColor(Color c){
         GetComponent<TextMeshProUGUI>().color = c;
    }
    
}







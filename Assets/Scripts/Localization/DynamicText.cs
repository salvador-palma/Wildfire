 using System;
using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class DynamicText : MonoBehaviour
{

    public string oldPhrase;
    [HideInInspector] public string[] oldArgs;
    public bool isChat;
    private string Scene;
    private bool hasAssignedEvent; 
    void Awake(){
         if(!hasAssignedEvent){
            hasAssignedEvent = true;
            Translator.dropdownValueChange += TranslateComponent;  
         }
        Scene = SceneManager.GetActiveScene().name;    
        
    }
    public bool DebugIT;


    // String deverá seguir o formato translateText("Estou a dever {0} pau à {1}", String[] = {"500", "Betclic"})

    public void TranslateComponent(object sender, EventArgs e) 
    {
        if(DebugIT) Debug.Log("TranslateComponent: " + oldPhrase);
        try{
            if (onCouroutine && isChat){
                cond.value = 1;
            }else{
                SetText(oldPhrase,oldArgs);
            }
        }catch{
            if(Scene != SceneManager.GetActiveScene().name){
                Translator.dropdownValueChange -= TranslateComponent;
            }
            
        }
        

    }
    public void SetTextDirect(string text){
        GetComponent<TextMeshProUGUI>().text = text;
    }
    private string FitArgs(string text, string[] args = null){
        string fittedText = text;
        if(args != null){
            oldArgs = new string[args.Length];
            // Inserir argumentos
            for(int i = 0; i < args.Length; i++)
            {
                if (int.TryParse(args[i], out _)) oldArgs[i] = args[i]; // Se for número não busca tradução
                else oldArgs[i] = args[i]=="" ? "" : Translator.getTranslation(args[i]); 
                fittedText = fittedText.Replace("{"+i+"}", oldArgs[i]);
            }
        }
        return fittedText;
    }
    public void SetText(string text, string[] args = null)
    {
        if(!hasAssignedEvent){
            Translator.dropdownValueChange += TranslateComponent;  
            hasAssignedEvent = true;
        }
        string translatedText = text=="" ? "" :Translator.getTranslation(text);
        oldPhrase = translatedText;
        
        translatedText = FitArgs(translatedText, args);
        GetComponent<TextMeshProUGUI>().text = translatedText;
    }

    public void setColor(Color c){
         GetComponent<TextMeshProUGUI>().color = c;
    }


    Int cond;
    bool onCouroutine;
    public IEnumerator ShowTextTimed(string msg, Int condition, string[] arguments = null, string[] optionTxt  = null, UnityEngine.UI.Button[] Options=null, EventReference sound = new EventReference()){       
        onCouroutine = true;
        string formatting_buffer = "";
        cond = condition;
        condition.value=0;
        oldPhrase = msg;
        string translatedText = Translator.getTranslation(msg); //Texto traduzido nao formatado
        //oldPhrase = translatedText; //OldPhrase traduzida
        translatedText = FitArgs(translatedText, arguments); //Texto tradizido formatado / OldArgs traduzidos
        oldArgs = arguments;
        TextMeshProUGUI tmp = GetComponent<TextMeshProUGUI>();
        
        foreach(char c in translatedText){
            
            if(condition.value>0){  
                break;
            }
            if(c=='<' || formatting_buffer != ""){
                formatting_buffer += c;
                if(c=='>'){
                    tmp.text += formatting_buffer;
                    formatting_buffer = "";
                }
                
            }else{  
                tmp.text += c;
                switch(c){
                    case '.':
                    case '!':
                    case '?':
                        yield return new WaitForSeconds(0.2f);
                        break;
                    case ',':
                        yield return new WaitForSeconds(0.1f);
                        break;
                    case ' ':
                        yield return new WaitForSeconds(0.04f);
                        break;
                    default:
                        AudioManager.PlayOneShot(sound, Vector2.zero);
                        yield return new WaitForSeconds(0.01f);//0.01f
                        break;
                }
            }
        }
        SetText(oldPhrase,oldArgs);
        if(optionTxt != null){
            for(int i =0; i < optionTxt.Length; i++){
                Options[i].gameObject.SetActive(true);
            }
        }
        
        condition.value=1;
        onCouroutine = false;
        yield break;
    }
    
}







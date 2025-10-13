using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FixedImage : MonoBehaviour
{
    [System.Serializable]
    public class ImageTranslation
    {
        public string language;
        public Sprite image;
    }

    private string Scene;
    private Image TargetImage;
    private Sprite OGImage;

    [SerializeField] ImageTranslation[] imageTranslations;
    void Awake()
    {

        Translator.dropdownValueChange += TranslateComponent;
        Scene = SceneManager.GetActiveScene().name;

    }

    void Start()
    {
        TargetImage = GetComponent<Image>();
        OGImage = TargetImage.sprite;

        string lang = Translator.getCurrentLanguage();
        Debug.Log("Image Lang:" + lang);
        TargetImage.sprite = GetTranslatedImage(lang);
    }
    
    private Sprite GetTranslatedImage(string lang)
    {
        ImageTranslation imgTr = imageTranslations.FirstOrDefault(x => x.language == lang);
        return imgTr != null ? imgTr.image : OGImage;
        
    }
    

    private void TranslateComponent(object sender, EventArgs e)
    {
        try{
            TargetImage.sprite = GetTranslatedImage(Translator.getCurrentLanguage());

        }catch{
            if(Scene != SceneManager.GetActiveScene().name){
                Translator.dropdownValueChange -= TranslateComponent;
            }
        }
    }
}

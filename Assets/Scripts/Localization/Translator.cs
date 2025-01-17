using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System;
using System.Linq;
using UnityEngine.Video;

public static class Translator
{
    private static Dictionary<string, List<string>> translations; // Language, List<translations>
    public static event EventHandler dropdownValueChange;
    private static string csvName = "Translations"; 
    private static string currentLanguage = "English";
    private static string lastLanguage = "English";
    
    public static int getCurrentLanguageID(){
        return Array.IndexOf(getLanguagesAvailable(), currentLanguage);
    }

    private static void LoadCSV(string filename) 
    {
        currentLanguage = PlayerPrefs.GetString("Language", "English");
        translations = new Dictionary<string, List<string>>();
        TextAsset csvFile = Resources.Load<TextAsset>(filename);
        if(csvFile == null)
        {
            Debug.LogError($"CSV file {filename} not found");
            return;
        }

        // Iniciar as linguagens 
        using StringReader reader = new StringReader(csvFile.text);
        string firstLine = reader.ReadLine();
        string[] arr = firstLine.Split(';');
        foreach (string s in arr)
        {
            translations.Add(s, new List<string>());
            //Debug.Log($"Adicionado {s} como lingua do dicionario.");
        }

        while (reader.Peek() > -1)
        {
            string line = reader.ReadLine();
            string[] parts = line.Split(';');
            int lang = 0;
            foreach (string str in translations.Keys)
            {
                translations[str].Add(parts[lang++]);
            }


        }
    }

    public static void changeLanguage(string newLanguage)
    {
        lastLanguage = currentLanguage;
        currentLanguage = newLanguage;
        PlayerPrefs.SetString("Language", currentLanguage);
        dropdownValueChange?.Invoke(null, new EventArgs());   
    }

    public static string getTranslation(string oldWord)
    {
        if(translations == null) LoadCSV(csvName);
        int oldEntryIndex = translations[lastLanguage].IndexOf(oldWord); // Buscar o indice na lista da ultima lingua (se não existir criar entrada, se existir ver se a tradução existe)
        int englishIndex = translations["English"].IndexOf(oldWord); // Se nao houver tradução a oldword estara com o valor default em ingles, este indice exite, se nao existe e preciso criar entrada

        // Debug.Log(translations[currentLanguage][oldEntryIndex]);
        // Missing entry
        if(oldEntryIndex == -1 && englishIndex == -1) {  
            Debug.Log(translations["English"].ToList());
            Debug.Log(oldWord);
            AddCsvEntry(oldWord);
            return oldWord;
        }

        // Missing translation
        if(englishIndex != -1 && translations[currentLanguage][englishIndex].Contains("<Missing") // Se a entrada existe em ingles, a old word esta em ingles 
        || oldEntryIndex != -1 && translations[currentLanguage][oldEntryIndex].Contains("<Missing")) { //Se a entrada existe na current, a old word esta em numa non default language e nao da para pesquisar o seu index em ingles tem que ser usado este 
            if(oldEntryIndex != -1) return translations["English"][oldEntryIndex];
            if(englishIndex != -1) return translations["English"][englishIndex];
        }

        string translatedText = "";
        if(englishIndex != -1) translatedText = translations[currentLanguage][englishIndex];
        if(oldEntryIndex != -1) translatedText = translations[currentLanguage][oldEntryIndex];
        
        //Debug.Log($"Traduzir {oldWord} para {translatedText}");
        
        return translatedText;
    }

    // Ads entry 
    private static void AddCsvEntry(string oldWord)
    {
        


        string filePath = Path.Combine(Application.dataPath, "Resources", $"{csvName}.csv");

        try
        {
            translations["English"].Add(oldWord);
            string entry = $"{oldWord}";


            foreach (string language in translations.Keys){
                if(language.Equals("English")) continue;
                //translations[language].Add($"<Missing {language} translation>");
                entry += $";<Missing {language} translation>";
            }

            using(StreamWriter sw = new StreamWriter(filePath, true))
            {
                sw.WriteLine(entry);
            }
            //LoadCSV(csvName);
            Debug.Log("Nova entrada no CSV criada para: " + oldWord);
        }
        catch
        {
            Debug.LogError("Erro a criar nova entrada no CSV Translations");
        }
    }

    public static string[] getLanguagesAvailable()
    {
        if(translations == null) LoadCSV(csvName);
        return translations.Keys.ToArray();
    }
}

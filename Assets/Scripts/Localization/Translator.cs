using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System;
using System.Linq;
using UnityEngine.Video;
using Unity.VisualScripting;

public static class Translator
{
    private static Dictionary<string, List<string>> translations; // Language, List<translations>
    public static event EventHandler dropdownValueChange;
    private static string csvName = "Updated"; // "TranslationsFull+Demo" for demo version
    //private static string csvName = "TranslationsFull+Demo"; 
    private static string currentLanguage = "English";
    private static string lastLanguage = "English";

    private static bool WriteMissingText = false;
    private static bool WarnMissingText = true;
    private static bool SavePreferences = true;
    private static string DefaultLanguageBuild = "简体中文";
    private static bool DebugLineForLineReading = false;

    private static bool TranslatorsVersion = false;
    
    public static int getCurrentLanguageID(){
        return Array.IndexOf(getLanguagesAvailable(), currentLanguage);
    }
    public static void AddIfNotExists(string word){
        if(translations == null) LoadCSV(csvName);
        if(translations["English"].Contains(word.Trim())) return;
        AddCsvEntry(word);
    }
    private static void CopyToPersistentDataPath(){
        string persistentPath = Path.Combine(Application.persistentDataPath, csvName+".csv");
        TextAsset csvFile = Resources.Load<TextAsset>(csvName);
        if(csvFile == null)
        {
            Debug.LogError($"CSV file {csvName} not found");
            return;
        }
        File.WriteAllText(persistentPath, csvFile.text);
        Debug.Log("File copied from Resources to persistentDataPath.");
    }
    private static void LoadCSV(string filename) 
    {
        currentLanguage = SavePreferences ? PlayerPrefs.GetString("Language", "English") : DefaultLanguageBuild;
        Debug.Log("Changed language to " + currentLanguage + " from " + lastLanguage);
        translations = new Dictionary<string, List<string>>();
        TextAsset csvFile = null;
        if(TranslatorsVersion){
            string filePath = Path.Combine(Application.persistentDataPath, csvName+".csv");
            if(!File.Exists(filePath))
            {
                CopyToPersistentDataPath();
            }
            string csvContent = File.ReadAllText(filePath);
            csvFile = new TextAsset(csvContent);
        }else{
            csvFile = Resources.Load<TextAsset>(filename);
        }
        
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
            translations.Add(s.Trim(), new List<string>());
            //Debug.Log($"Adicionado {s} como lingua do dicionario.");
        }

        while (reader.Peek() > -1)
        {
            string line = reader.ReadLine();
            if(DebugLineForLineReading){Debug.Log(line);}
            if(line[0] == '#') continue;
            string[] parts = line.Split(';');
            int lang = 0;
            foreach (string str in translations.Keys)
            {

                translations[str].Add(parts[lang++].Trim());
            }


        }
        Debug.LogError("FINISHED READING: " + translations["English"].Count + " words");
    }

    public static void changeLanguage(string newLanguage)
    {
        lastLanguage = currentLanguage;
        currentLanguage = newLanguage;
        Debug.Log("Changed language to " + currentLanguage + " from " + lastLanguage);
        PlayerPrefs.SetString("Language", currentLanguage);
        dropdownValueChange?.Invoke(null, new EventArgs());   
    }
    //OLD WORD ALWAYS IN ENGLISH
    public static string getTranslation(string oldWord)
    {
        //Debug.Log($"Traduzir {oldWord} para {currentLanguage}");
        try{
            if(oldWord == null) return oldWord;
            oldWord = oldWord.Trim();
            if(translations == null) LoadCSV(csvName);

            if(System.Text.RegularExpressions.Regex.IsMatch(oldWord, @"^[\d.,]+$")){return oldWord;}

            // int oldEntryIndex = translations[lastLanguage].IndexOf(oldWord); // Buscar o indice na lista da ultima lingua (se não existir criar entrada, se existir ver se a tradução existe)
            int englishIndex = translations["English"].IndexOf(oldWord); // Se nao houver tradução a oldword estara com o valor default em ingles, este indice exite, se nao existe e preciso criar entrada

            // Debug.Log(translations[currentLanguage][oldEntryIndex]);
            // Missing entry
            if(englishIndex == -1) {  
                if(WriteMissingText){
                    AddCsvEntry(oldWord);
                }
                Debug.Log("No Translation: " + oldWord);
                return oldWord;
            }

            // Missing translation
            if(englishIndex != -1 && translations[currentLanguage][englishIndex].Contains("<Missing")) { //Se a entrada existe na current, a old word esta em numa non default language e nao da para pesquisar o seu index em ingles tem que ser usado este 
                //return translations["English"][englishIndex];
                return WarnMissingText ? "<size=115%><color=red>MISSING TRANSLATION WARN THE DEV</color></size>":translations["English"][englishIndex];
            }

            string translatedText = "";
            if(englishIndex != -1) translatedText = translations[currentLanguage][englishIndex];
             return translatedText;
            //Debug.Log($"Traduzir {oldWord} para {translatedText}");
        }catch(Exception e){
            Debug.LogError(e);
            Debug.LogError(e.StackTrace);
            Debug.LogError("Erro ao traduzir " + oldWord);
        }
        return "Error";
       
    }

    // Ads entry 
    private static void AddCsvEntry(string oldWord)
    {
        
        if(WriteMissingText == false) return;

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

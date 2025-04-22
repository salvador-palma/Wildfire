using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DropdownScript : MonoBehaviour
{
    TMP_Dropdown dropdown;

    public TMP_Dropdown resolutionDropdown;
    public TMP_Dropdown windowModeDropdown;
    public TMP_Dropdown fpsCapDropdown;

    public Resolution[] resolutions;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        dropdown = GetComponent<TMP_Dropdown>();
        dropdown.ClearOptions();

        foreach(string lang in Translator.getLanguagesAvailable())
        {
            TMP_Dropdown.OptionData newOption = new TMP_Dropdown.OptionData(lang);
            dropdown.options.Add(newOption);
        }
        dropdown.RefreshShownValue();
        dropdown.value = Translator.getCurrentLanguageID();
        dropdown.onValueChanged.AddListener(OnDropdownValueChanged);


        PopulateResolutionDropdown();
        PopulateWindowModeDropdown();
        PopulateFPSCapDropdown();
        resolutionDropdown.onValueChanged.AddListener(SetResolution);
        windowModeDropdown.onValueChanged.AddListener(SetWindowMode);
        fpsCapDropdown.onValueChanged.AddListener(SetFPSCap);
    }

    private void OnDropdownValueChanged(int value)
    {   
        // Change language
        Translator.changeLanguage(dropdown.options[value].text);
        Debug.Log("Language changed to " + dropdown.options[value].text);
 
    }

    // GRAPHICS SETTINGS
    void SetWindowMode(int index)
    {
        PlayerPrefs.SetInt("WindowMode", index);
        Debug.Log("WindowMode: " + index);
        switch (index)
        {
            case 0: Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen; break;
            case 1: Screen.fullScreenMode = FullScreenMode.Windowed; break;
            case 2: Screen.fullScreenMode = FullScreenMode.FullScreenWindow; break;
        }
    }
    void SetResolution(int index)
    {
        PlayerPrefs.SetInt("Resolution", index);
        Resolution selectedRes = resolutions[index];
        Debug.Log("Resolution: " + selectedRes.width + "x" + selectedRes.height);
        Screen.SetResolution(selectedRes.width, selectedRes.height, Screen.fullScreenMode);
    }

    void SetFPSCap(int index)
    {
        PlayerPrefs.SetInt("FPSCap", index);
        

        string selectedOption = fpsCapDropdown.options[index].text;
         Debug.Log("FPSCap: " + selectedOption);

        Application.targetFrameRate = selectedOption == "Unlimited" ? -1 : int.Parse(selectedOption);
    }

    void PopulateResolutionDropdown()
    {
        resolutions = Screen.resolutions;
        resolutions = resolutions.ToList().Distinct().ToArray();
        List<Resolution> resList = new List<Resolution>();
        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();

        foreach (Resolution res in resolutions)
        {
            if(options.Contains(res.width + "x" + res.height)) continue;
            options.Add(res.width + "x" + res.height );
            resList.Add(res);
        }
        resolutions = resList.ToArray();

        int savedRes = PlayerPrefs.GetInt("Resolution", GetCurrentResolutionIndex());
        savedRes = resolutions.Length > savedRes ? savedRes : GetCurrentResolutionIndex();
        Debug.Log("Resolution: " + savedRes);
        SetResolution(savedRes);
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = options.IndexOf(resolutions[savedRes].width + "x" + resolutions[savedRes].height);
        resolutionDropdown.RefreshShownValue();
    }

    void PopulateWindowModeDropdown()
    {
        windowModeDropdown.ClearOptions();
        windowModeDropdown.AddOptions(new List<string> { "Fullscreen", "Windowed", "Borderless" });
        int savedMode = PlayerPrefs.GetInt("WindowMode", 0);
        Debug.Log("WindowMode: " + savedMode);
        SetWindowMode(savedMode);
        windowModeDropdown.value = savedMode;
        windowModeDropdown.RefreshShownValue();
    }

    void PopulateFPSCapDropdown()
    {
        fpsCapDropdown.ClearOptions();
        List<string> options = new List<string> { "30", "60", "120", "144", "165", "240", "Unlimited" };
        fpsCapDropdown.AddOptions(options);


        int fpsSaved = PlayerPrefs.GetInt("FPSCap", 6);
        fpsSaved = fpsSaved <= -1 ? 6 : fpsSaved; 
        Debug.Log("FPSCap: " + fpsSaved);
        
        fpsCapDropdown.value = fpsSaved;
        SetFPSCap(fpsSaved);
        fpsCapDropdown.RefreshShownValue();
    }

    int GetCurrentResolutionIndex()
    {
        for (int i = 0; i < resolutions.Length; i++)
        {
            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                return i;
            }
        }
        return 0;
    }

    

}
